CREATE PROCEDURE [dbo].[ProcessSqliteAdvanceRequestData]
	@batchId BIGINT
AS
BEGIN

	-- if batch is already processed - return	
	IF NOT EXISTS(SELECT 1 FROM dbo.SqliteActionBatch 
			WHERE Id = @batchId AND AdvanceRequestsSaved > 0
			AND BatchProcessed = 0)
	BEGIN
		RETURN;
	END

	-- First create required records in Day Table
	INSERT INTO dbo.[Day]
	([DATE])
	SELECT DISTINCT CAST(e.[AdvanceRequestDate] AS [Date])
	FROM dbo.SqliteAdvanceRequest e
	LEFT JOIN dbo.[Day] d on CAST(e.[AdvanceRequestDate] AS [Date]) = d.[DATE]
	WHERE e.BatchId = @batchId
	AND d.[DATE] IS NULL

	-- SqliteAdvanceRequest can have Advance Requests for Agreements, that were
	-- created on phone and not uploaded.  For such records, we have to first
	-- fill in AgreementId in SqliteAdvanceRequest table
	UPDATE dbo.SqliteAdvanceRequest
	SET AgreementId = agg.EntityAgreementId,
	EntityId = agg.EntityId,
	DateUpdated = SYSUTCDATETIME()
	FROM dbo.SqliteAdvanceRequest sqe
	INNER JOIN dbo.SqliteAgreement agg on sqe.ParentReferenceId = agg.PhoneDbId
	and sqe.IsNewAgreement = 1
	and agg.BatchId <= @batchId -- agreement has to come in same batch or before
	and sqe.IsProcessed = 0


	-- select current max Id from dbo.AdvanceRequest
	DECLARE @lastMaxId BIGINT
	SELECT @lastMaxId = ISNULL(MAX(Id),0) FROM dbo.AdvanceRequest

	-- Create Input/Issue Records
	INSERT INTO dbo.[AdvanceRequest]
	([EmployeeId], [DayId], [EntityId], 
	[EntityAgreementId],
	[AdvanceRequestDate], [Amount],	[ActivityId],
	[RequestNotes], [Status], CreatedBy, UpdatedBy,
	[SqliteAdvanceRequestId])
	SELECT 
	sqe.[EmployeeId], d.[Id] AS [DayId], sqe.EntityId, 
	sqe.[AgreementId],
	CAST(sqe.[AdvanceRequestDate] AS [Date]), sqe.[Amount], sqa.ActivityId,
	sqe.Notes, 'Pending', 'ProcessSqliteAdvanceRequestData', 'ProcessSqliteAdvanceRequestData',
	sqe.[Id]
	FROM dbo.SqliteAdvanceRequest sqe
	INNER JOIN dbo.[Day] d ON d.[DATE] = CAST(sqe.[AdvanceRequestDate] AS [Date])
	-- this join is to get activity Id
	INNER JOIN dbo.SqliteAction sqa on sqa.EmployeeId = sqe.EmployeeId
			AND sqa.[At] = sqe.AdvanceRequestDate
			AND sqa.PhoneDbId = sqe.ActivityId
	WHERE sqe.BatchId = @batchId AND sqe.IsProcessed = 0
	AND sqe.EntityId > 0 
	AND sqe.AgreementId > 0
	ORDER BY sqe.Id

	-- now we need to update the id in SqliteAdvanceRequest table
	UPDATE dbo.SqliteAdvanceRequest
	SET AdvanceRequestId = e.Id,
	IsProcessed = 1,
	DateUpdated = SYSUTCDATETIME()
	FROM dbo.SqliteAdvanceRequest se
	INNER JOIN dbo.[AdvanceRequest] e on se.Id = e.SqliteAdvanceRequestId
	AND se.BatchId = @batchId
	AND e.Id > @lastMaxId
END
