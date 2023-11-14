CREATE TABLE [dbo].[TallyTransactionHeader] (
    [SlNo]            INT            IDENTITY (1, 1) NOT NULL,
    [Guid]            NVARCHAR (MAX) NULL,
    [VoucherNo]       NVARCHAR (MAX) NULL,
    [Date]            DATETIME2 (7)  NULL,
    [VoucherTypeName] NVARCHAR (MAX) NULL,
    [LedgerName]      NVARCHAR (MAX) NULL,
    [LedgerGroup]     NVARCHAR (MAX) NULL,
    [Amount]          FLOAT (53)     NULL,
    [Narration]       NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_TallyTransactionHeader] PRIMARY KEY CLUSTERED ([SlNo] ASC)
);

