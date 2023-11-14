CREATE TABLE [dbo].[BankStatements] (
    [Id]                   BIGINT          IDENTITY (1, 1) NOT NULL,
    [Transaction_Date]     DATETIME        NULL,
    [Value_Date]           DATETIME        NULL,
    [RefNo_ChequeNo]       NVARCHAR (MAX)  NULL,
    [Description]          NVARCHAR (MAX)  NULL,
    [Branch_Code]          BIGINT          NULL,
    [Transaction_Mnemonic] NVARCHAR (MAX)  NULL,
    [Transaction_Literal]  NVARCHAR (MAX)  NULL,
    [Debit]                DECIMAL (18, 2) NULL,
    [Credit]               DECIMAL (18, 2) NULL,
    [Balance]              DECIMAL (18, 2) NULL,
    [AccountNo]            BIGINT          NULL,
    [BankName]             NVARCHAR (MAX)  NULL,
    [FileName]             NVARCHAR (MAX)  NULL,
    CONSTRAINT [PK_BankStatements] PRIMARY KEY CLUSTERED ([Id] ASC)
);