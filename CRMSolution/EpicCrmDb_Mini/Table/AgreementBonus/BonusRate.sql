CREATE TABLE [dbo].[BonusRate](
	[Id] [bigint] NOT NULL PRIMARY KEY IDENTITY(1,1),
	[SeasonName] [nvarchar](50) NOT NULL,
	[TypeName] [nvarchar](50) NOT NULL,
	[WeightTonsFrom] [decimal](19, 2) NOT NULL,
	[WeightTonsTo] [decimal](19, 2) NOT NULL,
	[RatePaise] [decimal](19, 2) NOT NULL
)
GO