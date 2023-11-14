CREATE TABLE [dbo].[SqliteTaskAction]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[BatchId] BIGINT NOT NULL REFERENCES  [SqliteActionBatch]([Id]),
	[EmployeeId] BIGINT NOT NULL,
	[IsNewTask] BIT NOT NULL DEFAULT 0,
	[TaskId] BIGINT NOT NULL,
	[ParentReferenceTaskId]  NVARCHAR(50) NOT NULL DEFAULT '', --GUID for new Task created on phone else Task_Id# 
	[TaskAssignmentId] BIGINT NOT NULL,
	[SqliteActionPhoneDbId] NVARCHAR(50) NOT NULL,

	[Status] NVARCHAR(50) NOT NULL,
	[NotificationDate] DATETIME2 NOT NULL,
	[TimeStamp] DATETIME2 NOT NULL,

	[PhoneDbId] NVARCHAR(50) NOT NULL,
	[TaskActionId] BIGINT NOT NULL DEFAULT 0,
	[IsProcessed] BIT NOT NULL DEFAULT 0,  
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
)
GO