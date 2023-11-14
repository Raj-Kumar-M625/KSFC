CREATE TABLE [dbo].[DivisionSegment]
(
    [Date] NVARCHAR(50) NOT NULL,

	[Division Code] [nvarchar](20) NOT NULL, --*
	[Division Name] [nvarchar](50) NOT NULL, --*

	[Segment Code] [nvarchar](20) NOT NULL, --*
	[Segment Name] [nvarchar](50) NOT NULL, --*
	[Division Prefix] [NVARCHAR](10) NOT NULL
)
