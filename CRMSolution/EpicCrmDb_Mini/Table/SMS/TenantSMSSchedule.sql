CREATE TABLE [dbo].[TenantSMSSchedule]
(
	-- This table defines the Daily schedule on which SMS is to be sent
	[Id] INT NOT NULL PRIMARY KEY Identity,
	[TenantId] BIGINT NOT NULL References dbo.Tenant,
	[TenantSmsTypeId] BIGINT NOT NULL References dbo.TenantSmsType,
	[WeekDayName] VARCHAR(10) NOT NULL,
	[SMSAt] Time NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT 1
)
