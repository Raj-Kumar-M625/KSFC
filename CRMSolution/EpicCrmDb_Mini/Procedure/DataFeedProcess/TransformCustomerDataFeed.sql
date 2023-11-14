CREATE PROCEDURE [dbo].[TransformCustomerDataFeed]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
		-- Stored procedure to put data from SAP data feed (tables)
		-- to local tables

		-- Step 1: Delete existing data if asked to do so
		-- Input Tables
		-- CustomerMaster

		IF @IsCompleteRefresh = 1
		BEGIN
			TRUNCATE TABLE dbo.Customer
		END

		-- Step 2: Insert data in Customer table
		INSERT INTO [dbo].[Customer]
		([CustomerCode], [Name], [Type], [CreditLimit], 
		[Outstanding], [LongOutstanding], [HQCode], [ContactNumber],
		[Target], [Sales], [Payment])
		SELECT 
		left(cm.[Customer Code], 20),
		left(cm.[Customer Name],100), 
		cm.[Type], 
		ISNULL(cm.[Credit Limit],0),
		ISNULL([Total Outstanding], 0),
		ISNULL([Total Long Overdue], 0),
		left(ISNULL([HQ Code], ''),10),
		ltrim(IsNULL([Contact Phone], '0')),
		ISNULL([Expected Business], 0), -- Target
		ISNULL(cm.[Sales],0),  -- Sales
		ISNULL([Collection], 0) -- Payment
		FROM dbo.CustomerMaster cm
		LEFT JOIN dbo.Customer c on cm.[Customer Code] IS NOT NULL AND
			left(cm.[Customer Code], 20) = c.CustomerCode
		WHERE cm.[Type] IS NOT NULL
		AND c.CustomerCode IS NULL


		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformCustomerDataFeed', 'Success'
END
