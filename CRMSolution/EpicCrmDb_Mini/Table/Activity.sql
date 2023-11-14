CREATE TABLE [dbo].[Activity]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[EmployeeDayId] BIGINT NOT NULL REFERENCES dbo.EmployeeDay,
	[ClientName] NVARCHAR(50) NOT NULL,
	[ClientPhone] NVARCHAR(20) NOT NULL,
	[ClientType] NVARCHAR(50) NOT NULL,
	[ClientCode] NVARCHAR(50) NOT NULL DEFAULT '',
	[AtBusiness] BIT NOT NULL DEFAULT 0,
	[ActivityTrackingType] INT NOT NULL DEFAULT 0,
	[ActivityType] NVARCHAR(50) NOT NULL, -- Field Trial Visit 1, 2, Order Request, Delivery
	[ActivityAmount] DECIMAL(19,2) NOT NULL DEFAULT 0.0,
	[Comments] NVARCHAR(2048) NOT NULL,
	[ImageCount] INT NOT NULL DEFAULT 0,
	[ContactCount] INT NOT NULL DEFAULT 0,
	[At] [DateTime2] NOT NULL,
	[DateCreated] [DATETIME2] NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] [DateTime2] NOT NULL DEFAULT SYSUTCDATETIME()
)
