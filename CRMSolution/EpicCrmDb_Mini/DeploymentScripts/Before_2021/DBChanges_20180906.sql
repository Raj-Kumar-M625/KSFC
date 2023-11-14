-- Changes for order pipeline
ALTER TABLE dbo.SqliteOrder
	ADD [TotalGST] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[NetAmount] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[MaxDiscountPercentage] DECIMAL(5,2) NOT NULL DEFAULT 0,
	[DiscountType] NVARCHAR(50) NOT NULL DEFAULT 'Amount',
	[AppliedDiscountPercentage] DECIMAL(5,2) NOT NULL DEFAULT 0,
	[ImageCount] INT NOT NULL DEFAULT 0
GO

ALTER TABLE dbo.SqliteOrderItem
ADD
	[DiscountPercent] DECIMAL(5,2) NOT NULL DEFAULT 0,
	[DiscountedPrice] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[ItemPrice] DECIMAL(19,2) NOT NULL DEFAULT 0,  -- DiscountedPrice * Qty
	[GstPercent] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[GstAmount] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[NetPrice] DECIMAL(19,2) NOT NULL DEFAULT 0
GO

CREATE TABLE [dbo].[SqliteOrderImage]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[SqliteOrderId] BIGINT NOT NULL REFERENCES SqliteOrder,
	[SequenceNumber] INT NOT NULL DEFAULT 0,  -- this value is added by code, but not using it subsequently.
	[ImageFileName] VARCHAR(100) NOT NULL DEFAULT 'FileNotFound.jpg'
)
GO

ALTER TABLE dbo.[Order]
ADD
	[ImageCount] INT NOT NULL DEFAULT 0,

	[TotalGST] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[NetAmount] DECIMAL(19,2) NOT NULL DEFAULT 0,

	[DiscountType] NVARCHAR(50) NOT NULL DEFAULT 'Amount',
	[DiscountPercentage] DECIMAL(5,2) NOT NULL DEFAULT 0,
	[RevisedDiscountPercentage] DECIMAL(5,2) NOT NULL DEFAULT 0,
	
	[RevisedTotalGST] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[RevisedNetAmount] DECIMAL(19,2) NOT NULL DEFAULT 0
GO

ALTER TABLE dbo.[OrderItem]
ADD
	[DiscountPercent] DECIMAL(5,2) NOT NULL DEFAULT 0,
	[DiscountedPrice] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[ItemPrice] DECIMAL(19,2) NOT NULL DEFAULT 0,  -- DiscountedPrice * Qty
	[GstPercent] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[GstAmount] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[NetPrice] DECIMAL(19,2) NOT NULL DEFAULT 0,

	[RevisedDiscountPercent] DECIMAL(5,2) NOT NULL DEFAULT 0,
	[RevisedDiscountedPrice] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[RevisedItemPrice] DECIMAL(19,2) NOT NULL DEFAULT 0,  -- RevisedDiscountedPrice * Qty
	[RevisedGstPercent] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[RevisedGstAmount] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[RevisedNetPrice] DECIMAL(19,2) NOT NULL DEFAULT 0
GO

CREATE TABLE [dbo].[OrderImage]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[OrderId] BIGINT NOT NULL REFERENCES dbo.[Order],
	[ImageId] BIGINT NOT NULL References [Image],
	[SequenceNumber] INT NOT NULL
)
GO

ALTER TABLE dbo.[Image]
ADD [OrderSourceId] BIGINT NOT NULL DEFAULT 0 -- used only during processing of Order Data
GO

IF OBJECT_ID ( '[dbo].[GetSqliteActionBatchForProcessing]', 'P' ) IS NOT NULL   
    DROP PROCEDURE [dbo].[GetSqliteActionBatchForProcessing];  
GO 

CREATE PROCEDURE [dbo].[GetSqliteActionBatchForProcessing]
	@recordCount int,
	@tenantId BIGINT,
	@employeeId BIGINT
