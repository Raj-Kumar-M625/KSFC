CREATE PROCEDURE [dbo].[AddTrackingData]
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

