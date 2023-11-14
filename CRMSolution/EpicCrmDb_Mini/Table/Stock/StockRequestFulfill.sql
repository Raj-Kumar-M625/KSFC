CREATE TABLE [dbo].[StockRequestFulfill]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[StockRequestId] BIGINT NOT NULL References dbo.[StockRequest],
	[StockRequestTagId] BIGINT NOT NULL References dbo.[StockRequestTag],
	[ItemMasterId] BIGINT NOT NULL REFERENCES dbo.ItemMaster,

	[StockInputId] BIGINT NOT NULL REFERENCES dbo.StockInput,

	[StockBalanceId] BIGINT NOT NULL REFERENCES dbo.StockBalance,
	[StockBalanceCyclicCount] BIGINT NOT NULL,
	[QuantityInHand] INT NOT NULL,
	[QuantityIssued] INT NOT NULL,

	[CyclicCount] BIGINT NOT NULL DEFAULT 1,
	[CreatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[UpdatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
)
