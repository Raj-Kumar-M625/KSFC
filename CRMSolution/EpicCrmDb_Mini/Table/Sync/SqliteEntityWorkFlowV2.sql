CREATE TABLE [dbo].[SqliteEntityWorkFlowV2]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
	[BatchId] BIGINT NOT NULL References dbo.SqliteActionBatch,
    [EmployeeId] BIGINT NOT NULL,
	[EntityId] BIGINT NOT NULL DEFAULT 0,
	[EntityType] NVARCHAR(50) NOT NULL, 
	[EntityName] NVARCHAR(50) NOT NULL, 
	[AgreementId] BIGINT NOT NULL DEFAULT 0,
	[Agreement] NVARCHAR(50) NOT NULL,
	[EntityWorkFlowDetailId] BIGINT NOT NULL DEFAULT 0,
	[FieldVisitDate] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

	[TypeName] NVARCHAR(50) NOT NULL DEFAULT '',
	[TagName] NVARCHAR(50) NOT NULL DEFAULT '',
	[Phase] NVARCHAR(50) NOT NULL,
	[IsStarted] BIT NOT NULL DEFAULT 0,
	[Date] DATE NOT NULL,
	[MaterialType] NVARCHAR(50),
	[MaterialQuantity] INT NOT NULL DEFAULT 0,
	[GapFillingRequired] BIT NOT NULL DEFAULT 0,
	[GapFillingSeedQuantity] INT NOT NULL DEFAULT 0,
	[LaborCount] INT NOT NULL DEFAULT 0,
	[PercentCompleted] INT NOT NULL DEFAULT 0,

	[ActivityId] NVARCHAR(50) NOT NULL DEFAULT '',
	[FollowUpCount] BIGINT NOT NULL DEFAULT 0,

	-- April 11 2020 - PJM
	[BatchNumber] NVARCHAR(50) NOT NULL DEFAULT '',
	[LandSize] NVARCHAR(10) NOT NULL DEFAULT '',
	[DWSEntry] NVARCHAR(50) NOT NULL DEFAULT '',
	[ItemCount] BIGINT NOT NULL DEFAULT 0, -- -- this is plant count or nipping count
	[ItemsUsedCount] BIGINT NOT NULL DEFAULT 0, -- this is the count of fertilizers/pesticides used
	[YieldExpected] BIGINT NOT NULL DEFAULT 0,
	[BagsIssued] BIGINT NOT NULL DEFAULT 0,
	[HarvestDate] DATE NOT NULL DEFAULT '1900-01-01',

	[IsProcessed] BIT NOT NULL DEFAULT 0,
	[EntityWorkFlowId] BIGINT NOT NULL DEFAULT 0,
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[Timestamp] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
)
