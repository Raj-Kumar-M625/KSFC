CREATE TABLE [dbo].[WorkFlowSchedule]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[Sequence] INT NOT NULL,
	[Phase] NVARCHAR(50) NOT NULL,
	[TargetStartAtDay] INT NOT NULL,
	[TargetEndAtDay] INT NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT 1
)
go

CREATE TABLE [dbo].[EntityWorkFlow]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[EmployeeId] BIGINT NOT NULL REFERENCES TenantEmployee(Id),
	[EmployeeCode] NVARCHAR(10) NOT NULL,
	[HQCode] NVARCHAR(10) NULL,
	[EntityId] BIGINT NOT NULL REFERENCES dbo.Entity,
	[EntityName] NVARCHAR(50) NOT NULL,
	[CurrentPhase] NVARCHAR(50) NOT NULL,
	[CurrentPhaseStartDate] DATE NOT NULL,
	[CurrentPhaseEndDate] DATE NOT NULL,
	[InitiationDate] DATE NOT NULL,
	[IsComplete] BIT NOT NULL DEFAULT 0,

	[DateCreated] DATETIME2 NOT NULL DEFAULT SysutcDATETIME(),
	[Timestamp]  DATETIME2 NOT NULL DEFAULT SysutcDATETIME(),
)
go

CREATE TABLE [dbo].[EntityWorkFlowDetail]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[EntityWorkFlowId] BIGINT NOT NULL REFERENCES dbo.EntityWorkFlow,

	[Sequence] INT NOT NULL,
	[Phase] NVARCHAR(50) NOT NULL,
	[PlannedStartDate] DATE NOT NULL,
	[PlannedEndDate] DATE NOT NULL,
	[PrevPhase] NVARCHAR(50) NOT NULL,

	[ActivityId] BIGINT NOT NULL DEFAULT 0,
	[IsComplete] BIT NOT NULL DEFAULT 0,
	[ActualDate] DATE NULL,
	[PrevPhaseActualDate] DATE NULL,
	[PhaseCompleteStatus] NVARCHAR(20) NULL,

	[EmployeeId] BIGINT NULL,
	[DateCreated] DATETIME2 NOT NULL DEFAULT SysutcDATETIME(),
	[Timestamp]  DATETIME2 NOT NULL DEFAULT SysutcDATETIME(),
)
go

CREATE TABLE [dbo].[SqliteEntityWorkFlow]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
	[ActivityId] BIGINT NOT NULL REFERENCES dbo.Activity,
	[EntityId] BIGINT NOT NULL DEFAULT 0,
	[EntityType] NVARCHAR(50) NOT NULL, 
	[EntityName] NVARCHAR(50) NOT NULL, 
	[Phase] NVARCHAR(50) NOT NULL,
	[Date] DATE NOT NULL,
	[IsProcessed] BIT NOT NULL DEFAULT 0,
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[Timestamp] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
)
go

-- Add Data

INSERT INTO dbo.WorkFlowSchedule
([Sequence], [Phase], [TargetStartAtDay], [TargetEndAtDay])
values
(10, 'Sowing Confirmation', 0, 0),
(20, 'Germination', 1, 15),
(30, 'Weeding', 15, 30),
(40, 'Staking', 1, 30),
(50, 'First Harvest', 30, 40)
go


