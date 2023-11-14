-- April 16 2020 - PJM - Seeds workflow

-- April 16 2020
ALTER PROCEDURE [dbo].[GetEntityWorkFlow]
    @tenantId BIGINT,
	@staffCode NVARCHAR(10),
	@areaCode NVARCHAR(10)
AS
BEGIN

	DECLARE @OfficeHierarchy TABLE
	(
		[ZoneCode] NVARCHAR(10),
		[ZoneName] NVARCHAR(50),
		[AreaCode] NVARCHAR(10),
		[AreaName] NVARCHAR(50),
		[TerritoryCode] NVARCHAR(10),
		[TerritoryName] NVARCHAR(50),
		[HQCode] NVARCHAR(10),
		[HQName] NVARCHAR(50)
	)

	-- Get Office Hierarchy
	INSERT INTO @OfficeHierarchy
	(ZoneCode, ZoneName, AreaCode, AreaName, TerritoryCode, TerritoryName, HQCode, HQName)
	exec [GetOfficeHierarchyForStaff] @tenantId, @staffCode

	-- select distinct hq codes
	DECLARE @hqcodes TABLE (HQCode NVARCHAR(10))

	INSERT INTO @hqcodes (HQCode)
	SELECT Distinct HQCode FROM @OfficeHierarchy
	WHERE AreaCode = @areaCode


	SELECT wf.EntityId, wf.EntityName, wf.Id, 
	wfd.TagName, wfd.Phase, wfd.PlannedStartDate, wfd.PlannedEndDate,
	wf.InitiationDate, wfd.IsComplete, wf.AgreementId, wfd.Id 'EntityWorkFlowDetailId',
	wfd.Notes, wfd.IsFollowUpRow,
	(CASE WHEN wfd.TagName = wf.TagName and wfd.IsFollowUpRow = 0 Then 1 ELSE 0 END) AS IsCurrentPhaseRow
	FROM dbo.EntityWorkFlow wf
	INNER JOIN @hqcodes hq on wf.HQCode = hq.HQCode
	INNER JOIN dbo.EntityWorkFlowDetail wfd on wf.Id = wfd.EntityWorkFlowId
	-- Send all workflow activities, to display on phone - April 15, 2020
	--AND (wfd.TagName = wf.TagName OR wfd.IsFollowUpRow = 1)
END
GO

-- Bug Fix when new TypeName (for next year) is created
-- with same TypeName as before.
ALTER PROCEDURE [dbo].[ProcessSqliteAgreementData]
	@batchId BIGINT,
	@agreementDefaultStatus NVARCHAR(50)
AS
BEGIN

	-- if batch is already processed - return	
	IF NOT EXISTS(SELECT 1 FROM dbo.SqliteActionBatch 
			WHERE Id = @batchId AND AgreementsSaved > 0
			AND BatchProcessed = 0)
	BEGIN
		RETURN;
	END

	-- First create required records in Day Table
	INSERT INTO dbo.[Day]
	([DATE])
	SELECT DISTINCT CAST(e.[AgreementDate] AS [Date])
	FROM dbo.SqliteAgreement e
	LEFT JOIN dbo.[Day] d on CAST(e.[AgreementDate] AS [Date]) = d.[DATE]
	WHERE e.BatchId = @batchId
	AND d.[DATE] IS NULL

	-- Fill Entity Id in Agreements that belong to new entity created on phone.
	Update dbo.SqliteAgreement
	SET EntityId = en.EntityId,
	EntityName = en.EntityName,
	DateUpdated = SYSUTCDATETIME()
	FROM dbo.SqliteAgreement ag
	INNER JOIN dbo.SqliteEntity en on ag.ParentReferenceId = en.PhoneDbId
	AND ag.BatchId = @batchId
	AND ag.IsNewEntity = 1
	AND ag.IsProcessed = 0

	-- store SqliteAgreement Rows in in-memory table
	DECLARE @sqliteAgg TABLE (ID BIGINT Identity, RowId BIGINT)
	INSERT INTO @sqliteAgg
	(RowId)
	SELECT Id FROM dbo.SqliteAgreement
	WHERE BatchId = @batchId
	AND isProcessed = 0
	ORDER BY Id

	-- Count the number of Agreements
	DECLARE @aggCount BIGINT
	SELECT @aggCount = count(*)	FROM @sqliteAgg

	-- Select Agreement Ids
	DECLARE @aggNum TABLE (Id BIGINT Identity, AgreementNumber NVARCHAR(50))

	UPDATE dbo.AgreementNumber
	SET ISUsed = 1,
	UsedTimeStamp = SYSUTCDATETIME()
	OUTPUT deleted.AgreementNumber INTO @aggNum
	FROM dbo.AgreementNumber an
	INNER JOIN 
	(
		SELECT TOP(@aggCount) Id
		FROM dbo.AgreementNumber WITH (READPAST)
		WHERE ISUsed = 0
		ORDER BY [Sequence]
	) an2 on an.Id = an2.Id


	DECLARE @insertTable TABLE (EntityAgreementId BIGINT, AgreementNumber NVARCHAR(50))

	-- Insert rows in dbo.EntityAgreement
	INSERT into dbo.EntityAgreement
	(EntityId, WorkflowSeasonId, AgreementNumber, LandSizeInAcres, 
	[Status], CreatedBy, UpdatedBy)
	OUTPUT inserted.Id, inserted.AgreementNumber INTO @insertTable
	SELECT ag.EntityId, wfs.Id, agg.AgreementNumber, ag.Acreage,
	@agreementDefaultStatus, 'ProcessSqliteAgreementData', 'ProcessSqliteAgreementData'
	FROM dbo.SqliteAgreement ag
	INNER JOIN dbo.WorkflowSeason wfs on wfs.TypeName = ag.TypeName
	and wfs.IsOpen = 1
	INNER JOIN @sqliteAgg sagg ON ag.Id = sagg.RowId
	INNER JOIN @aggNum agg ON agg.Id = sagg.ID


	-- Now update EntityAgreementId back in SqliteAgreement 
	Update dbo.SqliteAgreement
	SET EntityAgreementId = m3.EntityAgreementId,
	IsProcessed = 1,
	DateUpdated = SYSUTCDATETIME()
	FROM dbo.SqliteAgreement sagg
	INNER JOIN @sqliteAgg m1 ON sagg.Id = m1.RowId
	INNER JOIN @aggNum m2 on m1.Id = m2.Id
	INNER JOIN @insertTable m3 on m3.AgreementNumber = m2.AgreementNumber
END
GO

-- May 04 2020
CREATE TABLE [dbo].[TransporterInput]
(
	[Company name] NVARCHAR(50) NOT NULL,
	[Vehicle type] NVARCHAR(20) NOT NULL,
	[Vehicle No] NVARCHAR(20) NOT NULL,
	[Vehicle capacity in Kgs] INT NOT NULL,
	[Transportation Type] NVARCHAR(10) NOT NULL,
	[Hamali Rate per Bag (Rs)] DECIMAL(9,2) NOT NULL,
	[Silo To Return KM] INT NOT NULL,
	[Cost per Km] DECIMAL(9,2) NOT NULL,
	[Extra Cost per Ton] DECIMAL(9,2) NOT NULL
)
GO

CREATE TABLE [dbo].[Transporter]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[CompanyName] NVARCHAR(50) NOT NULL,
	[VehicleType] NVARCHAR(20) NOT NULL,
	[VehicleNo] NVARCHAR(20) NOT NULL,
	[TransportationType] NVARCHAR(10) NOT NULL,
	[SiloToReturnKM] INT NOT NULL,
	[VehicleCapacityKgs] INT NOT NULL,
	[HamaliRatePerBag] DECIMAL(9,2) NOT NULL,
	[CostPerKm] DECIMAL(9,2) NOT NULL,
	[ExtraCostPerTon] DECIMAL(9,2) NOT NULL
)

