CREATE TABLE [dbo].[SqliteSTRImage]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[SqliteSTRId] BIGINT NOT NULL REFERENCES SqliteSTR(Id),
	[SequenceNumber] INT NOT NULL DEFAULT 0,
	[ImageFileName] VARCHAR(100) NOT NULL DEFAULT 'FileNotFound.jpg'
)
