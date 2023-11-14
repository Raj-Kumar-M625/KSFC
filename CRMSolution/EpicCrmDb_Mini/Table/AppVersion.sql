CREATE TABLE [dbo].[AppVersion]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[Version] NVARCHAR(10) NOT NULL,
	[Comment] NVARCHAR(100),
	[EffectiveDate] [DATE] NOT NULL,
	[ExpiryDate] [DATE] NOT NULL
)
