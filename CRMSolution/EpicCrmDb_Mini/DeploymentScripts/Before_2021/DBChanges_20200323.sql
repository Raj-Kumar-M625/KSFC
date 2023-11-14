-- March 23 2020 - PJMargo Contract Farming
-- (Changes for Parallel Workflow activity)

INSERT into dbo.codeTable
(codeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
('IssueReturnActivityType', '', 'Input Issue', 10, 1, 1),
('IssueReturnActivityType', '', 'Input Return', 20, 1, 1)
go

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

ALTER TABLE [dbo].[SqliteIssueReturn]
ADD	[SlipNumber] NVARCHAR(50) NOT NULL DEFAULT '',
	[Acreage] DECIMAL(19,2) NOT NULL DEFAULT 0
GO

ALTER TABLE [dbo].[IssueReturn]
ADD	[SlipNumber] NVARCHAR(50) NOT NULL DEFAULT '',
	[LandSizeInAcres] DECIMAL(19,2) NOT NULL DEFAULT 0
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
	[SqliteIssueReturnId], [SlipNumber], [LandSizeInAcres])
	OUTPUT INSERTED.EntityId, '', '', inserted.EntityAgreementId, '', '', inserted.Quantity,
	inserted.ItemMasterId, '', '', inserted.TransactionType INTO @NewItems
	SELECT 
	sqe.[EmployeeId], d.[Id] AS [DayId], sqe.EntityId, 
	CASE WHEN sqe.[AgreementId] = 0 THEN NULL ELSE sqe.[AgreementId] END,
	sqe.[ItemId],
	CAST(sqe.[IssueReturnDate] AS [Date]), sqe.[TranType], sqe.[Quantity], sqa.ActivityId,
	sqe.[Id], sqe.SlipNumber, sqe.Acreage

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

-- March 27 2020
ALTER TABLE [dbo].[WorkFlowSchedule]
ADD
	[TagName] NVARCHAR(50) NOT NULL DEFAULT ''
GO

ALTER TABLE [dbo].[EntityWorkFlowDetail]
ADD	[TagName] NVARCHAR(50) NOT NULL DEFAULT ''
GO

ALTER TABLE [dbo].[SqliteEntityWorkFlowV2]
ADD 
	[TagName] NVARCHAR(50) NOT NULL DEFAULT ''
GO

UPDATE dbo.WorkFlowSchedule
SET TagName = Phase
GO

UPDATE dbo.EntityWorkFlowDetail
SET TagName = Phase
GO

-- March 29 2020
ALTER TABLE dbo.EntityWorkFlow
ADD [TagName] NVARCHAR(50) NOT NULL DEFAULT ''
GO

UPDATE dbo.EntityWorkFlow
SET TagName = CurrentPhase
GO

ALTER TABLE dbo.EntityWorkFlow
DROP COLUMN CurrentPhase, CurrentPhaseStartDate, CurrentPhaseEndDate
GO

CREATE PROCEDURE [dbo].[ProcessSqliteEntityWorkFlowDataV3]
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
	INSERT INTO @sqliteEntityWorkFlow 
	(ID, EmployeeId, EmployeeCode, [Date], HQCode)
	SELECT s2.Id, s2.EmployeeId, te.EmployeeCode, s2.[Date], et.HQCode
	FROM dbo.SqliteEntityWorkFlowV2 s2 
	INNER JOIN dbo.TenantEmployee te on s2.EmployeeId = te.Id
	--INNER JOIN dbo.SalesPerson sp on te.EmployeeCode = sp.StaffCode
	INNER JOIN dbo.Entity et on et.Id = s2.EntityId
	WHERE s2.IsProcessed = 0
	AND s2.BatchId = @batchId

	-- Create an in memory table of work flow schedule
	-- this will have an additional column indicating previous phase
	-- Work flow schedule is a small table with say 5 to 10 rows.
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

	DECLARE @firstWorkflowStep TABLE
	(
		[TypeName] NVARCHAR(50) NOT NULL,
		[TagName]  NVARCHAR(50) NOT NULL,
		[Phase] NVARCHAR(50) NOT NULL
	)
	-- Select first step in workflow for each crop
	-- (Note: first step won't have parallel activity)
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

	DECLARE @newid NVARCHAR(50) = newid()
	DECLARE @newWorkFlow TABLE (ID BIGINT)

	-- INSERT NEW Rows in EntityWorkFlow 
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


	-- now create detail entries for newly created work flow
	INSERT INTO dbo.EntityWorkFlowDetail
	(EntityWorkFlowId, [Sequence], [TagName], [Phase], 
	[PlannedStartDate], [PlannedEndDate], 
	[PrevPhase])
	SELECT wf.Id, sch.[Sequence], sch.TagName, sch.Phase, 
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
	  TagName NVARCHAR(50),
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

	-- @updatedWorkFlowId table can have duplicates on parentId
	-- (as multiple rows for same agreement (due to parallel schedule) can get updated in previous query)
	-- Hence remove duplicates
	
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

	-- put the prev phase actual date, in next phase row of detail table
	UPDATE dbo.EntityWorkFlowDetail
	SET PrevPhaseActualDate = u.PhaseDate
	FROM dbo.EntityWorkFlowDetail d
	INNER JOIN @updatedWorkFlowId2 u on d.EntityWorkFlowId = u.ParentId
	AND d.PrevPhase = u.TagName

	-- Find out current phase that need to be updated in parent table
	-- For this pending workflow rows are ordered by Planned End Date Desc
	;WITH updateRecCTE(Id, TagName, rownumber)
	AS
	(
		SELECT wfd.[EntityWorkFlowId], wfd.TagName,
		ROW_NUMBER() OVER (PARTITION BY wfd.[EntityWorkFlowId] Order By wfd.[Sequence])
		FROM @updatedWorkFlowId2 uwf
		INNER JOIN dbo.EntityWorkFlowDetail wfd on uwf.ParentId = wfd.EntityWorkFlowId
		AND wfd.IsComplete = 0
	)
	INSERT INTO @entityWorkFlow
	(Id, TagName)
	SELECT Id, TagName FROM updateRecCTE
	WHERE rownumber = 1
	

	-- Now update the status values in Parent table
	-- (this only updates the rows when atleast there is one pending stage)
	UPDATE dbo.EntityWorkFlow
	SET TagName = memwf.TagName,
	--CurrentPhase = memWf.Phase,
	--CurrentPhaseStartDate = memWf.PhaseStartDate,
	--CurrentPhaseEndDate = memWf.PhaseEndDate,
	[Timestamp] = @updateTime
	FROM dbo.EntityWorkFlow wf
	INNER JOIN @entityWorkFlow memWf ON wf.Id = memWf.Id

	-- Now mark the rows as complete where phase is marked as complete in Detail table
	-- (this will happen only when all phases are complete, otherwise previous sql
	--  will have already set the phase in parent to next available phase)
	-- 
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

	-- Now mark the status in SqliteEntityWorkFlow table
	-- SqliteEntityWorkFlowV2 can have duplicates for AgreementId (due to parallel workflow)
	-- Mark all rows as processed
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
