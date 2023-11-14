DECLARE @OfficeHierarchy TABLE
(ZoneCode NVARCHAR(10),
 AreaCode NVARCHAR(10),
 TerritoryCode NVARCHAR(10),
 HQCode NVARCHAR(10)
)
INSERT INTO @OfficeHierarchy
(ZoneCode, AreaCode, TerritoryCode, HQCode)
values
('M1', 'B29', 'D12', 'D18'),
('M1', 'B29', 'D12', 'D5'),
('M1', 'B29', 'D1', 'D2'),
('M1', 'B29', 'D1', 'D57'),
('M1', 'B29', 'D4', 'D4'),
('M1', 'B29', 'D4', 'D52'),
('M1', 'B29', 'D10', 'D1'),
('M1', 'B29', 'D10', 'D24'),
('M1', 'B29', 'D10', 'D60'),
('M1', 'B29', 'D15', 'D28'),
('M1', 'B29', 'D15', 'D15'),
('M1', 'B29', 'D14', 'D26'),
('M1', 'B29', 'D14', 'D16'),
('M1', 'B29', 'D14', 'D44'),
('M1', 'B29', 'D16', 'D30'),
('M1', 'B29', 'D16', 'D37'),
('M1', 'B29', 'D2', 'D3'),
('M1', 'B29', 'D28', 'D58'),
('M1', 'B29', 'D28', 'D59'),
('M1', 'B29', 'D28', 'D19'),
('M1', 'B29', 'D28', 'D29'),
('M1', 'B29', 'D28', 'D48'),
('M1', 'B29', 'D28', 'D9'),
('M1', 'B29', 'D28', 'D8'),
('M1', 'B29', 'D21', 'D45'),
('M1', 'B29', 'D21', 'D41'),
('M1', 'B29', 'D5', 'D22'),
('M1', 'B29', 'D5', 'D6'),
('M1', 'B29', 'D19', 'D17'),
('M1', 'B29', 'D19', 'D35'),
('M1', 'B29', 'D24', 'D49'),
('M1', 'B29', 'D24', 'D36'),
('M1', 'B30', 'D22', 'D46'),
('M1', 'B30', 'D22', 'D55'),
('M1', 'B30', 'D6', 'D7'),
('M1', 'B30', 'D6', 'D54'),
('M1', 'B30', 'D23', 'D47'),
('M1', 'B30', 'D25', 'D51'),
('M1', 'B30', 'D11', 'D14'),
('M1', 'B30', 'D11', 'D27'),
('M1', 'B30', 'D9', 'D34'),
('M1', 'B30', 'D26', 'D53'),
('M1', 'B30', 'D26', 'D40'),
('M1', 'B31', 'D8', 'D12'),
('M1', 'B31', 'D8', 'D61'),
('M1', 'B31', 'D17', 'D32'),
('M1', 'B31', 'D3', 'D42'),
('M1', 'B31', 'D3', 'D43'),
('M1', 'B31', 'D27', 'D56'),
('M1', 'B31', 'D27', 'D50'),
('M1', 'B31', 'D20', 'D38'),
('M1', 'B31', 'D20', 'D39'),
('M1', 'B31', 'D7', 'D11'),
('M1', 'B31', 'D7', 'D20'),
('M1', 'B31', 'D18', 'D33'),
('M1', 'B31', 'D18', 'D23'),
('M1', 'B31', 'D13', 'D25'),
('M1', 'B31', 'D13', 'D31')

INSERT INTO dbo.OfficeHierarchy
(ZoneCode, Areacode, TerritoryCode, HQCode)
SELECT oh1.ZoneCode, oh1.Areacode, oh1.TerritoryCode, oh1.HQCode
FROM @OfficeHierarchy oh1
LEFT JOIN OfficeHierarchy oh2 on 
   oh1.ZoneCode = oh2.ZoneCode 
   and oh1.AreaCode = oh2.Areacode
   And oh1.TerritoryCode = oh2.TerritoryCode
   AND oh1.HQCode = oh2.HQCode
