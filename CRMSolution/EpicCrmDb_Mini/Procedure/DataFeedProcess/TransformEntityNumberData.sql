CREATE PROCEDURE [dbo].[TransformEntityNumberData]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
	
		DELETE FROM dbo.[EntityNumber]
		WHERE IsUsed = 0

		-- Step 2: Insert data
		INSERT INTO dbo.[EntityNumber]
		([Sequence], EntityNumber)
		SELECT  
		ani.[Sequence],
		ltrim(rtrim(ani.[EntityNumber]))
		FROM dbo.EntityNumberInput ani
		LEFT JOIN dbo.EntityNumber an on ani.EntityNumber = an.EntityNumber
		WHERE an.EntityNumber is null

		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformEntityNumberData', 'Success'
END
