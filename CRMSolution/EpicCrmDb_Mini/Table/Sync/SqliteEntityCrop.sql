CREATE TABLE [dbo].[SqliteEntityCrop]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [SqliteEntityId] BIGINT NOT NULL REFERENCES [SqliteEntity]([Id]),
    [Name] NVARCHAR(50) NOT NULL
)
