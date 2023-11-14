-- Prod Script for Nov 26 2017 deployment
-- Store Device Info in EmployeeDay table
-- Store Aggregate numbers in EmployeeDay table for Exec App

ALTER TABLE [dbo].[EmployeeDay]
    ADD [TotalOrderAmount]   DECIMAL (19, 2) DEFAULT 0 NOT NULL,
        [TotalPaymentAmount] DECIMAL (19, 2) DEFAULT 0 NOT NULL,
        [TotalReturnAmount]  DECIMAL (19, 2) DEFAULT 0 NOT NULL,
        [TotalExpenseAmount] DECIMAL (19, 2) DEFAULT 0 NOT NULL,
        [CurrentLatitude]    DECIMAL (19, 9) DEFAULT 0 NOT NULL,
        [CurrentLongitude]   DECIMAL (19, 9) DEFAULT 0 NOT NULL,
        [HasMultipleStarts]  BIT             DEFAULT 0 NOT NULL,
        [HQCode]             NVARCHAR (10)   DEFAULT '' NOT NULL,
        [AreaCode]           NVARCHAR (10)   DEFAULT '' NOT NULL,
	[PhoneModel] NVARCHAR(100),
	[PhoneOS] NVARCHAR(10),
	[AppVersion] NVARCHAR(10);
go

ALTER table dbo.SqliteAction
ADD	[PhoneModel] NVARCHAR(100),
	[PhoneOS] NVARCHAR(10),
	[AppVersion] NVARCHAR(10);
go

CREATE TABLE [dbo].[AppVersion]
(
	[Id] INT NOT NULL PRIMARY KEY Identity,
	[Version] NVARCHAR(10) NOT NULL,
	[EffectiveDate] [DATE] NOT NULL,
	[ExpiryDate] [DATE] NOT NULL
)
go

CREATE TABLE [dbo].[ExecAppImei]
(
	[Id] INT NOT NULL PRIMARY KEY Identity,
	[IMEINumber] NVARCHAR(50) NOT NULL,
	[EffectiveDate] DATE NOT NULL,
	[ExpiryDate] DATE NOT NULL
)
go

insert into dbo.appVersion
([Version], EffectiveDate, ExpiryDate)
values
('1.3', '2017-11-26', '2099-12-31'),
('legacy', '2017-01-01', '2099-12-31')

insert into dbo.ExecAppImei
(ImeiNumber, EffectiveDate, ExpiryDate)
values
('356164072424710','2017-11-26', '2099-12-31'),
('357629068350706','2017-11-26', '2099-12-31')
go


ALTER PROCEDURE [dbo].[AddTrackingData]
	@employeeDayId BIGINT,
	@trackingDateTime DateTime2,
	@latitude Decimal(19,9),
	@longitude Decimal(19,9),
	@activityId BIGINT,
	@isMilestone BIT,  -- 1 if we want to log this request irrespective of the fact that it is sent too soon
	@isStartOfDay BIT,
	@isEndOfDay BIT,
	@trackingId BIGINT OUTPUT
