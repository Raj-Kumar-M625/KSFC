CREATE TABLE Leave
(
Id bigint Not Null  PRIMARY KEY,
EmployeeId bigint Not Null,
EmployeeCode bigint Not Null,
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
UpdatedBy nvarchar(50) not  null,
DateUpdated datetime2(7) not null DEFAULT SYSUTCDATETIME()
)
