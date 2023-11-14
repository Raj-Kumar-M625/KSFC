CREATE TABLE [dbo].[SqliteOrder]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[BatchId] BIGINT NOT NULL REFERENCES dbo.SqliteActionBatch,
	[EmployeeId] BIGINT NOT NULL,
	[PhoneDbId] NVARCHAR(50) NOT NULL DEFAULT '',
	[CustomerCode] NVARCHAR(50) NOT NULL,
	[OrderType] NVARCHAR(10) NOT NULL, -- 'New' / 'Return'
	[OrderDate] DATETIME2 NOT NULL,
	[TotalAmount] DECIMAL(19,2) NOT NULL, -- discounted price * Qty
	[TotalGST] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[NetAmount] DECIMAL(19,2) NOT NULL DEFAULT 0,

	[MaxDiscountPercentage] DECIMAL(5,2) NOT NULL DEFAULT 0,
	[DiscountType] NVARCHAR(50) NOT NULL DEFAULT 'Amount',
	[AppliedDiscountPercentage] DECIMAL(5,2) NOT NULL DEFAULT 0,

	[ItemCount] BIGINT NOT NULL DEFAULT 0,
	[ImageCount] INT NOT NULL DEFAULT 0,

	[PhoneActivityId] NVARCHAR(50) NOT NULL DEFAULT '',
	[IsProcessed] BIT NOT NULL DEFAULT 0,  -- non-unique indexed
	[OrderId] BIGINT NOT NULL DEFAULT 0,
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
)
