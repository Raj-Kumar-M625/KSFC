CREATE TABLE [dbo].[EmployeeMonthlyTarget]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[EmployeeCode] NVARCHAR(10) NOT NULL,
	[Month] INT NOT NULL,
	[Year] INT NOT NULL,
	[Type] NVARCHAR(200) NOT NULL,
	[MonthlyTarget] DECIMAL(19,2) NOT NULL DEFAULT 0
)