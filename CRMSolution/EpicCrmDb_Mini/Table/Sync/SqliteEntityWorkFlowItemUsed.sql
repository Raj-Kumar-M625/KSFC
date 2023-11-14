CREATE TABLE [dbo].[SqliteEntityWorkFlowItemUsed]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
	[SqliteEntityWorkFlowId] BIGINT NOT NULL References dbo.SqliteEntityWorkFlowV2,
	[ItemName] NVARCHAR(50) NOT NULL
)
