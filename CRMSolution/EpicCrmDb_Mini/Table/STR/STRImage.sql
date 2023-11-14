CREATE TABLE [dbo].[STRImage]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[STRId] BIGINT NOT NULL References dbo.[STR],
	[SequenceNumber] INT NOT NULL DEFAULT 0,
	[ImageFileName] VARCHAR(100) NOT NULL DEFAULT 'FileNotFound.jpg'
)
