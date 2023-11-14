CREATE TABLE [dbo].[SqliteActionImage]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[SqliteActionId] BIGINT NOT NULL REFERENCES SqliteAction,
	[SequenceNumber] INT NOT NULL DEFAULT 0,  -- this value is added by code, but not using it subsequently.
	[ImageFileName] VARCHAR(100) NOT NULL DEFAULT 'FileNotFound.jpg'
)
