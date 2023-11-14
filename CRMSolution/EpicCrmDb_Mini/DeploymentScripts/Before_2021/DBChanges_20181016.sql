ALTER TABLE [dbo].[TenantEmployee]
Add	[AutoUploadFromPhone] BIT NOT NULL DEFAULT 0,
    [ActivityPageName] NVARCHAR(50) -- if null takes from urlResolver