CREATE PROCEDURE [dbo].[GetTenantSmsData]
	@recordCount int,
	@tenantId BIGINT,
	@smsType NVARCHAR(50) -- sms type
AS
BEGIN
    DECLARE @currentDateTime DATETIME2 = SYSUTCDATETIME();

	;WITH batchCTE(tenantSmsId)
	AS
	(
		SELECT TOP(@recordCount) o.Id  
		FROM dbo.[TenantSmsData] o WITH (NOLOCK)
		WHERE o.TenantId = @tenantId
		AND o.TemplateName = @smsType
		AND o.LockTimestamp IS NULL
		AND o.IsSent = 0
		AND o.IsFailed = 0
		ORDER BY o.Id
	)
	-- Lock the selected batch records
	UPDATE dbo.[TenantSmsData]
	SET LockTimestamp = @currentDateTime,
	[Timestamp] = @currentDateTime
	OUTPUT inserted.Id TenantSmsDataId,
	inserted.DataType,
	inserted.MessageData MessageData
	FROM dbo.[TenantSmsData] b
	INNER JOIN batchCTE cte ON b.Id = cte.TenantSmsId
END
GO
