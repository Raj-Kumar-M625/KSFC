CREATE TABLE [dbo].[MaterialMaster](
	[Product Code] [nvarchar](50) NOT NULL, -- *
	[Description] [nvarchar](100) NOT NULL, -- *
	[UOM] [nvarchar](10) NOT NULL, -- *
	[Product Group] [nvarchar](50) NOT NULL, --*
	[Brand Name] [nvarchar](50) NOT NULL, --*

	[Shelf Life in months] [BIGINT] NOT NULL, --*
	[Status] [nvarchar](10) NOT NULL,  -- ACtive/Inactive --*
	[Gst Code] [NVARCHAR](20) NULL DEFAULT '' --*
) ON [PRIMARY]
