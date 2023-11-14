-- 20.12.2018
ALTER PROCEDURE [dbo].[GetTrackingRecordsForDistanceCalculation]
	@recordCount int
AS
BEGIN

    DECLARE @currentDateTime DATETIME2 = SYSUTCDATETIME();

	UPDATE TOP(@recordCount) dbo.Tracking With (READPAST)
	SET LockTimestamp = @currentDateTime
	OUTPUT inserted.Id, inserted.BeginGPSLatitude, inserted.BeginGPSLongitude,
	       inserted.EndGPSLatitude, inserted.EndGPSLongitude,
		   inserted.IsMilestone, inserted.IsStartOfDay, inserted.IsEndOfDay
	WHERE IsMileStone = 1 AND ((DistanceCalculated = 0 AND LockTimestamp IS NULL) OR
	 (LockTimeStamp Is NOT NULL AND DATEDIFF(mi, LockTimestamp, @currentDateTime) >= 5))
END
GO

-- 22.12.2018
ALTER TABLE [dbo].[SqliteEntity]
ADD [ImageCount] INT NOT NULL DEFAULT 0;
GO

CREATE TABLE [dbo].[SqliteEntityImage]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[SqliteEntityId] BIGINT NOT NULL REFERENCES SqliteEntity(Id),
	[SequenceNumber] INT NOT NULL DEFAULT 0,
	[ImageFileName] VARCHAR(100) NOT NULL DEFAULT 'FileNotFound.jpg'
);
GO

ALTER TABLE [dbo].[Entity]
ADD [ImageCount] INT NOT NULL DEFAULT 0;
GO


CREATE TABLE [dbo].[EntityImage](
	[Id] [bigint] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[EntityId] [bigint] NOT NULL REFERENCES [dbo].[Entity](Id),
	[ImageId] [bigint] NOT NULL REFERENCES [dbo].[Image](Id),
	[SequenceNumber] [int] NOT NULL
);
GO

IF OBJECT_ID ( '[dbo].[ProcessSqliteEntityData]', 'P' ) IS NOT NULL   
    DROP PROCEDURE [dbo].[ProcessSqliteEntityData];  
GO 

CREATE PROCEDURE [dbo].[ProcessSqliteEntityData]
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
	[Latitude], [Longitude], [MNC], [MCC], [LAC], [CellId], 
	[SqliteEntityId], [ContactCount], [CropCount], [ImageCount])

	SELECT sqe.[EmployeeId], d.[Id] AS [DayId], sp.[HQCode], sqe.[AtBusiness], 
	sqe.[EntityType], sqe.[EntityName], sqe.[TimeStamp] AS [EntityDate], 
	sqe.[Address], sqe.[City], sqe.[State], sqe.[Pincode], sqe.[LandSize], 
	sqe.[Latitude], sqe.[Longitude], sqe.[MNC], sqe.[MCC], sqe.[LAC], sqe.[CellId], 
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

	-- now we need to update the id in SqliteEntity table
	UPDATE dbo.SqliteEntity
	SET EntityId = e.Id,
	IsProcessed = 1,
	DateUpdated = SYSUTCDATETIME()
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
END
GO

-- 23 12 2018
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

-- 29.12.2018
-- Adding two new columns, for Executive summary report
ALTER TABLE [dbo].[EmployeeDay]
ADD 
	[TotalActivityCount] INT NOT NULL DEFAULT 0,
	[CurrentLocTime] DATETIME2 NOT NULL DEFAULT '2000-01-01'
GO

ALTER PROCEDURE [dbo].[AddTrackingData]
	@employeeDayId BIGINT,
	@trackingDateTime DateTime2,
	@latitude Decimal(19,9),
	@longitude Decimal(19,9),
	@activityId BIGINT,
	@isMilestone BIT,  -- 1 if we want to log this request irrespective of the fact that it is sent too soon
	@isStartOfDay BIT,
	@isEndOfDay BIT,
	@trackingId BIGINT OUTPUT
