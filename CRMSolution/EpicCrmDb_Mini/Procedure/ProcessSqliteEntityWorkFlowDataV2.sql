--CREATE PROCEDURE [dbo].[ProcessSqliteEntityWorkFlowDataV2]
--	@batchId BIGINT
--AS
--BEGIN
--   /*
--    Conditions taken care of:
--	a) Can have more than one open work flow for an entity
--	   (but not of same type name)
--	b) Duplicate phase activity records are ignored

--	a can happen in following scenario
--	   - Downloaded entities on phone
--	   - Created a new workflow for an entity
--	   - Uploaded the batch
--	   - (batch is either under process or I did not download again)
--	   - Create another start work flow activity for same entity
--	   - Upload to server

--	b can happen in similar scenario as above

--	In the same batch, I can create multiple activities for same entity
--	(duplicate activities for same entity in same batch are ignored)

--	If we receive duplicates subsequently, it won't update the detail table
--	as the phase is marked as complete.
	  
--   */

--	DECLARE @entityWorkFlow TABLE 
--	(
--		[Id] BIGINT,
--		[Phase] NVARCHAR(50) NOT NULL,
--		[PhaseStartDate] DATE NOT NULL,
--		[PhaseEndDate] DATE NOT NULL
--	)

--	DECLARE @sqliteEntityWorkFlow TABLE 
--	(ID BIGINT, 
--	 EmployeeId BIGINT,
--	 EmployeeCode NVARCHAR(10),
--	 [HQCode] NVARCHAR(10),
--	 [Date] DATE
--	)

--	IF NOT EXISTS(SELECT 1 FROM dbo.SqliteActionBatch 
--			WHERE Id = @batchId AND NumberOfWorkFlowSaved > 0
--			AND BatchProcessed = 0)
--	BEGIN
--		RETURN;
--	END

--	---- if there are no unprocessed entries - return
--	--IF NOT EXISTS(SELECT 1 FROM @sqliteEntityWorkFlow)
--	--BEGIN
--	--	RETURN;
--	--END

--	-- fill Entity Id - if zero 
--	-- not needed now - as we are doing workflow only for approved agreements,
--	-- which will have both entityId and Agreement Id
--	/*
--	UPDATE dbo.SqliteEntityWorkFlowV2
--	SET EntityId = e.Id
--	FROM dbo.SqliteEntityWorkFlowV2 sewf
--	INNER JOIN dbo.Entity e on sewf.EntityType = e.EntityType
--	AND sewf.EntityName = e.EntityName
--	AND sewf.EntityId = 0
--	AND sewf.BatchId = @batchId
--	*/
	
--	-- For one Agreement, we will process only one row
--	-- so refresh @sqliteEntityWorkFlowV2 by removing duplicates
--	-- Also
--	-- Now in @sqliteEntityWorkFlow table
--	-- fill EmployeeId, EmployeeCode and Date
--	-- this is done so that we don't have to join the tables two times
--	-- in following queries.
--	;WITH singleRecCTE(Id, rownum)
--	AS
--	(
--		SELECT sewf.Id,
--		ROW_NUMBER() Over (Partition By sewf.AgreementId ORDER BY sewf.Id)
--		FROM dbo.SqliteEntityWorkFlowV2 sewf
--		WHERE BatchId = @batchId
--	)
--	INSERT INTO @sqliteEntityWorkFlow 
--	(ID, EmployeeId, EmployeeCode, [Date], HQCode)
--	SELECT s2.Id, s2.EmployeeId, te.EmployeeCode, s2.[Date], sp.[HQCode]
--	FROM singleRecCTE cte
--	INNER JOIN dbo.SqliteEntityWorkFlowV2 s2 on cte.Id = s2.Id
--	INNER JOIN dbo.TenantEmployee te on s2.EmployeeId = te.Id
--	INNER JOIN dbo.SalesPerson sp on te.EmployeeCode = sp.StaffCode
--	WHERE cte.rownum = 1
--	AND s2.IsProcessed = 0


--	-- Create a in memory table of work flow schedule
--	-- this will have an additional column indicating previous phase
--	-- Work flow schedule is a small table with say 5 to 10 rows.
--	DECLARE @WorkFlowSchedule TABLE
--	(
--		[Sequence] INT NOT NULL,
--		[TypeName] NVARCHAR(50) NOT NULL,
--		[Phase] NVARCHAR(50) NOT NULL,
--		[TargetStartAtDay] INT NOT NULL,
--		[TargetEndAtDay] INT NOT NULL,
--		[PrevPhase] NVARCHAR(50) NOT NULL
--	)

