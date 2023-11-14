-- Scripts to Enable BonusRate upload Data

Insert into CodeTable values
('ExcelUpload','BonusRate','BonusRateInput',170,1,1)
GO
-- Stored Procedure to TransformBonusRateDataFeed

CREATE PROCEDURE [dbo].[TransformBonusRateDataFeed]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
		-- Step 1: Truncate table if opted to do so

		IF @IsCompleteRefresh = 1
		BEGIN
			TRUNCATE TABLE dbo.BonusRate
		END

		-- Insert New data
		INSERT INTO dbo.BonusRate
		(
			[SeasonName],
			[TypeName],
			[WeightTonsFrom],
			[WeightTonsTo],
			[RatePaise]
		)
		SELECT  
			[SeasonName],
			[TypeName],
			[WeightTonsFrom],
			[WeightTonsTo],
			[RatePaise]
		
		FROM dbo.BonusRateInput


		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformBonusRateDataFeed', 'Success'
END
GO

-- Create BonusRateInout Table
CREATE TABLE [dbo].[BonusRateInput](
	[SeasonName] [nvarchar](50) NOT NULL,
	[TypeName] [nvarchar](50) NOT NULL,
	[WeightTonsFrom] [decimal](19, 2) NOT NULL,
	[WeightTonsTo] [decimal](19, 2) NOT NULL,
	[RatePaise] [decimal](19, 2) NOT NULL
)
GO