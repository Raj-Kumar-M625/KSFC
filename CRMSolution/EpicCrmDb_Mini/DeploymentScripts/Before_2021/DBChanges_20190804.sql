-- August 4 2019
ALTER TABLE dbo.TenantEmployee
ADD	[EnhancedDebugEnabled] BIT NOT NULL DEFAULT 0
GO

ALTER TABLE [dbo].[SqliteActionBatch]
ADD [BatchGuid] NVARCHAR(50) NOT NULL DEFAULT '',  -- this is coming from phone
	[NumberOfImages] BIGINT NOT NULL DEFAULT 0
GO

ALTER TABLE [dbo].[SqliteActionBatch]
ADD [NumberOfImagesSaved] BIGINT NOT NULL DEFAULT 0
GO

CREATE INDEX [IX_SqliteActionBatch_EmployeeId]
	ON [dbo].[SqliteActionBatch]
	(EmployeeId)
	INCLUDE ([BatchGuid])
GO

-- this index was designed already, but somehow did not
-- exist in tracking db
CREATE index IX_SqliteAction_BatchId
ON dbo.SqliteAction (BatchId) INCLUDE (Id)
GO

-- to put in sql
CREATE index IX_SqliteDeviceLog_BatchId
ON dbo.SqliteDeviceLog (BatchId)
GO

-- August 12 2019 - Tracking: Batches not getting processed

CREATE INDEX IX_SqliteActionBatch_BatchProcessed
ON dbo.SqliteActionBatch
(BatchProcessed) INCLUDE (UnderConstruction)

GO

CREATE INDEX IX_SqliteActionLocation_SqliteActionId
on dbo.SqliteActionLocation (SqliteActionId) 
Include (Id, IsGood, Latitude, Longitude)
GO

-- code table changes to support demo to multiple clients.
-- default has to be added here - as there are existing rows, and
-- we are setting a not null constraint
ALTER TABLE [dbo].[CodeTable]
ADD	[TenantId] BIGINT NOT NULL default 1 REFERENCES dbo.Tenant
GO

CREATE INDEX IX_CodeTable_TenantId
ON dbo.CodeTable
(TenantId) 
INCLUDE 
(Id, CodeType, CodeName, CodeValue, DisplaySequence, IsActive)
GO

ALTER TABLE [dbo].[FeatureControl]
	ADD [SecurityContextUser] NVARCHAR(20) NOT NULL DEFAULT ''
GO

-- Index Created on Multiplex environment on Aug 16 2019
-- (Clicking on Km on Activity page, was stalled)
CREATE Index IX_DistanceCalcErrorLog_TrackingId
on dbo.distanceCalcErrorLog (trackingId)
Include (ApiName, Id)
GO

---------------------------------------------
---------------------------------------------

ALTER PROCEDURE [dbo].[RegisterUser]
    @tenantId BIGINT,
	@timeIntervalInMs BIGINT,
	@staffCode NVARCHAR(10),
	@imei NVARCHAR(50),
	@outStatus INT OUTPUT,
	@outEmployeeId BIGINT OUTPUT
