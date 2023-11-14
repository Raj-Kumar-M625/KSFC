CREATE TABLE [dbo].[TenantSmsType]
(
	-- This table defines SMS Types
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[TenantId] BIGINT NOT NULL References dbo.Tenant,
	[TypeName] NVARCHAR(50) NOT NULL,
	[MessageText] NVARCHAR(1024) NOT NULL,
	[SprocName] NVARCHAR(100) NOT NULL,
	[SmsProcessClass] NVARCHAR(100) NOT NULL DEFAULT '',
	[PlaceHolders] NVARCHAR(1024),
	[IsActive] BIT NOT NULL DEFAULT 1
)
