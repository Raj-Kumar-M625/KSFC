CREATE TABLE [dbo].[EmployeeYearlyTarget]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[EmployeeCode] NVARCHAR(10) NOT NULL,
	[Year] INT NOT NULL,
	[Type] NVARCHAR(200) NOT NULL,
	[YearlyTarget] DECIMAL(19,2) NOT NULL DEFAULT 0
)