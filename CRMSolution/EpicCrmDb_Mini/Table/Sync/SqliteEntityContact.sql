CREATE TABLE [dbo].[SqliteEntityContact]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [SqliteEntityId] BIGINT NOT NULL REFERENCES [SqliteEntity]([Id]), 
    [Name] NVARCHAR(50) NOT NULL, 
    [PhoneNumber] NVARCHAR(20) NOT NULL, 
    [IsPrimary] BIT NOT NULL
)
