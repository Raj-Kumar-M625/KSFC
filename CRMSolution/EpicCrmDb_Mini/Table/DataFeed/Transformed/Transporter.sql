CREATE TABLE [dbo].[Transporter]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[CompanyCode] NVARCHAR(20) NOT NULL,
	[CompanyName] NVARCHAR(50) NOT NULL,
	[VehicleType] NVARCHAR(20) NOT NULL,
	[VehicleNo] NVARCHAR(20) NOT NULL,
	[TransportationType] NVARCHAR(10) NOT NULL,
	[SiloToReturnKM] INT NOT NULL,
	[VehicleCapacityKgs] INT NOT NULL,
	[HamaliRatePerBag] DECIMAL(9,2) NOT NULL,
	[CostPerKm] DECIMAL(9,2) NOT NULL,
	[ExtraCostPerTon] DECIMAL(9,2) NOT NULL
)
