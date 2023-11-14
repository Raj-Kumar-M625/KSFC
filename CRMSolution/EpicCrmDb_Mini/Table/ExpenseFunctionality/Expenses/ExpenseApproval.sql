CREATE TABLE [dbo].[ExpenseApproval]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[ExpenseId] BIGINT NOT NULL REFERENCES Expense,

	-- approval fields
	[ApproveLevel] NVARCHAR(20) NOT NULL, -- Territory / Area / Zone
	[ApproveDate] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[ApproveRef] NVARCHAR(255),
	[ApproveNotes] NVARCHAR(2048),
	[ApproveAmount] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[ApprovedBy] NVARCHAR(50) NOT NULL DEFAULT '',

	[Timestamp] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
)