AS
BEGIN
	-- RETURN Values of outStatus
	-- -1 User does not exist or blocked in SAP
	-- -2 User access is blocked in CRM
	-- -3 User is registered on another phone
	-- 1 User is already registered and active on current phone
	-- 2 New user registration
	SET @outStatus = 0;
	SET @outEmployeeId = -1;

	-- a. Check in SalesPerson table
	IF NOT EXISTS(SELECT 1 FROM dbo.SalesPerson WHERE CAST(StaffCode AS NVARCHAR(10)) = @staffCode AND IsActive = 1)
	BEGIN
	   SET @outStatus = -1;
	   RETURN
	END

	-- b. check in TenantEmployee table
	DECLARE @isActive BIT
	DECLARE @employeeId BIGINT
	SELECT @isActive = IsActive, @employeeId = Id FROM dbo.TenantEmployee WHERE EmployeeCode = @staffCode

	IF @employeeId IS NOT NULL -- record exist
	BEGIN
		-- retrieve status value
		IF @isActive = 0
		BEGIN
		   -- user access is blocked
		   SET @outStatus = -2;
		   RETURN;
		END
		
		-- check record in IMEI table
		DECLARE @imeiIsActiveStatus BIT
		DECLARE @imeiFromTable NVARCHAR(50)
		SELECT TOP(1) @imeiFromTable = IMEINumber FROM dbo.IMEI WHERE TenantEmployeeId = @employeeId AND IsActive = 1
		IF @imeiFromTable IS NOT NULL
		BEGIN
			IF @imeiFromTable = @imei
			BEGIN
			    -- user is coming back on same phone - may be after a app refresh
				SET @outEmployeeId = @employeeId;
				SET @outStatus = 1;
				RETURN;
			END
			-- user is registered on another phone
			SET @outStatus = -3;
			RETURN;
		END
	END

	-- ALLOW USER SIGNUP
	SELECT @outEmployeeId = Id FROM dbo.TenantEmployee WHERE EmployeeCode = @staffCode
	IF @outEmployeeId IS NULL OR @outEmployeeId <= 0
	BEGIN
		--DECLARE @tenantId BIGINT
		--DECLARE @timeIntervalInMs BIGINT
		--SELECT TOP(1) @tenantId = Id, @timeIntervalInMs = [TimeIntervalInMillisecondsForTracking] FROM dbo.Tenant;

		INSERT INTO dbo.TenantEmployee (TenantId, ManagerId, Name, EmployeeCode, TimeIntervalInMillisecondsForTracking, IsActive)
		SELECT @tenantId, NULL, sp.Name, @staffCode, @timeIntervalInMs, 1
		FROM dbo.SalesPerson sp
		WHERE CAST(sp.StaffCode AS NVARCHAR(10)) = @staffCode

		SET @outEmployeeId = SCOPE_IDENTITY()
	END

	-- before inserting a new record in dbo.IMEI
	-- clear the status of all other records where imei is @imei
	UPDATE dbo.IMEI SET IsActive = 0
	WHERE IMEINumber = @imei

	INSERT INTO dbo.IMEI (IMEINumber, TenantEmployeeId, IsActive)
	VALUES (@imei, @outEmployeeId, 1)

	SET @outStatus = 2;

END
GO

------------------------------