--	;with schCTE([Sequence], [TypeName], Phase, TargetStartAtDay, TargetEndAtDay, rownum)
--	AS
--	(
--		SELECT [Sequence], TypeName, Phase,  TargetStartAtDay, TargetEndAtDay,
--		ROW_NUMBER() OVER (Order By [TypeName],[Sequence])
--		FROM dbo.[WorkFlowSchedule]
--		WHERE IsActive = 1
--	)
--	INSERT INTO @WorkFlowSchedule
--	([Sequence], [TypeName], [Phase], [TargetStartAtDay], [TargetEndAtDay], [PrevPhase])
--	SELECT [Sequence], [TypeName], Phase, TargetStartAtDay, TargetEndAtDay,
--	ISNULL((SELECT Phase FROM schCTE WHERE rownum = p.rownum-1 and TypeName = p.TypeName), '') PrevPhase
--	FROM schCTE p


--	DECLARE @firstWorkflowStep TABLE
--	(
--		[TypeName] NVARCHAR(50) NOT NULL,
--		[Phase] NVARCHAR(50) NOT NULL
--	)
--	-- Select first step in workflow for each crop
--	;with phaseCTE(TypeName, Phase, [Sequence], rownum)
--	AS
--	(
--		SELECT TypeName, Phase, [Sequence],
--		ROW_NUMBER() OVER (Partition BY [TypeName] Order By [Sequence])
--		from dbo.WorkFlowSchedule
--	)
--	INSERT INTO @firstWorkflowStep
--	(TypeName, Phase)
--	SELECT TypeName, Phase
--	FROM phaseCTE
--	WHERE rownum = 1

--	DECLARE @newid NVARCHAR(50) = newid()
--	DECLARE @newWorkFlow TABLE (ID BIGINT)

--	-- INSERT NEW Rows in EntityWorkFlow 
--	INSERT into dbo.EntityWorkFlow
--	(EmployeeId, [EmployeeCode], HQCode, EntityId, EntityName, 
--	TypeName, CurrentPhase, [CurrentPhaseStartDate],
--	[CurrentPhaseEndDate], [InitiationDate], [IsComplete],
--	[AgreementId], [Agreement])
--	OUTPUT inserted.Id INTO @newWorkFlow
--	SELECT mem.EmployeeId, mem.EmployeeCode, mem.HQCode, sewf.EntityId, sewf.EntityName,
--	sewf.TypeName, @newid, '2000-01-01',
--	'2000-01-01', mem.[Date], 0, 
--	sewf.AgreementId, sewf.Agreement
--	FROM dbo.SqliteEntityWorkFlowV2 sewf
--	INNER JOIN @sqliteEntityWorkFlow mem ON mem.ID = sewf.Id
--	INNER JOIN @firstWorkflowStep fs ON sewf.TypeName = fs.TypeName
--	AND sewf.Phase = fs.Phase
--	-- there can be only one open work flow for an agreement
--	AND NOT EXISTS (SELECT 1 FROM dbo.EntityWorkFlow ewf2 
--				WHERE ewf2.IsComplete = 0
--				AND ewf2.EntityId = sewf.EntityId
--				AND ewf2.AgreementId = sewf.AgreementId
--				)


--	-- now create detail entries for newly created work flow
--	INSERT INTO dbo.EntityWorkFlowDetail
--	(EntityWorkFlowId, [Sequence], [Phase], 
--	[PlannedStartDate], [PlannedEndDate], 
--	[PrevPhase])
--	SELECT wf.Id, sch.[Sequence], sch.Phase, 
--	DATEADD(d, sch.TargetStartAtDay, wf.InitiationDate), DATEADD(d, sch.TargetEndAtDay, wf.InitiationDate),
--	sch.PrevPhase
--	FROM dbo.EntityWorkFlow wf
--	INNER JOIN @newWorkFlow nwf on wf.Id = nwf.ID
--	INNER JOIN @WorkFlowSchedule sch ON wf.TypeName = sch.TypeName


--	DECLARE @updateTime DATETIME2 = SYSUTCDATETIME()
--	DECLARE @updatedWorkFlowId TABLE 
--	( 
--	  Id BIGINT, 
--	  ParentId BIGINT, 
--	  PhaseDate DATE, 
--	  Phase NVARCHAR(50), 
--	  BatchId BIGINT 
--	)

--	-- Now we have parent/child entries for all (including new) work flow requests
--	-- Update the status in Detail table
--	-- This will output the Ids of parent table (EntityWorkFlow), for which details 
--	-- have been updated.
--	UPDATE dbo.EntityWorkFlowDetail
--	SET ActivityId = sqa.ActivityId,
--	IsComplete = 1,
--	ActualDate = sewf.[FieldVisitDate],
--	[Timestamp] = @updateTime,
--	--[PrevPhaseActualDate] = (
--	--							SELECT ActualDate 
--	--							FROM dbo.EntityWorkFlowDetail d2 
--	--							WHERE d2.EntityWorkFlowId = wfd.EntityWorkFlowId 
--	--								AND Phase = wfd.PrevPhase
--	--						),
--	PhaseCompleteStatus = CASE WHEN sewf.[Date] < wfd.PlannedStartDate THEN 'Early' 
--							   WHEN sewf.[Date] > wfd.PlannedEndDate THEN 'Late'
--							   WHEN sewf.[Date] >= wfd.PlannedStartDate AND sewf.[Date] <= wfd.PlannedEndDate THEN 'OnSchedule'
--							   ELSE ''
--							END,
--	EmployeeId = mem.EmployeeId,
--	-- 19.4.19
--	IsStarted = sewf.IsStarted,
--	WorkFlowDate = sewf.[Date],
--	MaterialType = sewf.MaterialType,
--	MaterialQuantity = sewf.MaterialQuantity,
--	GapFillingRequired = sewf.GapFillingRequired,
--	GapFillingSeedQuantity = sewf.GapFillingSeedQuantity,
--	LaborCount = sewf.LaborCount,
--	PercentCompleted = sewf.PercentCompleted,
--	BatchId = sewf.BatchId
--	OUTPUT inserted.Id, inserted.EntityWorkFlowId, inserted.ActualDate, 
--		inserted.Phase, inserted.BatchId
--	INTO @updatedWorkFlowId

