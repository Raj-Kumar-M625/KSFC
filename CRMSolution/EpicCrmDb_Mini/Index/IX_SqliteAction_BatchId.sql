CREATE index IX_SqliteAction_BatchId
ON dbo.SqliteAction (BatchId) INCLUDE (Id)
