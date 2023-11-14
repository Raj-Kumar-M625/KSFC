CREATE TABLE [dbo].[SqliteAgreement]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[BatchId] BIGINT NOT NULL REFERENCES dbo.SqliteActionBatch,
	[EmployeeId] BIGINT NOT NULL,

	[IsNewEntity] BIT NOT NULL,
	[EntityId] BIGINT NOT NULL,
	[EntityName] NVARCHAR(50) NOT NULL,

	[SeasonName] NVARCHAR(50) NOT NULL,
	[TypeName] NVARCHAR(50) NOT NULL,
	[Acreage] DECIMAL(19,2) NOT NULL,
	[AgreementDate] DATETIME2 NOT NULL,

	[ActivityId] NVARCHAR(50) NOT NULL DEFAULT '',
	[PhoneDbId] NVARCHAR(50) NOT NULL, 
	[ParentReferenceId]  NVARCHAR(50) NOT NULL DEFAULT '',

	[IsProcessed] BIT NOT NULL DEFAULT 0,  
	[EntityAgreementId] BIGINT NOT NULL DEFAULT 0,
	[TerritoryCode] nvarchar(10) null,
    [TerritoryName] nvarchar(50) null,
    [HQCode] nvarchar(10) null,
    [HQName] nvarchar(50) null,
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
)
