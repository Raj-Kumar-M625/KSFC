-- Nov 21 2019
DROP TABLE [dbo].[PPAInput]
GO

CREATE TABLE [dbo].[PPAInput]
(
	[Branch Code] [nvarchar](10) NOT NULL, 
	[Branch Name] [nvarchar](50) NOT NULL, 
	[EMP ID] [nvarchar](10) NOT NULL, 
	[EMP Name] [nvarchar](50) NOT NULL,
	[PPA Code] [Nvarchar](20) NOT NULL, 
	[PPA Name] [Nvarchar](50) NOT NULL, 
	[Contact Number] [NVARCHAR](20) NOT NULL, 
	[Location] NVARCHAR(50) NOT NULL
)
GO

CREATE TABLE [dbo].[PPA]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[TenantId] BIGINT NOT NULL REFERENCES dbo.Tenant,
	[AreaCode] [nvarchar](10) NOT NULL, 
	[StaffCode] NVARCHAR(10) NOT NULL,
	[PPACode] [Nvarchar](20) NOT NULL, 
	[PPAName] [Nvarchar](50) NOT NULL, 
	[PPAContact] [NVARCHAR](20) NOT NULL, 
	[Location] NVARCHAR(50) NOT NULL
)
GO

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
GO

------------------------------
-- Capture date from TStanes Input files
------------------------------
DROP TABLE [dbo].[StaffDailyReportDataInput]
GO

CREATE TABLE [dbo].[StaffDailyReportDataInput]
(
    [Date] DATE NOT NULL,
	[Branch Code] [nvarchar](20) NOT NULL,
	[Branch Name] [nvarchar](100) NOT NULL,
	[Division Code] [nvarchar](20) NOT NULL, --*
	[Division Name] [nvarchar](50) NOT NULL, --*
	[Staff Code] [nvarchar](10) NOT NULL, --*
	[Staff Name] [nvarchar](50) NOT NULL, --*
	[ItemSegmentCode] [NVARCHAR](20) NOT NULL,
	[ItemSegmentName] [NVARCHAR](50) NOT NULL,

	[Target Outstanding YTD] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Total Cost YTD] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[CGA YTD] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[GT 180 YTD] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Collection Target YTD] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Collection Actual YTD] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Sales Target YTD] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Sales Actual YTD] DECIMAL(19,2) NOT NULL DEFAULT 0,
)
go

DROP TABLE [dbo].[StaffDailyReportData]
GO

CREATE TABLE [dbo].[StaffDailyReportData]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[DATE] DATE NOT NULL,
	[TenantId] BIGINT NOT NULL REFERENCES dbo.Tenant,
	[StaffCode] [nvarchar](10) NOT NULL, --*
	[DivisionCode] [nvarchar](20) NOT NULL, --*
	[SegmentCode] [NVARCHAR](20) NOT NULL,

	[TargetOutstandingYTD] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[TotalCostYTD] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[CGAYTD] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[GT180YTD] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[CollectionTargetYTD] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[CollectionActualYTD] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[SalesTargetYTD] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[SalesActualYTD] DECIMAL(19,2) NOT NULL DEFAULT 0,
)
GO

ALTER PROCEDURE [dbo].[TransformStaffDailyReportData]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
		--DELETE FROM dbo.[StaffDailyReportData] WHERE TenantId = @tenantId
		-- doing a truncate to reset the id to 1, else on daily basis
		-- given the high volume of data, the ids may become too large soon.
		TRUNCATE TABLE dbo.[StaffDailyReportData]

		INSERT INTO dbo.StaffDailyReportData
		([DATE], TenantId, [StaffCode], [DivisionCode], [SegmentCode],
			[TargetOutstandingYTD],
			[TotalCostYTD],
			[CGAYTD],
			[GT180YTD],
			[CollectionTargetYTD],
			[CollectionActualYTD],
			[SalesTargetYTD],
			[SalesActualYTD]
		)
		SELECT [DATE], @tenantId, [Staff Code],	[Division Code], [ItemSegmentCode],
			[Target Outstanding YTD],
			[Total Cost YTD],
			[CGA YTD],
			[GT 180 YTD],
			[Collection Target YTD],
			[Collection Actual YTD],
			[Sales Target YTD],
			[Sales Actual YTD]
		FROM dbo.StaffDailyReportDataInput


		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformDailyReportData', 'Success'
END
GO

---------------------

DROP TABLE dbo.StaffMessageInput
GO

CREATE TABLE [dbo].[StaffMessageInput]
(
    [Date] DATE NOT NULL,
	[Staff Code] [nvarchar](10) NOT NULL, --*
	[Staff Name] [nvarchar](50) NOT NULL, --*
	[Message] [nvarchar](100) NOT NULL --*
)
go

DROP TABLE [dbo].[StaffMessage]
GO

