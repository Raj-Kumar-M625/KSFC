CREATE TABLE [dbo].[SqliteBankDetailImage]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[SqliteBankDetailId] BIGINT NOT NULL REFERENCES SqliteBankDetail(Id),
	[SequenceNumber] INT NOT NULL DEFAULT 0,
	[ImageFileName] VARCHAR(100) NOT NULL DEFAULT 'FileNotFound.jpg'
)
