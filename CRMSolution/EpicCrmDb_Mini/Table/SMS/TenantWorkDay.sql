CREATE TABLE [dbo].[TenantWorkDay]
(
	-- This table defines the Week days on which SMS is enabled
	[Id] INT NOT NULL PRIMARY KEY Identity,
	[TenantId] BIGINT NOT NULL References dbo.Tenant,
	[WeekDayName] VARCHAR(10) NOT NULL,
	[IsWorkingDay] BIT NOT NULL DEFAULT 1
)
