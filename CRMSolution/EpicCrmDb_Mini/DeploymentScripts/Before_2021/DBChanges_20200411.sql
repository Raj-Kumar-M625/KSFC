-- April 11 2020 - PJM - Seeds workflow
/*
INSERT INTO dbo.WorkFlowSchedule
(TypeName, TagName, Phase, [Sequence], TargetStartAtDay, TargetEndAtDay)
values
('SEEDS', 'Sowing', 'Sowing', 10, 1, 1),
('SEEDS', 'Mulch Removal', 'Removal of Mulch', 20, 3, 4),
('SEEDS', 'Germination Spray', 'Percent Germination observation', 30, 7, 10),
('SEEDS', 'Germination Spray', 'Preventive spray in Nursery 1', 30, 7, 10),
('SEEDS', 'Preventive spray in Nursery 2', 'Preventive spray in Nursery 2', 40, 10, 12),
('SEEDS', 'Main Land Preparation', 'Main Land Preparation', 50, 15, 18),
('SEEDS', 'Transplanting', 'Transplanting', 60, 18, 22),
('SEEDS', 'Inter Cultivation', 'Inter Cultivation', 70, 21, 26),
('SEEDS', 'Integrated Pesticides spray 1', 'Integrated Pesticides spray 1', 80, 30, 33),
('SEEDS', 'Nipping/Spray', 'Nipping', 90, 43, 48),
('SEEDS', 'Nipping/Spray', 'Plant Nutrition Spray 1', 90, 43, 48),
('SEEDS', '1st Top Dressing', '1st Top Dressing', 100, 48, 53),
('SEEDS', 'Earthing Up', 'Earthing Up', 110, 48, 53),
('SEEDS', 'Integrated Pesticides spray 2', 'Integrated Pesticides spray 2', 120, 58, 63),
('SEEDS', 'Harvest Preparation', 'Harvest Preparation', 130, 63, 68)
GO

INSERT INTO dbo.WorkFlowFollowUp
(TypeName, Phase, PhoneDataEntryPage, FollowUpPhaseTag, MinFollowUps, MaxFollowUps, TargetStartAtDay, TargetEndAtDay)
VALUES
('SEEDS',  'Sowing', 'PJMSowingWorkflowPage', '', 0, 0, 0, 0),
('SEEDS',  'Removal of Mulch', 'PJMCommonWorkflowPage', '', 0, 0, 0, 0),
('SEEDS',  'Percent Germination observation', 'PJMGerminationWorkflowPage', '', 0, 0, 0, 0),
('SEEDS',  'Preventive spray in Nursery 1', 'PJMSprayWorkflowPage', '', 0, 0, 0, 0),
('SEEDS',  'Preventive spray in Nursery 2', 'PJMSprayWorkflowPage', '', 0, 0, 0, 0),
('SEEDS',  'Main Land Preparation', 'PJMCommonWorkflowPage', '', 0, 0, 0, 0),
('SEEDS',  'Transplanting', 'PJMTransplantWorkflowPage', '', 0, 0, 0, 0),
('SEEDS',  'Inter Cultivation', 'PJMCommonWorkflowPage', '', 0, 0, 0, 0),
('SEEDS',  'Integrated Pesticides spray 1', 'PJMSprayWorkflowPage',  '', 0, 0, 0, 0),
('SEEDS',  'Nipping', 'PJMNippingWorkflowPage',  '', 0, 0, 0, 0),
('SEEDS',  'Plant Nutrition Spray 1', 'PJMSprayWorkflowPage',  '', 0, 0, 0, 0),
('SEEDS',  '1st Top Dressing', 'PJMFertilizerWorkflowPage',  '', 0, 0, 0, 0),
('SEEDS',  'Earthing Up', 'PJMCommonWorkflowPage',  '', 0, 0, 0, 0),
('SEEDS',  'Integrated Pesticides spray 2', 'PJMSprayWorkflowPage',  '', 0, 0, 0, 0),
('SEEDS',  'Harvest Preparation', 'PJMHarvestPrepWorkflowPage',  'PJMHarvest', 1, 2, 1, 10),
('SEEDS',  'Harvest', 'PJMHarvestWorkflowPage', 'PJMHarvest', 1, 2, 1, 10)
GO

INSERT INTO dbo.CodeTable
(CodeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
('WorkFlowFollowUp', 'PJMHarvest', 'Harvest Preparation', 10, 1, 1),
('WorkFlowFollowUp', 'PJMHarvest', 'Harvest', 20, 1, 1)
GO

INSERT INTO dbo.WorkFlowSeason
(SeasonName, TypeName, StartDate, EndDate, IsOpen)
VALUES
('PJMSeeds', 'SEEDS', '2020-04-10', '2099-12-31', 1)
GO
*/

ALTER TABLE [dbo].[SqliteEntityWorkFlowV2]
ADD	[BatchNumber] NVARCHAR(50) NOT NULL DEFAULT '',
	[LandSize] NVARCHAR(10) NOT NULL DEFAULT '',
	[DWSEntry] NVARCHAR(50) NOT NULL DEFAULT '',
	[ItemCount] BIGINT NOT NULL DEFAULT 0, -- -- this is plant count or nipping count
	[ItemsUsedCount] BIGINT NOT NULL DEFAULT 0, -- this is the count of fertilizers/pesticides used
	[YieldExpected] BIGINT NOT NULL DEFAULT 0,
	[BagsIssued] BIGINT NOT NULL DEFAULT 0,
	[HarvestDate] DATE NOT NULL DEFAULT '1900-01-01'
