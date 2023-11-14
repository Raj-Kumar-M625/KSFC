CREATE TABLE [dbo].[CustomerDivisionBalanceInput]
(
    [Date] DATE NOT NULL,
	[Customer Code] [nvarchar](20) NOT NULL,
	[Customer Name] [nvarchar](100) NOT NULL,
	[Division Code] [nvarchar](20) NOT NULL, --*
	[Division Name] [nvarchar](50) NOT NULL, --*

	[Credit Limit] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Target Sales YTD] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Total Balance YTD] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Total Overdue] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Contact Phone] [NVARCHAR] (50) NULL DEFAULT '0',
	[Total Sales] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Total Payment YTD] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[ItemSegmentCode] [NVARCHAR](20) NOT NULL,
	[ItemSegmentName] [NVARCHAR](50) NOT NULL
)
go