AS
BEGIN
   -- Records tracking activity
   -- distanceInMeters that is passed from Mobile Device is not saved in db.
    SET @trackingId = 0
	-- check if a record already exist in Employee Day table
	-- At end of the day, we do need to log a tracking entry as MileStone flag set to 1 - hence the OR condition here;
	IF EXISTS(SELECT 1 FROM dbo.EmployeeDay WHERE Id = @employeeDayId AND (EndTime is null OR @isMilestone = 1))
	BEGIN
	    -- Find out Begin coordinates from last record
		DECLARE @beginLatitude DECIMAL(19,9) = 0
		DECLARE @beginLongitude DECIMAL(19,9) = 0
		DECLARE @chainedTrackingId BIGINT = 0
		DECLARE @lastTrackingDateTime DATETIME2

		IF @isStartOfDay = 1
		BEGIN
			SET @chainedTrackingId = NULL
		END
		ELSE IF @isMilestone = 1
		BEGIN
		    -- for milestone recordings - look for previous milestone entries or first entry
			SELECT @chainedTrackingId = MAX(ID)
				FROM dbo.Tracking WITH (NOLOCK)
				WHERE EmployeeDayId = @employeeDayId
				AND IsMilestone = 1
		END
		ELSE
		BEGIN
			SELECT @chainedTrackingId = MAX(ID)
				FROM dbo.Tracking WITH (NOLOCK)
				WHERE EmployeeDayId = @employeeDayId
		END
 
		------------------

		SELECT @beginLatitude = EndGpsLatitude,
		       @beginLongitude = EndGpsLongitude,
			   @lastTrackingDateTime = [At]
		FROM dbo.tracking WITH (NOLOCK)
		WHERE Id = ISNULL(@chainedTrackingId,0)

		-- if tracking request is coming too soon - from configured time
		-- don't log the tracking request
		DECLARE @logCurrentTrackingRequest BIT = 0;
		IF @isMilestone = 1 OR ISNULL(@chainedTrackingId,0) = 0
		BEGIN
		    -- must log milestone entries;
			-- first tracking request for day - log it;
			SET @logCurrentTrackingRequest = 1
		END
		ELSE
		IF @beginLatitude = @latitude AND @beginLongitude = @longitude
		BEGIN
			SET @logCurrentTrackingRequest = 0
		END
		ELSE
		BEGIN
			-- find out expected delay
			DECLARE @TimeIntervalInMillisecondsForTracking BIGINT
			SELECT @TimeIntervalInMillisecondsForTracking = te.TimeIntervalInMillisecondsForTracking
			FROM dbo.TenantEmployee te
			INNER JOIN dbo.EmployeeDay ed on te.Id = ed.TenantEmployeeId
			AND ed.Id = @employeeDayId

			-- add time to last tracking record time
			--IF @trackingDateTime >= DATEADD(ms, @TimeIntervalInMillisecondsForTracking, @lastTrackingDateTime)
			BEGIN TRY
				IF DATEDIFF(ms, @lastTrackingDateTime, @trackingDateTime) >= @TimeIntervalInMillisecondsForTracking
				BEGIN
					SET @logCurrentTrackingRequest = 1
				END
			END TRY
			BEGIN CATCH
				SET @logCurrentTrackingRequest = 0
			END CATCH
		END
		
		------------------------------

		IF @logCurrentTrackingRequest = 1
		BEGIN
			-- find activity type
			DECLARE @activityType NVARCHAR(50) = ''
			SELECT @activityType = ISNULL(activityType, '')
			FROM dbo.Activity
			WHERE ID = @activityId

			-- Create new record in Tracking table
			INSERT into dbo.Tracking
			(ChainedTrackingId, EmployeeDayId, [At], BeginGPSLatitude, BeginGPSLongitude, 
			EndGPSLatitude, EndGPSLongitude, BeginLocationName, EndLocationName, BingMapsDistanceInMeters, 
			GoogleMapsDistanceInMeters, LinearDistanceInMeters,
			DistanceCalculated, LockTimestamp, 
			ActivityId, IsMilestone, IsStartOfDay, IsEndOfDay, ActivityType)
			VALUES
			(@chainedTrackingId, @employeeDayId, @trackingDateTime, @beginLatitude, @beginLongitude,
			 @latitude, @longitude, NULL, NULL, 0,
			 0, 0,
			 --@isStartOfDay, Null,
			 0, Null,
			 -- if it is start of day request, then we mark it as calculated
			 -- for start of day also, leave distance calculated as 0 - so we can get location name for reports
			 @activityId, @isMilestone,
			 @isStartOfDay,
			 @isEndOfDay,
			 @activityType)

			SET @trackingId = SCOPE_IDENTITY()

             -- update most recent location in EmployeeDay table as well
			 UPDATE dbo.EmployeeDay
			 SET CurrentLatitude = @latitude,
			 CurrentLongitude = @longitude,
			 CurrentLocTime = @trackingDateTime
			 WHERE Id = @employeeDayId
		END
		ELSE
		BEGIN
			SET @trackingId = -2
		END
	END
