CREATE TABLE [dbo].[ActivityContact]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [ActivityId] BIGINT NOT NULL REFERENCES [Activity]([Id]), 
    [Name] NVARCHAR(50) NOT NULL, 
    [PhoneNumber] NVARCHAR(20) NOT NULL, 
    [IsPrimary] BIT NOT NULL
)
