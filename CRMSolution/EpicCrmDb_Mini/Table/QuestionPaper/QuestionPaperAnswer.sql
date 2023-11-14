CREATE TABLE [dbo].[QuestionPaperAnswer](
	[Id] [bigint] NOT NULL PRIMARY KEY IDENTITY(1,1),
	[QuestionPaperQuestionId] [bigint] NOT NULL REFERENCES [QuestionPaperQuestion]([Id]),
	[AText] [nvarchar](512) NOT NULL,
) 
GO