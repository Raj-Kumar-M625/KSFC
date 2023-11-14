ALTER TABLE [dbo].[SqliteActionBatch]
ADD [NumberOfEntities] BIGINT NOT NULL DEFAULT 0,
	[NumberOfEntitiesSaved] BIGINT NOT NULL DEFAULT 0,
	[DuplicateEntityCount] BIGINT NOT NULL DEFAULT 0;
GO

CREATE TABLE [dbo].[SqliteEntity]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [BatchId] BIGINT NOT NULL  REFERENCES dbo.SqliteActionBatch, 
    [EmployeeId] BIGINT NOT NULL, 
    [PhoneDbId] NVARCHAR(50) NOT NULL, 
	[ContactCount] INT NOT NULL DEFAULT 0,
	[CropCount] INT NOT NULL DEFAULT 0,
    [AtBusiness] BIT NOT NULL, 
    [EntityType] NVARCHAR(50) NOT NULL, 
    [EntityName] NVARCHAR(50) NOT NULL, 
    [Address] NVARCHAR(100) NULL, 
    [City] NVARCHAR(50) NULL, 
    [State] NVARCHAR(50) NULL, 
    [Pincode] NVARCHAR(10) NULL, 
    [LandSize] NVARCHAR(50) NULL , 
    [TimeStamp] DATETIME2 NOT NULL, 
    [Latitude] DECIMAL(19, 9) NOT NULL, 
    [Longitude] DECIMAL(19, 9) NOT NULL, 
    [LocationTaskStatus] NVARCHAR(50) NULL, 
    [LocationException] NVARCHAR(50) NULL, 
    [MNC] BIGINT NOT NULL, 
    [MCC] BIGINT NOT NULL, 
    [LAC] BIGINT NOT NULL, 
    [CellId] BIGINT NOT NULL, 
    [IsProcessed] BIT NOT NULL DEFAULT 0, 
    [EntityId] BIGINT NOT NULL DEFAULT 0, 
    [DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(), 
    [DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
)
GO

CREATE TABLE [dbo].[SqliteEntityContact]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [SqliteEntityId] BIGINT NOT NULL REFERENCES [SqliteEntity]([Id]), 
    [Name] NVARCHAR(50) NOT NULL, 
    [PhoneNumber] NVARCHAR(20) NOT NULL, 
    [IsPrimary] BIT NOT NULL
);
GO

CREATE TABLE [dbo].[SqliteEntityCrop]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [SqliteEntityId] BIGINT NOT NULL REFERENCES [SqliteEntity]([Id]),
    [Name] NVARCHAR(50) NOT NULL
);
GO

--------------

CREATE TABLE [dbo].[Entity]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[EmployeeId] BIGINT NOT NULL REFERENCES TenantEmployee(Id),
	[DayId] BIGINT NOT NULL REFERENCES dbo.[Day](Id),
	[HQCode] NVARCHAR(10) NULL,

	[ContactCount] INT NOT NULL DEFAULT 0,
	[CropCount] INT NOT NULL DEFAULT 0,

	[AtBusiness] BIT NOT NULL, 
    [EntityType] NVARCHAR(50) NOT NULL, 
    [EntityName] NVARCHAR(50) NOT NULL, 
	[EntityDate] DATETIME2 NOT NULL,
    [Address] NVARCHAR(100) NULL, 
    [City] NVARCHAR(50) NULL, 
    [State] NVARCHAR(50) NULL, 
    [Pincode] NVARCHAR(10) NULL, 
    [LandSize] NVARCHAR(50) NULL , 
    [Latitude] DECIMAL(19, 9) NOT NULL, 
    [Longitude] DECIMAL(19, 9) NOT NULL, 
    [MNC] BIGINT NOT NULL, 
    [MCC] BIGINT NOT NULL, 
    [LAC] BIGINT NOT NULL, 
    [CellId] BIGINT NOT NULL,

    --For dealers who have been added as customers and once approved need not be shown in entity
	[IsApproved] BIT NOT NULL DEFAULT 0,
	[ApproveDate] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[ApproveRef] NVARCHAR(255),
	[ApproveNotes] NVARCHAR(2048),
	[ApprovedBy] NVARCHAR(50) NOT NULL DEFAULT '',

	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[SqliteEntityId] BIGINT NOT NULL
)
GO

CREATE TABLE [dbo].[EntityContact]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[EntityId] BIGINT NOT NULL REFERENCES [Entity]([Id]), 
    [Name] NVARCHAR(50) NOT NULL, 
    [PhoneNumber] NVARCHAR(20) NOT NULL, 
    [IsPrimary] BIT NOT NULL
);
GO

CREATE TABLE [dbo].[EntityCrop]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
    [EntityId] BIGINT NOT NULL REFERENCES [Entity]([Id]),
	[CropName] NVARCHAR(50) NOT NULL
);
GO

-------------

CREATE PROCEDURE [dbo].[ProcessSqliteEntityData]
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

	-- Create Entity Records
	INSERT INTO dbo.[Entity]
	([EmployeeId], [DayId], [HQCode], [AtBusiness], 
	[EntityType], [EntityName], [EntityDate], 
	[Address], [City], [State], [Pincode], [LandSize], 
	[Latitude], [Longitude], [MNC], [MCC], [LAC], [CellId], 
	[SqliteEntityId], [ContactCount], [CropCount])

	SELECT sqe.[EmployeeId], d.[Id] AS [DayId], sp.[HQCode], sqe.[AtBusiness], 
	sqe.[EntityType], sqe.[EntityName], sqe.[TimeStamp] AS [EntityDate], 
	sqe.[Address], sqe.[City], sqe.[State], sqe.[Pincode], sqe.[LandSize], 
	sqe.[Latitude], sqe.[Longitude], sqe.[MNC], sqe.[MCC], sqe.[LAC], sqe.[CellId], 
	sqe.[Id], sqe.[ContactCount], sqe.[CropCount]

	FROM dbo.SqliteEntity sqe
	INNER JOIN dbo.[Day] d ON d.[DATE] = CAST(sqe.[TimeStamp] AS [Date])
	INNER JOIN dbo.TenantEmployee te on te.Id = sqe.EmployeeId
	INNER JOIN dbo.SalesPerson sp on te.EmployeeCode = sp.StaffCode
	WHERE sqe.BatchId = @batchId AND sqe.IsProcessed = 0
	ORDER BY sqe.Id


	-- now we need to update the id in SqliteEntity table
	UPDATE dbo.SqliteEntity
	SET EntityId = e.Id,
	IsProcessed = 1,
	DateUpdated = SYSUTCDATETIME()
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
END
GO
-------------

ALTER TABLE [dbo].[FeatureControl]
ADD [EntityFeature] BIT NOT NULL DEFAULT 0
GO
