CREATE PROCEDURE [dbo].[TransformGRNNumberData]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
	
		DELETE FROM dbo.[GRNNumber]
		WHERE IsUsed = 0

		-- Step 2: Insert data
		INSERT INTO dbo.[GRNNumber]
		([Sequence], GRNNumber)
		SELECT  
		ani.[Sequence],
		ltrim(rtrim(ani.[GRNNumber]))
		FROM dbo.GRNNumberInput ani
		LEFT JOIN dbo.GRNNumber an on ani.GRNNumber = an.GRNNumber
		WHERE an.GRNNumber is null

		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformGRNNumberData', 'Success'
END
GO
