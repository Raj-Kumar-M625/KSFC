CREATE TABLE [dbo].[TenantEmployee]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[TenantId] BIGINT NOT NULL References dbo.Tenant,
	[ManagerId] BIGINT References dbo.TenantEmployee,
	[Name] NVARCHAR(50) NOT NULL,
	[EmployeeCode] NVARCHAR(10) NOT NULL,
	[TimeIntervalInMillisecondsForTracking] BIGINT NOT NULL DEFAULT 5000,
	[SendLogFromPhone] BIT NOT NULL DEFAULT 0,
	[AutoUploadFromPhone] BIT NOT NULL DEFAULT 0,
	[ExecAppAccess] BIT NOT NULL DEFAULT 0,
	[ActivityPageName] NVARCHAR(50), -- if null takes from urlResolver
	[LocationFromType] NVARCHAR(50) NULL, -- Xamarin/GPS/Network/Auto
	[EnhancedDebugEnabled] BIT NOT NULL DEFAULT 0,
	[TestFeatureEnabled] BIT NOT NULL DEFAULT 0,
	[VoiceFeatureEnabled] BIT NOT NULL DEFAULT 0,
	[ExecAppDetailLevel] INT NOT NULL DEFAULT 0,

	[IsActive] Bit NOT NULL Default 1,
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
)