-- for performance
CREATE NONCLUSTERED INDEX [IX_EmployeeDay_DayId] ON [dbo].[EmployeeDay]
(
	[DayId] ASC
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

/*
Check if this index already exist - if does not create it.
DROP INDEX [IX_Tracking_EmployeeDayId] ON [dbo].[Tracking]
GO
CREATE NONCLUSTERED INDEX [IX_Tracking_EmployeeDayId] ON [dbo].[Tracking]
(	[EmployeeDayId] ASC) INCLUDE ([GoogleMapsDistanceInMeters], IsMileStone)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
*/

----------------------------------------
-- August 14 2019
----------------------------------------
ALTER TABLE [dbo].[OfficeHierarchy]
ADD	[TenantId] BIGINT NOT NULL default 1 REFERENCES dbo.Tenant
GO


ALTER PROCEDURE [dbo].[GetOfficeHierarchyForAll]
	@tenantId BIGINT
AS
BEGIN

	DECLARE @OfficeHierarchy TABLE
	(
	    [StaffCode] NVARCHAR(10),
		[ZoneCode] NVARCHAR(10),
		[AreaCode] NVARCHAR(10),
		[TerritoryCode] NVARCHAR(10),
		[HQCode] NVARCHAR(10)
	)

	;WITH spaCTE(StaffCode, CodeType, CodeValue) AS
	(
		SELECT StaffCode, CodeType, CodeValue
		FROM dbo.SalesPersonAssociation
		WHERE IsDeleted = 0
	)
	INSERT INTO @OfficeHierarchy
	(StaffCode, ZoneCode, AreaCode, TerritoryCode, HQCode)

	SELECT spa.StaffCode, oh.ZoneCode, oh.AreaCode, oh.TerritoryCode, oh.HQCode 
	FROM dbo.OfficeHierarchy oh
	INNER JOIN spaCTE spa on spa.CodeType = 'Zone' AND oh.ZoneCode = spa.CodeValue 
	AND oh.IsActive = 1 and oh.TenantId = @tenantId

   UNION

   SELECT spa.StaffCode, oh.ZoneCode, oh.AreaCode, oh.TerritoryCode, oh.HQCode 
   FROM dbo.OfficeHierarchy oh
   INNER JOIN spaCTE spa on spa.CodeType = 'AreaOffice' AND oh.AreaCode = spa.CodeValue
   AND oh.IsActive = 1 and oh.TenantId = @tenantId

   UNION

   SELECT spa.StaffCode, oh.ZoneCode, oh.AreaCode, oh.TerritoryCode, oh.HQCode 
   FROM dbo.OfficeHierarchy oh
   INNER JOIN spaCTE spa on spa.CodeType = 'Territory' AND oh.TerritoryCode = spa.CodeValue
   AND oh.IsActive = 1 and oh.TenantId = @tenantId

   UNION

   SELECT spa.StaffCode, oh.ZoneCode, oh.AreaCode, oh.TerritoryCode, oh.HQCode 
   FROM dbo.OfficeHierarchy oh
   INNER JOIN spaCTE spa on spa.CodeType = 'HeadQuarter' AND oh.HQCode = spa.CodeValue
   AND oh.IsActive = 1 and oh.TenantId = @tenantId


   -- Prepare Return Result
   ;With codeTableCte(CodeType, CodeValue, CodeName) AS
   (
	SELECT CodeType, CodeValue, CodeName
	FROM dbo.CodeTable with (NOLOCK)
	WHERE TenantId = @tenantId
	AND IsActive = 1
   )
	SELECT DISTINCT oh.StaffCode, oh.ZoneCode, ct.CodeName AS ZoneName,
		oh.AreaCode, ct0.CodeName AS AreaName,
		oh.TerritoryCode, ct1.CodeName AS TerritoryName,
		oh.HQCode, ct2.CodeName AS HQName
	FROM @OfficeHierarchy oh
	INNER JOIN codeTableCte ct on ct.CodeType = 'Zone' and ct.CodeValue = oh.ZoneCode
	INNER JOIN codeTableCte ct0 on ct0.CodeType = 'AreaOffice' and ct0.CodeValue = oh.AreaCode
	INNER JOIN codeTableCte ct1 on ct1.CodeType = 'Territory' and ct1.CodeValue = oh.TerritoryCode
	INNER JOIN codeTableCte ct2 on ct2.CodeType = 'HeadQuarter' and ct2.CodeValue = oh.HQCode
END
GO

ALTER PROCEDURE [dbo].[GetOfficeHierarchyForStaff]
    @tenantId BIGINT,
	@staffCode NVARCHAR(10)
AS
BEGIN

	DECLARE @OfficeHierarchy TABLE
	(
		[ZoneCode] NVARCHAR(10),
		[AreaCode] NVARCHAR(10),
		[TerritoryCode] NVARCHAR(10),
		[HQCode] NVARCHAR(10)
	)


	;WITH spaCTE(CodeType, CodeValue) AS
	(
		SELECT CodeType, CodeValue
		FROM dbo.SalesPersonAssociation
		WHERE StaffCode = @staffCode
		AND IsDeleted = 0
	)
	INSERT INTO @OfficeHierarchy
	(ZoneCode, AreaCode, TerritoryCode, HQCode)

	SELECT oh.ZoneCode, oh.AreaCode, oh.TerritoryCode, oh.HQCode 
	FROM dbo.OfficeHierarchy oh
	INNER JOIN spaCTE spa on spa.CodeType = 'Zone' AND oh.ZoneCode = spa.CodeValue 
	AND oh.IsActive = 1 and oh.TenantId = @tenantId

   UNION

   SELECT oh.ZoneCode, oh.AreaCode, oh.TerritoryCode, oh.HQCode 
   FROM dbo.OfficeHierarchy oh
   INNER JOIN spaCTE spa on spa.CodeType = 'AreaOffice' AND oh.AreaCode = spa.CodeValue
   AND oh.IsActive = 1 and oh.TenantId = @tenantId

   UNION

   SELECT oh.ZoneCode, oh.AreaCode, oh.TerritoryCode, oh.HQCode 
   FROM dbo.OfficeHierarchy oh
   INNER JOIN spaCTE spa on spa.CodeType = 'Territory' AND oh.TerritoryCode = spa.CodeValue
   AND oh.IsActive = 1 and oh.TenantId = @tenantId

   UNION

   SELECT oh.ZoneCode, oh.AreaCode, oh.TerritoryCode, oh.HQCode 
   FROM dbo.OfficeHierarchy oh
   INNER JOIN spaCTE spa on spa.CodeType = 'HeadQuarter' AND oh.HQCode = spa.CodeValue
   AND oh.IsActive = 1 and oh.TenantId = @tenantId


   -- Prepare Return Result
   ;With codeTableCte(CodeType, CodeValue, CodeName) AS
   (
	SELECT CodeType, CodeValue, CodeName
	FROM dbo.CodeTable with (NOLOCK)
	WHERE TenantId = @tenantId
	AND IsActive = 1
   )
	SELECT DISTINCT oh.ZoneCode, ct.CodeName AS ZoneName,
		oh.AreaCode, ct0.CodeName AS AreaName,
		oh.TerritoryCode, ct1.CodeName AS TerritoryName,
		oh.HQCode, ct2.CodeName AS HQName
	FROM @OfficeHierarchy oh
	INNER JOIN codeTableCte ct on ct.CodeType = 'Zone' and ct.CodeValue = oh.ZoneCode
	INNER JOIN codeTableCte ct0 on ct0.CodeType = 'AreaOffice' and ct0.CodeValue = oh.AreaCode
	INNER JOIN codeTableCte ct1 on ct1.CodeType = 'Territory' and ct1.CodeValue = oh.TerritoryCode
	INNER JOIN codeTableCte ct2 on ct2.CodeType = 'HeadQuarter' and ct2.CodeValue = oh.HQCode
END
GO

ALTER PROCEDURE [dbo].[GetOfficeHierarchyForSuperAdmin]
   @tenantId BIGINT
AS
BEGIN
   -- Prepare Return Result
      ;With codeTableCte(CodeType, CodeValue, CodeName) AS
   (
	SELECT CodeType, CodeValue, CodeName
	FROM dbo.CodeTable with (NOLOCK)
	WHERE TenantId = @tenantId
	AND IsActive = 1
   )
	SELECT DISTINCT oh.ZoneCode, ct.CodeName AS ZoneName,
		oh.AreaCode, ct0.CodeName AS AreaName,
		oh.TerritoryCode, ct1.CodeName AS TerritoryName,
		oh.HQCode, ct2.CodeName AS HQName
	FROM dbo.OfficeHierarchy oh
	INNER JOIN codeTableCte ct on ct.CodeType = 'Zone' and ct.CodeValue = oh.ZoneCode
	INNER JOIN codeTableCte ct0 on ct0.CodeType = 'AreaOffice' and ct0.CodeValue = oh.AreaCode
	INNER JOIN codeTableCte ct1 on ct1.CodeType = 'Territory' and ct1.CodeValue = oh.TerritoryCode
	INNER JOIN codeTableCte ct2 on ct2.CodeType = 'HeadQuarter' and ct2.CodeValue = oh.HQCode
	WHERE oh.IsActive = 1 and oh.TenantId = @tenantId
END
GO

ALTER PROCEDURE [dbo].[TransformOfficeHierarchy]
  @tenantId BIGINT
AS
BEGIN
		-- Stored procedure to refresh OfficeHierarchy
		-- to local tables

	  DELETE FROM dbo.CodeTable where CodeType = 'Zone' and TenantId = @tenantId
	  INSERT INTO dbo.CodeTable
	  (CodeType, CodeName, CodeValue, DisplaySequence, TenantId)
	  SELECT Distinct 'Zone', [Zone], [Zone Code], 10, @tenantId
	  FROM dbo.ZoneareaTerritory
	  WHERE [Zone] IS NOT NULL AND
			[Zone Code] IS NOT NULL

	  DELETE FROM dbo.CodeTable where CodeType = 'AreaOffice' and TenantId = @tenantId
	  INSERT INTO dbo.CodeTable
	  (CodeType, CodeName, CodeValue, DisplaySequence,TenantId)
	  SELECT Distinct 'AreaOffice', [Area Office], [AO Code], 10,@tenantId
	  FROM dbo.ZoneareaTerritory
	  WHERE [Area Office] IS NOT NULL
	  AND [AO Code] IS NOT NULL

	  DELETE FROM dbo.CodeTable where CodeType = 'Territory' and TenantId = @tenantId
	  INSERT INTO dbo.CodeTable
	  (CodeType, CodeName, CodeValue, DisplaySequence, TenantId)
	  SELECT Distinct 'Territory', [Territory Name], [TR Code], 10, @tenantId
	  FROM dbo.ZoneareaTerritory
	  WHERE [Territory Name] IS NOT NULL
	  AND [TR Code] IS NOT NULL

	  DELETE FROM dbo.CodeTable where CodeType = 'HeadQuarter' and TenantId = @tenantId
	  INSERT INTO dbo.CodeTable
	  (CodeType, CodeName, CodeValue, DisplaySequence, TenantId)
	  SELECT Distinct 'HeadQuarter', [HQ Name], [HQ Code], 10, @tenantId
	  FROM dbo.ZoneareaTerritory
	  WHERE [HQ Name] IS NOT NULL AND
	  [HQ Code] IS NOT NULL

	  DELETE FROM dbo.OfficeHierarchy WHERE tenantId = @tenantId
	  INSERT INTO dbo.OfficeHierarchy
	  (ZoneCode, AreaCode, TerritoryCode, HQCode, TenantId)
	  SELECT [Zone Code], [AO Code], [TR Code], [HQ Code], @tenantId
	  FROM dbo.ZoneareaTerritory
	  WHERE [Zone Code] IS NOT NULL AND 
		  [AO Code] IS NOT NULL AND
		  [TR Code] IS NOT NULL AND
		  [HQ Code] IS NOT NULL


		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformOfficeHierarchy', 'Success'
END
GO

DROP TABLE [dbo].[CustomerMaster]
GO
CREATE TABLE [dbo].[CustomerMaster](
	[Customer Code] [nvarchar](20) NOT NULL,
	[Customer Name] [nvarchar](100) NOT NULL,
	[Type] [nvarchar](20) NOT NULL,
	[District Name] [nvarchar](50) NULL,
	[State Name] [nvarchar](50) NULL,
	[Branch Name] [nvarchar](50) NULL,
	[Pincode] [nvarchar](10) NULL,
	[HQ Code] [nvarchar](10) NULL,

	[Credit Limit] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Expected Business] DECIMAL(19,2) NOT NULL  DEFAULT 0,
	[Total Outstanding] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Total Long Overdue] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Contact Phone] [NVARCHAR](50) NULL DEFAULT '0',
	[Sales] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Collection] DECIMAL(19,2) NOT NULL DEFAULT 0,
) ON [PRIMARY]

