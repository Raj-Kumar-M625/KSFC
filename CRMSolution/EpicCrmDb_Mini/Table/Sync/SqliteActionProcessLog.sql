CREATE TABLE [dbo].[SqliteActionProcessLog]
(
	-- table to log action data processing requests - received
	-- either from task manager or from user
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[TenantId] BIGINT NOT NULL,
	[EmployeeId] BIGINT NOT NULL,
	[LockAcquiredStatus] BIT NOT NULL,
	[HasCompleted] BIT NOT NULL DEFAULT 0,
	[HasFailed] BIT NOT NULL DEFAULT 0,
	[At] DATETIME2 NOT NULL DEFAULT SysutcDateTime(),
	[Timestamp] DATETIME2 NOT NULL DEFAULT SysutcDateTime()
)
