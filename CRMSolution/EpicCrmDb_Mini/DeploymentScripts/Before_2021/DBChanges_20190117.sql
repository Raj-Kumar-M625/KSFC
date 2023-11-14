CREATE INDEX [IX_SqliteAction_BatchId]
	ON [dbo].[SqliteAction]
	(BatchId)
go

-- drop this index first and then recreate it with new included column
CREATE INDEX [IX_Tracking_DistanceCalculated]
	ON [dbo].[Tracking]
	([DistanceCalculated])
	INCLUDE ([IsMileStone], [LockTimestamp])
go