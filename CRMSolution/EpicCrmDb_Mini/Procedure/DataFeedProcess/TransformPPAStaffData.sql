CREATE PROCEDURE [dbo].[TransformPPAStaffData]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN

		-- Step 1 : Delete Full HQ Assignment Data for Existing PPA assignments in case of complete refresh
		IF @IsCompleteRefresh = 1 
		BEGIN
			DELETE FROM dbo.[SalesPersonAssociation] 
			WHERE LEN(StaffCode) = 10 AND CodeType = 'HeadQuarter' -- Length (10) for PPA defines specifically for Tstanes
		END
		-- Delete Partial PPA assignment data, Only for PPA code newly uploaded
		ELSE
		BEGIN
			DELETE FROM dbo.[SalesPersonAssociation]
			WHERE StaffCode IN 
			(	SELECT DISTINCT(ltrim([PPA Code]))
				FROM dbo.[PPAStaffInput]
			) AND CodeType = 'HeadQuarter'
		END


		-- Step 2 : Insert New data
		INSERT INTO dbo.[SalesPersonAssociation]
		(
			[StaffCode], [CodeType], [CodeValue], [IsDeleted], [DateCreated], [DateUpdated], [CreatedBy], [UpdatedBy]
		)
		SELECT 
			p.[PPA Code],
			'HeadQuarter',
			s.[HQ Code],
			0,
			SYSUTCDATETIME(),
			SYSUTCDATETIME(),
			'TransformPPAStaffData',
			'TransformPPAStaffData'
		FROM dbo.[PPAStaffInput] p
		INNER JOIN dbo.[StaffHQInput] s
		ON p.[Staff Code] = s.[Staff Code]
		GROUP BY p.[PPA Code], s.[HQ code]


		-- Step 3 : Update HQcode for all the PPA in SalesPerson Table
		-- Note : It will update HQCode for only those PPA where its respective SO mapping is available

		DECLARE @ppaHQ TABLE (StaffCode NVARCHAR(10), HQCode NVARCHAR(10))

		INSERT INTO @ppaHQ
		(
			[StaffCode], [HQCode]
		)
		SELECT a.StaffCode, a.CodeValue
		FROM dbo.salespersonassociation a
		INNER JOIN 
		(
		SELECT StaffCode, MIN(id) AS id FROM dbo.salespersonassociation
		WHERE Len(StaffCode) > 9 AND CodeType = 'HeadQuarter'
		GROUP BY StaffCode
		) AS b
		ON a.StaffCode = b.StaffCode
		AND a.id = b.id


		UPDATE s
		SET s.HQCode = p.HQCode		
		FROM dbo.[SalesPerson] s
		INNER JOIN @ppaHQ p
		ON s.[StaffCode] = p.StaffCode
		WHERE BusinessRole = 'PPA'

		-- Here we are disabling PPA ID's for which correct SO mapping is not available

		UPDATE s
		SET s.IsActive = 0
		FROM dbo.SalesPerson s
		WHERE s.StaffCode NOT IN 
		(
		SELECT DISTINCT(StaffCode) FROM @ppaHQ
		)
		AND BusinessRole = 'PPA'
		
		-------------------------

		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformPPAStaffData', 'Success'
END
GO