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