AS
BEGIN
   -- Records tracking activity
   -- distanceInMeters that is passed from Mobile Device is not saved in db.
    SET @trackingId = 0
	-- check if a record already exist in Employee Day table
	-- At end of the day, we do need to log a tracking entry as MileStone flag set to 1 - hence the OR condition here;
	IF EXISTS(SELECT 1 FROM dbo.EmployeeDay WHERE Id = @employeeDayId AND (EndTime is null OR @isMilestone = 1))
	BEGIN
	    -- Find out Begin coordinates from last record
		DECLARE @beginLatitude DECIMAL(19,9) = 0
		DECLARE @beginLongitude DECIMAL(19,9) = 0
		DECLARE @chainedTrackingId BIGINT = 0
		DECLARE @lastTrackingDateTime DATETIME2

		IF @isStartOfDay = 1
		BEGIN
			SET @chainedTrackingId = NULL
		END
		ELSE IF @isMilestone = 1
		BEGIN
		    -- for milestone recordings - look for previous milestone entries or first entry
			SELECT @chainedTrackingId = MAX(ID)
				FROM dbo.Tracking WITH (NOLOCK)
				WHERE EmployeeDayId = @employeeDayId
				AND IsMilestone = 1
		END
		ELSE
		BEGIN
			SELECT @chainedTrackingId = MAX(ID)
				FROM dbo.Tracking WITH (NOLOCK)
				WHERE EmployeeDayId = @employeeDayId
		END
 
		------------------

		SELECT @beginLatitude = EndGpsLatitude,
		       @beginLongitude = EndGpsLongitude,
			   @lastTrackingDateTime = [At]
		FROM dbo.tracking WITH (NOLOCK)
		WHERE Id = ISNULL(@chainedTrackingId,0)

		-- if tracking request is coming too soon - from configured time
		-- don't log the tracking request
		DECLARE @logCurrentTrackingRequest BIT = 0;
		IF @isMilestone = 1 OR ISNULL(@chainedTrackingId,0) = 0
		BEGIN
		    -- must log milestone entries;
			-- first tracking request for day - log it;
			SET @logCurrentTrackingRequest = 1
		END
		ELSE
		IF @beginLatitude = @latitude AND @beginLongitude = @longitude
		BEGIN
			SET @logCurrentTrackingRequest = 0
		END
		ELSE
		BEGIN
			-- find out expected delay
			DECLARE @TimeIntervalInMillisecondsForTracking BIGINT
			SELECT @TimeIntervalInMillisecondsForTracking = te.TimeIntervalInMillisecondsForTracking
			FROM dbo.TenantEmployee te
			INNER JOIN dbo.EmployeeDay ed on te.Id = ed.TenantEmployeeId
			AND ed.Id = @employeeDayId

			-- add time to last tracking record time
			IF DATEDIFF(ms, @lastTrackingDateTime, @trackingDateTime) >= @TimeIntervalInMillisecondsForTracking
			BEGIN
				SET @logCurrentTrackingRequest = 1
			END
		END
		
		------------------------------

		IF @logCurrentTrackingRequest = 1
		BEGIN
			-- Create new record in Tracking table
			INSERT into dbo.Tracking
			(ChainedTrackingId, EmployeeDayId, [At], BeginGPSLatitude, BeginGPSLongitude, 
			EndGPSLatitude, EndGPSLongitude, BeginLocationName, EndLocationName, BingMapsDistanceInMeters, 
			GoogleMapsDistanceInMeters, LinearDistanceInMeters,
			DistanceCalculated, LockTimestamp, 
			ActivityId, IsMilestone, IsStartOfDay, IsEndOfDay)
			VALUES
			(@chainedTrackingId, @employeeDayId, @trackingDateTime, @beginLatitude, @beginLongitude,
			 @latitude, @longitude, NULL, NULL, 0,
			 0, 0,
			 --@isStartOfDay, Null,
			 0, Null,
			 -- if it is start of day request, then we mark it as calculated
			 -- for start of day also, leave distance calculated as 0 - so we can get location name for reports
			 @activityId, @isMilestone,
			 @isStartOfDay,
			 @isEndOfDay)

			SET @trackingId = SCOPE_IDENTITY()

             -- update most recent location in EmployeeDay table as well
			 UPDATE dbo.EmployeeDay
			 SET CurrentLatitude = @latitude,
			 CurrentLongitude = @longitude
			 WHERE Id = @employeeDayId
		END
		ELSE
		BEGIN
			SET @trackingId = -2
		END
	END
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
		(TenantEmployeeId, DayId, StartTime)
		SELECT Distinct o.EmployeeId, d.Id, CAST(o.ExpenseDate as [Date])
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
	(TenantEmployeeId, DayId, StartTime)
	SELECT Distinct o.EmployeeId, d.Id, CAST(o.PaymentDate as [Date])
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



go
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
	(TenantEmployeeId, DayId, StartTime)
	SELECT Distinct o.EmployeeId, d.Id, CAST(o.ReturnOrderDate as [Date])
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
		SELECT @employeeDayId=Id FROM dbo.EmployeeDay WHERE TenantEmployeeId=@employeeId AND DayId = @dayId
		UPDATE dbo.EmployeeDay 
		SET EndTime = Null, TotalDistanceInMeters = 0,
		    HasMultipleStarts = 1
		WHERE Id = @employeeDayId 
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



go
ALTER PROCEDURE [dbo].[GetInFieldSalesPeople]
		@inputDate DateTime2
