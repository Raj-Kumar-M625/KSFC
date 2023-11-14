-- May 23 2020

ALTER TABLE [dbo].[DWS]
ADD 
	[SiloDeductWt] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[SiloDeductWtOverride] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[NetPayableWt] DECIMAL(19,2) NOT NULL DEFAULT 0,

	[RatePerKg] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[GoodsPrice] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[DeductAmount] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[NetPayable] DECIMAL(19,2) NOT NULL DEFAULT 0,

	[OrigBagCount] BIGINT NOT NULL DEFAULT 0,
	[OrigFilledBagsKg] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[OrigEmptyBagsKg] DECIMAL(19,2) NOT NULL DEFAULT 0,
	
	[Status] NVARCHAR(50) NOT NULL DEFAULT 'Pending',
	[Comments] NVARCHAR(100) NOT NULL DEFAULT '',

	[WtApprovedBy] NVARCHAR(50) NULL,
	[WtApprovedDate] DATETIME2 NULL,
	[AmountApprovedBy] NVARCHAR(50) NULL,
	[AmountApprovedDate] DATETIME2 NULL,

	[PaidBy] NVARCHAR(50) NULL,
	[PaidDate] DATETIME2 NULL,
	[PaymentReference] NVARCHAR(50) NULL
GO

ALTER TABLE [dbo].[IssueReturn]
ADD 
	[CyclicCount] BIGINT NOT NULL DEFAULT 1
GO

ALTER TABLE [dbo].[STRTag]
ADD
	[IsEditAllowed] BIT NOT NULL DEFAULT 1,
	[CyclicCount] BIGINT NOT NULL DEFAULT 1
GO

ALTER TABLE [dbo].[STRWeight]
ADD
	[IsEditAllowed] BIT NOT NULL DEFAULT 1,
	[CyclicCount] BIGINT NOT NULL DEFAULT 1
GO


ALTER PROCEDURE [dbo].[ProcessSqliteSTRData]
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
	ActivityId, [SqliteSTRDWSId], CreatedBy, DateCreated, DateUpdated,
	[OrigBagCount], [OrigFilledBagsKg], [OrigEmptyBagsKg]
	)
	OUTPUT inserted.Id, inserted.SqliteSTRDWSId into @insertedDWS
	SELECT t2.STRTagId, t2.Id, input.DWSNumber, input.DWSDate, input.BagCount,
	input.[FilledBagsWeightKg], input.[EmptyBagsWeightKg], input.EntityId, input.EntityName,
	input.AgreementId, input.Agreement, input.[EntityWorkFlowDetailId], input.TypeName, input.TagName, 
	sqa.ActivityId, input.Id, 'ProcessSqliteSTRData', @currentTime, @currentTime,
	input.BagCount, input.[FilledBagsWeightKg], input.[EmptyBagsWeightKg]

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

ALTER PROCEDURE [dbo].[ReAssignDwsSTRNumber]
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
	SELECT @toStrId = Id
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

		IF @fromSTRTagId <> @toStrTagId
		BEGIN
			UPDATE dbo.STRTag
			SET CyclicCount = CyclicCount + 1
			WHERE ID IN (@toSTRTagId, @fromSTRTagId)
		END

		EXEC dbo.RecalculateSTRTotals @fromStrId, @updatedBy
		EXEC dbo.RecalculateSTRTotals @toStrId, @updatedBy
	END
END
GO

-- May 25 2020

ALTER TABLE dbo.DWS
ADD SiloDeductPercent Decimal(19,2) NOT NULL DEFAULT 0,
[GoodsWeight] DECIMAL(19,2) NOT NULL DEFAULT 0
GO

DROP Procedure [dbo].[ProcessSqliteEntityWorkFlowData]
GO

DROP Procedure [dbo].[ProcessSqliteEntityWorkFlowDataV2]
GO

ALTER TABLE [dbo].[WorkflowSeason]
ADD	[RatePerKg] DECIMAL(19,2) NOT NULL DEFAULT 0
GO


