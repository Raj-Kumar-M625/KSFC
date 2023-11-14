CREATE TABLE [dbo].[VendorBankAccount]
(
	[ID] INT NOT NULL PRIMARY KEY IDENTITY,
    [BankMasterID] INT NULL,
    [VendorID] INT NULL,
    [BeneficiaryName] NVARCHAR(100) NULL,
    [AccountNumber] NVARCHAR(20) NULL,
    [StartDate] DATETIME2 NULL,
    [EndDate] DATETIME2 NULL,
    [Status] BIT NOT NULL DEFAULT 1,
    [CreatedBy] NVARCHAR(100) NULL,
    [CreatedOn] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [ModifedBy] NVARCHAR(100) NULL,
    [ModifiedOn] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT [FK_VendorBankAccount_ToBankMaster] FOREIGN KEY ([BankMasterID]) REFERENCES [BankMaster]([ID]),
    CONSTRAINT [FK_VendorBankAccount_ToVendor] FOREIGN KEY ([VendorID]) REFERENCES [Vendor]([ID])
)
