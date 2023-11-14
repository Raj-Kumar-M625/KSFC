CREATE TABLE [dbo].[PriceList](
	[Product Code] [nvarchar](50) NOT NULL,  --*
	[MRP] Decimal(19,2) NOT NULL DEFAULT 0, --*
	[Dist Rate] Decimal(19,2) NOT NULL DEFAULT 0, --*
	[PD Rate] Decimal(19,2) NOT NULL DEFAULT 0, --*
	[Dealers Rate] Decimal(19,2) NOT NULL DEFAULT 0, --*
	[AO] [nvarchar](10) NULL, --*
	[Stock] [BIGINT] NOT NULL DEFAULT 0, --*
) ON [PRIMARY]
