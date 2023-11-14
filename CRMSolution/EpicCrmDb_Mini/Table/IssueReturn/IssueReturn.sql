CREATE TABLE [dbo].[IssueReturn]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[EmployeeId] BIGINT NOT NULL REFERENCES TenantEmployee,
	[DayId] BIGINT NOT NULL REFERENCES dbo.[Day],
	[EntityId] BIGINT NOT NULL REFERENCES dbo.Entity,
	[EntityAgreementId] BIGINT NULL REFERENCES dbo.EntityAgreement,
	[ItemMasterId] BIGINT NOT NULL REFERENCES dbo.ItemMaster,
	[TransactionDate] DATE NOT NULL,
	[TransactionType] NVARCHAR(50) NOT NULL, -- Issue/Return/Abandoned
	[Quantity] INT NOT NULL,
	[SlipNumber] NVARCHAR(50) NOT NULL DEFAULT '',
	[LandSizeInAcres] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[ItemRate] DECIMAL(19,2) NOT NULL DEFAULT 0,

	[AppliedTransactionType] NVARCHAR(50) NOT NULL, -- Issue/Return/Abandoned
	[AppliedItemMasterId] BIGINT NOT NULL REFERENCES dbo.ItemMaster,
	[AppliedQuantity] INT NOT NULL DEFAULT 0,
	[AppliedItemRate] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Status] NVARCHAR(50) NOT NULL DEFAULT 'Pending',

	[ActivityId] BIGINT NOT NULL,
	[SqliteIssueReturnId] BIGINT NOT NULL DEFAULT 0,
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

	[CreatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[UpdatedBy] NVARCHAR(50) NOT NULL DEFAULT '',

	[Comments] NVARCHAR(100) NOT NULL DEFAULT '',
	[CyclicCount] BIGINT NOT NULL DEFAULT 1,
)
GO
