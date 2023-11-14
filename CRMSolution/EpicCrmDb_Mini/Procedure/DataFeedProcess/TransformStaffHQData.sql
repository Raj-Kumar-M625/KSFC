CREATE PROCEDURE [dbo].[TransformStaffHQData]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN

	--DECLARE @insertTable TABLE ([Id] INT NOT NULL PRIMARY KEY Identity,
	--[StaffCode] NVARCHAR(10) NOT NULL,
	--[CodeType] NVARCHAR(50) NOT NULL,
	--[CodeValue] NVARCHAR(50) NOT NULL,
	--[IsDeleted] BIT NOT NULL DEFAULT 0,
	--[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	--[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	--[CreatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	--[UpdatedBy] NVARCHAR(50) NOT NULL DEFAULT '')
	-- Step 1: Get all exiting assignment data in temp table (Except HQ Assignment)
	--SELECT * FROM dbo.SalesPersonAssociation
	--WHERE CodeType = 'HeadQuarter'

		-- Step 1: Delete HQ Assignment table if opted to do so
		IF @IsCompleteRefresh = 1
		BEGIN
			DELETE FROM dbo.[SalesPersonAssociation]
			WHERE LEN(StaffCode) < 10 AND CodeType = 'HeadQuarter'
		END
		ELSE
		BEGIN
			DELETE FROM dbo.[SalesPersonAssociation]
			WHERE StaffCode IN 
			(	SELECT DISTINCT(ltrim([Staff Code]))
				FROM dbo.[StaffHQInput]
			) AND CodeType = 'HeadQuarter'
		END


		-- Insert New data
		INSERT INTO dbo.[SalesPersonAssociation]
		(
			[StaffCode], [CodeType], [CodeValue], [IsDeleted], [DateCreated], [DateUpdated], [CreatedBy], [UpdatedBy]
		)
		SELECT  
			ltrim([Staff Code]),
			'HeadQuarter',
			ltrim(rtrim([HQ Code])),
			0,
			SYSUTCDATETIME(),
			SYSUTCDATETIME(),
			'TransformStaffHQData',
			'TransformStaffHQData'
		FROM dbo.[StaffHQInput]

		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformStaffHQData', 'Success'
END
GO