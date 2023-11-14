CREATE PROCEDURE [dbo].[ProcessSqliteReturnOrderData]
	@batchId BIGINT
AS
BEGIN
	-- if batch is already processed - return	
	IF NOT EXISTS(SELECT 1 FROM dbo.SqliteActionBatch 
			WHERE Id = @batchId AND NumberOfReturnsSaved > 0
			AND BatchProcessed = 0)
	BEGIN
		RETURN;
	END

	DECLARE @NewReturnOrders TABLE
	(EmployeeId BIGINT,
	 DayId BIGINT,
	 TotalAmount DECIMAL(19,2)
	)

	-- to aid in processing, we have added a column SqliteReturnOrderId in dbo.ReturnOrder table;

	-- First create required records in Day Table
	INSERT INTO dbo.[Day]
	([DATE])
	SELECT DISTINCT CAST(o.ReturnOrderDate AS [Date])
	FROM dbo.SqliteReturnOrder o
	LEFT JOIN dbo.[Day] d on CAST(o.ReturnOrderDate AS [Date]) = d.[DATE]
	WHERE o.BatchId = @batchId
	AND d.[DATE] IS NULL

	-- Create records in EmployeeDay table if it not already exist.
	INSERT INTO dbo.EmployeeDay
	(TenantEmployeeId, DayId, StartTime, EndTime)
	SELECT Distinct o.EmployeeId, d.Id, CAST(o.ReturnOrderDate as [Date]), CAST(o.ReturnOrderDate as [Date])
	FROM dbo.SqliteReturnOrder o
	INNER JOIN dbo.[Day] d on d.[Date] = CAST(o.ReturnOrderDate as [DATE])
	AND o.BatchId = @batchId
	LEFT JOIN dbo.EmployeeDay ed on ed.TenantEmployeeId = o.EmployeeId
	          AND ed.DayId = d.Id
	WHERE ed.Id IS NULL

	-- Find out duplicate ReturnOrders first and set their id to original id
	UPDATE so
	SET ReturnOrderId = o.ReturnOrderId,
	IsProcessed = 1,
	DateUpdated = SYSUTCDATETIME()
	FROM dbo.SqliteReturnOrder so
	INNER JOIN dbo.SqliteReturnOrder o on so.ReturnOrderDate = o.ReturnOrderDate AND
	o.EmployeeId = so.EmployeeId AND
	o.ItemCount = so.ItemCount AND
	o.TotalAmount = so.TotalAmount AND
	o.CustomerCode = so.CustomerCode AND
	o.ReturnOrderId > 0 AND
	so.BatchId = @batchId

	DECLARE @dupRows BIGINT = @@RowCount
	IF @dupRows > 0
	BEGIN
		UPDATE dbo.SqliteActionBatch
		SET DuplicateReturnCount = @dupRows,
		Timestamp = SYSUTCDATETIME()
		WHERE id = @batchId		
	END

	-- Now create records in dbo.ReturnOrder Table
	INSERT INTO dbo.[ReturnOrder]
	(EmployeeId, DayId, CustomerCode, ReturnOrderDate, TotalAmount, ItemCount, SqliteReturnOrderId, ReferenceNumber, Comment)
	OUTPUT inserted.EmployeeId, inserted.DayId, inserted.TotalAmount INTO @NewReturnOrders
	SELECT EmployeeId, d.Id, CustomerCode, CAST(o.ReturnOrderDate as [DATE]), o.TotalAmount, o.ItemCount, o.Id, o.ReferenceNum, o.Comment
	FROM dbo.SqliteReturnOrder o
	INNER JOIN dbo.[Day] d ON d.[DATE] = CAST(o.ReturnOrderDate AS [Date])
	WHERE BatchId = @batchId AND IsProcessed = 0
	ORDER BY o.Id

	-- Now update the total amounts in EmployeeDay table
	UPDATE dbo.EmployeeDay 
	SET TotalReturnAmount = TotalReturnAmount + o.TotalAmount
	FROM dbo.EmployeeDay ed
	INNER JOIN 
		(SELECT employeeId, dayid, SUM(TotalAmount) TotalAmount FROM @NewReturnOrders GROUP BY employeeId, dayId) o 
	ON ed.TenantEmployeeId = o.EmployeeId AND ed.DayId = o.DayId

	-- now we need to copy line items
	INSERT INTO dbo.ReturnOrderItem
	(ReturnOrderId, SerialNumber, ProductCode, UnitQuantity, UnitPrice, Amount, Comment)
	SELECT o.Id, soi.SerialNumber, soi.ProductCode, soi.UnitQuantity, soi.UnitPrice, soi.Amount, soi.Comment
	FROM dbo.SqliteReturnOrderItem soi
	INNER JOIN dbo.SqliteReturnOrder so on soi.SqliteReturnOrderId = so.Id
	AND so.BatchId = @batchId AND IsProcessed = 0
	INNER JOIN dbo.[ReturnOrder] o on so.Id = o.SqliteReturnOrderId

	-- now we need to update the ReturnOrderId back in SqliteReturnOrder table
	UPDATE dbo.SqliteReturnOrder
	SET ReturnOrderId = o.Id,
	IsProcessed = 1,
	DateUpdated = SYSUTCDATETIME()
	FROM dbo.SqliteReturnOrder so
	INNER JOIN dbo.[ReturnOrder] o on so.Id = o.SqliteReturnOrderId
	AND so.BatchId = @batchId
END