INSERT into dbo.codeTable
(codeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
('ExcelUpload', 'Transporter', 'TransporterInput', 120, 1, 1)
go

CREATE PROCEDURE [dbo].[TransformTransporterDataFeed]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
		-- Step 1: Truncate table if opted to do so

		IF @IsCompleteRefresh = 1
		BEGIN
			TRUNCATE TABLE dbo.Transporter
		END

		-- Update existing Data
		UPDATE dbo.Transporter
		SET 
			[CompanyName] = ti.[Company name],
			[VehicleType] = ti.[Vehicle type],
			[TransportationType] = ti.[Transportation Type],
			[SiloToReturnKM] = ti.[Silo To Return KM],
			[VehicleCapacityKgs] = ti.[Vehicle capacity in Kgs],
			[HamaliRatePerBag] = ti.[Hamali Rate per Bag (Rs)],
			[CostPerKm] = ti.[Cost per Km],
			[ExtraCostPerTon] = ti.[Extra Cost per Ton]
		FROM dbo.Transporter t
		INNER JOIN dbo.TransporterInput ti on t.VehicleNo = ti.[Vehicle No]

		-- Insert New data
		INSERT INTO dbo.Transporter
		(
			[CompanyName],
			[VehicleType],
			[VehicleNo],
			[TransportationType],
			[SiloToReturnKM],
			[VehicleCapacityKgs],
			[HamaliRatePerBag],
			[CostPerKm],
			[ExtraCostPerTon] 
		)
		SELECT  
			[Company name],
			[Vehicle type],
			[Vehicle No],
			[Transportation Type],
			[Silo To Return KM],
			[Vehicle capacity in Kgs],
			[Hamali Rate per Bag (Rs)],
			[Cost per Km],
			[Extra Cost per Ton] 
		
		FROM dbo.TransporterInput ti
		LEFT JOIN dbo.Transporter t ON ti.[Vehicle No] = t.VehicleNo
		WHERE t.VehicleNo IS NULL


		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformTransporterDataFeed', 'Success'
END
GO

ALTER table dbo.WorkFlowFollowUp
ADD	[MaxDWS] INT NOT NULL DEFAULT 0
GO

Update dbo.WorkFlowFollowUp
SET MaxDWS = 2
WHERE TypeName = 'SEEDS' and Phase = 'Harvest'
AND IsActive = 1
GO


DROP TABLE [dbo].[ItemMasterInput]
GO
CREATE TABLE [dbo].[ItemMasterInput]
(
	[ItemCode] NVARCHAR(50) NOT NULL,
	[ItemDesc] NVARCHAR(100) NOT NULL,
	[Category] NVARCHAR(50) NOT NULL,
	[Unit]     NVARCHAR(10) NOT NULL,
	[Rate] DECIMAL(19,2) NOT NULL,
	[Classification] NVARCHAR(10),
	[CropName] NVARCHAR(50) NOT NULL
)
GO

ALTER TABLE [dbo].[ItemMaster]
ADD	[Rate] DECIMAL(19,2) NOT NULL DEFAULT 0
GO

ALTER PROCEDURE [dbo].[TransformItemMasterData]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
		-- can't truncate itemMaster as it is being referenced by IssueReturn table

		-- Step 1: Update Data
		UPDATE dbo.[ItemMaster]
		SET 
		[ItemDesc] = ani.ItemDesc,
		[Category] = ani.Category,
		[Unit] = ani.Unit,
		[Classification] = left(ltrim(rtrim(IsNULL(ani.[Classification], ''))), 10),
		[TypeName] = ani.CropName,
		[Rate] = ani.Rate
		FROM dbo.ItemMaster an
		INNER JOIN dbo.ItemMasterInput ani on an.ItemCode = ani.ItemCode
		and an.TypeName = ani.CropName


		-- Step 2: Insert data
		INSERT INTO dbo.[ItemMaster]
		([ItemCode], [ItemDesc], [Category], [Unit], [Classification], [TypeName], [Rate])
		SELECT  
		ani.[ItemCode], ani.[ItemDesc], ani.[Category], ani.[Unit], 
		left(ltrim(rtrim(IsNULL(ani.[Classification], ''))), 10),
		ani.[CropName], ani.Rate
		FROM dbo.ItemMasterInput ani
		LEFT JOIN dbo.ItemMaster an on ani.ItemCode = an.ItemCode
		and an.TypeName = ani.CropName
		WHERE an.ItemCode is null

		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformItemMasterData', 'Success'
END
GO


ALTER TABLE [dbo].[SqliteIssueReturn]
ADD	[ItemRate] DECIMAL(19,2) NOT NULL DEFAULT 0
GO

ALTER TABLE [dbo].[IssueReturn]
ADD	[ItemRate] DECIMAL(19,2) NOT NULL DEFAULT 0
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

	-- Create Input/Issue Records
	INSERT INTO dbo.[IssueReturn]
	([EmployeeId], [DayId], [EntityId], 
	[EntityAgreementId], 
	[ItemMasterId],
	[TransactionDate], [TransactionType], [Quantity], [ActivityId],
	[SqliteIssueReturnId], [SlipNumber], [LandSizeInAcres], [ItemRate])
	OUTPUT INSERTED.EntityId, '', '', inserted.EntityAgreementId, '', '', inserted.Quantity,
	inserted.ItemMasterId, '', '', inserted.TransactionType INTO @NewItems
	SELECT 
	sqe.[EmployeeId], d.[Id] AS [DayId], sqe.EntityId, 
	CASE WHEN sqe.[AgreementId] = 0 THEN NULL ELSE sqe.[AgreementId] END,
	sqe.[ItemId],
	CAST(sqe.[IssueReturnDate] AS [Date]), sqe.[TranType], sqe.[Quantity], sqa.ActivityId,
	sqe.[Id], sqe.SlipNumber, sqe.Acreage, sqe.ItemRate

	FROM dbo.SqliteIssueReturn sqe
	INNER JOIN dbo.[Day] d ON d.[DATE] = CAST(sqe.[IssueReturnDate] AS [Date])
	-- this join is to get activity Id
	INNER JOIN dbo.SqliteAction sqa on sqa.EmployeeId = sqe.EmployeeId
			AND sqa.[At] = sqe.IssueReturnDate
			AND sqa.PhoneDbId = sqe.ActivityId
	WHERE sqe.BatchId = @batchId AND sqe.IsProcessed = 0
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

-- May 10 2020 - STR/DWS
ALTER TABLE [dbo].[SqliteActionBatch]
ADD	
	[STRCount] BIGINT NOT NULL DEFAULT 0,
	[STRSavedCount] BIGINT NOT NULL DEFAULT 0
GO

CREATE TABLE [dbo].[SqliteSTR]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[BatchId] BIGINT NOT NULL REFERENCES dbo.SqliteActionBatch,
	[EmployeeId] BIGINT NOT NULL,

	[STRNumber] NVARCHAR(50) NOT NULL,
	[VehicleNumber] NVARCHAR(50) NOT NULL,
	[DriverName] NVARCHAR(50) NOT NULL,
	[DriverPhone] NVARCHAR(50) NOT NULL,
	[DWSCount] BIGINT NOT NULL,
	[BagCount] BIGINT NOT NULL,
	[GrossWeight] DECIMAL(19,2) NOT NULL,
	[NetWeight] DECIMAL(19,2) NOT NULL,
	[StartOdometer] BIGINT NOT NULL,
	[EndOdometer] BIGINT NOT NULL,
	[STRDate] DATETIME2 NOT NULL,
	[IsNew] BIT NOT NULL,
	[IsTransferred] BIT NOT NULL,
	[TransfereeName] NVARCHAR(50) NOT NULL,
	[TransfereePhone] NVARCHAR(50) NOT NULL,

	[ImageCount] INT NOT NULL DEFAULT 0,
	[TimeStamp] DATETIME2 NOT NULL,
	[ActivityId] NVARCHAR(50) NOT NULL DEFAULT '',
	[TimeStamp2] DATETIME2 NOT NULL,
	[ActivityId2] NVARCHAR(50) NOT NULL DEFAULT '',
	[PhoneDbId] NVARCHAR(50) NOT NULL, 

	[IsProcessed] BIT NOT NULL DEFAULT 0,  
	[STRId] BIGINT NOT NULL DEFAULT 0,
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
)
GO

CREATE TABLE [dbo].[SqliteSTRDWS]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[SqliteSTRId] BIGINT NOT NULL REFERENCES SqliteSTR(Id),

	[EntityId] BIGINT NOT NULL,
	[EntityName] NVARCHAR(50) NOT NULL,

	[AgreementId] BIGINT NOT NULL,
	[Agreement] NVARCHAR(50) NOT NULL,
	[EntityWorkFlowDetailId] BIGINT NOT NULL,
	[TypeName] NVARCHAR(50) NOT NULL,
	[TagName] NVARCHAR(50) NOT NULL,

	[DWSNumber] BIGINT NOT NULL,
	[BagCount] BIGINT NOT NULL,
	[FilledBagsWeightKg] DECIMAL(19,2) NOT NULL,
	[EmptyBagsWeightKg] DECIMAL(19,2) NOT NULL,

	[DWSDate] DATETIME2 NOT NULL,

	[ActivityId] NVARCHAR(50) NOT NULL DEFAULT '',

	[IsProcessed] BIT NOT NULL DEFAULT 0,  
	[DWSId] BIGINT NOT NULL DEFAULT 0,
)
GO

CREATE TABLE [dbo].[SqliteSTRImage]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[SqliteSTRId] BIGINT NOT NULL REFERENCES SqliteSTR(Id),
	[SequenceNumber] INT NOT NULL DEFAULT 0,
	[ImageFileName] VARCHAR(100) NOT NULL DEFAULT 'FileNotFound.jpg'
)
GO

ALTER PROCEDURE [dbo].[ClearEmployeeData]
	@employeeId BIGINT
