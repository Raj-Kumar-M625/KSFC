CREATE PROCEDURE [dbo].[GetOfficeHierarchyForSuperAdmin]
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