CREATE TABLE [dbo].[CustomerDivisionBalance]
(
	[ID] BIGINT Primary Key Identity,
	[DATE] DATE NOT NULL,
	[CustomerCode] [nvarchar](20) NOT NULL,
	[DivisionCode] [nvarchar](20) NOT NULL, --*
	[SegmentCode] [NVARCHAR](20) NOT NULL,

	[CreditLimit] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Outstanding] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[LongOutstanding] DECIMAL(19,2) NOT NULL,

	[Target] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Sales] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Payment] DECIMAL(19,2) NOT NULL DEFAULT 0
)
