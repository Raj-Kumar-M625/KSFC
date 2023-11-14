CREATE TABLE [dbo].[SOReportingLevel]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[TenantId] BIGINT NOT NULL REFERENCES dbo.Tenant,
	[StaffCode] [nvarchar](10) NOT NULL, --*
	[ReportingStaffCode] [nvarchar](20) NOT NULL,
	[ReportingLevel] INT NOT NULL
)