END
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

		-- keep count of total activities for exec crm application
		UPDATE dbo.EmployeeDay
		SET TotalActivityCount = TotalActivityCount + 1
		WHERE Id = @employeeDayId
	END
END
GO

ALTER PROCEDURE [dbo].[GetInFieldSalesPeople]
		@inputDate DateTime2
AS
BEGIN

	DECLARE @SignedInEmployeeData TABLE
	(
        EmployeeDayId BIGINT,
		EmployeeId BIGINT,
		StartTime DATETIME2,
		EndTime DATETIME2,
		TotalOrderAmount DECIMAL(19,2),
		TotalPaymentAmount DECIMAL(19,2),
		TotalReturnAmount DECIMAL(19,2),
		TotalExpenseAmount DECIMAL(19,2),
		TotalActivityCount INT,
		Latitude DECIMAL(19,9),
		Longitude DECIMAL(19,9),
		CurrentLocTime DATETIME2,
		PhoneModel NVARCHAR(100),
		PhoneOS NVARCHAR(10),
		AppVersion NVARCHAR(10)
	)

	INSERT INTO @SignedInEmployeeData
	(EmployeeDayId, EmployeeId, StartTime, EndTime, 
	 TotalOrderAmount, TotalPaymentAmount, TotalReturnAmount, TotalExpenseAmount,
	 Latitude, Longitude, PhoneModel, PhoneOS, AppVersion,
	 TotalActivityCount, CurrentLocTime
	)
	SELECT ed.Id, ed.TenantEmployeeId, StartTime, EndTime, 
	TotalOrderAmount, TotalPaymentAmount, 
	TotalReturnAmount, TotalExpenseAmount,
	CurrentLatitude, CurrentLongitude,
	PhoneModel, PhoneOS, AppVersion,
	ed.TotalActivityCount, ed.CurrentLocTime
	FROM dbo.[Day] d
	INNER JOIN dbo.EmployeeDay ed on d.Id = ed.DayId
	AND d.[DATE] = CAST(@inputDate AS [Date])
	
	-- RESULT SET QUERY
	;WITH cteAreaCodes(AreaCode)
	AS
	( 
		SELECT Distinct  AreaCode FROM dbo.OfficeHierarchy 
		WHERE IsActive = 1
	)
	SELECT ISNULL(cte.AreaCode, '***') AreaCode, 
	ISNULL(sq.StaffCode, '') StaffCode, 
	ISNULL(sq.IsInFieldToday, 0) IsInFieldToday,
	ISNULL(sq.IsRegisteredOnPhone,0) IsRegisteredOnPhone,
	sq.StartTime,
	sq.EndTime,
	ISNULL(sq.TotalOrderAmount, 0) as TotalOrderAmount,
	ISNULL(sq.TotalPaymentAmount,0) as TotalPaymentAmount,
	ISNULL(sq.TotalReturnAmount,0) as TotalReturnAmount,
	ISNULL(sq.TotalExpenseAmount,0) as TotalExpenseAmount,
	ISNULL(sq.Latitude,0) as Latitude,
	ISNULL(sq.Longitude,0) as Longitude,
	ISNULL(sq.PhoneModel, '') AS PhoneModel,
	ISNULL(sq.PhoneOS, '') AS PhoneOS,
	ISNULL(sq.AppVersion, '') AS AppVersion,
	ISNULL(sq.TotalActivityCount, 0) AS TotalActivityCount,
	ISNULL(sq.CurrentLocTime, '2000-01-01') AS CurrentLocTime

	FROM cteAreaCodes cte
	FULL OUTER JOIN 
			(
				-- Employee Code and Area Codes, IsInFieldToday, IsRegisteredOnPhone
				SELECT sp.StaffCode, 
				ISNULL(oh.AreaCode, '***') AreaCode, 
				CASE WHEN ed.EmployeeId IS NULL THEN 0 ELSE 1 END IsInFieldToday,
				CASE WHEN te.Id IS NULL THEN 0 ELSE 1 END IsRegisteredOnPhone,

				ed.StartTime,
				ed.EndTime,
				ed.TotalOrderAmount,
				ed.TotalPaymentAmount,
				ed.TotalReturnAmount,
				ed.TotalExpenseAmount,
				ed.Latitude,
				ed.Longitude,
				ed.PhoneModel,
				ed.PhoneOS,
				ed.AppVersion,
				ed.TotalActivityCount,
				ed.CurrentLocTime

				FROM dbo.SalesPerson sp
				LEFT join dbo.OfficeHierarchy oh on sp.HQCode = oh.HQCode and oh.IsActive = 1
				LEFT JOIN dbo.TenantEmployee te on te.EmployeeCode = sp.StaffCode and te.IsActive = 1
				LEFT JOIN @SignedInEmployeeData ed on te.Id = ed.EmployeeId
				WHERE sp.IsActive = 1
			) sq ON cte.AreaCode = sq.AreaCode

  -- Cases covered
  -- 1. SalesPerson table may have a HQ code that does not exist in Office hierarchy
  --    We still want such a record in the resultset with Areacode as '***'
