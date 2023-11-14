--------------------------------------------
-- Schema Changes - May 6 2018
-- Remove Duplicate EORP from input
--------------------------------------------
ALTER TABLE [dbo].[SqliteActionBatch]
ADD [DuplicateExpenseCount] BIGINT NOT NULL DEFAULT 0
GO

ALTER TABLE [dbo].[SqliteActionBatch]
ADD [DuplicateOrderCount] BIGINT NOT NULL DEFAULT 0
GO

ALTER TABLE [dbo].[SqliteActionBatch]
ADD [DuplicateReturnCount] BIGINT NOT NULL DEFAULT 0
GO

ALTER TABLE [dbo].[SqliteActionBatch]
ADD [DuplicatePaymentCount] BIGINT NOT NULL DEFAULT 0
GO

ALTER PROCEDURE [dbo].[ProcessSqliteExpenseData]
	@batchId BIGINT
AS
BEGIN
	-- if batch is already processed - return	
	IF NOT EXISTS(SELECT 1 FROM dbo.SqliteActionBatch 
			WHERE Id = @batchId AND ExpenseLineSavedCount > 0
			AND BatchProcessed = 0)
	BEGIN
		RETURN;
	END

	DECLARE @NewExpenses TABLE
	(EmployeeId BIGINT,
	 DayId BIGINT,
	 TotalAmount DECIMAL(19,2)
	)

	-- First create Expense Record

	-- Fetch expense Date
	DECLARE @expenseDate DATETIME2
	SELECT @expenseDate = ExpenseDate
	FROM dbo.SqliteActionBatch 
	WHERE Id = @batchId AND ExpenseLineSavedCount > 0

	DECLARE @expenseId BIGINT = 0

	IF @expenseDate IS NOT NULL
	BEGIN
	   -- Create Day Record if not already there
	   DECLARE @dayId BIGINT
	   DECLARE @dt DATE = CAST(@expenseDate AS [Date])

		IF EXISTS(SELECT 1 FROM dbo.[Day] WHERE [Date] = @dt)
		BEGIN
			SELECT @dayId = Id FROM dbo.[Day] WHERE [Date] = @dt
		END
		ELSE
		BEGIN
			INSERT INTO dbo.[Day] ([Date]) VALUES (@dt)
			SET @dayId = SCOPE_IDENTITY()
		END

		-- Create a record in EmployeeDay table, if it does not already exist
		-- Ideally the record in EmployeeDay table should exist, but there could be
		-- error in tracking data capture
		INSERT INTO dbo.EmployeeDay
		(TenantEmployeeId, DayId, StartTime, EndTime)
		SELECT Distinct o.EmployeeId, d.Id, CAST(o.ExpenseDate as [Date]), CAST(o.ExpenseDate as [Date])
		FROM dbo.SqliteActionBatch o
		INNER JOIN dbo.[Day] d on d.[Date] = CAST(o.ExpenseDate as [DATE])
		AND o.Id = @batchId
		LEFT JOIN dbo.EmployeeDay ed on ed.TenantEmployeeId = o.EmployeeId
				  AND ed.DayId = d.Id
		WHERE ed.Id IS NULL

		-- identify duplicates first
		UPDATE b1
		SET ExpenseId = b2.ExpenseId,
		[Timestamp] = SYSUTCDATETIME()
		FROM dbo.SqliteActionBatch b1
		INNER JOIN dbo.SqliteActionBatch b2 on b1.ExpenseDate = b2.ExpenseDate
		AND b1.EmployeeId = b2.EmployeeId
		AND b1.TotalExpenseAmount = b2.TotalExpenseAmount
		AND b2.ExpenseId > 0
		AND b1.Id = @batchId

		DECLARE @dupRows BIGINT = @@RowCount
		IF @dupRows > 0
		BEGIN
		    SELECT @expenseId = ExpenseId FROM dbo.SqliteActionBatch WHERE ID = @batchId
			UPDATE dbo.SqliteActionBatch
			SET DuplicateExpenseCount = @dupRows,
			Timestamp = SYSUTCDATETIME()
			WHERE id = @batchId		
		END

		IF @dupRows > 0
		BEGIN
			-- Create expense Record
			INSERT INTO dbo.Expense
			(EmployeeId, DayId, TotalAmount)
			OUTPUT inserted.EmployeeId, inserted.DayId, inserted.TotalAmount INTO @NewExpenses
			SELECT EmployeeId, @dayId, TotalExpenseAmount
			FROM dbo.SqliteActionBatch WHERE Id = @batchId AND ExpenseId = 0
			SET @expenseId = SCOPE_IDENTITY()

			-- Now update the total amounts in EmployeeDay table
			UPDATE dbo.EmployeeDay 
			SET TotalExpenseAmount = TotalExpenseAmount + o.TotalAmount
			FROM dbo.EmployeeDay ed
			INNER JOIN 
				(SELECT employeeId, dayid, SUM(TotalAmount) TotalAmount FROM @NewExpenses GROUP BY employeeId, dayId) o 
			ON ed.TenantEmployeeId = o.EmployeeId AND ed.DayId = o.DayId

			-- Update batch record with expense Id
			UPDATE dbo.SqliteActionBatch
			SET ExpenseId = @expenseId,
			[Timestamp] = SYSUTCDATETIME()
			WHERE ID = @batchId AND ExpenseId = 0

			-- Now create expense line items
			INSERT INTO dbo.ExpenseItem
			(ExpenseId, SequenceNumber, ExpenseType, TransportType, Amount, OdometerStart, OdometerEnd, ImageCount, FuelType, FuelQuantityInLiters, Comment)
			SELECT @expenseId, SequenceNumber, ExpenseType, VehicleType, Amount, OdometerStart, OdometerEnd, ImageCount, FuelType, FuelQuantityInLiters, Comment
			FROM dbo.SqliteExpense e
			WHERE e.BatchId = @batchId
			AND @expenseId > 0
		END

		-- Update ExpenseItemId in SqliteExpense
		-- (even for duplicate record)
		UPDATE dbo.SqliteExpense
		SET ExpenseItemId = ei.Id,
		IsProcessed = 1,
		DateUpdated = SYSUTCDATETIME()
		FROM dbo.SqliteExpense sle
		INNER JOIN dbo.ExpenseItem ei on sle.SequenceNumber = ei.SequenceNumber
		AND sle.BatchId = @batchId
		AND ei.ExpenseId = @expenseId

		IF @dupRows > 0
		BEGIN
			-- Now Images
			INSERT INTO dbo.[Image]
			(SourceId, [ImageFileName])  -- source id is used in next query listed below
			SELECT sei.id, sei.[ImageFileName]
			FROM dbo.SqliteExpenseImage sei
			INNER JOIN dbo.SqliteExpense se on sei.SqliteExpenseId = se.Id
			AND se.BatchId = @batchId
			AND @expenseId > 0
		
			-- Now create entries in ExpenseItemImage
			INSERT INTO dbo.ExpenseItemImage
			(ExpenseItemId, ImageId, SequenceNumber)
			SELECT ei.Id,i.[Id], sei.SequenceNumber
			FROM dbo.SqliteExpenseImage sei
			INNER JOIN dbo.[Image] i on sei.Id = i.SourceId
			INNER JOIN dbo.SqliteExpense sle on sei.SqliteExpenseId = sle.Id
			INNER JOIN dbo.ExpenseItem ei on sle.SequenceNumber = ei.SequenceNumber
			AND sle.BatchId = @batchId
			AND ei.ExpenseId = @expenseId
		END
	END
