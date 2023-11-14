CREATE PROCEDURE [dbo].[GetSqliteActionBatchForProcessing]
	@recordCount int,
	@tenantId BIGINT,
	@employeeId BIGINT
AS
BEGIN
    DECLARE @currentDateTime DATETIME2 = SYSUTCDATETIME();

	-- update mobileDataProcessingAt as record is picked for processing
	-- this is to create a sliding expiration of 15 minutes, before tenant record
	-- is considered in hanged state;
	UPDATE dbo.Tenant
	SET MobileDataProcessingAt = @currentDateTime
	WHERE Id = @tenantId

	;WITH batchCTE(batchId)
	AS
	(
		-- join tenantEmployee and batch tables
		-- to find all batches for given tenant + employee (?)
		-- Order the batches by employeeId and Time
		-- Select Top count
		-- (only active tenant/employees are selected)
		SELECT TOP(@recordCount) st.Id  -- batchId
		FROM dbo.TenantEmployee te WITH (NOLOCK)
		INNER JOIN dbo.SqliteActionBatch st WITH (READPAST) ON te.Id = st.EmployeeId
		AND te.IsActive = 1
		AND st.UnderConstruction = 0  -- batch should be fully constructed and ready for processing
		INNER JOIN dbo.Tenant t ON t.Id = te.TenantId
		AND t.IsActive = 1
		WHERE te.TenantId = @tenantId
		AND (@employeeId = -1 OR te.Id = @employeeId)
		--AND ((st.BatchProcessed = 0 And st.LockTimestamp IS NULL) OR
		--(st.LockTimeStamp Is NOT NULL AND DATEDIFF(mi, st.LockTimestamp, @currentDateTime) >= 5))

		AND (st.BatchProcessed = 0 And st.LockTimestamp IS NULL)
		--(st.LockTimeStamp Is NOT NULL AND DATEDIFF(mi, st.LockTimestamp, @currentDateTime) >= 5))

		ORDER BY st.EmployeeId, st.[At]
	)
	-- Lock the selected batch records
	UPDATE dbo.SqliteActionBatch
	SET LockTimestamp = @currentDateTime
	OUTPUT inserted.Id BatchId
	FROM dbo.SqliteActionBatch b
	INNER JOIN batchCTE cte ON b.Id = cte.batchId
END


