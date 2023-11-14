CREATE PROCEDURE [dbo].[TransformOfficeHierarchy]
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