AS
BEGIN
	BEGIN TRY

		BEGIN TRANSACTION

		-- Delete Activity Data
		DECLARE @activity Table ( Id BIGINT )
		DECLARE @image TABLE (Id BIGINT)

		-- first find out the activity ids that need to be deleted
		INSERT INTO @activity (Id)
		SELECT a.id from dbo.Activity a
		INNER JOIN dbo.EmployeeDay ed on ed.Id = a.employeeDayId
		and ed.TenantEmployeeId = @employeeId

		-- delete activity images
		-- store image Ids first - delete images at the end, as we need to delete images for payment as well;
		INSERT INTO @image (Id)
		SELECT ImageId FROM dbo.ActivityImage ai 
					 INNER JOIN @activity a on ai.ActivityId = a.Id

		print 'ActivityImage'
		DELETE FROM dbo.ActivityImage
		WHERE ActivityId IN (SELECT id FROM @activity)

		print 'ActivityContact'
		DELETE FROM dbo.ActivityContact WHERE ActivityId in (SELECT ID FROM @activity)


		print 'SqliteEntityWorkFlow'
		-- this table is no longer used;
		DELETE FROM dbo.SqliteEntityWorkFlow WHERE  ActivityId in (SELECT ID FROM @activity)

		-- April 11 2020
		print 'SqliteEntityWorkFlowItemUsed'
		DELETE FROM [dbo].[SqliteEntityWorkFlowItemUsed]
		WHERE SqliteEntityWorkFlowId IN 
			(SELECT Id FROM dbo.SqliteEntityWorkFlowV2 WHERE EmployeeId = @employeeId)

		-- April 11 2020
		print 'SqliteEntityWorkFlowFollowUp'
		DELETE FROM [dbo].SqliteEntityWorkFlowFollowUp
		WHERE SqliteEntityWorkFlowId IN 
			(SELECT Id FROM dbo.SqliteEntityWorkFlowV2 WHERE EmployeeId = @employeeId)

		print 'SqliteEntityWorkFlowV2'
		DELETE FROM dbo.SqliteEntityWorkFlowV2 WHERE EmployeeId = @employeeId


		print 'Activity'
		DELETE from dbo.Activity WHERE id in (SELECT Id from @activity)

		-----------------
		print 'dbo.DistanceCalcErrorLog'
		DELETE from dbo.DistanceCalcErrorLog
		WHERE id in
		(SELECT l.id from dbo.distanceCalcErrorLog l
		INNER JOIN dbo.Tracking t on l.TrackingId = t.Id
		INNER JOIN dbo.EmployeeDay ed on ed.Id = t.employeeDayId
		and ed.TenantEmployeeId = @employeeId)

		print 'dbo.Tracking'
		DELETE from dbo.Tracking
		WHERE id in
		(SELECT t.id from dbo.Tracking t
		INNER JOIN dbo.EmployeeDay ed on ed.Id = t.employeeDayId
		and ed.TenantEmployeeId = @employeeId)

		print 'employeeDay'
		DELETE from dbo.employeeDay WHERE TenantEmployeeId = @employeeId

		print 'imei'
		DELETE from dbo.Imei WHERE TenantEmployeeId = @employeeId

		--DELETE FROM dbo.SqliteAction WHERE EmployeeId = @employeeId
		--DELETE FROM dbo.SqliteActionBatch WHERE EmployeeId = @employeeId

		-- delete expense Data as well
		print 'SqliteExpenseImage'
		DELETE FROM dbo.SqliteExpenseImage WHERE SqliteExpenseId in (SELECT Id FROM dbo.SqliteExpense WHERE EmployeeId = @employeeId)
		print 'SqliteExpense'
		DELETE FROM dbo.SqliteExpense WHERE EmployeeId = @employeeId

		-- Delete SqliteOrder data
		print 'SqliteOrderItem'
		DELETE FROM dbo.SqliteOrderItem WHERE SqliteOrderId IN (SELECT Id FROM dbo.[SqliteOrder] WHERE EmployeeId = @employeeId)
		print 'SqliteOrderImage'
		DELETE FROM dbo.SqliteOrderImage WHERE SqliteOrderId IN (SELECT Id FROM dbo.[SqliteOrder] WHERE EmployeeId = @employeeId)
		print '[SqliteOrder]'
		DELETE FROM dbo.[SqliteOrder] WHERE EmployeeId = @employeeId

		-- delete SqlLiteAction Data as well
		print 'SqliteActionImage'
		DELETE FROM dbo.SqliteActionImage where SqliteActionId in (SELECT id FROM dbo.SqliteAction WHERE EmployeeId = @employeeId)
		print 'SqliteActionContact'
		DELETE FROM dbo.SqliteActionContact WHERE SqliteActionId in (SELECT id FROM dbo.SqliteAction WHERE EmployeeId = @employeeId)
		print 'SqliteActionLocation'
		DELETE FROM dbo.SqliteActionLocation WHERE SqliteActionId in (SELECT id FROM dbo.SqliteAction WHERE EmployeeId = @employeeId)
		print 'SqliteAction'
		DELETE FROM dbo.SqliteAction WHERE EmployeeId = @employeeId
		print 'SqliteActionDup'
		DELETE FROM dbo.SqliteActionDup WHERE EmployeeId = @employeeId

		-- delete SqlitePayment data as well
		print 'SqlitePaymentImage'
		DELETE FROM dbo.SqlitePaymentImage WHERE SqlitePaymentId in (SELECT Id FROM dbo.SqlitePayment WHERE EmployeeId = @employeeId)
		print 'SqlitePayment'
		DELETE FROM dbo.SqlitePayment WHERE EmployeeId = @employeeId

		-- Delete SqliteReturnOrder data
		print 'SqliteReturnOrderItem'
		DELETE FROM dbo.SqliteReturnOrderItem WHERE SqliteReturnOrderId IN (SELECT Id FROM dbo.[SqliteReturnOrder] WHERE EmployeeId = @employeeId)
		print '[SqliteReturnOrder]'
		DELETE FROM dbo.[SqliteReturnOrder] WHERE EmployeeId = @employeeId

		-- Delete SqliteEntity data
		DECLARE @SqliteEntity TABLE (Id BIGINT)
		INSERT INTO @SqliteEntity SELECT Id FROM dbo.[SqliteEntity] WHERE EmployeeId = @employeeId
		print '[SqliteEntityContact]'
		DELETE FROM dbo.[SqliteEntityContact] WHERE [SqliteEntityId] IN (SELECT Id FROM @SqliteEntity)
		print '[SqliteEntityCrop]'
		DELETE FROM dbo.[SqliteEntityCrop] WHERE [SqliteEntityId] IN (SELECT Id FROM @SqliteEntity)
		print '[SqliteEntityImage]'
		DELETE FROM dbo.[SqliteEntityImage] WHERE [SqliteEntityId] IN (SELECT Id FROM @SqliteEntity)
		print '[SqliteEntityLocation]'
		DELETE FROM dbo.[SqliteEntityLocation] WHERE [SqliteEntityId] IN (SELECT Id FROM @SqliteEntity)
		print '[SqliteEntity]'
		DELETE FROM dbo.[SqliteEntity] WHERE EmployeeId = @employeeId

		-- Delete SqliteLeave data
		print '[SqliteLeave]'
		DELETE FROM dbo.[SqliteLeave] WHERE EmployeeId = @employeeId

		-- Delete SqliteCancelledLeave data
		print '[SqliteCancelledLeave]'
		DELETE FROM dbo.[SqliteCancelledLeave] WHERE EmployeeId = @employeeId
		
		-- 
		print '[SqliteIssueReturn]'
		DELETE FROM dbo.[SqliteIssueReturn] WHERE EmployeeId = @employeeId

		-- Dec 8 2019 
		print '[SqliteAgreement]'
		DELETE FROM dbo.[SqliteAgreement] WHERE EmployeeId = @employeeId

		print '[SqliteAdvanceRequest]'
		DELETE FROM dbo.[SqliteAdvanceRequest] WHERE EmployeeId = @employeeId

		print '[SqliteTerminateAgreement]'
		DELETE FROM dbo.[SqliteTerminateAgreement] WHERE EmployeeId = @employeeId
		-- End Dec 8 2019

		-- March 18 2020
		print '[SqliteBankDetailImage]'
		DELETE FROM dbo.SqliteBankDetailImage WHERE SqliteBankDetailId in
		( SELECT id FROM dbo.SqliteBankDetail WHERE EmployeeId = @employeeId )

		print '[SqliteBankDetail]'
		DELETE FROM dbo.[SqliteBankDetail] WHERE EmployeeId = @employeeId

		-- End March 18 2020

		-- May 10 2020
		print '[SqliteSTRImage]'
		DELETE FROM dbo.SqliteSTRImage WHERE [SqliteSTRId] in
		( SELECT id FROM dbo.SqliteSTR WHERE EmployeeId = @employeeId )

		print '[SqliteSTRDWS]'
		DELETE FROM dbo.SqliteSTRDWS WHERE [SqliteSTRId] in
		( SELECT id FROM dbo.SqliteSTR WHERE EmployeeId = @employeeId )

		print '[SqliteSTR]'
		DELETE FROM dbo.[SqliteSTR] WHERE EmployeeId = @employeeId

		-- End May 10 2020



		-- Delete Device Log
		print '[SqliteDeviceLog]'
		DELETE FROM dbo.[SqliteDeviceLog] WHERE EmployeeId = @employeeId
		print 'SqliteActionBatch'
		DELETE FROM dbo.SqliteActionBatch WHERE EmployeeId = @employeeId

		-- store image ids first for processed expense data
		INSERT INTO @image (id)
		SELECT ImageId 
		FROM dbo.ExpenseItemImage WHERE ExpenseItemId IN 
		(SELECT ei.id FROM dbo.ExpenseItem ei
		INNER JOIN dbo.Expense e on ei.ExpenseId = e.Id
		AND e.EmployeeId = @employeeId)

		print 'ExpenseItemImage'
		DELETE FROM dbo.ExpenseItemImage WHERE ExpenseItemId IN 
		(SELECT ei.id FROM dbo.ExpenseItem ei INNER JOIN dbo.Expense e on ei.ExpenseId = e.Id AND e.EmployeeId = @employeeId)

		print 'ExpenseItem'
		DELETE FROM dbo.ExpenseItem WHERE ExpenseId in (SELECT Id FROM dbo.Expense WHERE EmployeeId = @employeeId)
		print 'ExpenseApproval'
		DELETE FROM dbo.ExpenseApproval WHERE ExpenseId in (SELECT Id FROM dbo.Expense WHERE EmployeeId = @employeeId)
		print 'Expense'
		DELETE FROM dbo.Expense WHERE EmployeeId = @employeeId

		-- Delete order Data
		print 'OrderItem'
		DELETE FROM dbo.OrderItem WHERE OrderId IN (SELECT Id FROM dbo.[Order] WHERE EmployeeId = @employeeId)

		INSERT INTO @image (id)
		SELECT oim.ImageId
		FROM dbo.OrderImage oim
		INNER JOIN dbo.[Order] o on o.Id = oim.OrderId
		AND o.EmployeeId = @employeeId

		print 'OrderImage'
		DELETE FROM dbo.OrderImage WHERE OrderId IN (SELECT Id FROM dbo.[Order] WHERE EmployeeId = @employeeId)

		print '[Order]'
		DELETE FROM dbo.[Order] WHERE EmployeeId = @employeeId


		-- Delete Return Order Data
		print 'ReturnOrderItem'
		DELETE FROM dbo.ReturnOrderItem WHERE ReturnOrderId IN (SELECT Id FROM dbo.[ReturnOrder] WHERE EmployeeId = @employeeId)
		print '[ReturnOrder]'
		DELETE FROM dbo.[ReturnOrder] WHERE EmployeeId = @employeeId

		-- DELETE Workflow data
		print 'EntityWorkFlowDetailItemUsed'
		DELETE FROM dbo.EntityWorkFlowDetailItemUsed
		WHERE EntityWorkFlowDetailId IN (SELECT d.ID FROM dbo.EntityWorkFlowDetail d
		             INNER JOIN dbo.EntityWorkFlow p on d.EntityWorkFlowId = p.Id
					 AND p.EmployeeId = @employeeId)

		print 'EntityWorkFlowDetail'
		DELETE FROM dbo.EntityWorkFlowDetail
		WHERE ID IN (SELECT d.ID FROM dbo.EntityWorkFlowDetail d
		             INNER JOIN dbo.EntityWorkFlow p on d.EntityWorkFlowId = p.Id
					 AND p.EmployeeId = @employeeId)

		print 'EntityWorkFlow'
		DELETE FROM dbo.EntityWorkFlow WHERE EmployeeId = @employeeId

		print 'Issue/Return'
		DELETE FROM dbo.[IssueReturn] WHERE EmployeeId = @employeeId

		-- Dec 8 2019
		print 'AdvanceRequest'
		DELETE FROM dbo.[AdvanceRequest] WHERE EmployeeId = @employeeId

		print 'TerminateAgreementRequest'
		DELETE FROM dbo.[TerminateAgreementRequest] WHERE EmployeeId = @employeeId

		-- End Dec 8 2019

		-- Delete Entity data
		DECLARE @Entity TABLE (Id BIGINT)
		INSERT INTO @Entity SELECT Id FROM dbo.[Entity] WHERE EmployeeId = @employeeId

		INSERT INTO @image (id)
		SELECT oim.ImageId
		FROM dbo.EntityImage oim
		INNER JOIN dbo.[Entity] o on o.Id = oim.EntityId
		AND o.EmployeeId = @employeeId

		print '[EntityContact]'
		DELETE FROM dbo.[EntityContact] WHERE [EntityId] IN (SELECT Id FROM @Entity)
		print '[EntityCrop]'
		DELETE FROM dbo.[EntityCrop] WHERE [EntityId] IN (SELECT Id FROM @Entity)
		print 'EntityImage'
		DELETE FROM dbo.EntityImage WHERE EntityId IN (SELECT Id FROM @Entity)
		print 'EntityAgreement'
		-- clear foreign key reference first
		UPDATE dbo.[IssueReturn] SET EntityAgreementId = NULL
		WHERE EntityAgreementId IN 
		(
		   SELECT ID FROM dbo.EntityAgreement WHERE EntityId IN (SELECT Id FROM @Entity)
		)

		-- Dec 8 2019
		-- delete data from TerminateAgreementRequest for same agreement that is being deleted
		DELETE FROM dbo.TerminateAgreementRequest
		WHERE EntityAgreementId in
		(
			SELECT ID FROM dbo.EntityAgreement WHERE EntityId IN (SELECT Id FROM @Entity)
		)

		-- delete data from AdvanceRequest for same agreement that is being deleted
		DELETE FROM dbo.AdvanceRequest
		WHERE EntityAgreementId in
		(
			SELECT ID FROM dbo.EntityAgreement WHERE EntityId IN (SELECT Id FROM @Entity)
		)


		DELETE FROM dbo.EntityAgreement WHERE EntityId IN (SELECT Id FROM @Entity)

		-- User 1 has created an entity
		-- User 2 has created a workflow based on this entity
		-- Question: Should we delete the workflow created by user 2 on this entity
		-- Answer: Basic use of this script is to delete the data that is created by test users
		--         during testing on live site.  So the answer is yes.


		print 'EntityWorkFlowDetail - again'
		DELETE FROM dbo.EntityWorkFlowDetail
		WHERE ID IN (SELECT d.ID FROM dbo.EntityWorkFlowDetail d
		             INNER JOIN dbo.EntityWorkFlow p on d.EntityWorkFlowId = p.Id
					 AND p.EntityId in (SELECT Id FROM @Entity))

		print 'EntityWorkFlow - again'
		DELETE FROM dbo.EntityWorkFlow WHERE EntityId in (SELECT Id FROM @Entity)

		print 'IssueReturn - again'
		DELETE FROM dbo.[IssueReturn] WHERE EntityId in (SELECT Id FROM @Entity)

		-- March 19 2020
		print 'EntityBankDetailImage'
		DELETE FROM dbo.EntityBankDetailImage
		WHERE EntityBankDetailId IN
		(SELECT ebd.ID FROM dbo.EntityBankDetail ebd
		INNER JOIN @Entity e2 on e2.Id = ebd.EntityId)

		print 'EntityBankDetail'
		DELETE FROM dbo.EntityBankDetail
		WHERE EntityId IN (SELECT Id from @Entity)
		-- END of changes on March 19 2020


		print '[Entity]'
		DELETE FROM dbo.[Entity] WHERE ID in (SELECT Id from @Entity)

		--
		-- Delete Payment Data
		--
		INSERT INTO @image (id)
		SELECT pim.ImageId
		FROM dbo.PaymentImage pim
		INNER JOIN dbo.Payment p on p.Id = pim.PaymentId
		AND p.EmployeeId = @employeeId

		print 'PaymentImage'
		DELETE FROM dbo.PaymentImage WHERE PaymentId IN (SELECT Id FROM dbo.[Payment] WHERE EmployeeId = @employeeId)
		print '[Payment]'
		DELETE FROM dbo.[Payment] WHERE EmployeeId = @employeeId

		print 'EntityImage - again'
		DELETE FROM dbo.EntityImage WHERE ImageId IN (SELECT Id FROM @image)

		-- DELETE IMAGES
		print 'Image'
		DELETE FROM dbo.[Image]
		WHERE Id in (SELECT Id FROM @image)

		-- CLEAR DEVICE LOG
		print 'SqliteDeviceLog'
		DELETE FROM dbo.SqliteDeviceLog WHERE EmployeeId = @employeeId

		print 'TenantEmployee'
		DELETE from dbo.TenantEmployee WHERE id = @employeeId
		COMMIT
	END TRY

	BEGIN CATCH
		PRINT 'Inside Catch...'
		PRINT ERROR_MESSAGE()
		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:ClearEmployeeData', ERROR_MESSAGE()

		ROLLBACK TRANSACTION
		throw;

	END CATCH
