-- Table to add project details for followup task functionality

CREATE TABLE [dbo].[Project]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(50) NOT NULL,
	[Description]  NVARCHAR(200),

	[PlannedStartDate] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[PlannedEndDate] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[ActualStartDate] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[ActualEndDate] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[ProjectCategory] NVARCHAR(50) NOT NULL,

	[Status] NVARCHAR(50) NOT NULL DEFAULT 'Open',
	[CyclicCount] INT NOT NULL DEFAULT 0,
	[IsActive] BIT NOT NULL DEFAULT 0,

	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[CreatedBy] NVARCHAR(50) NOT NULL,
	[UpdatedBy] NVARCHAR(50) NOT NULL
)
GO

INSERT INTO [dbo].[CodeTable] 
("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive", "TenantId") 
VALUES 
('ProjectStatus', '', 'Open', 10, 'True', 1),
('ProjectStatus', '', 'InProgress', 20, 'True', 1),
('ProjectStatus', '', 'Completed', 30, 'True', 1);

INSERT INTO [dbo].[CodeTable] 
("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive", "TenantId") 
VALUES 
('ProjectCategory', '', 'Default', 10, 'True', 1),
('ProjectCategory', '', 'Development', 20, 'True', 1),
('ProjectCategory', '', 'Maintenance', 30, 'True', 1);

ALTER TABLE [dbo].[FeatureControl]
ADD
	[ProjectOption] BIT NOT NULL DEFAULT 0,
	[FollowUpTaskOption] BIT NOT NULL DEFAULT 0
GO


-- Tasks and Followup Functionality

INSERT INTO [dbo].[CodeTable] 
("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive", "TenantId") 
VALUES 
('TaskStatus', '', 'Open', 10, 'True', 1),
('TaskStatus', '', 'Completed', 20, 'True', 1),
('TaskStatus', '', 'On-Hold', 30, 'True', 1),
('TaskStatus', '', 'Cancelled', 40, 'True', 1);

--INSERT INTO [dbo].[CodeTable] 
--("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive", "TenantId") 
--VALUES 
--('TaskType', '', 'Self-Assigned', 10, 'True', 1),
--('TaskType', '', 'Manager', 20, 'True', 1);


-- Inserting a default project for all tasks 

INSERT INTO dbo.Project VALUES ('Default', 'Default all task will be assigned to this project', '2021-09-18 00:00:00.0000000', '2099-01-01 00:00:00.0000000', '2021-09-18 00:00:00.0000000', '2099-09-18 00:00:00.0000000', 'Default',
'Open', 1, 'True', '2021-09-18 00:00:00.0000000', '2021-09-18 00:00:00.0000000', 'SuperAdmin', 'SuperAdmin');



CREATE TABLE [dbo].[ProjectAssignment]
(
	[Id] INT NOT NULL PRIMARY KEY Identity,
	[XRefProjectId] BIGINT NOT NULL REFERENCES dbo.Project([Id]),
	[EmployeeId] BIGINT NOT NULL References dbo.TenantEmployee(Id),
	
	[StartDate] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[EndDate] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[IsAssigned] BIT NOT NULL DEFAULT 1,
	[IsSelfAssigned] BIT NOT NULL DEFAULT 0,
	[Comments] NVARCHAR(2048) NOT NULL,
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[CreatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[UpdatedBy] NVARCHAR(50) NOT NULL DEFAULT ''
)
GO

CREATE TABLE [dbo].[ProjectAudit]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[XRefProjectId] BIGINT NOT NULL,
	[Name] NVARCHAR(50) NOT NULL,
	[Description]  NVARCHAR(200),

	[PlannedStartDate] DATETIME2 NOT NULL,
	[PlannedEndDate] DATETIME2 NOT NULL,
	[ActualStartDate] DATETIME2 NOT NULL,
	[ActualEndDate] DATETIME2 NOT NULL,
	[ProjectCategory] NVARCHAR(50) NOT NULL,

	[Status] NVARCHAR(50) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT 0,

	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[CreatedBy] NVARCHAR(50) NOT NULL
)
GO

