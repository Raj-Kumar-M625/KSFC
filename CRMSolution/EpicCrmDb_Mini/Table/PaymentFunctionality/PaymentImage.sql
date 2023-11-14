CREATE TABLE [dbo].[PaymentImage]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[PaymentId] BIGINT NOT NULL REFERENCES dbo.Payment,
	[ImageId] BIGINT NOT NULL References [Image],
	[SequenceNumber] INT NOT NULL
)
