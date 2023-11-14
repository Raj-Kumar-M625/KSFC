-- Sep 30 2020

ALTER TABLE dbo.TenantEmployee
ADD 
VoiceFeatureEnabled BIT NOT NULL DEFAULT 0,
ExecAppDetailLevel INT NOT NULL DEFAULT 0
GO

ALTER TABLE [dbo].[ExecAppImei]
ADD	[ExecAppDetailLevel] INT NOT NULL DEFAULT 2
GO

ALTER PROCEDURE [dbo].[GetInFieldSalesPeople]
		@inputDate DateTime2,
		@tenantId BIGINT
AS
BEGIN

	DECLARE @SignedInEmployeeData TABLE
	(
        EmployeeDayId BIGINT,
		EmployeeId BIGINT,
		StartTime DATETIME2,
		EndTime DATETIME2,
		TotalOrderAmount DECIMAL(19,2),
		TotalPaymentAmount DECIMAL(19,2),
		TotalReturnAmount DECIMAL(19,2),
		TotalExpenseAmount DECIMAL(19,2),
		TotalActivityCount INT,
		Latitude DECIMAL(19,9),
		Longitude DECIMAL(19,9),
		CurrentLocTime DATETIME2,
		PhoneModel NVARCHAR(100),
		PhoneOS NVARCHAR(10),
		AppVersion NVARCHAR(10)
	)

	INSERT INTO @SignedInEmployeeData
	(EmployeeDayId, EmployeeId, StartTime, EndTime, 
	 TotalOrderAmount, TotalPaymentAmount, TotalReturnAmount, TotalExpenseAmount,
	 Latitude, Longitude, PhoneModel, PhoneOS, AppVersion,
	 TotalActivityCount, CurrentLocTime
	)
	SELECT ed.Id, ed.TenantEmployeeId, StartTime, EndTime, 
	TotalOrderAmount, TotalPaymentAmount, 
	TotalReturnAmount, TotalExpenseAmount,
	CurrentLatitude, CurrentLongitude,
	PhoneModel, PhoneOS, AppVersion,
	ed.TotalActivityCount, ed.CurrentLocTime
	FROM dbo.[Day] d
	INNER JOIN dbo.EmployeeDay ed on d.Id = ed.DayId
	AND d.[DATE] = CAST(@inputDate AS [Date])
	
	-- RESULT SET QUERY
	;WITH cteAreaCodes(ZoneCode, AreaCode, TerritoryCode, HQCode)
	AS
	( 
		SELECT Distinct  ZoneCode, AreaCode, TerritoryCode, HQCode
		FROM dbo.OfficeHierarchy 
		WHERE IsActive = 1
		AND TenantId = @tenantId
	)
	SELECT 
	ISNULL(cte.ZoneCode, '***') ZoneCode, 
	ISNULL(cte.AreaCode, '***') AreaCode, 
	ISNULL(cte.TerritoryCode, '***') TerritoryCode,
	ISNULL(cte.HQCode, '***') HQCode, 
	ISNULL(sq.StaffCode, '') StaffCode, 
	ISNULL(sq.IsInFieldToday, 0) IsInFieldToday,
	ISNULL(sq.IsRegisteredOnPhone,0) IsRegisteredOnPhone,
	sq.StartTime,
	sq.EndTime,
	ISNULL(sq.TotalOrderAmount, 0) as TotalOrderAmount,
	ISNULL(sq.TotalPaymentAmount,0) as TotalPaymentAmount,
	ISNULL(sq.TotalReturnAmount,0) as TotalReturnAmount,
	ISNULL(sq.TotalExpenseAmount,0) as TotalExpenseAmount,
	ISNULL(sq.Latitude,0) as Latitude,
	ISNULL(sq.Longitude,0) as Longitude,
	ISNULL(sq.PhoneModel, '') AS PhoneModel,
	ISNULL(sq.PhoneOS, '') AS PhoneOS,
	ISNULL(sq.AppVersion, '') AS AppVersion,
	ISNULL(sq.TotalActivityCount, 0) AS TotalActivityCount,
	ISNULL(sq.CurrentLocTime, '2000-01-01') AS CurrentLocTime

	FROM cteAreaCodes cte
	FULL OUTER JOIN 
			(
				-- Employee Code and Area Codes, IsInFieldToday, IsRegisteredOnPhone
				SELECT sp.StaffCode, 
				ISNULL(oh.ZoneCode, '***') ZoneCode, 
				ISNULL(oh.AreaCode, '***') AreaCode, 
				ISNULL(oh.TerritoryCode, '***') TerritoryCode,
				ISNULL(oh.HQCode, '***') HQCode, 
				CASE WHEN ed.EmployeeId IS NULL THEN 0 ELSE 1 END IsInFieldToday,
				CASE WHEN te.Id IS NULL THEN 0 ELSE 1 END IsRegisteredOnPhone,

				ed.StartTime,
				ed.EndTime,
				ed.TotalOrderAmount,
				ed.TotalPaymentAmount,
				ed.TotalReturnAmount,
				ed.TotalExpenseAmount,
				ed.Latitude,
				ed.Longitude,
				ed.PhoneModel,
				ed.PhoneOS,
				ed.AppVersion,
				ed.TotalActivityCount,
				ed.CurrentLocTime

				FROM dbo.SalesPerson sp
				LEFT JOIN dbo.OfficeHierarchy oh on sp.HQCode = oh.HQCode and oh.IsActive = 1 AND oh.TenantId = @tenantId
				LEFT JOIN dbo.TenantEmployee te on te.EmployeeCode = sp.StaffCode and te.IsActive = 1 AND te.TenantId = @tenantId
				LEFT JOIN @SignedInEmployeeData ed on te.Id = ed.EmployeeId
				WHERE sp.IsActive = 1
			) sq ON 
			cte.ZoneCode = sq.ZoneCode AND
			cte.AreaCode = sq.AreaCode AND
			cte.TerritoryCode = sq.TerritoryCode AND
			cte.HQCode = sq.HQCode


  -- Cases covered
  -- 1. SalesPerson table may have a HQ code that does not exist in Office hierarchy
  --    We still want such a record in the resultset with Areacode as '***'
