CREATE PROCEDURE [dbo].[TransformCustomerDivisionBalance]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
		TRUNCATE TABLE dbo.CustomerDivisionBalance

		-- Step 2: Insert data in CustomerDivisionBalance table
		INSERT INTO [dbo].[CustomerDivisionBalance]
		([DATE], [CustomerCode], [DivisionCode], [SegmentCode], 
		[CreditLimit], [Outstanding], [LongOutstanding], 
		[Target], [Sales], [Payment])
		SELECT 
		[DATE],
		left([Customer Code], 20),
		ltrim(rtrim([Division Code])),
		ltrim(rtrim([ItemSegmentCode])),

		ISNULL([Credit Limit],0),
		ISNULL([Total Balance YTD], 0),
		ISNULL([Total Overdue], 0),
		ISNULL([Target Sales YTD] ,0),
		ISNULL([Total Sales],0),
		ISNULL([Total Payment YTD],0)
		FROM dbo.CustomerDivisionBalanceInput

		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformCustomerDivisionBalance', 'Success'
END
