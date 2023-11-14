CREATE PROCEDURE [dbo].[GetInFieldSalesPeople]
		@inputDate DateTime2,
		@tenantId BIGINT
AS
BEGIN

	DECLARE @SignedInEmployeeData TABLE
	(
        EmployeeDayId BIGINT,
		EmployeeId BIGINT,
		StartTime DATETIME2,
		EndTime DATETIME2,
		TotalOrderAmount DECIMAL(19,2),
		TotalPaymentAmount DECIMAL(19,2),
		TotalReturnAmount DECIMAL(19,2),
		TotalExpenseAmount DECIMAL(19,2),
		TotalActivityCount INT,
		Latitude DECIMAL(19,9),
		Longitude DECIMAL(19,9),
		CurrentLocTime DATETIME2,
		PhoneModel NVARCHAR(100),
		PhoneOS NVARCHAR(10),
		AppVersion NVARCHAR(10)
	)

	INSERT INTO @SignedInEmployeeData
	(EmployeeDayId, EmployeeId, StartTime, EndTime, 
	 TotalOrderAmount, TotalPaymentAmount, TotalReturnAmount, TotalExpenseAmount,
	 Latitude, Longitude, PhoneModel, PhoneOS, AppVersion,
	 TotalActivityCount, CurrentLocTime
	)
	SELECT ed.Id, ed.TenantEmployeeId, StartTime, EndTime, 
	TotalOrderAmount, TotalPaymentAmount, 
	TotalReturnAmount, TotalExpenseAmount,
	CurrentLatitude, CurrentLongitude,
	PhoneModel, PhoneOS, AppVersion,
	ed.TotalActivityCount, ed.CurrentLocTime
	FROM dbo.[Day] d
	INNER JOIN dbo.EmployeeDay ed on d.Id = ed.DayId
	AND d.[DATE] = CAST(@inputDate AS [Date])
	
	-- RESULT SET QUERY
	;WITH cteAreaCodes(ZoneCode, AreaCode, TerritoryCode, HQCode)
	AS
	( 
		SELECT Distinct  ZoneCode, AreaCode, TerritoryCode, HQCode
		FROM dbo.OfficeHierarchy 
		WHERE IsActive = 1
		AND TenantId = @tenantId
	)
	SELECT 
	ISNULL(cte.ZoneCode, '***') ZoneCode, 
	ISNULL(cte.AreaCode, '***') AreaCode, 
	ISNULL(cte.TerritoryCode, '***') TerritoryCode,
	ISNULL(cte.HQCode, '***') HQCode, 
	ISNULL(sq.StaffCode, '') StaffCode, 
	ISNULL(sq.IsInFieldToday, 0) IsInFieldToday,
	ISNULL(sq.IsRegisteredOnPhone,0) IsRegisteredOnPhone,
	sq.StartTime,
	sq.EndTime,
	ISNULL(sq.TotalOrderAmount, 0) as TotalOrderAmount,
	ISNULL(sq.TotalPaymentAmount,0) as TotalPaymentAmount,
	ISNULL(sq.TotalReturnAmount,0) as TotalReturnAmount,
	ISNULL(sq.TotalExpenseAmount,0) as TotalExpenseAmount,
	ISNULL(sq.Latitude,0) as Latitude,
	ISNULL(sq.Longitude,0) as Longitude,
	ISNULL(sq.PhoneModel, '') AS PhoneModel,
	ISNULL(sq.PhoneOS, '') AS PhoneOS,
	ISNULL(sq.AppVersion, '') AS AppVersion,
	ISNULL(sq.TotalActivityCount, 0) AS TotalActivityCount,
	ISNULL(sq.CurrentLocTime, '2000-01-01') AS CurrentLocTime

	FROM cteAreaCodes cte
	FULL OUTER JOIN 
			(
				-- Employee Code and Area Codes, IsInFieldToday, IsRegisteredOnPhone
				SELECT sp.StaffCode, 
				ISNULL(oh.ZoneCode, '***') ZoneCode, 
				ISNULL(oh.AreaCode, '***') AreaCode, 
				ISNULL(oh.TerritoryCode, '***') TerritoryCode,
				ISNULL(oh.HQCode, '***') HQCode, 
				CASE WHEN ed.EmployeeId IS NULL THEN 0 ELSE 1 END IsInFieldToday,
				CASE WHEN te.Id IS NULL THEN 0 ELSE 1 END IsRegisteredOnPhone,

				ed.StartTime,
				ed.EndTime,
				ed.TotalOrderAmount,
				ed.TotalPaymentAmount,
				ed.TotalReturnAmount,
				ed.TotalExpenseAmount,
				ed.Latitude,
				ed.Longitude,
				ed.PhoneModel,
				ed.PhoneOS,
				ed.AppVersion,
				ed.TotalActivityCount,
				ed.CurrentLocTime

				FROM dbo.SalesPerson sp
				LEFT JOIN dbo.OfficeHierarchy oh on sp.HQCode = oh.HQCode and oh.IsActive = 1 AND oh.TenantId = @tenantId
				LEFT JOIN dbo.TenantEmployee te on te.EmployeeCode = sp.StaffCode and te.IsActive = 1 AND te.TenantId = @tenantId
				LEFT JOIN @SignedInEmployeeData ed on te.Id = ed.EmployeeId
				WHERE sp.IsActive = 1
			) sq ON 
			cte.ZoneCode = sq.ZoneCode AND
			cte.AreaCode = sq.AreaCode AND
			cte.TerritoryCode = sq.TerritoryCode AND
			cte.HQCode = sq.HQCode


  -- Cases covered
  -- 1. SalesPerson table may have a HQ code that does not exist in Office hierarchy
  --    We still want such a record in the resultset with Areacode as '***'
END
