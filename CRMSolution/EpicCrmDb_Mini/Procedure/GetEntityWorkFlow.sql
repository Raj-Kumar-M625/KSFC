CREATE PROCEDURE [dbo].[GetEntityWorkFlow]
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


	SELECT wf.EntityId, e.EntityName, wf.Id, 
	wfd.TagName, wfd.Phase, wfd.PlannedStartDate, wfd.PlannedEndDate,
	wf.InitiationDate, wfd.IsComplete, wf.AgreementId, wfd.Id 'EntityWorkFlowDetailId',
	wfd.Notes, wfd.IsFollowUpRow,
	(CASE WHEN wfd.TagName = wf.TagName and wfd.IsFollowUpRow = 0 Then 1 ELSE 0 END) AS IsCurrentPhaseRow,
	wfd.IsActive
	FROM dbo.EntityWorkFlow wf
	INNER JOIN dbo.Entity e on wf.EntityId = e.Id
	INNER JOIN @hqcodes hq on e.HQCode = hq.HQCode
	INNER JOIN dbo.EntityWorkFlowDetail wfd on wf.Id = wfd.EntityWorkFlowId
	-- Send all workflow activities, to display on phone - April 15, 2020
	--AND (wfd.TagName = wf.TagName OR wfd.IsFollowUpRow = 1)

	-- July 29 2020 - Don't consider HQCode from EntityWorkFlow table
	-- as User is allowed to change Entity's HQCode in dbo.EntityTable
	-- in that case, the HQCode in EntityWorkFlow table becomes stale.
END
GO
