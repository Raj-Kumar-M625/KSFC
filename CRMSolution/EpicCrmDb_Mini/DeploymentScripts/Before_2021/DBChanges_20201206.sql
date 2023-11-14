-- Dec 06 2020

Drop Table dbo.ItemMasterInput
go

CREATE TABLE [dbo].[ItemMasterInput]
(
	[ItemCode] NVARCHAR(50) NOT NULL,
	[ItemDesc] NVARCHAR(100) NOT NULL,
	[Category] NVARCHAR(50) NOT NULL,
	[Unit]     NVARCHAR(10) NOT NULL,
	[Rate] DECIMAL(19,2) NOT NULL,
	[ReturnRate] DECIMAL(19,2) NOT NULL,
	[Classification] NVARCHAR(10),
	[CropName] NVARCHAR(50) NOT NULL
)
GO

ALTER TABLE [dbo].[ItemMasterTypeName]
ADD 
	[Rate] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[ReturnRate] DECIMAL(19,2) NOT NULL DEFAULT 0
GO

UPDATE dbo.ItemMasterTypeName
SET Rate = im.Rate,
ReturnRate = im.Rate
FROM dbo.ItemMasterTypeName imtn
INNER JOIN dbo.ItemMaster im on imtn.ItemMasterId = im.Id
GO

declare @schema_name nvarchar(256)
declare @table_name nvarchar(256)
declare @col_name nvarchar(256)
declare @Command  nvarchar(1000)

set @schema_name = N'dbo'
set @table_name = N'ItemMaster'
set @col_name = N'Rate'

select @Command = 'ALTER TABLE ' + @schema_name + '.[' + @table_name + '] DROP CONSTRAINT ' + d.name
 from sys.tables t
  join sys.default_constraints d on d.parent_object_id = t.object_id
  join sys.columns c on c.object_id = t.object_id and c.column_id = d.parent_column_id
 where t.name = @table_name
  and t.schema_id = schema_id(@schema_name)
  and c.name = @col_name

--print @Command
execute(@Command)
GO

ALTER TABLE [dbo].[ItemMaster]
DROP COLUMN [Rate]
GO

ALTER PROCEDURE [dbo].[TransformItemMasterData]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
		-- can't truncate itemMaster as it is being referenced by IssueReturn table
		DECLARE @ItemMasterInput TABLE
		(
			[ID] BIGINT NOT NULL Identity,
			[ItemCode] NVARCHAR(50),
			[ItemDesc] NVARCHAR(100),
			[Category] NVARCHAR(50),
			[Unit]     NVARCHAR(10),
			--[Rate] DECIMAL(19,2),
			--[ReturnRate] DECIMAL(19,2),
			[Classification] NVARCHAR(10),
			IsDeleted BIT NOT NULL
		)

		-- Step 1: Put data in an in memory table - this is to get Id for each row;
		INSERT INTO @ItemMasterInput
		(ItemCode, ItemDesc, Category, Unit, Classification, IsDeleted)
		SELECT
		ItemCode, ItemDesc, Category, Unit, Classification, 0
		FROM dbo.ItemMasterInput

		-- Step 2: Now take only unique rows
		; with itemMasterCTE(Id, rowNumber)
		AS
		(
			SELECT Id, 
			Row_NUMBER() OVER (Partition By itemCode order by ItemCode)
			FROM @ItemMasterInput
		)
		UPDATE @ItemMasterInput
		SET IsDeleted = 1
		FROM @itemMasterInput input
		INNER JOIN itemMasterCTE cte on input.Id = cte.Id
		AND cte.rowNumber > 1

		-- Step 3: Remove duplicates from in memory table
		DELETE FROM @ItemMasterInput
		WHERE IsDeleted = 1

		-- Step 4: Update existing items Data
		UPDATE dbo.[ItemMaster]
		SET 
		[ItemDesc] = ani.ItemDesc,
		[Category] = ani.Category,
		[Unit] = ani.Unit,
		[Classification] = left(ltrim(rtrim(IsNULL(ani.[Classification], ''))), 10),
		IsActive = 1
		FROM dbo.ItemMaster an
		INNER JOIN @ItemMasterInput ani on an.ItemCode = ani.ItemCode

		-- Step 5: Insert new items data
		INSERT INTO dbo.[ItemMaster]
		([ItemCode], [ItemDesc], [Category], [Unit], [Classification], IsActive)
		SELECT  
		ani.[ItemCode], ani.[ItemDesc], ani.[Category], ani.[Unit], 
		left(ltrim(rtrim(IsNULL(ani.[Classification], ''))), 10), 1
		FROM @ItemMasterInput ani
		LEFT JOIN dbo.ItemMaster an on ani.ItemCode = an.ItemCode
		WHERE an.ItemCode is null

		-- Step 6: Mark the rows as inactive in ItemMaster table
		-- A row can come again in future uploads, hence the case
		IF @IsCompleteRefresh = 1
		BEGIN
			UPDATE dbo.ItemMaster
			SET IsActive = CASE when ani.ItemCode is null then 0 else 1 END
			FROM dbo.ItemMaster an
			LEFT JOIN @itemMasterInput ani on an.ItemCode = ani.ItemCode
		END

		-- Step 7: If Complete Refresh - delete existing data in ItemMasterTypeName
		IF @IsCompleteRefresh = 1
		BEGIN
			TRUNCATE TABLE dbo.ItemMasterTypeName
		END

		-- Step 8: Update Rates in existing Rows
		UPDATE dbo.ItemMasterTypeName
		SET Rate = input.Rate,
		ReturnRate = input.ReturnRate
		FROM dbo.ItemMasterTypeName imtn
		INNER JOIN dbo.ItemMaster im on imtn.ItemMasterId = im.Id
		AND im.IsActive = 1
		INNER JOIN dbo.ItemMasterInput input on input.ItemCode = im.ItemCode
		AND input.CropName = imtn.TypeName


		-- Step 9 Create new rows in ItemMasterTypeName table
		; with cropCTE(Id, cropName, Rate, ReturnRate)
		AS
		(
			SELECT ani.Id, input.CropName, input.Rate, input.ReturnRate
			FROM dbo.ItemMasterInput input
			INNER JOIN dbo.ItemMaster ani on input.ItemCode = ani.ItemCode
			AND ani.IsActive = 1
		)
		INSERT INTO dbo.ItemMasterTypeName
		(ItemMasterId, TypeName, Rate, ReturnRate)
		SELECT c.Id, c.Cropname, c.Rate, c.ReturnRate
		FROM cropCTE c
		LEFT JOIN dbo.ItemMasterTypeName p on c.Id = p.ItemMasterId AND c.CropName = p.TypeName
		WHERE p.ItemMasterId is null


		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformItemMasterData', 'Success'
END
GO
