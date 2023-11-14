CREATE PROCEDURE [dbo].[TransformAgreementNumberData]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
	
		DELETE FROM dbo.[AgreementNumber]
		WHERE IsUsed = 0

		-- Step 2: Insert data
		INSERT INTO dbo.[AgreementNumber]
		([Sequence], AgreementNumber)
		SELECT  
		ani.[Sequence],
		ltrim(rtrim(ani.[AgreementNumber]))
		FROM dbo.AgreementNumberInput ani
		LEFT JOIN dbo.AgreementNumber an on ani.AgreementNumber = an.AgreementNumber
		WHERE an.AgreementNumber is null

		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformAgreementNumberData', 'Success'
END
