-- Schema changes deployed on Dec 08 2017
alter table dbo.SalesPerson 	
Add	[Grade] NVARCHAR(50)
go

ALTER TABLE dbo.Expense
ADD	[IsZoneApproved] BIT NOT NULL DEFAULT 0,
	[IsAreaApproved] BIT NOT NULL DEFAULT 0,
	[IsTerritoryApproved] BIT NOT NULL DEFAULT 0
GO

ALTER TABLE [dbo].[Expense] DROP CONSTRAINT [DF__Expense__IsAppro__151B244E]
GO

ALTER TABLE [dbo].[Expense] DROP CONSTRAINT [DF__Expense__Approve__160F4887]
GO

ALTER TABLE [dbo].[Expense] DROP CONSTRAINT [DF__Expense__Approve__17036CC0]
GO

ALTER TABLE [dbo].[Expense] DROP CONSTRAINT [DF__Expense__Approve__14270015]
GO


ALTER TABLE dbo.Expense
DROP COLUMN 
	[IsApproved],
	[ApproveDate],
	[ApproveRef],
	[ApproveNotes] ,
	[ApproveAmount] ,
	[ApprovedBy] 
GO


CREATE TABLE [dbo].[ExpenseApproval]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[ExpenseId] BIGINT NOT NULL REFERENCES Expense,

	-- approval fields
	[ApproveLevel] NVARCHAR(20) NOT NULL, -- Territory / Area / Zone
	[ApproveDate] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[ApproveRef] NVARCHAR(255),
	[ApproveNotes] NVARCHAR(2048),
	[ApproveAmount] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[ApprovedBy] NVARCHAR(50) NOT NULL DEFAULT '',

	[Timestamp] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
)
GO

ALTER TABLE [dbo].[SqliteActionBatch]
	ADD [UnderConstruction] BIT NOT NULL DEFAULT 0
GO








delete from dbo.CodeTable where CodeType = 'Grade'
go
insert into dbo.CodeTable
(CodeType, CodeValue, CodeName, DisplaySequence, IsActive)
values
('Grade', 'M1', 'Single AC room in 3 Star Hotels', 10, 1),
('Grade', 'M2', 'Single AC room in 3 Star Hotels', 20, 1),
('Grade', 'M3', 'Rs. 2500', 30, 1),
('Grade', 'M4A', 'Rs. 2000', 40, 1),
('Grade', 'M4B', 'Rs. 1750', 50, 1),
('Grade', 'M5A', 'Rs. 1400', 60, 1),
('Grade', 'M5B', 'Rs. 1250', 70, 1),
('Grade', 'M6', 'Rs. 1000', 80, 1),
('Grade', 'M7', 'Rs. 900', 90, 1),
('Grade', 'M8', 'Rs. 700', 100, 1),
('Grade', 'M9', 'Rs. 450', 110, 1),
('Grade', 'M10A', 'Rs. 450', 120, 1)
---------
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

		-- Step 7: Update names changes from SalesPerson table to TenantEmployee table
		UPDATE dbo.TenantEmployee
		SET Name = sp.Name
		FROM dbo.TenantEmployee te
		INNER JOIN dbo.SalesPerson sp on te.EmployeeCode = sp.StaffCode
		AND te.Name <> sp.Name


		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformDataFeed', 'Success'
END
go

ALTER PROCEDURE [dbo].[GetSqliteActionBatchForProcessing]
	@recordCount int,
	@tenantId BIGINT,
	@employeeId BIGINT
AS
BEGIN
    DECLARE @currentDateTime DATETIME2 = SYSUTCDATETIME();

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
		AND ((st.BatchProcessed = 0 And st.LockTimestamp IS NULL) OR
		(st.LockTimeStamp Is NOT NULL AND DATEDIFF(mi, st.LockTimestamp, @currentDateTime) >= 5))
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

ALTER PROCEDURE [dbo].[StartEmployeeDay]
	@employeeId BIGINT,
	@startDateTime DateTime2,

    @PhoneModel NVARCHAR(100),
    @PhoneOS NVARCHAR(10),
    @AppVersion NVARCHAR(10),

	@employeeDayId BIGINT OUTPUT
