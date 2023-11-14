CREATE TABLE [dbo].[Config]
(
	[Id] INT NOT NULL Identity PRIMARY KEY,
	[ConfigName] VARCHAR(100) NOT NULL,
	[ConfigBooleanValue] BIT NOT NULL
)
