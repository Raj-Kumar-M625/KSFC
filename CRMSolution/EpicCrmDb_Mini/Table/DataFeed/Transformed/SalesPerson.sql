CREATE TABLE [dbo].[SalesPerson]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[StaffCode] NVARCHAR(10) NOT NULL,
	[Name] NVARCHAR(50) NOT NULL,
	[Phone] NVARCHAR(50),
	[HQCode] NVARCHAR(50) NOT NULL DEFAULT '',
	[Grade] NVARCHAR(50),
	[Department] NVARCHAR(50),
	[Designation] NVARCHAR(50),
	[OwnershipType] NVARCHAR(50),
	[VehicleType] NVARCHAR(50),
	[FuelType] NVARCHAR(50),
	[VehicleNumber] NVARCHAR(15),
	[BusinessRole] NVARCHAR(50) NOT NULL DEFAULT '',
	[OverridePrivateVehicleRatePerKM] BIT NOT NULL DEFAULT 0,
	[TwoWheelerRatePerKM] Decimal(19,2) NOT NULL DEFAULT 0,
	[FourWheelerRatePerKM] Decimal(19,2) NOT NULL DEFAULT 0,
	[IsActive] BIT NOT NULL
)
