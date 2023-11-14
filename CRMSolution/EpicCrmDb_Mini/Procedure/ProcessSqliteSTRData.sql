CREATE PROCEDURE [dbo].[ProcessSqliteSTRData]
	@batchId BIGINT
AS
BEGIN

	-- if batch is already processed - return	
	IF NOT EXISTS(SELECT 1 FROM dbo.SqliteActionBatch 
			WHERE Id = @batchId AND STRSavedCount > 0
			AND BatchProcessed = 0)
	BEGIN
		RETURN;
	END

	-- First create required records in Day Table
	INSERT INTO dbo.[Day]
	([DATE])
	SELECT DISTINCT CAST(e.[STRDate] AS [Date])
	FROM dbo.SqliteSTR e
	LEFT JOIN dbo.[Day] d on CAST(e.[STRDate] AS [Date]) = d.[DATE]
	WHERE e.BatchId = @batchId
	AND d.[DATE] IS NULL

	DECLARE @currentTime DATETIME2 = SYSUTCDATETIME()


	-- Create new rows in STRTag
	INSERT INTO dbo.[STRTag]
	(STRNumber, STRDate, CreatedBy, DateCreated, DateUpdated)
	SELECT input.STRNumber, input.STRDate, 'ProcessSqliteSTRData', @currentTime, @currentTime 
	FROM dbo.SqliteSTR input
	LEFT JOIN dbo.[STRTag] tag on input.STRNumber = tag.STRNumber
			  AND input.STRDate = tag.STRDate
    WHERE input.BatchId = @batchId
	AND tag.STRNumber is NULL


	DECLARE @insertedSTR TABLE 
	( 
	  Id BIGINT,   -- STRId
	  STRTagId BIGINT,
	  SqliteSTRId BIGINT,
	  EmployeeId BIGINT
	)

    -- Create Rows in STR table
	INSERT INTO dbo.[STR]
	(STRTagId, EmployeeId, VehicleNumber, DriverName, DriverPhone,
	DWSCount, BagCount, GrossWeight, NetWeight, 
	StartOdometer, EndOdometer, IsNew, IsTransferred, 
	TransfereeName, TransfereePhone,
	ImageCount, ActivityId, ActivityId2, BatchId, CreatedBy, SqliteSTRId,
	DateCreated, DateUpdated
	)
	OUTPUT inserted.Id, inserted.STRTagId, inserted.SqliteSTRId, inserted.EmployeeId INTO @insertedSTR
	SELECT 
	tag.Id, input.EmployeeId, input.VehicleNumber, input.DriverName, input.DriverPhone,
	input.DWSCount, input.BagCount, input.GrossWeight, input.NetWeight,
	input.StartOdometer, input.EndOdometer, input.IsNew, input.IsTransferred, 
	input.TransfereeName, input.TransfereePhone,
	input.ImageCount, sqa.ActivityId, sqa2.ActivityId, input.BatchId, 'ProcessSqliteSTRData', input.Id,
	@currentTime, @currentTime
	FROM dbo.SqliteSTR input
	INNER JOIN dbo.[STRTag] tag on input.STRNumber = tag.STRNumber
	       AND input.STRDate = tag.STRDate
		   AND input.BatchId = @batchId

	INNER JOIN dbo.SqliteAction sqa on sqa.[At] = input.[TimeStamp]
	AND sqa.EmployeeId = input.EmployeeId
	AND sqa.PhoneDbId = input.ActivityId

	INNER JOIN dbo.SqliteAction sqa2 on sqa2.[At] = input.[TimeStamp2]
	AND sqa2.EmployeeId = input.EmployeeId
	AND sqa2.PhoneDbId = input.ActivityId2

	-- Update the values in SqliteSTR table
	UPDATE dbo.SqliteSTR
	SET STRId = t2.Id,
	IsProcessed = 1
	FROM dbo.SqliteSTR t1
	INNER JOIN @insertedSTR t2 on t1.Id = t2.SqliteSTRId

	-- Now Images
	INSERT into dbo.STRImage
	(STRId, SequenceNumber, ImageFileName)
	SELECT 
	 t2.Id, input.SequenceNumber, input.ImageFileName
	 FROM dbo.SqliteSTRImage input
	 INNER JOIN @insertedSTR t2 ON t2.SqliteSTRId = input.SqliteSTRId
	-- only for the rows for which entry is made in STR table.


	DECLARE @insertedDWS TABLE 
	( 
	  Id BIGINT,   -- DWSId
	  [SqliteSTRDWSId] BIGINT
	)

	-- Create Rows in DWS Table
	INSERT INTO dbo.DWS
	(STRTagId, STRId, DWSNumber, DWSDate, BagCount, 
	[FilledBagsWeightKg], [EmptyBagsWeightKg], EntityId, 
	AgreementId, Agreement, [EntityWorkFlowDetailId], TypeName, TagName, 
	ActivityId, [SqliteSTRDWSId], CreatedBy, DateCreated, DateUpdated,
	[OrigBagCount], [OrigFilledBagsKg], [OrigEmptyBagsKg]
	)
	OUTPUT inserted.Id, inserted.SqliteSTRDWSId into @insertedDWS
	SELECT t2.STRTagId, t2.Id, input.DWSNumber, input.DWSDate, input.BagCount,
	input.[FilledBagsWeightKg], input.[EmptyBagsWeightKg], input.EntityId,
	input.AgreementId, input.Agreement, input.[EntityWorkFlowDetailId], input.TypeName, input.TagName, 
	sqa.ActivityId, input.Id, 'ProcessSqliteSTRData', @currentTime, @currentTime,
	input.BagCount, input.[FilledBagsWeightKg], input.[EmptyBagsWeightKg]

	FROM dbo.SqliteSTRDWS input

	INNER JOIN @insertedSTR t2 on t2.SqliteSTRId = input.SqliteSTRId
	-- only for the rows for which entry is made in STR table.

	INNER JOIN dbo.SqliteAction sqa on sqa.[At] = input.[DWSDate]
	AND sqa.EmployeeId = t2.EmployeeId
	AND sqa.PhoneDbId = input.ActivityId

	---- these two joins are to get HQCode
	--INNER JOIN dbo.EntityWorkFlowDetail wfd ON wfd.Id = input.EntityWorkFlowDetailId
	--INNER JOIN dbo.EntityWorkFlow wf on wf.Id = wfd.EntityWorkFlowId

	-- Update id in SqliteSTRDWS table
	UPDATE dbo.SqliteSTRDWS
	SET DWSId = t2.Id,
	IsProcessed = 1
	FROM dbo.SqliteSTRDWS t1
	INNER JOIN @insertedDWS t2 on t1.Id = t2.[SqliteSTRDWSId]
END
GO
