-- April 03 2020 - PJM - Follow Up activity

CREATE TABLE [dbo].[WorkFlowFollowUp]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[TypeName] NVARCHAR(50) NOT NULL, -- could be crop name
	[Phase] NVARCHAR(50) NOT NULL,

	[PhoneDataEntryPage] NVARCHAR(50) NOT NULL DEFAULT '',
	[FollowUpPhaseTag] NVARCHAR(50) NOT NULL DEFAULT '',
	    -- can be called a fk in Code Table

	[MinFollowUps] INT NOT NULL DEFAULT 0,
	[MaxFollowUps] INT NOT NULL DEFAULT 0,

	-- used at the time of follow up activity creation only
	[TargetStartAtDay] INT NOT NULL DEFAULT 0,
	[TargetEndAtDay] INT NOT NULL DEFAULT 0,

	[IsActive] BIT NOT NULL DEFAULT 1
)
GO

ALTER TABLE [dbo].[SqliteEntityWorkFlowV2]
ADD [EntityWorkFlowDetailId] BIGINT NOT NULL DEFAULT 0,
	[FollowUpCount] BIGINT NOT NULL DEFAULT 0
GO

CREATE TABLE [dbo].[SqliteEntityWorkFlowFollowUp]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
	[SqliteEntityWorkFlowId] BIGINT NOT NULL References dbo.SqliteEntityWorkFlowV2,
	[Phase] NVARCHAR(50) NOT NULL,
	[StartDate] DATE NOT NULL,
	[EndDate] DATE NOT NULL,
	[Notes] NVARCHAR(100) NOT NULL DEFAULT ''
)
GO


ALTER TABLE [dbo].[EntityWorkFlowDetail]
ADD 
	[IsFollowUpRow] BIT NOT NULL DEFAULT 0,
	[Notes] NVARCHAR(100) NOT NULL DEFAULT '',
	[ParentRowId] BIGINT NOT NULL DEFAULT 0
GO

CREATE PROCEDURE [dbo].[GetEntityWorkFlow]
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
	wfd.Notes, wfd.IsFollowUpRow
	FROM dbo.EntityWorkFlow wf
	INNER JOIN @hqcodes hq on wf.HQCode = hq.HQCode
	INNER JOIN dbo.EntityWorkFlowDetail wfd on wf.Id = wfd.EntityWorkFlowId
	AND (wfd.TagName = wf.TagName OR wfd.IsFollowUpRow = 1)
END
GO

ALTER PROCEDURE [dbo].[ProcessSqliteEntityWorkFlowDataV3]
	@batchId BIGINT
