CREATE PROCEDURE [dbo].[TransformTransporterDataFeed]
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
