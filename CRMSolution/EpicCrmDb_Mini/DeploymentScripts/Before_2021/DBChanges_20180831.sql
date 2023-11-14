ALTER TABLE [dbo].[SqliteActionBatch]
ADD [NumberOfLeaves] BIGINT NOT NULL DEFAULT 0,
	[NumberOfLeavesSaved] BIGINT NOT NULL DEFAULT 0,

	[NumberOfCancelledLeaves] BIGINT NOT NULL DEFAULT 0,
	[NumberOfCancelledLeavesSaved] BIGINT NOT NULL DEFAULT 0;
GO