CREATE PROCEDURE [dbo].[SMS_GetStartDayStaffCode]
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

	-- get the list of staff codes who did not start their day
	SELECT te.EmployeeCode
	FROM dbo.TenantEmployee te
	INNER JOIN dbo.SalesPerson sp ON te.EmployeeCode = sp.StaffCode
	      AND te.IsActive = 1 
	      AND sp.IsActive = 1
		  AND te.TenantId = @tenantId
	INNER JOIN dbo.[Day] d ON d.[Date] = @runDate
	LEFT JOIN dbo.EmployeeDay ed ON te.Id = ed.TenantEmployeeId
	      AND ed.DayId = d.Id
		  AND ed.AppVersion <> '***'

	-- if I continued through mid night, and created activity after midnight
	-- I get an entry in employee day with app version = '***'
	-- this should not be treated as if I have started my day

	WHERE ed.TenantEmployeeId is null
END

