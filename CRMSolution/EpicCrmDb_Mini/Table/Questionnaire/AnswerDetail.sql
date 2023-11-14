CREATE TABLE [dbo].[AnswerDetail](
	[Id] [bigint] NOT NULL PRIMARY KEY IDENTITY(1,1),
	AnswerId [bigint] NOT NULL REFERENCES [Answer]([Id]),
	QuestionPaperQuestionId [bigint] NOT NULL DEFAULT 0,
	QuestionPaperAnswerId [bigint] NOT NULL DEFAULT 0
)
GO