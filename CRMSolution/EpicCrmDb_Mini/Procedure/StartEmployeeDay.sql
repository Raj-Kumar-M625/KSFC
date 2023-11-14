CREATE PROCEDURE [dbo].[StartEmployeeDay]
	@employeeId BIGINT,
	@startDateTime DateTime2,

    @PhoneModel NVARCHAR(100),
    @PhoneOS NVARCHAR(10),
    @AppVersion NVARCHAR(10),

	@employeeDayId BIGINT OUTPUT
AS
BEGIN
   -- Records start of the day activity
   -- First Selet DayId from dbo.Day table for start date time
   DECLARE @dayId BIGINT
   DECLARE @startDate DATE = CAST(@startDateTime AS [Date])

	IF EXISTS(SELECT 1 FROM dbo.[Day] WHERE [Date] = @startDate)
	BEGIN
		SELECT @dayId = Id FROM dbo.[Day] WHERE [Date] = @startDate
	END
	ELSE
	BEGIN
		INSERT INTO dbo.[Day] ([Date]) VALUES (@startDate)
		SET @dayId = SCOPE_IDENTITY()
	END

	SET @employeeDayId = 0

	-- check if a record already exist in Employee Day table.
	IF EXISTS(SELECT 1 FROM dbo.EmployeeDay WHERE TenantEmployeeId=@employeeId AND DayId = @dayId)
	BEGIN
		DECLARE @currentAppVersion NVARCHAR(10)
		SELECT @employeeDayId=Id,
		       @currentAppVersion = ISNULL(AppVersion, '***')
		FROM dbo.EmployeeDay WHERE TenantEmployeeId=@employeeId AND DayId = @dayId

		UPDATE dbo.EmployeeDay 
		SET EndTime = Null, TotalDistanceInMeters = 0,
		    HasMultipleStarts = 1,
			AppVersion = Case when @currentAppVersion='***' Then @AppVersion Else AppVersion END,
			PhoneModel = Case when @currentAppVersion='***' Then @PhoneModel Else PhoneModel END,
			PhoneOS = Case when @currentAppVersion='***' Then @PhoneOS Else PhoneOS END
		WHERE Id = @employeeDayId 
		-- for auto created/on need employee days, app version is set to *** or null
		-- hence when actual start day comes, we want to fill app version.
	END

	ELSE

	BEGIN
		-- pick up HQ Code from SalesPerson table
		DECLARE @hqCode VARCHAR(10)
		DECLARE @areaCode VARCHAR(10)

		-- Copy the hq code and area code in Employee Day table
		-- as later sales Person's HQ Code can change
		-- And For reporting purposes, we should be using the HQ Code
		-- that the person had on the date he worked.
		SELECT Top(1) @hqCode = ISNULL(oh.HQCode,''), @areaCode = ISNULL(oh.AreaCode, '')
		FROM dbo.TenantEmployee te 
		INNER JOIN dbo.SalesPerson sp on te.EmployeeCode = sp.StaffCode
		AND te.Id = @employeeId
		INNER JOIN dbo.OfficeHierarchy oh on sp.HQCode = oh.HQCode
		AND oh.IsActive = 1

		INSERT INTO dbo.EmployeeDay
		(TenantEmployeeId, DayId, StartTime, HQCode, AreaCode, PhoneModel, PhoneOS, AppVersion)
		VALUES
		(@employeeId, @dayId, @startDateTime, ISNULL(@hqCode, ''), ISNULL(@areaCode, ''),
			@PhoneModel, @PhoneOS, @AppVersion)

		SET @employeeDayId = SCOPE_IDENTITY()
	END
END

