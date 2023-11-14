CREATE TABLE [dbo].[SqliteAction]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[SqliteTableId] BIGINT NOT NULL, 
	[BatchId] BIGINT NOT NULL References dbo.SqliteActionBatch,
	[PhoneDbId] NVARCHAR(50) NOT NULL DEFAULT '',
    [EmployeeId] BIGINT NOT NULL,
    [At] DATETIME2 NOT NULL,
    [Name] NVARCHAR(20) NOT NULL,
	[ActivityTrackingType] INT NOT NULL DEFAULT 0,
    [Latitude] DECIMAL(19,9) NOT NULL,
    [Longitude] DECIMAL(19,9) NOT NULL,
	
    [MNC] BIGINT NOT NULL,
    [MCC] BIGINT NOT NULL,
    [LAC] BIGINT NOT NULL,
	[CellId] BIGINT NOT NULL,

	[DerivedLocSource] NVARCHAR(50) NULL,
	[DerivedLatitude] DECIMAL(19,9) NOT NULL DEFAULT 0,
    [DerivedLongitude] DECIMAL(19,9) NOT NULL DEFAULT 0,

	[ClientName] NVARCHAR(50) NOT NULL,
	[ClientPhone] NVARCHAR(20) NOT NULL,
	[ClientType] NVARCHAR(50) NOT NULL,
	[ClientCode] NVARCHAR(50) NOT NULL DEFAULT '',
	[ActivityType] NVARCHAR(50) NOT NULL, 
	[Comments] NVARCHAR(2048) NOT NULL,

	[ImageCount] INT NOT NULL DEFAULT 0,
	[ContactCount] INT NOT NULL DEFAULT 0,
	[LocationCount] INT NOT NULL DEFAULT 0,

	[AtBusiness] BIT NOT NULL DEFAULT 0,

	[InstrumentId] NVARCHAR(50) NOT NULL DEFAULT '',
	[ActivityAmount] DECIMAL(19,2) NOT NULL DEFAULT 0.0,

	-- coloumns for server side processing
	[IsProcessed] BIT NOT NULL DEFAULT 0,  -- non-unique indexed
	[IsPostedSuccessfully] BIT NOT NULL DEFAULT 0,
	[TrackingId] BIGINT NOT NULL DEFAULT 0,
	[ActivityId] BIGINT NOT NULL DEFAULT 0,

	-- Columns for Device Info - filled only for start day entry
	[PhoneModel] NVARCHAR(100),
	[PhoneOS] NVARCHAR(10),
	[AppVersion] NVARCHAR(10),
	[IMEI] NVARCHAR(50) NOT NULL DEFAULT '',

	[LocationTaskStatus] NVARCHAR(50) NULL, 
    [LocationException] NVARCHAR(256) NULL, 

	[DateCreated] [DATETIME2] NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] [DateTime2] NOT NULL DEFAULT SYSUTCDATETIME()
)