AS
BEGIN

	DECLARE @SignedInEmployeeData TABLE
	(
        EmployeeDayId BIGINT,
		EmployeeId BIGINT,
		StartTime DATETIME2,
		EndTime DATETIME2,
		TotalOrderAmount DECIMAL(19,2),
		TotalPaymentAmount DECIMAL(19,2),
		TotalReturnAmount DECIMAL(19,2),
		TotalExpenseAmount DECIMAL(19,2),
		Latitude DECIMAL(19,9),
		Longitude DECIMAL(19,9),
		PhoneModel NVARCHAR(100),
		PhoneOS NVARCHAR(10),
		AppVersion NVARCHAR(10)
	)

	INSERT INTO @SignedInEmployeeData
	(EmployeeDayId, EmployeeId, StartTime, EndTime, 
	 TotalOrderAmount, TotalPaymentAmount, TotalReturnAmount, TotalExpenseAmount,
	 Latitude, Longitude, PhoneModel, PhoneOS, AppVersion
	)
	SELECT ed.Id, ed.TenantEmployeeId, StartTime, EndTime, 
	TotalOrderAmount, TotalPaymentAmount, 
	TotalReturnAmount, TotalExpenseAmount,
	CurrentLatitude, CurrentLongitude,
	PhoneModel, PhoneOS, AppVersion
	FROM dbo.[Day] d
	INNER JOIN dbo.EmployeeDay ed on d.Id = ed.DayId
	AND d.[DATE] = CAST(@inputDate AS [Date])
	
	-- RESULT SET QUERY
	;WITH cteAreaCodes(AreaCode)
	AS
	( 
		SELECT Distinct  AreaCode FROM dbo.OfficeHierarchy 
		WHERE IsActive = 1
	)
	SELECT ISNULL(cte.AreaCode, '***') AreaCode, 
	ISNULL(sq.StaffCode, '') StaffCode, 
	ISNULL(sq.IsInFieldToday, 0) IsInFieldToday,
	ISNULL(sq.IsRegisteredOnPhone,0) IsRegisteredOnPhone,
	sq.StartTime,
	sq.EndTime,
	ISNULL(sq.TotalOrderAmount, 0) as TotalOrderAmount,
	ISNULL(sq.TotalPaymentAmount,0) as TotalPaymentAmount,
	ISNULL(sq.TotalReturnAmount,0) as TotalReturnAmount,
	ISNULL(sq.TotalExpenseAmount,0) as TotalExpenseAmount,
	ISNULL(sq.Latitude,0) as Latitude,
	ISNULL(sq.Longitude,0) as Longitude,
	ISNULL(sq.PhoneModel, '') AS PhoneModel,
	ISNULL(sq.PhoneOS, '') AS PhoneOS,
	ISNULL(sq.AppVersion, '') AS AppVersion

	FROM cteAreaCodes cte
	FULL OUTER JOIN 
			(
				-- Employee Code and Area Codes, IsInFieldToday, IsRegisteredOnPhone
				SELECT sp.StaffCode, 
				ISNULL(oh.AreaCode, '***') AreaCode, 
				CASE WHEN ed.EmployeeId IS NULL THEN 0 ELSE 1 END IsInFieldToday,
				CASE WHEN te.Id IS NULL THEN 0 ELSE 1 END IsRegisteredOnPhone,

				ed.StartTime,
				ed.EndTime,
				ed.TotalOrderAmount,
				ed.TotalPaymentAmount,
				ed.TotalReturnAmount,
				ed.TotalExpenseAmount,
				ed.Latitude,
				ed.Longitude,
				ed.PhoneModel,
				ed.PhoneOS,
				ed.AppVersion

				FROM dbo.SalesPerson sp
				LEFT join dbo.OfficeHierarchy oh on sp.HQCode = oh.HQCode and oh.IsActive = 1
				LEFT JOIN dbo.TenantEmployee te on te.EmployeeCode = sp.StaffCode and te.IsActive = 1
				LEFT JOIN @SignedInEmployeeData ed on te.Id = ed.EmployeeId
				WHERE sp.IsActive = 1
			) sq ON cte.AreaCode = sq.AreaCode

  -- Cases covered
  -- 1. SalesPerson table may have a HQ code that does not exist in Office hierarchy
  --    We still want such a record in the resultset with Areacode as '***'
END

go
