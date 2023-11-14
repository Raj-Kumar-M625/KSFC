CREATE TABLE [dbo].[OrderImage]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[OrderId] BIGINT NOT NULL REFERENCES dbo.[Order],
	[ImageId] BIGINT NOT NULL References [Image],
	[SequenceNumber] INT NOT NULL
)
