CREATE TABLE [dbo].[StockRequest]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[StockRequestTagId] BIGINT NOT NULL References dbo.[StockRequestTag],

	[ItemMasterId] BIGINT NOT NULL REFERENCES dbo.ItemMaster,
	[Quantity] INT NOT NULL,

	[Status] NVARCHAR(50) NOT NULL DEFAULT 'PendingFulfillment',

	[CyclicCount] BIGINT NOT NULL DEFAULT 1,

	[CreatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[UpdatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
)
