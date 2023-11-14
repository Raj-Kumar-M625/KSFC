CREATE TABLE [dbo].[QuestionPaper]
(
	[Id] [bigint] NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Name] [nvarchar](100) NOT NULL,
	[EntityType] [nvarchar](50) NULL,
	[QuestionCount] [bigint] NOT NULL,
	[DateCreated] Datetime2 NOT NULL DEFAULT sysutcdatetime(),
	[DateUpdated] Datetime2 NOT NULL DEFAULT sysutcdatetime(),
	[IsActive] [bit] NOT NULL DEFAULT 0,
)
GO