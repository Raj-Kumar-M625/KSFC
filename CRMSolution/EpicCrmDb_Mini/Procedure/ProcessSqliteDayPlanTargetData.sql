CREATE PROCEDURE [dbo].[ProcessSqliteDayPlanTargetData]
	@batchId BIGINT
AS
BEGIN
	-- Kartik June 04 2021

	-- if batch is already processed - return
	IF NOT EXISTS(SELECT 1 FROM dbo.SqliteActionBatch
			WHERE Id = @batchId AND DayPlanTargetSaved > 0
			AND BatchProcessed = 0)
	BEGIN
		RETURN;
	END

	-- First create required records in Day Table
	INSERT INTO dbo.[Day]
	([DATE])
	SELECT DISTINCT CAST(e.[PlanDate] AS [Date])
	FROM dbo.SqliteDayPlanTarget e
	LEFT JOIN dbo.[Day] d on CAST(e.[PlanDate] AS [Date]) = d.[DATE]
	WHERE e.BatchId = @batchId
	AND d.[DATE] IS NULL


	-- Create DayPlan Target Records
	INSERT INTO dbo.[DayPlanTarget]
	(TenantId, EmployeeId, DayId, EmployeeCode, PlanDate, TargetSales, TargetCollection, TargetVigoreSales,TargetDealerAppointment, SqliteDayPlanTargetId, TargetDemoActivity)
	SELECT te.TenantId, sd.EmployeeId, d.Id, te.EmployeeCode, CAST(sd.PlanDate as [DATE]), sd.TargetSales, sd.TargetCollection, sd.TargetVigoreSales, sd.TargetDealerAppointment, sd.Id, sd.TargetDemoActivity
	FROM dbo.SqliteDayPlanTarget sd
	INNER JOIN dbo.[Day] d ON d.[DATE] = CAST(sd.PlanDate AS [Date])
	INNER JOIN dbo.[TenantEmployee] te ON te.Id = sd.EmployeeId
	WHERE BatchId = @batchId AND sd.IsProcessed = 0
	ORDER BY sd.Id

	-- Now update DayPlanTargetId back in SqliteDayPlanTarget
	Update dbo.SqliteDayPlanTarget
	SET DayPlanTargetId = dpt.Id,
	IsProcessed = 1,
	DateUpdated = SYSUTCDATETIME()
	FROM dbo.SqliteDayPlanTarget sd
	INNER JOIN dbo.[DayPlanTarget] dpt on sd.Id = dpt.SqliteDayPlanTargetId
	AND sd.BatchId = @batchId

END