CREATE TABLE [dbo].[Division]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[TenantId] BIGINT NOT NULL REFERENCES dbo.Tenant,
	[DivisionCode] [nvarchar](20) NOT NULL, --*
	[SegmentCode] [nvarchar](20) NOT NULL, --*
)