DROP TABLE [dbo].[EmployeeMaster]
GO
CREATE TABLE [dbo].[EmployeeMaster](

	[Staff Code] [nvarchar](10) NOT NULL, --*
	[Name] [nvarchar](50) NOT NULL, --*
	[Phone] [NVARCHAR](20) NOT NULL, --*
	[Head Quarter] [nvarchar](50) NOT NULL DEFAULT '', --*
	[Action] [nvarchar](10) NOT NULL, --*
) ON [PRIMARY]
GO

DROP TABLE [dbo].[MaterialMaster]
GO
CREATE TABLE [dbo].[MaterialMaster](
	[Product Code] [nvarchar](50) NOT NULL, -- *
	[Description] [nvarchar](100) NOT NULL, -- *
	[UOM] [nvarchar](10) NOT NULL, -- *
	[Product Group] [nvarchar](50) NOT NULL, --*
	[Brand Name] [nvarchar](50) NOT NULL, --*
	[Shelf Life in months] [BIGINT] NOT NULL, --*
	[Status] [nvarchar](10) NOT NULL,  -- ACtive/Inactive --*
	[Gst Code] [NVARCHAR](20) NULL DEFAULT '' --*
) ON [PRIMARY]

DROP TABLE [dbo].[PriceList]
GO
CREATE TABLE [dbo].[PriceList](
	[Product Code] [nvarchar](50) NOT NULL,  --*
	[MRP] Decimal(19,2) NOT NULL DEFAULT 0, --*
	[Dist Rate] Decimal(19,2) NOT NULL DEFAULT 0, --*
	[PD Rate] Decimal(19,2) NOT NULL DEFAULT 0, --*
	[Dealers Rate] Decimal(19,2) NOT NULL DEFAULT 0, --*
	[AO] [nvarchar](10) NULL, --*
	[Stock] [BIGINT] NOT NULL DEFAULT 0, --*
) ON [PRIMARY]

