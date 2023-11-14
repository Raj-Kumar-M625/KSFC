CREATE PROCEDURE [dbo].[TransformDataFeed]
	@tenantId BIGINT
AS
BEGIN
		-- Step 1: Update names changes from SalesPerson table to TenantEmployee table
		UPDATE dbo.TenantEmployee
		SET Name = sp.Name
		FROM dbo.TenantEmployee te
		INNER JOIN dbo.SalesPerson sp on te.EmployeeCode = sp.StaffCode
		AND te.Name <> sp.Name

		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformDataFeed', 'Success'
END
