CREATE INDEX IX_SqliteActionLocation_SqliteActionId
ON dbo.SqliteActionLocation 
(SqliteActionId) 
INCLUDE 
(Id, IsGood, Latitude, Longitude)
