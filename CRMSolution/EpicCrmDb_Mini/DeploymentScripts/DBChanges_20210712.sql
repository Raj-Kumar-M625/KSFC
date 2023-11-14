ALTER TABLE [dbo].[Answer]
ADD
	[SqliteAnswerID] BIGINT NULL
GO

ALTER PROCEDURE [dbo].[ProcessSqliteQuestionnaireData]
	@batchId BIGINT
AS
BEGIN

	-- Author: Rajesh V / Vasanth; 

DECLARE @currentTime DATETIME2 = SYSUTCDATETIME()	
DECLARE @CQID BIGINT

	SET NOCOUNT ON;	
	-- if batch is already processed - return	
	IF NOT EXISTS(SELECT 1 FROM [dbo].[SqliteActionBatch]			
			WHERE [SqliteActionBatch].Id = @batchId 			
			AND [SqliteActionBatch].QuestionnaireTargetSaved > 0
			AND [SqliteActionBatch].BatchProcessed = 0 )
	BEGIN
		RETURN;
	END

BEGIN TRANSACTION

	-- First create required records in Day Table (NO Questionnaire Date coming from the Phone JSON)
	--INSERT INTO dbo.[Day]
	--([DATE])
	--SELECT DISTINCT CAST(e.[QuestionnaireDate] AS [Date])
	--FROM dbo.SqliteQuestionnaire e
	--LEFT JOIN dbo.[Day] d on CAST(e.[QuestionnaireDate] AS [Date]) = d.[DATE]
	--WHERE e.BatchId = @batchId
	--AND d.[DATE] IS NULL

	-- Move the data into [CustomerQuestionnaire] from [SqliteQuestionnaire]
	INSERT INTO [dbo].[CustomerQuestionnaire] 
	(	
		SqliteQuestionnaireId,
		QuestionnaireDate,
		EmployeeId,
		CustomerCode,
		QuestionPaperId,
		QuestionPaperName,		
		ActivityId,
		DateCreated,
		DateUpdated,
		CreatedBy,	
		UpdatedBy
	)
    SELECT SQ.Id,
		SQ.QuestionnaireDate,
		SQ.EmployeeId,
		SA.ClientCode,		
		SQ.SqliteQuestionPaperId,
		SQ.SqliteQuestionPaperName,
		SA.ActivityId,
		SYSUTCDATETIME(),
		SYSUTCDATETIME(),
		'ProcessSqliteQuestionaireData',
		'ProcessSqliteQuestionaireData'			
	FROM [dbo].[SqliteQuestionnaire] AS SQ
	INNER JOIN [dbo].[SqliteAction] AS SA ON SA.PhoneDbId = SQ.ActivityId
	WHERE SQ.IsProcessed = 0 AND SQ.BatchId = @batchId 

	
-- Move the data into [Answer] from [SqliteAnswer]
	INSERT INTO [dbo].[Answer] 
		( 
			CrossRefId,
			QuestionPaperQuestionId, 
			HasTextComment,
			TextComment, 
			DateCreated, 
			DateUpdated,
			SqliteAnswerID
		)
		SELECT	CQ.ID,
			SA.QuestionPaperQuestionId, 
			SA.HasTextComment, 
			SA.TextComment, 
			GETDATE(), 
			GETDATE(),
			SA.ID
	FROM SqliteAnswer SA 
	INNER JOIN SqliteQuestionnaire SQ ON SA.CrossRefId = SQ.Id
	INNER JOIN CustomerQuestionnaire CQ ON CQ.SqliteQuestionnaireId = SQ.ID
	WHERE SQ.BatchId = @batchId AND SQ.IsProcessed = 0
	
	
	
	---- Move the data into [AnswerDetail] from [SqliteAnswerDetail]
	INSERT INTO [dbo].[AnswerDetail]
	(
		AnswerId,
		QuestionPaperQuestionId,
		QuestionPaperAnswerId
	)	
	SELECT A.Id,
		SAD.SqliteQuestionPaperQuestionId, 
		SAD.SqliteQuestionPaperAnswerId
	FROM [dbo].[SqliteAnswerDetail] SAD	 
	INNER JOIN SqliteAnswer SA ON SAD.AnswerId = SA.ID 	
	INNER JOIN SqliteQuestionnaire SQ ON SA.CrossRefId = SQ.Id
	INNER JOIN CustomerQuestionnaire CQ ON CQ.SqliteQuestionnaireId = SQ.ID
	INNER JOIN Answer A ON A.SqliteAnswerID = SA.Id
	WHERE SQ.BatchId = @batchId AND SQ.IsProcessed = 0
	
	-- Cursor to Update [CustomerQuestionnaireId] in [SqliteQuestionnaire]
	DECLARE CustQId CURSOR FOR SELECT Id FROM SqliteQuestionnaire WHERE BatchId = @batchId AND IsProcessed = 0 
	OPEN CustQId
	FETCH NEXT FROM CustQId INTO @CQID
	WHILE @@FETCH_STATUS =0 
	BEGIN
	UPDATE dbo.SqliteQuestionnaire SET CustomerQuestionnaireId = @CQID
							WHERE BatchId = @batchId AND IsProcessed = 0 AND Id = @CQID
	FETCH NEXT FROM CustQId INTO @CQID
	END
	CLOSE CustQId
	DEALLOCATE CustQId
	
	-- Fill Entity Id in Questionnaire that belong to new entity created on phone.
	Update dbo.SqliteQuestionnaire 
	SET EntityId = en.EntityId, EntityName = en.EntityName, DateUpdated = SYSUTCDATETIME()
	FROM dbo.SqliteQuestionnaire sq
	INNER JOIN dbo.SqliteEntity en on sq.ParentReferenceId = en.PhoneDbId
	AND sq.BatchId = @batchId AND sq.IsNewEntity = 1 AND sq.IsProcessed = 0

	-- Update the SqlLiteQuestionnaire Is processed flag
	UPDATE dbo.SqliteQuestionnaire SET IsProcessed = 1 WHERE IsProcessed = 0 AND BatchId = @batchId 

	IF @@ERROR = 0 
		COMMIT TRANSACTION
	ELSE
		ROLLBACK TRANSACTION
		RETURN 
	END	

GO


