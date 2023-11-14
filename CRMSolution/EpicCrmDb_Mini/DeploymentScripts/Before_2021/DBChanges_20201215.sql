-- Dec 15 2020

CREATE TABLE [dbo].[SurveyNumberInput]
(
	[Sequence] BIGINT NOT NULL,
	[SurveyNumber] NVARCHAR(50) NOT NULL
)
GO

CREATE TABLE [dbo].[SurveyNumber]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[Sequence] BIGINT NOT NULL,
	[SurveyNumber] NVARCHAR(50) NOT NULL,
	[IsUsed] BIT NOT NULL DEFAULT 0,
	[UsedTimestamp] DATETIME2 NOT NULL DEFAULT SysUtcDateTime()
)
go

CREATE PROCEDURE [dbo].[TransformSurveyNumberData]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
		DELETE FROM dbo.[SurveyNumber]
		WHERE IsUsed = 0

		-- Step 2: Insert data
		INSERT INTO dbo.[SurveyNumber]
		([Sequence], SurveyNumber)
		SELECT  
		ani.[Sequence],
		ltrim(rtrim(ani.[SurveyNumber]))
		FROM dbo.SurveyNumberInput ani
		LEFT JOIN dbo.SurveyNumber an on ani.SurveyNumber = an.SurveyNumber
		WHERE an.SurveyNumber is null

		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformSurveyNumberData', 'Success'
END
GO

-- Create Survey Request Numbers
DECLARE @n INT = 1
DECLARE @v NVARCHAR(10)
WHILE @n < 50000
BEGIN
   -- Survey Number
   IF NOT EXISTS(SELECT 1 FROM dbo.SurveyNumber WHERE [Sequence] = @n)
   BEGIN
	   SET @v = CONCAT('0', @n)
	   SET @v = CONCAT('SRY', REPLICATE('0', 5-LEN(@v)), @v)

	   INSERT INTO dbo.SurveyNumber
		([Sequence], SurveyNumber)
		VALUES
		(@n, @v)
    END

	SET @n = @n + 1
END
GO

CREATE TABLE [dbo].[EntitySurvey]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[EntityId] BIGINT NOT NULL REFERENCES [Entity]([Id]), 
	[WorkflowSeasonId]  BIGINT NOT NULL REFERENCES [WorkFlowSeason]([Id]), 
	[SurveyNumber] NVARCHAR(50) NOT NULL,

	[MajorCrop] NVARCHAR(50) NOT NULL,
	[LastCrop] NVARCHAR(50) NOT NULL,
	[WaterSource] NVARCHAR(50) NOT NULL,
	[SoilType] NVARCHAR(50) NOT NULL,
	[SowingDate] DATETIME2 NOT NULL,
	[LandSizeInAcres] DECIMAL(19,2) NOT NULL DEFAULT 0,

	[Status] NVARCHAR(50) NOT NULL DEFAULT 'Approved',

	[ActivityId] BIGINT NOT NULL DEFAULT 0,

	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[CreatedBy] NVARCHAR(50) NOT NULL,
	[UpdatedBy] NVARCHAR(50) NOT NULL
)
GO

-- Data Migration from Entity to EntitySurvey

	-- store Entity Rows in in-memory table
	DECLARE @entity TABLE (ID BIGINT Identity, RowId BIGINT)
	INSERT INTO @entity
	(RowId)
	SELECT Id FROM dbo.Entity
	WHERE ISNULL(MajorCrop, '') != ''
	ORDER BY Id

	-- Select Survey Ids
	DECLARE @recCount BIGINT
	SELECT @recCount = count(*)	FROM @entity

	IF @recCount > 0
	BEGIN
		DECLARE @SVYNum TABLE (Id BIGINT Identity, SurveyNumber NVARCHAR(50))
		UPDATE dbo.SurveyNumber
		SET ISUsed = 1,
		UsedTimeStamp = SYSUTCDATETIME()
		OUTPUT deleted.SurveyNumber INTO @SVYNum
		FROM dbo.SurveyNumber an
		INNER JOIN 
		(
			SELECT TOP(@recCount) Id
			FROM dbo.SurveyNumber WITH (READPAST)
			WHERE ISUsed = 0
			ORDER BY [Sequence]
		) an2 on an.Id = an2.Id
	
		INSERT INTO dbo.EntitySurvey
		(
		 EntityId, WorkFlowSeasonId, SurveyNumber,
		 MajorCrop, LastCrop, WaterSource, SoilType, SowingDate, LandSizeInAcres, 
		 CreatedBy, UpdatedBy
		)
		SELECT 
		e.ID, wfs.Id, svy.SurveyNumber,
		MajorCrop, LastCrop, WaterSource, SoilType, SowingDate, CAST(LandSize AS DECIMAL(19,2)),
		'DataMigration', 'DataMigration'
		FROM @entity e2
		INNER JOIN Entity e on e.Id = e2.RowId
		INNER JOIN WorkFlowSeason wfs ON e.SowingType = wfs.TypeName
		AND wfs.IsOpen = 1
		INNER JOIN @SVYNum svy on svy.Id = e2.Id
	END