--	FROM dbo.EntityWorkFlowDetail wfd
--	INNER JOIN dbo.EntityWorkFlow wf on wfd.EntityWorkFlowId = wf.Id
--		AND wf.IsComplete = 0
--		AND wfd.IsComplete = 0
--	INNER JOIN dbo.SqliteEntityWorkFlowV2 sewf ON sewf.EntityId = wf.EntityId
--	AND sewf.AgreementId = wf.AgreementId
--	AND wfd.Phase = sewf.Phase
--	INNER JOIN @sqliteEntityWorkFlow mem ON mem.ID = sewf.Id
--	-- this join is to get activity Id
--	INNER JOIN dbo.SqliteAction sqa on sqa.[At] = sewf.FieldVisitDate
--	AND sqa.EmployeeId = mem.EmployeeId
--	AND sqa.PhoneDbId = sewf.ActivityId


--	-- put the prev phase actual date, in next phase row of detail table
--	UPDATE dbo.EntityWorkFlowDetail
--	SET PrevPhaseActualDate = u.PhaseDate
--	FROM dbo.EntityWorkFlowDetail d
--	INNER JOIN @updatedWorkFlowId u on d.EntityWorkFlowId = u.ParentId
--	AND d.PrevPhase = u.Phase

--	-- Find out current phase that need to be updated in parent table
--	;WITH updateRecCTE(Id, [Sequence], Phase, PlannedStartDate, PlannedEndDate, rownumber)
--	AS
--	(
--		SELECT uwf.ParentId, wfd.[Sequence], wfd.[Phase], wfd.PlannedStartDate, wfd.PlannedEndDate,
--		ROW_NUMBER() OVER (PARTITION BY uwf.ParentId Order By wfd.[Sequence])
--		FROM @updatedWorkFlowId uwf
--		INNER JOIN dbo.EntityWorkFlowDetail wfd on uwf.ParentId = wfd.EntityWorkFlowId
--		AND wfd.IsComplete = 0
--	)
--	INSERT INTO @entityWorkFlow
--	(Id, Phase, PhaseStartDate, PhaseEndDate)
--	SELECT Id, Phase, PlannedStartDate, PlannedEndDate FROM updateRecCTE
--	WHERE rownumber = 1
	

--	-- Now update the status values in Parent table
--	-- (this only updates the rows when atleast there is one pending stage)
--	UPDATE dbo.EntityWorkFlow
--	SET CurrentPhase = memWf.Phase,
--	CurrentPhaseStartDate = memWf.PhaseStartDate,
--	CurrentPhaseEndDate = memWf.PhaseEndDate,
--	[Timestamp] = @updateTime
--	FROM dbo.EntityWorkFlow wf
--	INNER JOIN @entityWorkFlow memWf ON wf.Id = memWf.Id

--	-- Now mark the rows as complete where phase is marked as complete in Detail table
--	-- (this will happen only when all phases are complete, otherwise previous sql
--	--  will have already set the phase in parent to next available phase)
--	UPDATE dbo.EntityWorkFlow
--	SET IsComplete = 1,
--	[Timestamp] = @updateTime
--	FROM dbo.EntityWorkFlow wf
--	INNER JOIN @updatedWorkFlowId uwf on wf.Id = uwf.ParentId
--	INNER JOIN dbo.EntityWorkFlowDetail wfd on wfd.EntityWorkFlowId = wf.Id
--	AND wfd.Phase = wf.CurrentPhase
--	AND wfd.IsComplete = 1

--	-- Now mark the status in SqliteEntityWorkFlow table
--	UPDATE dbo.SqliteEntityWorkFlowV2
--	SET IsProcessed = 1,
--	[Timestamp] = @updateTime,
--	EntityWorkFlowId = uwf.ParentId
--	FROM dbo.SqliteEntityWorkFlowV2 sewf
--	INNER JOIN dbo.EntityWorkFlow ewf on ewf.AgreementId = sewf.AgreementId
--	INNER JOIN @updatedWorkFlowId uwf on uwf.ParentId = ewf.Id
--	AND uwf.BatchId = sewf.BatchId
--END
