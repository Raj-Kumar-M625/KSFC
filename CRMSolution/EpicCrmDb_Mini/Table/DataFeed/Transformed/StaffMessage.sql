CREATE TABLE [dbo].[StaffMessage]
(
    [ID] BIGINT Primary Key Identity,
	[DATE] DATE NOT NULL,
	[StaffCode] [nvarchar](10) NOT NULL, --*
	[Message] [nvarchar](100) NOT NULL --*
)
GO
