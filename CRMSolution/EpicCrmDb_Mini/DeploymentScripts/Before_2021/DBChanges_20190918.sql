ALTER table [dbo].[CustomerMaster]
drop column [District Name],
	[State Name],
	[Branch Name],
	[Pincode]
go

Alter table dbo.Customer
drop column 
	[District] ,
	[State],
	[Branch] ,
	[Pincode] 
go

ALTER PROCEDURE [dbo].[TransformCustomerDataFeed]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
		-- Stored procedure to put data from SAP data feed (tables)
		-- to local tables

		-- Step 1: Delete existing data if asked to do so
		-- Input Tables
		-- CustomerMaster

		IF @IsCompleteRefresh = 1
		BEGIN
			TRUNCATE TABLE dbo.Customer
		END

		-- Step 2: Insert data in Customer table
		INSERT INTO [dbo].[Customer]
		([CustomerCode], [Name], [Type], [CreditLimit], 
		[Outstanding], [LongOutstanding], [HQCode], [ContactNumber],
		[Target], [Sales], [Payment])
		SELECT 
		left(cm.[Customer Code], 20),
		left(cm.[Customer Name],100), 
		cm.[Type], 
		ISNULL(cm.[Credit Limit],0),
		ISNULL([Total Outstanding], 0),
		ISNULL([Total Long Overdue], 0),
		left(ISNULL([HQ Code], ''),10),
		ltrim(IsNULL([Contact Phone], '0')),
		ISNULL([Expected Business], 0), -- Target
		ISNULL(cm.[Sales],0),  -- Sales
		ISNULL([Collection], 0) -- Payment
		FROM dbo.CustomerMaster cm
		LEFT JOIN dbo.Customer c on cm.[Customer Code] IS NOT NULL AND
			left(cm.[Customer Code], 20) = c.CustomerCode
		WHERE cm.[Type] IS NOT NULL
		AND c.CustomerCode IS NULL


		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformCustomerDataFeed', 'Success'
END
go

ALTER TABLE [dbo].[EmployeeMaster]
ADD	[DepartmentOrDivision] NVARCHAR(50)
GO

ALTER PROCEDURE [dbo].[TransformSalesPersonDataFeed]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
		-- Stored procedure to put data from SAP data feed (tables)
		-- to local tables

		-- Step 1: Truncate table if opted to do so
		-- Input Tables
		-- EmployeeMaster

		IF @IsCompleteRefresh = 1
		BEGIN
			TRUNCATE TABLE dbo.SalesPerson
		END

		-- Step 2: Insert data in SalesPerson table
		INSERT INTO dbo.SalesPerson
		([StaffCode], [Name], [Phone], [HQCode], [IsActive], [Department])
		SELECT  ltrim([Staff Code]),
		ISNULL(ltrim(rtrim(em.Name)),''),
		ltrim(Str(IsNULL(em.[Phone], 0), 50, 0)),
		ltrim(rtrim(ISNULL([Head Quarter], ''))),
		Case when ltrim(rtrim([Action])) = 'ACTIVE' THEN 1 ELSE 0 END,
		rtrim(ltrim(ISNULL(DepartmentOrDivision, '')))
		FROM dbo.EmployeeMaster em
		LEFT JOIN dbo.SalesPerson sp ON [Staff Code] IS NOT NULL
				AND ltrim(em.[Staff Code]) = sp.StaffCode
		WHERE sp.StaffCode IS NULL


		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformSalesPersonDataFeed', 'Success'
END
go

