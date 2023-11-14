ALTER TABLE [dbo].[FeatureControl]
ADD
	[BonusCalculationPaymentOption] BIT NOT NULL DEFAULT 0
GO

ALTER TABLE [dbo].[FeatureControl]
ADD
	[SurveyFormReport] BIT NOT NULL DEFAULT 0
GO


------------------------------------------------------------------
---Insert Query for Bank List in CodeTable On for prod site-------
------------------------------------------------------------------

INSERT INTO "CodeTable" ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive", "TenantId") 
VALUES 
('CustomerBank', '', 'ALLAHABAD BANK', 10, 'True', 1),
('CustomerBank', '', 'AXIS BANK LTD', 20, 'True', 1),
('CustomerBank', '', 'BANK OF BARODA', 30, 'True', 1),
('CustomerBank', '', 'BANK OF INDIA', 40, 'True', 1),
('CustomerBank', '', 'CANARA BANK', 50, 'True', 1),
('CustomerBank', '', 'CENTRAL BANK OF INDIA', 60, 'True', 1),
('CustomerBank', '', 'DAVANAGERE  DISTRICT CENTRAL CO-OPERATIVE  BANK', 70, 'True', 1),
('CustomerBank', '', 'FEDERAL BANK', 80, 'True', 1),
('CustomerBank', '', 'HDFC BANK', 90, 'True', 1),
('CustomerBank', '', 'IDFC BANK', 100, 'True', 1),
('CustomerBank', '', 'INDIAN OVERSEAS BANK', 110, 'True', 1),
('CustomerBank', '', 'KARNATAKA BANK', 120, 'True', 1),
('CustomerBank', '', 'KARNATAKA VIKAS GRAMEENA BANK', 130, 'True', 1),
('CustomerBank', '', 'LAKSHMI VILAS BANK', 140, 'True', 1),
('CustomerBank', '', 'PRAGATHI KRISHNA GRAMIN BANK', 150, 'True', 1),
('CustomerBank', '', 'PUNJAB NATIONAL BANK', 160, 'True', 1),
('CustomerBank', '', 'SOUTH INDIAN BANK', 170, 'True', 1),
('CustomerBank', '', 'STATE BANK OF INDIA', 180, 'True', 1),
('CustomerBank', '', 'SUCO BANK', 190, 'True', 1),
('CustomerBank', '', 'THE BELLARY DISTRICT CO-OPERTIVE CENTRAL BANK LTD', 200, 'True', 1),
('CustomerBank', '', 'THE KARNATAKA STATE APEX COOP. BANK', 210, 'True', 1),
('CustomerBank', '', 'THE KARUR VYSYA BANK LIMITED', 220, 'True', 1),
('CustomerBank', '', 'TUMKUR DCC BANK', 230, 'True', 1),
('CustomerBank', '', 'UCO BANK', 240, 'True', 1),
('CustomerBank', '', 'UNION BANK OF INDIA', 250, 'True', 1),
('CustomerBank', '', 'VIKAS SOUHARDA CO OPERATIVE BANK LTD', 260, 'True', 1);
GO


---------------------------
--  Questionnaire 20210427
---------------------------

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

CREATE TABLE [dbo].[QuestionPaperAnswer](
	[Id] [bigint] NOT NULL PRIMARY KEY IDENTITY(1,1),
	[QuestionPaperQuestionId] [bigint] NOT NULL REFERENCES [QuestionPaperQuestion]([Id]),
	[AText] [nvarchar](512) NOT NULL,
) 
GO

