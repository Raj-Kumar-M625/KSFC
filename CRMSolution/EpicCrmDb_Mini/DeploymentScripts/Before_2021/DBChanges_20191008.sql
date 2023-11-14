ALTER TABLE [dbo].[FeatureControl]
ADD 
	[WrongLocationReport] BIT NOT NULL DEFAULT 0,
	[RedFarmerModule] BIT NOT NULL DEFAULT 0
GO

ALTER TABLE [dbo].[EntityAgreement]
ADD
	[IsPassbookReceived] BIT NOT NULL DEFAULT 0,
	[PassbookReceivedDate] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[LandSizeInAcres] DECIMAL(19,2) NOT NULL DEFAULT 0
GO

CREATE TABLE [dbo].[Audit](
	[Id] [bigint] IDENTITY(1,1) NOT NULL Primary Key,
	[TableName] [nvarchar](100) NOT NULL,
	[PrimaryKey] [nvarchar](50) NOT NULL,
	[FieldName] [nvarchar](100) NOT NULL,
	[OldValue] [nvarchar](512) NULL,
	[NewValue] [nvarchar](512) NULL,
	[Timestamp] [datetime2](7) NOT NULL DEFAULT (sysutcdatetime()),
	[User] [nvarchar](50) NULL
)
go

--INSERT into dbo.codeTable
--(codeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
--values
--('ActiveCrop', '', 'Gherkin', 10, 1, 1),
--('ActiveCrop', '', 'Baby corn', 20, 1, 1),
--('ActiveCrop', '', 'Jalapenos', 30, 1, 1),
--('ActiveCrop', '', 'Piri Piri', 40, 1, 1)
--go

INSERT into dbo.codeTable
(codeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
('TerminateAgreementReason', '', 'Severe pest & disease infestation', 10, 1, 1),
('TerminateAgreementReason', '', 'Water scarcity', 20, 1, 1),
('TerminateAgreementReason', '', 'Power issue', 30, 1, 1),
('TerminateAgreementReason', '', 'Heavy rain damage', 40, 1, 1),
('TerminateAgreementReason', '', 'Water stagnation', 50, 1, 1),
('TerminateAgreementReason', '', 'Animal damage', 60, 1, 1),
('TerminateAgreementReason', '', 'Poor germination/stunted growth', 70, 1, 1),
('TerminateAgreementReason', '', 'Poor management', 80, 1, 1),
('TerminateAgreementReason', '', 'Others', 90, 1, 1)
go

INSERT into dbo.codeTable
(codeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
('RedFarmerRequestStatus', '', 'Pending', 10, 1, 1),
('RedFarmerRequestStatus', 'Yes', 'Approved', 20, 1, 1),
('RedFarmerRequestStatus', 'No', 'Not Approved', 30, 1, 1)
go

INSERT into dbo.codeTable
(codeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
('AgreementStatus', '', 'No Agreement', 5, 1, 1),
('AgreementStatus', '', 'Not approved', 25, 1, 1),
('AgreementStatus', '', 'Closed', 40, 1, 1)
go

-- Oct 10 2019
ALTER TABLE [dbo].[EntityAgreement]
ALTER COLUMN	[Status] NVARCHAR(50) NOT NULL
GO

CREATE TABLE [dbo].[TerminateAgreementRequest]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,

	[EmployeeId] BIGINT NOT NULL REFERENCES TenantEmployee(Id),
	[DayId] BIGINT NOT NULL REFERENCES dbo.[Day](Id),
	[EntityId] BIGINT NOT NULL REFERENCES dbo.Entity,
	[EntityAgreementId] BIGINT NOT NULL REFERENCES [EntityAgreement]([Id]), 

	[RequestDate] DATE NOT NULL,
	[RequestReason] NVARCHAR(50) NOT NULL,
	[Status] NVARCHAR(50) NOT NULL DEFAULT 'Pending',
	[ActivityId] BIGINT NOT NULL DEFAULT 0,

	[RequestNotes] NVARCHAR(512),

	[SqliteTerminateAgreementId] BIGINT NOT NULL,
	[ReviewNotes] NVARCHAR(512),
	[ReviewedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[ReviewDate] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[CreatedBy] NVARCHAR(50) NOT NULL,
	[UpdatedBy] NVARCHAR(50) NOT NULL
)
GO

ALTER TABLE [dbo].[EntityWorkFlow]  WITH CHECK ADD FOREIGN KEY([AgreementId])
REFERENCES [dbo].[EntityAgreement] ([Id])
GO

-- run only in reitzel test environment
/*
delete from dbo.EntityWorkFlowDetail
where entityWorkFlowId in (
select id from dbo.EntityWorkFlow
where agreementId = 0
)

delete from dbo.EntityWorkFlow where AgreementId = 0
*/

ALTER TABLE [dbo].[EntityWorkFlow]  WITH CHECK ADD FOREIGN KEY([AgreementId])
REFERENCES [dbo].[EntityAgreement] ([Id])
GO


CREATE TABLE [dbo].[AgreementNumber]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[Sequence] BIGINT NOT NULL,
	[AgreementNumber] NVARCHAR(50) NOT NULL,
	[IsUsed] BIT NOT NULL DEFAULT 0,
	[UsedTimestamp] DATETIME2 NOT NULL DEFAULT SysUtcDateTime()
)
go

CREATE TABLE [dbo].[AgreementNumberInput]
(
	[Sequence] BIGINT NOT NULL,
	[AgreementNumber] NVARCHAR(50) NOT NULL
)
go

CREATE PROCEDURE [dbo].[TransformAgreementNumberData]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
	
		DELETE FROM dbo.[AgreementNumber]
		WHERE IsUsed = 0

		-- Step 2: Insert data
		INSERT INTO dbo.[AgreementNumber]
		([Sequence], AgreementNumber)
		SELECT  
		ani.[Sequence],
		ltrim(rtrim(ani.[AgreementNumber]))
		FROM dbo.AgreementNumberInput ani
		LEFT JOIN dbo.AgreementNumber an on ani.AgreementNumber = an.AgreementNumber
		WHERE an.AgreementNumber is null

		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformAgreementNumberData', 'Success'
END
GO

INSERT into dbo.codeTable
(codeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
('ExcelUpload', 'AgreementNumber', 'AgreementNumberInput', 100, 1, 1)
go

ALTER TABLE [dbo].[WorkFlowSchedule]
ADD	[TypeName] NVARCHAR(50) NOT NULL DEFAULT 'Gherkin'
GO

-- Oct 15 2019
DROP TABLE [dbo].[SqliteIssueReturn]
GO
CREATE TABLE [dbo].[SqliteIssueReturn]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[BatchId] BIGINT NOT NULL REFERENCES dbo.SqliteActionBatch,
	[EmployeeId] BIGINT NOT NULL,

	[IsNewEntity] BIT NOT NULL,
	[IsNewAgreement] BIT NOT NULL,

	[EntityId] BIGINT NOT NULL,
	[EntityName] NVARCHAR(50) NOT NULL,
	[AgreementId] BIGINT NOT NULL DEFAULT 0,
	[Agreement] NVARCHAR(50) NOT NULL,
	[TranType] NVARCHAR(50) NOT NULL,
	[ItemId] BIGINT NOT NULL,
	[ItemCode] NVARCHAR(100) NOT NULL,
	[Quantity] INT NOT NULL,
	[IssueReturnDate] DATETIME2 NOT NULL,

	[ActivityId] NVARCHAR(50) NOT NULL DEFAULT '',
	[ParentReferenceId]  NVARCHAR(50) NOT NULL DEFAULT '',

	[IsProcessed] BIT NOT NULL DEFAULT 0,  
	[IssueReturnId] BIGINT NOT NULL DEFAULT 0,
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
)
GO

CREATE TABLE [dbo].[SqliteAgreement]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[BatchId] BIGINT NOT NULL REFERENCES dbo.SqliteActionBatch,
	[EmployeeId] BIGINT NOT NULL,

	[IsNewEntity] BIT NOT NULL,
	[EntityId] BIGINT NOT NULL,
	[EntityName] NVARCHAR(50) NOT NULL,

	[SeasonName] NVARCHAR(50) NOT NULL,
	[TypeName] NVARCHAR(50) NOT NULL,
	[Acreage] DECIMAL(19,2) NOT NULL,
	[AgreementDate] DATETIME2 NOT NULL,

	[ActivityId] NVARCHAR(50) NOT NULL DEFAULT '',
	[PhoneDbId] NVARCHAR(50) NOT NULL, 
	[ParentReferenceId]  NVARCHAR(50) NOT NULL DEFAULT '',

	[IsProcessed] BIT NOT NULL DEFAULT 0,  
	[EntityAgreementId] BIGINT NOT NULL DEFAULT 0,
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
)
GO

CREATE TABLE [dbo].[SqliteAdvanceRequest]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[BatchId] BIGINT NOT NULL REFERENCES dbo.SqliteActionBatch,
	[EmployeeId] BIGINT NOT NULL,

	[IsNewEntity] BIT NOT NULL,
	[IsNewAgreement] BIT NOT NULL,

	[EntityId] BIGINT NOT NULL,
	[EntityName] NVARCHAR(50) NOT NULL,
	[AgreementId] BIGINT NOT NULL DEFAULT 0,
	[Agreement] NVARCHAR(50) NOT NULL,

	[Amount] DECIMAL(19,2) NOT NULL,
	[AdvanceRequestDate] DATETIME2 NOT NULL,
	[Notes] NVARCHAR(512) NOT NULL DEFAULT '',
	[ActivityId] NVARCHAR(50) NOT NULL DEFAULT '',
	[ParentReferenceId]  NVARCHAR(50) NOT NULL DEFAULT '',

	[IsProcessed] BIT NOT NULL DEFAULT 0,  
	[AdvanceRequestId] BIGINT NOT NULL DEFAULT 0,
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
)
GO

ALTER TABLE [dbo].[SqliteActionBatch]
ADD
	[Agreements] BIGINT NOT NULL DEFAULT 0,
	[AgreementsSaved] BIGINT NOT NULL DEFAULT 0,

	[AdvanceRequests] BIGINT NOT NULL DEFAULT 0,
	[AdvanceRequestsSaved] BIGINT NOT NULL DEFAULT 0,

	[TerminateRequests] BIGINT NOT NULL DEFAULT 0,
	[TerminateRequestsSaved] BIGINT NOT NULL DEFAULT 0
GO

ALTER TABLE [dbo].[FeatureControl]
ADD 
	[AdvanceRequestModule] BIT NOT NULL DEFAULT 0
GO

-- Oct 16 2019
CREATE PROCEDURE [dbo].[ProcessSqliteAgreementData]
	@batchId BIGINT
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
	'Pending', 'ProcessSqliteAgreementData', 'ProcessSqliteAgreementData'
	FROM dbo.SqliteAgreement ag
	INNER JOIN dbo.WorkflowSeason wfs on wfs.TypeName = ag.TypeName
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

	-- Create Input/Issue Records
	INSERT INTO dbo.[IssueReturn]
	([EmployeeId], [DayId], [EntityId], 
	[EntityAgreementId], 
	[ItemMasterId],
	[TransactionDate], [TransactionType], [Quantity], [ActivityId],
	[SqliteIssueReturnId])

	SELECT 
	sqe.[EmployeeId], d.[Id] AS [DayId], sqe.EntityId, 
	CASE WHEN sqe.[AgreementId] = 0 THEN NULL ELSE sqe.[AgreementId] END,
	sqe.[ItemId],
	CAST(sqe.[IssueReturnDate] AS [Date]), sqe.[TranType], sqe.[Quantity], sqa.ActivityId,
	sqe.[Id]

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
END
GO

CREATE TABLE [dbo].[AdvanceRequest]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[EmployeeId] BIGINT NOT NULL REFERENCES TenantEmployee,
	[DayId] BIGINT NOT NULL REFERENCES dbo.[Day],
	[EntityId] BIGINT NOT NULL REFERENCES dbo.Entity,
	[EntityAgreementId] BIGINT NOT NULL REFERENCES dbo.EntityAgreement,

	[AdvanceRequestDate] DATE NOT NULL,
	[Amount] DECIMAL(19,2) NOT NULL,

	[ActivityId] BIGINT NOT NULL,
	[SqliteAdvanceRequestId] BIGINT NOT NULL DEFAULT 0,
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

	[RequestNotes] NVARCHAR(512),
	[Status] NVARCHAR(50) NOT NULL DEFAULT 'Pending',
	[AmountApproved] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[ApproveNotes] NVARCHAR(512),
	[ReviewedBy] NVARCHAR(50),
	[ReviewDate] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[CreatedBy] NVARCHAR(50),
	[UpdatedBy] NVARCHAR(50),
)
GO

