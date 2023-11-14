--Insert new column EmployeeId"
Alter table EntityAgreement
Add EmployeeId bigint null
GO

--Rename the existing Stored Procedure  as "ProcessSqlliteAgreementDataOld1"

Exec
  sp_rename  'dbo.ProcessSqliteAgreementData' ,   'ProcessSqliteAgreementDataOld1'
  Go
--Added new SP --

CREATE PROCEDURE [dbo].[ProcessSqliteAgreementData]
	@batchId BIGINT,
	@agreementDefaultStatus NVARCHAR(50)
AS
BEGIN

	-- if batch is already processed - return	
	IF NOT EXISTS(SELECT 1 FROM dbo.SqliteActionBatch 
			WHERE Id = @batchId AND AgreementsSaved > 0
			AND BatchProcessed = 0)
	BEGIN
		RETURN;
	END

	-- First create required records in Day Table
	INSERT INTO dbo.[Day]
	([DATE])
	SELECT DISTINCT CAST(e.[AgreementDate] AS [Date])
	FROM dbo.SqliteAgreement e
	LEFT JOIN dbo.[Day] d on CAST(e.[AgreementDate] AS [Date]) = d.[DATE]
	WHERE e.BatchId = @batchId
	AND d.[DATE] IS NULL

	-- Fill Entity Id in Agreements that belong to new entity created on phone.
	Update dbo.SqliteAgreement
	SET EntityId = en.EntityId,
	EntityName = en.EntityName,
	DateUpdated = SYSUTCDATETIME()
	FROM dbo.SqliteAgreement ag
	INNER JOIN dbo.SqliteEntity en on ag.ParentReferenceId = en.PhoneDbId
	AND ag.BatchId = @batchId
	AND ag.IsNewEntity = 1
	AND ag.IsProcessed = 0

	-- store SqliteAgreement Rows in in-memory table
	DECLARE @sqliteAgg TABLE (ID BIGINT Identity, RowId BIGINT)
	INSERT INTO @sqliteAgg
	(RowId)
	SELECT Id FROM dbo.SqliteAgreement
	WHERE BatchId = @batchId
	AND isProcessed = 0
	ORDER BY Id

	-- Count the number of Agreements
	DECLARE @aggCount BIGINT
	SELECT @aggCount = count(*)	FROM @sqliteAgg

	-- Select Agreement Ids
	DECLARE @aggNum TABLE (Id BIGINT Identity, AgreementNumber NVARCHAR(50))

	UPDATE dbo.AgreementNumber
	SET ISUsed = 1,
	UsedTimeStamp = SYSUTCDATETIME()
	OUTPUT deleted.AgreementNumber INTO @aggNum
	FROM dbo.AgreementNumber an
	INNER JOIN 
	(
		SELECT TOP(@aggCount) Id
		FROM dbo.AgreementNumber WITH (READPAST)
		WHERE ISUsed = 0
		ORDER BY [Sequence]
	) an2 on an.Id = an2.Id

	--Select OfficeHierarchy Details--
	Declare @EntityOffice  TABLE (SAgreementId BIGINT, ZoneCode NVARCHAR(10) , ZoneName nvarchar(50) ,AreaCode nvarchar(10) ,
	AreaName nvarchar(50) ,TerritoryCode nvarchar(10) ,TerritoryName nvarchar(50) ,HQCode nvarchar(10) ,HQName nvarchar(50) )
	Insert into @EntityOffice
	(SAgreementId,ZoneCode,ZoneName,AreaCode,AreaName,TerritoryCode,TerritoryName,HQCode,HQName )  
	SELECT Distinct(sag.Id) As EntityId, oh.ZoneCode, ct.CodeName AS ZoneName,
			oh.AreaCode, ct0.CodeName AS AreaName,
			oh.TerritoryCode, ct1.CodeName AS TerritoryName,
			oh.HQCode, ct2.CodeName AS HQName
		FROM dbo.OfficeHierarchy oh
		Inner JOIN  SqliteAgreement sag on sag.HQCode = oh.HQCode
		INNER JOIN CodeTable ct on ct.CodeType = 'Zone' and ct.CodeValue = oh.ZoneCode
		INNER JOIN CodeTable ct0 on ct0.CodeType = 'AreaOffice' and ct0.CodeValue = oh.AreaCode
		INNER JOIN CodeTable ct1 on ct1.CodeType = 'Territory' and ct1.CodeValue = oh.TerritoryCode
		INNER JOIN CodeTable ct2 on ct2.CodeType = 'HeadQuarter' and ct2.CodeValue = oh.HQCode		
		WHERE oh.IsActive = 1 
		
	
	DECLARE @insertTable TABLE (EntityAgreementId BIGINT, AgreementNumber NVARCHAR(50))

	-- Insert rows in dbo.EntityAgreement
	INSERT into dbo.EntityAgreement
	(EntityId, WorkflowSeasonId, AgreementNumber, LandSizeInAcres, RatePerKg,
	[Status], CreatedBy, UpdatedBy, ActivityId ,ZoneCode,ZoneName,AreaCode,AreaName,TerritoryCode,TerritoryName,
	HQCode,HQName,EmployeeId)
	OUTPUT inserted.Id, inserted.AgreementNumber INTO @insertTable
	SELECT ag.EntityId, wfs.Id, agg.AgreementNumber, ag.Acreage, wfs.RatePerKg,
	@agreementDefaultStatus, 'ProcessSqliteAgreementData', 'ProcessSqliteAgreementData', 
	
	CASE WHEN sqa.ActivityId is NULL THEN 0 ELSE sqa.ActivityId END , eo.ZoneCode,eo.ZoneName,
	eo.AreaCode,eo.AreaName,eo.TerritoryCode,eo.TerritoryName,
	eo.HQCode,eo.HQName,ag.EmployeeId

	FROM dbo.SqliteAgreement ag
	INNER JOIN dbo.WorkflowSeason wfs on wfs.TypeName = ag.TypeName
	and wfs.IsOpen = 1
	INNER JOIN @sqliteAgg sagg ON ag.Id = sagg.RowId
	INNER JOIN @aggNum agg ON agg.Id = sagg.ID

	LEFT JOIN dbo.SqliteAction sqa on sqa.[At] = ag.AgreementDate
	AND sqa.EmployeeId = ag.EmployeeId
	AND sqa.PhoneDbId = ag.ActivityId
	INNER JOIN @EntityOffice eo on ag.Id=eo.SAgreementId 

	-- Now update EntityAgreementId back in SqliteAgreement 
	Update dbo.SqliteAgreement
	SET EntityAgreementId = m3.EntityAgreementId,
	IsProcessed = 1,
	DateUpdated = SYSUTCDATETIME()
	FROM dbo.SqliteAgreement sagg
	INNER JOIN @sqliteAgg m1 ON sagg.Id = m1.RowId
	INNER JOIN @aggNum m2 on m1.Id = m2.Id
	INNER JOIN @insertTable m3 on m3.AgreementNumber = m2.AgreementNumber
END