GO

ALTER TABLE dbo.Entity
DROP COLUMN
	[MajorCrop],
	[LastCrop],
	[WaterSource],
	[SoilType],
	[SowingType],
	[SowingDate] 
GO

ALTER PROCEDURE [dbo].[ProcessSqliteEntityData]
	@batchId BIGINT
AS
BEGIN
	-- if batch is already processed - return	
	IF NOT EXISTS(SELECT 1 FROM dbo.SqliteActionBatch 
			WHERE Id = @batchId AND NumberOfEntitiesSaved > 0
			AND BatchProcessed = 0)
	BEGIN
		RETURN;
	END

	-- First create required records in Day Table
	INSERT INTO dbo.[Day]
	([DATE])
	SELECT DISTINCT CAST(e.[TimeStamp] AS [Date])
	FROM dbo.SqliteEntity e
	LEFT JOIN dbo.[Day] d on CAST(e.[TimeStamp] AS [Date]) = d.[DATE]
	WHERE e.BatchId = @batchId
	AND d.[DATE] IS NULL

	-- Identify duplicate Entity records that have already come in some other batch
	--UPDATE se
	--SET EntityId = e.EntityId,
	--IsProcessed = 1,
	--DateUpdated = SYSUTCDATETIME()
	--FROM dbo.SqliteEntity se
	--INNER JOIN dbo.[SqliteEntity] e on se.[TimeStamp] = e.[TimeStamp]
	--AND e.EmployeeId = se.EmployeeId
	--AND e.PhoneDbId = se.PhoneDbId
	--AND se.BatchId = @batchId

	DECLARE @dupRows BIGINT = @@RowCount
	IF @dupRows > 0
	BEGIN
		UPDATE dbo.SqliteActionBatch
		SET DuplicateEntityCount = @dupRows,
		Timestamp = SYSUTCDATETIME()
		WHERE id = @batchId		
	END

	-- select current max entity Id
	DECLARE @lastMaxEntityId BIGINT
	SELECT @lastMaxEntityId = ISNULL(MAX(Id),0) FROM dbo.Entity

	-- We need to assign EntityNumber to each new Entity
	-- March 04 2020
	-- store new Rows id in in-memory table
	DECLARE @sqliteEnt TABLE (ID BIGINT Identity, RowId BIGINT)
	INSERT INTO @sqliteEnt
	(RowId)
	SELECT Id FROM dbo.SqliteEntity
	WHERE BatchId = @batchId
	AND isProcessed = 0
	ORDER BY Id

	-- Count the number of Entities
	DECLARE @entCount BIGINT
	SELECT @entCount = count(*)	FROM @sqliteEnt

	-- Select entity numbers
	DECLARE @entNum TABLE (Id BIGINT Identity, EntityNumber NVARCHAR(50))

	-- take as many entity numbers from EntityNumber table
	-- (may have to enhance to check that we get enough / required entity numbers)
	UPDATE dbo.EntityNumber
	SET ISUsed = 1,
	UsedTimeStamp = SYSUTCDATETIME()
	OUTPUT deleted.EntityNumber INTO @entNum
	FROM dbo.EntityNumber an
	INNER JOIN 
	(
		SELECT TOP(@entCount) Id
		FROM dbo.EntityNumber WITH (READPAST)
		WHERE ISUsed = 0
		ORDER BY [Sequence]
	) an2 on an.Id = an2.Id

	-- Create Entity Records, with running Entity Number filled
	INSERT INTO dbo.[Entity]
	([EmployeeId], [DayId], [HQCode], [AtBusiness], 
	[EntityType], [EntityName], [EntityDate], 
	[Address], [City], [State], [Pincode], [LandSize], 
	[Latitude], [Longitude],
	[UniqueIdType], [UniqueId], [TaxId],

	[FatherHusbandName], [TerritoryCode], [TerritoryName], [HQName], 
	-- Removed on Dec 15 2020
	--[MajorCrop], [LastCrop], [WaterSource], [SoilType], [SowingType], [SowingDate],

	[SqliteEntityId], [ContactCount], [CropCount], [ImageCount], [EntityNumber])

	SELECT sqe.[EmployeeId], d.[Id] AS [DayId], 
	     case when ltrim(rtrim(ISNULL(sqe.HQCode, ''))) = '' THEN sp.[HQCode] ELSE ltrim(rtrim(sqe.HQCode)) END,
	sqe.[AtBusiness], 
	sqe.[EntityType], sqe.[EntityName], sqe.[TimeStamp] AS [EntityDate], 
	sqe.[Address], sqe.[City], sqe.[State], sqe.[Pincode], sqe.[LandSize], 
	sqe.[DerivedLatitude], sqe.[DerivedLongitude], 
	sqe.[UniqueIdType], sqe.[UniqueId], sqe.[TaxId],

	sqe.[FatherHusbandName], sqe.TerritoryCode, sqe.TerritoryName, sqe.HQName,
	--sqe.MajorCrop, sqe.LastCrop, sqe.WaterSource, sqe.SoilType, sqe.SowingType, sqe.SowingDate,

	sqe.[Id], sqe.[ContactCount], sqe.[CropCount], sqe.[ImageCount], ent.EntityNumber

	FROM dbo.SqliteEntity sqe
	INNER JOIN dbo.[Day] d ON d.[DATE] = CAST(sqe.[TimeStamp] AS [Date])
	INNER JOIN dbo.TenantEmployee te on te.Id = sqe.EmployeeId
	INNER JOIN dbo.SalesPerson sp on te.EmployeeCode = sp.StaffCode

	INNER JOIN @sqliteEnt snt ON sqe.Id = snt.RowId
	INNER JOIN @entNum ent ON ent.Id = snt.ID

	WHERE sqe.BatchId = @batchId AND sqe.IsProcessed = 0
	ORDER BY sqe.Id

	-- Now Images
	UPDATE dbo.[Image] SET SourceId = 0 WHERE SourceId > 0

	INSERT INTO dbo.[Image]
	(SourceId, [ImageFileName])  
	SELECT sei.Id, sei.[ImageFileName]
	FROM dbo.SqliteEntityImage sei
	INNER JOIN dbo.SqliteEntity se on sei.SqliteEntityId = se.Id
	AND se.BatchId = @batchId
	AND se.IsProcessed = 0
		
	-- Now create entries in EntityImage
	INSERT INTO dbo.EntityImage
	(EntityId, ImageId, SequenceNumber)
	SELECT e.Id, i.[Id], sei.SequenceNumber
	FROM dbo.SqliteEntityImage sei
	INNER JOIN dbo.[Image] i on sei.Id = i.SourceId
	INNER JOIN dbo.SqliteEntity sle on sei.SqliteEntityId = sle.Id
	AND sle.BatchId = @batchId
	INNER JOIN dbo.[Entity] e on sle.Id = e.SqliteEntityId

	-- Prepare for SMS
	DECLARE @NewProfile TABLE
	( EntityId BIGINT,
	  EntityName NVARCHAR(50),
	  PhoneNumber NVARCHAR(20)
	)

	-- now we need to update the id in SqliteEntity table
	UPDATE dbo.SqliteEntity
	SET EntityId = e.Id,
	IsProcessed = 1,
	DateUpdated = SYSUTCDATETIME()
	OUTPUT inserted.EntityId, '', '' INTO @NewProfile
	FROM dbo.SqliteEntity se
	INNER JOIN dbo.[Entity] e on se.Id = e.SqliteEntityId
	AND se.BatchId = @batchId
	AND e.Id > @lastMaxEntityId

	--Create Entity Contacts
	INSERT INTO dbo.[EntityContact]
	([EntityId], [Name], [PhoneNumber], [IsPrimary])
	SELECT se.[EntityId], sqecn.[Name], sqecn.[PhoneNumber], sqecn.[IsPrimary]
	FROM dbo.SqliteEntityContact sqecn
	INNER JOIN dbo.SqliteEntity se on se.Id = sqecn.SqliteEntityId
	AND se.BatchId = @batchId
	AND se.EntityId > @lastMaxEntityId

	--Create Entity Crops
	INSERT INTO dbo.[EntityCrop]
	([EntityId], [CropName])
	SELECT se.[EntityId], sqecr.[Name] AS [CropName]
	FROM dbo.SqliteEntityCrop sqecr
	INNER JOIN dbo.SqliteEntity se on se.Id = sqecr.SqliteEntityId
	AND se.BatchId = @batchId
	AND se.EntityId > @lastMaxEntityId

	-- retrieve tenant Id for batch
	DECLARE @tenantId BIGINT
	SELECT @tenantId = TenantId
	FROM dbo.SqliteActionBatch b
	INNER JOIN dbo.TenantEmployee te on b.EmployeeId = te.Id
	WHERE b.Id = @batchId

	-- PUT Name and phone number in new records where sms is to be sent
	UPDATE @NewProfile
	SET EntityName = ec.Name,
	PhoneNumber = ec.PhoneNumber
	FROM @NewProfile np
	INNER JOIN dbo.EntityContact ec on np.EntityId = ec.EntityId
	and ec.IsPrimary = 1

	-- Insert into Table for SMS
	INSERT INTO dbo.TenantSmsData
	(TenantId, TemplateName, DataType, MessageData)
	SELECT @tenantId, 'EntityProfile', 'XML', 
	(SELECT * FROM @NewProfile i WHERE i.EntityId = o.EntityId FOR XML PATH('Row'))
	FROM @NewProfile o
