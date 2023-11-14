CREATE TABLE [dbo].[SqliteEntityImage]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[SqliteEntityId] BIGINT NOT NULL REFERENCES SqliteEntity(Id),
	[SequenceNumber] INT NOT NULL DEFAULT 0,
	[ImageFileName] VARCHAR(100) NOT NULL DEFAULT 'FileNotFound.jpg'
)
