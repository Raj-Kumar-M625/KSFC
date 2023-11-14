CREATE TABLE [dbo].[TenantHoliday]
(
	-- This table defines the Holidays on which SMS is not to be sent
	[Id] INT NOT NULL PRIMARY KEY Identity,
	[TenantId] BIGINT NOT NULL References dbo.Tenant,
	[HolidayDate] DATE NOT NULL,
	[Description] NVARCHAR(50) NULL,
	[IsActive] BIT NOT NULL DEFAULT 1
)

CREATE TABLE [dbo].[TenantSMSLog]
(
	-- This table defines the Daily schedule on which SMS is to be sent
	[Id] INT NOT NULL PRIMARY KEY Identity,
	[TenantId] BIGINT NOT NULL References dbo.Tenant,
	[SMSDateTime] DATETIME2 NOT NULL,
	[SMSText] NVARCHAR(2000) NOT NULL,
	[SMSApiResponse] NVARCHAR(Max)
)

CREATE TABLE [dbo].[TenantSMSSchedule]
(
	-- This table defines the Daily schedule on which SMS is to be sent
	[Id] INT NOT NULL PRIMARY KEY Identity,
	[TenantId] BIGINT NOT NULL References dbo.Tenant,
	[WeekDayName] VARCHAR(10) NOT NULL,
	[SMSAt] Time NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT 1
)

CREATE TABLE [dbo].[TenantWorkDay]
(
	-- This table defines the Week days on which SMS is enabled
	[Id] INT NOT NULL PRIMARY KEY Identity,
	[TenantId] BIGINT NOT NULL References dbo.Tenant,
	[WeekDayName] VARCHAR(10) NOT NULL,
	[IsWorkingDay] BIT NOT NULL DEFAULT 1
)

CREATE PROCEDURE [dbo].[GetInFieldSalesPeople]
		@inputDate DateTime2
AS
BEGIN

	DECLARE @SignedInEmployeeData TABLE
	(
        TrackingRecordId BIGINT,
        TrackingTime DATETIME2,
        EmployeeId BIGINT,
        EmployeeDayId BIGINT,
        EmployeeName NVARCHAR(50),
        Latitude DECIMAL(19,9),
        Longitude DECIMAL(19,9),
        EmployeeCode BIGINT
	)

	-- Get the list of people who are in field today
	INSERT INTO @SignedInEmployeeData
	(TrackingRecordId, TrackingTime, EmployeeId, EmployeeDayId, EmployeeName, Latitude, Longitude, EmployeeCode)
	EXEC [dbo].[GetSignedInEmployeeData] @inputDate

	-- RESULT SET QUERY
	;WITH cteAreaCodes(AreaCode)
	AS
	( 
		SELECT Distinct  AreaCode FROM dbo.OfficeHierarchy 
		WHERE IsActive = 1
	)
	SELECT ISNULL(cte.AreaCode, '***') AreaCode, 
	ISNULL(sq.StaffCode, '') StaffCode, 
	ISNULL(sq.IsInFieldToday, 0) IsInFieldToday,
	ISNULL(sq.IsRegisteredOnPhone,0) IsRegisteredOnPhone
	FROM cteAreaCodes cte
	FULL OUTER JOIN 
			(
				-- Employee Code and Area Codes, IsInFieldToday, IsRegisteredOnPhone
				SELECT sp.StaffCode, ISNULL(oh.AreaCode, '***') AreaCode, 
				CASE WHEN ed.EmployeeId IS NULL THEN 0 ELSE 1 END IsInFieldToday,
				CASE WHEN te.Id IS NULL THEN 0 ELSE 1 END IsRegisteredOnPhone
				FROM dbo.SalesPerson sp
				LEFT join dbo.OfficeHierarchy oh on sp.HQCode = oh.HQCode and oh.IsActive = 1
				LEFT JOIN dbo.TenantEmployee te on te.EmployeeCode = sp.StaffCode and te.IsActive = 1
				LEFT JOIN @SignedInEmployeeData ed ON sp.StaffCode = ed.EmployeeCode
				WHERE sp.IsActive = 1
			) sq ON cte.AreaCode = sq.AreaCode

  -- Cases covered
  -- 1. SalesPerson table may have a HQ code that does not exist in Office hierarchy
  --    We still want such a record in the resultset with Areacode as '***'
END