END
GO

-- 2.1.2019
IF OBJECT_ID ( 'dbo.ClearEmployeeData', 'P' ) IS NOT NULL   
    DROP PROCEDURE dbo.ClearEmployeeData;  
GO  

CREATE PROCEDURE [dbo].[ClearEmployeeData]
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
		DELETE FROM dbo.SqliteEntityWorkFlow WHERE  ActivityId in (SELECT ID FROM @activity)

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
		print '[SqliteEntity]'
		DELETE FROM dbo.[SqliteEntity] WHERE EmployeeId = @employeeId

		-- Delete SqliteLeave data
		print '[SqliteLeave]'
		DELETE FROM dbo.[SqliteLeave] WHERE EmployeeId = @employeeId

		-- Delete SqliteCancelledLeave data
		print '[SqliteCancelledLeave]'
		DELETE FROM dbo.[SqliteCancelledLeave] WHERE EmployeeId = @employeeId
		
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

-- 4.1.2019
-- support for multiple location
ALTER TABLE [dbo].[SqliteAction]
ADD [LocationCount] INT NOT NULL DEFAULT 0,
	[DerivedLocSource] NVARCHAR(50) NULL,
	[DerivedLatitude] DECIMAL(19,9) NOT NULL DEFAULT 0,
    [DerivedLongitude] DECIMAL(19,9) NOT NULL DEFAULT 0
GO

CREATE TABLE [dbo].[SqliteActionLocation]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [SqliteActionId] BIGINT NOT NULL REFERENCES [SqliteAction]([Id]), 
	[Source] NVARCHAR(50) NOT NULL,
    [Latitude] DECIMAL(19,9) NOT NULL,
    [Longitude] DECIMAL(19,9) NOT NULL,
	[UtcAt] DATETIME2 NOT NULL,
	[LocationTaskStatus] NVARCHAR(50) NULL, 
    [LocationException] NVARCHAR(256) NULL, 
	[IsGood] BIT NOT NULL DEFAULT 0
)
GO

ALTER TABLE [dbo].[TenantEmployee]
ADD	[LocationFromType] NVARCHAR(50) NULL -- Xamarin/GPS/Network/Auto
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
		DELETE FROM dbo.SqliteEntityWorkFlow WHERE  ActivityId in (SELECT ID FROM @activity)

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
		print '[SqliteEntity]'
		DELETE FROM dbo.[SqliteEntity] WHERE EmployeeId = @employeeId

		-- Delete SqliteLeave data
		print '[SqliteLeave]'
		DELETE FROM dbo.[SqliteLeave] WHERE EmployeeId = @employeeId

		-- Delete SqliteCancelledLeave data
		print '[SqliteCancelledLeave]'
		DELETE FROM dbo.[SqliteCancelledLeave] WHERE EmployeeId = @employeeId
		
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

ALTER TABLE [dbo].[PurgeLog]
ADD [EntityPurged] BIGINT NOT NULL DEFAULT 0
GO

