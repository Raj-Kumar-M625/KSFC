CREATE PROCEDURE [dbo].[RegisterUser]
    @tenantId BIGINT,
	@timeIntervalInMs BIGINT,
	@staffCode NVARCHAR(10),
	@imei NVARCHAR(50),
	@outStatus INT OUTPUT,
	@outEmployeeId BIGINT OUTPUT
AS
BEGIN
	-- RETURN Values of outStatus
	-- -1 User does not exist or blocked in SAP
	-- -2 User access is blocked in CRM
	-- -3 User is registered on another phone
	-- 1 User is already registered and active on current phone
	-- 2 New user registration
	SET @outStatus = 0;
	SET @outEmployeeId = -1;

	-- a. Check in SalesPerson table
	IF NOT EXISTS(SELECT 1 FROM dbo.SalesPerson WHERE CAST(StaffCode AS NVARCHAR(10)) = @staffCode AND IsActive = 1)
	BEGIN
	   SET @outStatus = -1;
	   RETURN
	END

	-- b. check in TenantEmployee table
	DECLARE @isActive BIT
	DECLARE @employeeId BIGINT
	SELECT @isActive = IsActive, @employeeId = Id FROM dbo.TenantEmployee WHERE EmployeeCode = @staffCode

	IF @employeeId IS NOT NULL -- record exist
	BEGIN
		-- retrieve status value
		IF @isActive = 0
		BEGIN
		   -- user access is blocked
		   SET @outStatus = -2;
		   RETURN;
		END
		
		-- check record in IMEI table
		DECLARE @imeiIsActiveStatus BIT
		DECLARE @imeiFromTable NVARCHAR(50)
		SELECT TOP(1) @imeiFromTable = IMEINumber FROM dbo.IMEI WHERE TenantEmployeeId = @employeeId AND IsActive = 1
		IF @imeiFromTable IS NOT NULL
		BEGIN
			IF @imeiFromTable = @imei
			BEGIN
			    -- user is coming back on same phone - may be after a app refresh
				SET @outEmployeeId = @employeeId;
				SET @outStatus = 1;
				RETURN;
			END
			-- user is registered on another phone
			SET @outStatus = -3;
			RETURN;
		END
	END

	-- ALLOW USER SIGNUP
	SELECT @outEmployeeId = Id FROM dbo.TenantEmployee WHERE EmployeeCode = @staffCode
	IF @outEmployeeId IS NULL OR @outEmployeeId <= 0
	BEGIN
		--DECLARE @tenantId BIGINT
		--DECLARE @timeIntervalInMs BIGINT
		--SELECT TOP(1) @tenantId = Id, @timeIntervalInMs = [TimeIntervalInMillisecondsForTracking] FROM dbo.Tenant;

		INSERT INTO dbo.TenantEmployee (TenantId, ManagerId, Name, EmployeeCode, TimeIntervalInMillisecondsForTracking, IsActive)
		SELECT @tenantId, NULL, sp.Name, @staffCode, @timeIntervalInMs, 1
		FROM dbo.SalesPerson sp
		WHERE CAST(sp.StaffCode AS NVARCHAR(10)) = @staffCode

		SET @outEmployeeId = SCOPE_IDENTITY()
	END

	-- before inserting a new record in dbo.IMEI
	-- clear the status of all other records where imei is @imei
	UPDATE dbo.IMEI SET IsActive = 0
	WHERE IMEINumber = @imei

	INSERT INTO dbo.IMEI (IMEINumber, TenantEmployeeId, IsActive)
	VALUES (@imei, @outEmployeeId, 1)

	SET @outStatus = 2;

END

