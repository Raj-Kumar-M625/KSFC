----Nov 25 2021 (Added a new column in Entity,SqliteEntity Table  for Geolife client)


-- Reverting to Previous version


--ALTER TABLE Entity
--ADD  Consent  BIT NOT NULL  DEFAULT 0
--GO


--ALTER TABLE SqliteEntity
--ADD  Consent  BIT NOT NULL  DEFAULT 0
--GO

--ALTER PROCEDURE [dbo].[ProcessSqliteEntityData]
--	@batchId BIGINT
--AS
--BEGIN

--	-- if batch is already processed - return	
--	IF NOT EXISTS(SELECT 1 FROM dbo.SqliteActionBatch 
--			WHERE Id = @batchId AND NumberOfEntitiesSaved > 0
--			AND BatchProcessed = 0)
--	BEGIN
--		RETURN;
--	END

--	-- First create required records in Day Table
--	INSERT INTO dbo.[Day]
--	([DATE])
--	SELECT DISTINCT CAST(e.[TimeStamp] AS [Date])
--	FROM dbo.SqliteEntity e
--	LEFT JOIN dbo.[Day] d on CAST(e.[TimeStamp] AS [Date]) = d.[DATE]
--	WHERE e.BatchId = @batchId
--	AND d.[DATE] IS NULL

--	-- Identify duplicate Entity records that have already come in some other batch
--	--UPDATE se
--	--SET EntityId = e.EntityId,
--	--IsProcessed = 1,
--	--DateUpdated = SYSUTCDATETIME()
--	--FROM dbo.SqliteEntity se
--	--INNER JOIN dbo.[SqliteEntity] e on se.[TimeStamp] = e.[TimeStamp]
--	--AND e.EmployeeId = se.EmployeeId
--	--AND e.PhoneDbId = se.PhoneDbId
--	--AND se.BatchId = @batchId

--	DECLARE @dupRows BIGINT = @@RowCount
--	IF @dupRows > 0
--	BEGIN
--		UPDATE dbo.SqliteActionBatch
--		SET DuplicateEntityCount = @dupRows,
--		Timestamp = SYSUTCDATETIME()
--		WHERE id = @batchId		
--	END

--	-- select current max entity Id
--	DECLARE @lastMaxEntityId BIGINT
--	SELECT @lastMaxEntityId = ISNULL(MAX(Id),0) FROM dbo.Entity

--	-- We need to assign EntityNumber to each new Entity
--	-- March 04 2020
--	-- store new Rows id in in-memory table
--	DECLARE @sqliteEnt TABLE (ID BIGINT Identity, RowId BIGINT)
--	INSERT INTO @sqliteEnt
--	(RowId)
--	SELECT Id FROM dbo.SqliteEntity
--	WHERE BatchId = @batchId
--	AND isProcessed = 0
--	ORDER BY Id

--	-- Count the number of Entities
--	DECLARE @entCount BIGINT
--	SELECT @entCount = count(*)	FROM @sqliteEnt

--	-- Select entity numbers
--	DECLARE @entNum TABLE (Id BIGINT Identity, EntityNumber NVARCHAR(50))

--	-- take as many entity numbers from EntityNumber table
--	-- (may have to enhance to check that we get enough / required entity numbers)
--	UPDATE dbo.EntityNumber
--	SET ISUsed = 1,
--	UsedTimeStamp = SYSUTCDATETIME()
--	OUTPUT deleted.EntityNumber INTO @entNum
--	FROM dbo.EntityNumber an
--	INNER JOIN 
--	(
--		SELECT TOP(@entCount) Id
--		FROM dbo.EntityNumber WITH (READPAST)
--		WHERE ISUsed = 0
--		ORDER BY [Sequence]
--	) an2 on an.Id = an2.Id

--	-- Create Entity Records, with running Entity Number filled
--	INSERT INTO dbo.[Entity]
--	([EmployeeId], [DayId], [HQCode], [AtBusiness], [Consent],
--	[EntityType], [EntityName], [EntityDate], 
--	[Address], [City], [State], [Pincode], [LandSize], 
--	[Latitude], [Longitude],
--	[UniqueIdType], [UniqueId], [TaxId],

--	[FatherHusbandName], [TerritoryCode], [TerritoryName], [HQName], 
--	-- Removed on Dec 15 2020
--	--[MajorCrop], [LastCrop], [WaterSource], [SoilType], [SowingType], [SowingDate],

--	[SqliteEntityId], [ContactCount], [CropCount], [ImageCount], [EntityNumber])

--	SELECT sqe.[EmployeeId], d.[Id] AS [DayId], 
--	     case when ltrim(rtrim(ISNULL(sqe.HQCode, ''))) = '' THEN sp.[HQCode] ELSE ltrim(rtrim(sqe.HQCode)) END,
--	sqe.[AtBusiness], sqe.[Consent],
--	sqe.[EntityType], sqe.[EntityName], sqe.[TimeStamp] AS [EntityDate], 
--	sqe.[Address], sqe.[City], sqe.[State], sqe.[Pincode], sqe.[LandSize], 
--	sqe.[DerivedLatitude], sqe.[DerivedLongitude], 
--	sqe.[UniqueIdType], sqe.[UniqueId], sqe.[TaxId],