CREATE PROCEDURE [dbo].[ProcessSqliteAdvanceRequestData]
	@batchId BIGINT
AS
BEGIN

	-- if batch is already processed - return	
	IF NOT EXISTS(SELECT 1 FROM dbo.SqliteActionBatch 
			WHERE Id = @batchId AND AdvanceRequestsSaved > 0
			AND BatchProcessed = 0)
	BEGIN
		RETURN;
	END

	-- First create required records in Day Table
	INSERT INTO dbo.[Day]
	([DATE])
	SELECT DISTINCT CAST(e.[AdvanceRequestDate] AS [Date])
	FROM dbo.SqliteAdvanceRequest e
	LEFT JOIN dbo.[Day] d on CAST(e.[AdvanceRequestDate] AS [Date]) = d.[DATE]
	WHERE e.BatchId = @batchId
	AND d.[DATE] IS NULL

	-- SqliteAdvanceRequest can have Advance Requests for Agreements, that were
	-- created on phone and not uploaded.  For such records, we have to first
	-- fill in AgreementId in SqliteAdvanceRequest table
	UPDATE dbo.SqliteAdvanceRequest
	SET AgreementId = agg.EntityAgreementId,
	EntityId = agg.EntityId,
	DateUpdated = SYSUTCDATETIME()
	FROM dbo.SqliteAdvanceRequest sqe
	INNER JOIN dbo.SqliteAgreement agg on sqe.ParentReferenceId = agg.PhoneDbId
	and sqe.IsNewAgreement = 1
	and agg.BatchId <= @batchId -- agreement has to come in same batch or before
	and sqe.IsProcessed = 0


	-- select current max Id from dbo.AdvanceRequest
	DECLARE @lastMaxId BIGINT
	SELECT @lastMaxId = ISNULL(MAX(Id),0) FROM dbo.AdvanceRequest

	-- Create Input/Issue Records
	INSERT INTO dbo.[AdvanceRequest]
	([EmployeeId], [DayId], [EntityId], 
	[EntityAgreementId],
	[AdvanceRequestDate], [Amount],	[ActivityId],
	[RequestNotes], [Status], CreatedBy, UpdatedBy,
	[SqliteAdvanceRequestId])
	SELECT 
	sqe.[EmployeeId], d.[Id] AS [DayId], sqe.EntityId, 
	sqe.[AgreementId],
	CAST(sqe.[AdvanceRequestDate] AS [Date]), sqe.[Amount], sqa.ActivityId,
	sqe.Notes, 'Pending', 'ProcessSqliteAdvanceRequestData', 'ProcessSqliteAdvanceRequestData',
	sqe.[Id]
	FROM dbo.SqliteAdvanceRequest sqe
	INNER JOIN dbo.[Day] d ON d.[DATE] = CAST(sqe.[AdvanceRequestDate] AS [Date])
	-- this join is to get activity Id
	INNER JOIN dbo.SqliteAction sqa on sqa.EmployeeId = sqe.EmployeeId
			AND sqa.[At] = sqe.AdvanceRequestDate
			AND sqa.PhoneDbId = sqe.ActivityId
	WHERE sqe.BatchId = @batchId AND sqe.IsProcessed = 0
	AND sqe.EntityId > 0 
	AND sqe.AgreementId > 0
	ORDER BY sqe.Id

	-- now we need to update the id in SqliteAdvanceRequest table
	UPDATE dbo.SqliteAdvanceRequest
	SET AdvanceRequestId = e.Id,
	IsProcessed = 1,
	DateUpdated = SYSUTCDATETIME()
	FROM dbo.SqliteAdvanceRequest se
	INNER JOIN dbo.[AdvanceRequest] e on se.Id = e.SqliteAdvanceRequestId
	AND se.BatchId = @batchId
	AND e.Id > @lastMaxId