END	
GO

-- May 12 2020
DROP TABLE [dbo].[TransporterInput];
CREATE TABLE [dbo].[TransporterInput]
(
	[Company Code] NVARCHAR(20) NOT NULL,
	[Company name] NVARCHAR(50) NOT NULL,
	[Vehicle type] NVARCHAR(20) NOT NULL,
	[Vehicle No] NVARCHAR(20) NOT NULL,
	[Vehicle capacity in Kgs] INT NOT NULL,
	[Transportation Type] NVARCHAR(10) NOT NULL,
	[Hamali Rate per Bag (Rs)] DECIMAL(9,2) NOT NULL,
	[Silo To Return KM] INT NOT NULL,
	[Cost per Km] DECIMAL(9,2) NOT NULL,
	[Extra Cost per Ton] DECIMAL(9,2) NOT NULL
)
GO

ALTER TABLE [dbo].[Transporter]
ADD	[CompanyCode] NVARCHAR(20) NOT NULL DEFAULT ''
GO

ALTER PROCEDURE [dbo].[TransformTransporterDataFeed]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
		-- Step 1: Truncate table if opted to do so

		IF @IsCompleteRefresh = 1
		BEGIN
			TRUNCATE TABLE dbo.Transporter
		END

		-- Update existing Data
		UPDATE dbo.Transporter
		SET 
		    [CompanyCode] = ti.[Company code],
			[CompanyName] = ti.[Company name],
			[VehicleType] = ti.[Vehicle type],
			[TransportationType] = ti.[Transportation Type],
			[SiloToReturnKM] = ti.[Silo To Return KM],
			[VehicleCapacityKgs] = ti.[Vehicle capacity in Kgs],
			[HamaliRatePerBag] = ti.[Hamali Rate per Bag (Rs)],
			[CostPerKm] = ti.[Cost per Km],
			[ExtraCostPerTon] = ti.[Extra Cost per Ton]
		FROM dbo.Transporter t
		INNER JOIN dbo.TransporterInput ti on t.VehicleNo = ti.[Vehicle No]

		-- Insert New data
		INSERT INTO dbo.Transporter
		(
		    [CompanyCode],
			[CompanyName],
			[VehicleType],
			[VehicleNo],
			[TransportationType],
			[SiloToReturnKM],
			[VehicleCapacityKgs],
			[HamaliRatePerBag],
			[CostPerKm],
			[ExtraCostPerTon] 
		)
		SELECT  
		    [Company code],
			[Company name],
			[Vehicle type],
			[Vehicle No],
			[Transportation Type],
			[Silo To Return KM],
			[Vehicle capacity in Kgs],
			[Hamali Rate per Bag (Rs)],
			[Cost per Km],
			[Extra Cost per Ton] 
		
		FROM dbo.TransporterInput ti
		LEFT JOIN dbo.Transporter t ON ti.[Vehicle No] = t.VehicleNo
		WHERE t.VehicleNo IS NULL


		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformTransporterDataFeed', 'Success'
