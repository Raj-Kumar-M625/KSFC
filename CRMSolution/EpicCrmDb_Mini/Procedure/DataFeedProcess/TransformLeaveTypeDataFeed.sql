
CREATE PROCEDURE TransformLeaveTypeDataFeed
@tenantId BIGINT

	AS 
	BEGIN
	
			TRUNCATE TABLE dbo.LeaveType
		
		INSERT INTO dbo.LeaveType
		(
		EmployeeCode,
		LeaveType,
		TotalLeaves,
		StartDate,
		EndDate
		)
		SELECT 
			[Employee Code] ,
			[Leave Type]  ,
			[Total Leaves] ,
			[Start Date] ,
			[End Date]  

			FROM dbo.LeaveTypeInput

		INSERT INTO dbo.ErrorLog

		(Process, LogText)

		SELECT 'SP:TransformLeaveTypeDataFeed', 'Success'
END

