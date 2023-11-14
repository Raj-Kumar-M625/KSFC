CREATE TABLE [dbo].[SqliteReturnOrderItem]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[SqliteReturnOrderId] BIGINT NOT NULL REFERENCES dbo.[SqliteReturnOrder],
	[SerialNumber] INT NOT NULL,
	[ProductCode] NVARCHAR(50) NOT NULL,
	[UnitQuantity] INT NOT NULL DEFAULT 0,
	[UnitPrice] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Amount] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Comment] NVARCHAR(2048),
)
