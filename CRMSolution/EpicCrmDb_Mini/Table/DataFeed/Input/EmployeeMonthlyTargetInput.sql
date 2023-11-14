CREATE TABLE [dbo].[EmployeeMonthlyTargetInput]
(
	[Employee Code] NVARCHAR(10) NOT NULL,
	[Month] INT NOT NULL,
	[Year] INT NOT NULL,
	[Type] NVARCHAR(200) NOT NULL,
	[Monthly Target] DECIMAL(19,2) NOT NULL DEFAULT 0
)