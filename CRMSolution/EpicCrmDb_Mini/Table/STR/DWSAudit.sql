CREATE TABLE [dbo].[DWSAudit](
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[STRTagId] [bigint] NOT NULL,
	[STRId] [bigint] NOT NULL,
	[DWSId] [bigint] NOT NULL,
	[DWSNumber] NVARCHAR(50) NOT NULL,
	[DWSDate] [datetime2](7) NOT NULL,
	[BagCount] [bigint] NOT NULL,
	[FilledBagsWeightKg] [decimal](19, 2) NOT NULL,
	[EmptyBagsWeightKg] [decimal](19, 2) NOT NULL,
	[EntityId] [bigint] NOT NULL,
	[AgreementId] [bigint] NOT NULL,
	[Agreement] [nvarchar](50) NOT NULL,
	[EntityWorkFlowDetailId] [bigint] NOT NULL,
	[TypeName] [nvarchar](50) NOT NULL,
	[TagName] [nvarchar](50) NOT NULL,

	[CreatedBy] [nvarchar](50) NOT NULL DEFAULT (''),
	[DateCreated] [datetime2](7) NOT NULL DEFAULT (sysutcdatetime())
)
