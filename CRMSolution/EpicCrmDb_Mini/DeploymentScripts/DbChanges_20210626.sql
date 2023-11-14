-- DEALER QUESTIONNAIRE TABLES ; Date 25-0-2021
------------------------------------------------
CREATE TABLE [dbo].[CustomerQuestionnaire](
	[Id] [bigint] NOT NULL PRIMARY KEY IDENTITY(1,1),
	SqliteQuestionnaireId [bigint] NOT NULL DEFAULT 0,
	QuestionnaireDate DATETIME2(7) NULL,
	EmployeeId [bigint] NOT NULL,
	CustomerCode [nvarchar](50) NOT NULL,
	QuestionPaperId [bigint] REFERENCES [QuestionPaper]([Id]),
	QuestionPaperName [nvarchar](50) NOT NULL,
	ActivityId [bigint] NOT NULL DEFAULT 0,
	DateCreated DATETIME2(7) NOT NULL DEFAULT SYSUTCDATETIME(),
	DateUpdated DATETIME2(7) NOT NULL DEFAULT SYSUTCDATETIME(),
	CreatedBy [NVARCHAR](50) NOT NULL,
	UpdatedBy [NVARCHAR](50) NOT NULL,
)
GO
---------------------------------------------------------
CREATE TABLE [dbo].[Answer](
	[Id] [bigint] NOT NULL PRIMARY KEY IDENTITY(1,1),
	CrossRefId	[bigint] NOT NULL REFERENCES [CustomerQuestionnaire]([Id]),
	QuestionPaperQuestionId [bigint] NOT NULL DEFAULT 0,
	HasTextComment [bit] NOT NULL  DEFAULT 0,
	TextComment [nvarchar](2048) NULL,
	DateCreated datetime2(7) NOT NULL DEFAULT SYSUTCDATETIME(),
	DateUpdated datetime2(7) NOT NULL DEFAULT SYSUTCDATETIME()
)
GO
---------------------------------------------------------
CREATE TABLE [dbo].[AnswerDetail](
	[Id] [bigint] NOT NULL PRIMARY KEY IDENTITY(1,1),
	AnswerId [bigint] NOT NULL REFERENCES [Answer]([Id]),
	QuestionPaperQuestionId [bigint] NOT NULL DEFAULT 0,
	QuestionPaperAnswerId [bigint] NOT NULL DEFAULT 0
)
GO

-----------------------------------------------------------
CREATE TABLE [dbo].[SqliteQuestionnaire] (
	[Id] [bigint] NOT NULL PRIMARY KEY IDENTITY(1,1),
	BatchId [bigint] NOT NULL REFERENCES  [SqliteActionBatch]([Id]),
	EmployeeId [bigint] NOT NULL,
	IsNewEntity [bit] NOT NULL DEFAULT 0,
	EntityId [bigint] NOT NULL,
	EntityName [nvarchar](50) NOT NULL,
	QuestionnaireDate DATETIME2(7) NOT NULL DEFAULT SYSUTCDATETIME(),
	SqliteQuestionPaperId [bigint] NOT NULL DEFAULT 0,
	SqliteQuestionPaperName [nvarchar](50) NOT NULL,
	ActivityId [nvarchar](50) NOT NULL,
	PhoneDbId [nvarchar](50) NOT NULL,
	ParentReferenceId [nvarchar](50) NOT NULL,
	IsProcessed [bit] NOT NULL  DEFAULT 0,
	CustomerQuestionnaireId [bigint] NOT NULL  DEFAULT 0,
	DateCreated datetime2(7) NOT NULL DEFAULT SYSUTCDATETIME(),
	DateUpdated datetime2(7) NOT NULL DEFAULT SYSUTCDATETIME()
)
GO
------------------------------------------------------------
CREATE TABLE [dbo].[SqliteAnswer] (
	[Id] [bigint] NOT NULL PRIMARY KEY IDENTITY(1000,1),
	CrossRefId [BIGINT] NOT NULL REFERENCES  [SqliteQuestionnaire]([Id]),
	QuestionPaperQuestionId [BIGINT] NOT NULL DEFAULT 0,
	HasTextComment BIT NOT NULL DEFAULT 0,
	TextComment [NVARCHAR](2048) NULL
)
GO
---------------------------------------------------------------
CREATE TABLE [dbo].[SqliteAnswerDetail] (
	[Id] [bigint] NOT NULL PRIMARY KEY IDENTITY(1,1),
	AnswerId [bigint] NOT NULL REFERENCES [SqliteAnswer]([Id]),
	SqliteQuestionPaperQuestionId BIGINT NOT NULL DEFAULT 0,
	SqliteQuestionPaperAnswerId BIGINT NOT NULL DEFAULT 0

)
GO
------------------------------------------------------------
ALTER TABLE [dbo].[SqliteActionBatch]
ADD
	[QuestionnaireTarget] [bigint] NOT NULL DEFAULT 0

GO

ALTER TABLE [dbo].[SqliteActionBatch]
ADD
	[QuestionnaireTargetSaved] [bigint] NOT NULL DEFAULT 0

GO

------------------------------------------------------------------
-- STORED PROC TO PROCESS SQLITE DATA
-------------------------------------------------------------------
CREATE PROCEDURE [dbo].[ProcessSqliteQuestionnaireData]
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
			DateUpdated
		)
	SELECT	SA.CrossRefId,
			SA.QuestionPaperQuestionId, 
			SA.HasTextComment, 
			SA.TextComment, 
			@currentTime, 
			@currentTime 
	FROM SqliteAnswer SA
	WHERE SA.CrossRefId IN (SELECT Id FROM dbo.SqliteQuestionnaire  WHERE IsProcessed = 0 AND BatchId = @batchId)
	
	-- Move the data into [AnswerDetail] from [SqliteAnswerDetail]
	INSERT INTO [dbo].[AnswerDetail]
	(
		AnswerId,
		QuestionPaperQuestionId,
		QuestionPaperAnswerId
	)
	SELECT SAD.AnswerId, 
		SAD.SqliteQuestionPaperQuestionId, 
		SAD.SqliteQuestionPaperAnswerId
	FROM [dbo].[SqliteAnswerDetail] SAD
	WHERE SAD.AnswerId IN (SELECT Id FROM dbo.SqliteAnswer WHERE CrossRefId IN (SELECT Id FROM dbo.SqliteQuestionnaire WHERE IsProcessed = 0))

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




ALTER TABLE [dbo].[FeatureControl]
ADD
	[QuestionnaireFeature] BIT NOT NULL DEFAULT 0
GO


