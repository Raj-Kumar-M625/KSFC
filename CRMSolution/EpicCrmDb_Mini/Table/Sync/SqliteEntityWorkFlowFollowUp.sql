CREATE TABLE [dbo].[SqliteEntityWorkFlowFollowUp]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
	[SqliteEntityWorkFlowId] BIGINT NOT NULL References dbo.SqliteEntityWorkFlowV2,
	[Phase] NVARCHAR(50) NOT NULL,
	[StartDate] DATE NOT NULL,
	[EndDate] DATE NOT NULL,
	[Notes] NVARCHAR(100) NOT NULL DEFAULT ''
)
