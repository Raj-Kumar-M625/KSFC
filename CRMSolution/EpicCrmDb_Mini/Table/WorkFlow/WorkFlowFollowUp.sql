CREATE TABLE [dbo].[WorkFlowFollowUp]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[TypeName] NVARCHAR(50) NOT NULL, -- could be crop name
	[Phase] NVARCHAR(50) NOT NULL,

	[PhoneDataEntryPage] NVARCHAR(50) NOT NULL DEFAULT '',
	[FollowUpPhaseTag] NVARCHAR(50) NOT NULL DEFAULT '',
	    -- can be called a fk in Code Table

	[MinFollowUps] INT NOT NULL DEFAULT 0,
	[MaxFollowUps] INT NOT NULL DEFAULT 0,

	-- used at the time of follow up activity creation only
	[TargetStartAtDay] INT NOT NULL DEFAULT 0,
	[TargetEndAtDay] INT NOT NULL DEFAULT 0,

	-- A -ve number indicates that DWS is optional
	[MaxDWS] INT NOT NULL DEFAULT 0,

	[IsActive] BIT NOT NULL DEFAULT 1
)
