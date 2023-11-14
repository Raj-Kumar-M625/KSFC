CREATE TABLE [dbo].[SqlitePaymentImage]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[SqlitePaymentId] BIGINT NOT NULL REFERENCES SqlitePayment,
	[SequenceNumber] INT NOT NULL DEFAULT 0,
	[ImageFileName] VARCHAR(100) NOT NULL DEFAULT 'FileNotFound.jpg'
)
