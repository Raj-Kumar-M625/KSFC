CREATE PROCEDURE [dbo].[ProcessSqliteOrderData]
	@batchId BIGINT
AS
BEGIN
	-- if batch is already processed - return	
	IF NOT EXISTS(SELECT 1 FROM dbo.SqliteActionBatch 
			WHERE Id = @batchId AND NumberOfOrdersSaved > 0
			AND BatchProcessed = 0)
	BEGIN
		RETURN;
	END

	DECLARE @NewOrders TABLE
	(EmployeeId BIGINT,
	 DayId BIGINT,
	 TotalAmount DECIMAL(19,2),
	 TotalGST DECIMAL(19,2),
	 NetAmount DECIMAL(19,2)
	)
	
	-- to aid in processing, we have added a column SqliteOrderId in dbo.Order table;

	-- First create required records in Day Table
	INSERT INTO dbo.[Day]
	([DATE])
	SELECT DISTINCT CAST(o.OrderDate AS [Date])
	FROM dbo.SqliteOrder o
	LEFT JOIN dbo.[Day] d on CAST(o.OrderDate AS [Date]) = d.[DATE]
	WHERE o.BatchId = @batchId
	AND d.[DATE] IS NULL

	-- Create records in EmployeeDay table if it not already exist.
	-- (for employeeday created here, we are setting end time as well
	--  so that when user ends the day, we can book the end day in
	--  last day that was actually started);
	INSERT INTO dbo.EmployeeDay
	(TenantEmployeeId, DayId, StartTime, EndTime)
	SELECT Distinct o.EmployeeId, d.Id, CAST(o.OrderDate as [Date]), CAST(o.OrderDate as [Date])
	FROM dbo.SqliteOrder o
	INNER JOIN dbo.[Day] d on d.[Date] = CAST(o.OrderDate as [DATE])
	AND o.BatchId = @batchId
	LEFT JOIN dbo.EmployeeDay ed on ed.TenantEmployeeId = o.EmployeeId
	          AND ed.DayId = d.Id
	WHERE ed.Id IS NULL

	-- Find out duplicate orders first and set their order id to original order id
	UPDATE so
	SET OrderId = o.OrderId,
	IsProcessed = 1,
	DateUpdated = SYSUTCDATETIME()
	FROM dbo.[SqliteOrder] so
	INNER JOIN dbo.[SqliteOrder] o on so.OrderDate = o.OrderDate AND
	o.EmployeeId = so.EmployeeId AND
	o.ItemCount = so.ItemCount AND
	o.TotalAmount = so.TotalAmount AND
	o.CustomerCode = so.CustomerCode AND
	o.OrderId > 0 AND
	so.BatchId = @batchId

	DECLARE @dupRows BIGINT = @@RowCount
	IF @dupRows > 0
	BEGIN
		UPDATE dbo.SqliteActionBatch
		SET DuplicateOrderCount = @dupRows,
		Timestamp = SYSUTCDATETIME()
		WHERE id = @batchId		
	END

	-- Now create records in dbo.Order Table
	-- (At the time of processing batches, set RevisedTotalAmount to TotalAmount)
	INSERT INTO dbo.[Order]
	(EmployeeId, DayId, CustomerCode, OrderType, OrderDate, TotalAmount, 
	TotalGST, NetAmount, DiscountType, DiscountPercentage,
	ItemCount, SqliteOrderId, RevisedTotalAmount,
	RevisedTotalGST, RevisedNetAmount, RevisedDiscountPercentage, ImageCount
	)

	OUTPUT inserted.EmployeeId, inserted.DayId, inserted.TotalAmount,
	       inserted.TotalGST, inserted.NetAmount INTO @NewOrders

	SELECT EmployeeId, d.Id, CustomerCode, o.OrderType, CAST(o.OrderDate as [DATE]), o.TotalAmount, 
	o.TotalGST, o.NetAmount, o.DiscountType, o.AppliedDiscountPercentage,
	o.ItemCount, o.Id, o.TotalAmount,
	o.TotalGST, o.NetAmount, o.AppliedDiscountPercentage, o.ImageCount
	FROM dbo.SqliteOrder o
	INNER JOIN dbo.[Day] d ON d.[DATE] = CAST(o.OrderDate AS [Date])
	WHERE BatchId = @batchId AND o.IsProcessed = 0
	ORDER BY o.Id

	-- Now update the total amounts in EmployeeDay table
	UPDATE dbo.EmployeeDay 
	SET TotalOrderAmount = TotalOrderAmount + o.NetAmount
	FROM dbo.EmployeeDay ed
	INNER JOIN 
	     (SELECT employeeId, dayid, ISNULL(SUM(NetAmount),0) NetAmount FROM @NewOrders GROUP BY employeeId, dayId) o 
	ON ed.TenantEmployeeId = o.EmployeeId AND ed.DayId = o.DayId

	-- now we need to copy line items
	-- (Similarly at the time of batch processes, set Revised Unit Quantity and RevisedAmount to UnitQuantity and Amount)
	INSERT INTO dbo.OrderItem
	(OrderId, SerialNumber, ProductCode, UnitQuantity, UnitPrice,
	DiscountPercent, DiscountedPrice, ItemPrice, GstPercent, GstAmount, NetPrice,
	 Amount, RevisedUnitQuantity, RevisedAmount,
	 RevisedDiscountPercent, RevisedDiscountedPrice, RevisedItemPrice, RevisedGstPercent, RevisedGstAmount, RevisedNetPrice
	 )
	SELECT o.Id, soi.SerialNumber, soi.ProductCode, soi.UnitQuantity, soi.UnitPrice,
	soi.DiscountPercent, soi.DiscountedPrice, soi.ItemPrice, soi.GstPercent, soi.GstAmount, soi.NetPrice,
	 soi.Amount, soi.UnitQuantity, soi.Amount,
	 soi.DiscountPercent, soi.DiscountedPrice, soi.ItemPrice, soi.GstPercent, soi.GstAmount, soi.NetPrice
	FROM dbo.SqliteOrderItem soi
	INNER JOIN dbo.SqliteOrder so on soi.SqliteOrderId = so.Id
	AND so.BatchId = @batchId AND so.IsProcessed = 0
	INNER JOIN dbo.[Order] o on so.Id = o.SqliteOrderId

	-- Now Images
	UPDATE dbo.[Image] SET SourceId = 0 WHERE SourceId > 0

	INSERT INTO dbo.[Image]
	(SourceId, [ImageFileName])  -- Order source id is used in next query listed below
	SELECT soi.Id, soi.[ImageFileName]
	FROM dbo.SqliteOrderImage soi
	INNER JOIN dbo.SqliteOrder so on soi.SqliteOrderId = so.Id
	AND so.BatchId = @batchId
	AND so.IsProcessed = 0
		
	-- Now create entries in OrderImage
	INSERT INTO dbo.OrderImage
	(OrderId, ImageId, SequenceNumber)
	SELECT o.Id, i.[Id], soi.SequenceNumber
	FROM dbo.SqliteOrderImage soi
	INNER JOIN dbo.[Image] i on soi.Id = i.SourceId
	INNER JOIN dbo.SqliteOrder slo on soi.SqliteOrderId = slo.Id
	AND slo.BatchId = @batchId
	INNER JOIN dbo.[Order] o on slo.Id = o.SqliteOrderId

	-- now we need to update the orderId back in SqliteOrder table
	UPDATE dbo.SqliteOrder
	SET OrderId = o.Id,
	IsProcessed = 1,
	DateUpdated = SYSUTCDATETIME()
	FROM dbo.SqliteOrder so
	INNER JOIN dbo.[Order] o on so.Id = o.SqliteOrderId
	AND so.BatchId = @batchId
END

