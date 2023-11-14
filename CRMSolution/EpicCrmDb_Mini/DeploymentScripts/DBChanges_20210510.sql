------Transporter Payment--------

CREATE TABLE [dbo].[STRPayment](
	Id bigint not null identity(1,1) primary key,
	STRTagId bigint foreign key references STRTag(Id),
	STRNumber nvarchar(50) null,
	PaymentReference nvarchar(50) null,
	ShedToFirstLoadingOdo bigint null,
	TotalRunningKms int null,
	TransportationCharges decimal(19,2) null,
	ExtraTonnage decimal(19,2) null,
	ExtraTonCharges decimal(19,2) null,
	TollCharges decimal(19,2) null,
	WeighmentCharges decimal(19,2) null,
	HamaliCharges decimal(19,2) null,
	Others decimal(19,2) null,
	NetPayableAmount decimal(19,2) null,
	Comments nvarchar(500) null,
	[Status] nvarchar(50) not null,
	[DateCreated] [datetime2](7) NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] [datetime2](7) NOT NULL DEFAULT SYSUTCDATETIME(),
	[CreatedBy] [nvarchar](50) NOT NULL,
	[UpdatedBy] [nvarchar](50) NOT NULL
)
GO
---------------------------------------
CREATE TABLE TransporterBankDetails(
	Id bigint NOT NULL identity(1,1) PRIMARY KEY,
	CompanyCode nvarchar(50) NOT NULL,
	--IsSelfAccount bit NOT NULL,
	AccountHolderName nvarchar(50) NOT NULL,
	--AccountHolderPAN nvarchar(50) NOT NULL,
	BankName nvarchar(50) NOT NULL,
	BankAccount nvarchar(50) NOT NULL,
	BankIFSC nvarchar(50) NOT NULL,
	BankBranch nvarchar(50) NOT NULL,
	--DateCreated datetime2(7) NOT NULL,
	--DateUpdated datetime2(7) NOT NULL,
	--CreatedBy nvarchar(50) NOT NULL,
	--UpdatedBy nvarchar(50) NOT NULL,
	IsActive bit NOT NULL 
	--Status nvarchar(50) NOT NULL,
	--IsApproved bit NOT NULL,
	--Comments nvarchar(100) NOT NULL,
)
GO
-------------------------------------------
Create Table TransporterPaymentReference
(
	Id bigint not null identity(1,1) primary key,
	PaymentReference nvarchar(50) null,
	Comments nvarchar(100) not null DEFAULT '',
	NetPayableAmount decimal(19,2) null,
	STRNumber nvarchar(50) not null,
	CreatedBy nvarchar(50) not null,
	UpdatedBy nvarchar(50) not null,
	DateCreated datetime2(7) not null DEFAULT SYSUTCDATETIME(),
	DateUpdated datetime2(7) not null DEFAULT SYSUTCDATETIME(),
	AccountNumber nvarchar(50) null,
	AccountName nvarchar(50) null,
	AccountAddress nvarchar(50) null,
	AccountEmail nvarchar(50) null,
	PaymentType nvarchar(50) null,
	SenderInfo nvarchar(50) null,
	LocalTimeStamp datetime2(7) not null
)
Go

DROP TABLE [dbo].[TransporterInput]
GO

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
GO


ALTER PROCEDURE [dbo].[TransformTransporterDataFeed]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
		-- Step 1: Truncate table if opted to do so

		IF @IsCompleteRefresh = 1
		BEGIN
			TRUNCATE TABLE dbo.Transporter
			--TRUNCATE TABLE dbo.TransporterBankDetails
		END

		-- Get data of Transport Bank Details
		DECLARE @tbd TABLE (CompanyCode NVARCHAR(50), AccName NVARCHAR(50), BankAccountNumber NVARCHAR(50), 
		BankName NVARCHAR(50), BankIFSC NVARCHAR(50), BankBranch NVARCHAR(50))
		INSERT INTO @tbd
		(CompanyCode, AccName, BankAccountNumber, BankName, BankIFSC, BankBranch)
		SELECT DISTINCT [Company Code], [Account Holder Name], [Bank Account Number], [Bank Name], [Bank IFSC],[Bank Branch] 
		FROM dbo.TransporterInput

		-- Update existing Data
		UPDATE dbo.TransporterBankDetails
		SET
			[AccountHolderName] = tbd.[AccName],
			[BankAccount] = tbd.[BankAccountNumber],
			[BankName] = tbd.[BankName],
			[BankIFSC] = tbd.[BankIFSC],
			[BankBranch] = tbd.[BankBranch]
		FROM dbo.TransporterBankDetails tbi
		INNER JOIN @tbd tbd on tbi.CompanyCode = tbd.[CompanyCode] 

		UPDATE dbo.Transporter
		SET 
		    [CompanyCode] = ti.[Company code],
			[CompanyName] = ti.[Company name],
			[VehicleType] = ti.[Vehicle type],
			[TransportationType] = ti.[Transportation Type],
			[SiloToReturnKM] = ti.[Silo To Return KM],
			[VehicleCapacityKgs] = ti.[Vehicle capacity in Kgs],
			[HamaliRatePerBag] = ti.[Hamali Rate per Bag (Rs)],
			[CostPerKm] = ti.[Cost per Km],
			[ExtraCostPerTon] = ti.[Extra Cost per Ton]
		FROM dbo.Transporter t
		INNER JOIN dbo.TransporterInput ti on t.VehicleNo = ti.[Vehicle No]

		-- Insert New data

		INSERT INTO dbo.TransporterBankDetails
		(
		[CompanyCode], [AccountHolderName],	[BankAccount], [BankName], [BankIFSC], [BankBranch], [IsActive]
		)
		SELECT DISTINCT [Company Code], [Account Holder Name], [Bank Account Number], [Bank Name], [Bank IFSC],[Bank Branch], 1 
		FROM dbo.TransporterInput ti
		LEFT JOIN dbo.TransporterBankDetails tb ON ti.[Company Code] = tb.CompanyCode
		WHERE tb.[CompanyCode] IS NULL

		INSERT INTO dbo.Transporter
		(
		    [CompanyCode], [CompanyName], [VehicleType], [VehicleNo], [TransportationType],	[SiloToReturnKM], [VehicleCapacityKgs],
			[HamaliRatePerBag],	[CostPerKm], [ExtraCostPerTon] 
		)
		SELECT  
		    [Company code], [Company name], [Vehicle type], [Vehicle No], [Transportation Type], [Silo To Return KM],
			[Vehicle capacity in Kgs], [Hamali Rate per Bag (Rs)], [Cost per Km], [Extra Cost per Ton] 		
		FROM dbo.TransporterInput ti
		LEFT JOIN dbo.Transporter t ON ti.[Vehicle No] = t.VehicleNo
		WHERE t.VehicleNo IS NULL

		-- InActive Back account - row can come again in future uploads, hence the case
		IF @IsCompleteRefresh = 1
		BEGIN
			UPDATE dbo.TransporterBankDetails
			SET IsActive = CASE when tbd.CompanyCode is null then 0 else 1 END
			FROM dbo.TransporterBankDetails tb
			LEFT JOIN @tbd tbd on tb.CompanyCode = tbd.CompanyCode
		END


		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformTransporterDataFeed', 'Success'
END