AS
BEGIN
   -- Records start of the day activity
   -- First Selet DayId from dbo.Day table for start date time
   DECLARE @dayId BIGINT
   DECLARE @startDate DATE = CAST(@startDateTime AS [Date])

	IF EXISTS(SELECT 1 FROM dbo.[Day] WHERE [Date] = @startDate)
	BEGIN
		SELECT @dayId = Id FROM dbo.[Day] WHERE [Date] = @startDate
	END
	ELSE
	BEGIN
		INSERT INTO dbo.[Day] ([Date]) VALUES (@startDate)
		SET @dayId = SCOPE_IDENTITY()
	END

	SET @employeeDayId = 0

	-- check if a record already exist in Employee Day table.
	IF EXISTS(SELECT 1 FROM dbo.EmployeeDay WHERE TenantEmployeeId=@employeeId AND DayId = @dayId)
	BEGIN
		DECLARE @currentAppVersion NVARCHAR(10)
		SELECT @employeeDayId=Id,
		       @currentAppVersion = ISNULL(AppVersion, '***')
		FROM dbo.EmployeeDay WHERE TenantEmployeeId=@employeeId AND DayId = @dayId

		UPDATE dbo.EmployeeDay 
		SET EndTime = Null, TotalDistanceInMeters = 0,
		    HasMultipleStarts = 1,
			AppVersion = Case when @currentAppVersion='***' Then @AppVersion Else AppVersion END,
			PhoneModel = Case when @currentAppVersion='***' Then @PhoneModel Else PhoneModel END,
			PhoneOS = Case when @currentAppVersion='***' Then @PhoneOS Else PhoneOS END
		WHERE Id = @employeeDayId 
		-- for auto created/on need employee days, app version is set to *** or null
		-- hence when actual start day comes, we want to fill app version.
	END

	ELSE

	BEGIN
		-- pick up HQ Code from SalesPerson table
		DECLARE @hqCode VARCHAR(10)
		DECLARE @areaCode VARCHAR(10)

		-- Copy the hq code and area code in Employee Day table
		-- as later sales Person's HQ Code can change
		-- And For reporting purposes, we should be using the HQ Code
		-- that the person had on the date he worked.
		SELECT Top(1) @hqCode = ISNULL(oh.HQCode,''), @areaCode = ISNULL(oh.AreaCode, '')
		FROM dbo.TenantEmployee te 
		INNER JOIN dbo.SalesPerson sp on te.EmployeeCode = sp.StaffCode
		AND te.Id = @employeeId
		INNER JOIN dbo.OfficeHierarchy oh on sp.HQCode = oh.HQCode
		AND oh.IsActive = 1

		INSERT INTO dbo.EmployeeDay
		(TenantEmployeeId, DayId, StartTime, HQCode, AreaCode, PhoneModel, PhoneOS, AppVersion)
		VALUES
		(@employeeId, @dayId, @startDateTime, ISNULL(@hqCode, ''), ISNULL(@areaCode, ''),
			@PhoneModel, @PhoneOS, @AppVersion)

		SET @employeeDayId = SCOPE_IDENTITY()
	END
END
GO

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

	-- Now create records in dbo.Order Table
	-- (At the time of processing batches, set RevisedTotalAmount to TotalAmount)
	INSERT INTO dbo.[Order]
	(EmployeeId, DayId, CustomerCode, OrderType, OrderDate, TotalAmount, ItemCount, SqliteOrderId, RevisedTotalAmount)
	OUTPUT inserted.EmployeeId, inserted.DayId, inserted.TotalAmount INTO @NewOrders
	SELECT EmployeeId, d.Id, CustomerCode, o.OrderType, CAST(o.OrderDate as [DATE]), o.TotalAmount, o.ItemCount, o.Id, o.TotalAmount
	FROM dbo.SqliteOrder o
	INNER JOIN dbo.[Day] d ON d.[DATE] = CAST(o.OrderDate AS [Date])
	WHERE BatchId = @batchId
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

		-- Create expense Record
		DECLARE @expenseId BIGINT
		INSERT INTO dbo.Expense
		(EmployeeId, DayId, TotalAmount)
		OUTPUT inserted.EmployeeId, inserted.DayId, inserted.TotalAmount INTO @NewExpenses
		SELECT EmployeeId, @dayId, TotalExpenseAmount
		FROM dbo.SqliteActionBatch WHERE Id = @batchId
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
		WHERE ID = @batchId

		-- Now create expense line items
		INSERT INTO dbo.ExpenseItem
		(ExpenseId, SequenceNumber, ExpenseType, TransportType, Amount, OdometerStart, OdometerEnd, ImageCount, FuelType, FuelQuantityInLiters, Comment)
		SELECT @expenseId, SequenceNumber, ExpenseType, VehicleType, Amount, OdometerStart, OdometerEnd, ImageCount, FuelType, FuelQuantityInLiters, Comment
		FROM dbo.SqliteExpense
		WHERE BatchId = @batchId

		-- Update ExpenseItemId in SqliteExpense
		UPDATE dbo.SqliteExpense
		SET ExpenseItemId = ei.Id,
		IsProcessed = 1,
		DateUpdated = SYSUTCDATETIME()
		FROM dbo.SqliteExpense sle
		INNER JOIN dbo.ExpenseItem ei on sle.SequenceNumber = ei.SequenceNumber
		AND sle.BatchId = @batchId
		AND ei.ExpenseId = @expenseId

		-- Now Images
		INSERT INTO dbo.[Image]
		(SourceId, [Data])  -- source id is used in next query listed below
		SELECT sei.id, sei.[Image]
		FROM dbo.SqliteExpenseImage sei
		INNER JOIN dbo.SqliteExpense se on sei.SqliteExpenseId = se.Id
		AND se.BatchId = @batchId
		
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
GO

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

	-- Create Payment Records
	INSERT INTO dbo.[Payment]
	(EmployeeId, DayId, CustomerCode, PaymentType, PaymentDate, TotalAmount, Comment, ImageCount, SqlitePaymentId)
	OUTPUT inserted.EmployeeId, inserted.DayId, inserted.TotalAmount INTO @NewPayments
	SELECT EmployeeId, d.Id, CustomerCode, p.PaymentType, CAST(p.PaymentDate as [DATE]), p.TotalAmount, p.Comment, p.ImageCount, p.Id
	FROM dbo.SqlitePayment p
	INNER JOIN dbo.[Day] d ON d.[DATE] = CAST(p.PaymentDate AS [Date])
	WHERE BatchId = @batchId
	ORDER BY p.Id

	-- Now update the total amounts in EmployeeDay table
	UPDATE dbo.EmployeeDay 
	SET TotalPaymentAmount = TotalPaymentAmount + o.TotalAmount
	FROM dbo.EmployeeDay ed
	INNER JOIN 
	     (SELECT employeeId, dayid, SUM(TotalAmount) TotalAmount FROM @NewPayments GROUP BY employeeId, dayId) o 
	 ON ed.TenantEmployeeId = o.EmployeeId AND ed.DayId = o.DayId

	-- now we need to update the Payment back in SqlitePayment table
	UPDATE dbo.SqlitePayment
	SET PaymentId = p.Id,
	IsProcessed = 1,
	DateUpdated = SYSUTCDATETIME()
	FROM dbo.SqlitePayment sp
	INNER JOIN dbo.[Payment] p on sp.Id = p.SqlitePaymentId
	AND sp.BatchId = @batchId

	-- Now Images
	INSERT INTO dbo.[Image]
	(SourceId, PaymentSourceId, [Data])  -- Payment source id is used in next query listed below
	SELECT 0, spi.id, spi.[Image]
	FROM dbo.SqlitePaymentImage spi
	INNER JOIN dbo.SqlitePayment sp on spi.SqlitePaymentId = sp.Id
	AND sp.BatchId = @batchId
		
	-- Now create entries in PaymentImage
	INSERT INTO dbo.PaymentImage
	(PaymentId, ImageId, SequenceNumber)
	SELECT p.Id, i.[Id], spi.SequenceNumber
	FROM dbo.SqlitePaymentImage spi
	INNER JOIN dbo.[Image] i on spi.Id = i.PaymentSourceId
	INNER JOIN dbo.SqlitePayment slp on spi.SqlitePaymentId = slp.Id
	AND slp.BatchId = @batchId
	INNER JOIN dbo.Payment p on slp.Id = p.SqlitePaymentId
