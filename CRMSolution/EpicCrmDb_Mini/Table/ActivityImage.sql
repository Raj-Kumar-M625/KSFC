CREATE TABLE [dbo].[ActivityImage]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[ActivityId] BIGINT NOT NULL REFERENCES dbo.Activity,
	[ImageId] BIGINT NOT NULL References dbo.[Image],
	[SequenceNumber] INT NOT NULL
)
