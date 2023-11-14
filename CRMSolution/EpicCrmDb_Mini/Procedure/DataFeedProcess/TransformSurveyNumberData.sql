CREATE PROCEDURE [dbo].[TransformSurveyNumberData]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
		DELETE FROM dbo.[SurveyNumber]
		WHERE IsUsed = 0

		-- Step 2: Insert data
		INSERT INTO dbo.[SurveyNumber]
		([Sequence], SurveyNumber)
		SELECT  
		ani.[Sequence],
		ltrim(rtrim(ani.[SurveyNumber]))
		FROM dbo.SurveyNumberInput ani
		LEFT JOIN dbo.SurveyNumber an on ani.SurveyNumber = an.SurveyNumber
		WHERE an.SurveyNumber is null

		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformSurveyNumberData', 'Success'
END
GO