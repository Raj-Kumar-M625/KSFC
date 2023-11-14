CREATE TABLE [dbo].[TransportType]
(
    -- expense types that are valid for an expense entry.
	[Id] BIGINT NOT NULL Identity PRIMARY KEY,
	[DisplaySequence] INT NOT NULL,
	[TransportTypeCode] NVARCHAR(50) NOT NULL,
	[ReimbursementRatePerUnit] Decimal(19,2) NOT NULL DEFAULT 0,
	[IsPublic] BIT NOT NULL DEFAULT 0,
	[IsActive] BIT NOT NULL DEFAULT 1
)
