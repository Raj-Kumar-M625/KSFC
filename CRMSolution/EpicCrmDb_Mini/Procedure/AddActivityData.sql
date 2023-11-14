CREATE PROCEDURE [dbo].[AddActivityData]
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
	@activityTrackingType INT,
	@activityId BIGINT OUTPUT
AS
BEGIN
   -- Records tracking activity
   SET @activityId = 0
	-- check if a record already exist in Employee Day table
	IF EXISTS(SELECT 1 FROM dbo.EmployeeDay WHERE Id = @employeeDayId)
	BEGIN
		INSERT into dbo.Activity
		(EmployeeDayId, ClientName, ClientPhone, ClientType, ActivityType, Comments, ImageCount, [At], ClientCode, ActivityAmount, AtBusiness, ContactCount, ActivityTrackingType)
		VALUES
		(@employeeDayId, @clientName, @clientPhone, @clientType, @activityType, @comments, @imageCount, @activityDateTime, @clientCode, @activityAmount, @atBusiness, @contactCount, @activityTrackingType)

		SET @activityId = SCOPE_IDENTITY()

		-- create a row in Activity Type if not already there
		IF NOT EXISTS(SELECT 1 FROM dbo.ActivityType WHERE ActivityName = @activityType)
		BEGIN
		    INSERT INTO dbo.ActivityType
			(ActivityName)
			VALUES
			(@activityType)
		END

		-- keep count of total activities for exec crm application
		UPDATE dbo.EmployeeDay
		SET TotalActivityCount = TotalActivityCount + 1
		WHERE Id = @employeeDayId
	END
END

