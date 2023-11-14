CREATE TABLE [dbo].[EmployeeMaster](

	[Staff Code] [nvarchar](10) NOT NULL, --*
	[Name] [nvarchar](50) NOT NULL, --*
	[Phone] [NVARCHAR](20) NOT NULL, --*
	[Head Quarter] [nvarchar](50) NOT NULL DEFAULT '', --*
	[Action] [nvarchar](10) NOT NULL, --*
	[DepartmentOrDivision] NVARCHAR(50),
	[Designation] NVARCHAR(50),
	[Business Role] NVARCHAR(50),
	[Expense Rate Override] NVARCHAR(50),
	[Two Wheeler RatePerKM] Decimal(19,2),
	[Four Wheeler RatePerKM] Decimal(19,2)
) ON [PRIMARY]
GO