CREATE TABLE [dbo].[StaffMessage]
(
    [ID] BIGINT Primary Key Identity,
	[DATE] DATE NOT NULL,
	[StaffCode] [nvarchar](10) NOT NULL, --*
	[Message] [nvarchar](100) NOT NULL --*
)
GO

ALTER PROCEDURE [dbo].[TransformStaffMessageData]
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
GO
-----------------------

DROP TABLE [dbo].[CustomerDivisionBalanceInput]
GO

CREATE TABLE [dbo].[CustomerDivisionBalanceInput]
(
    [Date] DATE NOT NULL,
	[Customer Code] [nvarchar](20) NOT NULL,
	[Customer Name] [nvarchar](100) NOT NULL,
	[Division Code] [nvarchar](20) NOT NULL, --*
	[Division Name] [nvarchar](50) NOT NULL, --*

	[Credit Limit] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Target Sales YTD] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Total Balance YTD] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Total Overdue] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Contact Phone] [NVARCHAR] (50) NULL DEFAULT '0',
	[Total Sales] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Total Payment YTD] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[ItemSegmentCode] [NVARCHAR](20) NOT NULL,
	[ItemSegmentName] [NVARCHAR](50) NOT NULL
)
go

DROP TABLE [dbo].[CustomerDivisionBalance]
GO

CREATE TABLE [dbo].[CustomerDivisionBalance]
(
	[ID] BIGINT Primary Key Identity,
	[DATE] DATE NOT NULL,
	[CustomerCode] [nvarchar](20) NOT NULL,
	[DivisionCode] [nvarchar](20) NOT NULL, --*
	[SegmentCode] [NVARCHAR](20) NOT NULL,

	[CreditLimit] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Outstanding] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[LongOutstanding] DECIMAL(19,2) NOT NULL,

	[Target] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Sales] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Payment] DECIMAL(19,2) NOT NULL DEFAULT 0
)
GO

ALTER PROCEDURE [dbo].[TransformCustomerDivisionBalance]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
		TRUNCATE TABLE dbo.CustomerDivisionBalance

		-- Step 2: Insert data in CustomerDivisionBalance table
		INSERT INTO [dbo].[CustomerDivisionBalance]
		([DATE], [CustomerCode], [DivisionCode], [SegmentCode], 
		[CreditLimit], [Outstanding], [LongOutstanding], 
		[Target], [Sales], [Payment])
		SELECT 
		[DATE],
		left([Customer Code], 20),
		ltrim(rtrim([Division Code])),
		ltrim(rtrim([ItemSegmentCode])),

		ISNULL([Credit Limit],0),
		ISNULL([Total Balance YTD], 0),
		ISNULL([Total Overdue], 0),
		ISNULL([Target Sales YTD] ,0),
		ISNULL([Total Sales],0),
		ISNULL([Total Payment YTD],0)
		FROM dbo.CustomerDivisionBalanceInput

		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformCustomerDivisionBalance', 'Success'
END
GO

----------------

ALTER PROCEDURE [dbo].[GetCustomerDivisionBalance]
    @tenantId BIGINT,
	@staffCode NVARCHAR(10),
	@areaCode NVARCHAR(10)
AS
BEGIN

	DECLARE @OfficeHierarchy TABLE
	(
		[ZoneCode] NVARCHAR(10),
		[ZoneName] NVARCHAR(50),
		[AreaCode] NVARCHAR(10),
		[AreaName] NVARCHAR(50),
		[TerritoryCode] NVARCHAR(10),
		[TerritoryName] NVARCHAR(50),
		[HQCode] NVARCHAR(10),
		[HQName] NVARCHAR(50)
	)

	-- Get Office Hierarchy
	INSERT INTO @OfficeHierarchy
	(ZoneCode, ZoneName, AreaCode, AreaName, TerritoryCode, TerritoryName, HQCode, HQName)
	exec [GetOfficeHierarchyForStaff] @tenantId, @staffCode

	-- select distinct hq codes
	DECLARE @hqcodes TABLE (HQCode NVARCHAR(10))

	INSERT INTO @hqcodes (HQCode)
	SELECT Distinct HQCode FROM @OfficeHierarchy
	WHERE AreaCode = @areaCode

	SELECT 
	cdb.[Date],
	cdb.CustomerCode,
	cdb.DivisionCode,
	cdb.SegmentCode,
	cdb.CreditLimit,
	cdb.Outstanding,
	cdb.LongOutstanding,
	cdb.[Target],
	cdb.Sales,
	cdb.Payment
	FROM dbo.Customer c
	INNER JOIN @hqcodes hq on c.HQCode = hq.HQCode
	and c.IsActive = 1
	INNER JOIN dbo.CustomerDivisionBalance cdb on c.CustomerCode = cdb.CustomerCode
END
GO