ALTER PROCEDURE [dbo].[PurgeStagingData]
	@nukeFromDate DateTime,
	@nukeUptoDate DateTime
AS
BEGIN
	DECLARE @BatchIds Table ( BatchId BIGINT )
	DECLARE @ActionPurged BIGINT = 0
	DECLARE @ActionDupPurged BIGINT = 0
	DECLARE @ExpensePurged BIGINT = 0
	DECLARE @OrderPurged BIGINT = 0
	DECLARE @PaymentPurged BIGINT = 0
	DECLARE @ReturnPurged BIGINT = 0
	DECLARE @EntityPurged BIGINT = 0

	INSERT INTO @BatchIds
	(BatchId)
	SELECT ID FROM dbo.SqliteActionbatch
	WHERE BatchProcessed = 1 and LockTimestamp is null and UnderConstruction = 0
	and at >= @nukeFromDate and at <= @nukeUptoDate

	-- DELETE Action Image
	DELETE FROM dbo.SqliteActionImage
	WHERE SqliteActionId In 
	 ( SELECT id FROM dbo.SqliteAction a
	   JOIN @BatchIds b on b.BatchId = a.BatchId
	 )

    -- DELETE SqliteActionContact
	DELETE FROM dbo.SqliteActionContact
	WHERE SqliteActionId In 
	 ( SELECT id FROM dbo.SqliteAction a
	   JOIN @BatchIds b on b.BatchId = a.BatchId
	 )

	-- DELETE SqliteActionLocation
	DELETE FROM dbo.SqliteActionLocation
	WHERE SqliteActionId In 
	 ( SELECT id FROM dbo.SqliteAction a
	   JOIN @BatchIds b on b.BatchId = a.BatchId
	 )

	-- DELETE Action
	DELETE FROM dbo.SqliteAction 
	WHERE BatchId in (SELECT BatchId FROM @BatchIds)
	SET @ActionPurged = @@ROWCOUNT


	-- DELETE Action Dup
	DELETE FROM dbo.SqliteActionDup
	WHERE BatchId in (SELECT BatchId FROM @BatchIds)
	SET @ActionDupPurged = @@ROWCOUNT

	-- DELETE Expense Image
	DELETE FROM dbo.SqliteExpenseImage
	WHERE SqliteExpenseId In 
	 ( SELECT id FROM dbo.SqliteExpense a
	   JOIN @BatchIds b on b.BatchId = a.BatchId
	 )

	-- DELETE Expense
	DELETE FROM dbo.SqliteExpense
	WHERE BatchId in (SELECT BatchId FROM @BatchIds)
	SET @ExpensePurged = @@ROWCOUNT

	-- DELETE Payment Image
	DELETE FROM dbo.SqlitePaymentImage
	WHERE SqlitePaymentId In 
	 ( SELECT id FROM dbo.SqlitePayment a
	   JOIN @BatchIds b on b.BatchId = a.BatchId
	 )

	-- DELETE Payment
	DELETE FROM dbo.SqlitePayment
	WHERE BatchId in (SELECT BatchId FROM @BatchIds)
	SET @PaymentPurged = @@ROWCOUNT


	-- DELETE OrderItem
	DELETE FROM dbo.SqliteOrderItem
	WHERE SqliteOrderId In 
	 ( SELECT id FROM dbo.SqliteOrder a
	   JOIN @BatchIds b on b.BatchId = a.BatchId
	 )

	-- DELETE Order
	DELETE FROM dbo.SqliteOrder
	WHERE BatchId in (SELECT BatchId FROM @BatchIds)
	SET @OrderPurged = @@ROWCOUNT


	-- Delete ReturnOrderItem
	DELETE FROM dbo.SqliteReturnOrderItem
	WHERE SqliteReturnOrderId In 
	 ( SELECT id FROM dbo.SqliteReturnOrder a
	   JOIN @BatchIds b on b.BatchId = a.BatchId
	 )

	-- DELETE Return
	DELETE FROM dbo.SqliteReturnOrder
	WHERE BatchId in (SELECT BatchId FROM @BatchIds)
	SET @ReturnPurged = @@ROWCOUNT

	-- DELETE Entity Image

	DELETE FROM dbo.SqliteEntityImage
	WHERE SqliteEntityId In 
	 ( SELECT id FROM dbo.SqliteEntity a
	   JOIN @BatchIds b on b.BatchId = a.BatchId
	 )

    -- DELETE SqliteEntityContact
	DELETE FROM dbo.SqliteEntityContact
	WHERE SqliteEntityId In 
	 ( SELECT id FROM dbo.SqliteEntity a
	   JOIN @BatchIds b on b.BatchId = a.BatchId
	 )

	-- DELETE SqliteEntityCrop
	DELETE FROM dbo.SqliteEntityCrop
	WHERE SqliteEntityId In 
	 ( SELECT id FROM dbo.SqliteEntity a
	   JOIN @BatchIds b on b.BatchId = a.BatchId
	 )

	-- DELETE SqliteEntity
	DELETE FROM dbo.SqliteEntity
	WHERE BatchId in (SELECT BatchId FROM @BatchIds)
	SET @EntityPurged = @@ROWCOUNT


	INSERT INTO dbo.PurgeLog
	(DateFrom, DateTo, ActionPurged, ActionDupPurged, ExpensePurged, OrderPurged, PaymentPurged, ReturnPurged, EntityPurged)
	VALUES
	(
		@nukeFromDate, 
		@nukeUptoDate, 
		@ActionPurged,
		@ActionDupPurged,
		@ExpensePurged,
		@OrderPurged,
		@PaymentPurged,
		@ReturnPurged,
		@EntityPurged
	)