DROP TABLE [dbo].[ZoneAreaTerritory]
GO
CREATE TABLE [dbo].[ZoneAreaTerritory](
	[Zone] [nvarchar](50) NOT NULL,--*
	[Zone Code] [nvarchar](10) NOT NULL, --*
	[Area Office] [nvarchar](50) NOT NULL,--*
	[AO Code] [nvarchar](10) NOT NULL, --*
	[Territory Name] [nvarchar](50) NOT NULL, --*
	[TR Code] [nvarchar](10) NOT NULL, --*
	[HQ Name] [nvarchar](50) NOT NULL, --*
	[HQ Code] [nvarchar](10) NOT NULL, --*
) 
GO

CREATE PROCEDURE [dbo].[TableSchema]
  @tableName NVARCHAR(100)
AS
BEGIN
		-- Stored procedure to get table schema for Upload routine
	SELECT 
	ISNULL(ORDINAL_POSITION,0) Position, 
	ISNULL(Column_name,'') ColumnName, 
	CASE WHEN Is_Nullable = 'YES' THEN 1 ELSE 0 END IsNullable,
	--IS_NULLABLE IsNullable, 
	ISNULL(Data_Type,'') DataType, 
	IsNull(CHARACTER_MAXIMUM_LENGTH, '') CharMaxLen, 
	IsNull(column_Default, '') ColumnDefault
	FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_Name = @tableName
	ORDER by ORDINAL_POSITION
