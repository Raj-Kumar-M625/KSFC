CREATE PROCEDURE [dbo].[TransformEmployeeMonthlyTargetData]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
		-- Step 1: Truncate table if opted to do so

		TRUNCATE TABLE dbo.[EmployeeMonthlyTarget]

		-- Insert New data
		INSERT INTO dbo.EmployeeMonthlyTarget
		(
			[EmployeeCode],
			[Month],
			[Year],
			[Type],
			[MonthlyTarget]
		)
		SELECT  
			[Employee Code],
			[Month],
			[Year],
			[Type],
			[Monthly Target]		
		FROM dbo.EmployeeMonthlyTargetInput

		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformEmployeeMonthlyTargetData', 'Success'
END
GO
