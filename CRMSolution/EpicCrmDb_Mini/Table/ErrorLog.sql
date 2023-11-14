CREATE TABLE [dbo].[ErrorLog]
(
	[Id] INT NOT NULL PRIMARY KEY Identity,
	[Process] VARCHAR(50),
	[LogText] VARCHAR(Max),
	[LogSnip] VARCHAR(256),
	[At] DATETIME2 DEFAULT SysUtcDateTime()
)
