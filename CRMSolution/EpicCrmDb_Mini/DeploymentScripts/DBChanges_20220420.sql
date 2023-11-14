CREATE TABLE Leave
(
Id bigint Not Null  PRIMARY KEY,
EmployeeId bigint Not Null,
EmployeeCode nvarchar(50) Not Null,
StartDate date Not Null,
EndDate date Not Null,
LeaveType  nvarchar(50) not null,
LeaveReason nvarchar(50) not null,
Comment nvarchar(512) not null,
SqliteLeaveId bigint not null,
DaysCountExcludingHolidays int not null,
DaysCount int not null,
LeaveStatus nvarchar(50) Not NULL DEFAULT 'Pending',
ApproveNotes nvarchar(2048) DEFAULT NULL,
CreatedBy nvarchar(50) not null,
DateCreated datetime2(7) not null DEFAULT SYSUTCDATETIME(),
UpdatedBy nvarchar(50) not null,
DateUpdated datetime2(7) not null DEFAULT SYSUTCDATETIME(),
)
GO

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
GO

--Modify SqliteLeave Table
-- Drop constraints first 

ALTER TABLE SqliteLeave
DROP COLUMN IsHalfDayLeave
GO

ALTER TABLE SqliteLeave
ADD [DaysCountExcludingHolidays] int not null DEFAULT 0,
[DaysCount] int not null DEFAULT 0,
[TimeStamp] datetime2(7) not null DEFAULT SYSUTCDATETIME()
GO

UPDATE CodeTable
SET CodeName='CompOff Leave' , CodeValue='CompOff Leave'
WHERE CodeName='Comp. Off Leave'
GO
