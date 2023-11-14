CREATE TABLE [dbo].[StaffHQInput]
(
	[Staff Code] NVARCHAR(10) NOT NULL,
	[HQ Code] NVARCHAR(10) NOT NULL
)
GO

CREATE TABLE [dbo].[PPAStaffInput]
(
	[PPA Code] NVARCHAR(10) NOT NULL,
	[Staff Code] NVARCHAR(10) NOT NULL
)
GO

INSERT into dbo.codeTable
(codeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
('ExcelUpload', 'StaffHQMapping', 'StaffHQInput', 180, 1, 1),
('ExcelUpload', 'PPAStaffMapping', 'PPAStaffInput', 190, 1, 1)
GO


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
		GROUP BY [Staff Code], [HQ Code] --- In order to exclude duplicate inserts

		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformStaffHQData', 'Success'
END
GO



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

		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformPPAStaffData', 'Success'
END
GO