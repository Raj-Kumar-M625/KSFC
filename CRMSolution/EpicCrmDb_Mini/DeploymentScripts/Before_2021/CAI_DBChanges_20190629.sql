-- Taken from
--
-- C:\EpicCrm\CRMSolution\EpicCrmDb_Mini\DeploymentScripts\DBChanges_20190402.sql
-- C:\EpicCrm\CRMSolution\EpicCrmDb_Mini\DeploymentScripts\DBChanges_20190513.sql
-- C:\EpicCrm\CRMSolution\EpicCrmDb_Mini\DeploymentScripts\DBChanges_20190514.sql
-- C:\EpicCrm\CRMSolution\EpicCrmDb_Mini\DeploymentScripts\DBChanges_20190522.sql
-- C:\EpicCrm\CRMSolution\EpicCrmDb_Mini\DeploymentScripts\DBChanges_20190617.sql

Alter table dbo.Entity
ADD [AgreementCount] INT NOT NULL DEFAULT 0
go

CREATE TABLE [dbo].[WorkflowSeason]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[SeasonName] NVARCHAR(50) NOT NULL,
	[TypeName] NVARCHAR(50) NOT NULL, -- could be crop name
	[StartDate] DATE NOT NULL,
	[EndDate] DATE NOT NULL,
	[IsOpen] BIT NOT NULL DEFAULT 1,
	[ReferenceId] NVARCHAR(50) NULL, -- not used now
	[Description] NVARCHAR(128) NOT NULL DEFAULT '',
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
)
GO

CREATE TABLE [dbo].[EntityAgreement]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[EntityId] BIGINT NOT NULL REFERENCES [Entity]([Id]), 
	[WorkflowSeasonId]  BIGINT NOT NULL REFERENCES [WorkFlowSeason]([Id]), 
	[AgreementNumber] NVARCHAR(50) NOT NULL,
	[Status] NVARCHAR(10) NOT NULL DEFAULT 'Pending',

	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[CreatedBy] NVARCHAR(50) NOT NULL,
	[UpdatedBy] NVARCHAR(50) NOT NULL
)
GO

--INSERT INTO dbo.WorkflowSeason
--(SeasonName, TypeName, StartDate, EndDate, IsOpen)
--values
--('Gherkin_0419', 'Gherkin', '2019-04-02', '2099-12-31', 1)
--go

INSERT INTO dbo.CodeTable
(CodeType, CodeName, CodeValue, DisplaySequence, IsActive)
values
('AgreementStatus', '', 'Pending', 10, 1),
('AgreementStatus', '', 'Approved', 20, 1),
('AgreementStatus', '', 'Terminated', 30, 1)

go

CREATE TABLE [dbo].[ItemMaster]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[ItemCode] NVARCHAR(100) NOT NULL,
	[ItemDesc] NVARCHAR(100) NOT NULL,
	[Category] NVARCHAR(50) NOT NULL,
	[Unit]     NVARCHAR(10) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT 1
)
GO

-- table modified on 24.4.19 - added entityId
CREATE TABLE [dbo].[IssueReturn]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[EmployeeId] BIGINT NOT NULL REFERENCES TenantEmployee,
	[DayId] BIGINT NOT NULL REFERENCES dbo.[Day],
	[EntityId] BIGINT NOT NULL REFERENCES dbo.Entity,
	[EntityAgreementId] BIGINT NOT NULL DEFAULT 0, 
	[ItemMasterId] BIGINT NOT NULL REFERENCES dbo.ItemMaster,
	[TransactionDate] DATE NOT NULL,
	[TransactionType] NVARCHAR(50) NOT NULL, -- Issue/Return/Abandoned
	[Quantity] INT NOT NULL,
	[ActivityId] BIGINT NOT NULL,
	[SqliteIssueReturnId] BIGINT NOT NULL DEFAULT 0,
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
)
GO

INSERT INTO dbo.CodeTable
(CodeType, CodeName, CodeValue, DisplaySequence, IsActive)
values
('TransactionType', '', 'Issue', 10, 1),
('TransactionType', '', 'Return', 20, 1),
('TransactionType', '', 'Abandoned', 30, 1)
go

INSERT INTO dbo.CodeTable
(CodeType, CodeName, CodeValue, DisplaySequence, IsActive)
values
('ItemType', '', 'Fertilizers', 10, 1),
('ItemType', '', 'Pesticides', 20, 1),
('ItemType', '', 'Seeds', 30, 1),
('ItemType', '', 'Others', 40, 1)
go