END
GO



declare @schema_name nvarchar(256)
declare @table_name nvarchar(256)
declare @col_name nvarchar(256)
declare @Command  nvarchar(1000)

set @schema_name = N'dbo'
set @table_name = N'SqliteEntity'
set @col_name = N'MajorCrop'

select @Command = 'ALTER TABLE ' + @schema_name + '.[' + @table_name + '] DROP CONSTRAINT ' + d.name
 from sys.tables t
  join sys.default_constraints d on d.parent_object_id = t.object_id
  join sys.columns c on c.object_id = t.object_id and c.column_id = d.parent_column_id
 where t.name = @table_name
  and t.schema_id = schema_id(@schema_name)
  and c.name = @col_name

--print @Command
execute(@Command)
GO

declare @schema_name nvarchar(256)
declare @table_name nvarchar(256)
declare @col_name nvarchar(256)
declare @Command  nvarchar(1000)

set @schema_name = N'dbo'
set @table_name = N'SqliteEntity'
set @col_name = N'LastCrop'

select @Command = 'ALTER TABLE ' + @schema_name + '.[' + @table_name + '] DROP CONSTRAINT ' + d.name
 from sys.tables t
  join sys.default_constraints d on d.parent_object_id = t.object_id
  join sys.columns c on c.object_id = t.object_id and c.column_id = d.parent_column_id
 where t.name = @table_name
  and t.schema_id = schema_id(@schema_name)
  and c.name = @col_name