INSERT into dbo.codeTable
(codeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
('ExcelUpload', 'DivisionSegment', 'DivisionSegment', 60, 1, 1),
('ExcelUpload', 'Staff Message', 'StaffMessageInput', 70, 1, 1),
('ExcelUpload', 'Customer Division Balance', 'CustomerDivisionBalanceInput', 80, 1, 1),
('ExcelUpload', 'StaffDailyReportData', 'StaffDailyReportDataInput', 90, 1, 1)
go

CREATE TABLE [dbo].[DivisionSegment]
(
    [Date] NVARCHAR(50) NOT NULL,
	[Division Code] [nvarchar](20) NOT NULL, --*
	[Division Name] [nvarchar](50) NOT NULL, --*
	[Segment Code] [nvarchar](20) NOT NULL, --*
	[Segment Name] [nvarchar](50) NOT NULL, --*
	[Division Prefix] [NVARCHAR](10) NOT NULL
)
go


CREATE TABLE [dbo].[StaffMessageInput]
(
    [Date] NVARCHAR(50) NOT NULL,

	[Staff Code] [nvarchar](10) NOT NULL, --*
	[Staff Name] [nvarchar](50) NOT NULL, --*

	[Message] [nvarchar](100) NOT NULL --*
)
go


CREATE TABLE [dbo].[StaffDailyReportDataInput]
(
    [Date] NVARCHAR(50) NOT NULL,
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

-- Sep 22 2019
CREATE TABLE [dbo].[Division]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[TenantId] BIGINT NOT NULL REFERENCES dbo.Tenant,
	[DivisionCode] [nvarchar](20) NOT NULL, --*
	[SegmentCode] [nvarchar](20) NOT NULL, --*
)
go

CREATE PROCEDURE [dbo].[TransformDivisionSegmentData]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN

	  DELETE FROM dbo.CodeTable where CodeType = 'Division' and TenantId = @tenantId
	  INSERT INTO dbo.CodeTable
	  (CodeType, CodeName, CodeValue, DisplaySequence, TenantId)
	  SELECT Distinct 'Division', [Division Name], [Division Code], 10, @tenantId
	  FROM dbo.DivisionSegment

	  DELETE FROM dbo.CodeTable where CodeType = 'Segment' and TenantId = @tenantId
	  INSERT INTO dbo.CodeTable
	  (CodeType, CodeName, CodeValue, DisplaySequence, TenantId)
	  SELECT Distinct 'Segment', [Segment Name], [Segment Code], 10, @tenantId
	  FROM dbo.DivisionSegment

	  DELETE FROM dbo.CodeTable where CodeType = 'DivisionPrefix' and TenantId = @tenantId
	  INSERT INTO dbo.CodeTable
	  (CodeType, CodeName, CodeValue, DisplaySequence, TenantId)
	  SELECT Distinct 'DivisionPrefix', [Division Name], [Division Prefix], 10, @tenantId
	  FROM dbo.DivisionSegment


	  DELETE FROM dbo.[Division] WHERE tenantId = @tenantId
		INSERT INTO dbo.[Division]
		(TenantId, DivisionCode, SegmentCode)
		SELECT  
		@tenantId,
		rtrim(ltrim([Division Code])),
		rtrim(ltrim([Segment Code]))
		FROM dbo.DivisionSegment

		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformDivisionSegmentData', 'Success'
END
GO

CREATE TABLE [dbo].[StaffMessage]
(
    [ID] BIGINT Primary Key Identity,
	[StaffCode] [nvarchar](10) NOT NULL, --*
	[Message] [nvarchar](100) NOT NULL --*
)
GO

CREATE PROCEDURE [dbo].[TransformStaffMessageData]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
		TRUNCATE TABLE dbo.[StaffMessage]

		-- Step 2: Insert data
		INSERT INTO dbo.[StaffMessage]
		(StaffCode, [Message])
		SELECT  
		rtrim(ltrim([Staff Code])),
		ltrim(rtrim([Message]))
		FROM dbo.StaffMessageInput

		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformStaffMessageData', 'Success'
END
GO

CREATE TABLE [dbo].[StaffDailyReportData]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
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

CREATE PROCEDURE [dbo].[TransformStaffDailyReportData]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
		TRUNCATE TABLE dbo.[StaffDailyReportData]

		INSERT INTO dbo.StaffDailyReportData
		(TenantId, [StaffCode], [DivisionCode], [SegmentCode],
			[TargetOutstandingYTD],
			[TotalCostYTD],
			[CGAYTD],
			[GT180YTD],
			[CollectionTargetYTD],
			[CollectionActualYTD],
			[SalesTargetYTD],
			[SalesActualYTD]
		)
		SELECT @tenantId, [Staff Code],	[Division Code], [ItemSegmentCode],
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

-- 01.10.2019
CREATE TABLE [dbo].[StaffDivisionInput]
(
	[Staff Code] [nvarchar](10) NOT NULL, --*
	[Staff Name] [nvarchar](50) NOT NULL, --*
	[Division Code] [nvarchar](20) NOT NULL, --*
	[Division Name] [nvarchar](50) NOT NULL --*
)
GO

CREATE TABLE [dbo].[StaffDivision]
(
	[ID] BIGINT PRIMARY Key Identity,
	[StaffCode] [nvarchar](10) NOT NULL, --*
	[DivisionCode] [nvarchar](20) NOT NULL
)
GO

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
GO

INSERT into dbo.codeTable
(codeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
('ExcelUpload', 'Staff Division', 'StaffDivisionInput', 100, 1, 1)
go

-- Oct 1 2019


CREATE TABLE [dbo].[CustomerDivisionBalanceInput]
(
    [Date] NVARCHAR(50) NOT NULL,
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

drop table dbo.CustomerDivisionBalance
go

CREATE TABLE [dbo].[CustomerDivisionBalance]
(
	[ID] BIGINT Primary Key Identity,
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
go

CREATE PROCEDURE [dbo].[TransformCustomerDivisionBalance]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
		TRUNCATE TABLE dbo.CustomerDivisionBalance

		-- Step 2: Insert data in CustomerDivisionBalance table
		INSERT INTO [dbo].[CustomerDivisionBalance]
		([CustomerCode], [DivisionCode], [SegmentCode], 
		[CreditLimit], [Outstanding], [LongOutstanding], 
		[Target], [Sales], [Payment])
		SELECT 
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
go

alter table dbo.[ExcelUploadStatus]
alter column [UploadFileName] NVARCHAR(512) NOT NULL
go

alter table dbo.[ExcelUploadHistory]
alter column [UploadFileName] NVARCHAR(512) NOT NULL
go

Insert into dbo.CodeTable
(tenantId, CodeType, CodeName, CodeValue, IsActive, DisplaySequence)
values
(1, 'ExcelUpload', 'Customer', 'CustomerMaster', 1, 10)
go

-- Oct 2 2019
CREATE INDEX [IX_CustomerDivisionBalance_CustomerCode]
	ON [dbo].[CustomerDivisionBalance]
	(CustomerCode)
GO

CREATE PROCEDURE [dbo].[GetCustomerDivisionBalance]
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

	SELECT cdb.CustomerCode,
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
