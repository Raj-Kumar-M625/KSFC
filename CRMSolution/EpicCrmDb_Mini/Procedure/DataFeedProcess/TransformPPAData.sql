CREATE PROCEDURE [dbo].[TransformPPAData]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
		DELETE FROM dbo.PPA where TenantId = @tenantId

		-- Step 2: Insert data
		INSERT INTO dbo.[PPA]
		(TenantId, AreaCode, StaffCode, PPACode, PPAName, PPAContact, [Location])
		SELECT  
		@tenantId,
		[Branch Code],
		[EMP ID],
		[PPA Code],
		[PPA Name], 
		[Contact Number],
		[Location]
		FROM dbo.PPAInput

		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformPPAData', 'Success'
END
