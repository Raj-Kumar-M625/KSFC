CREATE TABLE [dbo].[StaffDailyReportDataInput]
(
    [Date] DATE NOT NULL,
	[Branch Code] [nvarchar](20) NOT NULL,
	[Branch Name] [nvarchar](100) NOT NULL,
	[Division Code] [nvarchar](20) NOT NULL, --*
	[Division Name] [nvarchar](50) NOT NULL, --*
	[Staff Code] [nvarchar](10) NOT NULL, --*
	[Staff Name] [nvarchar](50) NOT NULL, --*
	[ItemSegmentCode] [NVARCHAR](20) NOT NULL,
	[ItemSegmentName] [NVARCHAR](50) NOT NULL,

	[Target Outstanding YTD] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Total Cost YTD] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[CGA YTD] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[GT 180 YTD] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Collection Target YTD] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Collection Actual YTD] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Sales Target YTD] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Sales Actual YTD] DECIMAL(19,2) NOT NULL DEFAULT 0,
)
go
