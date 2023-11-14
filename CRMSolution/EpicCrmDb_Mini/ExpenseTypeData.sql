--DECLARE @ExpenseName NVARCHAR(50)

--SET @ExpenseName = 'Repair'
--IF NOT EXISTS(SELECT 1 FROM dbo.ExpenseType WHERE ExpenseTypeCode = @ExpenseName)
--BEGIN
--	INSERT INTO dbo.ExpenseType (ExpenseTypeCode, [DisplaySequence]) Values (@ExpenseName, 10)
--END

--SET @ExpenseName = 'Daily Allowance'
--IF NOT EXISTS(SELECT 1 FROM dbo.ExpenseType WHERE ExpenseTypeCode = @ExpenseName)
--BEGIN
--	INSERT INTO dbo.ExpenseType (ExpenseTypeCode, [DisplaySequence]) Values (@ExpenseName, 20)
--END

--SET @ExpenseName = 'Telephone'
--IF NOT EXISTS(SELECT 1 FROM dbo.ExpenseType WHERE ExpenseTypeCode = @ExpenseName)
--BEGIN
--	INSERT INTO dbo.ExpenseType (ExpenseTypeCode, [DisplaySequence]) Values (@ExpenseName, 30)
--END

--SET @ExpenseName = 'Internet'
--IF NOT EXISTS(SELECT 1 FROM dbo.ExpenseType WHERE ExpenseTypeCode = @ExpenseName)
--BEGIN
--	INSERT INTO dbo.ExpenseType (ExpenseTypeCode, [DisplaySequence]) Values (@ExpenseName, 40)
--END

--SET @ExpenseName = 'Lodge/Rent'
--IF NOT EXISTS(SELECT 1 FROM dbo.ExpenseType WHERE ExpenseTypeCode = @ExpenseName)
--BEGIN
--	INSERT INTO dbo.ExpenseType (ExpenseTypeCode, [DisplaySequence]) Values (@ExpenseName, 50)
--END


--SET @ExpenseName = 'Travel-Public'
--IF NOT EXISTS(SELECT 1 FROM dbo.ExpenseType WHERE ExpenseTypeCode = @ExpenseName)
--BEGIN
--	INSERT INTO dbo.ExpenseType (ExpenseTypeCode, [DisplaySequence]) Values (@ExpenseName, 60)
--END


--SET @ExpenseName = 'Travel-Private'
--IF NOT EXISTS(SELECT 1 FROM dbo.ExpenseType WHERE ExpenseTypeCode = @ExpenseName)
--BEGIN
--	INSERT INTO dbo.ExpenseType (ExpenseTypeCode, [DisplaySequence]) Values (@ExpenseName, 70)
--END


----------------------------------
-- Transport Type Data
----------------------------------
DECLARE @TransportTypeCode VARCHAR(50)

SET @TransportTypeCode = 'Bus'
IF NOT EXISTS(SELECT 1 FROM dbo.TransportType WHERE TransportTypeCode = @TransportTypeCode)
BEGIN
	INSERT INTO dbo.TransportType
	(DisplaySequence, TransportTypeCode, ReimbursementRatePerUnit, IsPublic)
	VALUES
	(10, @TransportTypeCode, 0, 1)
END


SET @TransportTypeCode = 'Train'
IF NOT EXISTS(SELECT 1 FROM dbo.TransportType WHERE TransportTypeCode = @TransportTypeCode)
BEGIN
	INSERT INTO dbo.TransportType
	(DisplaySequence, TransportTypeCode, ReimbursementRatePerUnit, IsPublic)
	VALUES
	(20, @TransportTypeCode, 0, 1)
END


SET @TransportTypeCode = 'Auto'
IF NOT EXISTS(SELECT 1 FROM dbo.TransportType WHERE TransportTypeCode = @TransportTypeCode)
BEGIN
	INSERT INTO dbo.TransportType
	(DisplaySequence, TransportTypeCode, ReimbursementRatePerUnit, IsPublic)
	VALUES
	(30, @TransportTypeCode, 0, 1)
END


SET @TransportTypeCode = 'Taxi'
IF NOT EXISTS(SELECT 1 FROM dbo.TransportType WHERE TransportTypeCode = @TransportTypeCode)
BEGIN
	INSERT INTO dbo.TransportType
	(DisplaySequence, TransportTypeCode, ReimbursementRatePerUnit, IsPublic)
	VALUES
	(40, @TransportTypeCode, 0, 1)
END


SET @TransportTypeCode = 'Flight'
IF NOT EXISTS(SELECT 1 FROM dbo.TransportType WHERE TransportTypeCode = @TransportTypeCode)
BEGIN
	INSERT INTO dbo.TransportType
	(DisplaySequence, TransportTypeCode, ReimbursementRatePerUnit, IsPublic)
	VALUES
	(50, @TransportTypeCode, 0, 1)
END


SET @TransportTypeCode = 'Two Wheeler'
IF NOT EXISTS(SELECT 1 FROM dbo.TransportType WHERE TransportTypeCode = @TransportTypeCode)
BEGIN
	INSERT INTO dbo.TransportType
	(DisplaySequence, TransportTypeCode, ReimbursementRatePerUnit, IsPublic)
	VALUES
	(60, @TransportTypeCode, 2.4, 0)
END


SET @TransportTypeCode = 'Four Wheeler'
IF NOT EXISTS(SELECT 1 FROM dbo.TransportType WHERE TransportTypeCode = @TransportTypeCode)
BEGIN
	INSERT INTO dbo.TransportType
	(DisplaySequence, TransportTypeCode, ReimbursementRatePerUnit, IsPublic)
	VALUES
	(70, @TransportTypeCode, 7.5, 0)
END

---------------

INSERT into dbo.Config
(ConfigName, ConfigBooleanValue)
values
('MinimizeMapApiCallsForDistance', 1)
GO

INSERT into dbo.Config
(ConfigName, ConfigBooleanValue)
values
('LogDistanceCalcError', 1)
GO