-- April 5 2019
ALTER TABLE [dbo].[SqliteActionBatch]
ADD
	[NumberOfIssueReturns] BIGINT NOT NULL DEFAULT 0,
	[NumberOfIssueReturnsSaved] BIGINT NOT NULL DEFAULT 0
GO

CREATE TABLE [dbo].[SqliteIssueReturn]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[BatchId] BIGINT NOT NULL REFERENCES dbo.SqliteActionBatch,
	[EmployeeId] BIGINT NOT NULL,

	[EntityId] BIGINT NOT NULL,
	[EntityName] NVARCHAR(50) NOT NULL,
	[AgreementId] BIGINT NOT NULL DEFAULT 0,
	[Agreement] NVARCHAR(50) NOT NULL,
	[TranType] NVARCHAR(50) NOT NULL,
	[ItemId] BIGINT NOT NULL,
	[ItemCode] NVARCHAR(100) NOT NULL,
	[Quantity] INT NOT NULL,
	[IssueReturnDate] DATETIME2 NOT NULL,

	[IsProcessed] BIT NOT NULL DEFAULT 0,  
	[IssueReturnId] BIGINT NOT NULL DEFAULT 0,
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
)
GO


ALTER TABLE [dbo].[FeatureControl]
ADD	[IssueReturnFeature] BIT NOT NULL DEFAULT 0
GO

ALTER TABLE [dbo].[EmployeeMaster]
ALTER COLUMN [Staff Code] [nvarchar](255) NULL
GO

-- April 11 2019
ALTER TABLE [dbo].[WorkFlowSchedule]
ADD	[PhoneDataEntryPage] NVARCHAR(50) NOT NULL DEFAULT ''
GO

UPDATE dbo.WorkFlowSchedule
SET PhoneDataEntryPage = 'SowingWorkflowEntryPage'
WHERE PHASE = 'Sowing Confirmation'

UPDATE dbo.WorkFlowSchedule
SET PhoneDataEntryPage = 'CommonWorkflowEntryPage'
WHERE PHASE = 'Germination'

UPDATE dbo.WorkFlowSchedule
SET PhoneDataEntryPage = 'CommonWorkflowEntryPage'
WHERE PHASE = 'Weeding'

UPDATE dbo.WorkFlowSchedule
SET PhoneDataEntryPage = 'CommonWorkflowEntryPage'
WHERE PHASE = 'Staking'

UPDATE dbo.WorkFlowSchedule
SET PhoneDataEntryPage = 'FirstHarvestWorkflowEntryPage'
WHERE PHASE = 'First Harvest'


ALTER TABLE [dbo].[ItemMaster]
ADD	[Classification] NVARCHAR(10) NOT NULL DEFAULT ''
GO

UPDATE dbo.WorkFlowSchedule
SET Phase = 'Sowing'
WHERE Phase = 'Sowing Confirmation'
GO

-- April 18 2019
-- April 5 2019
ALTER TABLE [dbo].[SqliteActionBatch]
ADD
	[NumberOfWorkFlow] BIGINT NOT NULL DEFAULT 0,
	[NumberOfWorkFlowSaved] BIGINT NOT NULL DEFAULT 0
GO

CREATE TABLE [dbo].[SqliteEntityWorkFlowV2]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
	[BatchId] BIGINT NOT NULL References dbo.SqliteActionBatch,
    [EmployeeId] BIGINT NOT NULL,
	[EntityId] BIGINT NOT NULL DEFAULT 0,
	[EntityType] NVARCHAR(50) NOT NULL, 
	[EntityName] NVARCHAR(50) NOT NULL, 
	[AgreementId] BIGINT NOT NULL DEFAULT 0,
	[Agreement] NVARCHAR(50) NOT NULL,

	[Phase] NVARCHAR(50) NOT NULL,
	[IsStarted] BIT NOT NULL DEFAULT 0,
	[Date] DATE NOT NULL,
	[MaterialType] NVARCHAR(50),
	[MaterialQuantity] INT NOT NULL DEFAULT 0,
	[GapFillingRequired] BIT NOT NULL DEFAULT 0,
	[GapFillingSeedQuantity] INT NOT NULL DEFAULT 0,
	[LaborCount] INT NOT NULL DEFAULT 0,
	[PercentCompleted] INT NOT NULL DEFAULT 0,

	[IsProcessed] BIT NOT NULL DEFAULT 0,
	[EntityWorkFlowId] BIGINT NOT NULL DEFAULT 0,
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[Timestamp] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
)
GO