END
GO

Insert into dbo.CodeTable
(tenantId, CodeType, CodeName, CodeValue, IsActive, DisplaySequence)
values
(1, 'ExcelUpload', 'Customer', 'CustomerMaster', 1, 10),
(1, 'ExcelUpload', 'Employee', 'EmployeeMaster', 1, 20),
(1, 'ExcelUpload', 'Product List', 'MaterialMaster', 1, 30),
(1, 'ExcelUpload', 'Product Price', 'PriceList', 1, 40),
(1, 'ExcelUpload', 'Org. Hierarchy', 'ZoneAreaTerritory', 1, 50)
GO

CREATE TABLE [dbo].[ExcelUploadStatus]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[TenantId] BIGINT NOT NULL REFERENCES dbo.Tenant,
	[UploadType] NVARCHAR(50) NOT NULL,
	[UploadTable] NVARCHAR(50) NOT NULL,
	[UploadFileName] NVARCHAR(128) NOT NULL,
	[RecordCount] BIGINT NOT NULL,
	[RequestedBy] NVARCHAR(50) NOT NULL,
	[IsCompleteRefresh] BIT NOT NULL DEFAULT 0,
	[RequestTimestamp] DATETIME2 NOT NULL,
	[IsPosted] BIT NOT NULL DEFAULT 0,
	[PostingTimestamp] DATETIME2 NOT NULL
)
GO

CREATE TABLE [dbo].[ExcelUploadHistory]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[TenantId] BIGINT NOT NULL REFERENCES dbo.Tenant,
	[UploadType] NVARCHAR(50) NOT NULL,
	[UploadFileName] NVARCHAR(128) NOT NULL,
	[RecordCount] BIGINT NOT NULL,
	[RequestedBy] NVARCHAR(50) NOT NULL,
	[IsCompleteRefresh] BIT NOT NULL DEFAULT 0,
	[RequestTimestamp] DATETIME2 NOT NULL,
	[PostingTimestamp] DATETIME2 NOT NULL
)
GO

CREATE PROCEDURE [dbo].[TransformCustomerDataFeed]
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
		[Outstanding], [LongOutstanding], [District], [State], [Branch], [Pincode], [HQCode], [ContactNumber],
		[Target], [Sales], [Payment])
		SELECT 
		left(cm.[Customer Code], 20),
		left(cm.[Customer Name],100), 
		cm.[Type], 
		ISNULL(cm.[Credit Limit],0),
		ISNULL([Total Outstanding], 0),
		ISNULL([Total Long Overdue], 0),
		left(ISNULL(cm.[District Name], ''),50),
		left(ISNULL(cm.[State Name],''),50),
		left(ISNULL(cm.[Branch Name],''),50),
		left(ISNULL(cm.Pincode,''),10),
		left(ISNULL([HQ Code], ''),10),
		ltrim(Str(IsNULL([Contact Phone], 0), 50, 0)),
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
GO

ALTER PROCEDURE [dbo].[TransformDataFeed]
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
GO

