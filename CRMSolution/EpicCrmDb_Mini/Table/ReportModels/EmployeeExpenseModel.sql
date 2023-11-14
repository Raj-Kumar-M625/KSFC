CREATE TABLE [dbo].[EmployeeExpenseModel]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[EmployeeId] BIGINT,
	[DayId] BIGINT,
	[StaffCode] NVARCHAR(50),
	[Name] NVARCHAR(50),
	[ExpenseDate] DATETIME2,
	[ExpenseHQCode] NVARCHAR(50),
	[ModeAndClassOfFare] DECIMAL(19,2),
	[LodgeRent] DECIMAL(19,2),
	[LocalConveyance] DECIMAL(19,2),
	[OutstationConveyance] DECIMAL(19,2),
	[IncdlCharges] DECIMAL(19,2),
	[CommunicationExpenses] DECIMAL(19,2),
	[TotalExpenseAmount] DECIMAL(19,2),

	[StartPosition] NVARCHAR(128),
	[EndPosition] NVARCHAR(128),
	[StartTime] DATETIME2,
	[EndTime] DATETIME2,

	[Department] NVARCHAR(50),
	[Designation] NVARCHAR(50),

	[ZoneCode] NVARCHAR(10),
	[ZoneName] NVARCHAR(50),
	[AreaCode] NVARCHAR(10),
	[AreaName] NVARCHAR(50),
	[TerritoryCode] NVARCHAR(10),
	[TerritoryName] NVARCHAR(50),
	[HQCode] NVARCHAR(10),
	[HQName] NVARCHAR(50),
	TrackingDistanceInMeters DECIMAL(19,2)
)
