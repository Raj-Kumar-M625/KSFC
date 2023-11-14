CREATE PROCEDURE [dbo].[SMS_GetEndDayStaffCode]
	@tenantId BIGINT,
	@runDate DATE
AS
BEGIN
	-- first create a row in [Day] table, if not already there
	IF NOT EXISTS( SELECT 1 FROM dbo.[Day] WHERE [Date] = @runDate )
	BEGIN
		INSERT INTO dbo.[Day] ([DATE])
		SELECT @runDate
	END

	-- get the list of staff codes who started the day but did not end it
	SELECT te.EmployeeCode
	FROM dbo.TenantEmployee te
	INNER JOIN dbo.SalesPerson sp ON te.EmployeeCode = sp.StaffCode
	      AND te.IsActive = 1 
	      AND sp.IsActive = 1
		  AND te.TenantId = @tenantId
	INNER JOIN dbo.[Day] d ON d.[Date] = @runDate
	INNER JOIN dbo.EmployeeDay ed ON te.Id = ed.TenantEmployeeId
	      AND ed.DayId = d.Id
		  AND ed.EndTime IS NULL
END
