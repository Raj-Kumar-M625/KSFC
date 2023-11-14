CREATE PROCEDURE [dbo].[TransformProductDataFeed]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
		-- Stored procedure to put data from SAP data feed (tables)
		-- to local tables

		-- Step 1: Not defined.
		-- Input Tables
		-- MaterialMaster
		-- PriceList

		-- Step 2: Wipe out existing data - IsCompleteRefresh is not used here

		DELETE FROM dbo.ProductPrice
		DELETE FROM dbo.Product;
		DELETE FROM dbo.ProductGroup;

		-- Step 3: Insert data in Product Group and Product table
		INSERT INTO dbo.ProductGroup
		(GroupName)
		SELECT DISTINCT ltrim(rtrim([Product Group])) FROM dbo.MaterialMaster
		WHERE [Product Group] IS NOT NULL

		-- Step 4: Insert data in Product table
		;WITH materialCTE(productCode, ProductGroup, Name, UOM, BrandName, ShelfLifeInMonths, IsActive, GstCode, RN )
		AS
		(
			SELECT rtrim(ltrim(mm.[Product Code])),
			ltrim(rtrim([Product Group])),
			left(ISNULL(mm.[Description], ''),100), 
			ISNULL(mm.UOM,''), 
			ISNULL(mm.[Brand Name],''),
			ISNULL(mm.[Shelf Life in months], 0),
			Case when mm.[Status] = 'ACTIVE' THEN 1 ELSE 0 END,
			ISNULL(mm.[Gst Code], ''),
			Row_Number() OVER (Partition By rtrim(ltrim(mm.[Product Code])) ORDER BY rtrim(ltrim(mm.[Product Code])))
			FROM dbo.MaterialMaster mm
			WHERE [Product Group] IS NOT NULL
		)
		INSERT INTO dbo.Product
		(GroupId, ProductCode, Name, UOM, BrandName, ShelfLifeInMonths, IsActive, MRP, GstCode)
		SELECT pg.Id, cte.productCode, cte.Name, cte.UOM, cte.BrandName, cte.ShelfLifeInMonths, CTE.IsActive, 0, cte.GstCode
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
		ISNULL(pl.[PD Rate],0),
		ISNULL(pl.[Dealers Rate],0)
		FROM dbo.PriceList pl
		INNER JOIN dbo.Product p ON ltrim(rtrim(pl.[PRODUCT CODE])) = p.ProductCode
		AND pl.[PRODUCT CODE] IS NOT NULL

		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformProductDataFeed', 'Success'
END
