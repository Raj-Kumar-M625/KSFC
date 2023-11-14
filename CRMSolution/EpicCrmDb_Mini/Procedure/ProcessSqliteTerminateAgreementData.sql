CREATE PROCEDURE [dbo].[ProcessSqliteTerminateAgreementData]
	@batchId BIGINT
AS
BEGIN

	-- if batch is already processed - return	
	IF NOT EXISTS(SELECT 1 FROM dbo.SqliteActionBatch 
			WHERE Id = @batchId AND TerminateRequestsSaved > 0
			AND BatchProcessed = 0)
	BEGIN
		RETURN;
	END

	-- First create required records in Day Table
	INSERT INTO dbo.[Day]
	([DATE])
	SELECT DISTINCT CAST(e.[RequestDate] AS [Date])
	FROM dbo.SqliteTerminateAgreement e
	LEFT JOIN dbo.[Day] d on CAST(e.[RequestDate] AS [Date]) = d.[DATE]
	WHERE e.BatchId = @batchId
	AND d.[DATE] IS NULL

	-- SqliteTerminateAgreement can only be created for approved agreements

	-- select current max Id from dbo.TerminateAgreementRequest
	DECLARE @lastMaxId BIGINT
	SELECT @lastMaxId = ISNULL(MAX(Id),0) FROM dbo.TerminateAgreementRequest

	-- Create New Records
	INSERT INTO dbo.[TerminateAgreementRequest]
	([EmployeeId], [DayId], [EntityId], [EntityAgreementId],
	[RequestDate], [RequestReason], [Status],	[ActivityId],
	[RequestNotes], [SqliteTerminateAgreementId],  CreatedBy, UpdatedBy)
	SELECT 
	sqe.[EmployeeId], d.[Id] AS [DayId], sqe.EntityId, 	sqe.[AgreementId],
	CAST(sqe.[RequestDate] AS [Date]), sqe.Reason, 'Pending', sqa.ActivityId,
	sqe.Notes, sqe.[Id],  'ProcessSqliteTerminateAgreementData', 'ProcessSqliteTerminateAgreementData'

	FROM dbo.SqliteTerminateAgreement sqe
	INNER JOIN dbo.[Day] d ON d.[DATE] = CAST(sqe.[RequestDate] AS [Date])
	-- this join is to get activity Id
	INNER JOIN dbo.SqliteAction sqa on sqa.EmployeeId = sqe.EmployeeId
			AND sqa.[At] = sqe.RequestDate
			AND sqa.PhoneDbId = sqe.ActivityId
	WHERE sqe.BatchId = @batchId AND sqe.IsProcessed = 0
	AND sqe.EntityId > 0 
	AND sqe.AgreementId > 0
	ORDER BY sqe.Id

	-- now we need to update the id in SqliteTerminateAgreement table
	UPDATE dbo.SqliteTerminateAgreement
	SET TerminateAgreementId = e.Id,
	IsProcessed = 1,
	DateUpdated = SYSUTCDATETIME()
	FROM dbo.SqliteTerminateAgreement se
	INNER JOIN dbo.[TerminateAgreementRequest] e on se.Id = e.SqliteTerminateAgreementId
	AND se.BatchId = @batchId
	AND e.Id > @lastMaxId
END
