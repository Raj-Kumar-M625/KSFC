CREATE TABLE [dbo].[EntityAgreement]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[EntityId] BIGINT NOT NULL REFERENCES [Entity]([Id]), 
	[WorkflowSeasonId]  BIGINT NOT NULL REFERENCES [WorkFlowSeason]([Id]), 
	[AgreementNumber] NVARCHAR(50) NOT NULL,
	[LandSizeInAcres] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Status] NVARCHAR(50) NOT NULL DEFAULT 'Pending',

	[IsPassbookReceived] BIT NOT NULL DEFAULT 0,
	[PassbookReceivedDate] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

	[RatePerKg] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[ActivityId] BIGINT NOT NULL DEFAULT 0,
	ZoneCode nvarchar(10) ,
    ZoneName nvarchar(50) ,
	AreaCode nvarchar(10) ,
	AreaName nvarchar(50) ,
	TerritoryCode nvarchar(10) ,
	TerritoryName nvarchar(50) ,
	HQCode nvarchar(10) ,
	HQName nvarchar(50) ,
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[CreatedBy] NVARCHAR(50) NOT NULL,
	[UpdatedBy] NVARCHAR(50) NOT NULL,
	[BonusProcessed] BIT NOT NULL DEFAULT 0
)
