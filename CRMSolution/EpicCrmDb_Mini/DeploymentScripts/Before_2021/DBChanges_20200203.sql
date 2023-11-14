-- Feb 03 2020 - TStanes
ALTER TABLE [dbo].[StaffDailyReportData]
ADD	[AreaCode] [NVARCHAR](20) NOT NULL DEFAULT ''
GO

ALTER PROCEDURE [dbo].[TransformStaffDailyReportData]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
		--DELETE FROM dbo.[StaffDailyReportData] WHERE TenantId = @tenantId
		-- doing a truncate to reset the id to 1, else on daily basis
		-- given the high volume of data, the ids may become too large soon.
		TRUNCATE TABLE dbo.[StaffDailyReportData]

		INSERT INTO dbo.StaffDailyReportData
		([DATE], TenantId, [StaffCode], [DivisionCode], [SegmentCode], [AreaCode],
			[TargetOutstandingYTD],
			[TotalCostYTD],
			[CGAYTD],
			[GT180YTD],
			[CollectionTargetYTD],
			[CollectionActualYTD],
			[SalesTargetYTD],
			[SalesActualYTD]
		)
		SELECT [DATE], @tenantId, [Staff Code],	[Division Code], [ItemSegmentCode], [Branch Code],
			[Target Outstanding YTD],
			[Total Cost YTD],
			[CGA YTD],
			[GT 180 YTD],
			[Collection Target YTD],
			[Collection Actual YTD],
			[Sales Target YTD],
			[Sales Actual YTD]
		FROM dbo.StaffDailyReportDataInput


		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformDailyReportData', 'Success'
END
GO
