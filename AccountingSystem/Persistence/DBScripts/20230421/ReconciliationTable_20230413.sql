
CREATE TABLE [dbo].[Reconciliation](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[BankTransactionsId]  int  NOT  NULL,
	[TransactionsId] int  NOT NULL,
	TransactionType int NOT  NULL,
	Amount decimal NOT NULL,
	ReconciledDate DateTime2(7)  NULL,
      TransactionDate DateTime2(7)  NULL,
	ReconcileStatus int NOT  NULL,
	MatchedDate  DateTime2(7) NOT NULL,
	MatchedBy nvarchar(50) NOT NULL,
	IsActive bit NOT NULL,
	ClosingBalance decimal ,
	Description nvarchar(50) ,
	ChargeOrPayment nvarchar(5) Not NULL,
	CreatedBy  nvarchar(50) NOT NULL,
	CreatedDate DateTime2(7) NOT NULL,
	UpdatedBy  nvarchar(50) NOT NULL,
	UpdatedDate DateTime2(7) NOT NULL
 CONSTRAINT [PK_Reconciliation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) 
GO