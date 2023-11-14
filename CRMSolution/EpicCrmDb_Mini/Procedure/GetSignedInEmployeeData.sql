CREATE PROCEDURE [dbo].[GetSignedInEmployeeData]
	@inputDate DateTime2
AS
BEGIN
    -- Sproc to get latest coordinates that we have for people in the field
	-- on any given day.
	WITH employeeDataCTE(TrackingRecordId, TrackingTime, EmployeeId, EmployeeDayId, EmployeeName, 
						Latitude, Longitude, EmployeeCode, rownumber)
	AS
	(
	   SELECT t.Id, t.[At], te.Id, ed.Id, te.Name, t.EndGPSLatitude, t.EndGPSLongitude,
	   te.EmployeeCode,
	   ROW_NUMBER() OVER (Partition by t.EmployeeDayId Order by t.[At] DESC)
	   FROM dbo.[Day] d
	   INNER JOIN dbo.EmployeeDay ed on d.Id = ed.DayId
		AND d.[Date] = CAST(@inputDate AS [Date])
		INNER JOIN dbo.TenantEmployee te on te.Id = ed.TenantEmployeeId
	   INNER JOIN dbo.Tracking t on t.EmployeeDayId = ed.Id
   )
   SELECT TrackingRecordId, TrackingTime, EmployeeId, EmployeeDayId, EmployeeName, Latitude, Longitude, EmployeeCode
   FROM employeeDataCTE 
   WHERE rownumber = 1
END
