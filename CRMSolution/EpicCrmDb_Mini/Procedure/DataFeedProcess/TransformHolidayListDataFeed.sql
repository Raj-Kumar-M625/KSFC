CREATE PROCEDURE TransformHolidayListDataFeed
@tenantId BIGINT
	AS 
	BEGIN
	
		TRUNCATE TABLE dbo.HolidayList
		
		INSERT INTO dbo.HolidayList
		(
		AreaCode,
		[Date],
		[Description],
		StartDate,
		EndDate
		)
		SELECT 
			[Area Code],
			[Date],
			[Description],
			[Start Date] ,
			[End Date]  

			FROM dbo.HolidayListInput

		INSERT INTO dbo.ErrorLog

		(Process, LogText)

		SELECT 'SP:TransformHolidayListDataFeed', 'Success'
END