CREATE TABLE [dbo].[DayPlanTarget](
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[TenantId] BIGINT NOT NULL REFERENCES dbo.Tenant([Id]),
	[EmployeeId] [bigint] NOT NULL REFERENCES dbo.TenantEmployee([Id]),
	[DayId] [bigint] NOT NULL REFERENCES dbo.[Day]([Id]),
	[EmployeeCode]  NVARCHAR(10) NOT NULL,
	[PlanDate]  DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[TargetSales] DECIMAL(19, 2) NOT NULL,
	[TargetCollection] DECIMAL(19, 2) NOT NULL,
	[TargetVigoreSales] DECIMAL(19, 2) NOT NULL DEFAULT 0,
	[TargetDealerAppointment] INT NOT NULL,
	[SqliteDayPlanTargetId] BIGINT NOT NULL DEFAULT 0,
	[TargetDemoActivity] INT NOT NULL DEFAULT 0
)
GO