END
GO


INSERT INTO dbo.WorkflowSeason
(SeasonName, TypeName, StartDate, EndDate, IsOpen)
values
('Babycorn_0419', 'Babycorn', '2019-04-02', '2099-12-31', 1),
('JalapenoPepper_0419', 'Jalapeno pepper', '2019-04-02', '2099-12-31', 1),
('BananaPepper_0419', 'Banana pepper', '2019-04-02', '2099-12-31', 1),
('PiriPiriChilliGreen_0419', 'Piri Piri chilli-Green', '2019-04-02', '2099-12-31', 1),
('PiriPiriChilliRed_0419', 'Piri Piri chilli-Red', '2019-04-02', '2099-12-31', 1)
go

-- an entity can have multiple agreements
-- so drop unique index and create index
drop index IX_EntityAgreement_EntityId  on dbo.EntityAgreement
go

CREATE INDEX IX_EntityAgreement_EntityId
ON dbo.EntityAgreement (EntityId)
go


-- Oct 17 2019
insert into dbo.WorkFlowSchedule
([Sequence], Phase, TargetStartAtDay, TargetEndAtDay, IsActive, PhoneDataEntryPage, TypeName)
values
(10, 'Sowing Confirmation', 1, 15, 1, '', 'Babycorn'),
(20, 'Weeding', 15, 30, 1, '', 'Babycorn'),
(30, 'First Harvest', 50, 70, 1, '', 'Babycorn'),

(10, 'Transplanting', 1, 15, 1, '', 'Jalapeno pepper'),
(20, 'Weeding', 15, 25, 1, '', 'Jalapeno pepper'),
(30, 'First Harvest', 55, 65, 1, '', 'Jalapeno pepper'),

(10, 'Transplanting', 1, 15, 1, '', 'Banana pepper'),
(20, 'Weeding', 15, 25, 1, '', 'Banana pepper'),
(30, 'First Harvest', 45, 55, 1, '', 'Banana pepper'),

(10, 'Transplanting', 1, 15, 1, '', 'Piri Piri chilli-Green'),
(20, 'Weeding', 15, 25, 1, '', 'Piri Piri chilli-Green'),
(30, 'First Harvest', 65, 75, 1, '', 'Piri Piri chilli-Green'),


(10, 'Transplanting', 1, 15, 1, '', 'Piri Piri chilli-Red'),
(20, 'Weeding', 15, 25, 1, '', 'Piri Piri chilli-Red'),
(30, 'First Harvest', 80, 95, 1, '', 'Piri Piri chilli-Red')
GO

-- Set page to common page now for all new crops
UPDATE dbo.WorkFlowSchedule
set PhoneDataEntryPage = 'CommonWorkflowEntryPage'
where PhoneDataEntryPage = ''
go

update dbo.WorkFlowSchedule
set PhoneDataEntryPage = 'SowingWorkflowEntryPage'
WHERE Typename in ('Babycorn', 'Jalapeno pepper', 'Banana pepper', 'Piri Piri chilli-Green', 'Piri Piri chilli-Red')
and phase in ('Sowing Confirmation', 'Transplanting')

update dbo.WorkFlowSchedule
set PhoneDataEntryPage = 'FirstHarvestWorkflowEntryPage'
WHERE Typename in ('Babycorn', 'Jalapeno pepper', 'Banana pepper', 'Piri Piri chilli-Green', 'Piri Piri chilli-Red')
and phase in ('First Harvest')



ALTER TABLE [dbo].[SqliteEntityWorkFlowV2]
ADD
	[TypeName] NVARCHAR(50) NOT NULL DEFAULT ''
GO

ALTER TABLE [dbo].[EntityWorkFlow]
ADD
	[TypeName] NVARCHAR(50) NOT NULL DEFAULT ''
GO

ALTER PROCEDURE [dbo].[ProcessSqliteEntityWorkFlowDataV2]
	@batchId BIGINT
