CREATE TABLE [dbo].[DataFeedProcessLog]
(
	-- table to log data feed processing requests - received
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[TenantId] BIGINT NOT NULL,
	[ProcessName] NVARCHAR(50) NOT NULL DEFAULT 'DataFeed',
	[LockAcquiredStatus] BIT NOT NULL,
	[HasCompleted] BIT NOT NULL DEFAULT 0,
	[HasFailed] BIT NOT NULL DEFAULT 0,
	[At] DATETIME2 NOT NULL DEFAULT SysutcDateTime(),
	[Timestamp] DATETIME2 NOT NULL DEFAULT SysutcDateTime()
)