-- 19.4.2019
ALTER TABLE [dbo].[EntityWorkFlow]
ADD 
	[AgreementId] BIGINT NOT NULL DEFAULT 0,
	[Agreement] NVARCHAR(50) NOT NULL DEFAULT ''
GO

ALTER TABLE [dbo].[EntityWorkFlowDetail]
ADD
	[IsStarted] BIT NOT NULL DEFAULT 0,
	[WorkFlowDate] DATE,
	[MaterialType] NVARCHAR(50),
	[MaterialQuantity] INT NOT NULL DEFAULT 0,
	[GapFillingRequired] BIT NOT NULL DEFAULT 0,
	[GapFillingSeedQuantity] INT NOT NULL DEFAULT 0,
	[LaborCount] INT NOT NULL DEFAULT 0,
	[PercentCompleted] INT NOT NULL DEFAULT 0,
	[BatchId] BIGINT NOT NULL DEFAULT 0
GO


-- this is todo in database
--ALTER TABLE [dbo].[Entity]
--DROP COLUMN 
--    --For dealers who have been added as customers and once approved need not be shown in entity
--	[IsApproved],
--	[ApproveDate],
--	[ApproveRef],
--	[ApproveNotes],
--	[ApprovedBy]
--GO

ALTER TABLE [dbo].[Entity]
ADD [UpdatedBy] NVARCHAR(50) NOT NULL DEFAULT ''
GO

CREATE UNIQUE INDEX IX_EntityAgreement_AgreementNumber
ON dbo.EntityAgreement (AgreementNumber)
WHERE AgreementNumber <> ''
GO

CREATE UNIQUE INDEX IX_EntityAgreement_EntityId
ON dbo.EntityAgreement (EntityId)
GO

-- 20.4.2019
ALTER TABLE [dbo].[SqliteEntityWorkFlowV2]
ADD
	[FieldVisitDate] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
GO


ALTER TABLE [dbo].[SqliteIssueReturn]
ADD	[ActivityId] NVARCHAR(50) NOT NULL DEFAULT ''
GO

ALTER TABLE [dbo].[SqliteEntityWorkFlowV2]
ADD	[ActivityId] NVARCHAR(50) NOT NULL DEFAULT ''
GO

CREATE PROCEDURE [dbo].[ProcessSqliteEntityWorkFlowDataV2]
	@batchId BIGINT
