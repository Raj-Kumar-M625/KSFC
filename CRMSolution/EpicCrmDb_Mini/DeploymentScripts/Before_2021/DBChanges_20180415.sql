CREATE TABLE [dbo].[BankAccount]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[AreaCode] NVARCHAR(10) NOT NULL,
	[BankName] NVARCHAR(50) NOT NULL,
	[BankPhone] NVARCHAR(20) NOT NULL DEFAULT '',
	[AccountNumber] NVARCHAR(50) NOT NULL,
	[IFSC] NVARCHAR(50) NOT NULL,

	[DateCreated] DATETIME2 NOT NULL DEFAULT Sysutcdatetime(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[CreatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[UpdatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[IsActive] Bit NOT NULL Default 1,
)
go

Alter table dbo.FeatureControl
add [BankAccountFeature] BIT NOT NULL DEFAULT 0
go