END
GO

---------

ALTER PROCEDURE [dbo].[ProcessSqliteOrderData]
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
	 TotalAmount DECIMAL(19,2)
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
	(EmployeeId, DayId, CustomerCode, OrderType, OrderDate, TotalAmount, ItemCount, SqliteOrderId, RevisedTotalAmount)
	OUTPUT inserted.EmployeeId, inserted.DayId, inserted.TotalAmount INTO @NewOrders
	SELECT EmployeeId, d.Id, CustomerCode, o.OrderType, CAST(o.OrderDate as [DATE]), o.TotalAmount, o.ItemCount, o.Id, o.TotalAmount
	FROM dbo.SqliteOrder o
	INNER JOIN dbo.[Day] d ON d.[DATE] = CAST(o.OrderDate AS [Date])
	WHERE BatchId = @batchId AND o.IsProcessed = 0
	ORDER BY o.Id

	-- Now update the total amounts in EmployeeDay table
	UPDATE dbo.EmployeeDay 
	SET TotalOrderAmount = TotalOrderAmount + o.TotalAmount
	FROM dbo.EmployeeDay ed
	INNER JOIN 
	     (SELECT employeeId, dayid, SUM(TotalAmount) TotalAmount FROM @NewOrders GROUP BY employeeId, dayId) o 
	ON ed.TenantEmployeeId = o.EmployeeId AND ed.DayId = o.DayId

	-- now we need to copy line items
	-- (Similarly at the time of batch processes, set Revised Unit Quantity and RevisedAmount to UnitQuantity and Amount)
	INSERT INTO dbo.OrderItem
	(OrderId, SerialNumber, ProductCode, UnitQuantity, UnitPrice, Amount, RevisedUnitQuantity, RevisedAmount)
	SELECT o.Id, soi.SerialNumber, soi.ProductCode, soi.UnitQuantity, soi.UnitPrice, soi.Amount, soi.UnitQuantity, soi.Amount
	FROM dbo.SqliteOrderItem soi
	INNER JOIN dbo.SqliteOrder so on soi.SqliteOrderId = so.Id
	AND so.BatchId = @batchId AND so.IsProcessed = 0
	INNER JOIN dbo.[Order] o on so.Id = o.SqliteOrderId

	-- now we need to update the orderId back in SqliteOrder table
	UPDATE dbo.SqliteOrder
	SET OrderId = o.Id,
	IsProcessed = 1,
	DateUpdated = SYSUTCDATETIME()
	FROM dbo.SqliteOrder so
	INNER JOIN dbo.[Order] o on so.Id = o.SqliteOrderId
	AND so.BatchId = @batchId