CREATE PROCEDURE [dbo].[ProcessSqliteEntityWorkFlowData]
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
	 IsDup BIT,
	 EmployeeId BIGINT,
	 EmployeeCode NVARCHAR(10),
	 [HQCode] NVARCHAR(10),
	 [Date] DATE
	)

	-- rows are continuously being added to dbo.SqliteEntityWorkFlow 
	-- on other thread as batches are coming in.
	-- We need to take a handle on the ids that we are ready to process.
	INSERT INTO @sqliteEntityWorkFlow 
	(ID)
	SELECT Id FROM dbo.SqliteEntityWorkFlow 
	WHERE IsProcessed = 0

	-- if there are no unprocessed entries - return
	IF NOT EXISTS(SELECT 1 FROM @sqliteEntityWorkFlow)
	BEGIN
		RETURN;
	END

	-- fill Entity Id - if zero
	UPDATE dbo.SqliteEntityWorkFlow
	SET EntityId = e.Id
	FROM dbo.SqliteEntityWorkFlow sewf
	INNER JOIN @sqliteEntityWorkFlow mem ON mem.ID = sewf.Id
	INNER JOIN dbo.Entity e on sewf.EntityType = e.EntityType
	AND sewf.EntityName = e.EntityName
	AND sewf.EntityId = 0
	
	-- For one entity, we will process only one row
	-- so refresh @sqliteEntityWorkFlow by removing duplicates
	;WITH singleRecCTE(Id, rownum)
	AS
	(
		SELECT mem.Id,
		ROW_NUMBER() Over (Partition By sewf.EntityId ORDER BY sewf.ActivityId)
		FROM dbo.SqliteEntityWorkFlow sewf
		INNER JOIN @sqliteEntityWorkFlow mem ON sewf.Id = mem.ID
	)
	UPDATE @sqliteEntityWorkFlow
	SET IsDup = 1
	FROM @sqliteEntityWorkFlow mem1
	INNER JOIN singleRecCTE cte on mem1.Id = cte.Id
	AND cte.rownum > 1

	DELETE FROM @sqliteEntityWorkFlow WHERE IsDup = 1

	-- Now in @sqliteEntityWorkFlow table
	-- fill EmployeeId, EmployeeCode and Date
	-- this is done so that we don't have to join the tables two times
	-- in following queries.
	UPDATE @sqliteEntityWorkFlow
	SET EmployeeId = te.Id,
	    EmployeeCode = te.EmployeeCode,
		[Date] = d.[Date],
		HQCode = sp.HQCode
	FROM @sqliteEntityWorkFlow s1
	INNER JOIN dbo.SqliteEntityWorkFlow s2 on s1.Id = s2.Id
	INNER JOIN dbo.Activity act on s2.ActivityId = act.Id
	INNER JOIN dbo.EmployeeDay ed on act.EmployeeDayId = ed.Id
	INNER JOIN dbo.TenantEmployee te on ed.TenantEmployeeId = te.Id
	INNER JOIN dbo.SalesPerson sp on te.EmployeeCode = sp.StaffCode
	INNER JOIN dbo.[Day] d on ed.DayId = d.Id



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
	[CurrentPhaseEndDate], [InitiationDate], [IsComplete])
	SELECT mem.EmployeeId, mem.EmployeeCode, mem.HQCode, sewf.EntityId, sewf.EntityName, '', '2000-01-01',
	'2000-01-01', mem.[Date], 0
	FROM dbo.SqliteEntityWorkFlow sewf
	INNER JOIN @sqliteEntityWorkFlow mem ON mem.ID = sewf.Id

	--INNER JOIN dbo.Activity act on sewf.ActivityId = act.Id
	--INNER JOIN dbo.EmployeeDay ed on act.EmployeeDayId = ed.Id
	--INNER JOIN dbo.TenantEmployee te on ed.TenantEmployeeId = te.Id
	--INNER JOIN dbo.SalesPerson sp on te.EmployeeCode = sp.StaffCode
	--INNER JOIN dbo.[Day] d on ed.DayId = d.Id

	WHERE sewf.IsProcessed = 0
	AND sewf.Phase = @firstStep
	-- there can be only one open work flow for an entity
	AND NOT EXISTS (SELECT 1 FROM dbo.EntityWorkFlow ewf2 
				WHERE ewf2.IsComplete = 0
				AND ewf2.EntityId = sewf.EntityId)



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
	( ParentId BIGINT, PhaseDate DATE, Phase NVARCHAR(50) )

	-- Now we have parent/child entries for all (including new) work flow requests
	-- Update the status in Detail table
	-- This will output the Ids of parent table (EntityWorkFlow), for which details 
	-- have been updated.
	UPDATE dbo.EntityWorkFlowDetail
	SET ActivityId = sewf.ActivityId,
	IsComplete = 1,
	ActualDate = sewf.[Date],
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
	EmployeeId = mem.EmployeeId
	OUTPUT inserted.EntityWorkFlowId, inserted.ActualDate, inserted.Phase INTO @updatedWorkFlowId
	FROM dbo.EntityWorkFlowDetail wfd
	INNER JOIN dbo.EntityWorkFlow wf on wfd.EntityWorkFlowId = wf.Id
		AND wf.IsComplete = 0
		AND wfd.IsComplete = 0
	INNER JOIN dbo.SqliteEntityWorkFlow sewf ON sewf.EntityId = wf.EntityId
	AND wfd.Phase = sewf.Phase
	INNER JOIN @sqliteEntityWorkFlow mem ON mem.ID = sewf.Id

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
	UPDATE dbo.SqliteEntityWorkFlow
	SET IsProcessed = 1,
	[Timestamp] = @updateTime
	FROM dbo.SqliteEntityWorkFlow sewf
	INNER JOIN @sqliteEntityWorkFlow mem ON mem.ID = sewf.Id
END
GO

ALTER TABLE [dbo].[FeatureControl]
ADD [WorkFlowReportFeature] BIT NOT NULL DEFAULT 0
GO