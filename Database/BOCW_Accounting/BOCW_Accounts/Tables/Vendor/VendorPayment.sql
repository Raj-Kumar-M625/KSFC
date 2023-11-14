CREATE TABLE [dbo].[VendorPayment]
(
	[ID] INT NOT NULL PRIMARY KEY,
	[VendorID] INT NOT NULL ,
	[PaymentReferenceNo]  Varchar(50) NOT NULL,
	[PaymentDate] DATETIME2 NOT NULL,
	[PaidAmount] DECIMAL(18,2) NOT NULL,
	[PaymentStatus] NVARCHAR(50) NOT NULL,
	[ApprovedBy] NVARCHAR(50) NOT NULL,
	[CreatedBy] NVARCHAR(100) NULL,
    [CreatedOn] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [ModifiedBy] NVARCHAR(100) NULL,
    [ModifiedOn] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	  CONSTRAINT [FK_VendorPayment_ToVendor] FOREIGN KEY ([VendorID]) REFERENCES [Vendor]([ID])
	  )
