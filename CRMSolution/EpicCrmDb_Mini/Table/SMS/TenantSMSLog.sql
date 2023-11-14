CREATE TABLE [dbo].[TenantSMSLog]
(
	-- This table defines the Daily schedule on which SMS is to be sent
	[Id] INT NOT NULL PRIMARY KEY Identity,
	[TenantId] BIGINT NOT NULL References dbo.Tenant,
	[SmsType] NVARCHAR(50) NOT NULL,
	[SMSDateTime] DATETIME2 NOT NULL,
	[SMSText] NVARCHAR(2000) NOT NULL,
	[SMSApiResponse] NVARCHAR(Max),
	[SenderName] NVARCHAR(50) NOT NULL
)
