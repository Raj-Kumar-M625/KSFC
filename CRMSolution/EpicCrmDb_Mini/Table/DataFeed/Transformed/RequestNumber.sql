CREATE TABLE [dbo].[RequestNumber]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[Sequence] BIGINT NOT NULL,
	[RequestNumber] NVARCHAR(20) NOT NULL,
	[IsUsed] BIT NOT NULL DEFAULT 0,
	[UsedTimestamp] DATETIME2 NOT NULL DEFAULT SysUtcDateTime()
)