WHERE oh2.ZoneCode is null

-- ignore time part for these columns in three tables
-- this is to aid the search
update dbo.[Order]
set OrderDate = CAST(OrderDate as [Date])

update dbo.ReturnOrder
set ReturnOrderDate =CAST(returnOrderDate as [Date])

update dbo.Payment
set PaymentDate =CAST(PaymentDate as [Date])



-- Sample Data for Sales Person Association
insert into dbo.SalesPersonAssociation
( StaffCode, CodeType, CodeValue, IsDeleted)
values
( '20001019', 'HeadQuarter', 'HQ1', 0),
( '20001019', 'HeadQuarter', 'HQ2', 0),
( '20001019', 'AreaOffice', 'AO1', 0),
( '20001017', 'AreaOffice', 'AO2', 0),
( '20000409', 'Zone', 'Z1', 0),
( '20000409', 'Zone', 'Z1', 1),
( '20001307', 'Territory', 'T1', 0)



-- Sep 28 2017
USE EpicCrmDb_Mini_20170712
go
Begin Try
  BEGIN TRANSACTION
  DELETE FROM dbo.CodeTable where CodeType = 'Zone'
  INSERT INTO dbo.CodeTable
  (CodeType, CodeName, CodeValue, DisplaySequence)
  SELECT Distinct 'Zone', Zone, [Zone Code], 10 
  FROM [multiplex_tracking_staging_db].dbo.ZoneareaTerritory

  DELETE FROM dbo.CodeTable where CodeType = 'AreaOffice'
  INSERT INTO dbo.CodeTable
  (CodeType, CodeName, CodeValue, DisplaySequence)
  SELECT Distinct 'AreaOffice', [Area Office], [AO Code], 10 
  FROM [multiplex_tracking_staging_db].dbo.ZoneareaTerritory

  DELETE FROM dbo.CodeTable where CodeType = 'Territory'
  INSERT INTO dbo.CodeTable
  (CodeType, CodeName, CodeValue, DisplaySequence)
  SELECT Distinct 'Territory', [Territory Name], [TR Code], 10 
  FROM [multiplex_tracking_staging_db].dbo.ZoneareaTerritory

  DELETE FROM dbo.CodeTable where CodeType = 'HeadQuarter'
  INSERT INTO dbo.CodeTable
  (CodeType, CodeName, CodeValue, DisplaySequence)
  SELECT Distinct 'HeadQuarter', [HQ Name], [HQ Code], 10 
  FROM [multiplex_tracking_staging_db].dbo.ZoneareaTerritory

  DELETE FROM dbo.OfficeHierarchy
  INSERT INTO dbo.OfficeHierarchy
  (ZoneCode, AreaCode, TerritoryCode, HQCode)
  SELECT [Zone Code], [AO Code], [TR Code], [HQ Code]
  FROM [multiplex_tracking_staging_db].dbo.ZoneareaTerritory
  Commit

  END TRY
  BEGIN CATCH
    rollback transaction;
    throw
  End CATCH

  ------------------

  DECLARE @codetable Table
(
	[CodeType] NVARCHAR(50) NOT NULL,
	[CodeName] NVARCHAR(50) NOT NULL,
	[CodeValue] NVARCHAR(50) NOT NULL,
	[DisplaySequence] INT NOT NULL,
	[IsActive] BIT NOT NULL
)

INSERT INTO @codetable
(CodeType, CodeName, CodeValue, DisplaySequence, IsActive)
values
('PaymentType', '', 'NEFT', 50, 1),
('PaymentType', '', 'RTGS', 60, 1)


INSERT INTO dbo.CodeTable
(CodeType, CodeName, CodeValue, DisplaySequence)
SELECT mct.CodeType, mct.codeName, mct.CodeValue, mct.DisplaySequence
FROM @codetable mct
LEFT JOIN dbo.CodeTable ct on mct.CodeType = ct.CodeType
    AND mct.CodeName = ct.CodeName
	AND mct.CodeValue = ct.CodeValue
WHERE ct.CodeType IS NULL
