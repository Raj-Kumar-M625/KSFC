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
