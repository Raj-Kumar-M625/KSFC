CREATE TABLE [dbo].[ExcelUploadHistory]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[TenantId] BIGINT NOT NULL REFERENCES dbo.Tenant,
	[UploadType] NVARCHAR(50) NOT NULL,
	[UploadFileName] NVARCHAR(512) NOT NULL,
	[RecordCount] BIGINT NOT NULL,
	[RequestedBy] NVARCHAR(50) NOT NULL,
	[IsCompleteRefresh] BIT NOT NULL DEFAULT 0,
	[RequestTimestamp] DATETIME2 NOT NULL,
	[PostingTimestamp] DATETIME2 NOT NULL
)
