CREATE TABLE [dbo].[EntityWorkFlowDetail]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[EntityWorkFlowId] BIGINT NOT NULL REFERENCES dbo.EntityWorkFlow,

	[Sequence] INT NOT NULL,
	[TagName] NVARCHAR(50) NOT NULL DEFAULT '',
	[Phase] NVARCHAR(50) NOT NULL,
	[PlannedStartDate] DATE NOT NULL,
	[PlannedEndDate] DATE NOT NULL,
	[PrevPhase] NVARCHAR(50) NOT NULL,

	[ActivityId] BIGINT NOT NULL DEFAULT 0,
	[IsComplete] BIT NOT NULL DEFAULT 0,
	[ActualDate] DATE NULL,
	[PrevPhaseActualDate] DATE NULL,
	[PhaseCompleteStatus] NVARCHAR(20) NULL,

	-- new fields on 19.4.19
	[IsStarted] BIT NOT NULL DEFAULT 0,
	[WorkFlowDate] DATE,
	[MaterialType] NVARCHAR(50),
	[MaterialQuantity] INT NOT NULL DEFAULT 0,
	[GapFillingRequired] BIT NOT NULL DEFAULT 0,
	[GapFillingSeedQuantity] INT NOT NULL DEFAULT 0,
	[LaborCount] INT NOT NULL DEFAULT 0,
	[PercentCompleted] INT NOT NULL DEFAULT 0,
	[BatchId] BIGINT NOT NULL DEFAULT 0,

	-- April 11, 2020
	[BatchNumber] NVARCHAR(50) NOT NULL DEFAULT '',
	[LandSize] NVARCHAR(10) NOT NULL DEFAULT '',
	[DWSEntry] NVARCHAR(50) NOT NULL DEFAULT '',
	[ItemCount] BIGINT NOT NULL DEFAULT 0, -- this is plant count or nipping count
	[ItemsUsedCount] BIGINT NOT NULL DEFAULT 0, -- this is the count of fertilizers/pesticides used
	[YieldExpected] BIGINT NOT NULL DEFAULT 0,
	[BagsIssued] BIGINT NOT NULL DEFAULT 0,
	[HarvestDate] DATE NOT NULL DEFAULT '1900-01-01',


	-- April 5 2020 - to indicate if it is a follow up activity row
	[IsFollowUpRow] BIT NOT NULL DEFAULT 0,
	[ParentRowId] BIGINT NOT NULL DEFAULT 0,
	[Notes] NVARCHAR(100) NOT NULL DEFAULT '',

	[EmployeeId] BIGINT NULL,
	[DateCreated] DATETIME2 NOT NULL DEFAULT SysutcDATETIME(),
	[Timestamp]  DATETIME2 NOT NULL DEFAULT SysutcDATETIME(),

	[IsActive] BIT NOT NULL DEFAULT 1,
	[CreatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[UpdatedBy] NVARCHAR(50) NOT NULL DEFAULT ''
)
