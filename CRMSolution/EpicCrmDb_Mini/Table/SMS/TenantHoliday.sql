CREATE TABLE [dbo].[TenantHoliday]
(
	-- This table defines the Holidays on which SMS is not to be sent
	[Id] INT NOT NULL PRIMARY KEY Identity,
	[TenantId] BIGINT NOT NULL References dbo.Tenant,
	[HolidayDate] DATE NOT NULL,
	[Description] NVARCHAR(50) NULL,
	[IsActive] BIT NOT NULL DEFAULT 1
)
