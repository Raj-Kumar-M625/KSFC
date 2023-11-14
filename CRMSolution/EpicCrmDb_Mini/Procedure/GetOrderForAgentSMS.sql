CREATE PROCEDURE [dbo].[GetOrderForAgentSMS]
	@recordCount int,
	@tenantId BIGINT,
	@employeeId BIGINT
AS
BEGIN
    DECLARE @currentDateTime DATETIME2 = SYSUTCDATETIME();

	;WITH batchCTE(orderId)
	AS
	(
		SELECT TOP(@recordCount) o.Id  -- order Id
		FROM dbo.[Order] o WITH (NOLOCK)
		INNER JOIN dbo.TenantEmployee t ON t.Id = o.EmployeeId
		WHERE t.TenantId = @tenantId
		AND (@employeeId = -1 OR t.Id = @employeeId)
		AND o.LockTimestamp IS NULL
		AND o.IsAgentSMSSent = 0

		ORDER BY o.Id
	)
	-- Lock the selected batch records
	UPDATE dbo.[Order]
	SET LockTimestamp = @currentDateTime
	OUTPUT inserted.Id OrderId
	FROM dbo.[Order] b
	INNER JOIN batchCTE cte ON b.Id = cte.orderId
END