AS
BEGIN
   /*
    Conditions taken care of:
	a) Can't have more than one open work flow for an entity
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
	UPDATE dbo.SqliteEntityWorkFlowV2
	SET EntityId = e.Id
	FROM dbo.SqliteEntityWorkFlowV2 sewf
	INNER JOIN dbo.Entity e on sewf.EntityType = e.EntityType
	AND sewf.EntityName = e.EntityName
	AND sewf.EntityId = 0
	AND sewf.BatchId = @batchId
	
	-- For one entity, we will process only one row
	-- so refresh @sqliteEntityWorkFlow by removing duplicates
	-- Also
	-- Now in @sqliteEntityWorkFlow table
	-- fill EmployeeId, EmployeeCode and Date
	-- this is done so that we don't have to join the tables two times
	-- in following queries.
	;WITH singleRecCTE(Id, rownum)
	AS
	(
		SELECT sewf.Id,
		ROW_NUMBER() Over (Partition By sewf.EntityId ORDER BY sewf.Id)
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


	-- Create a in memory table of work flow schedule
	-- this will have an additional column indicating previous phase
	-- Work flow schedule is a small table with say 5 to 10 rows.
	DECLARE @WorkFlowSchedule TABLE
	(
		[Sequence] INT NOT NULL,
		[Phase] NVARCHAR(50) NOT NULL,
		[TargetStartAtDay] INT NOT NULL,
		[TargetEndAtDay] INT NOT NULL,
		[PrevPhase] NVARCHAR(50) NOT NULL
	)

	;with schCTE([Sequence], Phase, TargetStartAtDay, TargetEndAtDay, rownum)
	AS
	(
		SELECT [Sequence], Phase,  TargetStartAtDay, TargetEndAtDay,
		ROW_NUMBER() OVER (Order By [Sequence])
		FROM dbo.[WorkFlowSchedule]
		WHERE IsActive = 1
	)
	INSERT INTO @WorkFlowSchedule
	([Sequence], [Phase], [TargetStartAtDay], [TargetEndAtDay], [PrevPhase])
	SELECT [Sequence], Phase,
	TargetStartAtDay, TargetEndAtDay,
	ISNULL((SELECT Phase FROM schCTE WHERE rownum = p.rownum-1), '') PrevPhase
	FROM schCTE p


	-- Select first step in workflow
	DECLARE @firstStep NVARCHAR(50)
	SELECT TOP 1 @firstStep = Phase
	FROM @WorkFlowSchedule
	ORDER BY [Sequence]

	-- INSERT NEW Rows in EntityWorkFlow 
	INSERT into dbo.EntityWorkFlow
	(EmployeeId, [EmployeeCode], HQCode, EntityId, EntityName, CurrentPhase, [CurrentPhaseStartDate],
	[CurrentPhaseEndDate], [InitiationDate], [IsComplete],
	[AgreementId], [Agreement])
	SELECT mem.EmployeeId, mem.EmployeeCode, mem.HQCode, sewf.EntityId, sewf.EntityName, '', '2000-01-01',
	'2000-01-01', mem.[Date], 0, 
	sewf.AgreementId, sewf.Agreement
	FROM dbo.SqliteEntityWorkFlowV2 sewf
	INNER JOIN @sqliteEntityWorkFlow mem ON mem.ID = sewf.Id
	WHERE sewf.IsProcessed = 0
	AND sewf.Phase = @firstStep
	-- there can be only one open work flow for an entity
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
	INNER JOIN @WorkFlowSchedule sch ON 1 = 1
	AND wf.CurrentPhase = ''


	DECLARE @updateTime DATETIME2 = SYSUTCDATETIME()

	DECLARE @updatedWorkFlowId TABLE 
	( Id BIGINT, ParentId BIGINT, PhaseDate DATE, Phase NVARCHAR(50), BatchId BIGINT )

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

DROP INDEX [IX_SqliteAction_EmpIdActionName] ON dbo.SqliteAction
GO

CREATE /*UNIQUE*/ INDEX [IX_SqliteAction_EmpIdActionName]
	ON [dbo].[SqliteAction]
	(EmployeeId, [AT], ActivityTrackingType)
GO

CREATE TABLE [dbo].[ActivityType]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[ActivityName] NVARCHAR(50) NOT NULL, 
	[DateCreated] [DATETIME2] NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] [DateTime2] NOT NULL DEFAULT SYSUTCDATETIME()
)
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
	@activityId BIGINT OUTPUT
AS
BEGIN
   -- Records tracking activity
   SET @activityId = 0
	-- check if a record already exist in Employee Day table
	IF EXISTS(SELECT 1 FROM dbo.EmployeeDay WHERE Id = @employeeDayId)
	BEGIN
		INSERT into dbo.Activity
		(EmployeeDayId, ClientName, ClientPhone, ClientType, ActivityType, Comments, ImageCount, [At], ClientCode, ActivityAmount, AtBusiness, ContactCount)
		VALUES
		(@employeeDayId, @clientName, @clientPhone, @clientType, @activityType, @comments, @imageCount, @activityDateTime, @clientCode, @activityAmount, @atBusiness, @contactCount)

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

-- 
-- table modified on 24.4.19 - added entityId
drop table dbo.IssueReturn
go
CREATE TABLE [dbo].[IssueReturn]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[EmployeeId] BIGINT NOT NULL REFERENCES TenantEmployee,
	[DayId] BIGINT NOT NULL REFERENCES dbo.[Day],
	[EntityId] BIGINT NOT NULL REFERENCES dbo.Entity,
	[EntityAgreementId] BIGINT NULL REFERENCES dbo.EntityAgreement,
	[ItemMasterId] BIGINT NOT NULL REFERENCES dbo.ItemMaster,
	[TransactionDate] DATE NOT NULL,
	[TransactionType] NVARCHAR(50) NOT NULL, -- Issue/Return/Abandoned
	[Quantity] INT NOT NULL,
	[ActivityId] BIGINT NOT NULL,
	[SqliteIssueReturnId] BIGINT NOT NULL DEFAULT 0,
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
)
GO