AS
BEGIN
    DECLARE @currentDateTime DATETIME2 = SYSUTCDATETIME();

	-- update mobileDataProcessingAt as record is picked for processing
	-- this is to create a sliding expiration of 15 minutes, before tenant record
	-- is considered in hanged state;
	UPDATE dbo.Tenant
	SET MobileDataProcessingAt = @currentDateTime
	WHERE Id = @tenantId

	;WITH batchCTE(batchId)
	AS
	(
		-- join tenantEmployee and batch tables
		-- to find all batches for given tenant + employee (?)
		-- Order the batches by employeeId and Time
		-- Select Top count
		-- (only active tenant/employees are selected)
		SELECT TOP(@recordCount) st.Id  -- batchId
		FROM dbo.TenantEmployee te WITH (NOLOCK)
		INNER JOIN dbo.SqliteActionBatch st WITH (READPAST) ON te.Id = st.EmployeeId
		AND te.IsActive = 1
		AND st.UnderConstruction = 0  -- batch should be fully constructed and ready for processing
		INNER JOIN dbo.Tenant t ON t.Id = te.TenantId
		AND t.IsActive = 1
		WHERE te.TenantId = @tenantId
		AND (@employeeId = -1 OR te.Id = @employeeId)
		--AND ((st.BatchProcessed = 0 And st.LockTimestamp IS NULL) OR
		--(st.LockTimeStamp Is NOT NULL AND DATEDIFF(mi, st.LockTimestamp, @currentDateTime) >= 5))

		AND (st.BatchProcessed = 0 And st.LockTimestamp IS NULL)
		--(st.LockTimeStamp Is NOT NULL AND DATEDIFF(mi, st.LockTimestamp, @currentDateTime) >= 5))

		ORDER BY st.EmployeeId, st.[At]
	)
	-- Lock the selected batch records
	UPDATE dbo.SqliteActionBatch
	SET LockTimestamp = @currentDateTime
	OUTPUT inserted.Id BatchId
	FROM dbo.SqliteActionBatch b
	INNER JOIN batchCTE cte ON b.Id = cte.batchId
END
GO

IF OBJECT_ID ( 'dbo.ClearEmployeeData', 'P' ) IS NOT NULL   
    DROP PROCEDURE dbo.ClearEmployeeData;  
GO  

CREATE PROCEDURE [dbo].[ClearEmployeeData]
	@employeeId BIGINT