ALTER TABLE [dbo].[EntityAgreement]
ADD	[RatePerKg] DECIMAL(19,2) NOT NULL DEFAULT 0
GO


-- do this two updates only for PJMargo
UPDATE dbo.WorkFlowSeason
SET RatePerKg = 5.75 WHERE TYPEName='SEEDS'
GO

UPDATE dbo.EntityAgreement
SET RatePerKg = 5.75 
WHERE WorkFlowSeasonId = (SELECT Id from dbo.WorkFlowSeason where TypeName = 'SEEDS')
GO

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
	(EntityId, WorkflowSeasonId, AgreementNumber, LandSizeInAcres, RatePerKg,
	[Status], CreatedBy, UpdatedBy)
	OUTPUT inserted.Id, inserted.AgreementNumber INTO @insertTable
	SELECT ag.EntityId, wfs.Id, agg.AgreementNumber, ag.Acreage, wfs.RatePerKg,
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
------------------


ALTER TABLE [dbo].[STRTag]
ADD [STRWeightId] BIGINT NULL REFERENCES dbo.STRWeight
GO

CREATE PROCEDURE [dbo].[SetDWSStatus]
	@strTagId BIGINT,
	@toDWSStatus NVARCHAR(50),
	@updatedBy NVARCHAR(50)
AS
BEGIN
	UPDATE dbo.DWS
	SET 
	[Status] = @toDWSStatus,
	UpdatedBy = @updatedBy,
	DateUpdated = SysUTCDateTime()
	WHERE STRTagId = @strTagId

	UPDATE dbo.STRTag
	SET CyclicCount = CyclicCount + 1
	WHERE Id = @strTagId
END
GO

CREATE PROCEDURE [dbo].[CalculateDWSOnSiloCheck]
    @strWeightId BIGINT,
	@strTagId BIGINT,
	@updatedBy NVARCHAR(50)
AS
BEGIN
    DECLARE @siloDeductPercent DECIMAL(19,2) = 0
	SELECT @siloDeductPercent = DeductionPercent
	FROM dbo.STRWeight
	WHERE Id = @strWeightId

	;With dwsCTE(Id, GoodsWeight, SiloDeductWt, RatePerKg)
	AS
	(
	  SELECT d.Id, 
	  (FilledBagsWeightKg - EmptyBagsWeightKg),
	  ((FilledBagsWeightKg - EmptyBagsWeightKg) * @siloDeductPercent) / 100.0,
	  ea.RatePerKg
	  FROM dbo.DWS d
	  INNER JOIN dbo.EntityAgreement ea on ea.Id = d.AgreementId
	  WHERE d.STRTagId = @strTagId
	)
	UPDATE dbo.DWS
	SET
	SiloDeductPercent = @siloDeductPercent,
	GoodsWeight = c.GoodsWeight,
	SiloDeductWt = c.SiloDeductWt,
	SiloDeductWtOverride = c.SiloDeductWt,
	NetPayableWt = c.GoodsWeight - c.SiloDeductWt,
	RatePerKg = c.RatePerKg,
	GoodsPrice = (c.GoodsWeight - c.SiloDeductWt) * c.RatePerKg,
	DeductAmount = 0,
	NetPayable = (c.GoodsWeight - c.SiloDeductWt) * c.RatePerKg,
	UpdatedBy = @updatedBy,
	DateUpdated = SysUTCDateTime()
	FROM dbo.DWS d
	INNER JOIN dwsCTE c on d.Id = c.Id


	UPDATE dbo.STRTag
	SET CyclicCount = CyclicCount + 1,
	STRWeightId = @strWeightId
	WHERE Id = @strTagId
END
GO

-- 26.05.2020
ALTER TABLE [dbo].[FeatureControl]
ADD
	[DWSApproveWeightOption] BIT NOT NULL DEFAULT 0,
	[DWSApproveAmountOption] BIT NOT NULL DEFAULT 0,
	[DWSPaymentOption] BIT NOT NULL DEFAULT 0
