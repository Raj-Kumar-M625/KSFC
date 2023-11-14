CREATE PROCEDURE [dbo].[EndEmployeeDay]
	@employeeDayId BIGINT,
	@endDateTime DateTime2,
	@status INT OUTPUT
AS
BEGIN
   -- Records end of the day activity
   DECLARE @returnStatus INT = -1
	-- check if a record already exist in Employee Day table
	IF EXISTS(SELECT 1 FROM dbo.EmployeeDay WHERE Id = @employeeDayId AND EndTime IS NULL)
	BEGIN
		UPDATE dbo.EmployeeDay 
		SET EndTime = @endDateTime
		--TotalDistanceInMeters = 
		--	(SELECT ISNULL(Sum(BingMapsDistanceInMeters),0) FROM dbo.Tracking WHERE EmployeeDayId = @employeeDayId)
		WHERE Id = @employeeDayId
		
		SET @returnStatus = 1
	END

	SELECT @status = @returnStatus
END

