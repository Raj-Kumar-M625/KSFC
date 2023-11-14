
CREATE PROCEDURE [dbo].[ProcessSqliteLeaveData]
	@batchId BIGINT
AS
BEGIN
	-- if batch is already processed - return	
	IF NOT EXISTS(SELECT 1 FROM dbo.SqliteActionBatch 
			WHERE Id = @batchId AND NumberOfLeavesSaved > 0
			AND BatchProcessed = 0)
	BEGIN
		RETURN;
	END
	
	-- First create required records in Day Table
	INSERT INTO dbo.[Day]
	([DATE])
	SELECT DISTINCT CAST (se.[TimeStamp] AS [Date])
	FROM dbo.SqliteLeave se
	LEFT JOIN dbo.[Day] d on CAST(se.[TimeStamp] AS [Date]) = d.[DATE]
	WHERE se.BatchId = @batchId
	AND d.[DATE] IS NULL

	-- Create Leave Records
	INSERT INTO Leave
	(EmployeeId,EmployeeCode,StartDate,EndDate,Leavetype,LeaveReason,Comment,
	SqliteLeaveId,DaysCountExcludingHolidays,DaysCount,CreatedBy,UpdatedBy)
	SELECT sl.EmployeeId,te.EmployeeCode,CAST (sl.StartDate AS [Date]),CAST (sl.EndDate AS [Date]),
	sl.LeaveType,sl.LeaveReason,sl.Comment,sl.Id,sl.DaysCountExcludingHolidays,sl.DaysCount,te.[Name],te.[Name]
	FROM SqliteLeave sl
	INNER JOIN dbo.[TenantEmployee] te ON te.Id = sl.EmployeeId
	WHERE BatchId = @batchId AND sl.IsProcessed = 0
	ORDER BY sl.Id

	-- Now update Leave back in SqliteLeave
	Update dbo.SqliteLeave
	SET LeaveId = l.Id,
	IsProcessed = 1,
	DateUpdated = SYSUTCDATETIME()
	FROM dbo.SqliteLeave sl
	INNER JOIN dbo.[Leave] l on sl.Id = l.SqliteLeaveId
	AND sl.BatchId = @batchId

END
