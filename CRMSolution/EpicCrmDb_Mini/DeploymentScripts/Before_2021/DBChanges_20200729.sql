-- July 29 2020
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


	SELECT wf.EntityId, e.EntityName, wf.Id, 
	wfd.TagName, wfd.Phase, wfd.PlannedStartDate, wfd.PlannedEndDate,
	wf.InitiationDate, wfd.IsComplete, wf.AgreementId, wfd.Id 'EntityWorkFlowDetailId',
	wfd.Notes, wfd.IsFollowUpRow,
	(CASE WHEN wfd.TagName = wf.TagName and wfd.IsFollowUpRow = 0 Then 1 ELSE 0 END) AS IsCurrentPhaseRow
	FROM dbo.EntityWorkFlow wf
	INNER JOIN dbo.Entity e on wf.EntityId = e.Id
	INNER JOIN @hqcodes hq on e.HQCode = hq.HQCode
	INNER JOIN dbo.EntityWorkFlowDetail wfd on wf.Id = wfd.EntityWorkFlowId
	-- Send all workflow activities, to display on phone - April 15, 2020
	--AND (wfd.TagName = wf.TagName OR wfd.IsFollowUpRow = 1)

	-- July 29 2020 - Don't consider HQCode from EntityWorkFlow table
	-- as User is allowed to change Entity's HQCode in dbo.EntityTable
	-- in that case, the HQCode in EntityWorkFlow table becomes stale.
END
GO

ALTER TABLE [dbo].[EntityWorkFlow]
DROP COLUMN [HQCode]
GO

ALTER TABLE [dbo].[EntityWorkFlow]
DROP COLUMN [EntityName]
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
	(ID, EntityWorkFlowDetailId, EmployeeId, EmployeeCode, [Date])
	SELECT s2.Id, s2.EntityWorkFlowDetailId, s2.EmployeeId, te.EmployeeCode, s2.[Date]
	FROM dbo.SqliteEntityWorkFlowV2 s2 
	INNER JOIN dbo.TenantEmployee te on s2.EmployeeId = te.Id
	--INNER JOIN dbo.Entity et on et.Id = s2.EntityId
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
	(EmployeeId, [EmployeeCode], EntityId, 
	TypeName, TagName, 
	[InitiationDate], [IsComplete],
	[AgreementId], [Agreement])
	OUTPUT inserted.Id INTO @newWorkFlow
	SELECT mem.EmployeeId, mem.EmployeeCode, sewf.EntityId, 
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

/*
ALTER table dbo.dws
drop constraint DF__DWS__HQCode__5F891AA4
GO
*/

ALTER TABLE dbo.DWS
DROP COLUMN [EntityName]
GO

ALTER TABLE dbo.DWS
DROP COLUMN [HQCode]
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
	[FilledBagsWeightKg], [EmptyBagsWeightKg], EntityId, 
	AgreementId, Agreement, [EntityWorkFlowDetailId], TypeName, TagName, 
	ActivityId, [SqliteSTRDWSId], CreatedBy, DateCreated, DateUpdated,
	[OrigBagCount], [OrigFilledBagsKg], [OrigEmptyBagsKg]
	)
	OUTPUT inserted.Id, inserted.SqliteSTRDWSId into @insertedDWS
	SELECT t2.STRTagId, t2.Id, input.DWSNumber, input.DWSDate, input.BagCount,
	input.[FilledBagsWeightKg], input.[EmptyBagsWeightKg], input.EntityId,
	input.AgreementId, input.Agreement, input.[EntityWorkFlowDetailId], input.TypeName, input.TagName, 
	sqa.ActivityId, input.Id, 'ProcessSqliteSTRData', @currentTime, @currentTime,
	input.BagCount, input.[FilledBagsWeightKg], input.[EmptyBagsWeightKg]

	FROM dbo.SqliteSTRDWS input

	INNER JOIN @insertedSTR t2 on t2.SqliteSTRId = input.SqliteSTRId
	-- only for the rows for which entry is made in STR table.

	INNER JOIN dbo.SqliteAction sqa on sqa.[At] = input.[DWSDate]
	AND sqa.EmployeeId = t2.EmployeeId
	AND sqa.PhoneDbId = input.ActivityId

	---- these two joins are to get HQCode
	--INNER JOIN dbo.EntityWorkFlowDetail wfd ON wfd.Id = input.EntityWorkFlowDetailId
	--INNER JOIN dbo.EntityWorkFlow wf on wf.Id = wfd.EntityWorkFlowId

	-- Update id in SqliteSTRDWS table
	UPDATE dbo.SqliteSTRDWS
	SET DWSId = t2.Id,
	IsProcessed = 1
	FROM dbo.SqliteSTRDWS t1
	INNER JOIN @insertedDWS t2 on t1.Id = t2.[SqliteSTRDWSId]
END
GO

-- August 02 2020
ALTER TABLE [dbo].[SalesPerson]
ADD
	[BusinessRole] NVARCHAR(50) NOT NULL DEFAULT ''
