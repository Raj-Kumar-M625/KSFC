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