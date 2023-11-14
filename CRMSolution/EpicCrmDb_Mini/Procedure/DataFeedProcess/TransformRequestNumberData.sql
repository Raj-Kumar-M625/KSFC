CREATE PROCEDURE [dbo].[TransformRequestNumberData]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
	
		DELETE FROM dbo.[RequestNumber]
		WHERE IsUsed = 0

		-- Step 2: Insert data
		INSERT INTO dbo.[RequestNumber]
		([Sequence], RequestNumber)
		SELECT  
		ani.[Sequence],
		ltrim(rtrim(ani.[RequestNumber]))
		FROM dbo.RequestNumberInput ani
		LEFT JOIN dbo.RequestNumber an on ani.RequestNumber = an.RequestNumber
		WHERE an.RequestNumber is null

		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformRequestNumberData', 'Success'
END
GO
