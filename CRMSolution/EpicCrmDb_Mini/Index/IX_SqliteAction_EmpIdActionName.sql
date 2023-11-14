CREATE /*UNIQUE*/ INDEX [IX_SqliteAction_EmpIdActionName]
	ON [dbo].[SqliteAction]
	(EmployeeId, [AT], ActivityTrackingType)
