CREATE TABLE [dbo].[VendorDefaults]
(
	[ID] INT NOT NULL PRIMARY KEY IDENTITY,
    [VendorID] INT NULL,
    [Category] NVARCHAR(50) NULL,
    [GSTPercentage] DECIMAL(5,2) NULL,
    [PaymentTerms] NVARCHAR(20) NULL,
    [TDSSection] NVARCHAR(50) NULL,
    [PaymentMethod] NVARCHAR(10) NULL,
    [TDSPercentage] DECIMAL(5,2) NULL,
    [GST_TDSPercentage] DECIMAL(5,2) NULL,
    [CreatedBy] NVARCHAR(100) NULL,
    [CreatedOn] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [ModifiedBy] NVARCHAR(100) NULL,
    [ModifiedOn] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT [FK_VendorDefaults_ToVendor] FOREIGN KEY ([VendorID]) REFERENCES [Vendor]([ID])
)
