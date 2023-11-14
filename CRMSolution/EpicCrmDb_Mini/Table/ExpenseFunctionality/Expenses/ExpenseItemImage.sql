CREATE TABLE [dbo].[ExpenseItemImage]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[ExpenseItemId] BIGINT NOT NULL REFERENCES dbo.ExpenseItem,
	[ImageId] BIGINT NOT NULL References [Image],
	[SequenceNumber] INT NOT NULL
)
