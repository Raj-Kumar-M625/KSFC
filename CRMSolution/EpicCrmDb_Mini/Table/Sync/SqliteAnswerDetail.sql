CREATE TABLE [dbo].[SqliteAnswerDetail] (
	[Id] [bigint] NOT NULL PRIMARY KEY IDENTITY(1,1),
	AnswerId [bigint] NOT NULL REFERENCES [SqliteAnswer]([Id]),
	SqliteQuestionPaperQuestionId BIGINT NOT NULL DEFAULT 0,
	SqliteQuestionPaperAnswerId BIGINT NOT NULL DEFAULT 0

)
GO