GO

ALTER TABLE [dbo].[DWS]
ADD
	[HQCode] NVARCHAR(10) NOT NULL DEFAULT ''
GO

ALTER PROCEDURE [dbo].[ProcessSqliteSTRData]
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
	HQCode,
	AgreementId, Agreement, [EntityWorkFlowDetailId], TypeName, TagName, 
	ActivityId, [SqliteSTRDWSId], CreatedBy, DateCreated, DateUpdated,
	[OrigBagCount], [OrigFilledBagsKg], [OrigEmptyBagsKg]
	)
	OUTPUT inserted.Id, inserted.SqliteSTRDWSId into @insertedDWS
	SELECT t2.STRTagId, t2.Id, input.DWSNumber, input.DWSDate, input.BagCount,
	input.[FilledBagsWeightKg], input.[EmptyBagsWeightKg], input.EntityId, input.EntityName,
	wf.HQCode,
	input.AgreementId, input.Agreement, input.[EntityWorkFlowDetailId], input.TypeName, input.TagName, 
	sqa.ActivityId, input.Id, 'ProcessSqliteSTRData', @currentTime, @currentTime,
	input.BagCount, input.[FilledBagsWeightKg], input.[EmptyBagsWeightKg]

	FROM dbo.SqliteSTRDWS input

	INNER JOIN @insertedSTR t2 on t2.SqliteSTRId = input.SqliteSTRId
	-- only for the rows for which entry is made in STR table.

	INNER JOIN dbo.SqliteAction sqa on sqa.[At] = input.[DWSDate]
	AND sqa.EmployeeId = t2.EmployeeId
	AND sqa.PhoneDbId = input.ActivityId

	-- these two joins are to get HQCode
	INNER JOIN dbo.EntityWorkFlowDetail wfd ON wfd.Id = input.EntityWorkFlowDetailId
	INNER JOIN dbo.EntityWorkFlow wf on wf.Id = wfd.EntityWorkFlowId

	-- Update id in SqliteSTRDWS table
	UPDATE dbo.SqliteSTRDWS
	SET DWSId = t2.Id,
	IsProcessed = 1
	FROM dbo.SqliteSTRDWS t1
	INNER JOIN @insertedDWS t2 on t1.Id = t2.[SqliteSTRDWSId]
END
GO

ALTER TABLE [dbo].[DWS]
ADD	[CyclicCount] BIGINT NOT NULL DEFAULT 1
GO


ALTER TABLE [dbo].[DWS]
ALTER COLUMN [Comments] NVARCHAR(500) NOT NULL
GO

