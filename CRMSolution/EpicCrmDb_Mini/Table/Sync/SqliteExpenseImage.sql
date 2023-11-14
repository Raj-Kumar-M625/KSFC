CREATE TABLE [dbo].[SqliteExpenseImage]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[SqliteExpenseId] BIGINT NOT NULL REFERENCES SqliteExpense,
	[SequenceNumber] INT NOT NULL DEFAULT 0,
	[ImageFileName] VARCHAR(100) NOT NULL DEFAULT 'FileNotFound.jpg'
)
