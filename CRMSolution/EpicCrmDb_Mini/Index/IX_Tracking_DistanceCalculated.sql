CREATE INDEX [IX_Tracking_DistanceCalculated]
	ON [dbo].[Tracking]
	([DistanceCalculated])
	INCLUDE ([IsMileStone], [LockTimestamp])
