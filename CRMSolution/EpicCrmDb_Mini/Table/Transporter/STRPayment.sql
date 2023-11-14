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