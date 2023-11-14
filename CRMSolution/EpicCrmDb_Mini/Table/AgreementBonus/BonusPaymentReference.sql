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