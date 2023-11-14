DROP INDEX [IX_Tracking_EmployeeDayId] ON dbo.Tracking
go

CREATE INDEX [IX_Tracking_EmployeeDayId]
	ON [dbo].[Tracking]
	(EmployeeDayId)
	INCLUDE ([GoogleMapsDistanceInMeters], IsMilestone, Id, IsStartOfDay, IsEndOfDay, [at], endLocationName)
go

CREATE PROCEDURE [dbo].[GetOfficeHierarchyForAll]
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
	AND oh.IsActive = 1

   UNION

   SELECT spa.StaffCode, oh.ZoneCode, oh.AreaCode, oh.TerritoryCode, oh.HQCode 
   FROM dbo.OfficeHierarchy oh
   INNER JOIN spaCTE spa on spa.CodeType = 'AreaOffice' AND oh.AreaCode = spa.CodeValue
   AND oh.IsActive = 1

   UNION

   SELECT spa.StaffCode, oh.ZoneCode, oh.AreaCode, oh.TerritoryCode, oh.HQCode 
   FROM dbo.OfficeHierarchy oh
   INNER JOIN spaCTE spa on spa.CodeType = 'Territory' AND oh.TerritoryCode = spa.CodeValue
   AND oh.IsActive = 1

   UNION

   SELECT spa.StaffCode, oh.ZoneCode, oh.AreaCode, oh.TerritoryCode, oh.HQCode 
   FROM dbo.OfficeHierarchy oh
   INNER JOIN spaCTE spa on spa.CodeType = 'HeadQuarter' AND oh.HQCode = spa.CodeValue
   AND oh.IsActive = 1


   -- Prepare Return Result
	SELECT DISTINCT oh.StaffCode, oh.ZoneCode, ct.CodeName AS ZoneName,
		oh.AreaCode, ct0.CodeName AS AreaName,
		oh.TerritoryCode, ct1.CodeName AS TerritoryName,
		oh.HQCode, ct2.CodeName AS HQName
	FROM @OfficeHierarchy oh
	INNER JOIN dbo.CodeTable ct on ct.CodeType = 'Zone' and ct.CodeValue = oh.ZoneCode
	INNER JOIN dbo.CodeTable ct0 on ct0.CodeType = 'AreaOffice' and ct0.CodeValue = oh.AreaCode
	INNER JOIN dbo.CodeTable ct1 on ct1.CodeType = 'Territory' and ct1.CodeValue = oh.TerritoryCode
	INNER JOIN dbo.CodeTable ct2 on ct2.CodeType = 'HeadQuarter' and ct2.CodeValue = oh.HQCode
END