INSERT into dbo.CodeTable
(CodeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
('QuestionnaireType', 'Dealer', 'Dealer Questionnaire 1', 10, 1, 1)

INSERT into dbo.CodeTable
(CodeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
('QuestionnaireType', 'Dealer', 'Dealer Questionnaire 2', 20, 1, 1)

INSERT into dbo.CodeTable
(CodeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
('QuestionnaireType', 'Farmer', 'Farmer Questionnaire', 10, 1, 1)
GO



-------------------------------
-- DayPlanning 20210603
-------------------------------


INSERT into dbo.CodeTable
(CodeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
('ActivityType', '', 'New Dealer Appointed', 10, 1, 1)
GO

CREATE TABLE [dbo].[DayPlanTarget](
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[TenantId] BIGINT NOT NULL REFERENCES dbo.Tenant([Id]),
	[EmployeeId] [bigint] NOT NULL REFERENCES dbo.TenantEmployee([Id]),
	[DayId] [bigint] NOT NULL REFERENCES dbo.[Day]([Id]),
	[EmployeeCode]  NVARCHAR(10) NOT NULL,
	[PlanDate]  DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[TargetSales] DECIMAL(19, 2) NOT NULL,
	[TargetCollection] DECIMAL(19, 2) NOT NULL,
	[TargetVigoreSales] DECIMAL(19, 2) NOT NULL DEFAULT 0,
	[TargetDealerAppointment] INT NOT NULL
)
GO

CREATE TABLE [dbo].[SqliteDayPlanTarget](
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[BatchId] BIGINT NOT NULL REFERENCES dbo.SqliteActionBatch,
	[EmployeeId] BIGINT NOT NULL,
	[PlanDate]  DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[TargetSales] DECIMAL(19, 2) NOT NULL,
	[TargetCollection] DECIMAL(19, 2) NOT NULL,
	[TargetVigoreSales] DECIMAL(19, 2) NOT NULL DEFAULT 0,
	[TargetDealerAppointment] INT NOT NULL,
	[PhoneDbId] NVARCHAR(50) NOT NULL, 
	[IsProcessed] BIT NOT NULL DEFAULT 0,  
	[DayPlanTargetId] BIGINT NOT NULL DEFAULT 0,
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
)
GO

ALTER TABLE [dbo].[SqliteActionBatch]
ADD
	[DayPlanTarget] BIGINT NOT NULL DEFAULT 0,
	[DayPlanTargetSaved] BIGINT NOT NULL DEFAULT 0
GO

ALTER TABLE [dbo].[DayPlanTarget]
ADD
	[SqliteDayPlanTargetId] BIGINT NOT NULL DEFAULT 0
GO


CREATE PROCEDURE [dbo].[ProcessSqliteDayPlanTargetData]
	@batchId BIGINT
AS
BEGIN
	-- Kartik June 04 2021

	-- if batch is already processed - return	
	IF NOT EXISTS(SELECT 1 FROM dbo.SqliteActionBatch 
			WHERE Id = @batchId AND DayPlanTargetSaved > 0
			AND BatchProcessed = 0)
	BEGIN
		RETURN;
	END

	-- First create required records in Day Table
	INSERT INTO dbo.[Day]
	([DATE])
	SELECT DISTINCT CAST(e.[PlanDate] AS [Date])
	FROM dbo.SqliteDayPlanTarget e
	LEFT JOIN dbo.[Day] d on CAST(e.[PlanDate] AS [Date]) = d.[DATE]
	WHERE e.BatchId = @batchId
	AND d.[DATE] IS NULL


	-- Create DayPlan Target Records
	INSERT INTO dbo.[DayPlanTarget]
	(TenantId, EmployeeId, DayId, EmployeeCode, PlanDate, TargetSales, TargetCollection, TargetVigoreSales,TargetDealerAppointment, SqliteDayPlanTargetId)
	SELECT te.TenantId, sd.EmployeeId, d.Id, te.EmployeeCode, CAST(sd.PlanDate as [DATE]), sd.TargetSales, sd.TargetCollection, sd.TargetVigoreSales, sd.TargetDealerAppointment, sd.Id
	FROM dbo.SqliteDayPlanTarget sd
	INNER JOIN dbo.[Day] d ON d.[DATE] = CAST(sd.PlanDate AS [Date])
	INNER JOIN dbo.[TenantEmployee] te ON te.Id = sd.EmployeeId
	WHERE BatchId = @batchId AND sd.IsProcessed = 0
	ORDER BY sd.Id

	-- Now update DayPlanTargetId back in SqliteDayPlanTarget
	Update dbo.SqliteDayPlanTarget
	SET DayPlanTargetId = dpt.Id,
	IsProcessed = 1,
	DateUpdated = SYSUTCDATETIME()
	FROM dbo.SqliteDayPlanTarget sd
	INNER JOIN dbo.[DayPlanTarget] dpt on sd.Id = dpt.SqliteDayPlanTargetId
	AND sd.BatchId = @batchId

END
GO


-- Pankaj - Day Planning

INSERT into dbo.CodeTable
(CodeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
('DayPlanType', 'Sales', 'Sales', 10, 1, 1),
('DayPlanType', 'Collections', 'Collections', 20, 1, 1),
('DayPlanType', 'Dealer Appointment', 'Dealer Appointment', 30, 1, 1)
GO

INSERT into dbo.CodeTable
(CodeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
('TargetStatus', 'Achieved', 'Achieved', 10, 1, 1),
('TargetStatus', 'Not Achieved', 'Not Achieved', 20, 1, 1)

GO

ALTER TABLE [dbo].[FeatureControl]
ADD
	[DayPlanReport] BIT NOT NULL DEFAULT 0
GO


-- Tstanes - Will be used for autoexport of Order data

ALTER TABLE [dbo].[Order]
ADD
	[IsExported] BIT NOT NULL DEFAULT 0,
	[ExportedDate] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
GO



------------------------------------------------------------
-- May 22 2021 --- Questionnaire Data Sync and Process Batch
------------------------------------------------------------

--CREATE TABLE [dbo].[SqliteQuestionnaire]
--(
--	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
--	[BatchId] BIGINT NOT NULL REFERENCES dbo.SqliteActionBatch([Id]),
--	[EmployeeId] BIGINT NOT NULL,

--	[IsNewEntity] BIT NOT NULL,
--	[EntityId] BIGINT NOT NULL,
--	[EntityName] NVARCHAR(50) NOT NULL,

--	[SqliteQuestionPaperId] BIGINT NOT NULL,
--	[SqliteQuestionPaperName] NVARCHAR(50) NOT NULL,
--	[ActivityId] NVARCHAR(50) NOT NULL DEFAULT '',
--	[PhoneDbId] NVARCHAR(50) NOT NULL, 
--	[ParentReferenceId]  NVARCHAR(50) NOT NULL DEFAULT '',

--	[IsProcessed] BIT NOT NULL DEFAULT 0,  
--	[CustomerQuestionnaireId] BIGINT NOT NULL DEFAULT 0,
--	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
--	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
--)
--GO

--CREATE TABLE [dbo].[SqliteAnswer]
--(
--	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
--	[CrossRefId] BIGINT NOT NULL REFERENCES dbo.SqliteQuestionnaire([Id]), 
--	[QuestionPaperQuestionId]  BIGINT NOT NULL,
--	[HasTextComment] BIT NOT NULL DEFAULT 0,
--	[TextComment] NVARCHAR(2048) NULL
--)
--GO


--CREATE TABLE [dbo].[SqliteAnswerDetail]
--(
--	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
--    [AnswerId] BIGINT NOT NULL REFERENCES dbo.SqliteAnswer([Id]),
--    [SqliteQuestionPaperQuestionId] BIGINT NOT NULL,
--    [SqliteQuestionPaperAnswerId] BIGINT NOT NULL
--)
--GO



-------Master Table for Questionnaire Data

--CREATE TABLE [dbo].[CustomerQuestionnaire] 
--(
--	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
--	[EmployeeId] BIGINT NOT NULL,
--	[CustomerCode] NVARCHAR(50) NOT NULL,
--	[QuestionPaperId] BIGINT NOT NULL REFERENCES dbo.QuestionPaper([Id]),
--	[QuestionPaperName] NVARCHAR(50) NOT NULL,
--	[ActivityId]  BIGINT NOT NULL,
--	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
--	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
--	[CreatedBy] NVARCHAR(50) NOT NULL,
--	[UpdatedBy] NVARCHAR(50) NOT NULL
--)
--GO

	  
--CREATE TABLE [dbo].[Answer] 
--(
--	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
--	[CrossRefId] BIGINT NOT NULL REFERENCES dbo.CustomerQuestionnaire([Id]), 
--	[QuestionPaperQuestionId]  BIGINT NOT NULL REFERENCES dbo.QuestionPaperQuestion([Id]),
--	[HasTextComment] BIT NOT NULL DEFAULT 0,
--	[TextComment] NVARCHAR(2048) NULL,
--	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
--	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
--)
--GO  
  
  
--CREATE TABLE [dbo].[AnswerDetail]
--(
--	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
--    [AnswerId] BIGINT NOT NULL REFERENCES dbo.Answer([Id]),
--    [QuestionPaperQuestionId] BIGINT NOT NULL REFERENCES dbo.QuestionPaperQuestion([Id]),
--    [QuestionPaperAnswerId] BIGINT NOT NULL REFERENCES dbo.QuestionPaperAnswer([Id]),
--)
--GO
