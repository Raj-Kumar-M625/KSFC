CREATE TABLE [dbo].[BonusRateInput](
	[SeasonName] [nvarchar](50) NOT NULL,
	[TypeName] [nvarchar](50) NOT NULL,
	[WeightTonsFrom] [decimal](19, 2) NOT NULL,
	[WeightTonsTo] [decimal](19, 2) NOT NULL,
	[RatePaise] [decimal](19, 2) NOT NULL
)
GO