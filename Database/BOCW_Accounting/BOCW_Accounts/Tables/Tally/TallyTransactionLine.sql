CREATE TABLE [dbo].[TallyTransactionLine] (
    [SlNo]            INT            IDENTITY (1, 1) NOT NULL,
    [Date]            DATETIME       NULL,
    [Guid]            NVARCHAR (MAX) NULL,
    [VoucherNo]       NVARCHAR (MAX) NULL,
    [VoucherTypeName] NVARCHAR (MAX) NULL,
    [LedgerName]      NVARCHAR (MAX) NULL,
    [ChequeNo]        NVARCHAR (MAX) NULL,
    [ChequeDate]      DATETIME       NULL,
    [DebitAmount]     FLOAT (53)     NULL,
    [CreditAmount]    FLOAT (53)     NULL,
    [Narration]       VARCHAR (MAX)  NULL,
    CONSTRAINT [PK_TallyTransactionLine] PRIMARY KEY CLUSTERED ([SlNo] ASC)
);

