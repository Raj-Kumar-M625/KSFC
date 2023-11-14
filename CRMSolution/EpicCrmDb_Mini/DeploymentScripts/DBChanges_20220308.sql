ALTER TABLE [dbo].[ExpenseItem]
ADD
	[DeductedAmount] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[RevisedAmount] DECIMAL(19,2) NOT NULL DEFAULT 0
GO

UPDATE [dbo].[ExpenseItem]
SET [RevisedAmount] = [Amount]

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
			SET DuplicateExpenseCount = ExpenseLineSavedCount,
			Timestamp = SYSUTCDATETIME()
			WHERE id = @batchId
		END

		IF @dupRows = 0
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
			(ExpenseId, SequenceNumber, ExpenseType, TransportType, Amount, OdometerStart, OdometerEnd, ImageCount, FuelType, FuelQuantityInLiters, Comment, DeductedAmount, RevisedAmount)
			SELECT @expenseId, SequenceNumber, ExpenseType, VehicleType, Amount, OdometerStart, OdometerEnd, ImageCount, FuelType, FuelQuantityInLiters, Comment, 0, Amount
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

		IF @dupRows = 0
		BEGIN
			UPDATE dbo.[Image] SET SourceId = 0 WHERE SourceId > 0

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

