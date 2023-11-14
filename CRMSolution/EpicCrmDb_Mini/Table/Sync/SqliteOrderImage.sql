CREATE TABLE [dbo].[SqliteOrderImage]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[SqliteOrderId] BIGINT NOT NULL REFERENCES SqliteOrder,
	[SequenceNumber] INT NOT NULL DEFAULT 0,  -- this value is added by code, but not using it subsequently.
	[ImageFileName] VARCHAR(100) NOT NULL DEFAULT 'FileNotFound.jpg'
)
