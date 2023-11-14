CREATE INDEX IX_CodeTable_TenantId
ON dbo.CodeTable
(TenantId) 
INCLUDE 
(Id, CodeType, CodeName, CodeValue, DisplaySequence, IsActive)
