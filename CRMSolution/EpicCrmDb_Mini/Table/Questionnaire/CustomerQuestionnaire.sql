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