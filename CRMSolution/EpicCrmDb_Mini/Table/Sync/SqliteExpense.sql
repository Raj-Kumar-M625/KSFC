CREATE TABLE [dbo].[SqliteExpense]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[SqliteTableId] BIGINT NOT NULL, 
	[BatchId] BIGINT NOT NULL References dbo.SqliteActionBatch,
    [EmployeeId] BIGINT NOT NULL,

	[SequenceNumber] INT NOT NULL DEFAULT 0,
    [ExpenseType] NVARCHAR(50),
	[Amount] DECIMAL(19,2) DEFAULT 0,
	[OdometerStart] BIGINT DEFAULT 0,
	[OdometerEnd] BIGINT DEFAULT 0,
	[VehicleType] NVARCHAR(50),
	[FuelType] NVARCHAR(50),
	[FuelQuantityInLiters] DECIMAL(19,2) NOT NULL DEFAULT 0,

	-- computed column
	[ImageCount] INT NOT NULL DEFAULT 0,

	[Comment] NVARCHAR(2048),

	-- coloumns for server side processing
	[IsProcessed] BIT NOT NULL DEFAULT 0,  -- non-unique indexed
	[ExpenseItemId] BIGINT NOT NULL DEFAULT 0,
	[DateCreated] [DATETIME2] NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] [DATETIME2] NOT NULL DEFAULT SYSUTCDATETIME()
)