AS
BEGIN
	BEGIN TRY

		BEGIN TRANSACTION

		-- Delete Activity Data
		DECLARE @activity Table ( Id BIGINT )
		DECLARE @image TABLE (Id BIGINT)

		-- first find out the activity ids that need to be deleted
		INSERT INTO @activity (Id)
		SELECT a.id from dbo.Activity a
		INNER JOIN dbo.EmployeeDay ed on ed.Id = a.employeeDayId
		and ed.TenantEmployeeId = @employeeId

		-- delete activity images
		-- store image Ids first - delete images at the end, as we need to delete images for payment as well;
		INSERT INTO @image (Id)
		SELECT ImageId FROM dbo.ActivityImage ai 
					 INNER JOIN @activity a on ai.ActivityId = a.Id

		DELETE FROM dbo.ActivityImage
		WHERE ActivityId IN (SELECT id FROM @activity)

		DELETE from dbo.Activity
		WHERE id in (SELECT Id from @activity)

		-----------------

		DELETE from dbo.DistanceCalcErrorLog
		WHERE id in
		(SELECT l.id from dbo.distanceCalcErrorLog l
		INNER JOIN dbo.Tracking t on l.TrackingId = t.Id
		INNER JOIN dbo.EmployeeDay ed on ed.Id = t.employeeDayId
		and ed.TenantEmployeeId = @employeeId)

		DELETE from dbo.Tracking
		WHERE id in
		(SELECT t.id from dbo.Tracking t
		INNER JOIN dbo.EmployeeDay ed on ed.Id = t.employeeDayId
		and ed.TenantEmployeeId = @employeeId)

		DELETE from dbo.employeeDay WHERE TenantEmployeeId = @employeeId

		DELETE from dbo.Imei WHERE TenantEmployeeId = @employeeId


		--DELETE FROM dbo.SqliteAction WHERE EmployeeId = @employeeId
		--DELETE FROM dbo.SqliteActionBatch WHERE EmployeeId = @employeeId

		-- delete expense Data as well
		DELETE FROM dbo.SqliteExpenseImage WHERE SqliteExpenseId in (SELECT Id FROM dbo.SqliteExpense WHERE EmployeeId = @employeeId)
		DELETE FROM dbo.SqliteExpense WHERE EmployeeId = @employeeId

		-- Delete SqliteOrder data
		DELETE FROM dbo.SqliteOrderItem WHERE SqliteOrderId IN (SELECT Id FROM dbo.[SqliteOrder] WHERE EmployeeId = @employeeId)
		DELETE FROM dbo.SqliteOrderImage WHERE SqliteOrderId IN (SELECT Id FROM dbo.[SqliteOrder] WHERE EmployeeId = @employeeId)
		DELETE FROM dbo.[SqliteOrder] WHERE EmployeeId = @employeeId

		-- delete SqlLiteAction Data as well
		DELETE FROM dbo.SqliteActionImage where SqliteActionId in (SELECT id FROM dbo.SqliteAction WHERE EmployeeId = @employeeId)
		DELETE FROM dbo.SqliteAction WHERE EmployeeId = @employeeId
		DELETE FROM dbo.SqliteActionDup WHERE EmployeeId = @employeeId

		-- delete SqlitePayment data as well
		DELETE FROM dbo.SqlitePaymentImage WHERE SqlitePaymentId in (SELECT Id FROM dbo.SqlitePayment WHERE EmployeeId = @employeeId)
		DELETE FROM dbo.SqlitePayment WHERE EmployeeId = @employeeId

		-- Delete SqliteReturnOrder data
		DELETE FROM dbo.SqliteReturnOrderItem WHERE SqliteReturnOrderId IN (SELECT Id FROM dbo.[SqliteReturnOrder] WHERE EmployeeId = @employeeId)
		DELETE FROM dbo.[SqliteReturnOrder] WHERE EmployeeId = @employeeId

		-- Delete SqliteEntity data
		DECLARE @SqliteEntity TABLE (Id BIGINT)
		INSERT INTO @SqliteEntity SELECT Id FROM dbo.[SqliteEntity] WHERE EmployeeId = @employeeId
		DELETE FROM dbo.[SqliteEntityContact] WHERE [SqliteEntityId] IN (SELECT Id FROM @SqliteEntity)
		DELETE FROM dbo.[SqliteEntityCrop] WHERE [SqliteEntityId] IN (SELECT Id FROM @SqliteEntity)
		DELETE FROM dbo.[SqliteEntity] WHERE EmployeeId = @employeeId

		-- Delete SqliteLeave data
		DELETE FROM dbo.[SqliteLeave] WHERE EmployeeId = @employeeId

		-- Delete SqliteCancelledLeave data
		DELETE FROM dbo.[SqliteCancelledLeave] WHERE EmployeeId = @employeeId
		
		DELETE FROM dbo.SqliteActionBatch WHERE EmployeeId = @employeeId

		-- store image ids first for processed expense data
		INSERT INTO @image (id)
		SELECT ImageId 
		FROM dbo.ExpenseItemImage WHERE ExpenseItemId IN 
		(SELECT ei.id FROM dbo.ExpenseItem ei
		INNER JOIN dbo.Expense e on ei.ExpenseId = e.Id
		AND e.EmployeeId = @employeeId)

		DELETE FROM dbo.ExpenseItemImage WHERE ExpenseItemId IN 
		(SELECT ei.id FROM dbo.ExpenseItem ei INNER JOIN dbo.Expense e on ei.ExpenseId = e.Id AND e.EmployeeId = @employeeId)

		DELETE FROM dbo.ExpenseItem WHERE ExpenseId in (SELECT Id FROM dbo.Expense WHERE EmployeeId = @employeeId)
		DELETE FROM dbo.ExpenseApproval WHERE ExpenseId in (SELECT Id FROM dbo.Expense WHERE EmployeeId = @employeeId)
		DELETE FROM dbo.Expense WHERE EmployeeId = @employeeId

		-- Delete order Data
		DELETE FROM dbo.OrderItem WHERE OrderId IN (SELECT Id FROM dbo.[Order] WHERE EmployeeId = @employeeId)

		INSERT INTO @image (id)
		SELECT oim.ImageId
		FROM dbo.OrderImage oim
		INNER JOIN dbo.[Order] o on o.Id = oim.OrderId
		AND o.EmployeeId = @employeeId

		DELETE FROM dbo.OrderImage WHERE OrderId IN (SELECT Id FROM dbo.[Order] WHERE EmployeeId = @employeeId)

		DELETE FROM dbo.[Order] WHERE EmployeeId = @employeeId



		-- Delete Return Order Data
		DELETE FROM dbo.ReturnOrderItem WHERE ReturnOrderId IN (SELECT Id FROM dbo.[ReturnOrder] WHERE EmployeeId = @employeeId)
		DELETE FROM dbo.[ReturnOrder] WHERE EmployeeId = @employeeId

		-- Delete Entity data
		DECLARE @Entity TABLE (Id BIGINT)
		INSERT INTO @Entity SELECT Id FROM dbo.[Entity] WHERE EmployeeId = @employeeId
		DELETE FROM dbo.[EntityContact] WHERE [EntityId] IN (SELECT Id FROM @Entity)
		DELETE FROM dbo.[EntityCrop] WHERE [EntityId] IN (SELECT Id FROM @Entity)
		DELETE FROM dbo.[Entity] WHERE EmployeeId = @employeeId


		--
		-- Delete Payment Data
		--
		INSERT INTO @image (id)
		SELECT pim.ImageId
		FROM dbo.PaymentImage pim
		INNER JOIN dbo.Payment p on p.Id = pim.PaymentId
		AND p.EmployeeId = @employeeId

		DELETE FROM dbo.PaymentImage WHERE PaymentId IN (SELECT Id FROM dbo.[Payment] WHERE EmployeeId = @employeeId)
		DELETE FROM dbo.[Payment] WHERE EmployeeId = @employeeId

		-- DELETE IMAGES
		DELETE FROM dbo.[Image]
		WHERE Id in (SELECT Id FROM @image)

		DELETE from dbo.TenantEmployee WHERE id = @employeeId

		COMMIT
	END TRY

	BEGIN CATCH
		
		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:ClearEmployeeData', ERROR_MESSAGE()

		ROLLBACK TRANSACTION
		throw;

	END CATCH
