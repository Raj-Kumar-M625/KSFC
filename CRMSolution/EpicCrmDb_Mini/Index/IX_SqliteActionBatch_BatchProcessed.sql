CREATE INDEX IX_SqliteActionBatch_BatchProcessed
ON dbo.SqliteActionBatch
(BatchProcessed) INCLUDE (UnderConstruction)