-- May 28 2020
CREATE TABLE [dbo].[DWSPaymentReference]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[PaymentReference] NVARCHAR(50) NULL,
	[Comments] NVARCHAR(100) NOT NULL DEFAULT '',
	[TotalNetPayable] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[DWSCount] BIGINT NOT NULL DEFAULT 0,
	[DWSNumbers] NVARCHAR(2048) NULL,

	[CreatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[UpdatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
)
GO

-- May 30 2020
ALTER TABLE [dbo].[DWS]  WITH CHECK ADD FOREIGN KEY([EntityId])
REFERENCES [dbo].[Entity] ([Id])
GO

ALTER TABLE [dbo].[DWS]  WITH CHECK ADD FOREIGN KEY([AgreementId])
REFERENCES [dbo].[EntityAgreement] ([Id])
GO

-- May 31 2020
ALTER TABLE [dbo].[Activity]
ADD	[ActivityTrackingType] INT NOT NULL DEFAULT 0
GO

ALTER PROCEDURE [dbo].[AddActivityData]
	@employeeDayId BIGINT,
	@activityDateTime DateTime2,
	@clientName NVARCHAR(50),
	@clientPhone NVARCHAR(20),
	@clientType NVARCHAR(50),
	@activityType NVARCHAR(50),
	@comments NVARCHAR(2048),
	@clientCode NVARCHAR(50),
	@activityAmount DECIMAL(19,2),
	@atBusiness BIT,
	@imageCount INT,
	@contactCount INT,
	@activityTrackingType INT,
	@activityId BIGINT OUTPUT
AS
BEGIN
   -- Records tracking activity
   SET @activityId = 0
	-- check if a record already exist in Employee Day table
	IF EXISTS(SELECT 1 FROM dbo.EmployeeDay WHERE Id = @employeeDayId)
	BEGIN
		INSERT into dbo.Activity
		(EmployeeDayId, ClientName, ClientPhone, ClientType, ActivityType, Comments, ImageCount, [At], ClientCode, ActivityAmount, AtBusiness, ContactCount, ActivityTrackingType)
		VALUES
		(@employeeDayId, @clientName, @clientPhone, @clientType, @activityType, @comments, @imageCount, @activityDateTime, @clientCode, @activityAmount, @atBusiness, @contactCount, @activityTrackingType)

		SET @activityId = SCOPE_IDENTITY()

		-- create a row in Activity Type if not already there
		IF NOT EXISTS(SELECT 1 FROM dbo.ActivityType WHERE ActivityName = @activityType)
		BEGIN
		    INSERT INTO dbo.ActivityType
			(ActivityName)
			VALUES
			(@activityType)
		END

		-- keep count of total activities for exec crm application
		UPDATE dbo.EmployeeDay
		SET TotalActivityCount = TotalActivityCount + 1
		WHERE Id = @employeeDayId
	END
END
GO

ALTER TABLE [dbo].[STRWeight]
ADD
	[VehicleNumber] NVARCHAR(50) NOT NULL DEFAULT '',
	[DWSCount] BIGINT NOT NULL DEFAULT 0
GO

-- May 31 2020 - 5:37pm
ALTER TABLE [dbo].[BankAccount]
ADD
	[AccountName] NVARCHAR(50) NOT NULL DEFAULT '',
	[AccountAddress] NVARCHAR(50) NOT NULL DEFAULT '',
	[AccountEmail] NVARCHAR(50) NOT NULL DEFAULT ''
GO

ALTER TABLE dbo.DWS
ADD
	[BankAccountName] NVARCHAR(50) NULL,
	[BankName] NVARCHAR(50) NULL,
	[BankAccount] NVARCHAR(50) NULL,
	[BankIFSC] NVARCHAR(50) NULL,
	[BankBranch] NVARCHAR(50) NULL
GO

ALTER TABLE [dbo].[DWSPaymentReference]
ADD
	[AccountNumber] NVARCHAR(50) NULL,
	[AccountName] NVARCHAR(50) NULL,
	[AccountAddress] NVARCHAR(50) NULL,
	[AccountEmail] NVARCHAR(50) NULL,
	[PaymentType]  NVARCHAR(50) NULL,
	[SenderInfo]  NVARCHAR(50) NULL
GO

ALTER TABLE [dbo].[DWSPaymentReference]
ADD
	[LocalTimeStamp] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
GO


ALTER TABLE [dbo].[Tenant]
ADD
	[TransformingDataFeedAt] DATETIME2,
	[ParsingUploadAt] DATETIME2
GO

-- June 5 2020
CREATE PROCEDURE [dbo].[TableList]
AS
BEGIN
		-- Stored procedure to get table names
	SELECT TABLE_NAME
	FROM INFORMATION_SCHEMA.Tables
	WHERE table_Name not like '%Image%'
	AND TABLE_Name not like '%ASPNet%'
	--AND TABLE_Name not like '%Log%'
	ORDER BY TABLE_Name
END
GO

-- June 8 2020
-- BankAccountFeature column has a space at the end-
-- to fix it drop the column and add it again.
-- have to drop its constraint.
--ALTER table dbo.FeatureControl 
--drop constraint DF__FeatureCo__BankA__53D770D6

ALTER table dbo.FeatureControl
drop column BankAccountfeature
GO

ALTER table dbo.FeatureControl
ADD [BankAccountFeature] BIT NOT NULL DEFAULT 0
GO
