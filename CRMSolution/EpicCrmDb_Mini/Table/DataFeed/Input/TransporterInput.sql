CREATE TABLE [dbo].[TransporterInput]
(
	[Company Code] NVARCHAR(20) NOT NULL,
	[Company name] NVARCHAR(50) NOT NULL,
	[Vehicle type] NVARCHAR(20) NOT NULL,
	[Vehicle No] NVARCHAR(20) NOT NULL,
	[Vehicle capacity in Kgs] INT NOT NULL,
	[Transportation Type] NVARCHAR(10) NOT NULL,
	[Hamali Rate per Bag (Rs)] DECIMAL(9,2) NOT NULL,
	[Silo To Return KM] INT NOT NULL,
	[Cost per Km] DECIMAL(9,2) NOT NULL,
	[Extra Cost per Ton] DECIMAL(9,2) NOT NULL,
	[Account Holder Name] nvarchar(50) NOT NULL DEFAULT '',
	[Bank Account Number] nvarchar(50) NOT NULL DEFAULT '',
	[Bank Name] nvarchar(50) NOT NULL DEFAULT '',
	[Bank IFSC] nvarchar(50) NOT NULL DEFAULT '',
	[Bank Branch] nvarchar(50) NOT NULL DEFAULT ''
)