GO

insert into dbo.CodeTable
(CodeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
( 'BusinessRole','', 'FieldStaff', 10, 1, 1),
( 'BusinessRole', '', 'Executive',  20, 1, 1),
( 'BusinessRole', '', 'Manager',  30, 1, 1)
go

-- Aug 25 2020
insert into dbo.CodeTable
(CodeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
( 'WorkflowStatus', 'Active', 1, 10, 1, 1),
( 'WorkflowStatus', 'InActive', 0,  20, 1, 1)
go

ALTER TABLE [dbo].[EntityWorkFlowDetail]
ADD 
	[IsActive] BIT NOT NULL DEFAULT 1,
	[CreatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[UpdatedBy] NVARCHAR(50) NOT NULL DEFAULT ''
GO

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


	SELECT wf.EntityId, e.EntityName, wf.Id, 
	wfd.TagName, wfd.Phase, wfd.PlannedStartDate, wfd.PlannedEndDate,
	wf.InitiationDate, wfd.IsComplete, wf.AgreementId, wfd.Id 'EntityWorkFlowDetailId',
	wfd.Notes, wfd.IsFollowUpRow,
	(CASE WHEN wfd.TagName = wf.TagName and wfd.IsFollowUpRow = 0 Then 1 ELSE 0 END) AS IsCurrentPhaseRow,
	wfd.IsActive
	FROM dbo.EntityWorkFlow wf
	INNER JOIN dbo.Entity e on wf.EntityId = e.Id
	INNER JOIN @hqcodes hq on e.HQCode = hq.HQCode
	INNER JOIN dbo.EntityWorkFlowDetail wfd on wf.Id = wfd.EntityWorkFlowId
	-- Send all workflow activities, to display on phone - April 15, 2020
	--AND (wfd.TagName = wf.TagName OR wfd.IsFollowUpRow = 1)

	-- July 29 2020 - Don't consider HQCode from EntityWorkFlow table
	-- as User is allowed to change Entity's HQCode in dbo.EntityTable
	-- in that case, the HQCode in EntityWorkFlow table becomes stale.
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
	(ID, EntityWorkFlowDetailId, EmployeeId, EmployeeCode, [Date])
	SELECT s2.Id, s2.EntityWorkFlowDetailId, s2.EmployeeId, te.EmployeeCode, s2.[Date]
	FROM dbo.SqliteEntityWorkFlowV2 s2 
	INNER JOIN dbo.TenantEmployee te on s2.EmployeeId = te.Id
	--INNER JOIN dbo.Entity et on et.Id = s2.EntityId
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
	(EmployeeId, [EmployeeCode], EntityId, 
	TypeName, TagName, 
	[InitiationDate], [IsComplete],
	[AgreementId], [Agreement])
	OUTPUT inserted.Id INTO @newWorkFlow
	SELECT mem.EmployeeId, mem.EmployeeCode, sewf.EntityId, 
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
	--
	-- [ This step is also repeated/duplicated in [UpdateWorkflowStatus]
	-- Aug 25 2020
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
		AND wfd.IsActive = 1 -- ignore rows marked as inactive
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
	--
	-- [ This step is also repeated/duplicated in [UpdateWorkflowStatus]
	-- Aug 25 2020
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

CREATE PROCEDURE [dbo].[UpdateWorkflowStatus]
	@entityWorkflowId BIGINT,
	@currentUser NVARCHAR(50)
AS
BEGIN
	/* Called from web when status changes take place
		created by making a copy of [ProcessSqliteEntityWorkFlowDataV3]
     */

	DECLARE @entityWorkFlow TABLE 
	(
		[Id] BIGINT,
		[TagName] NVARCHAR(50) NOT NULL
	)

	DECLARE @updateTime DATETIME2 = SYSUTCDATETIME()
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
		FROM dbo.EntityWorkFlowDetail wfd 
		WHERE @entityWorkflowId = wfd.EntityWorkFlowId
		AND wfd.IsComplete = 0
		AND wfd.IsFollowUpRow = 0
		AND wfd.IsActive = 1 -- ignore rows marked as inactive
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
	--
	-- [ This step is also repeated/duplicated in [UpdateWorkflowStatus]
	-- Aug 25 2020 ]
	----------------------------------------

	DECLARE @isComplete BIT
	SELECT @isComplete = CASE WHEN totalRows = completedRows THEN 1 ELSE 0 END
	FROM
	(
		SELECT COUNT(*) totalRows, SUM(CASE WHEN IsComplete = 1 THEN 1 ELSE 0 END) completedRows
		FROM dbo.EntityWorkFlowDetail wfd 
		WHERE wfd.EntityWorkFlowId = @entityWorkflowId
		AND wfd.IsActive = 1
	) iq

	UPDATE dbo.EntityWorkFlow
	SET IsComplete = @isComplete,
	[Timestamp] = @updateTime
	WHERE Id = @entityWorkflowId
	AND IsComplete <> @isComplete

END
GO
