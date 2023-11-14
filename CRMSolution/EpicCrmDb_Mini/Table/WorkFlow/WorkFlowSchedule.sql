CREATE TABLE [dbo].[WorkFlowSchedule]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[TypeName] NVARCHAR(50) NOT NULL, -- could be crop name
	[Sequence] INT NOT NULL,
	[TagName] NVARCHAR(50) NOT NULL DEFAULT '',
	[Phase] NVARCHAR(50) NOT NULL,
	[TargetStartAtDay] INT NOT NULL,
	[TargetEndAtDay] INT NOT NULL,
	
	--[PhoneDataEntryPage] NVARCHAR(50) NOT NULL DEFAULT '',
	-- (the field is now in WorkFlowFollowUp table)

	[IsActive] BIT NOT NULL DEFAULT 1
)
