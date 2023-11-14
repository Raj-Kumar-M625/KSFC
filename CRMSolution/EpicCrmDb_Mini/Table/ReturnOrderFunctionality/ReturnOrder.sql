CREATE TABLE [dbo].[ReturnOrder]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[EmployeeId] BIGINT NOT NULL REFERENCES TenantEmployee,
	[DayId] BIGINT NOT NULL REFERENCES dbo.[Day],
	[CustomerCode] NVARCHAR(50) NOT NULL,
	[ReturnOrderDate] DATETIME2 NOT NULL,
	[TotalAmount] DECIMAL(19,2) NOT NULL,
	[ItemCount] BIGINT NOT NULL DEFAULT 0,
	[ReferenceNumber] NVARCHAR(256),
	[Comment] NVARCHAR(2048),

	-- approval fields
	[IsApproved] BIT NOT NULL DEFAULT 0,
	[ApproveDate] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[ApproveRef] NVARCHAR(255),
	[ApproveNotes] NVARCHAR(2048),
	[ApproveAmount] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[ApprovedBy] NVARCHAR(50) NOT NULL DEFAULT '',

	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[SqliteReturnOrderId] BIGINT NOT NULL  -- used while processing orders from sqliteReturnOrder table
)
