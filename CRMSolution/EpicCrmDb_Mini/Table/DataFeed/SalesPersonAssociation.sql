CREATE TABLE [dbo].[SalesPersonAssociation]
(
    -- This table defines what all levels (Zone/Area/Territory/HQ Codes), a 
	-- Sales person is associated with.
	[Id] INT NOT NULL PRIMARY KEY Identity,
	[StaffCode] NVARCHAR(10) NOT NULL,
	[CodeType] NVARCHAR(50) NOT NULL,
	[CodeValue] NVARCHAR(50) NOT NULL,
	[IsDeleted] BIT NOT NULL DEFAULT 0,
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[CreatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[UpdatedBy] NVARCHAR(50) NOT NULL DEFAULT ''
)
