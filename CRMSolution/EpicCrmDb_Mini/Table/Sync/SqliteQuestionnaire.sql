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