AS
BEGIN
   /*
    * Modified version of V2 (March 29 2020)
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
		[TagName] NVARCHAR(50) NOT NULL
		--[Phase] NVARCHAR(50) NOT NULL,
		--[PhaseStartDate] DATE NOT NULL,
		--[PhaseEndDate] DATE NOT NULL
	)

	DECLARE @sqliteEntityWorkFlow TABLE 
	(ID BIGINT, 
	EntityWorkFlowDetailId BIGINT,
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
	-- (changed March 27 2020 - with parallel workflow activities, there can be multiples)
	-- so refresh @sqliteEntityWorkFlowV2 by removing duplicates
	-- Also
	-- Now in @sqliteEntityWorkFlow table
	-- fill EmployeeId, EmployeeCode and Date
	-- this is done so that we don't have to join the tables two times
	-- in following queries.
	----------------------------------------
	-- Step 1
	-- Find out transaction rows that need to be processed
	----------------------------------------
	INSERT INTO @sqliteEntityWorkFlow 
	(ID, EntityWorkFlowDetailId, EmployeeId, EmployeeCode, [Date], HQCode)
	SELECT s2.Id, s2.EntityWorkFlowDetailId, s2.EmployeeId, te.EmployeeCode, s2.[Date], et.HQCode
	FROM dbo.SqliteEntityWorkFlowV2 s2 
	INNER JOIN dbo.TenantEmployee te on s2.EmployeeId = te.Id
	INNER JOIN dbo.Entity et on et.Id = s2.EntityId
	WHERE s2.IsProcessed = 0
	AND s2.BatchId = @batchId

	----------------------------------------
	-- Step 2
	-- Create an in memory table of work flow schedule
	-- this will have an additional column indicating previous phase
	-- Work flow schedule is a small table with say 5 to 10 rows.
	----------------------------------------
	DECLARE @WorkFlowSchedule TABLE
	(
		[Sequence] INT NOT NULL,
		[TypeName] NVARCHAR(50) NOT NULL,
		[TagName] NVARCHAR(50) NOT NULL,
		[Phase] NVARCHAR(50) NOT NULL,
		[TargetStartAtDay] INT NOT NULL,
		[TargetEndAtDay] INT NOT NULL,
		[PrevPhase] NVARCHAR(50) NOT NULL
	)

	;with schCTE([Sequence], [TypeName], TagName, Phase, TargetStartAtDay, TargetEndAtDay)
	AS
	(
		SELECT [Sequence], TypeName, TagName, Phase, TargetStartAtDay, TargetEndAtDay
		FROM dbo.[WorkFlowSchedule]
		WHERE IsActive = 1
	)
	INSERT INTO @WorkFlowSchedule
	([Sequence], [TypeName], TagName, Phase, TargetStartAtDay, TargetEndAtDay, PrevPhase)
	SELECT [Sequence], [TypeName], TagName, Phase, TargetStartAtDay, TargetEndAtDay,
	ISNULL((SELECT Top 1 TagName 
			FROM schCTE 
			WHERE TypeName = p.TypeName and [Sequence] < p.[Sequence] 
			ORDER BY [Sequence] Desc ), '') PrevPhase
	FROM schCTE p


	----------------------------------------
	-- Step 3
	-- Select first step in workflow for each crop
	-- (Note: first step won't have parallel activity)
	----------------------------------------
	DECLARE @firstWorkflowStep TABLE
	(
		[TypeName] NVARCHAR(50) NOT NULL,
		[TagName]  NVARCHAR(50) NOT NULL,
		[Phase] NVARCHAR(50) NOT NULL
	)
	;with phaseCTE(TypeName, TagName, Phase, [Sequence], rownum)
	AS
	(
		SELECT TypeName, TagName, Phase, [Sequence],
		ROW_NUMBER() OVER (Partition BY [TypeName] Order By [Sequence])
		from dbo.WorkFlowSchedule
		WHERE IsActive = 1
	)
	INSERT INTO @firstWorkflowStep
	(TypeName, TagName, Phase)
	SELECT TypeName, TagName, Phase
	FROM phaseCTE
	WHERE rownum = 1


	----------------------------------------
	-- Step 4
	-- INSERT NEW Rows in EntityWorkFlow 
	----------------------------------------
	DECLARE @newid NVARCHAR(50) = newid()
	DECLARE @newWorkFlow TABLE (ID BIGINT)

	INSERT into dbo.EntityWorkFlow
	(EmployeeId, [EmployeeCode], HQCode, EntityId, EntityName, 
	TypeName, TagName, 
	[InitiationDate], [IsComplete],
	[AgreementId], [Agreement])
	OUTPUT inserted.Id INTO @newWorkFlow
	SELECT mem.EmployeeId, mem.EmployeeCode, mem.HQCode, sewf.EntityId, sewf.EntityName,
	sewf.TypeName, @newid, 
	mem.[Date], 0, 
	sewf.AgreementId, sewf.Agreement
	FROM dbo.SqliteEntityWorkFlowV2 sewf
	INNER JOIN @sqliteEntityWorkFlow mem ON mem.ID = sewf.Id
	INNER JOIN @firstWorkflowStep fs ON sewf.TypeName = fs.TypeName
	AND sewf.TagName = fs.TagName
	AND sewf.Phase = fs.Phase
	-- there can be only one open work flow for an agreement
	AND NOT EXISTS (SELECT 1 FROM dbo.EntityWorkFlow ewf2 
				WHERE ewf2.IsComplete = 0
				AND ewf2.EntityId = sewf.EntityId
				AND ewf2.AgreementId = sewf.AgreementId
				)

	----------------------------------------
	-- Step 5
	-- now create detail entries for newly created work flow
	----------------------------------------
	INSERT INTO dbo.EntityWorkFlowDetail
	(EntityWorkFlowId, [Sequence], [TagName], [Phase], 
	[PlannedStartDate], [PlannedEndDate], 
	[PrevPhase], IsFollowUpRow)
	SELECT wf.Id, sch.[Sequence], sch.TagName, sch.Phase, 
	DATEADD(d, sch.TargetStartAtDay, wf.InitiationDate), DATEADD(d, sch.TargetEndAtDay, wf.InitiationDate),
	sch.PrevPhase, 0
	FROM dbo.EntityWorkFlow wf
	INNER JOIN @newWorkFlow nwf on wf.Id = nwf.ID
	INNER JOIN @WorkFlowSchedule sch ON wf.TypeName = sch.TypeName


	----------------------------------------
	-- Step 6
	-- Now we have parent/child entries for all (including new) work flow requests
	-- Update the status in Detail table
	-- This will output the Ids of parent table (EntityWorkFlow), for which details 
	-- have been updated.
	----------------------------------------
	DECLARE @updateTime DATETIME2 = SYSUTCDATETIME()
	DECLARE @updatedWorkFlowId TABLE 
	( 
	  Id BIGINT, 
	  ParentId BIGINT, 
	  PhaseDate DATE, 
	  TagName NVARCHAR(50),
	  Phase NVARCHAR(50), 
	  BatchId BIGINT 
	)

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
		inserted.TagName, inserted.Phase, inserted.BatchId
	INTO @updatedWorkFlowId

	FROM dbo.EntityWorkFlowDetail wfd
	INNER JOIN dbo.EntityWorkFlow wf on wfd.EntityWorkFlowId = wf.Id
		AND wf.IsComplete = 0
		AND wfd.IsComplete = 0

	INNER JOIN dbo.SqliteEntityWorkFlowV2 sewf ON sewf.EntityId = wf.EntityId
	AND sewf.AgreementId = wf.AgreementId
	AND wfd.TagName = sewf.TagName
	AND wfd.Phase = sewf.Phase

	INNER JOIN @sqliteEntityWorkFlow mem ON mem.ID = sewf.Id
	-- this join is to get activity Id

	INNER JOIN dbo.SqliteAction sqa on sqa.[At] = sewf.FieldVisitDate
	AND sqa.EmployeeId = mem.EmployeeId
	AND sqa.PhoneDbId = sewf.ActivityId


	----------------------------------------
	-- Step 7
	-- @updatedWorkFlowId table can have duplicates on parentId
	-- (as multiple rows for same agreement (due to parallel schedule) can get updated in previous query)
	-- Hence remove duplicates
	----------------------------------------

	DECLARE @updatedWorkFlowId2 TABLE 
	( 
	  Id BIGINT, 
	  ParentId BIGINT, 
	  PhaseDate DATE, 
	  TagName NVARCHAR(50),
	  Phase NVARCHAR(50), 
	  BatchId BIGINT 
	)
    
	;With uwfCte(Id, ParentId, PhaseDate, TagName, Phase, BatchId, rowNumber)
	AS
	(
	  SELECT Id, ParentId, PhaseDate, TagName, Phase, BatchId, 
	  ROW_NUMBER() OVER (PARTITION BY ParentId Order By PhaseDate DESC)
	  FROM @updatedWorkFlowId
	)
	INSERT INTO @updatedWorkFlowId2
	(Id, ParentId, PhaseDate, TagName, Phase, BatchId)
	SELECT Id, ParentId, PhaseDate, TagName, Phase, BatchId
	FROM uwfCte
	WHERE rowNumber = 1


	----------------------------------------
	-- Step 8
	-- put the prev phase actual date, in next phase row of detail table
	----------------------------------------

	UPDATE dbo.EntityWorkFlowDetail
	SET PrevPhaseActualDate = u.PhaseDate
	FROM dbo.EntityWorkFlowDetail d
	INNER JOIN @updatedWorkFlowId2 u on d.EntityWorkFlowId = u.ParentId
	AND d.PrevPhase = u.TagName


	----------------------------------------
	-- Step 8a
	-- Create follow up entries
	----------------------------------------
	
	INSERT INTO dbo.EntityWorkFlowDetail
	(EntityWorkFlowId, [Sequence], [TagName], [Phase], 
	[PlannedStartDate], [PlannedEndDate], 
	[PrevPhase], IsFollowUpRow, PrevPhaseActualDate, Notes, ParentRowId)
	SELECT wfd.EntityWorkFlowId, wfd.[Sequence], wfd.TagName, f.Phase,
	f.StartDate, f.EndDate,
	wfd.Phase, 1, wfd.ActualDate, f.Notes, wfd.Id
	
	FROM @sqliteEntityWorkFlow mw 
	INNER JOIN dbo.SqliteEntityWorkFlowFollowUp f ON mw.Id = f.SqliteEntityWorkFlowId
	-- follow ups can come only for existing workflows
	AND mw.EntityWorkFlowDetailId > 0
	INNER JOIN dbo.EntityWorkFlowDetail wfd ON wfd.Id = mw.EntityWorkFlowDetailId
	           -- this is the row that resulted in follow up, hence taking it as template

	----------------------------------------
	-- Step 9
	-- Find out current phase that need to be updated in parent table
	-- (This update ignores rows created as follow Up rows)
	-- Follow up activity rows does not restrict the workflow from moving to next phase/stage
	----------------------------------------
	;WITH updateRecCTE(Id, TagName, rownumber)
	AS
	(
		SELECT wfd.[EntityWorkFlowId], wfd.TagName,
		ROW_NUMBER() OVER (PARTITION BY wfd.[EntityWorkFlowId] Order By wfd.[Sequence])
		FROM @updatedWorkFlowId2 uwf
		INNER JOIN dbo.EntityWorkFlowDetail wfd on uwf.ParentId = wfd.EntityWorkFlowId
		AND wfd.IsComplete = 0
		AND wfd.IsFollowUpRow = 0
	)
	INSERT INTO @entityWorkFlow
	(Id, TagName)
	SELECT Id, TagName 
	FROM updateRecCTE
	WHERE rownumber = 1
	
	----------------------------------------
	-- Step 10
	-- Now update the status values in Parent table
	-- (this only updates the rows when atleast there is one pending stage)
	----------------------------------------
	UPDATE dbo.EntityWorkFlow
	SET TagName = memwf.TagName,
	[Timestamp] = @updateTime
	FROM dbo.EntityWorkFlow wf
	INNER JOIN @entityWorkFlow memWf ON wf.Id = memWf.Id
	AND wf.TagName <> memwf.TagName

	----------------------------------------
	-- Step 11
	-- Now mark the rows as complete where phase is marked as complete in Detail table
	-- (this will happen only when all phases are complete, otherwise previous sql
	--  will have already set the phase in parent to next available phase)
	----------------------------------------
	UPDATE dbo.EntityWorkFlow
	SET IsComplete = 1,
	[Timestamp] = @updateTime
	FROM dbo.EntityWorkFlow wf
	INNER JOIN (
			-- @updatedWorkFlowId2 has parentIds that are updated in current session
			-- Left Join returns parentIds that are not complete
			-- So result of left join will be the parentIds that are complete
			SELECT u1.ParentId FROM
			@updatedWorkFlowId2 u1
			LEFT JOIN (
					SELECT Distinct wfd.EntityWorkFlowId AS ParentId
					FROM @updatedWorkFlowId2 uwf 
					INNER JOIN dbo.EntityWorkFlowDetail wfd on wfd.EntityWorkFlowId = uwf.ParentId
					WHERE wfd.IsComplete = 0
			) u2 ON u1.ParentId = u2.ParentId
			WHERE u2.ParentId IS NULL
	) sq ON wf.Id = sq.ParentId

	--UPDATE dbo.EntityWorkFlow
	--SET IsComplete = 1,
	--[Timestamp] = @updateTime
	--FROM dbo.EntityWorkFlow wf
	--INNER JOIN @updatedWorkFlowId2 uwf on wf.Id = uwf.ParentId
	--INNER JOIN dbo.EntityWorkFlowDetail wfd on wfd.EntityWorkFlowId = wf.Id
	--AND wfd.TagName = wf.TagName
	--AND wfd.IsComplete = 1


	----------------------------------------
	-- Step 12
	-- Now mark the status in SqliteEntityWorkFlow table
	-- SqliteEntityWorkFlowV2 can have duplicates for AgreementId (due to parallel workflow)
	-- Mark all rows as processed
	----------------------------------------
	UPDATE dbo.SqliteEntityWorkFlowV2
	SET IsProcessed = 1,
	[Timestamp] = @updateTime,
	EntityWorkFlowId = uwf.ParentId
	FROM dbo.SqliteEntityWorkFlowV2 sewf
	INNER JOIN dbo.EntityWorkFlow ewf on ewf.AgreementId = sewf.AgreementId
	INNER JOIN @updatedWorkFlowId2 uwf on uwf.ParentId = ewf.Id
	AND uwf.BatchId = sewf.BatchId
END
GO

-- Drop Data Entry page name from workFlowSchedule table

CREATE TABLE [dbo].[WorkFlowSchedule_Temp]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[TypeName] NVARCHAR(50) NOT NULL, -- could be crop name
	[Sequence] INT NOT NULL,
	[TagName] NVARCHAR(50) NOT NULL DEFAULT '',
	[Phase] NVARCHAR(50) NOT NULL,
	[TargetStartAtDay] INT NOT NULL,
	[TargetEndAtDay] INT NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT 1
)
GO

INSERT INTO dbo.WorkFlowFollowUp
(TypeName, Phase, PhoneDataEntryPage)
SELECT TypeName, Phase, PhoneDataEntryPage
from dbo.WorkFlowSchedule
WHERE IsActive = 1
GO


INSERT INTO dbo.WorkFlowSchedule_Temp
( TypeName, Sequence, TagName, Phase, TargetStartAtDay, TargetEndAtDay, IsActive)
SELECT
TypeName, Sequence, TagName, Phase, TargetStartAtDay, TargetEndAtDay, IsActive
FROM dbo.WorkFlowSchedule
GO

Drop table dbo.WorkFlowSchedule
GO

exec sp_rename 'WorkFlowSchedule_Temp', 'WorkFlowSchedule'
GO

--===============================

-- Setting up test data
/*
Update dbo.WorkFlowFollowUp
SET FollowUpPhaseTag = 'GherkinHarvestTag',
    MinFollowUps = 1,
	MaxFollowUps = 2
WHERE TypeName = 'Gherkin' and Phase = 'First Harvest'
GO

Insert into dbo.WorkFlowFollowUp
(TypeName, Phase, PhoneDataEntryPage, FollowUpPhaseTag, MinFollowUps, MaxFollowUps, TargetStartAtDay, TargetEndAtDay)
values
('Gherkin', 'HarvestPlanning', 'CommonWorkFlowEntryPage', '', 0, 0, 1, 5),
('Gherkin', 'Harvest', 'CommonWorkFlowEntryPage', 'GherkinHarvestTag', 1, 2, 3, 7)
GO

INSERT INTO dbo.CodeTable
(CodeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
('WorkFlowFollowUp', 'GherkinHarvestTag', 'HarvestPlanning', 10, 1, 1),
('WorkFlowFollowUp', 'GherkinHarvestTag', 'Harvest', 20, 1, 1)
GO

-- Data Set up - April 5 2020
Update dbo.WorkFlowFollowUp
SET MaxFollowUps = 3,
FollowUpPhaseTag = 'GherkinsSpray1'
WHERE TypeName = 'Gherkins' and Phase = 'Insect Spray 1'

Update dbo.WorkFlowFollowUp
SET MaxFollowUps = 3,
FollowUpPhaseTag = 'GherkinsSpray2'
WHERE TypeName = 'Gherkins' and Phase = 'Insect Spray 2'

Update dbo.WorkFlowFollowUp
SET MaxFollowUps = 3,
FollowUpPhaseTag = 'GherkinsSpray3'
WHERE TypeName = 'Gherkins' and Phase = 'Insect Spray 3'


Update dbo.WorkFlowFollowUp
SET MaxFollowUps = 1,
FollowUpPhaseTag = 'GherkinsHarvest'
WHERE TypeName = 'Gherkins' and Phase = 'Harvest'

INSERT INTO dbo.WorkFlowFollowUp
(TypeName, Phase, PhoneDataEntryPage, TargetStartAtDay, TargetEndAtDay)
Values
('Gherkins', 'GS1P1', 'CommonWorkflowEntryPage', 21, 22),
('Gherkins', 'GS1P2', 'CommonWorkflowEntryPage', 22, 23),
('Gherkins', 'GS1P3', 'CommonWorkflowEntryPage', 5, 10),

('Gherkins', 'GS2P1', 'CommonWorkflowEntryPage', 1, 5),
('Gherkins', 'GS2P2', 'CommonWorkflowEntryPage', 2, 6),
('Gherkins', 'GS2P3', 'CommonWorkflowEntryPage', 3, 7),

('Gherkins', 'GS3P1', 'CommonWorkflowEntryPage', 1, 5),
('Gherkins', 'GS3P2', 'CommonWorkflowEntryPage', 4, 6),
('Gherkins', 'GS3P3', 'CommonWorkflowEntryPage', 7, 9),

('Gherkins', 'CleanUp', 'CommonWorkflowEntryPage', 5, 10)

INSERT INTO dbo.CodeTable
(CodeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
('WorkFlowFollowUp', 'GherkinsSpray1', 'GS1P1', 10, 1, 1),
('WorkFlowFollowUp', 'GherkinsSpray1', 'GS1P2', 20, 1, 1),
('WorkFlowFollowUp', 'GherkinsSpray1', 'GS1P3', 20, 1, 1),

('WorkFlowFollowUp', 'GherkinsSpray2', 'GS2P1', 10, 1, 1),
('WorkFlowFollowUp', 'GherkinsSpray2', 'GS2P2', 20, 1, 1),
('WorkFlowFollowUp', 'GherkinsSpray2', 'GS2P3', 20, 1, 1),

('WorkFlowFollowUp', 'GherkinsSpray3', 'GS3P1', 10, 1, 1),
('WorkFlowFollowUp', 'GherkinsSpray3', 'GS3P2', 20, 1, 1),
('WorkFlowFollowUp', 'GherkinsSpray3', 'GS3P3', 20, 1, 1),

('WorkFlowFollowUp', 'GherkinsHarvest', 'CleanUp', 10, 1, 1)

GO
*/