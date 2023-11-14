CREATE TABLE [dbo].[ExcelUploadError]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[ExcelUploadStatusId] BIGINT NOT NULL REFERENCES dbo.ExcelUploadStatus,

	[MessageType] NVARCHAR(50) NOT NULL,
    [CellReference] NVARCHAR(50) NOT NULL,
    [ExpectedValue] NVARCHAR(50) NOT NULL,
    [ActualValue] NVARCHAR(512) NOT NULL,
    [Description] NVARCHAR(512) NOT NULL
)
