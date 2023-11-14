CREATE TABLE [dbo].[SqliteActionContact]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [SqliteActionId] BIGINT NOT NULL REFERENCES [SqliteAction]([Id]), 
    [Name] NVARCHAR(50) NOT NULL, 
    [PhoneNumber] NVARCHAR(20) NOT NULL, 
    [IsPrimary] BIT NOT NULL
)
