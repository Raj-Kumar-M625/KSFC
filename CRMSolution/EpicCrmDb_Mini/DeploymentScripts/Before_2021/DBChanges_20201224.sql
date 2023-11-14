-- Dec 24 2020

ALTER TABLE [dbo].[SalesPerson]
ADD	[OverridePrivateVehicleRatePerKM] BIT NOT NULL DEFAULT 0,
	[TwoWheelerRatePerKM] Decimal(19,2) NOT NULL DEFAULT 0,
	[FourWheelerRatePerKM] Decimal(19,2) NOT NULL DEFAULT 0
GO
