-- Schema Changes - Nov 19 2017
--------------------------------
	Alter table dbo.[Order] add [RevisedTotalAmount] DECIMAL(19,2) DEFAULT 0 NOT NULL
go	
	Alter table dbo.OrderItem 
	Add [RevisedUnitQuantity] INT NOT NULL DEFAULT 0,
	 [RevisedAmount] DECIMAL(19,2) NOT NULL DEFAULT 0,
	 [DateUpdated] Datetime2,
	 [UpdatedBy] NVARCHAR(50)
go

update dbo.[order] set revisedTotalAmount = totalAmount
go

update dbo.OrderItem set RevisedUnitQuantity = UnitQuantity,RevisedAmount = Amount
go

insert into dbo.CodeTable
(Codetype, Codevalue, DisplaySequence, IsActive)
values
('CustomerType', 'Society', 40, 1),
('ExpenseType', 'DA (Outstation)', 12, 1)
go

update dbo.CodeTable set CodeValue = 'DA (Local)' where CodeValue = 'Daily Allowance' and CodeType='ExpenseType'
go


ALTER PROCEDURE [dbo].[TransformDataFeed]
	@tenantId BIGINT
AS
BEGIN
		-- Stored procedure to put data from SAP data feed (tables)
		-- to local tables

		-- Step 1: Not defined.
		-- Input Tables
		-- CustomerMaster
		-- MaterialMaster
		-- EmployeeMaster
		-- PriceList

		-- Nov 17 2017 - don't delete existing data from Sales Person table
		-- From Employee Master, take new records and put them in SalesPerson table.

		-- Step 2: Wipe out existing data
		DELETE FROM dbo.ProductPrice
		DELETE FROM dbo.Product;
		DELETE FROM dbo.ProductGroup;
		DELETE FROM dbo.Customer;
		--DELETE FROM dbo.SalesPerson;

		-- Step 3: Insert data in Product Group and Product table
		INSERT INTO dbo.ProductGroup
		(GroupName)
		SELECT DISTINCT ltrim(rtrim([Product Group])) FROM dbo.MaterialMaster
		WHERE [Product Group] IS NOT NULL

		-- Step 4: Insert data in Product table
		;WITH materialCTE(productCode, ProductGroup, Name, UOM, BrandName, ShelfLifeInMonths, IsActive, RN)
		AS
		(
			SELECT rtrim(ltrim(mm.Material)),
			ltrim(rtrim([Product Group])),
			ISNULL(mm.[Description], ''), 
			ISNULL(mm.UOM,''), 
			ISNULL(mm.[Brand Name],''),
			ISNULL(mm.[Shelf Life in months], 0),
			Case when mm.[Status] = 'ACTIVE' THEN 1 ELSE 0 END,
			Row_Number() OVER (Partition By rtrim(ltrim(mm.Material)) ORDER BY rtrim(ltrim(mm.Material)) )
			FROM dbo.MaterialMaster mm
			WHERE [Product Group] IS NOT NULL
		)
		INSERT INTO dbo.Product
		(GroupId, ProductCode, Name, UOM, BrandName, ShelfLifeInMonths, IsActive, MRP)
		SELECT pg.Id, cte.productCode, cte.Name, cte.UOM, cte.BrandName, cte.ShelfLifeInMonths, CTE.IsActive, 0
		 FROM materialCTE cte
		INNER JOIN dbo.ProductGroup pg on cte.ProductGroup = pg.GroupName
		AND cte.RN = 1
	
		-- Step 4: Insert data in ProductPrice table
		INSERT INTO dbo.ProductPrice
		(ProductId, AreaCode, Stock, [MRP], DistPrice, PDISTPrice, DEALERPrice)
		SELECT p.Id, 
		LEFT(ISNULL(pl.AO, ''),10),
		ISNULL(pl.Stock, 0),
		ISNULL(pl.MRP,0),
		ISNULL(pl.[Dist Rate],0), 
		ISNULL(pl.[PD'S Rate],0),
		ISNULL(pl.[Dealers Rate],0)
		FROM dbo.PriceList pl
		INNER JOIN dbo.Product p ON ltrim(rtrim(pl.[PRODUCT CODE])) = p.ProductCode
		AND pl.[PRODUCT CODE] IS NOT NULL

		-- Step 5: Insert data in SalesPerson table
		INSERT INTO dbo.SalesPerson
		([StaffCode], [Name], [Phone], [HQCode], [IsActive])
		SELECT  ltrim(STR(ISNULL([Staff Code],0),10,0)),
		ISNULL(ltrim(rtrim(em.Name)),''),
		ltrim(Str(IsNULL(em.[Phone], 0), 50, 0)),
		ltrim(rtrim(ISNULL([Head Quarter], ''))),
		Case when ltrim(rtrim([Action])) = 'ACTIVE' THEN 1 ELSE 0 END
		FROM dbo.EmployeeMaster em
		LEFT JOIN dbo.SalesPerson sp ON [Staff Code] IS NOT NULL
				AND ltrim(STR(ISNULL(em.[Staff Code],0),10,0)) = sp.StaffCode
		WHERE sp.StaffCode IS NULL

		-- Step 6: Insert data in Customer table
		INSERT INTO [dbo].[Customer]
		([CustomerCode], [Name], [Type], [CreditLimit], 
		[Outstanding], [LongOutstanding], [District], [State], [Branch], [Pincode], [HQCode], [ContactNumber],
		[Target], [Sales], [Payment])
		SELECT 
		left(cm.[Customer Code], 20),
		left(cm.[Customer Name],50), 
		cm.[Type], 
		ISNULL(cm.[Credit Limit],0),
		ISNULL([Total Outstanding], 0),
		ISNULL([Total Overdue >90], 0),
		left(cm.[District Name],50),
		left(cm.[State Name],50),
		left(cm.[Branch Name],50),
		left(cm.Pincode,10),
		left(ISNULL([HQ Code], ''),10),
		ltrim(Str(IsNULL([Contact #], 0), 50, 0)),
		ISNULL([Expected Business], 0), -- Target
		ISNULL([Sales],0),  -- Sales
		ISNULL([Collection], 0) -- Payment
		FROM dbo.CustomerMaster cm
		WHERE cm.[Type] IS NOT NULL

		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformDataFeed', 'Success'
END
go

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
	INSERT INTO dbo.EmployeeDay
	(TenantEmployeeId, DayId, StartTime)
	SELECT Distinct o.EmployeeId, d.Id, CAST(o.OrderDate as [Date])
	FROM dbo.SqliteOrder o
	INNER JOIN dbo.[Day] d on d.[Date] = CAST(o.OrderDate as [DATE])
	AND o.BatchId = @batchId
	LEFT JOIN dbo.EmployeeDay ed on ed.TenantEmployeeId = o.EmployeeId
	          AND ed.DayId = d.Id
	WHERE ed.Id IS NULL

	-- Now create records in dbo.Order Table
	-- (At the time of processing batches, set RevisedTotalAmount to TotalAmount)
	INSERT INTO dbo.[Order]
	(EmployeeId, DayId, CustomerCode, OrderType, OrderDate, TotalAmount, ItemCount, SqliteOrderId, RevisedTotalAmount)
	SELECT EmployeeId, d.Id, CustomerCode, o.OrderType, CAST(o.OrderDate as [DATE]), o.TotalAmount, o.ItemCount, o.Id, o.TotalAmount
	FROM dbo.SqliteOrder o
	INNER JOIN dbo.[Day] d ON d.[DATE] = CAST(o.OrderDate AS [Date])
	WHERE BatchId = @batchId
	ORDER BY o.Id

	-- now we need to copy line items
	-- (Similarly at the time of batch processes, set Revised Unit Quantity and RevisedAmount to UnitQuantity and Amount)
	INSERT INTO dbo.OrderItem
	(OrderId, SerialNumber, ProductCode, UnitQuantity, UnitPrice, Amount, RevisedUnitQuantity, RevisedAmount)
	SELECT o.Id, soi.SerialNumber, soi.ProductCode, soi.UnitQuantity, soi.UnitPrice, soi.Amount, soi.UnitQuantity, soi.Amount
	FROM dbo.SqliteOrderItem soi
	INNER JOIN dbo.SqliteOrder so on soi.SqliteOrderId = so.Id
	AND so.BatchId = @batchId
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
go

