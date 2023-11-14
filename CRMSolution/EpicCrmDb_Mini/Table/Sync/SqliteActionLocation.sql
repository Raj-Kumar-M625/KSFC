CREATE TABLE [dbo].[SqliteActionLocation]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [SqliteActionId] BIGINT NOT NULL REFERENCES [SqliteAction]([Id]), 
	[Source] NVARCHAR(50) NOT NULL,
    [Latitude] DECIMAL(19,9) NOT NULL,
    [Longitude] DECIMAL(19,9) NOT NULL,
	[UtcAt] DATETIME2 NOT NULL,
	[LocationTaskStatus] NVARCHAR(50) NULL, 
    [LocationException] NVARCHAR(256) NULL, 
	[IsGood] BIT NOT NULL DEFAULT 0
)
