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