END
GO

--=======================================
-- SCHEMA CHANAGE FOR ITEMMASTER TABLE
--=======================================
-- Step 1
alter table dbo.ItemMaster
add IsDeleted BIT NOT NULL DEFAULT 0
go

-- Step 2
;with itemMasterCTE(Id, ItemCode, rownumber)
AS
(
	SELECT Id, ItemCode, 
	ROW_NUMBER() Over (Partition By ItemCode Order by ItemCode) rowNumber
	FROM dbo.ItemMaster
)
UPDATE dbo.ItemMaster
SET IsDeleted = 1
FROM dbo.ItemMaster im
INNER JOIN itemMasterCTE cte on im.Id = cte.Id
AND cte.rownumber > 1
go

-- Step 3 - create a new table
CREATE TABLE [dbo].[ItemMasterTypeName]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[ItemMasterId] BIGINT NOT NULL REFERENCES dbo.ItemMaster,
	[ItemMasterId2] BIGINT NOT NULL, -- this column to be dropped after Step 7
	[TypeName] NVARCHAR(50) NOT NULL DEFAULT ''
)
GO

-- Step 4 - create separate table for ItemMaster and TypeName
;with itemMasterCTE(Id, ItemCode)
AS
(
  SELECT Id, ItemCode
  FROM dbo.ItemMaster
  WHERE IsDeleted = 0
)
INSERT INTO dbo.ItemMasterTypeName
(ItemMasterId, ItemMasterId2, TypeName)
SELECT cte.Id, im.Id, im.typeName
FROM dbo.ItemMaster im
INNER JOIN itemMasterCTE cte on im.ItemCode = cte.ItemCode
AND typeName != ''
order by im.ItemCode
go