ALTER TABLE [dbo].[SqliteIssueReturn]
ADD	[SqliteEntityId]  NVARCHAR(50) NOT NULL DEFAULT ''
GO

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

	-- First create required records in Day Table
	INSERT INTO dbo.[Day]
	([DATE])
	SELECT DISTINCT CAST(e.[IssueReturnDate] AS [Date])
	FROM dbo.SqliteIssueReturn e
	LEFT JOIN dbo.[Day] d on CAST(e.[IssueReturnDate] AS [Date]) = d.[DATE]
	WHERE e.BatchId = @batchId
	AND d.[DATE] IS NULL

	-- SqliteIssueReturn can have issues/returns for entities, that were
	-- created on phone and not uploaded.  For such records, we have to first
	-- fill in entity Id
	UPDATE dbo.SqliteIssueReturn
	SET EntityId = se.EntityId
	FROM dbo.SqliteIssueReturn sqe
	INNER JOIN dbo.SqliteEntity se on sqe.SqliteEntityId = se.PhoneDbId
	AND se.BatchId <= @batchId -- entity has to come in same batch or before
	AND sqe.EntityId = 0
	AND se.EntityId > 0
	AND se.IsProcessed = 1

	-- select current max entity Id
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

-- 25.4.2019
ALTER TABLE [dbo].[FeatureControl]
ADD	
	[FieldActivityReportFeature] BIT NOT NULL DEFAULT 0,
	[EntityProgressReportFeature] BIT NOT NULL DEFAULT 0,
	[AbsenteeReportFeature] BIT NOT NULL DEFAULT 0,
	[AppSignUpReportFeature] BIT  NOT NULL DEFAULT 0,
	[AppSignInReportFeature] BIT  NOT NULL DEFAULT 0,
	[ActivityReportFeature] BIT  NOT NULL DEFAULT 0,
	[ActivityByTypeReportFeature] BIT  NOT NULL DEFAULT 0,
	[MAPFeature] BIT  NOT NULL DEFAULT 0
GO

-- 28.4.2019
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
		print 'EntityWorkFlowDetail'
		DELETE FROM dbo.EntityWorkFlowDetail
		WHERE ID IN (SELECT d.ID FROM dbo.EntityWorkFlowDetail d
		             INNER JOIN dbo.EntityWorkFlow p on d.EntityWorkFlowId = p.Id
					 AND p.EmployeeId = @employeeId)

		print 'EntityWorkFlow'
		DELETE FROM dbo.EntityWorkFlow WHERE EmployeeId = @employeeId

		print 'Issue/Return'
		DELETE FROM dbo.[IssueReturn] WHERE EmployeeId = @employeeId


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

		-- DELETE IMAGES
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


-- C:\EpicCrm\CRMSolution\EpicCrmDb_Mini\DeploymentScripts\DBChanges_20190513.sql
ALTER TABLE [dbo].[FeatureControl]
ADD	
	[AttendanceSummaryReportFeature] BIT NOT NULL DEFAULT 0
GO

-- C:\EpicCrm\CRMSolution\EpicCrmDb_Mini\DeploymentScripts\DBChanges_20190514.sql
-- 14.5.2019

INSERT INTO dbo.TenantWorkDay
(TenantId, WeekDayName, IsWorkingDay)
values
(1, 'Monday', 1),
(1, 'Tuesday', 1),
(1, 'Wednesday', 1),
(1, 'Thursday', 1),
(1, 'Friday', 1),
(1, 'Saturday', 1),
(1, 'Sunday', 1)
go

insert into dbo.TenantSmsType
(TenantId, [TypeName], MessageText, SprocName, IsActive, SmsProcessClass)
values
(1, 'Start Day', 'If you are on duty, please Start Day on mobile app or else will be considered Absent.', 'SMS_GetStartDayStaffCode', 1, 'StartEndDaySms'),
(1, 'End Day', 'If you are not travelling on duty, please "End Day" on mobile app.', 'SMS_GetEndDayStaffCode', 1, 'StartEndDaySms')
;

DECLARE @smsTypeForStartDay BIGINT
SELECT @smsTypeForStartDay = id FROM dbo.TenantSMSType where [TypeName] = 'Start Day';

DECLARE @smsTypeForEndDay BIGINT
SELECT @smsTypeForEndDay = id FROM dbo.TenantSMSType where [TypeName] = 'End Day';

