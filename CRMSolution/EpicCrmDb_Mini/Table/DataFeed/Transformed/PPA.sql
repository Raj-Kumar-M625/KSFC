CREATE TABLE [dbo].[PPA]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[TenantId] BIGINT NOT NULL REFERENCES dbo.Tenant,
	[AreaCode] [nvarchar](10) NOT NULL, 
	[StaffCode] NVARCHAR(10) NOT NULL,
	[PPACode] [Nvarchar](20) NOT NULL, 
	[PPAName] [Nvarchar](50) NOT NULL, 
	[PPAContact] [NVARCHAR](20) NOT NULL, 
	[Location] NVARCHAR(50) NOT NULL
)
