CREATE PROCEDURE [dbo].[TransformStaffDivisionData]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
		TRUNCATE TABLE dbo.[StaffDivision]

		-- Step 2: Insert data
		INSERT INTO dbo.[StaffDivision]
		(StaffCode, [DivisionCode])
		SELECT  
		rtrim(ltrim([Staff Code])),
		rtrim(ltrim([Division Code]))
		FROM dbo.StaffDivisionInput

		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformStaffDivisionData', 'Success'
END