END
GO

ALTER TABLE [dbo].[FeatureControl]
ADD
	[STRFeature] BIT NOT NULL DEFAULT 0,
	[DWSPaymentReport] BIT NOT NULL DEFAULT 0,
	[TransporterPaymentReport] BIT NOT NULL DEFAULT 0,
	[STRWeighControl] BIT NOT NULL DEFAULT 0,
	[STRSiloControl] BIT NOT NULL DEFAULT 0
GO

CREATE TABLE [dbo].[STRTag]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[STRNumber] NVARCHAR(50) NOT NULL,
	[STRDate] DATETIME2 NOT NULL,

	[Status] NVARCHAR(50) NOT NULL DEFAULT 'Pending',

	[CreatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[UpdatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
)
GO

CREATE TABLE [dbo].[STR]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[STRTagId] BIGINT NOT NULL References dbo.[STRTag],
	[EmployeeId] BIGINT NOT NULL References dbo.TenantEmployee,

	[VehicleNumber] NVARCHAR(50) NOT NULL,
	[DriverName] NVARCHAR(50) NOT NULL,
	[DriverPhone] NVARCHAR(50) NOT NULL,
	[DWSCount] BIGINT NOT NULL,
	[BagCount] BIGINT NOT NULL,
	[GrossWeight] DECIMAL(19,2) NOT NULL,
	[NetWeight] DECIMAL(19,2) NOT NULL,
	[StartOdometer] BIGINT NOT NULL,
	[EndOdometer] BIGINT NOT NULL,
	[IsNew] BIT NOT NULL,
	[IsTransferred] BIT NOT NULL,
	[TransfereeName] NVARCHAR(50) NOT NULL,
	[TransfereePhone] NVARCHAR(50) NOT NULL,

	[ImageCount] INT NOT NULL DEFAULT 0,
	[ActivityId] BIGINT NOT NULL DEFAULT 0,
	[ActivityId2] BIGINT NOT NULL DEFAULT 0,

	[BatchId] BIGINT NOT NULL DEFAULT 0,

	[CreatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[UpdatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

	[SqliteSTRId] BIGINT NOT NULL DEFAULT 0,
)
GO

CREATE TABLE [dbo].[DWS]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[STRTagId] BIGINT NOT NULL References dbo.[STRTag],
	[STRId] BIGINT NOT NULL References dbo.[STR],

	[DWSNumber] BIGINT NOT NULL,
	[DWSDate] DATETIME2 NOT NULL,

	[BagCount] BIGINT NOT NULL,
	[FilledBagsWeightKg] DECIMAL(19,2) NOT NULL,
	[EmptyBagsWeightKg] DECIMAL(19,2) NOT NULL,

	[EntityId] BIGINT NOT NULL,
	[EntityName] NVARCHAR(50) NOT NULL,

	[AgreementId] BIGINT NOT NULL,
	[Agreement] NVARCHAR(50) NOT NULL,
	[EntityWorkFlowDetailId] BIGINT NOT NULL,
	[TypeName] NVARCHAR(50) NOT NULL,
	[TagName] NVARCHAR(50) NOT NULL,

	[ActivityId] BIGINT NOT NULL DEFAULT 0,

	[CreatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[UpdatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

	[SqliteSTRDWSId] BIGINT NOT NULL DEFAULT 0,
)
GO

CREATE TABLE [dbo].[STRImage]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[STRId] BIGINT NOT NULL References dbo.[STR],
	[SequenceNumber] INT NOT NULL DEFAULT 0,
	[ImageFileName] VARCHAR(100) NOT NULL DEFAULT 'FileNotFound.jpg'
)
GO

CREATE PROCEDURE [dbo].[ProcessSqliteSTRData]
	@batchId BIGINT
AS
BEGIN

	-- if batch is already processed - return	
	IF NOT EXISTS(SELECT 1 FROM dbo.SqliteActionBatch 
			WHERE Id = @batchId AND STRSavedCount > 0
			AND BatchProcessed = 0)
	BEGIN
		RETURN;
	END

	-- First create required records in Day Table
	INSERT INTO dbo.[Day]
	([DATE])
	SELECT DISTINCT CAST(e.[STRDate] AS [Date])
	FROM dbo.SqliteSTR e
	LEFT JOIN dbo.[Day] d on CAST(e.[STRDate] AS [Date]) = d.[DATE]
	WHERE e.BatchId = @batchId
	AND d.[DATE] IS NULL

	DECLARE @currentTime DATETIME2 = SYSUTCDATETIME()


	-- Create new rows in STRTag
	INSERT INTO dbo.[STRTag]
	(STRNumber, STRDate, CreatedBy, DateCreated, DateUpdated)
	SELECT input.STRNumber, input.STRDate, 'ProcessSqliteSTRData', @currentTime, @currentTime 
	FROM dbo.SqliteSTR input
	LEFT JOIN dbo.[STRTag] tag on input.STRNumber = tag.STRNumber
			  AND input.STRDate = tag.STRDate
    WHERE input.BatchId = @batchId
	AND tag.STRNumber is NULL


	DECLARE @insertedSTR TABLE 
	( 
	  Id BIGINT,   -- STRId
	  STRTagId BIGINT,
	  SqliteSTRId BIGINT,
	  EmployeeId BIGINT
	)

    -- Create Rows in STR table
	INSERT INTO dbo.[STR]
	(STRTagId, EmployeeId, VehicleNumber, DriverName, DriverPhone,
	DWSCount, BagCount, GrossWeight, NetWeight, 
	StartOdometer, EndOdometer, IsNew, IsTransferred, 
	TransfereeName, TransfereePhone,
	ImageCount, ActivityId, ActivityId2, BatchId, CreatedBy, SqliteSTRId,
	DateCreated, DateUpdated
	)
	OUTPUT inserted.Id, inserted.STRTagId, inserted.SqliteSTRId, inserted.EmployeeId INTO @insertedSTR
	SELECT 
	tag.Id, input.EmployeeId, input.VehicleNumber, input.DriverName, input.DriverPhone,
	input.DWSCount, input.BagCount, input.GrossWeight, input.NetWeight,
	input.StartOdometer, input.EndOdometer, input.IsNew, input.IsTransferred, 
	input.TransfereeName, input.TransfereePhone,
	input.ImageCount, sqa.ActivityId, sqa2.ActivityId, input.BatchId, 'ProcessSqliteSTRData', input.Id,
	@currentTime, @currentTime
	FROM dbo.SqliteSTR input
	INNER JOIN dbo.[STRTag] tag on input.STRNumber = tag.STRNumber
	       AND input.STRDate = tag.STRDate
		   AND input.BatchId = @batchId

	INNER JOIN dbo.SqliteAction sqa on sqa.[At] = input.[TimeStamp]
	AND sqa.EmployeeId = input.EmployeeId
	AND sqa.PhoneDbId = input.ActivityId

	INNER JOIN dbo.SqliteAction sqa2 on sqa2.[At] = input.[TimeStamp2]
	AND sqa2.EmployeeId = input.EmployeeId
	AND sqa2.PhoneDbId = input.ActivityId2

	-- Update the values in SqliteSTR table
	UPDATE dbo.SqliteSTR
	SET STRId = t2.Id,
	IsProcessed = 1
	FROM dbo.SqliteSTR t1
	INNER JOIN @insertedSTR t2 on t1.Id = t2.SqliteSTRId

	-- Now Images
	INSERT into dbo.STRImage
	(STRId, SequenceNumber, ImageFileName)
	SELECT 
	 t2.Id, input.SequenceNumber, input.ImageFileName
	 FROM dbo.SqliteSTRImage input
	 INNER JOIN @insertedSTR t2 ON t2.SqliteSTRId = input.SqliteSTRId
	-- only for the rows for which entry is made in STR table.


	DECLARE @insertedDWS TABLE 
	( 
	  Id BIGINT,   -- DWSId
	  [SqliteSTRDWSId] BIGINT
	)

	-- Create Rows in DWS Table
	INSERT INTO dbo.DWS
	(STRTagId, STRId, DWSNumber, DWSDate, BagCount, 
	[FilledBagsWeightKg], [EmptyBagsWeightKg], EntityId, EntityName,
	AgreementId, Agreement, [EntityWorkFlowDetailId], TypeName, TagName, 
	ActivityId, [SqliteSTRDWSId], CreatedBy, DateCreated, DateUpdated
	)
	OUTPUT inserted.Id, inserted.SqliteSTRDWSId into @insertedDWS
	SELECT t2.STRTagId, t2.Id, input.DWSNumber, input.DWSDate, input.BagCount,
	input.[FilledBagsWeightKg], input.[EmptyBagsWeightKg], input.EntityId, input.EntityName,
	input.AgreementId, input.Agreement, input.[EntityWorkFlowDetailId], input.TypeName, input.TagName, 
	sqa.ActivityId, input.Id, 'ProcessSqliteSTRData', @currentTime, @currentTime
	FROM dbo.SqliteSTRDWS input

	INNER JOIN @insertedSTR t2 on t2.SqliteSTRId = input.SqliteSTRId
	-- only for the rows for which entry is made in STR table.

	INNER JOIN dbo.SqliteAction sqa on sqa.[At] = input.[DWSDate]
	AND sqa.EmployeeId = t2.EmployeeId
	AND sqa.PhoneDbId = input.ActivityId

	-- Update id in SqliteSTRDWS table
	UPDATE dbo.SqliteSTRDWS
	SET DWSId = t2.Id,
	IsProcessed = 1
	FROM dbo.SqliteSTRDWS t1
	INNER JOIN @insertedDWS t2 on t1.Id = t2.[SqliteSTRDWSId]
END
GO


ALTER PROCEDURE [dbo].[ClearEmployeeData]
	@employeeId BIGINT
AS
BEGIN
	BEGIN TRY

		BEGIN TRANSACTION

		-- Delete Activity Data
		DECLARE @activity Table ( Id BIGINT )
		DECLARE @image TABLE (Id BIGINT)

		-- first find out the activity ids that need to be deleted
		INSERT INTO @activity (Id)
		SELECT a.id from dbo.Activity a
		INNER JOIN dbo.EmployeeDay ed on ed.Id = a.employeeDayId
		and ed.TenantEmployeeId = @employeeId

		-- delete activity images
		-- store image Ids first - delete images at the end, as we need to delete images for payment as well;
		INSERT INTO @image (Id)
		SELECT ImageId FROM dbo.ActivityImage ai 
					 INNER JOIN @activity a on ai.ActivityId = a.Id

		print 'ActivityImage'
		DELETE FROM dbo.ActivityImage
		WHERE ActivityId IN (SELECT id FROM @activity)

		print 'ActivityContact'
		DELETE FROM dbo.ActivityContact WHERE ActivityId in (SELECT ID FROM @activity)


		print 'SqliteEntityWorkFlow'
		-- this table is no longer used;
		DELETE FROM dbo.SqliteEntityWorkFlow WHERE  ActivityId in (SELECT ID FROM @activity)

		-- April 11 2020
		print 'SqliteEntityWorkFlowItemUsed'
		DELETE FROM [dbo].[SqliteEntityWorkFlowItemUsed]
		WHERE SqliteEntityWorkFlowId IN 
			(SELECT Id FROM dbo.SqliteEntityWorkFlowV2 WHERE EmployeeId = @employeeId)

		-- April 11 2020
		print 'SqliteEntityWorkFlowFollowUp'
		DELETE FROM [dbo].SqliteEntityWorkFlowFollowUp
		WHERE SqliteEntityWorkFlowId IN 
			(SELECT Id FROM dbo.SqliteEntityWorkFlowV2 WHERE EmployeeId = @employeeId)

		print 'SqliteEntityWorkFlowV2'
		DELETE FROM dbo.SqliteEntityWorkFlowV2 WHERE EmployeeId = @employeeId


		print 'Activity'
		DELETE from dbo.Activity WHERE id in (SELECT Id from @activity)

		-----------------
		print 'dbo.DistanceCalcErrorLog'
		DELETE from dbo.DistanceCalcErrorLog
		WHERE id in
		(SELECT l.id from dbo.distanceCalcErrorLog l
		INNER JOIN dbo.Tracking t on l.TrackingId = t.Id
		INNER JOIN dbo.EmployeeDay ed on ed.Id = t.employeeDayId
		and ed.TenantEmployeeId = @employeeId)

		print 'dbo.Tracking'
		DELETE from dbo.Tracking
		WHERE id in
		(SELECT t.id from dbo.Tracking t
		INNER JOIN dbo.EmployeeDay ed on ed.Id = t.employeeDayId
		and ed.TenantEmployeeId = @employeeId)

		print 'employeeDay'
		DELETE from dbo.employeeDay WHERE TenantEmployeeId = @employeeId

		print 'imei'
		DELETE from dbo.Imei WHERE TenantEmployeeId = @employeeId

		--DELETE FROM dbo.SqliteAction WHERE EmployeeId = @employeeId
		--DELETE FROM dbo.SqliteActionBatch WHERE EmployeeId = @employeeId

		-- delete expense Data as well
		print 'SqliteExpenseImage'
		DELETE FROM dbo.SqliteExpenseImage WHERE SqliteExpenseId in (SELECT Id FROM dbo.SqliteExpense WHERE EmployeeId = @employeeId)
		print 'SqliteExpense'
		DELETE FROM dbo.SqliteExpense WHERE EmployeeId = @employeeId

		-- Delete SqliteOrder data
		print 'SqliteOrderItem'
		DELETE FROM dbo.SqliteOrderItem WHERE SqliteOrderId IN (SELECT Id FROM dbo.[SqliteOrder] WHERE EmployeeId = @employeeId)
		print 'SqliteOrderImage'
		DELETE FROM dbo.SqliteOrderImage WHERE SqliteOrderId IN (SELECT Id FROM dbo.[SqliteOrder] WHERE EmployeeId = @employeeId)
		print '[SqliteOrder]'
		DELETE FROM dbo.[SqliteOrder] WHERE EmployeeId = @employeeId

		-- delete SqlLiteAction Data as well
		print 'SqliteActionImage'
		DELETE FROM dbo.SqliteActionImage where SqliteActionId in (SELECT id FROM dbo.SqliteAction WHERE EmployeeId = @employeeId)
		print 'SqliteActionContact'
		DELETE FROM dbo.SqliteActionContact WHERE SqliteActionId in (SELECT id FROM dbo.SqliteAction WHERE EmployeeId = @employeeId)
		print 'SqliteActionLocation'
		DELETE FROM dbo.SqliteActionLocation WHERE SqliteActionId in (SELECT id FROM dbo.SqliteAction WHERE EmployeeId = @employeeId)
		print 'SqliteAction'
		DELETE FROM dbo.SqliteAction WHERE EmployeeId = @employeeId
		print 'SqliteActionDup'
		DELETE FROM dbo.SqliteActionDup WHERE EmployeeId = @employeeId

		-- delete SqlitePayment data as well
		print 'SqlitePaymentImage'
		DELETE FROM dbo.SqlitePaymentImage WHERE SqlitePaymentId in (SELECT Id FROM dbo.SqlitePayment WHERE EmployeeId = @employeeId)
		print 'SqlitePayment'
		DELETE FROM dbo.SqlitePayment WHERE EmployeeId = @employeeId

		-- Delete SqliteReturnOrder data
		print 'SqliteReturnOrderItem'
		DELETE FROM dbo.SqliteReturnOrderItem WHERE SqliteReturnOrderId IN (SELECT Id FROM dbo.[SqliteReturnOrder] WHERE EmployeeId = @employeeId)
		print '[SqliteReturnOrder]'
		DELETE FROM dbo.[SqliteReturnOrder] WHERE EmployeeId = @employeeId

		-- Delete SqliteEntity data
		DECLARE @SqliteEntity TABLE (Id BIGINT)
		INSERT INTO @SqliteEntity SELECT Id FROM dbo.[SqliteEntity] WHERE EmployeeId = @employeeId
		print '[SqliteEntityContact]'
		DELETE FROM dbo.[SqliteEntityContact] WHERE [SqliteEntityId] IN (SELECT Id FROM @SqliteEntity)
		print '[SqliteEntityCrop]'
		DELETE FROM dbo.[SqliteEntityCrop] WHERE [SqliteEntityId] IN (SELECT Id FROM @SqliteEntity)
		print '[SqliteEntityImage]'
		DELETE FROM dbo.[SqliteEntityImage] WHERE [SqliteEntityId] IN (SELECT Id FROM @SqliteEntity)
		print '[SqliteEntityLocation]'
		DELETE FROM dbo.[SqliteEntityLocation] WHERE [SqliteEntityId] IN (SELECT Id FROM @SqliteEntity)
		print '[SqliteEntity]'
		DELETE FROM dbo.[SqliteEntity] WHERE EmployeeId = @employeeId

		-- Delete SqliteLeave data
		print '[SqliteLeave]'
		DELETE FROM dbo.[SqliteLeave] WHERE EmployeeId = @employeeId

		-- Delete SqliteCancelledLeave data
		print '[SqliteCancelledLeave]'
		DELETE FROM dbo.[SqliteCancelledLeave] WHERE EmployeeId = @employeeId
		
		-- 
		print '[SqliteIssueReturn]'
		DELETE FROM dbo.[SqliteIssueReturn] WHERE EmployeeId = @employeeId

		-- Dec 8 2019 
		print '[SqliteAgreement]'
		DELETE FROM dbo.[SqliteAgreement] WHERE EmployeeId = @employeeId

		print '[SqliteAdvanceRequest]'
		DELETE FROM dbo.[SqliteAdvanceRequest] WHERE EmployeeId = @employeeId

		print '[SqliteTerminateAgreement]'
		DELETE FROM dbo.[SqliteTerminateAgreement] WHERE EmployeeId = @employeeId
		-- End Dec 8 2019

		-- March 18 2020
		print '[SqliteBankDetailImage]'
		DELETE FROM dbo.SqliteBankDetailImage WHERE SqliteBankDetailId in
		( SELECT id FROM dbo.SqliteBankDetail WHERE EmployeeId = @employeeId )

		print '[SqliteBankDetail]'
		DELETE FROM dbo.[SqliteBankDetail] WHERE EmployeeId = @employeeId

		-- End March 18 2020

		-- May 10 2020
		print '[SqliteSTRImage]'
		DELETE FROM dbo.SqliteSTRImage WHERE [SqliteSTRId] in
		( SELECT id FROM dbo.SqliteSTR WHERE EmployeeId = @employeeId )

		print '[SqliteSTRDWS]'
		DELETE FROM dbo.SqliteSTRDWS WHERE [SqliteSTRId] in
		( SELECT id FROM dbo.SqliteSTR WHERE EmployeeId = @employeeId )

		print '[SqliteSTR]'
		DELETE FROM dbo.[SqliteSTR] WHERE EmployeeId = @employeeId

		-- End May 10 2020

		-- Delete Device Log
		print '[SqliteDeviceLog]'
		DELETE FROM dbo.[SqliteDeviceLog] WHERE EmployeeId = @employeeId
		print 'SqliteActionBatch'
		DELETE FROM dbo.SqliteActionBatch WHERE EmployeeId = @employeeId

		-- store image ids first for processed expense data
		INSERT INTO @image (id)
		SELECT ImageId 
		FROM dbo.ExpenseItemImage WHERE ExpenseItemId IN 
		(SELECT ei.id FROM dbo.ExpenseItem ei
		INNER JOIN dbo.Expense e on ei.ExpenseId = e.Id
		AND e.EmployeeId = @employeeId)

		print 'ExpenseItemImage'
		DELETE FROM dbo.ExpenseItemImage WHERE ExpenseItemId IN 
		(SELECT ei.id FROM dbo.ExpenseItem ei INNER JOIN dbo.Expense e on ei.ExpenseId = e.Id AND e.EmployeeId = @employeeId)

		print 'ExpenseItem'
		DELETE FROM dbo.ExpenseItem WHERE ExpenseId in (SELECT Id FROM dbo.Expense WHERE EmployeeId = @employeeId)
		print 'ExpenseApproval'
		DELETE FROM dbo.ExpenseApproval WHERE ExpenseId in (SELECT Id FROM dbo.Expense WHERE EmployeeId = @employeeId)
		print 'Expense'
		DELETE FROM dbo.Expense WHERE EmployeeId = @employeeId

		-- Delete order Data
		print 'OrderItem'
		DELETE FROM dbo.OrderItem WHERE OrderId IN (SELECT Id FROM dbo.[Order] WHERE EmployeeId = @employeeId)

		INSERT INTO @image (id)
		SELECT oim.ImageId
		FROM dbo.OrderImage oim
		INNER JOIN dbo.[Order] o on o.Id = oim.OrderId
		AND o.EmployeeId = @employeeId

		print 'OrderImage'
		DELETE FROM dbo.OrderImage WHERE OrderId IN (SELECT Id FROM dbo.[Order] WHERE EmployeeId = @employeeId)

		print '[Order]'
		DELETE FROM dbo.[Order] WHERE EmployeeId = @employeeId


		-- Delete Return Order Data
		print 'ReturnOrderItem'
		DELETE FROM dbo.ReturnOrderItem WHERE ReturnOrderId IN (SELECT Id FROM dbo.[ReturnOrder] WHERE EmployeeId = @employeeId)
		print '[ReturnOrder]'
		DELETE FROM dbo.[ReturnOrder] WHERE EmployeeId = @employeeId

		-- May 12 2020 - DELETE STR/DWS Data
		DECLARE @insertedSTR TABLE 
		( 
		  Id BIGINT,   -- STRId
		  STRTagId BIGINT
		)
		INSERT INTO @insertedSTR
		(Id, STRTagId)
		SELECT Id, STRTagId
		FROM dbo.[STR]
		WHERE EmployeeId = @employeeId

		-- STR may have data for different employee Id as well - tagged to same STRTagId
		-- so select those as well
		INSERT INTO @insertedSTR
		(Id, STRTagId)
		SELECT Id, STRTagId
		FROM dbo.[STR]
		WHERE STRTagId in (SELECT distinct STRTagId FROM @insertedSTR)


		DELETE FROM dbo.STRImage
		WHERE STRId IN (SELECT Id FROM @insertedSTR)

		DELETE FROM dbo.DWS
		WHERE STRId IN (SELECT Id FROM @insertedSTR)

		DELETE FROM dbo.[STR]
		WHERE Id IN (SELECT Id FROM @insertedSTR)

		DELETE FROM dbo.[STRTag]
		WHERE Id IN (SELECT STRTagId FROM @insertedSTR)

		-- End May 12 2020


		-- DELETE Workflow data
		print 'EntityWorkFlowDetailItemUsed'
		DELETE FROM dbo.EntityWorkFlowDetailItemUsed
		WHERE EntityWorkFlowDetailId IN (SELECT d.ID FROM dbo.EntityWorkFlowDetail d
		             INNER JOIN dbo.EntityWorkFlow p on d.EntityWorkFlowId = p.Id
					 AND p.EmployeeId = @employeeId)

		print 'EntityWorkFlowDetail'
		DELETE FROM dbo.EntityWorkFlowDetail
		WHERE ID IN (SELECT d.ID FROM dbo.EntityWorkFlowDetail d
		             INNER JOIN dbo.EntityWorkFlow p on d.EntityWorkFlowId = p.Id
					 AND p.EmployeeId = @employeeId)

		print 'EntityWorkFlow'
		DELETE FROM dbo.EntityWorkFlow WHERE EmployeeId = @employeeId

		print 'Issue/Return'
		DELETE FROM dbo.[IssueReturn] WHERE EmployeeId = @employeeId

		-- Dec 8 2019
		print 'AdvanceRequest'
		DELETE FROM dbo.[AdvanceRequest] WHERE EmployeeId = @employeeId

		print 'TerminateAgreementRequest'
		DELETE FROM dbo.[TerminateAgreementRequest] WHERE EmployeeId = @employeeId

		-- End Dec 8 2019

		-- Delete Entity data
		DECLARE @Entity TABLE (Id BIGINT)
		INSERT INTO @Entity SELECT Id FROM dbo.[Entity] WHERE EmployeeId = @employeeId

		INSERT INTO @image (id)
		SELECT oim.ImageId
		FROM dbo.EntityImage oim
		INNER JOIN dbo.[Entity] o on o.Id = oim.EntityId
		AND o.EmployeeId = @employeeId

		print '[EntityContact]'
		DELETE FROM dbo.[EntityContact] WHERE [EntityId] IN (SELECT Id FROM @Entity)
		print '[EntityCrop]'
		DELETE FROM dbo.[EntityCrop] WHERE [EntityId] IN (SELECT Id FROM @Entity)
		print 'EntityImage'
		DELETE FROM dbo.EntityImage WHERE EntityId IN (SELECT Id FROM @Entity)
		print 'EntityAgreement'
		-- clear foreign key reference first
		UPDATE dbo.[IssueReturn] SET EntityAgreementId = NULL
		WHERE EntityAgreementId IN 
		(
		   SELECT ID FROM dbo.EntityAgreement WHERE EntityId IN (SELECT Id FROM @Entity)
		)

		-- Dec 8 2019
		-- delete data from TerminateAgreementRequest for same agreement that is being deleted
		DELETE FROM dbo.TerminateAgreementRequest
		WHERE EntityAgreementId in
		(
			SELECT ID FROM dbo.EntityAgreement WHERE EntityId IN (SELECT Id FROM @Entity)
		)

		-- delete data from AdvanceRequest for same agreement that is being deleted
		DELETE FROM dbo.AdvanceRequest
		WHERE EntityAgreementId in
		(
			SELECT ID FROM dbo.EntityAgreement WHERE EntityId IN (SELECT Id FROM @Entity)
		)


		DELETE FROM dbo.EntityAgreement WHERE EntityId IN (SELECT Id FROM @Entity)

		-- User 1 has created an entity
		-- User 2 has created a workflow based on this entity
		-- Question: Should we delete the workflow created by user 2 on this entity
		-- Answer: Basic use of this script is to delete the data that is created by test users
		--         during testing on live site.  So the answer is yes.


		print 'EntityWorkFlowDetail - again'
		DELETE FROM dbo.EntityWorkFlowDetail
		WHERE ID IN (SELECT d.ID FROM dbo.EntityWorkFlowDetail d
		             INNER JOIN dbo.EntityWorkFlow p on d.EntityWorkFlowId = p.Id
					 AND p.EntityId in (SELECT Id FROM @Entity))

		print 'EntityWorkFlow - again'
		DELETE FROM dbo.EntityWorkFlow WHERE EntityId in (SELECT Id FROM @Entity)

		print 'IssueReturn - again'
		DELETE FROM dbo.[IssueReturn] WHERE EntityId in (SELECT Id FROM @Entity)

		-- March 19 2020
		print 'EntityBankDetailImage'
		DELETE FROM dbo.EntityBankDetailImage
		WHERE EntityBankDetailId IN
		(SELECT ebd.ID FROM dbo.EntityBankDetail ebd
		INNER JOIN @Entity e2 on e2.Id = ebd.EntityId)

		print 'EntityBankDetail'
		DELETE FROM dbo.EntityBankDetail
		WHERE EntityId IN (SELECT Id from @Entity)
		-- END of changes on March 19 2020


		print '[Entity]'
		DELETE FROM dbo.[Entity] WHERE ID in (SELECT Id from @Entity)

		--
		-- Delete Payment Data
		--
		INSERT INTO @image (id)
		SELECT pim.ImageId
		FROM dbo.PaymentImage pim
		INNER JOIN dbo.Payment p on p.Id = pim.PaymentId
		AND p.EmployeeId = @employeeId

		print 'PaymentImage'
		DELETE FROM dbo.PaymentImage WHERE PaymentId IN (SELECT Id FROM dbo.[Payment] WHERE EmployeeId = @employeeId)
		print '[Payment]'
		DELETE FROM dbo.[Payment] WHERE EmployeeId = @employeeId

		print 'EntityImage - again'
		DELETE FROM dbo.EntityImage WHERE ImageId IN (SELECT Id FROM @image)

		-- DELETE IMAGES
		print 'Image'
		DELETE FROM dbo.[Image]
		WHERE Id in (SELECT Id FROM @image)

		-- CLEAR DEVICE LOG
		print 'SqliteDeviceLog'
		DELETE FROM dbo.SqliteDeviceLog WHERE EmployeeId = @employeeId

		print 'TenantEmployee'
		DELETE from dbo.TenantEmployee WHERE id = @employeeId
		COMMIT
	END TRY

	BEGIN CATCH
		PRINT 'Inside Catch...'
		PRINT ERROR_MESSAGE()
		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:ClearEmployeeData', ERROR_MESSAGE()

		ROLLBACK TRANSACTION
		throw;

	END CATCH
END	
GO


CREATE PROCEDURE [dbo].[ReAssignSTRNumber]
	@strId BIGINT,
	@fromStrTagId BIGINT,
	@toStrTagId BIGINT,
	@updatedBy NVARCHAR(50)
AS
BEGIN
	UPDATE dbo.DWS
	SET STRTagId = @toStrTagId,
	UpdatedBy = @updatedBy,
	DateUpdated = SysUTCDateTime()
	WHERE STRTagId = @fromSTRTagId
	AND STRId = @strId

	UPDATE dbo.[STR]
	SET STRTagId = @toStrTagId,
	UpdatedBy = @updatedBy,
	DateUpdated = SysUTCDateTime()
	WHERE STRTagId = @fromSTRTagId
	AND Id = @strId
END
GO

CREATE PROCEDURE [dbo].[ReAssignDwsSTRNumber]
	@dwsId BIGINT,
	@fromStrTagId BIGINT,
	@toStrTagId BIGINT,
	@updatedBy NVARCHAR(50)
AS
BEGIN
    -- find out original employeeId
	DECLARE @empId BIGINT
	DECLARE @fromStrId BIGINT
	SELECT @empId = s.EmployeeId,
	@fromSTrId = d.StrId
	FROM dbo.DWS d
	INNER JOIN dbo.[STR] s on d.STRId = s.Id
	AND d.Id = @dwsId

	-- Now find out STRId for the empId that is with @toStrTagId
	DECLARE @toStrId BIGINT
	SELECT top 1 @toStrId = Id
	FROM dbo.[STR] s
	WHERE s.StrTagId = @toSTrTagId
	AND s.EmployeeId = @empId

	-- we have to put the DWS in the toStrTag
	-- where Str record belongs to the original user.

	IF @toStrId > 0
	BEGIN
		UPDATE dbo.DWS
		SET STRTagId = @toStrTagId,
		STRId = @toStrId,
		UpdatedBy = @updatedBy,
		DateUpdated = SysUTCDateTime()
		WHERE STRTagId = @fromSTRTagId
		AND Id = @dwsId

		EXEC dbo.RecalculateSTRTotals @fromStrId, @updatedBy
		EXEC dbo.RecalculateSTRTotals @toStrId, @updatedBy
	END
END
GO

CREATE PROCEDURE [dbo].[ReCalculateSTRTotals]
	@strId BIGINT,
	@updatedBy NVARCHAR(50)
AS
BEGIN
	with dwsCTE(recCount, bagCount, FilledBagsWeightKg, EmptyBagsWeightKg)
	AS
	(
		SELECT 
		count(*),
		IsNull(sum(bagCount),0),
		IsNull(Sum(FilledBagsWeightKg),0),
		IsNull(Sum(EmptyBagsWeightKg),0)
		FROM dbo.DWS
		WHERE STRId = @strId
	)
	UPDATE dbo.[STR]
	SET DWSCount = cte.recCount,
	BagCount = cte.bagCount,
	GrossWeight = cte.FilledBagsWeightKg,
	NetWeight = cte.FilledBagsWeightKg - cte.EmptyBagsWeightKg,
	UpdatedBy = @updatedBy,
	DateUpdated = SysUTCDateTime()
	FROM dbo.[STR] s
	INNER JOIN dwsCTE cte ON s.Id = @strId
END
GO

-- May 15 2020
CREATE TABLE [dbo].[STRWeight]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[STRNumber] NVARCHAR(50) NOT NULL,
	[STRDate] DATETIME2 NOT NULL,

	[EntryWeight] DECIMAL(19,2) NOT NULL,
	[ExitWeight] DECIMAL(19,2) NOT NULL,
	[SiloNumber] NVARCHAR(50) NOT NULL,
	[SiloIncharge] NVARCHAR(50) NOT NULL,
	[UnloadingIncharge] NVARCHAR(50) NOT NULL,
	[ExitOdometer] BIGINT NOT NULL,
	[BagCount] BIGINT NOT NULL,
	[DeductionPercent] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Notes] NVARCHAR(1000) NOT NULL DEFAULT '',
	[Status] NVARCHAR(50) NOT NULL DEFAULT 'Pending',

	[CreatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[UpdatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
)
GO
