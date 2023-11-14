CREATE TABLE [dbo].[Payment]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[EmployeeId] BIGINT NOT NULL REFERENCES TenantEmployee,
	[DayId] BIGINT NOT NULL REFERENCES dbo.[Day],
	[CustomerCode] NVARCHAR(50) NOT NULL,

	[PaymentType] NVARCHAR(10) NOT NULL, -- Cash/Cheque
	[PaymentDate] DATETIME2 NOT NULL,
	[TotalAmount] DECIMAL(19,2) NOT NULL,
	[Comment] NVARCHAR(2048),
	[ImageCount] INT NOT NULL DEFAULT 0,

	-- approval fields
	[IsApproved] BIT NOT NULL DEFAULT 0,
	[ApproveDate] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[ApproveRef] NVARCHAR(255),
	[ApproveNotes] NVARCHAR(2048),
	[ApproveAmount] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[ApprovedBy] NVARCHAR(50) NOT NULL DEFAULT '',

	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[SqlitePaymentId] BIGINT NOT NULL  -- used while processing 
)
