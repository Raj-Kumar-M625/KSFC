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