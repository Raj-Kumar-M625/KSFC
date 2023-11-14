CREATE TABLE [dbo].[OrderItem]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[OrderId] BIGINT NOT NULL REFERENCES dbo.[Order],
	[SerialNumber] INT NOT NULL,
	[ProductCode] NVARCHAR(50) NOT NULL,
	[UnitQuantity] INT NOT NULL DEFAULT 0,
	[UnitPrice] DECIMAL(19,2) NOT NULL DEFAULT 0,  -- billing price
	[Amount] DECIMAL(19,2) NOT NULL DEFAULT 0,  -- legacy apk v1.3 or lower

	[DiscountPercent] DECIMAL(5,2) NOT NULL DEFAULT 0,
	[DiscountedPrice] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[ItemPrice] DECIMAL(19,2) NOT NULL DEFAULT 0,  -- DiscountedPrice * Qty
	[GstPercent] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[GstAmount] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[NetPrice] DECIMAL(19,2) NOT NULL DEFAULT 0,

	[RevisedUnitQuantity] INT NOT NULL DEFAULT 0,
	[RevisedAmount] DECIMAL(19,2) NOT NULL DEFAULT 0, -- legacy apk 1.3 or lower

	[RevisedDiscountPercent] DECIMAL(5,2) NOT NULL DEFAULT 0,
	[RevisedDiscountedPrice] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[RevisedItemPrice] DECIMAL(19,2) NOT NULL DEFAULT 0,  -- RevisedDiscountedPrice * Qty
	[RevisedGstPercent] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[RevisedGstAmount] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[RevisedNetPrice] DECIMAL(19,2) NOT NULL DEFAULT 0,

	[DateUpdated] Datetime2,
	[UpdatedBy] NVARCHAR(50)
)