END
GO

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

	-- Now create records in dbo.ReturnOrder Table
	INSERT INTO dbo.[ReturnOrder]
	(EmployeeId, DayId, CustomerCode, ReturnOrderDate, TotalAmount, ItemCount, SqliteReturnOrderId, ReferenceNumber, Comment)
	OUTPUT inserted.EmployeeId, inserted.DayId, inserted.TotalAmount INTO @NewReturnOrders
	SELECT EmployeeId, d.Id, CustomerCode, CAST(o.ReturnOrderDate as [DATE]), o.TotalAmount, o.ItemCount, o.Id, o.ReferenceNum, o.Comment
	FROM dbo.SqliteReturnOrder o
	INNER JOIN dbo.[Day] d ON d.[DATE] = CAST(o.ReturnOrderDate AS [Date])
	WHERE BatchId = @batchId
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
	AND so.BatchId = @batchId
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

ALTER PROCEDURE [dbo].[ClearEmployeeData]
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
		DELETE FROM dbo.[Order] WHERE EmployeeId = @employeeId

		-- Delete Return Order Data
		DELETE FROM dbo.ReturnOrderItem WHERE ReturnOrderId IN (SELECT Id FROM dbo.[ReturnOrder] WHERE EmployeeId = @employeeId)
		DELETE FROM dbo.[ReturnOrder] WHERE EmployeeId = @employeeId


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

ALTER PROCEDURE [dbo].[AddActivityData]
	@employeeDayId BIGINT,
	@activityDateTime DateTime2,
	@clientName NVARCHAR(50),
	@clientPhone NVARCHAR(20),
	@clientType NVARCHAR(50),
	@activityType NVARCHAR(50),
	@comments NVARCHAR(2048),
	@imageCount INT,
	@activityId BIGINT OUTPUT
AS
BEGIN
   -- Records tracking activity
   SET @activityId = 0
	-- check if a record already exist in Employee Day table
	IF EXISTS(SELECT 1 FROM dbo.EmployeeDay WHERE Id = @employeeDayId)
	BEGIN
		INSERT into dbo.Activity
		(EmployeeDayId, ClientName, ClientPhone, ClientType, ActivityType, Comments, ImageCount, [At])
		VALUES
		(@employeeDayId, @clientName, @clientPhone, @clientType, @activityType, @comments, @imageCount, @activityDateTime)

		SET @activityId = SCOPE_IDENTITY()
	END
END
GO
