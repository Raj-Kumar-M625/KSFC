CREATE PROCEDURE [dbo].[PostStockLedgerFromInput]
	@stockInputTagId BIGINT,
	@updatedBy NVARCHAR(50)
AS
BEGIN

	DECLARE @trn TABLE
	(
		[TransactionDate] DATE NOT NULL,
		[ItemMasterId] BIGINT NOT NULL,
		[ReferenceNo] NVARCHAR(20) NOT NULL,
		[LineNumber] INT NOT NULL,
		[Quantity] INT NOT NULL,
		[ZoneCode] NVARCHAR(50) NOT NULL,
		[AreaCode] NVARCHAR(50) NOT NULL,
		[TerritoryCode] NVARCHAR(50) NOT NULL,
		[HQCode] NVARCHAR(50) NOT NULL,
		[StaffCode] NVARCHAR(10) NOT NULL,
		[CyclicCount] BIGINT NOT NULL DEFAULT 1,
		[CreatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
		[UpdatedBy] NVARCHAR(50) NOT NULL DEFAULT ''
	)

	-- put transaction entries in an in-memory table.
	INSERT INTO @trn
	(TransactionDate, ItemMasterId, ReferenceNo, LineNumber, Quantity, 
	ZoneCode, AreaCode, TerritoryCode, HQCode, StaffCode, 
	CyclicCount, CreatedBy, UpdatedBy)
	SELECT
	tag.ReceiveDate, si.ItemMasterId, tag.GRNNumber, si.LineNumber, si.Quantity,
	tag.ZoneCode, tag.AreaCode, tag.TerritoryCode, tag.HQCode, '',
	1, @updatedBy, @updatedBy
	FROM dbo.StockInputTag tag
	INNER JOIN StockInput si on tag.Id = si.StockInputTagId
	AND tag.Id = @stockInputTagId
	AND si.Quantity > 0

	-- Create entries in StockLedger based on the stock received from vendors
	INSERT INTO dbo.StockLedger
	(TransactionDate, ItemMasterId, ReferenceNo, LineNumber, ReceiveQuantity, 
	ZoneCode, AreaCode, TerritoryCode, HQCode, StaffCode, 
	CyclicCount, CreatedBy, UpdatedBy)
	SELECT
	TransactionDate, ItemMasterId, ReferenceNo, LineNumber, Quantity, 
	ZoneCode, AreaCode, TerritoryCode, HQCode, StaffCode, 
	CyclicCount, CreatedBy, UpdatedBy
	FROM @trn


	-- Update Existing entries in StockBalance
	UPDATE dbo.StockBalance
	SET StockQuantity = b.StockQuantity + t.Quantity,
	CyclicCount = b.CyclicCount + 1,
	UpdatedBy = @updatedBy,
	DateUpdated = SysUtcDateTime()
	FROM dbo.StockBalance b
	INNER JOIN @trn t ON t.ItemMasterId = b.ItemMasterId
	AND t.ZoneCode = b.ZoneCode
	AND t.AreaCode = b.AreaCode
	AND t.TerritoryCode = b.TerritoryCode
	AND t.HQCode = b.HQCode
	AND t.StaffCode = b.StaffCode

	-- Create new entries in StockBalance
	INSERT INTO dbo.StockBalance
	(ItemMasterId, StockQuantity, 
	ZoneCode, AreaCode, TerritoryCode, HQCode, StaffCode, 
	CyclicCount, CreatedBy, UpdatedBy)
	SELECT t.ItemMasterId, t.Quantity,
	t.ZoneCode, t.AreaCode, t.TerritoryCode, t.HQCode, t.StaffCode, 
	1, @updatedBy, @updatedBy
	FROM @trn t
	LEFT JOIN dbo.StockBalance b ON t.ItemMasterId = b.ItemMasterId
	AND t.ZoneCode = b.ZoneCode
	AND t.AreaCode = b.AreaCode
	AND t.TerritoryCode = b.TerritoryCode
	AND t.HQCode = b.HQCode
	AND t.StaffCode = b.StaffCode
	WHERE b.Id IS NULL
END
GO
