CREATE INDEX [IX_Tracking_EmployeeDayId]
	ON [dbo].[Tracking]
	(EmployeeDayId)
	INCLUDE ([GoogleMapsDistanceInMeters], IsMilestone, Id, IsStartOfDay, IsEndOfDay, [at], endLocationName)