--print @Command
execute(@Command)
GO


declare @schema_name nvarchar(256)
declare @table_name nvarchar(256)
declare @col_name nvarchar(256)
declare @Command  nvarchar(1000)

set @schema_name = N'dbo'
set @table_name = N'SqliteEntity'
set @col_name = N'WaterSource'

select @Command = 'ALTER TABLE ' + @schema_name + '.[' + @table_name + '] DROP CONSTRAINT ' + d.name
 from sys.tables t
  join sys.default_constraints d on d.parent_object_id = t.object_id
  join sys.columns c on c.object_id = t.object_id and c.column_id = d.parent_column_id
 where t.name = @table_name
  and t.schema_id = schema_id(@schema_name)
  and c.name = @col_name

--print @Command
execute(@Command)
GO

declare @schema_name nvarchar(256)
declare @table_name nvarchar(256)
declare @col_name nvarchar(256)
declare @Command  nvarchar(1000)

set @schema_name = N'dbo'
set @table_name = N'SqliteEntity'
set @col_name = N'SoilType'

select @Command = 'ALTER TABLE ' + @schema_name + '.[' + @table_name + '] DROP CONSTRAINT ' + d.name
 from sys.tables t
  join sys.default_constraints d on d.parent_object_id = t.object_id
  join sys.columns c on c.object_id = t.object_id and c.column_id = d.parent_column_id
 where t.name = @table_name
  and t.schema_id = schema_id(@schema_name)
  and c.name = @col_name

