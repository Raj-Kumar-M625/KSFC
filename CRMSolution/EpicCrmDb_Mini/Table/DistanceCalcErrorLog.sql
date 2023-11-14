CREATE TABLE [dbo].[DistanceCalcErrorLog]
(
	[Id] BIGINT NOT NULL Identity PRIMARY KEY,
	[TrackingId] BIGINT NOT NULL REFERENCES dbo.tracking,
	[APIName] VARCHAR(10) NOT NULL,
	[ErrorText] NVARCHAR(4000) NOT NULL
)
