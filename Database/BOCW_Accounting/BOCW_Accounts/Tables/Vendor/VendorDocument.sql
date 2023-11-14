CREATE TABLE [dbo].[VendorDocument]
(
	[ID] INT NOT NULL PRIMARY KEY IDENTITY,
    [VendorID] INT NOT NULL,
    [DocumentID] INT NOT NULL,
    CONSTRAINT [FK_VendorDocument_ToVendor] FOREIGN KEY ([VendorID]) REFERENCES [Vendor]([ID]),
    CONSTRAINT [FK_VendorDocument_ToDocument] FOREIGN KEY ([DocumentID]) REFERENCES [Document]([ID])
)
