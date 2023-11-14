CREATE TABLE TransporterBankDetails(
	Id bigint NOT NULL identity(1,1) PRIMARY KEY,
	CompanyCode nvarchar(50) NOT NULL,
	AccountHolderName nvarchar(50) NOT NULL,
	BankName nvarchar(50) NOT NULL,
	BankAccount nvarchar(50) NOT NULL,
	BankIFSC nvarchar(50) NOT NULL,
	BankBranch nvarchar(50) NOT NULL,
	IsActive bit NOT NULL 
)