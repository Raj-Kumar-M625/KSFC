CREATE TABLE [dbo].[Image]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[SourceId] BIGINT NOT NULL,  -- used only during processing
	[ImageFileName] VARCHAR(100) NOT NULL DEFAULT 'FileNotFound.jpg'
)