AS
BEGIN
   /*
    Conditions taken care of:
	a) Can have more than one open work flow for an entity
	   (but not of same type name)
	b) Duplicate phase activity records are ignored

	a can happen in following scenario
	   - Downloaded entities on phone
	   - Created a new workflow for an entity
	   - Uploaded the batch
	   - (batch is either under process or I did not download again)
	   - Create another start work flow activity for same entity
	   - Upload to server

	b can happen in similar scenario as above

	In the same batch, I can create multiple activities for same entity
	(duplicate activities for same entity in same batch are ignored)

	If we receive duplicates subsequently, it won't update the detail table
	as the phase is marked as complete.
	  
   */

	DECLARE @entityWorkFlow TABLE 
	(
		[Id] BIGINT,
		[Phase] NVARCHAR(50) NOT NULL,
		[PhaseStartDate] DATE NOT NULL,
		[PhaseEndDate] DATE NOT NULL
	)

	DECLARE @sqliteEntityWorkFlow TABLE 
	(ID BIGINT, 
	 EmployeeId BIGINT,
	 EmployeeCode NVARCHAR(10),
	 [HQCode] NVARCHAR(10),
	 [Date] DATE
	)

	IF NOT EXISTS(SELECT 1 FROM dbo.SqliteActionBatch 
			WHERE Id = @batchId AND NumberOfWorkFlowSaved > 0
			AND BatchProcessed = 0)
	BEGIN
		RETURN;
	END

	---- if there are no unprocessed entries - return
	--IF NOT EXISTS(SELECT 1 FROM @sqliteEntityWorkFlow)
	--BEGIN
	--	RETURN;
	--END

	-- fill Entity Id - if zero 
	-- not needed now - as we are doing workflow only for approved agreements,
	-- which will have both entityId and Agreement Id
	/*
	UPDATE dbo.SqliteEntityWorkFlowV2
	SET EntityId = e.Id
	FROM dbo.SqliteEntityWorkFlowV2 sewf
	INNER JOIN dbo.Entity e on sewf.EntityType = e.EntityType
	AND sewf.EntityName = e.EntityName
	AND sewf.EntityId = 0
	AND sewf.BatchId = @batchId
	*/
	
	-- For one Agreement, we will process only one row
	-- so refresh @sqliteEntityWorkFlowV2 by removing duplicates
	-- Also
	-- Now in @sqliteEntityWorkFlow table
	-- fill EmployeeId, EmployeeCode and Date
	-- this is done so that we don't have to join the tables two times
	-- in following queries.
	;WITH singleRecCTE(Id, rownum)
	AS
	(
		SELECT sewf.Id,
		ROW_NUMBER() Over (Partition By sewf.AgreementId ORDER BY sewf.Id)
		FROM dbo.SqliteEntityWorkFlowV2 sewf
		WHERE BatchId = @batchId
	)
	INSERT INTO @sqliteEntityWorkFlow 
	(ID, EmployeeId, EmployeeCode, [Date], HQCode)
	SELECT s2.Id, s2.EmployeeId, te.EmployeeCode, s2.[Date], sp.[HQCode]
	FROM singleRecCTE cte
	INNER JOIN dbo.SqliteEntityWorkFlowV2 s2 on cte.Id = s2.Id
	INNER JOIN dbo.TenantEmployee te on s2.EmployeeId = te.Id
	INNER JOIN dbo.SalesPerson sp on te.EmployeeCode = sp.StaffCode
	WHERE cte.rownum = 1
	AND s2.IsProcessed = 0


	-- Create a in memory table of work flow schedule
	-- this will have an additional column indicating previous phase
	-- Work flow schedule is a small table with say 5 to 10 rows.
	DECLARE @WorkFlowSchedule TABLE
	(
		[Sequence] INT NOT NULL,
		[TypeName] NVARCHAR(50) NOT NULL,
		[Phase] NVARCHAR(50) NOT NULL,
		[TargetStartAtDay] INT NOT NULL,
		[TargetEndAtDay] INT NOT NULL,
		[PrevPhase] NVARCHAR(50) NOT NULL
	)

	;with schCTE([Sequence], [TypeName], Phase, TargetStartAtDay, TargetEndAtDay, rownum)
	AS
	(
		SELECT [Sequence], TypeName, Phase,  TargetStartAtDay, TargetEndAtDay,
		ROW_NUMBER() OVER (Order By [TypeName],[Sequence])
		FROM dbo.[WorkFlowSchedule]
		WHERE IsActive = 1
	)
	INSERT INTO @WorkFlowSchedule
	([Sequence], [TypeName], [Phase], [TargetStartAtDay], [TargetEndAtDay], [PrevPhase])
	SELECT [Sequence], [TypeName], Phase, TargetStartAtDay, TargetEndAtDay,
	ISNULL((SELECT Phase FROM schCTE WHERE rownum = p.rownum-1 and TypeName = p.TypeName), '') PrevPhase
	FROM schCTE p


	DECLARE @firstWorkflowStep TABLE
	(
		[TypeName] NVARCHAR(50) NOT NULL,
		[Phase] NVARCHAR(50) NOT NULL
	)
	-- Select first step in workflow for each crop
	;with phaseCTE(TypeName, Phase, [Sequence], rownum)
	AS
	(
		SELECT TypeName, Phase, [Sequence],
		ROW_NUMBER() OVER (Partition BY [TypeName] Order By [Sequence])
		from dbo.WorkFlowSchedule
	)
	INSERT INTO @firstWorkflowStep
	(TypeName, Phase)
	SELECT TypeName, Phase
	FROM phaseCTE
	WHERE rownum = 1

	DECLARE @newid NVARCHAR(50) = newid()
	DECLARE @newWorkFlow TABLE (ID BIGINT)

	-- INSERT NEW Rows in EntityWorkFlow 
	INSERT into dbo.EntityWorkFlow
	(EmployeeId, [EmployeeCode], HQCode, EntityId, EntityName, 
	TypeName, CurrentPhase, [CurrentPhaseStartDate],
	[CurrentPhaseEndDate], [InitiationDate], [IsComplete],
	[AgreementId], [Agreement])
	OUTPUT inserted.Id INTO @newWorkFlow
	SELECT mem.EmployeeId, mem.EmployeeCode, mem.HQCode, sewf.EntityId, sewf.EntityName,
	sewf.TypeName, @newid, '2000-01-01',
	'2000-01-01', mem.[Date], 0, 
	sewf.AgreementId, sewf.Agreement
	FROM dbo.SqliteEntityWorkFlowV2 sewf
	INNER JOIN @sqliteEntityWorkFlow mem ON mem.ID = sewf.Id
	INNER JOIN @firstWorkflowStep fs ON sewf.TypeName = fs.TypeName
	AND sewf.Phase = fs.Phase
	-- there can be only one open work flow for an agreement
	AND NOT EXISTS (SELECT 1 FROM dbo.EntityWorkFlow ewf2 
				WHERE ewf2.IsComplete = 0
				AND ewf2.EntityId = sewf.EntityId
				AND ewf2.AgreementId = sewf.AgreementId
				)


	-- now create detail entries for newly created work flow
	INSERT INTO dbo.EntityWorkFlowDetail
	(EntityWorkFlowId, [Sequence], [Phase], 
	[PlannedStartDate], [PlannedEndDate], 
	[PrevPhase])
	SELECT wf.Id, sch.[Sequence], sch.Phase, 
	DATEADD(d, sch.TargetStartAtDay, wf.InitiationDate), DATEADD(d, sch.TargetEndAtDay, wf.InitiationDate),
	sch.PrevPhase
	FROM dbo.EntityWorkFlow wf
	INNER JOIN @newWorkFlow nwf on wf.Id = nwf.ID
	INNER JOIN @WorkFlowSchedule sch ON wf.TypeName = sch.TypeName


	DECLARE @updateTime DATETIME2 = SYSUTCDATETIME()
	DECLARE @updatedWorkFlowId TABLE 
	( 
	  Id BIGINT, 
	  ParentId BIGINT, 
	  PhaseDate DATE, 
	  Phase NVARCHAR(50), 
	  BatchId BIGINT 
	)

	-- Now we have parent/child entries for all (including new) work flow requests
	-- Update the status in Detail table
	-- This will output the Ids of parent table (EntityWorkFlow), for which details 
	-- have been updated.
	UPDATE dbo.EntityWorkFlowDetail
	SET ActivityId = sqa.ActivityId,
	IsComplete = 1,
	ActualDate = sewf.[FieldVisitDate],
	[Timestamp] = @updateTime,
	--[PrevPhaseActualDate] = (
	--							SELECT ActualDate 
	--							FROM dbo.EntityWorkFlowDetail d2 
	--							WHERE d2.EntityWorkFlowId = wfd.EntityWorkFlowId 
	--								AND Phase = wfd.PrevPhase
	--						),
	PhaseCompleteStatus = CASE WHEN sewf.[Date] < wfd.PlannedStartDate THEN 'Early' 
							   WHEN sewf.[Date] > wfd.PlannedEndDate THEN 'Late'
							   WHEN sewf.[Date] >= wfd.PlannedStartDate AND sewf.[Date] <= wfd.PlannedEndDate THEN 'OnSchedule'
							   ELSE ''
							END,
	EmployeeId = mem.EmployeeId,
	-- 19.4.19
	IsStarted = sewf.IsStarted,
	WorkFlowDate = sewf.[Date],
	MaterialType = sewf.MaterialType,
	MaterialQuantity = sewf.MaterialQuantity,
	GapFillingRequired = sewf.GapFillingRequired,
	GapFillingSeedQuantity = sewf.GapFillingSeedQuantity,
	LaborCount = sewf.LaborCount,
	PercentCompleted = sewf.PercentCompleted,
	BatchId = sewf.BatchId
	OUTPUT inserted.Id, inserted.EntityWorkFlowId, inserted.ActualDate, 
		inserted.Phase, inserted.BatchId
	INTO @updatedWorkFlowId

	FROM dbo.EntityWorkFlowDetail wfd
	INNER JOIN dbo.EntityWorkFlow wf on wfd.EntityWorkFlowId = wf.Id
		AND wf.IsComplete = 0
		AND wfd.IsComplete = 0
	INNER JOIN dbo.SqliteEntityWorkFlowV2 sewf ON sewf.EntityId = wf.EntityId
	AND sewf.AgreementId = wf.AgreementId
	AND wfd.Phase = sewf.Phase
	INNER JOIN @sqliteEntityWorkFlow mem ON mem.ID = sewf.Id
	-- this join is to get activity Id
	INNER JOIN dbo.SqliteAction sqa on sqa.[At] = sewf.FieldVisitDate
	AND sqa.EmployeeId = mem.EmployeeId
	AND sqa.PhoneDbId = sewf.ActivityId


	-- put the prev phase actual date, in next phase row of detail table
	UPDATE dbo.EntityWorkFlowDetail
	SET PrevPhaseActualDate = u.PhaseDate
	FROM dbo.EntityWorkFlowDetail d
	INNER JOIN @updatedWorkFlowId u on d.EntityWorkFlowId = u.ParentId
	AND d.PrevPhase = u.Phase

	-- Find out current phase that need to be updated in parent table
	;WITH updateRecCTE(Id, [Sequence], Phase, PlannedStartDate, PlannedEndDate, rownumber)
	AS
	(
		SELECT uwf.ParentId, wfd.[Sequence], wfd.[Phase], wfd.PlannedStartDate, wfd.PlannedEndDate,
		ROW_NUMBER() OVER (PARTITION BY uwf.ParentId Order By wfd.[Sequence])
		FROM @updatedWorkFlowId uwf
		INNER JOIN dbo.EntityWorkFlowDetail wfd on uwf.ParentId = wfd.EntityWorkFlowId
		AND wfd.IsComplete = 0
	)
	INSERT INTO @entityWorkFlow
	(Id, Phase, PhaseStartDate, PhaseEndDate)
	SELECT Id, Phase, PlannedStartDate, PlannedEndDate FROM updateRecCTE
	WHERE rownumber = 1
	

	-- Now update the status values in Parent table
	-- (this only updates the rows when atleast there is one pending stage)
	UPDATE dbo.EntityWorkFlow
	SET CurrentPhase = memWf.Phase,
	CurrentPhaseStartDate = memWf.PhaseStartDate,
	CurrentPhaseEndDate = memWf.PhaseEndDate,
	[Timestamp] = @updateTime
	FROM dbo.EntityWorkFlow wf
	INNER JOIN @entityWorkFlow memWf ON wf.Id = memWf.Id

	-- Now mark the rows as complete where phase is marked as complete in Detail table
	-- (this will happen only when all phases are complete, otherwise previous sql
	--  will have already set the phase in parent to next available phase)
	UPDATE dbo.EntityWorkFlow
	SET IsComplete = 1,
	[Timestamp] = @updateTime
	FROM dbo.EntityWorkFlow wf
	INNER JOIN @updatedWorkFlowId uwf on wf.Id = uwf.ParentId
	INNER JOIN dbo.EntityWorkFlowDetail wfd on wfd.EntityWorkFlowId = wf.Id
	AND wfd.Phase = wf.CurrentPhase
	AND wfd.IsComplete = 1

	-- Now mark the status in SqliteEntityWorkFlow table
	UPDATE dbo.SqliteEntityWorkFlowV2
	SET IsProcessed = 1,
	[Timestamp] = @updateTime,
	EntityWorkFlowId = uwf.ParentId
	FROM dbo.SqliteEntityWorkFlowV2 sewf
	INNER JOIN dbo.EntityWorkFlow ewf on ewf.AgreementId = sewf.AgreementId
	INNER JOIN @updatedWorkFlowId uwf on uwf.ParentId = ewf.Id
	AND uwf.BatchId = sewf.BatchId
