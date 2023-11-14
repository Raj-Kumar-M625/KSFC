CREATE TABLE [dbo].[CommonMaster]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
    [CodeType] NVARCHAR(50) NULL,
    [CodeName] NVARCHAR(50) NULL,
    [CodeValue] NVARCHAR(50) NULL,
    [DisplaySequence] INT NULL,
    [IsActive] BIT NOT NULL DEFAULT 1
)
