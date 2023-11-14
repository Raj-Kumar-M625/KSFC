CREATE TABLE [dbo].[Tenant]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(50) NOT NULL,
	[Description] NVARCHAR(250) NOT NULL,
	[TimeIntervalInMillisecondsForTracking] BIGINT NOT NULL DEFAULT 5000,
	[RatePerDistanceUnit] DECIMAL(9,2) NOT NULL DEFAULT 8,
	[IsProcessingMobileData] BIT NOT NULL DEFAULT 0,
	[MobileDataProcessingAt] DATETIME2,
	[IsTransformingDataFeed] BIT NOT NULL DEFAULT 0,
	[TransformingDataFeedAt] DATETIME2,
	[IsSendingSMS] BIT NOT NULL DEFAULT 0,
	[SMSProcessingAt] DATETIME2,
	[IsUploadingImage] BIT NOT NULL DEFAULT 0,
	[UploadingImageAt] DATETIME2,

	[IsSMSEnabled] BIT NOT NULL DEFAULT 0,
	[MaxDiscountPercentage] DECIMAL(5,2) NOT NULL DEFAULT 7.0,
	[DiscountType] NVARCHAR(50) NOT NULL DEFAULT 'Amount',  -- Amount / Item

	[IsParsingUploadFile] BIT NOT NULL DEFAULT 0,
	[ParsingUploadAt] DATETIME2,

	[IsActive] Bit Not Null Default 1
)
