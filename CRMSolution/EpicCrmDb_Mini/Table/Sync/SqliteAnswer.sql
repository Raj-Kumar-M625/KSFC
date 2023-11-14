CREATE TABLE [dbo].[SqliteAnswer] (
	[Id] [bigint] NOT NULL PRIMARY KEY IDENTITY(1,1),
	CrossRefId [BIGINT] NOT NULL REFERENCES  [SqliteQuestionnaire]([Id]),
	QuestionPaperQuestionId [BIGINT] NOT NULL DEFAULT 0,
	HasTextComment BIT NOT NULL DEFAULT 0,
	TextComment [NVARCHAR](2048) NULL
)
GO