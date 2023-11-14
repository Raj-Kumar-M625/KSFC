CREATE TABLE [dbo].[Tracking]
(
    -- this table will have lot of data
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[ChainedTrackingId] BIGINT NULL REFERENCES dbo.Tracking,
	[EmployeeDayId] BIGINT NOT NULL References dbo.EmployeeDay,
	[ActivityId] BIGINT NOT NULL, --References dbo.Activity,
	[At] [DATETIME2] NOT NULL,
	[BeginGPSLatitude] DECIMAL(19,9) NOT NULL,
	[BeginGPSLongitude] DECIMAL(19,9) NOT NULL,
	[EndGPSLatitude] DECIMAL(19,9) NOT NULL,
	[EndGPSLongitude] DECIMAL(19,9) NOT NULL,
	[BeginLocationName] NVARCHAR(128),
	[EndLocationName] NVARCHAR(128),
	[BingMapsDistanceInMeters] Decimal(19,5) Not NULL Default 0,
	[GoogleMapsDistanceInMeters] Decimal(19,5) Not NULL Default 0,
	[LinearDistanceInMeters] Decimal(19,5) Not NULL Default 0,
	[DistanceCalculated] BIT NOT NULL DEFAULT 0,
	[IsMilestone] BIT NOT NULL DEFAULT 0,
	[IsStartOfDay] BIT NOT NULL DEFAULT 0,
	[IsEndOfDay] BIT NOT NULL DEFAULT 0,
	[LockTimestamp] DATETIME2,
	[ActivityType] NVARCHAR(50) NOT NULL DEFAULT ''
)
