CREATE TABLE [dbo].[BankAccountDetails] (
    [SlNo]                 INT            IDENTITY (1, 1) NOT NULL,
    [AccountNumber]        NVARCHAR (MAX) NULL,
    [BankName]             NVARCHAR (MAX) NULL,
    [AccountType]          NVARCHAR (MAX) NULL,
    [Purpose]              NVARCHAR (MAX) NULL,
    [CashFlow]             NVARCHAR (MAX) NULL,
    [TransactionsReceived] NVARCHAR (MAX) NULL,
    [ShortBankName]        NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_BankAccountDetails] PRIMARY KEY CLUSTERED ([SlNo] ASC)
);