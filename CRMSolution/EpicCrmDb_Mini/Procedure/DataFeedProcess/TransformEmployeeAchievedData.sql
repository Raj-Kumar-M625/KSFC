CREATE PROCEDURE [dbo].[TransformEmployeeAchievedData]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
		-- Step 1: Truncate table if opted to do so

		TRUNCATE TABLE dbo.[EmployeeAchieved]

		-- Insert New data
		INSERT INTO dbo.EmployeeAchieved
		(
			[EmployeeCode],
			[Month],
			[Year],
			[Type],
			[AchievedMonthly]
		)
		SELECT  
			[Employee Code],
			[Month],
			[Year],
			[Type],
			[Achieved Monthly]		
		FROM dbo.EmployeeAchievedInput

		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformEmployeeAchievedData', 'Success'
END
GO