CREATE TABLE [dbo].[Task]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[XRefProjectId] BIGINT NOT NULL REFERENCES dbo.Project([Id]),
	[Description]  NVARCHAR(200),

	[ActivityType] NVARCHAR(50) NOT NULL,
	[ClientType] NVARCHAR(50) NOT NULL,
	[ClientName] NVARCHAR(50) NOT NULL,
	[ClientCode] NVARCHAR(50) NOT NULL DEFAULT '',

	[PlannedStartDate] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[PlannedEndDate] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[ActualStartDate] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[ActualEndDate] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[Comments] NVARCHAR(2048) NOT NULL,

	[Status] NVARCHAR(50) NOT NULL,
	[CyclicCount] INT NOT NULL DEFAULT 0,
	[IsActive] BIT NOT NULL DEFAULT 0,

	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[CreatedBy] NVARCHAR(50) NOT NULL,
	[UpdatedBy] NVARCHAR(50) NOT NULL,
	[SqliteTaskId] BIGINT NOT NULL DEFAULT 0,
	[TimeStamp] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
)
GO

CREATE TABLE [dbo].[TaskAssignment]
(
	[Id] INT NOT NULL PRIMARY KEY Identity,
	[XRefTaskId] BIGINT NOT NULL REFERENCES dbo.Task([Id]),
	[EmployeeId] BIGINT NOT NULL References dbo.TenantEmployee(Id),
	
	[StartDate] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[EndDate] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[IsAssigned] BIT NOT NULL DEFAULT 1,
	[IsSelfAssigned] BIT NOT NULL DEFAULT 0,
	[Comments] NVARCHAR(2048) NOT NULL,
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[CreatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[UpdatedBy] NVARCHAR(50) NOT NULL DEFAULT ''
)
GO

CREATE TABLE [dbo].[TaskAction]
(
	[Id] INT NOT NULL PRIMARY KEY Identity,
	[XRefTaskId] BIGINT NOT NULL REFERENCES dbo.Task([Id]),
	[XRefActivityId] BIGINT NOT NULL DEFAULT 0,
	[XRefTaskAssignmentId] BIGINT NOT NULL DEFAULT 0,
	[EmployeeId] BIGINT NOT NULL References dbo.TenantEmployee(Id),
	
	[TimeStamp] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[SqliteTaskActionId] BIGINT NOT NULL DEFAULT 0,
	[Status] NVARCHAR(50) NOT NULL,

	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[CreatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[UpdatedBy] NVARCHAR(50) NOT NULL DEFAULT ''
)
GO


CREATE TABLE [dbo].[TaskAudit]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[XRefTaskId] BIGINT NOT NULL,
	[XRefTaskProjectId] BIGINT NOT NULL,
	[Description]  NVARCHAR(200),

	[ActivityType] NVARCHAR(50) NOT NULL,
	[ClientType] NVARCHAR(50) NOT NULL,
	[ClientName] NVARCHAR(50) NOT NULL,
	[ClientCode] NVARCHAR(50) NOT NULL DEFAULT '',

	[PlannedStartDate] DATETIME2 NOT NULL,
	[PlannedEndDate] DATETIME2 NOT NULL,
	[ActualStartDate] DATETIME2 NOT NULL,
	[ActualEndDate] DATETIME2 NOT NULL,
	[Comments] NVARCHAR(2048) NOT NULL,

	[Status] NVARCHAR(50) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT 0,

	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[CreatedBy] NVARCHAR(50) NOT NULL,
)
GO

CREATE TABLE [dbo].[SqliteTask]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[BatchId] BIGINT NOT NULL REFERENCES  [SqliteActionBatch]([Id]),
	[EmployeeId] BIGINT NOT NULL,

	[IsNewEntity] BIT NOT NULL DEFAULT 0,
	[ParentReferenceId]  NVARCHAR(50) NOT NULL DEFAULT '',
	[ProjectName] NVARCHAR(50) NOT NULL DEFAULT 'Default',
	[Description]  NVARCHAR(200),

	[ActivityType] NVARCHAR(50) NOT NULL,
	[ClientType] NVARCHAR(50) NOT NULL,
	[ClientName] NVARCHAR(50) NOT NULL,
	[ClientCode] NVARCHAR(50) NOT NULL DEFAULT '',

	[PlannedStartDate] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[PlannedEndDate] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[Comments] NVARCHAR(2048) NOT NULL,

	[Status] NVARCHAR(50) NOT NULL,
	[NotificationDate] DATETIME2 NOT NULL,
	[TimeStamp] DATETIME2 NOT NULL,

	[PhoneDbId] NVARCHAR(50) NOT NULL,
	[TaskId] BIGINT NOT NULL DEFAULT 0,
	[TaskAssignmentId] BIGINT NOT NULL DEFAULT 0,

	[IsProcessed] BIT NOT NULL DEFAULT 0,  
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
)
GO

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

