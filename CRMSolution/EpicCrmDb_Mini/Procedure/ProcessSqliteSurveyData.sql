CREATE PROCEDURE [dbo].[ProcessSqliteSurveyData]
	@batchId BIGINT,
	@surveyDefaultStatus NVARCHAR(50)
AS
BEGIN

	-- if batch is already processed - return	
	IF NOT EXISTS(SELECT 1 FROM dbo.SqliteActionBatch 
			WHERE Id = @batchId AND SurveysSaved > 0
			AND BatchProcessed = 0)
	BEGIN
		RETURN;
	END

	-- First create required records in Day Table
	INSERT INTO dbo.[Day]
	([DATE])
	SELECT DISTINCT CAST(e.[SurveyDate] AS [Date])
	FROM dbo.SqliteSurvey e
	LEFT JOIN dbo.[Day] d on CAST(e.[SurveyDate] AS [Date]) = d.[DATE]
	WHERE e.BatchId = @batchId
	AND d.[DATE] IS NULL

	-- Fill Entity Id in Surveys that belong to new entity created on phone.
	Update dbo.SqliteSurvey
	SET EntityId = en.EntityId,
	EntityName = en.EntityName,
	DateUpdated = SYSUTCDATETIME()
	FROM dbo.SqliteSurvey ag
	INNER JOIN dbo.SqliteEntity en on ag.ParentReferenceId = en.PhoneDbId
	AND ag.BatchId = @batchId
	AND ag.IsNewEntity = 1
	AND ag.IsProcessed = 0

	-- store SqliteSurvey Rows in in-memory table
	DECLARE @sqliteAgg TABLE (ID BIGINT Identity, RowId BIGINT)
	INSERT INTO @sqliteAgg
	(RowId)
	SELECT Id FROM dbo.SqliteSurvey
	WHERE BatchId = @batchId
	AND isProcessed = 0
	ORDER BY Id

	-- Count the number of Surveys
	DECLARE @aggCount BIGINT
	SELECT @aggCount = count(*)	FROM @sqliteAgg

	-- Select Survey Ids
	DECLARE @aggNum TABLE (Id BIGINT Identity, SurveyNumber NVARCHAR(50))

	UPDATE dbo.SurveyNumber
	SET ISUsed = 1,
	UsedTimeStamp = SYSUTCDATETIME()
	OUTPUT deleted.SurveyNumber INTO @aggNum
	FROM dbo.SurveyNumber an
	INNER JOIN 
	(
		SELECT TOP(@aggCount) Id
		FROM dbo.SurveyNumber WITH (READPAST)
		WHERE ISUsed = 0
		ORDER BY [Sequence]
	) an2 on an.Id = an2.Id


	DECLARE @insertTable TABLE (EntitySurveyId BIGINT, SurveyNumber NVARCHAR(50))

	-- Insert rows in dbo.EntitySurvey
	INSERT into dbo.EntitySurvey
	(EntityId, WorkflowSeasonId, SurveyNumber, 
	MajorCrop, LastCrop, WaterSource, SoilType, SowingDate,	LandSizeInAcres, 
	[Status], CreatedBy, UpdatedBy, ActivityId)
	OUTPUT inserted.Id, inserted.SurveyNumber INTO @insertTable
	SELECT ag.EntityId, wfs.Id, agg.SurveyNumber, 
	ag.MajorCrop, ag.LastCrop, ag.WaterSource, ag.SoilType, ag.SowingDate, ag.Acreage,
	@surveyDefaultStatus, 'ProcessSqliteSurveyData', 'ProcessSqliteSurveyData', 
	IsNULL(sqa.ActivityId, 0)

	FROM dbo.SqliteSurvey ag
	INNER JOIN dbo.WorkflowSeason wfs on wfs.SeasonName = ag.SeasonName
	AND wfs.TypeName = ag.SowingType
	and wfs.IsOpen = 1
	INNER JOIN @sqliteAgg sagg ON ag.Id = sagg.RowId
	INNER JOIN @aggNum agg ON agg.Id = sagg.ID
	LEFT JOIN dbo.SqliteAction sqa on sqa.[At] = ag.SurveyDate
	AND sqa.EmployeeId = ag.EmployeeId
	AND sqa.PhoneDbId = ag.ActivityId


	-- Now update EntitySurveyId back in SqliteSurvey
	Update dbo.SqliteSurvey
	SET EntitySurveyId = m3.EntitySurveyId,
	IsProcessed = 1,
	DateUpdated = SYSUTCDATETIME()
	FROM dbo.SqliteSurvey sagg
	INNER JOIN @sqliteAgg m1 ON sagg.Id = m1.RowId
	INNER JOIN @aggNum m2 on m1.Id = m2.Id
	INNER JOIN @insertTable m3 on m3.SurveyNumber = m2.SurveyNumber
END
