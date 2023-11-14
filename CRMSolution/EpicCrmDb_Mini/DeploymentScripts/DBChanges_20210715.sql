DROP TABLE [dbo].[STRPayment]
GO

DROP TABLE [dbo].[TransporterPaymentReference]
GO

CREATE TABLE [dbo].[STRPayment](
	[Id] bigint not null identity(1,1) primary key,
	[STRTagId] bigint foreign key references STRTag(Id),
	[STRNumber] nvarchar(50) null,
	[PaymentReference] nvarchar(50) null,
	[ShedToFirstLoadingOdo] bigint null,
	[StartOdometer] bigint null, --+
	[EndOdometer] bigint null, --+
	[SiloToReturnKm] int null, --+
	[TotalRunningKms] int null,
	[CostPerKm] decimal(9,2) NOT NULL DEFAULT 0, --
	[TransportationCharges] decimal(19,2) null,
	[VehicleCapacityKgs] INT NOT NULL DEFAULT 0, --
	[GrossWeight] decimal(19,2) null, --+
	[SiloWeight] decimal(19,2) null, --+
	[ExtraTonnage] decimal(19,2) null,
	[ExtraCostPerTon] DECIMAL(9,2) NOT NULL  DEFAULT 0, --
	[ExtraTonCharges] decimal(19,2) null,
	[TollCharges] decimal(19,2) null,
	[WeighmentCharges] decimal(19,2) null,
	[BagCount] bigint NOT NULL DEFAULT 0, --+
	[HamaliRatePerBag] decimal(19,2) not null DEFAULT 0, --+
	[HamaliCharges] decimal(19,2) null,
	[Others] decimal(19,2) null,
	[NetPayableAmount] decimal(19,2) null,
	[BankAccountName] NVARCHAR(50) NULL,
	[BankName] NVARCHAR(50) NULL,
	[BankAccount] NVARCHAR(50) NULL,
	[BankIFSC] NVARCHAR(50) NULL,
	[BankBranch] NVARCHAR(50) NULL,
	[Comments] nvarchar(500) NULL,
	[Status] nvarchar(50) NOT NULL DEFAULT '',
	[DateCreated] [datetime2](7) NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] [datetime2](7) NOT NULL DEFAULT SYSUTCDATETIME(),
	[CreatedBy] [nvarchar](50) NOT NULL DEFAULT '',
	[UpdatedBy] [nvarchar](50) NOT NULL DEFAULT ''
)
GO

Create Table TransporterPaymentReference
(
	[Id] bigint not null identity(1,1) primary key,
	[PaymentReference] nvarchar(50) null,
	[Comments] nvarchar(100) not null DEFAULT '',
	[NetPayableAmount] decimal(19,2) null,
	[STRCount] BIGINT NOT NULL DEFAULT 0,
	[STRNumber] NVARCHAR(2048) NULL,
	[CreatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[UpdatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[DateCreated] datetime2(7) not null DEFAULT SYSUTCDATETIME(),
	[DateUpdated] datetime2(7) not null DEFAULT SYSUTCDATETIME(),
	[AccountNumber] nvarchar(50) null,
	[AccountName] nvarchar(50) null,
	[AccountAddress] nvarchar(50) null,
	[AccountEmail] nvarchar(50) null,
	[PaymentType] nvarchar(50) null,
	[SenderInfo] nvarchar(50) null,
	[LocalTimeStamp] datetime2(7) not null DEFAULT SYSUTCDATETIME()
)
Go