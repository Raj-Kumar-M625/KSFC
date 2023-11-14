CREATE PROCEDURE [dbo].[sp_DashboardData]
	@reportStartDate DATE,
	@reportEndDate DATE
AS
BEGIN
	SELECT D.[Date], 
	ed.TenantEmployeeId,
	ed.Id EmployeeDayId, 
	-- always sum up the distance for reporting
	-- as employee Day has total distance stored only at end of day
	TotalDistanceInMetersAtMilestones = (SELECT ISNULL(Sum([GoogleMapsDistanceInMeters]),0) 
							FROM dbo.Tracking 
							WHERE EmployeeDayId = ed.Id and [GoogleMapsDistanceInMeters] > 0
							AND IsMilestone = 1
							),
	TotalDistanceInMetersAfterLastMilestone = 0.0,
	--(
	--	SELECT ISNULL(SUM([LinearDistanceInMeters]), 0)
	--	FROM dbo.Tracking t
	--	WHERE EmployeeDayId = ed.Id and [LinearDistanceInMeters] > 0
	--	AND t.Id > (SELECT MAX(Id) FROM dbo.Tracking WHERE IsMilestone = 1 AND EmployeeDayId = ed.Id)
	--),
	ActivityCount = (SELECT COUNT(*) FROM dbo.Activity WHERE EmployeeDayId = ed.Id)
	FROM dbo.Day D
	INNER JOIN dbo.EmployeeDay ed on ed.DayId = D.Id
	WHERE d.DATE >= @reportStartDate and d.[Date] <= @reportEndDate
	ORDER BY D.DATE DESC
END

