ALTER PROCEDURE [dbo].[GetTrackingRecordsForDistanceCalculation]
	@recordCount int
AS
BEGIN

    DECLARE @currentDateTime DATETIME2 = SYSUTCDATETIME();

	UPDATE TOP(@recordCount) dbo.Tracking With (READPAST)
	SET LockTimestamp = @currentDateTime
	OUTPUT inserted.Id, inserted.BeginGPSLatitude, inserted.BeginGPSLongitude,
	       inserted.EndGPSLatitude, inserted.EndGPSLongitude,
		   inserted.IsMilestone, inserted.IsStartOfDay, inserted.IsEndOfDay
	WHERE IsMileStone = 1 AND ((DistanceCalculated = 0 AND LockTimestamp IS NULL) OR
	 (LockTimeStamp Is NOT NULL AND DATEDIFF(mi, LockTimestamp, @currentDateTime) >= 5))
END