--	sqe.[FatherHusbandName], sqe.TerritoryCode, sqe.TerritoryName, sqe.HQName,
--	--sqe.MajorCrop, sqe.LastCrop, sqe.WaterSource, sqe.SoilType, sqe.SowingType, sqe.SowingDate,

--	sqe.[Id], sqe.[ContactCount], sqe.[CropCount], sqe.[ImageCount], ent.EntityNumber

--	FROM dbo.SqliteEntity sqe
--	INNER JOIN dbo.[Day] d ON d.[DATE] = CAST(sqe.[TimeStamp] AS [Date])
--	INNER JOIN dbo.TenantEmployee te on te.Id = sqe.EmployeeId
--	INNER JOIN dbo.SalesPerson sp on te.EmployeeCode = sp.StaffCode

--	INNER JOIN @sqliteEnt snt ON sqe.Id = snt.RowId
--	INNER JOIN @entNum ent ON ent.Id = snt.ID

--	WHERE sqe.BatchId = @batchId AND sqe.IsProcessed = 0
--	ORDER BY sqe.Id

--	-- Now Images
--	UPDATE dbo.[Image] SET SourceId = 0 WHERE SourceId > 0

--	INSERT INTO dbo.[Image]
--	(SourceId, [ImageFileName])  
--	SELECT sei.Id, sei.[ImageFileName]
--	FROM dbo.SqliteEntityImage sei
--	INNER JOIN dbo.SqliteEntity se on sei.SqliteEntityId = se.Id
--	AND se.BatchId = @batchId
--	AND se.IsProcessed = 0
		
--	-- Now create entries in EntityImage
--	INSERT INTO dbo.EntityImage
--	(EntityId, ImageId, SequenceNumber)
--	SELECT e.Id, i.[Id], sei.SequenceNumber
--	FROM dbo.SqliteEntityImage sei
--	INNER JOIN dbo.[Image] i on sei.Id = i.SourceId
--	INNER JOIN dbo.SqliteEntity sle on sei.SqliteEntityId = sle.Id
--	AND sle.BatchId = @batchId
--	INNER JOIN dbo.[Entity] e on sle.Id = e.SqliteEntityId

--	-- Prepare for SMS
--	DECLARE @NewProfile TABLE
--	( EntityId BIGINT,
--	  EntityName NVARCHAR(50),
--	  PhoneNumber NVARCHAR(20)
--	)

--	-- now we need to update the id in SqliteEntity table
--	UPDATE dbo.SqliteEntity
--	SET EntityId = e.Id,
--	IsProcessed = 1,
--	DateUpdated = SYSUTCDATETIME()
--	OUTPUT inserted.EntityId, '', '' INTO @NewProfile
--	FROM dbo.SqliteEntity se
--	INNER JOIN dbo.[Entity] e on se.Id = e.SqliteEntityId
--	AND se.BatchId = @batchId
--	AND e.Id > @lastMaxEntityId

--	--Create Entity Contacts
--	INSERT INTO dbo.[EntityContact]
--	([EntityId], [Name], [PhoneNumber], [IsPrimary])
--	SELECT se.[EntityId], sqecn.[Name], sqecn.[PhoneNumber], sqecn.[IsPrimary]
--	FROM dbo.SqliteEntityContact sqecn
--	INNER JOIN dbo.SqliteEntity se on se.Id = sqecn.SqliteEntityId
--	AND se.BatchId = @batchId
--	AND se.EntityId > @lastMaxEntityId

--	--Create Entity Crops
--	INSERT INTO dbo.[EntityCrop]
--	([EntityId], [CropName])
--	SELECT se.[EntityId], sqecr.[Name] AS [CropName]
--	FROM dbo.SqliteEntityCrop sqecr
--	INNER JOIN dbo.SqliteEntity se on se.Id = sqecr.SqliteEntityId
--	AND se.BatchId = @batchId
--	AND se.EntityId > @lastMaxEntityId

--	-- retrieve tenant Id for batch
--	DECLARE @tenantId BIGINT
--	SELECT @tenantId = TenantId
--	FROM dbo.SqliteActionBatch b
--	INNER JOIN dbo.TenantEmployee te on b.EmployeeId = te.Id
--	WHERE b.Id = @batchId

--	-- PUT Name and phone number in new records where sms is to be sent
--	UPDATE @NewProfile
--	SET EntityName = ec.Name,
--	PhoneNumber = ec.PhoneNumber
--	FROM @NewProfile np
--	INNER JOIN dbo.EntityContact ec on np.EntityId = ec.EntityId
--	and ec.IsPrimary = 1

--	-- Insert into Table for SMS
--	INSERT INTO dbo.TenantSmsData
--	(TenantId, TemplateName, DataType, MessageData)
--	SELECT @tenantId, 'EntityProfile', 'XML', 
--	(SELECT * FROM @NewProfile i WHERE i.EntityId = o.EntityId FOR XML PATH('Row'))
--	FROM @NewProfile o
--END