INSERT INTO dbo.TenantSmsSchedule
(TenantId, TenantSmsTypeId, WeekDayName, SmsAt, IsActive)
values
(1, @smsTypeForStartDay, 'Monday', '10:00', 1),
(1, @smsTypeForStartDay, 'Tuesday', '10:00', 1),
(1, @smsTypeForStartDay, 'Wednesday', '10:00', 1),
(1, @smsTypeForStartDay, 'Thursday', '10:00', 1),
(1, @smsTypeForStartDay, 'Friday', '10:00', 1),
(1, @smsTypeForStartDay, 'Saturday', '10:00', 1),
(1, @smsTypeForStartDay, 'Sunday', '10:00', 1),

(1, @smsTypeForEndDay, 'Monday', '20:30', 1),
(1, @smsTypeForEndDay, 'Tuesday', '20:30', 1),
(1, @smsTypeForEndDay, 'Wednesday', '20:30', 1),
(1, @smsTypeForEndDay, 'Thursday', '20:30', 1),
(1, @smsTypeForEndDay, 'Friday', '20:30', 1),
(1, @smsTypeForEndDay, 'Saturday', '20:30', 1),
(1, @smsTypeForEndDay, 'Sunday', '20:30', 1),

(1, @smsTypeForEndDay, 'Monday', '22:00', 1),
(1, @smsTypeForEndDay, 'Tuesday', '22:00', 1),
(1, @smsTypeForEndDay, 'Wednesday', '22:00', 1),
(1, @smsTypeForEndDay, 'Thursday', '22:00', 1),
(1, @smsTypeForEndDay, 'Friday', '22:00', 1),
(1, @smsTypeForEndDay, 'Saturday', '22:00', 1),
(1, @smsTypeForEndDay, 'Sunday', '22:00', 1)
GO

ALTER PROCEDURE [dbo].[SMS_GetStartDayStaffCode]
	@tenantId BIGINT,
	@runDate DATE
AS
BEGIN
	-- first create a row in [Day] table, if not already there
	IF NOT EXISTS( SELECT 1 FROM dbo.[Day] WHERE [Date] = @runDate )
	BEGIN
		INSERT INTO dbo.[Day] ([DATE])
		SELECT @runDate
	END

	-- get the list of staff codes who did not start their day
	SELECT te.EmployeeCode
	FROM dbo.TenantEmployee te
	INNER JOIN dbo.SalesPerson sp ON te.EmployeeCode = sp.StaffCode
	      AND te.IsActive = 1 
	      AND sp.IsActive = 1
		  AND te.TenantId = @tenantId
	INNER JOIN dbo.[Day] d ON d.[Date] = @runDate
	LEFT JOIN dbo.EmployeeDay ed ON te.Id = ed.TenantEmployeeId
	      AND ed.DayId = d.Id
		  AND ed.AppVersion <> '***'

	-- if I continued through mid night, and created activity after midnight
	-- I get an entry in employee day with app version = '***'
	-- this should not be treated as if I have started my day

	WHERE ed.TenantEmployeeId is null
END
GO

ALTER PROCEDURE [dbo].[SMS_GetEndDayStaffCode]
	@tenantId BIGINT,
	@runDate DATE
AS
BEGIN
	-- first create a row in [Day] table, if not already there
	IF NOT EXISTS( SELECT 1 FROM dbo.[Day] WHERE [Date] = @runDate )
	BEGIN
		INSERT INTO dbo.[Day] ([DATE])
		SELECT @runDate
	END

	-- get the list of staff codes who started the day but did not end it
	SELECT te.EmployeeCode
	FROM dbo.TenantEmployee te
	INNER JOIN dbo.SalesPerson sp ON te.EmployeeCode = sp.StaffCode
	      AND te.IsActive = 1 
	      AND sp.IsActive = 1
		  AND te.TenantId = @tenantId
	INNER JOIN dbo.[Day] d ON d.[Date] = @runDate
	INNER JOIN dbo.EmployeeDay ed ON te.Id = ed.TenantEmployeeId
	      AND ed.DayId = d.Id
		  AND ed.EndTime IS NULL
END
GO

-- C:\EpicCrm\CRMSolution\EpicCrmDb_Mini\DeploymentScripts\DBChanges_20190522.sql
-- 22.5.2019

ALTER TABLE dbo.Entity
ADD IsActive BIT NOT NULL DEFAULT 1



---=======================================

-- C:\EpicCrm\CRMSolution\EpicCrmDb_Mini\DeploymentScripts\DBChanges_20190617.sql

-- 17.06.2019

ALTER TABLE [dbo].[FeatureControl]
ADD
	[EmployeeExpenseReport] BIT NOT NULL DEFAULT 0,
	[DistanceReport] BIT NOT NULL DEFAULT 0

