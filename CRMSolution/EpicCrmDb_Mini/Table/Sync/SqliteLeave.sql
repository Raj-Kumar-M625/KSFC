CREATE TABLE [dbo].[SqliteLeave]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[BatchId] BIGINT NOT NULL REFERENCES dbo.SqliteActionBatch,
	[EmployeeId] BIGINT NOT NULL,
	[PhoneDbId] NVARCHAR(50) NOT NULL DEFAULT '',
	[StartDate] DATE NOT NULL,
	[EndDate] DATE NOT NULL,
	[LeaveType] NVARCHAR(50) NOT NULL,
	[LeaveReason] NVARCHAR(50) NOT NULL,
	[Comment] NVARCHAR(512),
	[DaysCountExcludingHolidays] int not null DEFAULT 0,
    [DaysCount] int not null DEFAULT 0,
    [TimeStamp] datetime2(7) not null DEFAULT SYSUTCDATETIME(),
	[IsProcessed] BIT NOT NULL DEFAULT 0,  -- non-unique indexed
	[LeaveId] BIGINT NOT NULL DEFAULT 0,
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
)
