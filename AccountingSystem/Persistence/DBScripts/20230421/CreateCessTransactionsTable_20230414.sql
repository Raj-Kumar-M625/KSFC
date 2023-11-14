CREATE TABLE [dbo].[CessTransactions](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ReferenceId] [int] NOT NULL,
	[VendorID] [int] NULL,
	[ChargeOrPayment] [nvarchar](10) NULL,
	[VendorName] [nvarchar](50) NULL,
	[PhoneNumber] [nvarchar](50) NULL,
	[ReferenceNumber] [int] NULL,
	[ReferenceType] [nvarchar](50) NOT NULL,
	[TransactionType] [int] NOT NULL,
	[GSTIN_Number] [nvarchar](50) NULL,
	[PAN_Number] [nvarchar](50) NULL,
	[TAN_Number] [nvarchar](50) NULL,
	[AccountNumber] [nvarchar](50) NULL,
	[BankName] [nvarchar](50) NULL,
	[BranchName] [nvarchar](50) NULL,
	[IFSCCode] [nvarchar](50) NULL,
	[UTRNumber] [nvarchar](50) NULL,
	[BillReferenceNo] [nvarchar](50) NULL,
	[BillNo] [nvarchar](50) NULL,
	[PaymentReferenceNo] [nvarchar](50) NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[Cheque_No] [nvarchar](50) NULL,
	[ChallanNo] [nvarchar](50) NULL,
	[Description] [varchar](100) NULL,
	[Scheme] [nvarchar](50) NULL,
	[AssesmentYear] [nvarchar](50) NOT NULL,
	[TransactionDate] [datetime2](7) NULL,
	[TransactionGeneratedDate] [datetime2](7) NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK__CessTransaction__3214EC277F63156A] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]