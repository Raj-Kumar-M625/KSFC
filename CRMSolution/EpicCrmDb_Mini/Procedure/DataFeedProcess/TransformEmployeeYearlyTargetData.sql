CREATE PROCEDURE [dbo].[TransformEmployeeYearlyTargetData]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
		-- Step 1: Truncate table if opted to do so

		TRUNCATE TABLE dbo.[EmployeeYearlyTarget]

		-- Insert New data
		INSERT INTO dbo.EmployeeYearlyTarget
		(
			[EmployeeCode],
			[Year],
			[Type],
			[YearlyTarget]
		)
		SELECT  
			[Employee Code],
			[Year],
			[Type],
			[Yearly Target]		
		FROM dbo.EmployeeYearlyTargetInput

		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformEmployeeYearlyTargetData', 'Success'
END
GO
