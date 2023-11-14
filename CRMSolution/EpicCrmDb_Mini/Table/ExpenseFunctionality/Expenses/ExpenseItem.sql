CREATE TABLE [dbo].[ExpenseItem]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[ExpenseId] BIGINT NOT NULL REFERENCES Expense,
	[SequenceNumber] INT NOT NULL,
	[ExpenseType] NVARCHAR(20) NOT NULL, -- Lodge/Internet/Travel-Public/Travel-Private (ExpenseType)
	         -- if it is Travel-Public or Travel-Private, look into transport Type
	[TransportType] NVARCHAR(20), -- Air/Bus/Train/Taxi/Auto (for Travel-Public) / 2 Wheeler (for Travel-Private)
	-- following two fields will be filled only for travel-private
	[Amount] DECIMAL(19,2) NOT NULL,
	[DeductedAmount] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[RevisedAmount] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[OdometerStart] BIGINT NOT NULL DEFAULT 0,
	[OdometerEnd] BIGINT NOT NULL DEFAULT 0,
	[ImageCount] INT NOT NULL DEFAULT 0,
	[FuelType] NVARCHAR(50),
	[FuelQuantityInLiters] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Comment] NVARCHAR(2048)
)