ALTER TABLE [dbo].[SqliteActionBatch]
ADD
	[Task] BIGINT NOT NULL DEFAULT 0,
	[TaskSaved] BIGINT NOT NULL DEFAULT 0,
	[TaskAction] BIGINT NOT NULL DEFAULT 0,
	[TaskActionSaved] BIGINT NOT NULL DEFAULT 0
GO

ALTER TABLE [dbo].[Task]
ADD
	[CreatedOnPhone] BIT NOT NULL DEFAULT 0
GO

ALTER TABLE [dbo].[SqliteTask]
ADD
	[ProjectId] BIGINT NOT NULL DEFAULT 1
GO
-- ProcessSqliteTaskData
-- ProcessSqliteTaskActionData

CREATE PROCEDURE [dbo].[ProcessSqliteTaskData]
	@batchId BIGINT
AS
BEGIN
	-- Kartik Oct 10 2021

	-- if batch is already processed - return	
	IF NOT EXISTS(SELECT 1 FROM dbo.SqliteActionBatch 
			WHERE Id = @batchId AND TaskSaved > 0
			AND BatchProcessed = 0)
	BEGIN
		RETURN;
	END

	-- First create required records in Day Table
	INSERT INTO dbo.[Day]
	([DATE])
	SELECT DISTINCT CAST(e.[TimeStamp] AS [Date])
	FROM dbo.SqliteTask e
	LEFT JOIN dbo.[Day] d on CAST(e.[TimeStamp] AS [Date]) = d.[DATE]
	WHERE e.BatchId = @batchId
	AND d.[DATE] IS NULL

	DECLARE @insertTable TABLE (TaskId BIGINT, SqliteTaskId BIGINT, CreatedBy NVARCHAR(50))

	-- Fill Entity Id in SqliteTask that belong to new entity created on phone.
	Update dbo.SqliteTask
	SET ClientCode = en.EntityId,
	ClientName = en.EntityName,
	DateUpdated = SYSUTCDATETIME()
	FROM dbo.SqliteTask st
	INNER JOIN dbo.SqliteEntity en on st.ParentReferenceId = en.PhoneDbId
	AND st.BatchId = @batchId
	AND st.IsNewEntity = 1
	AND st.IsProcessed = 0

	-- Create FollowupTask  Records
	INSERT INTO dbo.[Task]
	(XRefProjectId, [Description], ActivityType, ClientType ,ClientName, ClientCode, PlannedStartDate, PlannedEndDate, ActualStartDate, ActualEndDate, Comments, [Status], CyclicCount, IsActive, CreatedBy, UpdatedBy, SqliteTaskId, CreatedOnPhone)
	OUTPUT inserted.Id, inserted.SqliteTaskId, inserted.CreatedBy INTO @insertTable
	SELECT p.[Id], st.[Description], st.ActivityType, st.ClientType, st.ClientName, st.ClientCode, st.PlannedStartDate, st.PlannedEndDate, SYSUTCDATETIME(), SYSUTCDATETIME(), st.Comments, st.[Status], 1, 1, t.EmployeeCode, t.EmployeeCode, st.[Id], 1
	FROM dbo.SqliteTask st
	INNER JOIN dbo.[Day] d ON d.[DATE] = CAST(st.[TimeStamp] AS [Date])
	INNER JOIN dbo.[Project] p ON p.[Id] = st.ProjectId
	INNER JOIN dbo.[TenantEmployee] t ON st.EmployeeId = t.Id
	WHERE st.BatchId = @batchId AND st.IsProcessed = 0
	ORDER BY st.Id



	-- Create new record for FollowUpTask assignment
	INSERT INTO dbo.[TaskAssignment]
	(XRefTaskId, EmployeeId, StartDate, EndDate, IsAssigned, IsSelfAssigned, Comments, CreatedBy, UpdatedBy)
	SELECT m.TaskId, st.EmployeeId, st.PlannedStartDate, st.PlannedEndDate, 1, 1, '',  m.CreatedBy, m.CreatedBy
	FROM dbo.SqliteTask st
	INNER JOIN @insertTable m ON st.Id = m.SqliteTaskId


	-- Now update TaskId back in SqliteTask table
	Update dbo.SqliteTask
	SET TaskId = t.Id,
	TaskAssignmentId = ta.Id,
	IsProcessed = 1,
	DateUpdated = SYSUTCDATETIME()
	FROM dbo.SqliteTask s
	INNER JOIN dbo.[Task] t on s.Id = t.SqliteTaskId
	INNER JOIN dbo.[TaskAssignment] ta ON t.Id = ta.XRefTaskId 
	AND s.BatchId = @batchId

