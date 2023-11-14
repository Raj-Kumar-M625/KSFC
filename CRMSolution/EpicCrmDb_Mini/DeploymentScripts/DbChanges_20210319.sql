CREATE TABLE [dbo].[EmployeeAchievedInput]
(
	[Employee Code] NVARCHAR(10) NOT NULL,
	[Month] INT NOT NULL,
	[Year] INT NOT NULL,
	[Type] NVARCHAR(200) NOT NULL,
	[Achieved Monthly] DECIMAL(19,2) NOT NULL DEFAULT 0,
)

GO


CREATE TABLE [dbo].[EmployeeAchieved]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[EmployeeCode] NVARCHAR(10) NOT NULL,
	[Month] INT NOT NULL,
	[Year] INT NOT NULL,
	[Type] NVARCHAR(200) NOT NULL,
	[AchievedMonthly] DECIMAL(19,2) NOT NULL DEFAULT 0,
)

GO


INSERT into dbo.codeTable
(codeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
('ExcelUpload', 'EmployeeAchieved', 'EmployeeAchievedInput', 140, 1, 1)

GO

CREATE PROCEDURE [dbo].[TransformEmployeeAchievedData]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
		-- Step 1: Truncate table if opted to do so

		TRUNCATE TABLE dbo.[EmployeeAchieved]

		-- Insert New data
		INSERT INTO dbo.[EmployeeAchieved]
		(
			[EmployeeCode],
			[Month],
			[Year],
			[Type],
			[AchievedMonthly]
		)
		SELECT  
			[Employee Code],
			[Month],
			[Year],
			[Type],
			[Achieved Monthly]		
		FROM dbo.[EmployeeAchievedInput]

		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformEmployeeAchievedData', 'Success'
END
GO

-------------------------------------

CREATE TABLE [dbo].[EmployeeMonthlyTargetInput]
(
	[Employee Code] NVARCHAR(10) NOT NULL,
	[Month] INT NOT NULL,
	[Year] INT NOT NULL,
	[Type] NVARCHAR(200) NOT NULL,
	[Monthly Target] DECIMAL(19,2) NOT NULL DEFAULT 0
)

GO


CREATE TABLE [dbo].[EmployeeMonthlyTarget]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[EmployeeCode] NVARCHAR(10) NOT NULL,
	[Month] INT NOT NULL,
	[Year] INT NOT NULL,
	[Type] NVARCHAR(200) NOT NULL,
	[MonthlyTarget] DECIMAL(19,2) NOT NULL DEFAULT 0
)

GO

INSERT into dbo.codeTable
(codeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
('ExcelUpload', 'Employee Monthly Target', 'EmployeeMonthlyTargetInput', 150, 1, 1)

GO

CREATE PROCEDURE [dbo].[TransformEmployeeMonthlyTargetData]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
		-- Step 1: Truncate table if opted to do so

		TRUNCATE TABLE dbo.[EmployeeMonthlyTarget]

		-- Insert New data
		INSERT INTO dbo.EmployeeMonthlyTarget
		(
			[EmployeeCode],
			[Month],
			[Year],
			[Type],
			[MonthlyTarget]
		)
		SELECT  
			[Employee Code],
			[Month],
			[Year],
			[Type],
			[Monthly Target]		
		FROM dbo.EmployeeMonthlyTargetInput

		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformEmployeeMonthlyTargetData', 'Success'
END
GO

-----------------------------------------


CREATE TABLE [dbo].[EmployeeYearlyTargetInput]
(
	[Employee Code] NVARCHAR(10) NOT NULL,
	[Year] INT NOT NULL,
	[Type] NVARCHAR(200) NOT NULL,
	[Yearly Target] DECIMAL(19,2) NOT NULL DEFAULT 0
)

GO

CREATE TABLE [dbo].[EmployeeYearlyTarget]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[EmployeeCode] NVARCHAR(10) NOT NULL,
	[Year] INT NOT NULL,
	[Type] NVARCHAR(200) NOT NULL,
	[YearlyTarget] DECIMAL(19,2) NOT NULL DEFAULT 0
)

GO

INSERT into dbo.codeTable
(codeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
('ExcelUpload', 'Employee Yearly Target', 'EmployeeYearlyTargetInput', 160, 1, 1)

GO


CREATE PROCEDURE [dbo].[TransformEmployeeYearlyTargetData]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
		-- Step 1: Truncate table if opted to do so

		TRUNCATE TABLE dbo.[EmployeeYearlyTarget]

		-- Insert New data
		INSERT INTO dbo.EmployeeYearlyTarget
		(
			[EmployeeCode],
			[Year],
			[Type],
			[YearlyTarget]
		)
		SELECT  
			[Employee Code],
			[Year],
			[Type],
			[Yearly Target]		
		FROM dbo.EmployeeYearlyTargetInput

		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformEmployeeYearlyTargetData', 'Success'
END
GO
