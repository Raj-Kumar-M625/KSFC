CREATE TABLE [dbo].[EntityWorkFlowDetailItemUsed]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
	[EntityWorkFlowDetailId] BIGINT NOT NULL References dbo.EntityWorkFlowDetail,
	[ItemName] NVARCHAR(50) NOT NULL
)
