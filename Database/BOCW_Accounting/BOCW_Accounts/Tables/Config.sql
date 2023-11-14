CREATE TABLE [dbo].[Config]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
    [DocumentPath] NVARCHAR(200) NULL,
    [VendorFilePrefix] NVARCHAR(50) NULL

)
