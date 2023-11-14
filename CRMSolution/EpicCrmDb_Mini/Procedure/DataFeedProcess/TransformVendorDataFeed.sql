CREATE PROCEDURE [dbo].[TransformVendorDataFeed]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
		-- Step 1: Truncate table if opted to do so

		IF @IsCompleteRefresh = 1
		BEGIN
			TRUNCATE TABLE dbo.Vendor
		END

		-- Update existing Data
		UPDATE dbo.Vendor
		SET 
			[CompanyName] = ti.[Company name],
			[ContactPerson] = ti.[Contact person],
			[Address] = ti.[Address],
			[Mobile] = ti.[Mobile]
		FROM dbo.Vendor t
		INNER JOIN dbo.VendorInput ti on t.VendorId = ti.[Vendor Id]

		-- Insert New data
		INSERT INTO dbo.Vendor
		(
			[VendorId],
			[CompanyName],
			[ContactPerson],
			[Address],
			[Mobile]
		)
		SELECT  
			[Vendor Id],
			[Company name],
			[Contact person],
			ti.[Address],
			ti.[Mobile]
		
		FROM dbo.VendorInput ti
		LEFT JOIN dbo.Vendor t ON ti.[Vendor Id] = t.VendorId
		WHERE t.VendorId IS NULL


		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformVendorDataFeed', 'Success'
END
GO
