CREATE PROCEDURE [dbo].[TransformStaffMessageData]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
		TRUNCATE TABLE dbo.[StaffMessage]

		-- Step 2: Insert data
		INSERT INTO dbo.[StaffMessage]
		([DATE], StaffCode, [Message])
		SELECT  
		[DATE],
		rtrim(ltrim([Staff Code])),
		ltrim(rtrim([Message]))
		FROM dbo.StaffMessageInput

		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformStaffMessageData', 'Success'
END