END
GO



CREATE PROCEDURE [dbo].[ProcessSqliteTaskActionData]
	@batchId BIGINT
AS
BEGIN
	-- Kartik Oct 10 2021

	-- if batch is already processed - return	
	IF NOT EXISTS(SELECT 1 FROM dbo.SqliteActionBatch 
			WHERE Id = @batchId AND TaskAction > 0
			AND BatchProcessed = 0)
	BEGIN
		RETURN;
	END

	-- First create required records in Day Table
	INSERT INTO dbo.[Day]
	([DATE])
	SELECT DISTINCT CAST(e.[TimeStamp] AS [Date])
	FROM dbo.SqliteTaskAction e
	LEFT JOIN dbo.[Day] d on CAST(e.[TimeStamp] AS [Date]) = d.[DATE]
	WHERE e.BatchId = @batchId
	AND d.[DATE] IS NULL

	-- Fill Task Id in SqliteTaskAction that belong to new Task created on phone.
	Update dbo.SqliteTaskAction
	SET TaskId = st.TaskId,
	TaskAssignmentId = st.TaskAssignmentId,
	DateUpdated = SYSUTCDATETIME()
	FROM dbo.SqliteTaskAction sta
	INNER JOIN dbo.SqliteTask st on sta.ParentReferenceTaskId = st.PhoneDbId
	AND sta.BatchId = @batchId
	AND sta.IsNewTask = 1
	AND sta.IsProcessed = 0

	-- Create TaskAction  Records
	INSERT INTO dbo.[TaskAction]
	(XRefTaskId, XRefActivityId, XRefTaskAssignmentId, EmployeeId, [Status], SqliteTaskActionId, [TimeStamp], CreatedBy, UpdatedBy)
	SELECT sta.TaskId, sqa.ActivityId, sta.TaskAssignmentId,  sta.EmployeeId, sta.[Status], sta.Id, sta.[TimeStamp], 'ProcessSqliteSurveyData', 'ProcessSqliteSurveyData'
	FROM dbo.SqliteTaskAction sta
	INNER JOIN dbo.[Day] d ON d.[DATE] = CAST(sta.[TimeStamp] AS [Date])
	--INNER JOIN dbo.[SqliteTask] st ON sta.ParentReferenceTaskId = st.PhoneDbId
	INNER JOIN dbo.SqliteAction sqa ON sqa.EmployeeId = sta.EmployeeId
		AND sqa.PhoneDbId = sta.SqliteActionPhoneDbId
	WHERE sta.BatchId = @batchId AND sta.IsProcessed = 0
	ORDER BY sta.Id

	
	--- Now Update Task Status as send in TaskAction Batch
	Update dbo.Task
	SET [Status] = ta.[Status]
	FROM dbo.SqliteTaskAction sa
	INNER JOIN dbo.[TaskAction] ta ON sa.Id = ta.SqliteTaskActionId
	INNER JOIN dbo.[Task] t ON ta.XRefTaskId = t.Id
	AND sa.BatchId = @batchId


	-- Now update TaskActionId back in SqliteTaskAction table
	Update dbo.SqliteTaskAction
	SET TaskActionId = ta.Id,
	IsProcessed = 1,
	DateUpdated = SYSUTCDATETIME()
	FROM dbo.SqliteTaskAction sa
	INNER JOIN dbo.[TaskAction] ta on sa.Id = ta.SqliteTaskActionId
	AND sa.BatchId = @batchId

END
GO



 