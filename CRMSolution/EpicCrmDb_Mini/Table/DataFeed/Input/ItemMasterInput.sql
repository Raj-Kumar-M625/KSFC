CREATE TABLE [dbo].[ItemMasterInput]
(
	[ItemCode] NVARCHAR(50) NOT NULL,
	[ItemDesc] NVARCHAR(100) NOT NULL,
	[Category] NVARCHAR(50) NOT NULL,
	[Unit]     NVARCHAR(10) NOT NULL,
	[Rate] DECIMAL(19,2) NOT NULL,
	[ReturnRate] DECIMAL(19,2) NOT NULL,
	[Classification] NVARCHAR(10),
	[CropName] NVARCHAR(50) NOT NULL
)
