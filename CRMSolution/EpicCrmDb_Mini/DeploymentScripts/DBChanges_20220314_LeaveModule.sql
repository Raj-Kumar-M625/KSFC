--Added Two new code values into ExcelUpload CodeType--
INSERT into dbo.codeTable
(CodeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
('ExcelUpload', 'Leave Type', 'LeaveTypeInput', 40, 1, 1),
('ExcelUpload','Holiday List', 'HolidayListInput',50,1,1);
GO
--Created new LeaveType Input Table in WebApp main database--
CREATE TABLE LeaveTypeInput (
[Employee Code]  nvarchar(10) Not Null,
[Leave Type] nvarchar(20) Not Null ,
[Total Leaves] int Not Null,
[Start Date] datetime2(7) Not Null,
[End Date]   datetime2(7) Not Null,
);
GO
--Created new LeaveType table in WebApp main database--
CREATE TABLE LeaveType (
Id bigint Not Null Identity,
EmployeeCode nvarchar(10) Not Null,
LeaveType nvarchar(20) Not Null,
TotalLeaves int Not Null,
StartDate  datetime2(7) Not Null,
EndDate  datetime2(7) Not Null,
);
GO
-- Added Stored procedure to transform the uploaded leave Type data to download in mobile app--
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
--Added New Code Value into Leave Type Code Type:
INSERT into dbo.codeTable
(CodeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
('LeaveType', 'Sick Leave', 'Sick Leave', 40, 1, 1);
GO
--Added  new HolidayList Input Table in WebApp main database--
CREATE TABLE HolidayListInput (
[Area Code]  nvarchar(10) Not Null,
[Date] datetime2(7) Not Null ,
[Description] nvarchar(30) Not Null,
[Start Date] datetime2(7) Not Null,
[End Date]   datetime2(7) Not Null,
);
GO
-- Created new Holiday List Table in webApp database--
CREATE TABLE HolidayList (
Id bigint Not Null Identity,
AreaCode nvarchar(10) Not Null,
[Date]  datetime2(7) Not Null,
[Description]  nvarchar(50) Not Null,
StartDate  datetime2(7) Not Null,
EndDate  datetime2(7) Not Null,
);
GO

--Created Stored Procedure to transform Holiday List Data'--
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
--Added Leave Duration  Code Type into Code Table:
INSERT into dbo.codeTable
(codeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
('LeaveDuration', 'Half Day', 'Half Day', 10, 1, 1),
('LeaveDuration', 'Full Day', 'Full Day', 20, 1, 1);
GO