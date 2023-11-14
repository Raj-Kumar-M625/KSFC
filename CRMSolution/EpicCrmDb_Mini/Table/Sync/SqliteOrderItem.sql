CREATE TABLE [dbo].[SqliteOrderItem]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[SqliteOrderId] BIGINT NOT NULL REFERENCES dbo.[SqliteOrder],
	[SerialNumber] INT NOT NULL,
	[ProductCode] NVARCHAR(50) NOT NULL,
	[UnitQuantity] INT NOT NULL DEFAULT 0,
	[UnitPrice] DECIMAL(19,2) NOT NULL DEFAULT 0,

	[DiscountPercent] DECIMAL(5,2) NOT NULL DEFAULT 0,
	[DiscountedPrice] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[ItemPrice] DECIMAL(19,2) NOT NULL DEFAULT 0,  -- DiscountedPrice * Qty
	[GstPercent] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[GstAmount] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[NetPrice] DECIMAL(19,2) NOT NULL DEFAULT 0,

	[Amount] DECIMAL(19,2) NOT NULL DEFAULT 0  -- legacy apk v1.3 and before
)
