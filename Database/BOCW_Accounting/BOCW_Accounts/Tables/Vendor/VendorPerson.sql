﻿CREATE TABLE [dbo].[VendorPerson]
(
	[ID] INT NOT NULL PRIMARY KEY IDENTITY,
    [VendorID] INT NULL,
    [StartDate] DATETIME2 NULL,
    [EndDate] DATETIME2 NULL,
    [Status] BIT NOT NULL DEFAULT 1,
    [CreatedBy] NVARCHAR(100) NULL,
    [CreatedOn] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [ModifedBy] NVARCHAR(100) NULL,
    [ModifiedOn] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    CONSTRAINT [FK_VendorPerson_ToVendor] FOREIGN KEY ([VendorID]) REFERENCES [Vendor]([ID])
)