END
GO

INSERT into dbo.codeTable
(codeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
('AdvanceRequestStatus', '', 'Pending', 10, 1, 1),
('AdvanceRequestStatus', '', 'Approved', 20, 1, 1),
('AdvanceRequestStatus', '', 'Denied', 30, 1, 1),
('AdvanceRequestStatus', '', 'Partially Approved', 40, 1, 1)
go

CREATE TABLE [dbo].[SqliteTerminateAgreement]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[BatchId] BIGINT NOT NULL REFERENCES dbo.SqliteActionBatch,
	[EmployeeId] BIGINT NOT NULL,

	[EntityId] BIGINT NOT NULL,
	[EntityName] NVARCHAR(50) NOT NULL,
	[AgreementId] BIGINT NOT NULL DEFAULT 0,
	[Agreement] NVARCHAR(50) NOT NULL,

	[RequestDate] DATETIME2 NOT NULL,
	[TypeName] NVARCHAR(50) NOT NULL,
	[Reason] NVARCHAR(50) NOT NULL,
	[Notes] NVARCHAR(512) NOT NULL DEFAULT '',
	[ActivityId] NVARCHAR(50) NOT NULL DEFAULT '',

	[IsProcessed] BIT NOT NULL DEFAULT 0,  
	[TerminateAgreementId] BIGINT NOT NULL DEFAULT 0,
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
)
GO

