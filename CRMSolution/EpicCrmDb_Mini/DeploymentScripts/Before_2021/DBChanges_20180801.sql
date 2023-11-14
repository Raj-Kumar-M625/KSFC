ALTER TABLE [dbo].[Customer]
    ADD [Address1] NVARCHAR (50) NULL,
        [Address2] NVARCHAR (50) NULL,
        [Email]    NVARCHAR (50) NULL;

ALTER TABLE [dbo].[Customer]
	ADD [DateCreated] DATETIME2 NOT NULL DEFAULT Sysutcdatetime(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[CreatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[UpdatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[IsActive] Bit NOT NULL Default 1;
go

CREATE Unique INDEX [IX_Customer_CustomerCode]
	ON [dbo].[Customer]
	(CustomerCode)
go

insert into dbo.CodeTable
(CodeType, CodeValue, CodeName, DisplaySequence, IsActive)
values
('DealerType','DEALER','DEALER',10,1),
('DealerType','P.DISTRIBUTOR','P.DISTRIBUTOR',20,1),
('DealerType','DISTRIBUTOR','DISTRIBUTOR',30,1)

GO
ALTER PROCEDURE [dbo].[TransformDataFeed]
	@tenantId BIGINT
AS
BEGIN
		-- Stored procedure to put data from SAP data feed (tables)
		-- to local tables

		-- Step 1: Not defined.
		-- Input Tables
		-- CustomerMaster
		-- MaterialMaster
		-- EmployeeMaster
		-- PriceList

		-- Nov 17 2017 - don't delete existing data from Sales Person table
		-- From Employee Master, take new records and put them in SalesPerson table.

		-- Aug 02 2018 - don't delete existing data from customer table
		-- from Customer Master, take new records and put them in Customer table

		-- Step 2: Wipe out existing data
		DELETE FROM dbo.ProductPrice
		DELETE FROM dbo.Product;
		DELETE FROM dbo.ProductGroup;
		--DELETE FROM dbo.Customer;
		--DELETE FROM dbo.SalesPerson;

		-- Step 3: Insert data in Product Group and Product table
		INSERT INTO dbo.ProductGroup
		(GroupName)
		SELECT DISTINCT ltrim(rtrim([Product Group])) FROM dbo.MaterialMaster
		WHERE [Product Group] IS NOT NULL

		-- Step 4: Insert data in Product table
		;WITH materialCTE(productCode, ProductGroup, Name, UOM, BrandName, ShelfLifeInMonths, IsActive, RN)
		AS
		(
			SELECT rtrim(ltrim(mm.Material)),
			ltrim(rtrim([Product Group])),
			ISNULL(mm.[Description], ''), 
			ISNULL(mm.UOM,''), 
			ISNULL(mm.[Brand Name],''),
			ISNULL(mm.[Shelf Life in months], 0),
			Case when mm.[Status] = 'ACTIVE' THEN 1 ELSE 0 END,
			Row_Number() OVER (Partition By rtrim(ltrim(mm.Material)) ORDER BY rtrim(ltrim(mm.Material)) )
			FROM dbo.MaterialMaster mm
			WHERE [Product Group] IS NOT NULL
		)
		INSERT INTO dbo.Product
		(GroupId, ProductCode, Name, UOM, BrandName, ShelfLifeInMonths, IsActive, MRP)
		SELECT pg.Id, cte.productCode, cte.Name, cte.UOM, cte.BrandName, cte.ShelfLifeInMonths, CTE.IsActive, 0
		 FROM materialCTE cte
		INNER JOIN dbo.ProductGroup pg on cte.ProductGroup = pg.GroupName
		AND cte.RN = 1
	
		-- Step 4: Insert data in ProductPrice table
		INSERT INTO dbo.ProductPrice
		(ProductId, AreaCode, Stock, [MRP], DistPrice, PDISTPrice, DEALERPrice)
		SELECT p.Id, 
		LEFT(ISNULL(pl.AO, ''),10),
		ISNULL(pl.Stock, 0),
		ISNULL(pl.MRP,0),
		ISNULL(pl.[Dist Rate],0), 
		ISNULL(pl.[PD'S Rate],0),
		ISNULL(pl.[Dealers Rate],0)
		FROM dbo.PriceList pl
		INNER JOIN dbo.Product p ON ltrim(rtrim(pl.[PRODUCT CODE])) = p.ProductCode
		AND pl.[PRODUCT CODE] IS NOT NULL

		-- Step 5: Insert data in SalesPerson table
		INSERT INTO dbo.SalesPerson
		([StaffCode], [Name], [Phone], [HQCode], [IsActive])
		SELECT  ltrim(STR(ISNULL([Staff Code],0),10,0)),
		ISNULL(ltrim(rtrim(em.Name)),''),
		ltrim(Str(IsNULL(em.[Phone], 0), 50, 0)),
		ltrim(rtrim(ISNULL([Head Quarter], ''))),
		Case when ltrim(rtrim([Action])) = 'ACTIVE' THEN 1 ELSE 0 END
		FROM dbo.EmployeeMaster em
		LEFT JOIN dbo.SalesPerson sp ON [Staff Code] IS NOT NULL
				AND ltrim(STR(ISNULL(em.[Staff Code],0),10,0)) = sp.StaffCode
		WHERE sp.StaffCode IS NULL

		-- Step 6: Insert data in Customer table
		INSERT INTO [dbo].[Customer]
		([CustomerCode], [Name], [Type], [CreditLimit], 
		[Outstanding], [LongOutstanding], [District], [State], [Branch], [Pincode], [HQCode], [ContactNumber],
		[Target], [Sales], [Payment])
		SELECT 
		left(cm.[Customer Code], 20),
		left(cm.[Customer Name],50), 
		cm.[Type], 
		ISNULL(cm.[Credit Limit],0),
		ISNULL([Total Outstanding], 0),
		ISNULL([Total Overdue >90], 0),
		left(cm.[District Name],50),
		left(cm.[State Name],50),
		left(cm.[Branch Name],50),
		left(cm.Pincode,10),
		left(ISNULL([HQ Code], ''),10),
		ltrim(Str(IsNULL([Contact #], 0), 50, 0)),
		ISNULL([Expected Business], 0), -- Target
		ISNULL(cm.[Sales],0),  -- Sales
		ISNULL([Collection], 0) -- Payment
		FROM dbo.CustomerMaster cm
		LEFT JOIN dbo.Customer c on cm.[Customer Code] IS NOT NULL AND
			left(cm.[Customer Code], 20) = c.CustomerCode
		WHERE cm.[Type] IS NOT NULL
		AND c.CustomerCode IS NULL

		-- Step 7: Update names changes from SalesPerson table to TenantEmployee table
		UPDATE dbo.TenantEmployee
		SET Name = sp.Name
		FROM dbo.TenantEmployee te
		INNER JOIN dbo.SalesPerson sp on te.EmployeeCode = sp.StaffCode
		AND te.Name <> sp.Name


		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformDataFeed', 'Success'
END
GO