END
GO

------------------

ALTER PROCEDURE [dbo].[ProcessSqlitePaymentData]
	@batchId BIGINT
AS
BEGIN

	-- if batch is already processed - return	
	IF NOT EXISTS(SELECT 1 FROM dbo.SqliteActionBatch 
			WHERE Id = @batchId AND NumberOfPaymentsSaved > 0
			AND BatchProcessed = 0)
	BEGIN
		RETURN;
	END

	DECLARE @NewPayments TABLE
	(EmployeeId BIGINT,
	 DayId BIGINT,
	 TotalAmount DECIMAL(19,2)
	)

	-- First create required records in Day Table
	INSERT INTO dbo.[Day]
	([DATE])
	SELECT DISTINCT CAST(p.PaymentDate AS [Date])
	FROM dbo.SqlitePayment p
	LEFT JOIN dbo.[Day] d on CAST(p.PaymentDate AS [Date]) = d.[DATE]
	WHERE p.BatchId = @batchId
	AND d.[DATE] IS NULL

	-- Create records in EmployeeDay table if it not already exist.
	INSERT INTO dbo.EmployeeDay
	(TenantEmployeeId, DayId, StartTime, EndTime)
	SELECT Distinct o.EmployeeId, d.Id, CAST(o.PaymentDate as [Date]), CAST(o.PaymentDate as [Date])
	FROM dbo.SqlitePayment o
	INNER JOIN dbo.[Day] d on d.[Date] = CAST(o.PaymentDate as [DATE])
	AND o.BatchId = @batchId
	LEFT JOIN dbo.EmployeeDay ed on ed.TenantEmployeeId = o.EmployeeId
	          AND ed.DayId = d.Id
	WHERE ed.Id IS NULL

	-- Identify duplicate payment records that have already come in some other batch
	UPDATE sp
	SET PaymentId = p.PaymentId,
	IsProcessed = 1,
	DateUpdated = SYSUTCDATETIME()
	FROM dbo.SqlitePayment sp
	INNER JOIN dbo.[SqlitePayment] p on sp.PaymentDate = p.PaymentDate
	AND p.EmployeeId = sp.EmployeeId
	AND p.CustomerCode = sp.CustomerCode
	AND p.PaymentType = sp.PaymentType
	AND p.TotalAmount = sp.TotalAmount
	AND p.PaymentId > 0
	AND sp.BatchId = @batchId

	DECLARE @dupRows BIGINT = @@RowCount
	IF @dupRows > 0
	BEGIN
		UPDATE dbo.SqliteActionBatch
		SET DuplicatePaymentCount = @dupRows,
		Timestamp = SYSUTCDATETIME()
		WHERE id = @batchId		
	END

	-- Create Payment Records
	INSERT INTO dbo.[Payment]
	(EmployeeId, DayId, CustomerCode, PaymentType, PaymentDate, TotalAmount, Comment, ImageCount, SqlitePaymentId)
	OUTPUT inserted.EmployeeId, inserted.DayId, inserted.TotalAmount INTO @NewPayments
	SELECT EmployeeId, d.Id, CustomerCode, p.PaymentType, CAST(p.PaymentDate as [DATE]), p.TotalAmount, p.Comment, p.ImageCount, p.Id
	FROM dbo.SqlitePayment p
	INNER JOIN dbo.[Day] d ON d.[DATE] = CAST(p.PaymentDate AS [Date])
	WHERE BatchId = @batchId AND p.IsProcessed = 0
	ORDER BY p.Id

	-- Now update the total amounts in EmployeeDay table
	UPDATE dbo.EmployeeDay 
	SET TotalPaymentAmount = TotalPaymentAmount + o.TotalAmount
	FROM dbo.EmployeeDay ed
	INNER JOIN 
	     (SELECT employeeId, dayid, SUM(TotalAmount) TotalAmount FROM @NewPayments GROUP BY employeeId, dayId) o 
	 ON ed.TenantEmployeeId = o.EmployeeId AND ed.DayId = o.DayId

	-- Now Images
	INSERT INTO dbo.[Image]
	(SourceId, PaymentSourceId, [ImageFileName])  -- Payment source id is used in next query listed below
	SELECT 0, spi.id, spi.[ImageFileName]
	FROM dbo.SqlitePaymentImage spi
	INNER JOIN dbo.SqlitePayment sp on spi.SqlitePaymentId = sp.Id
	AND sp.BatchId = @batchId
	AND sp.IsProcessed = 0
		
	-- Now create entries in PaymentImage
	INSERT INTO dbo.PaymentImage
	(PaymentId, ImageId, SequenceNumber)
	SELECT p.Id, i.[Id], spi.SequenceNumber
	FROM dbo.SqlitePaymentImage spi
	INNER JOIN dbo.[Image] i on spi.Id = i.PaymentSourceId
	INNER JOIN dbo.SqlitePayment slp on spi.SqlitePaymentId = slp.Id
	AND slp.BatchId = @batchId
	INNER JOIN dbo.Payment p on slp.Id = p.SqlitePaymentId

	-- now we need to update the Payment back in SqlitePayment table
	UPDATE dbo.SqlitePayment
	SET PaymentId = p.Id,
	IsProcessed = 1,
	DateUpdated = SYSUTCDATETIME()
	FROM dbo.SqlitePayment sp
	INNER JOIN dbo.[Payment] p on sp.Id = p.SqlitePaymentId
	AND sp.BatchId = @batchId
END
GO

----------

ALTER PROCEDURE [dbo].[ProcessSqliteReturnOrderData]
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
GO
