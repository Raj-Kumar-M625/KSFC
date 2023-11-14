CREATE TABLE [dbo].[EntityBankDetailImage](
	[Id] [bigint] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[EntityBankDetailId] [bigint] NOT NULL REFERENCES [dbo].[EntityBankDetail](Id),
	[SequenceNumber] INT NOT NULL DEFAULT 0,
	[ImageFileName] VARCHAR(100) NOT NULL DEFAULT 'FileNotFound.jpg'
);

