CREATE TABLE [dbo].[QuestionPaperQuestion](
	[Id] [bigint] NOT NULL PRIMARY KEY IDENTITY(1,1),
	[QuestionPaperId] [bigint] NOT NULL REFERENCES [QuestionPaper]([Id]),
	[CategoryName] [nvarchar](50) NOT NULL,
	[SubCategoryName] [nvarchar](50) NOT NULL,
	[QuestionTypeName] [nvarchar](50) NOT NULL,
	[QText] [nvarchar](512) NOT NULL,
	[AdditionalComment] [bit] NOT NULL DEFAULT 0,
	[DisplaySequence] [int] NOT NULL DEFAULT 0,
	[CategoryDesc] [nvarchar](2048) NULL,
	[SubCategoryDesc] [nvarchar](2048) NULL,
	[DateCreated] [datetime] NOT NULL DEFAULT sysutcdatetime(),
	[DateUpdated] [datetime] NOT NULL DEFAULT sysutcdatetime(),
) 
GO