--Author:Swetha M, Purpose: Dealers Summary Report and Geo Tag Report on: 2022/12/28


ALTER TABLE [dbo].[FeatureControl]
ADD [DealersSummaryReport] BIT NOT NULL DEFAULT 0
GO
ALTER TABLE [dbo].[FeatureControl]
ADD [GeoTaggingReport] BIT NOT NULL DEFAULT 0
GO