END	
GO


IF OBJECT_ID ( 'dbo.ProcessSqliteOrderData', 'P' ) IS NOT NULL   
    DROP PROCEDURE dbo.ProcessSqliteOrderData;  
GO  

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
	(EmployeeId, DayId, CustomerCode, OrderType, OrderDate, TotalAmount, 
	TotalGST, NetAmount, DiscountType, DiscountPercentage,
	ItemCount, SqliteOrderId, RevisedTotalAmount,
	RevisedTotalGST, RevisedNetAmount, RevisedDiscountPercentage, ImageCount
	)

	OUTPUT inserted.EmployeeId, inserted.DayId, inserted.TotalAmount INTO @NewOrders
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
	SET TotalOrderAmount = TotalOrderAmount + o.TotalAmount
	FROM dbo.EmployeeDay ed
	INNER JOIN 
	     (SELECT employeeId, dayid, SUM(TotalAmount) TotalAmount FROM @NewOrders GROUP BY employeeId, dayId) o 
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
	INSERT INTO dbo.[Image]
	(SourceId, OrderSourceId, [ImageFileName])  -- Order source id is used in next query listed below
	SELECT 0, soi.Id, soi.[ImageFileName]
	FROM dbo.SqliteOrderImage soi
	INNER JOIN dbo.SqliteOrder so on soi.SqliteOrderId = so.Id
	AND so.BatchId = @batchId
	AND so.IsProcessed = 0
		
	-- Now create entries in OrderImage
	INSERT INTO dbo.OrderImage
	(OrderId, ImageId, SequenceNumber)
	SELECT o.Id, i.[Id], soi.SequenceNumber
	FROM dbo.SqliteOrderImage soi
	INNER JOIN dbo.[Image] i on soi.Id = i.OrderSourceId
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
GO

