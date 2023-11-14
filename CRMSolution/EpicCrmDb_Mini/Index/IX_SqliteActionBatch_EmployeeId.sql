CREATE INDEX [IX_SqliteActionBatch_EmployeeId]
	ON [dbo].[SqliteActionBatch]
	(EmployeeId)
	INCLUDE ([BatchGuid])