-- Oct 20 2019
CREATE PROCEDURE [dbo].[ProcessSqliteTerminateAgreementData]
	@batchId BIGINT
AS
BEGIN

	-- if batch is already processed - return	
	IF NOT EXISTS(SELECT 1 FROM dbo.SqliteActionBatch 
			WHERE Id = @batchId AND TerminateRequestsSaved > 0
			AND BatchProcessed = 0)
	BEGIN
		RETURN;
	END

	-- First create required records in Day Table
	INSERT INTO dbo.[Day]
	([DATE])
	SELECT DISTINCT CAST(e.[RequestDate] AS [Date])
	FROM dbo.SqliteTerminateAgreement e
	LEFT JOIN dbo.[Day] d on CAST(e.[RequestDate] AS [Date]) = d.[DATE]
	WHERE e.BatchId = @batchId
	AND d.[DATE] IS NULL

	-- SqliteTerminateAgreement can only be created for approved agreements

	-- select current max Id from dbo.TerminateAgreementRequest
	DECLARE @lastMaxId BIGINT
	SELECT @lastMaxId = ISNULL(MAX(Id),0) FROM dbo.TerminateAgreementRequest

	-- Create New Records
	INSERT INTO dbo.[TerminateAgreementRequest]
	([EmployeeId], [DayId], [EntityId], [EntityAgreementId],
	[RequestDate], [RequestReason], [Status],	[ActivityId],
	[RequestNotes], [SqliteTerminateAgreementId],  CreatedBy, UpdatedBy)
	SELECT 
	sqe.[EmployeeId], d.[Id] AS [DayId], sqe.EntityId, 	sqe.[AgreementId],
	CAST(sqe.[RequestDate] AS [Date]), sqe.Reason, 'Pending', sqa.ActivityId,
	sqe.Notes, sqe.[Id],  'ProcessSqliteTerminateAgreementData', 'ProcessSqliteTerminateAgreementData'

	FROM dbo.SqliteTerminateAgreement sqe
	INNER JOIN dbo.[Day] d ON d.[DATE] = CAST(sqe.[RequestDate] AS [Date])
	-- this join is to get activity Id
	INNER JOIN dbo.SqliteAction sqa on sqa.EmployeeId = sqe.EmployeeId
			AND sqa.[At] = sqe.RequestDate
			AND sqa.PhoneDbId = sqe.ActivityId
	WHERE sqe.BatchId = @batchId AND sqe.IsProcessed = 0
	AND sqe.EntityId > 0 
	AND sqe.AgreementId > 0
	ORDER BY sqe.Id

	-- now we need to update the id in SqliteTerminateAgreement table
	UPDATE dbo.SqliteTerminateAgreement
	SET TerminateAgreementId = e.Id,
	IsProcessed = 1,
	DateUpdated = SYSUTCDATETIME()
	FROM dbo.SqliteTerminateAgreement se
	INNER JOIN dbo.[TerminateAgreementRequest] e on se.Id = e.SqliteTerminateAgreementId
	AND se.BatchId = @batchId
	AND e.Id > @lastMaxId
END
GO

