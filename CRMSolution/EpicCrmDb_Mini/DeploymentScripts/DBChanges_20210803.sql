--Added Column BonusProcessed flag in EntityAgreement
ALTER TABLE [dbo].[EntityAgreement]
ADD
	[BonusProcessed] [bit] NOT NULL DEFAULT 0

GO

--Create Table BonusRate
CREATE TABLE [dbo].[BonusRate](
	[Id] [bigint] NOT NULL PRIMARY KEY IDENTITY(1,1),
	[SeasonName] [nvarchar](50) NOT NULL,
	[TypeName] [nvarchar](50) NOT NULL,
	[WeightTonsFrom] [decimal](19, 2) NOT NULL,
	[WeightTonsTo] [decimal](19, 2) NOT NULL,
	[RatePaise] [decimal](19, 2) NOT NULL
)
GO

--Create Table BonusAgreementDetail
CREATE TABLE [dbo].[BonusAgreementDetail](
	[Id] [bigint] NOT NULL PRIMARY KEY IDENTITY(1,1),
	[AgreementId] [bigint] NOT NULL REFERENCES [EntityAgreement]([Id]),
	[AgreementNumber] [nvarchar](50) NOT NULL,
	[AgreementDate] [datetime] NOT NULL,
	[EntityId] [bigint] NOT NULL REFERENCES [Entity]([Id]),
	[EntityName] [nvarchar](50) NOT NULL,
	[TypeName] [nvarchar](50) NOT NULL,
	[SeasonName] [nvarchar](50) NOT NULL,
	[LandSizeInAcres] [decimal](19, 2) NOT NULL,
	[RatePerKg] [decimal](19, 2) NOT NULL,
	[NetWeight] [decimal](19, 2) NOT NULL,
	[NetPaid] [decimal](19, 2) NOT NULL,
	[BonusRate] [decimal](19, 2) NOT NULL,
	[BonusAmountPayable] [decimal](19, 2) NOT NULL,
	[BonusAmountPaid] [decimal](19, 2) NOT NULL,
	[BonusStatus] [nvarchar](50) NOT NULL,
	[HQCode] [nvarchar](50) NOT NULL,
	[PaymentReference] [nvarchar](50) NULL,
	[BankAccountName] [nvarchar](50) NOT NULL,
	[BankName] [nvarchar](50) NOT NULL,
	[BankAccountNumber] [nvarchar](50) NOT NULL,
	[BankIFSC] [nvarchar](50) NOT NULL,
	[BankBranch] [nvarchar](50) NOT NULL,
	[Comments] [nvarchar](500) NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[UpdatedBy] [nvarchar](50) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateUpdated] [datetime] NOT NULL
 )
GO

--Create Table BonusPaymentReference
CREATE TABLE [dbo].[BonusPaymentReference](
	[Id] [bigint] NOT NULL PRIMARY KEY IDENTITY(1,1),
	[PaymentReference] [nvarchar](50) NOT NULL,
	[AgreementNumber] [nvarchar](MAX) NOT NULL,
	[AgreementCount][int] NOT NULL,
	[PaymentType] [nvarchar](50) NOT NULL,
	[BonusAmountPaid] [decimal](19, 2) NOT NULL,
	[SenderInfo] [nvarchar](50) NOT NULL,
	[Comments] [nvarchar](100) NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[UpdatedBy] [nvarchar](50) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateUpdated] [datetime] NOT NULL,
	[BankName] [nvarchar](50) NOT NULL,
	[AccountNumber] [nvarchar](50) NOT NULL,
	[AccountName] [nvarchar](50) NOT NULL,
	[AccountAddress] [nvarchar](50) NOT NULL,
	[AccountEmail] [nvarchar](50) NOT NULL
	)
GO