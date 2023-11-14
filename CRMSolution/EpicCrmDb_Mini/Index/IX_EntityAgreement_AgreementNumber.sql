CREATE UNIQUE INDEX IX_EntityAgreement_AgreementNumber
ON dbo.EntityAgreement (AgreementNumber)
WHERE AgreementNumber <> ''