END
GO

-- 17.1.2019
CREATE INDEX [IX_SqliteAction_BatchId]
	ON [dbo].[SqliteAction]
	(BatchId)
go

-- drop this index first and then recreate it with new included column
DROP INDEX [IX_Tracking_DistanceCalculated] ON dbo.Tracking
GO

CREATE INDEX [IX_Tracking_DistanceCalculated]
	ON [dbo].[Tracking]
	([DistanceCalculated])
	INCLUDE ([IsMileStone], [LockTimestamp])
go

-- 26.1.2019
UPDATE dbo.CodeTable
set CodeName = 'Aadhar'
where codeType = 'CustomerType'
and CodeValue = 'Farmer'
and IsActive = 1

go

ALTER TABLE [dbo].[SqliteEntity]
ADD
	[UniqueIdType] NVARCHAR(50) NOT NULL DEFAULT '',
	[UniqueId] NVARCHAR(50) NOT NULL DEFAULT '',
	[TaxId] NVARCHAR(50) NOT NULL DEFAULT '',
	[LocationCount] INT NOT NULL DEFAULT 0,

	[DerivedLocSource] NVARCHAR(50) NULL,
	[DerivedLatitude] DECIMAL(19,9) NOT NULL DEFAULT 0,
    [DerivedLongitude] DECIMAL(19,9) NOT NULL DEFAULT 0
GO

ALTER TABLE dbo.Entity
ADD	[UniqueIdType] NVARCHAR(50) NOT NULL DEFAULT '',
	[UniqueId] NVARCHAR(50) NOT NULL DEFAULT '',
	[TaxId] NVARCHAR(50) NOT NULL DEFAULT ''
GO

CREATE TABLE [dbo].[SqliteEntityLocation]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [SqliteEntityId] BIGINT NOT NULL REFERENCES [SqliteEntity]([Id]), 
	[Source] NVARCHAR(50) NOT NULL,
    [Latitude] DECIMAL(19,9) NOT NULL,
    [Longitude] DECIMAL(19,9) NOT NULL,
	[UtcAt] DATETIME2 NOT NULL,
	[LocationTaskStatus] NVARCHAR(50) NULL, 
    [LocationException] NVARCHAR(256) NULL, 
	[IsGood] BIT NOT NULL DEFAULT 0
)
go

ALTER TABLE dbo.Entity
DROP COLUMN MNC, MCC, LAC, CellId
go


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
		DELETE FROM dbo.SqliteEntityWorkFlow WHERE  ActivityId in (SELECT ID FROM @activity)

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

	-- now we need to update the id in SqliteEntity table
	UPDATE dbo.SqliteEntity
	SET EntityId = e.Id,
	IsProcessed = 1,
	DateUpdated = SYSUTCDATETIME()
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
END
GO

