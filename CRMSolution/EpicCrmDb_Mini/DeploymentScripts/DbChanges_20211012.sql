-- Insert FarmerSummaryReport column into Feature Control table
ALTER TABLE [dbo].[FeatureControl]
ADD
	[FarmerSummaryReport] BIT NOT NULL DEFAULT 0
GO
