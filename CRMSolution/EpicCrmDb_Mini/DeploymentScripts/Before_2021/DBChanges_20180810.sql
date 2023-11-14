Alter table dbo.Tenant
ADD	[MaxDiscountPercentage] DECIMAL(5,2) NOT NULL DEFAULT 7.0,
	[DiscountType] NVARCHAR(50) NOT NULL DEFAULT 'Amount'

--------

-- New table for leaves
CREATE TABLE [dbo].[SqliteLeave]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[BatchId] BIGINT NOT NULL REFERENCES dbo.SqliteActionBatch,
	[EmployeeId] BIGINT NOT NULL,
	[PhoneDbId] NVARCHAR(50) NOT NULL DEFAULT '',
	[IsHalfDayLeave] BIT NOT NULL DEFAULT 0,
	[StartDate] DATE NOT NULL,
	[EndDate] DATE NOT NULL,
	[LeaveType] NVARCHAR(50) NOT NULL,
	[LeaveReason] NVARCHAR(50) NOT NULL,
	[Comment] NVARCHAR(512),
	[IsProcessed] BIT NOT NULL DEFAULT 0,  -- non-unique indexed
	[LeaveId] BIGINT NOT NULL DEFAULT 0,
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
)
go

CREATE TABLE [dbo].[SqliteCancelledLeave]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[BatchId] BIGINT NOT NULL REFERENCES dbo.SqliteActionBatch,
	[EmployeeId] BIGINT NOT NULL,
	[LeaveId] BIGINT NOT NULL,
	[IsProcessed] BIT NOT NULL DEFAULT 0,  -- non-unique indexed
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
)
go