-- Step 5 -- update ids in existing table
update dbo.IssueReturn
set ItemMasterId = imtn.ItemMasterId
FROM dbo.IssueReturn ir
INNER JOIN dbo.ItemMasterTypeName imtn on ir.ItemMasterId = imtn.ItemMasterId2
AND ir.ItemMasterId != imtn.ItemMasterId
go

update dbo.IssueReturn
set AppliedItemMasterId = imtn.ItemMasterId
FROM dbo.IssueReturn ir
INNER JOIN dbo.ItemMasterTypeName imtn on ir.AppliedItemMasterId = imtn.ItemMasterId2
AND ir.AppliedItemMasterId != imtn.ItemMasterId
go

update dbo.StockBalance
set ItemMasterId = imtn.ItemMasterId
FROM dbo.StockBalance ir
INNER JOIN dbo.ItemMasterTypeName imtn on ir.ItemMasterId = imtn.ItemMasterId2
AND ir.ItemMasterId != imtn.ItemMasterId
go

update dbo.StockInput
set ItemMasterId = imtn.ItemMasterId
FROM dbo.StockInput ir
INNER JOIN dbo.ItemMasterTypeName imtn on ir.ItemMasterId = imtn.ItemMasterId2
AND ir.ItemMasterId != imtn.ItemMasterId
go

update dbo.StockLedger
set ItemMasterId = imtn.ItemMasterId
FROM dbo.StockLedger ir
INNER JOIN dbo.ItemMasterTypeName imtn on ir.ItemMasterId = imtn.ItemMasterId2
AND ir.ItemMasterId != imtn.ItemMasterId
go

update dbo.StockRequest
set ItemMasterId = imtn.ItemMasterId
FROM dbo.StockRequest ir
INNER JOIN dbo.ItemMasterTypeName imtn on ir.ItemMasterId = imtn.ItemMasterId2
AND ir.ItemMasterId != imtn.ItemMasterId
go

update dbo.StockRequestFulfill
set ItemMasterId = imtn.ItemMasterId
FROM dbo.StockRequestFulfill ir
INNER JOIN dbo.ItemMasterTypeName imtn on ir.ItemMasterId = imtn.ItemMasterId2
AND ir.ItemMasterId != imtn.ItemMasterId
go

-- Step 6: Remove the rows that were marked for Deleted
DELETE FROM dbo.ItemMaster
WHERE IsDeleted = 1
go

---- Step 7: Remove extra column also

ALTER TABLE dbo.ItemMaster
DROP COLUMN IsDeleted
go

--alter table dbo.ItemMaster
--drop constraint DF__ItemMaste__TypeN__4B380934
--go

ALTER TABLE dbo.ItemMaster
DROP COLUMN TypeName
go

ALTER TABLE [dbo].[ItemMasterTypeName]
DROP COLUMN ItemMasterId2
GO

-- Step 8 -- clean up stock balance with duplicates
-- since no live customer is using this module yet - I could delete the duplicates
-- (else I have to accumulate and create single row of totals)
;with stockBalanceCTE(id, ItemMasterId, StockQuantity, ZoneCode, AreaCode, TerritoryCode, HQCode, StaffCode, rowNumber)
as
(
 select id, ItemMasterId, StockQuantity, ZoneCode, AreaCode, TerritoryCode, HQCode, StaffCode,
 ROW_NUMBER() over (Partition By ItemMasterId, ZoneCode, AreaCode, TerritoryCode, HQCode, StaffCode ORDER BY ItemMasterId, ZoneCode, AreaCode, TerritoryCode, HQCode, StaffCode) rowNumber
 FROM dbo.StockBalance
)
delete from dbo.StockBalance where id in (
select id from stockBalanceCTE
where rowNumber > 1
)

--============================================