--print @Command
execute(@Command)
GO

declare @schema_name nvarchar(256)
declare @table_name nvarchar(256)
declare @col_name nvarchar(256)
declare @Command  nvarchar(1000)

set @schema_name = N'dbo'
set @table_name = N'SqliteEntity'
set @col_name = N'SowingType'

select @Command = 'ALTER TABLE ' + @schema_name + '.[' + @table_name + '] DROP CONSTRAINT ' + d.name
 from sys.tables t
  join sys.default_constraints d on d.parent_object_id = t.object_id
  join sys.columns c on c.object_id = t.object_id and c.column_id = d.parent_column_id
 where t.name = @table_name
  and t.schema_id = schema_id(@schema_name)
  and c.name = @col_name

--print @Command
execute(@Command)
GO

declare @schema_name nvarchar(256)
declare @table_name nvarchar(256)
declare @col_name nvarchar(256)
declare @Command  nvarchar(1000)

set @schema_name = N'dbo'
set @table_name = N'SqliteEntity'
set @col_name = N'SowingDate'

select @Command = 'ALTER TABLE ' + @schema_name + '.[' + @table_name + '] DROP CONSTRAINT ' + d.name
 from sys.tables t
  join sys.default_constraints d on d.parent_object_id = t.object_id
  join sys.columns c on c.object_id = t.object_id and c.column_id = d.parent_column_id
 where t.name = @table_name
  and t.schema_id = schema_id(@schema_name)
  and c.name = @col_name

--print @Command
execute(@Command)
GO

ALTER TABLE dbo.SqliteEntity
DROP COLUMN
	[MajorCrop],
	[LastCrop],
	[WaterSource],
	[SoilType],
	[SowingType],
	[SowingDate] 
GO


INSERT into dbo.codeTable
(codeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
('ExcelUpload', 'SurveyNumber', 'SurveyNumberInput', 140, 0, 1)
GO

ALTER TABLE [dbo].[SqliteActionBatch]
ADD
	[Surveys] BIGINT NOT NULL DEFAULT 0,
	[SurveysSaved] BIGINT NOT NULL DEFAULT 0
GO

CREATE TABLE [dbo].[SqliteSurvey]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[BatchId] BIGINT NOT NULL REFERENCES dbo.SqliteActionBatch,
	[EmployeeId] BIGINT NOT NULL,

	[IsNewEntity] BIT NOT NULL,
	[EntityId] BIGINT NOT NULL,
	[EntityName] NVARCHAR(50) NOT NULL,

	[SeasonName] NVARCHAR(50) NOT NULL,
	[SowingType] NVARCHAR(50) NOT NULL,
	[Acreage] DECIMAL(19,2) NOT NULL,
	[SowingDate] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

	[MajorCrop] NVARCHAR(50) NOT NULL DEFAULT '',
	[LastCrop] NVARCHAR(50) NOT NULL DEFAULT '',
	[WaterSource] NVARCHAR(50) NOT NULL DEFAULT '',
	[SoilType] NVARCHAR(50) NOT NULL DEFAULT '',
	
	[SurveyDate] DATETIME2 NOT NULL,
	[ActivityId] NVARCHAR(50) NOT NULL DEFAULT '',
	[PhoneDbId] NVARCHAR(50) NOT NULL, 
	[ParentReferenceId]  NVARCHAR(50) NOT NULL DEFAULT '',

	[IsProcessed] BIT NOT NULL DEFAULT 0,  
	[EntitySurveyId] BIGINT NOT NULL DEFAULT 0,
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
)
GO

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
GO


