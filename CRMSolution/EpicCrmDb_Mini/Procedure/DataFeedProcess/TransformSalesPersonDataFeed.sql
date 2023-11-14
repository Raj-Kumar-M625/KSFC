CREATE PROCEDURE [dbo].[TransformSalesPersonDataFeed]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
		-- Stored procedure to put data from SAP data feed (tables)
		-- to local tables

		-- Step 1: Truncate table if opted to do so
		-- Input Tables
		-- EmployeeMaster

		IF @IsCompleteRefresh = 1
		BEGIN
			TRUNCATE TABLE dbo.SalesPerson
		END

		-- Step 2: Insert data in SalesPerson table
		INSERT INTO dbo.SalesPerson
		([StaffCode], [Name], [Phone], [HQCode], [IsActive], [Department], [Designation], [BusinessRole], [OverridePrivateVehicleRatePerKM], [TwoWheelerRatePerKM], [FourWheelerRatePerKM])
		SELECT  ltrim([Staff Code]),
		ISNULL(ltrim(rtrim(em.Name)),''),
		ltrim(Str(IsNULL(em.[Phone], 0), 50, 0)),
		ltrim(rtrim(ISNULL([Head Quarter], ''))),
		Case when ltrim(rtrim([Action])) = 'ACTIVE' THEN 1 ELSE 0 END,
		ltrim(rtrim(ISNULL([DepartmentOrDivision], ''))),
		ltrim(rtrim(ISNULL(em.[Designation], ''))),
		ltrim(rtrim(ISNULL([Business Role], ''))),
		Case when ltrim(rtrim([Expense Rate Override])) ='YES' THEN 1 ELSE 0 END,
		ltrim(rtrim(ISNULL([Two Wheeler RatePerKM], 0))),
		ltrim(rtrim(ISNULL([Four Wheeler RatePerKM], 0)))
		FROM dbo.EmployeeMaster em
		LEFT JOIN dbo.SalesPerson sp ON [Staff Code] IS NOT NULL
				AND ltrim(em.[Staff Code]) = sp.StaffCode
		WHERE sp.StaffCode IS NULL


		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformSalesPersonDataFeed', 'Success'
END