ALTER PROCEDURE [dbo].[TransformItemMasterData]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
		-- can't truncate itemMaster as it is being referenced by IssueReturn table
		DECLARE @ItemMasterInput TABLE
		(
			[ID] BIGINT NOT NULL Identity,
			[ItemCode] NVARCHAR(50),
			[ItemDesc] NVARCHAR(100),
			[Category] NVARCHAR(50),
			[Unit]     NVARCHAR(10),
			[Rate] DECIMAL(19,2),
			[Classification] NVARCHAR(10),
			IsDeleted BIT NOT NULL
		)

		-- Step 1: Put data in an in memory table - this is to get Id for each row;
		INSERT INTO @ItemMasterInput
		(ItemCode, ItemDesc, Category, Unit, Rate, Classification, IsDeleted)
		SELECT
		ItemCode, ItemDesc, Category, Unit, Rate, Classification, 0
		FROM dbo.ItemMasterInput

		-- Step 2: Now take only unique rows
		; with itemMasterCTE(Id, rowNumber)
		AS
		(
			SELECT Id, 
			Row_NUMBER() OVER (Partition By itemCode order by ItemCode)
			FROM @ItemMasterInput
		)
		UPDATE @ItemMasterInput
		SET IsDeleted = 1
		FROM @itemMasterInput input
		INNER JOIN itemMasterCTE cte on input.Id = cte.Id
		AND cte.rowNumber > 1

		-- Step 3: Remove duplicates from in memory table
		DELETE FROM @ItemMasterInput
		WHERE IsDeleted = 1

		-- Step 4: Update existing items Data
		UPDATE dbo.[ItemMaster]
		SET 
		[ItemDesc] = ani.ItemDesc,
		[Category] = ani.Category,
		[Unit] = ani.Unit,
		[Classification] = left(ltrim(rtrim(IsNULL(ani.[Classification], ''))), 10),
		[Rate] = ani.Rate,
		IsActive = 1
		FROM dbo.ItemMaster an
		INNER JOIN @ItemMasterInput ani on an.ItemCode = ani.ItemCode

		-- Step 5: Insert new items data
		INSERT INTO dbo.[ItemMaster]
		([ItemCode], [ItemDesc], [Category], [Unit], [Classification], [Rate], IsActive)
		SELECT  
		ani.[ItemCode], ani.[ItemDesc], ani.[Category], ani.[Unit], 
		left(ltrim(rtrim(IsNULL(ani.[Classification], ''))), 10), ani.Rate, 1
		FROM @ItemMasterInput ani
		LEFT JOIN dbo.ItemMaster an on ani.ItemCode = an.ItemCode
		WHERE an.ItemCode is null

		-- Step 6: Mark the rows as inactive in ItemMaster table
		-- A row can come again in future uploads, hence the case
		IF @IsCompleteRefresh = 1
		BEGIN
			UPDATE dbo.ItemMaster
			SET IsActive = CASE when ani.ItemCode is null then 0 else 1 END
			FROM dbo.ItemMaster an
			LEFT JOIN @itemMasterInput ani on an.ItemCode = ani.ItemCode
		END

		-- Step 7: If Complete Refresh - delete existing data in ItemMasterTypeName
		IF @IsCompleteRefresh = 1
		BEGIN
			TRUNCATE TABLE dbo.ItemMasterTypeName
		END

		-- Step 8 Create new rows in ItemMasterTypeName table
		; with cropCTE(Id, cropName)
		AS
		(
			SELECT ani.Id, input.CropName
			FROM dbo.ItemMasterInput input
			INNER JOIN dbo.ItemMaster ani on input.ItemCode = ani.ItemCode
		)
		INSERT INTO dbo.ItemMasterTypeName
		(ItemMasterId, TypeName)
		SELECT c.Id, c.Cropname
		FROM cropCTE c
		LEFT JOIN dbo.ItemMasterTypeName p on c.Id = p.ItemMasterId AND c.CropName = p.TypeName
		WHERE p.ItemMasterId is null

		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformItemMasterData', 'Success'
END
GO

ALTER PROCEDURE [dbo].[ProcessSqliteIssueReturnData]
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
GO
