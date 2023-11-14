CREATE TABLE [dbo].[EmployeeYearlyTargetInput]
(
	[Employee Code] NVARCHAR(10) NOT NULL,
	[Year] INT NOT NULL,
	[Type] NVARCHAR(200) NOT NULL,
	[Yearly Target] DECIMAL(19,2) NOT NULL DEFAULT 0
)