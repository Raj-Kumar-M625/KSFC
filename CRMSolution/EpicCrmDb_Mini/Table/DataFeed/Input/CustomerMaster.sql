CREATE TABLE [dbo].[CustomerMaster](
	[Customer Code] [nvarchar](20) NOT NULL,
	[Customer Name] [nvarchar](100) NOT NULL,
	[Type] [nvarchar](20) NOT NULL,
	--[District Name] [nvarchar](50) NULL,
	--[State Name] [nvarchar](50) NULL,
	--[Branch Name] [nvarchar](50) NULL,
	--[Pincode] [nvarchar](10) NULL,
	[HQ Code] [nvarchar](10) NULL,

	[Credit Limit] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Expected Business] DECIMAL(19,2) NOT NULL  DEFAULT 0,
	[Total Outstanding] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Total Long Overdue] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Contact Phone] [NVARCHAR] (50) NULL DEFAULT '0',
	[Sales] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[Collection] DECIMAL(19,2) NOT NULL DEFAULT 0,
) ON [PRIMARY]