GO

CREATE TABLE [dbo].[SqliteEntityWorkFlowItemUsed]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
	[SqliteEntityWorkFlowId] BIGINT NOT NULL References dbo.SqliteEntityWorkFlowV2,
	[ItemName] NVARCHAR(50) NOT NULL
)

ALTER TABLE [dbo].[EntityWorkFlowDetail]
ADD	[BatchNumber] NVARCHAR(50) NOT NULL DEFAULT '',
	[LandSize] NVARCHAR(10) NOT NULL DEFAULT '',
	[DWSEntry] NVARCHAR(50) NOT NULL DEFAULT '',
	[ItemCount] BIGINT NOT NULL DEFAULT 0, -- this is plant count or nipping count
	[ItemsUsedCount] BIGINT NOT NULL DEFAULT 0, -- this is the count of fertilizers/pesticides used
	[YieldExpected] BIGINT NOT NULL DEFAULT 0,
	[BagsIssued] BIGINT NOT NULL DEFAULT 0,
	[HarvestDate] DATE NOT NULL DEFAULT '1900-01-01'
GO

CREATE TABLE [dbo].[EntityWorkFlowDetailItemUsed]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
	[EntityWorkFlowDetailId] BIGINT NOT NULL References dbo.EntityWorkFlowDetail,
	[ItemName] NVARCHAR(50) NOT NULL
)
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
	  BatchId BIGINT,
	  ItemsUsedCount BIGINT
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
	BatchId = sewf.BatchId,
	-- April 11 2020 - PJM
	BatchNumber = sewf.BatchNumber,
	LandSize = sewf.LandSize,
	DWSEntry = sewf.DWSEntry,
	ItemCount = sewf.ItemCount,
	ItemsUsedCount = sewf.ItemsUsedCount,
	YieldExpected = sewf.YieldExpected,
	BagsIssued = sewf.BagsIssued,
	HarvestDate = sewf.HarvestDate

	OUTPUT inserted.Id, inserted.EntityWorkFlowId, inserted.ActualDate, 
		inserted.TagName, inserted.Phase, inserted.BatchId, inserted.ItemsUsedCount
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
	-- Step 8b
	-- April 11 2020 - PJM
	-- Insert rows in EntityWorkFlowDetailItemUsed
	----------------------------------------
	INSERT INTO dbo.EntityWorkFlowDetailItemUsed
	(EntityWorkFlowDetailId, ItemName)
	SELECT mw.EntityWorkFlowDetailId, f.ItemName
	FROM @sqliteEntityWorkFlow mw 
	INNER JOIN dbo.SqliteEntityWorkFlowItemUsed f ON mw.Id = f.SqliteEntityWorkFlowId
	-- Items Used can come only for existing workflows
	AND mw.EntityWorkFlowDetailId > 0

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

-- April 12 2020
/*
INSERT INTO dbo.CodeTable
(CodeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
('Sprays', '', 'Econeem +', 10, 1, 1),
('Sprays', '', 'Ecohume Liquid', 20, 1, 1),
('Sprays', '', 'M45', 30, 1, 1),
('Sprays', '', '19 All', 40, 1, 1)
GO

INSERT INTO dbo.CodeTable
(CodeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
('Fertilizers', '', 'Mangala 13:0:45 1Kg', 10, 1, 1),
('Fertilizers', '', 'Mangala 19All 1Kg', 20, 1, 1),
('Fertilizers', '', 'N:P:K: 17:17:17', 30, 1, 1)
GO

INSERT INTO dbo.CodeTable
(CodeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
('Acres', '', '0.5', 10, 1, 1),
('Acres', '', '1.0', 20, 1, 1),
('Acres', '', '1.5', 30, 1, 1),
('Acres', '', '2.0', 40, 1, 1),
('Acres', '', '2.5', 50, 1, 1),
('Acres', '', '3.0', 60, 1, 1),
('Acres', '', '3.5', 70, 1, 1),
('Acres', '', '4.0', 80, 1, 1),
('Acres', '', '4.5', 90, 1, 1),
('Acres', '', '5.0', 100, 1, 1)
GO

DELETE FROM dbo.WorkFlowFollowUp
WHERE TypeName = 'SEEDS' and Phase='1st Top Dressing'
GO

INSERT INTO dbo.WorkFlowFollowUp
(TypeName, Phase, PhoneDataEntryPage, FollowUpPhaseTag, MinFollowUps, MaxFollowUps, TargetStartAtDay, TargetEndAtDay)
VALUES
('SEEDS',  '1st Top Dressing', 'PJMFertilizerWorkflowPage',  '', 0, 0, 0, 0)
GO
*/

-- April 15 2020
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
	wfd.Notes, wfd.IsFollowUpRow
	FROM dbo.EntityWorkFlow wf
	INNER JOIN @hqcodes hq on wf.HQCode = hq.HQCode
	INNER JOIN dbo.EntityWorkFlowDetail wfd on wf.Id = wfd.EntityWorkFlowId
	-- Send all workflow activities, to display on phone - April 15, 2020
	AND (wfd.TagName = wf.TagName OR wfd.IsFollowUpRow = 1)
END
GO
