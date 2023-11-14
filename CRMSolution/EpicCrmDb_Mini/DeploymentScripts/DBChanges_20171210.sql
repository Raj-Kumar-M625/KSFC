-- Schema changes deployed on Dec 11 2017
CREATE TABLE [dbo].[FeatureControl]
(
	[Id] INT NOT NULL PRIMARY KEY Identity,
	[UserName] NVARCHAR(50) NOT NULL,
	[ActivityFeature] BIT NOT NULL DEFAULT 0,
	[OrderFeature] BIT NOT NULL DEFAULT 0,
	[PaymentFeature] BIT NOT NULL DEFAULT 0,
	[OrderReturnFeature] BIT NOT NULL DEFAULT 0,
	[ExpenseFeature] BIT NOT NULL DEFAULT 0,
	[ExpenseReportFeature] BIT NOT NULL DEFAULT 0,
	[AttendanceReportFeature] BIT NOT NULL DEFAULT 0,
	[KPIFeature] BIT NOT NULL DEFAULT 0,
	[SalesPersonFeature] BIT NOT NULL DEFAULT 0,
	[CustomerFeature] BIT NOT NULL DEFAULT 0,
	[ProductFeature] BIT NOT NULL DEFAULT 0,
	[CrmUserFeature] BIT NOT NULL DEFAULT 0,
	[AssignmentFeature] BIT NOT NULL DEFAULT 0,
	[UploadDataFeature] BIT NOT NULL DEFAULT 0,
	
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
)
go

insert into dbo.FeatureControl
(UserName)
values
('administrator')
go

ALTER TABLE [dbo].[ExecAppImei] ADD [Comment] NVARCHAR(100)
go

