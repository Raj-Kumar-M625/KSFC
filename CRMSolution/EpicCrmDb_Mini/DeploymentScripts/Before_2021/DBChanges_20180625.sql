drop index [IX_Tracking_EmployeeDayId] on dbo.tracking
go

CREATE INDEX [IX_Tracking_EmployeeDayId]
	ON [dbo].[Tracking]
	(EmployeeDayId)
	INCLUDE ([GoogleMapsDistanceInMeters], IsMilestone)
go

CREATE INDEX [IX_Tracking_ActivityId]
ON [dbo].[Tracking]
(ActivityId)