-- Oct 21 2019
CREATE TABLE [dbo].[ItemMasterInput]
(
	[ItemCode] NVARCHAR(50) NOT NULL,
	[ItemDesc] NVARCHAR(100) NOT NULL,
	[Category] NVARCHAR(50) NOT NULL,
	[Unit]     NVARCHAR(10) NOT NULL,
	[Classification] NVARCHAR(10),
	[CropName] NVARCHAR(50)
)
GO
INSERT into dbo.codeTable
(codeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
('ExcelUpload', 'ItemMaster', 'ItemMasterInput', 110, 1, 1)
go

-- Oct 22 2019
ALTER TABLE [dbo].[ItemMaster]
ADD	[TypeName] NVARCHAR(50) NOT NULL DEFAULT ''
GO

CREATE PROCEDURE [dbo].[TransformItemMasterData]
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
		[TypeName] = ani.CropName
		FROM dbo.ItemMaster an
		INNER JOIN dbo.ItemMasterInput ani on an.ItemCode = ani.ItemCode
		and an.TypeName = ani.CropName


		-- Step 2: Insert data
		INSERT INTO dbo.[ItemMaster]
		([ItemCode], [ItemDesc], [Category], [Unit], [Classification], [TypeName])
		SELECT  
		ani.[ItemCode], ani.[ItemDesc], ani.[Category], ani.[Unit], 
		left(ltrim(rtrim(IsNULL(ani.[Classification], ''))), 10),
		ani.[CropName]
		FROM dbo.ItemMasterInput ani
		LEFT JOIN dbo.ItemMaster an on ani.ItemCode = an.ItemCode
		and an.TypeName = ani.CropName
		WHERE an.ItemCode is null

		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformItemMasterData', 'Success'
END
GO

ALTER TABLE [dbo].[TenantSmsType]
ADD	[PlaceHolders] NVARCHAR(1024)
GO

-- Oct 23 2019
delete from dbo.TenantSmsType
where TypeName in 
(
'EntityProfile',
'AgreementApproved',
'InputIssueOne',
'InputIssueMany',
'InputReturnMany',
'PassbookReceived',
'PartAdvanceApproved',
'FullAdvanceApproved',
'AdvanceDenied',
'AgreementClosed',
'AgreementTerminated',
'AgreementSettled'
)
GO


insert into dbo.TenantSmsType
(TenantId, TypeName, SprocName, IsActive, SmsProcessClass, MessageText, PlaceHolders)
values
(1, 'EntityProfile', '', 1, 'ReitzelSms', 'Welcome to {CompanyName} {EntityName}! Your profile has been created.',
'["CompanyName", "EntityName"]'),

(1, 'AgreementApproved', '', 1, 'ReitzelSms', 'Thank you {EntityName}, you have been successfully registered.  Your agreement number for {TypeName} is {AgreementNumber}',
'["EntityName", "TypeName", "AgreementNumber"]'),

(1, 'InputIssueOne', '', 1, 'ReitzelSms', '{EntityName}, you have received {Quantity} {ItemUnit} of {ItemCode} for {AgreementNumber} / {TypeName}.<NL>If not, please contact the field supervisor for clarification.',
'["EntityName", "TypeName", "AgreementNumber", "Quantity", "ItemCode", "ItemUnit"]'),

(1, 'InputIssueMany', '', 1, 'ReitzelSms', '{EntityName}, you have received these items for {AgreementNumber} / {TypeName} : <NL>{ItemDetails}<NL>If not, please contact the field supervisor for clarification.',
'["EntityName", "TypeName", "AgreementNumber", "ItemDetails"]'),

(1, 'InputReturnMany', '', 1, 'ReitzelSms', '{EntityName}, you have returned these items for {AgreementNumber} / {TypeName} : <NL>{ItemDetails}<NL>If not, please contact the field supervisor for clarification.',
'["EntityName", "TypeName", "AgreementNumber", "ItemDetails"]'),

(1, 'PassbookReceived', '', 1, 'ReitzelSms', 'Your passbook for agreement number {AgreementNumber} / {TypeName} has been received, final settlement will be done shortly.',
'["TypeName", "AgreementNumber"]'),

(1, 'PartAdvanceApproved', '', 1, 'ReitzelSms', 'Dear {FieldPersonName}, An advance request of Rs. {AmountRequested} for {EntityName} has been processed. Approved amount is Rs. {AmountApproved}. [{TypeName}-{AgreementNumber}]',
'["FieldPersonName", "AmountRequested", "EntityName", "AmountApproved", "TypeName", "AgreementNumber"]'),

(1, 'FullAdvanceApproved', '', 1, 'ReitzelSms', 'Dear {FieldPersonName}, An advance request of Rs. {AmountRequested} for {EntityName} has been processed and approved. [{TypeName}-{AgreementNumber}]',
'["FieldPersonName", "AmountRequested", "EntityName", "TypeName", "AgreementNumber"]'),

(1, 'AdvanceDenied', '', 1, 'ReitzelSms', 'Dear {FieldPersonName}, An advance request of Rs. {AmountRequested} for {EntityName} has been Denied. [{TypeName}-{AgreementNumber}]',
'["FieldPersonName", "AmountRequested", "EntityName", "TypeName", "AgreementNumber"]'),

(1, 'AgreementClosed', '', 1, 'ReitzelSms', 'Dear {FieldPersonName}, farmer agreement {AgreementNumber} / {TypeName} for {EntityName} has been closed.',
'["FieldPersonName", "EntityName", "TypeName", "AgreementNumber"]'),

(1, 'AgreementTerminated', '', 1, 'ReitzelSms', 'Dear {FieldPersonName}, farmer agreement {AgreementNumber} / {TypeName} for {EntityName} has been terminated.',
'["FieldPersonName", "EntityName", "TypeName", "AgreementNumber"]'),

(1, 'AgreementSettled', '', 1 , 'ReitzelSms', 'Your final settlement for Agreement {AgreementNumber} / {TypeName} has been done.  You may collect your account statement from office soon. Thank you for being a {CompanyName} farmer.',
'["TypeName", "AgreementNumber", "CompanyName"]')

GO

CREATE TABLE [dbo].[TenantSmsData]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[TenantId] BIGINT NOT NULL References dbo.Tenant,
	[TemplateName] NVARCHAR(50) NOT NULL,
	[DataType] NVARCHAR(10) NOT NULL, -- XML or JSON
	[MessageData] NVARCHAR(MAX) NOT NULL,
	[IsSent] BIT NOT NULL DEFAULT 0,
	[IsFailed] BIT NOT NULL DEFAULT 0,
	[FailedText] NVARCHAR(100),
	[LockTimestamp] DATETIME2,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT SysutcDateTime(),
	[Timestamp] DATETIME2 NOT NULL DEFAULT SysutcDateTime()
)
GO

CREATE PROCEDURE [dbo].[GetTenantSmsData]
	@recordCount int,
	@tenantId BIGINT,
	@smsType NVARCHAR(50) -- sms type
AS
BEGIN
    DECLARE @currentDateTime DATETIME2 = SYSUTCDATETIME();

	;WITH batchCTE(tenantSmsId)
	AS
	(
		SELECT TOP(@recordCount) o.Id  
		FROM dbo.[TenantSmsData] o WITH (NOLOCK)
		WHERE o.TenantId = @tenantId
		AND o.TemplateName = @smsType
		AND o.LockTimestamp IS NULL
		AND o.IsSent = 0
		AND o.IsFailed = 0
		ORDER BY o.Id
	)
	-- Lock the selected batch records
	UPDATE dbo.[TenantSmsData]
	SET LockTimestamp = @currentDateTime,
	[Timestamp] = @currentDateTime
	OUTPUT inserted.Id TenantSmsDataId,
	inserted.DataType,
	inserted.MessageData MessageData
	FROM dbo.[TenantSmsData] b
	INNER JOIN batchCTE cte ON b.Id = cte.TenantSmsId
END
GO

ALTER PROCEDURE [dbo].[ProcessSqliteEntityData]
	@batchId BIGINT
