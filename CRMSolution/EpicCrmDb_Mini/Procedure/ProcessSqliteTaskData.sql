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