CREATE TABLE [dbo].[Answer](
	[Id] [bigint] NOT NULL PRIMARY KEY IDENTITY(1,1),
	CrossRefId	[bigint] NOT NULL REFERENCES [CustomerQuestionnaire]([Id]),
	QuestionPaperQuestionId [bigint] NOT NULL DEFAULT 0,
	HasTextComment [bit] NOT NULL  DEFAULT 0,
	TextComment [nvarchar](2048) NULL,
	DateCreated datetime2(7) NOT NULL DEFAULT SYSUTCDATETIME(),
	DateUpdated datetime2(7) NOT NULL DEFAULT SYSUTCDATETIME(),
	[SqliteAnswerID] BIGINT NULL
)
GO