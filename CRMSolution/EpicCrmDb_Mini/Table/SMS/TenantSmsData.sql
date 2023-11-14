CREATE TABLE [dbo].[TenantSmsData]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[TenantId] BIGINT NOT NULL References dbo.Tenant,
	[TemplateName] NVARCHAR(50) NOT NULL,
	[DataType] NVARCHAR(10) NOT NULL, -- XML or JSON
	[MessageData] NVARCHAR(MAX) NOT NULL,
	[IsSent] BIT NOT NULL DEFAULT 0,
	[IsFailed] BIT NOT NULL DEFAULT 0,
	[FailedText] NVARCHAR(100),
	[LockTimestamp] DATETIME2,
	[CreatedOn] DATETIME2 NOT NULL DEFAULT SysutcDateTime(),
	[Timestamp] DATETIME2 NOT NULL DEFAULT SysutcDateTime()
)
