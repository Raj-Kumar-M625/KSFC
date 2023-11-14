-- table gets filled automatically in stored procedure, listing unique activity types
CREATE TABLE [dbo].[ActivityType]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[ActivityName] NVARCHAR(50) NOT NULL, 
	[DateCreated] [DATETIME2] NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] [DateTime2] NOT NULL DEFAULT SYSUTCDATETIME()
)