CREATE PROCEDURE [dbo].[TransformProductDataFeed]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
		-- Stored procedure to put data from SAP data feed (tables)
		-- to local tables

		-- Step 1: Not defined.
		-- Input Tables
		-- MaterialMaster
		-- PriceList

		-- Step 2: Wipe out existing data - IsCompleteRefresh is not used here

		DELETE FROM dbo.ProductPrice
		DELETE FROM dbo.Product;
		DELETE FROM dbo.ProductGroup;

		-- Step 3: Insert data in Product Group and Product table
		INSERT INTO dbo.ProductGroup
		(GroupName)
		SELECT DISTINCT ltrim(rtrim([Product Group])) FROM dbo.MaterialMaster
		WHERE [Product Group] IS NOT NULL

		-- Step 4: Insert data in Product table
		;WITH materialCTE(productCode, ProductGroup, Name, UOM, BrandName, ShelfLifeInMonths, IsActive, GstCode, RN )
		AS
		(
			SELECT rtrim(ltrim(mm.[Product Code])),
			ltrim(rtrim([Product Group])),
			left(ISNULL(mm.[Description], ''),100), 
			ISNULL(mm.UOM,''), 
			ISNULL(mm.[Brand Name],''),
			ISNULL(mm.[Shelf Life in months], 0),
			Case when mm.[Status] = 'ACTIVE' THEN 1 ELSE 0 END,
			ISNULL(mm.[Gst Code], ''),
			Row_Number() OVER (Partition By rtrim(ltrim(mm.[Product Code])) ORDER BY rtrim(ltrim(mm.[Product Code])))
			FROM dbo.MaterialMaster mm
			WHERE [Product Group] IS NOT NULL
		)
		INSERT INTO dbo.Product
		(GroupId, ProductCode, Name, UOM, BrandName, ShelfLifeInMonths, IsActive, MRP, GstCode)
		SELECT pg.Id, cte.productCode, cte.Name, cte.UOM, cte.BrandName, cte.ShelfLifeInMonths, CTE.IsActive, 0, cte.GstCode
		 FROM materialCTE cte
		INNER JOIN dbo.ProductGroup pg on cte.ProductGroup = pg.GroupName
		AND cte.RN = 1
	
		-- Step 4: Insert data in ProductPrice table
		INSERT INTO dbo.ProductPrice
		(ProductId, AreaCode, Stock, [MRP], DistPrice, PDISTPrice, DEALERPrice)
		SELECT p.Id, 
		LEFT(ISNULL(pl.AO, ''),10),
		ISNULL(pl.Stock, 0),
		ISNULL(pl.MRP,0),
		ISNULL(pl.[Dist Rate],0), 
		ISNULL(pl.[PD Rate],0),
		ISNULL(pl.[Dealers Rate],0)
		FROM dbo.PriceList pl
		INNER JOIN dbo.Product p ON ltrim(rtrim(pl.[PRODUCT CODE])) = p.ProductCode
		AND pl.[PRODUCT CODE] IS NOT NULL

		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformProductDataFeed', 'Success'
END
GO

CREATE PROCEDURE [dbo].[TransformSalesPersonDataFeed]
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
		([StaffCode], [Name], [Phone], [HQCode], [IsActive])
		SELECT  ltrim([Staff Code]),
		ISNULL(ltrim(rtrim(em.Name)),''),
		ltrim(Str(IsNULL(em.[Phone], 0), 50, 0)),
		ltrim(rtrim(ISNULL([Head Quarter], ''))),
		Case when ltrim(rtrim([Action])) = 'ACTIVE' THEN 1 ELSE 0 END
		FROM dbo.EmployeeMaster em
		LEFT JOIN dbo.SalesPerson sp ON [Staff Code] IS NOT NULL
				AND ltrim(em.[Staff Code]) = sp.StaffCode
		WHERE sp.StaffCode IS NULL


		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformSalesPersonDataFeed', 'Success'
END
GO

ALTER TABLE dbo.Customer
ALTER COLUMN [Name] NVARCHAR(100) NOT NULL
GO

ALTER TABLE [dbo].[SqliteActionBatch]
ADD
	[DataFileName] NVARCHAR(128) NOT NULL DEFAULT '',
	[DataFileSize] BIGINT NOT NULL DEFAULT 0
GO
