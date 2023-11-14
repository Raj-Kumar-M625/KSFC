-- 3 July 2019

ALTER TABLE dbo.[Tenant]
ADD	[IsUploadingImage] BIT NOT NULL DEFAULT 0,
	[UploadingImageAt] DATETIME2
go

ALTER TABLE [dbo].[DataFeedProcessLog]
ADD	[ProcessName] NVARCHAR(50) NOT NULL DEFAULT 'DataFeed'
GO
