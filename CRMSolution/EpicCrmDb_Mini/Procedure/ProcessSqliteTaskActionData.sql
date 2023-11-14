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