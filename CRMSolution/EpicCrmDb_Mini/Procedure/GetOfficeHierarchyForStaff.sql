CREATE PROCEDURE [dbo].[GetOfficeHierarchyForStaff]
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