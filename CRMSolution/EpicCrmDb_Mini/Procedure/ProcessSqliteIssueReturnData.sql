CREATE PROCEDURE [dbo].[ProcessSqliteIssueReturnData]
	@batchId BIGINT
AS
BEGIN

	-- if batch is already processed - return	
	IF NOT EXISTS(SELECT 1 FROM dbo.SqliteActionBatch 
			WHERE Id = @batchId AND NumberOfIssueReturnsSaved > 0
			AND BatchProcessed = 0)
	BEGIN
		RETURN;
	END

	DECLARE @tenantId BIGINT
	SELECT @tenantId = TenantId
	FROM dbo.SqliteActionBatch b
	INNER JOIN dbo.TenantEmployee te on b.EmployeeId = te.Id
	WHERE b.Id = @batchId

	-- First create required records in Day Table
	INSERT INTO dbo.[Day]
	([DATE])
	SELECT DISTINCT CAST(e.[IssueReturnDate] AS [Date])
	FROM dbo.SqliteIssueReturn e
	LEFT JOIN dbo.[Day] d on CAST(e.[IssueReturnDate] AS [Date]) = d.[DATE]
	WHERE e.BatchId = @batchId
	AND d.[DATE] IS NULL

	-- SqliteIssueReturn can have issues/returns for Agreements, that were
	-- created on phone and not uploaded.  For such records, we have to first
	-- fill in AgreementId in SqliteIssueReturn table
	UPDATE dbo.SqliteIssueReturn
	SET AgreementId = agg.EntityAgreementId,
	EntityId = agg.EntityId,
	DateUpdated = SYSUTCDATETIME()
	FROM dbo.SqliteIssueReturn sqe
	INNER JOIN dbo.SqliteAgreement agg on sqe.ParentReferenceId = agg.PhoneDbId
	and sqe.IsNewAgreement = 1
	and agg.BatchId <= @batchId -- agreement has to come in same batch or before
	and sqe.IsProcessed = 0


	-- select current max Id from dbo.IssueReturn
	DECLARE @lastMaxId BIGINT
	SELECT @lastMaxId = ISNULL(MAX(Id),0) FROM dbo.IssueReturn

	-- used for SMS
	DECLARE @NewItems TABLE
	( 
	  ID BIGINT IDENTITY,
	  EntityId BIGINT,
	  EntityName NVARCHAR(50),
	  PhoneNumber NVARCHAR(20),
	  EntityAgreementId BIGINT,
	  [AgreementNumber] NVARCHAR(50),
	  [TypeName] NVARCHAR(50),
	  [Quantity] INT,
	  [ItemMasterId] BIGINT,
	  [ItemCode] NVARCHAR(100),
	  [ItemUnit] NVARCHAR(10),
	  [TransactionType] NVARCHAR(50)
	)

	-- If ItemId does not exist in dbo.ItemMaster - find it out
	-- Oct 03 2020, (this case can happen if we normalized the ItemMaster table
	-- but user did not download the data)
	;WITH sirCTE(id, itemCode)
	AS
	(
		SELECT sqe.Id, sqe.ItemCode 
		FROM dbo.SqliteIssueReturn sqe
		LEFT JOIN dbo.ItemMaster im on sqe.ItemId = im.Id
		WHERE sqe.BatchId = @batchId AND sqe.IsProcessed = 0
		AND im.Id is NULL
	)
	UPDATE dbo.SqliteIssueReturn
	SET ItemId = ISNULL(im.Id, 0)
	FROM dbo.SqliteIssueReturn sir
	INNER JOIN sirCTE cte on sir.Id = cte.Id
	LEFT JOIN dbo.ItemMaster im on im.ItemCode = cte.ItemCode

	-- Create Input/Issue Records
	INSERT INTO dbo.[IssueReturn]
	([EmployeeId], [DayId], [EntityId], 
	[EntityAgreementId], 
	[ItemMasterId],
	[TransactionDate], [TransactionType], [Quantity], [ActivityId],
	[SqliteIssueReturnId], [SlipNumber], [LandSizeInAcres], [ItemRate],
	[AppliedTransactionType],
	[AppliedItemMasterId], [AppliedQuantity], [AppliedItemRate], [Status],
	[CreatedBy]
	)
	OUTPUT INSERTED.EntityId, '', '', inserted.EntityAgreementId, '', '', inserted.Quantity,
	inserted.ItemMasterId, '', '', inserted.TransactionType INTO @NewItems
	SELECT 
	sqe.[EmployeeId], d.[Id] AS [DayId], sqe.EntityId, 
	CASE WHEN sqe.[AgreementId] = 0 THEN NULL ELSE sqe.[AgreementId] END,
	sqe.[ItemId],
	CAST(sqe.[IssueReturnDate] AS [Date]), sqe.[TranType], sqe.[Quantity], sqa.ActivityId,
	sqe.[Id], sqe.SlipNumber, sqe.Acreage, sqe.ItemRate,
	sqe.[TranType],
	sqe.[ItemId], sqe.[Quantity], sqe.[ItemRate], 'Pending',
	'ProcessSqliteIssueReturnData'

	FROM dbo.SqliteIssueReturn sqe
	INNER JOIN dbo.[Day] d ON d.[DATE] = CAST(sqe.[IssueReturnDate] AS [Date])
	-- this join is to get activity Id
	INNER JOIN dbo.SqliteAction sqa on sqa.EmployeeId = sqe.EmployeeId
			AND sqa.[At] = sqe.IssueReturnDate
			AND sqa.PhoneDbId = sqe.ActivityId
	WHERE sqe.BatchId = @batchId AND sqe.IsProcessed = 0 
	AND sqe.ItemId > 0
	AND sqe.EntityId > 0 
	AND sqe.AgreementId > 0
	ORDER BY sqe.Id

	-- now we need to update the id in SqliteIssueReturn table
	UPDATE dbo.SqliteIssueReturn
	SET IssueReturnId = e.Id,
	IsProcessed = 1,
	DateUpdated = SYSUTCDATETIME()
	FROM dbo.SqliteIssueReturn se
	INNER JOIN dbo.[IssueReturn] e on se.Id = e.SqliteIssueReturnId
	AND se.BatchId = @batchId
	AND e.Id > @lastMaxId

	-- SMS
	-- fill entity's primary contact / phone in @NewItems table
	UPDATE @NewItems
	SET EntityName = ec.Name,
	PhoneNumber = ec.PhoneNumber
	FROM @NewItems np
	INNER JOIN dbo.EntityContact ec on np.EntityId = ec.EntityId
	and ec.IsPrimary = 1

	UPDATE @NewItems
	SET AgreementNumber = ea.AgreementNumber,
	TypeName = ws.TypeName
	FROM @NewItems np
	INNER JOIN dbo.EntityAgreement ea on ea.Id = np.EntityAgreementId
	INNER JOIN dbo.WorkflowSeason ws on ws.Id = ea.WorkflowSeasonId

	UPDATE @NewItems
	SET ItemCode = ea.ItemDesc,
	ItemUnit = ea.Unit
	FROM @NewItems np
	INNER JOIN dbo.ItemMaster ea on ea.Id = np.ItemMasterId

	-- create a table of counts 
	DECLARE @NewItemsStat TABLE
	( 
	  ID BIGINT IDENTITY,
	  EntityAgreementId BIGINT,
	  [TransactionType] NVARCHAR(50),
	  [NumberOfItems] BIGINT,
	  [ItemDetails] NVARCHAR(2000),

	  EntityName NVARCHAR(50),
	  PhoneNumber NVARCHAR(20),
	  [AgreementNumber] NVARCHAR(50),
	  [TypeName] NVARCHAR(50)
	)

	INSERT into @NewItemsStat
	(EntityAgreementId, TransactionType, NumberOfItems)
	SELECT EntityAgreementId, TransactionType, count(*)
	FROM @NewItems
	GROUP BY EntityAgreementId, TransactionType

	-- Insert into Table for SMS
	-- Issue types, where issued transactions are 1 only
	INSERT INTO dbo.TenantSmsData
	(TenantId, TemplateName, DataType, MessageData)
	SELECT @tenantId, 'InputIssueOne', 'XML', 
	(SELECT EntityName, PhoneNumber, AgreementNumber, TypeName, Quantity, ItemCode, ItemUnit
	 FROM @NewItems i WHERE i.Id = o.Id FOR XML PATH('Row'))
	FROM @NewItems o
	INNER JOIN @NewItemsStat nis 
	ON o.EntityAgreementId = nis.EntityAgreementId
	AND o.TransactionType = nis.TransactionType
	AND nis.TransactionType like '%Issue%'
	AND nis.NumberOfItems = 1

	-- delete the items from in memory table
	DELETE FROM @NewItemsStat
	WHERE NumberOfItems = 1
	AND TransactionType like '%Issue%'

	-- Now we are left with multiple issue or single/multiple returns, for single
	-- EntityId, AgreementId
	-- concatenate details from multiple rows of same type
	Update @NewItemsStat
	SET ItemDetails = 
	(
		select concat(ni2.ItemCode, ' ', ni2.Quantity, ' ', ni2.ItemUnit, ' , ')
		FROM @newItems ni2
		WHERE ni2.EntityAgreementId = nis.EntityAgreementId
		AND ni2.TransactionType = nis.TransactionType
		for xml path('')
	),
	EntityName = ni.EntityName,
	PhoneNumber = ni.PhoneNumber,
	AgreementNumber = ni.AgreementNumber,
	TypeName = ni.TypeName
	FROM @NewItemsStat nis
	INNER JOIN @NewItems ni on nis.EntityAgreementId = ni.EntityAgreementId
	AND nis.TransactionType = ni.TransactionType

	-- insert a row in sms data, for multiple issues for same agreement
	INSERT INTO dbo.TenantSmsData
	(TenantId, TemplateName, DataType, MessageData)
	SELECT @tenantId, 'InputIssueMany', 'XML', 
	(SELECT EntityName, PhoneNumber, AgreementNumber, TypeName, ItemDetails
	 FROM @NewItemsStat i WHERE i.Id = o.Id FOR XML PATH('Row'))
	FROM @NewItemsStat o
	WHERE o.TransactionType like '%Issue%'

	-- insert a row in sms data, for multiple returns for same agreement
	INSERT INTO dbo.TenantSmsData
	(TenantId, TemplateName, DataType, MessageData)
	SELECT @tenantId, 'InputReturnMany', 'XML', 
	(SELECT EntityName, PhoneNumber, AgreementNumber, TypeName, ItemDetails
	 FROM @NewItemsStat i WHERE i.Id = o.Id FOR XML PATH('Row'))
	FROM @NewItemsStat o
	WHERE o.TransactionType like '%Return%'

END