AS
BEGIN

	-- if batch is already processed - return	
	IF NOT EXISTS(SELECT 1 FROM dbo.SqliteActionBatch 
			WHERE Id = @batchId AND NumberOfEntitiesSaved > 0
			AND BatchProcessed = 0)
	BEGIN
		RETURN;
	END

	-- First create required records in Day Table
	INSERT INTO dbo.[Day]
	([DATE])
	SELECT DISTINCT CAST(e.[TimeStamp] AS [Date])
	FROM dbo.SqliteEntity e
	LEFT JOIN dbo.[Day] d on CAST(e.[TimeStamp] AS [Date]) = d.[DATE]
	WHERE e.BatchId = @batchId
	AND d.[DATE] IS NULL

	-- Identify duplicate Entity records that have already come in some other batch
	--UPDATE se
	--SET EntityId = e.EntityId,
	--IsProcessed = 1,
	--DateUpdated = SYSUTCDATETIME()
	--FROM dbo.SqliteEntity se
	--INNER JOIN dbo.[SqliteEntity] e on se.[TimeStamp] = e.[TimeStamp]
	--AND e.EmployeeId = se.EmployeeId
	--AND e.PhoneDbId = se.PhoneDbId
	--AND se.BatchId = @batchId

	DECLARE @dupRows BIGINT = @@RowCount
	IF @dupRows > 0
	BEGIN
		UPDATE dbo.SqliteActionBatch
		SET DuplicateEntityCount = @dupRows,
		Timestamp = SYSUTCDATETIME()
		WHERE id = @batchId		
	END

	-- select current max entity Id
	DECLARE @lastMaxEntityId BIGINT
	SELECT @lastMaxEntityId = ISNULL(MAX(Id),0) FROM dbo.Entity

	-- Create Entity Records
	INSERT INTO dbo.[Entity]
	([EmployeeId], [DayId], [HQCode], [AtBusiness], 
	[EntityType], [EntityName], [EntityDate], 
	[Address], [City], [State], [Pincode], [LandSize], 
	[Latitude], [Longitude],
	[UniqueIdType], [UniqueId], [TaxId],
	[SqliteEntityId], [ContactCount], [CropCount], [ImageCount])

	SELECT sqe.[EmployeeId], d.[Id] AS [DayId], sp.[HQCode], sqe.[AtBusiness], 
	sqe.[EntityType], sqe.[EntityName], sqe.[TimeStamp] AS [EntityDate], 
	sqe.[Address], sqe.[City], sqe.[State], sqe.[Pincode], sqe.[LandSize], 
	sqe.[DerivedLatitude], sqe.[DerivedLongitude], 
	sqe.[UniqueIdType], sqe.[UniqueId], sqe.[TaxId],
	sqe.[Id], sqe.[ContactCount], sqe.[CropCount], sqe.[ImageCount]

	FROM dbo.SqliteEntity sqe
	INNER JOIN dbo.[Day] d ON d.[DATE] = CAST(sqe.[TimeStamp] AS [Date])
	INNER JOIN dbo.TenantEmployee te on te.Id = sqe.EmployeeId
	INNER JOIN dbo.SalesPerson sp on te.EmployeeCode = sp.StaffCode
	WHERE sqe.BatchId = @batchId AND sqe.IsProcessed = 0
	ORDER BY sqe.Id

	-- Now Images
	UPDATE dbo.[Image] SET SourceId = 0 WHERE SourceId > 0

	INSERT INTO dbo.[Image]
	(SourceId, [ImageFileName])  
	SELECT sei.Id, sei.[ImageFileName]
	FROM dbo.SqliteEntityImage sei
	INNER JOIN dbo.SqliteEntity se on sei.SqliteEntityId = se.Id
	AND se.BatchId = @batchId
	AND se.IsProcessed = 0
		
	-- Now create entries in EntityImage
	INSERT INTO dbo.EntityImage
	(EntityId, ImageId, SequenceNumber)
	SELECT e.Id, i.[Id], sei.SequenceNumber
	FROM dbo.SqliteEntityImage sei
	INNER JOIN dbo.[Image] i on sei.Id = i.SourceId
	INNER JOIN dbo.SqliteEntity sle on sei.SqliteEntityId = sle.Id
	AND sle.BatchId = @batchId
	INNER JOIN dbo.[Entity] e on sle.Id = e.SqliteEntityId

	-- Prepare for SMS
	DECLARE @NewProfile TABLE
	( EntityId BIGINT,
	  EntityName NVARCHAR(50),
	  PhoneNumber NVARCHAR(20)
	)

	-- now we need to update the id in SqliteEntity table
	UPDATE dbo.SqliteEntity
	SET EntityId = e.Id,
	IsProcessed = 1,
	DateUpdated = SYSUTCDATETIME()
	OUTPUT inserted.EntityId, '', '' INTO @NewProfile
	FROM dbo.SqliteEntity se
	INNER JOIN dbo.[Entity] e on se.Id = e.SqliteEntityId
	AND se.BatchId = @batchId
	AND e.Id > @lastMaxEntityId


	--Create Entity Contacts
	INSERT INTO dbo.[EntityContact]
	([EntityId], [Name], [PhoneNumber], [IsPrimary])
	SELECT se.[EntityId], sqecn.[Name], sqecn.[PhoneNumber], sqecn.[IsPrimary]
	FROM dbo.SqliteEntityContact sqecn
	INNER JOIN dbo.SqliteEntity se on se.Id = sqecn.SqliteEntityId
	AND se.BatchId = @batchId
	AND se.EntityId > @lastMaxEntityId

	--Create Entity Crops
	INSERT INTO dbo.[EntityCrop]
	([EntityId], [CropName])
	SELECT se.[EntityId], sqecr.[Name] AS [CropName]
	FROM dbo.SqliteEntityCrop sqecr
	INNER JOIN dbo.SqliteEntity se on se.Id = sqecr.SqliteEntityId
	AND se.BatchId = @batchId
	AND se.EntityId > @lastMaxEntityId

	-- retrieve tenant Id for batch
	DECLARE @tenantId BIGINT
	SELECT @tenantId = TenantId
	FROM dbo.SqliteActionBatch b
	INNER JOIN dbo.TenantEmployee te on b.EmployeeId = te.Id
	WHERE b.Id = @batchId

	-- PUT Name and phone number in new records where sms is to be sent
	UPDATE @NewProfile
	SET EntityName = ec.Name,
	PhoneNumber = ec.PhoneNumber
	FROM @NewProfile np
	INNER JOIN dbo.EntityContact ec on np.EntityId = ec.EntityId
	and ec.IsPrimary = 1

	-- Insert into Table for SMS
	INSERT INTO dbo.TenantSmsData
	(TenantId, TemplateName, DataType, MessageData)
	SELECT @tenantId, 'EntityProfile', 'XML', 
	(SELECT * FROM @NewProfile i WHERE i.EntityId = o.EntityId FOR XML PATH('Row'))
	FROM @NewProfile o
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

	-- Create Input/Issue Records
	INSERT INTO dbo.[IssueReturn]
	([EmployeeId], [DayId], [EntityId], 
	[EntityAgreementId], 
	[ItemMasterId],
	[TransactionDate], [TransactionType], [Quantity], [ActivityId],
	[SqliteIssueReturnId])
	OUTPUT INSERTED.EntityId, '', '', inserted.EntityAgreementId, '', '', inserted.Quantity,
	inserted.ItemMasterId, '', '', inserted.TransactionType INTO @NewItems
	SELECT 
	sqe.[EmployeeId], d.[Id] AS [DayId], sqe.EntityId, 
	CASE WHEN sqe.[AgreementId] = 0 THEN NULL ELSE sqe.[AgreementId] END,
	sqe.[ItemId],
	CAST(sqe.[IssueReturnDate] AS [Date]), sqe.[TranType], sqe.[Quantity], sqa.ActivityId,
	sqe.[Id]

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

-- Nov 6 2019
insert into dbo.CodeTable
(CodeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
( 'CropType', 'Babycorn', 'Babycorn', 20, 1, 1),
( 'CropType', 'Jalapeno pepper', 'Jalapeno pepper', 30, 1, 1),
( 'CropType', 'Banana pepper', 'Banana pepper', 40, 1, 1),
( 'CropType', 'Piri Piri chilli-Green', 'Piri Piri chilli-Green', 50, 1, 1),
( 'CropType', 'Piri Piri chilli-Red', 'Piri Piri chilli-Red', 60, 1, 1)
go

DELETE from dbo.codeTable
where codeType = 'AgreementStatus'
and codeValue = 'No Agreement'
and IsActive = 1
and TenantId = 1
GO
