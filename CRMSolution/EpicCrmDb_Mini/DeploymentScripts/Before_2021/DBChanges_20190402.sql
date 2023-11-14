Alter table dbo.Entity
ADD [AgreementCount] INT NOT NULL DEFAULT 0
go

CREATE TABLE [dbo].[WorkflowSeason]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[SeasonName] NVARCHAR(50) NOT NULL,
	[TypeName] NVARCHAR(50) NOT NULL, -- could be crop name
	[StartDate] DATE NOT NULL,
	[EndDate] DATE NOT NULL,
	[IsOpen] BIT NOT NULL DEFAULT 1,
	[ReferenceId] NVARCHAR(50) NULL, -- not used now
	[Description] NVARCHAR(128) NOT NULL DEFAULT '',
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
)
GO

CREATE TABLE [dbo].[EntityAgreement]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[EntityId] BIGINT NOT NULL REFERENCES [Entity]([Id]), 
	[WorkflowSeasonId]  BIGINT NOT NULL REFERENCES [WorkFlowSeason]([Id]), 
	[AgreementNumber] NVARCHAR(50) NOT NULL,
	[Status] NVARCHAR(10) NOT NULL DEFAULT 'Pending',

	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[CreatedBy] NVARCHAR(50) NOT NULL,
	[UpdatedBy] NVARCHAR(50) NOT NULL
)
GO

INSERT INTO dbo.WorkflowSeason
(SeasonName, TypeName, StartDate, EndDate, IsOpen)
values
('Gherkin_0419', 'Gherkin', '2019-04-02', '2099-12-31', 1)
go

INSERT INTO dbo.CodeTable
(CodeType, CodeName, CodeValue, DisplaySequence, IsActive)
values
('AgreementStatus', '', 'Pending', 10, 1),
('AgreementStatus', '', 'Approved', 20, 1),
('AgreementStatus', '', 'Terminated', 30, 1)

go

CREATE TABLE [dbo].[ItemMaster]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[ItemCode] NVARCHAR(100) NOT NULL,
	[ItemDesc] NVARCHAR(100) NOT NULL,
	[Category] NVARCHAR(50) NOT NULL,
	[Unit]     NVARCHAR(10) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT 1
)
GO

-- table modified on 24.4.19 - added entityId
CREATE TABLE [dbo].[IssueReturn]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[EmployeeId] BIGINT NOT NULL REFERENCES TenantEmployee,
	[DayId] BIGINT NOT NULL REFERENCES dbo.[Day],
	[EntityId] BIGINT NOT NULL REFERENCES dbo.Entity,
	[EntityAgreementId] BIGINT NOT NULL DEFAULT 0, 
	[ItemMasterId] BIGINT NOT NULL REFERENCES dbo.ItemMaster,
	[TransactionDate] DATE NOT NULL,
	[TransactionType] NVARCHAR(50) NOT NULL, -- Issue/Return/Abandoned
	[Quantity] INT NOT NULL,
	[ActivityId] BIGINT NOT NULL,
	[SqliteIssueReturnId] BIGINT NOT NULL DEFAULT 0,
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
)
GO

INSERT INTO dbo.CodeTable
(CodeType, CodeName, CodeValue, DisplaySequence, IsActive)
values
('TransactionType', '', 'Issue', 10, 1),
('TransactionType', '', 'Return', 20, 1),
('TransactionType', '', 'Abandoned', 30, 1)
go

INSERT INTO dbo.CodeTable
(CodeType, CodeName, CodeValue, DisplaySequence, IsActive)
values
('ItemType', '', 'Fertilizers', 10, 1),
('ItemType', '', 'Pesticides', 20, 1),
('ItemType', '', 'Seeds', 30, 1),
('ItemType', '', 'Others', 40, 1)
go


INSERT INTO dbo.itemMaster
(ItemCode, ItemDesc, Category,Unit)
values
('10 26 26','10 26 26','Fertilizers','KGS'),
('12 32 16','12 32 16','Fertilizers','KGS'),
('13 40 13 NPK KG','13 40 13 NPK KG','Fertilizers','KGS'),
('14 06 21','14 06 21','Fertilizers','KGS'),
('14 35 14','14 35 14','Fertilizers','KGS'),
('16 20 0 13','16 20 0 13','Fertilizers','KGS'),
('17 17 17','17 17 17','Fertilizers','KGS'),
('17 17 17-Vijay','17 17 17-Vijay','Fertilizers','KGS'),
('19 19 19','19 19 19','Fertilizers','KGS'),
('19 19 19 FERTICARE KG','19 19 19 FERTICARE KG','Fertilizers','KGS'),
('19 19 19 SPRAY','19 19 19 SPRAY','Fertilizers','KGS'),
('19 19 19 WS','19 19 19 WS','Fertilizers','KGS'),
('20 20 0','20 20 0','Fertilizers','KGS'),
('20 20 0 13','20 20 0 13','Fertilizers','KGS'),
('20 20 0 16','20 20 0 16','Fertilizers','KGS'),
('28 28 0','28 28 0','Fertilizers','KGS'),
('ABACIN 100ML','ABACIN 100ML','Pesticides','BTL'),
('ABC (Abamectin) 100ml','ABC (Abamectin) 100ml','Pesticides','BTL'),
('ACROBAT 100G','ACROBAT 100G','Pesticides','PKT'),
('ACROBAT 200G','ACROBAT 200G','Pesticides','PKT'),
('ACROBAT 20G','ACROBAT 20G','Pesticides','PKT'),
('ADMIRE 2 G','ADMIRE 2 G','Pesticides','PKT'),
('ADMIRE 30G','ADMIRE 30G','Pesticides','PKT'),
('AGRI-40MM PVC PIPE','AGRI-40MM PVC PIPE','Others','FT'),
('AGRI-AIR FILTER ELEMENT (DRY)','AGRI-AIR FILTER ELEMENT (DRY)','Others','NOS'),
('Agri-Charging Alternator Belt','Agri-Charging Alternator Belt','Others','NOS'),
('AGRI-Coupler 63mm','AGRI-Coupler 63mm','Others','NOS'),
('AGRI-DG B''CHECK KIT','AGRI-DG B''CHECK KIT','Others','NOS'),
('AGRI-Elbow 63mm','AGRI-Elbow 63mm','Others','NOS'),
('Agri-Engine Lubricating oil Powergen Excel-Volvoline','Agri-Engine Lubricating oil Powergen Excel-Volvoline','Others','NOS'),
('Agri-Engine lubricating oil SAE 15 W 40 (3Ltr can)','Agri-Engine lubricating oil SAE 15 W 40 (3Ltr can)','Others','NOS'),
('Agri-Engine lubricating oil SAE 15 W 40 (5Ltr can)','Agri-Engine lubricating oil SAE 15 W 40 (5Ltr can)','Others','NOS'),
('AGRI-FILTER FOR AIR CLEANER','AGRI-FILTER FOR AIR CLEANER','Others','NOS'),
('Agri-Fuel Filter Element Kit','Agri-Fuel Filter Element Kit','Others','SET'),
('Agri-Generator Safety Unit - (GSU)','Agri-Generator Safety Unit - (GSU)','Others','NOS'),
('AGRI-Grading Sieve 10+ 40mm Yellow','AGRI-Grading Sieve 10+ 40mm Yellow','Others','NOS'),
('AGRI-Grading Sieve 10+ 48mm Blue','AGRI-Grading Sieve 10+ 48mm Blue','Others','NOS'),
('AGRI-Grading Sieve 20+ 30mm Yellow','AGRI-Grading Sieve 20+ 30mm Yellow','Others','NOS'),
('AGRI-Grading Sieve 300+ 12mm Blue','AGRI-Grading Sieve 300+ 12mm Blue','Others','NOS'),
('AGRI-GSU UNIT','AGRI-GSU UNIT','Others','SET'),
('Agri-Hose Inlet Radiator','Agri-Hose Inlet Radiator','Others','NOS'),
('Agri-Hose Radiator Outlet','Agri-Hose Radiator Outlet','Others','NOS'),
('Agri-Motor pump 15 Stage 7.5 HP','Agri-Motor pump 15 Stage 7.5 HP','Others','NOS'),
('"AGRI-MS Twisted Steel Rod 18mm 27"" Length"','"AGRI-MS Twisted Steel Rod 18mm 27"" Length"','Others','NOS'),
('AGRI-PVC 40mm/6BAR','AGRI-PVC 40mm/6BAR','Others','MTRS'),
('Agri-Radiator Coolant-Concentrate','Agri-Radiator Coolant-Concentrate','Others','NOS'),
('Agri-Radiator Fan Belt','Agri-Radiator Fan Belt','Others','NOS'),
('Agri-Rubber stamp','Agri-Rubber stamp','Others','NOS'),
('AGRI-Solvent Cement','AGRI-Solvent Cement','Others','LTR'),
('Agri-Spin on Lube oil filter - Red','Agri-Spin on Lube oil filter - Red','Others','NOS'),
('AGRI-Stock Register Book','AGRI-Stock Register Book','Others','NOS'),
('AGRI-Tee 63mm','AGRI-Tee 63mm','Others','NOS'),
('AGRI-VAF METER','AGRI-VAF METER','Others','SET'),
('Agri-Visiting cards','Agri-Visiting cards','Others','CARDS'),
('AJAX','AJAX','Seeds','NOS'),
('AJAX UNTREATED','AJAX UNTREATED','Seeds','NOS'),
('ALANTO 100ml','ALANTO 100ml','Pesticides','BTL'),
('ALIETTE 100G','ALIETTE 100G','Pesticides','PKT'),
('ALIETTE 250G','ALIETTE 250G','Pesticides','PKT'),
('ALIKA 40ml','ALIKA 40ml','Pesticides','BTL'),
('ALLWIN GOLD 100ML','ALLWIN GOLD 100ML','Pesticides','BTL'),
('ALLWIN GOLD 250ML','ALLWIN GOLD 250ML','Pesticides','ML'),
('ALLWIN GOLD 500ML','ALLWIN GOLD 500ML','Pesticides','ML'),
('AMAZE 100G','AMAZE 100G','Pesticides','PKT'),
('AMAZE 200G','AMAZE 200G','Pesticides','PKT'),
('AMINO GOLD 250ML','AMINO GOLD 250ML','Pesticides','BTL'),
('AMINO GOLD 500ML','AMINO GOLD 500ML','Pesticides','BTL'),
('AMINOVITA 100 ML','AMINOVITA 100 ML','Others','BTL'),
('AMISTAR 200ML','AMISTAR 200ML','Pesticides','BTL'),
('AMISTAR 50ML','AMISTAR 50ML','Pesticides','BTL'),
('Ammonium Acetate 500g','Ammonium Acetate 500g','Pesticides','GMS'),
('AMMONIUM NITRATE KG','AMMONIUM NITRATE KG','Fertilizers','PKT'),
('AMMONIUM POLY BORATE 250GM','AMMONIUM POLY BORATE 250GM','Fertilizers','PKT'),
('AMMONIUM SULPHATE KG','AMMONIUM SULPHATE KG','Fertilizers','KGS'),
('ANAXO(N)','ANAXO(N)','Seeds','NOS'),
('AVANA 500G','AVANA 500G','Pesticides','PKT'),
('AVANA KG','AVANA KG','Pesticides','KGS'),
('AVAUNT 100ML','AVAUNT 100ML','Pesticides','BTL'),
('AVAUNT 200ML','AVAUNT 200ML','Pesticides','BTL'),
('AVONE 100ML','AVONE 100ML','Pesticides','BTL'),
('AZOTOBACTOR 200G','AZOTOBACTOR 200G','Others','PKT'),
('BABY CORN CUTTER','BABY CORN CUTTER','Others','NOS'),
('BABY CORN SEEDS 5414 VARIETY','BABY CORN SEEDS 5414 VARIETY','Seeds','KGS'),
('BABY CORN SEEDS 5417 VARIETY','BABY CORN SEEDS 5417 VARIETY','Seeds','KGS'),
('BABY CORN SEEDS CP.B468 VARIETY','BABY CORN SEEDS CP.B468 VARIETY','Seeds','KGS'),
('BABY CORN SEEDS CP.B472 VARIETY','BABY CORN SEEDS CP.B472 VARIETY','Seeds','KGS'),
('BACILLUS SUBTILIS 1L','BACILLUS SUBTILIS 1L','Pesticides','BTL'),
('BACILUX 250g','BACILUX 250g','Pesticides','PKT'),
('BACTRINASHAK 20G','BACTRINASHAK 20G','Pesticides','PKT'),
('Bajra seeds 1kg','Bajra seeds 1kg','Seeds','KGS'),
('Banana pepper seeds 1Kg','Banana pepper seeds 1Kg','Seeds','KGS'),
('Basta 1L','Basta 1L','Pesticides','LTR'),
('BAYLETON 500G','BAYLETON 500G','Pesticides','PKT'),
('BELT EXPERT 50ml','BELT EXPERT 50ml','Pesticides','BTL'),
('BILZEB 1KG','BILZEB 1KG','Pesticides','PKT'),
('BILZEB 500G','BILZEB 500G','Pesticides','PKT'),
('Bio peat SG Kgs','Bio peat SG Kgs','Others','KGS'),
('BIOSTEIN 100ML','BIOSTEIN 100ML','Pesticides','BTL'),
('Biozyme 100ml','Biozyme 100ml','Pesticides','BTL'),
('BIOZYME 250ML','BIOZYME 250ML','Pesticides','BTL'),
('BIOZYME GRANULES 1Kg','BIOZYME GRANULES 1Kg','Fertilizers','KGS'),
('BLITOX 500G','BLITOX 500G','Pesticides','PKT'),
('BLUE BOARD','BLUE BOARD','Others','NOS'),
('BOON 100g','BOON 100g','Pesticides','PKT'),
('BORAN 1KG','BORAN 1KG','Pesticides','KGS'),
('BRISK 500 ml','BRISK 500 ml','Pesticides','BTL'),
('CABRIO TOP 300G','CABRIO TOP 300G','Pesticides','PKT'),
('CALIGAURD 250ML','CALIGAURD 250ML','Pesticides','BTL'),
('CALPHOMIL 250ML','CALPHOMIL 250ML','Pesticides','BTL'),
('CALYPSO SEEDS G','CALYPSO SEEDS G','Seeds','GMS'),
('CAN','CAN','Fertilizers','KGS'),
('CAP -Crates 600X400X275 SCH ORANGE','CAP -Crates 600X400X275 SCH ORANGE','Others','NOS'),
('CAP- Crates PC -714 STP EB BROWN','CAP- Crates PC -714 STP EB BROWN','Others','NOS'),
('CAP-Crates PC -714 STP EB BLACK','CAP-Crates PC -714 STP EB BLACK','Others','NOS'),
('CAP-Crates PC -714 STPEB ORANGE','CAP-Crates PC -714 STP EB ORANGE','Others','NOS'),
('CAPEX-Drip materials','CAPEX-Drip materials','Others','SET'),
('Capex-Iron Door With Window','Capex-Iron Door With Window','Others','NOS'),
('CAPEX-Lateral Pipe','CAPEX-Lateral Pipe','Others','MTRS'),
('Capex-MS Pipe','Capex-MS Pipe','Others','NOS'),
('CAPEX-Stone Chips 12MM (280x2=560sqft)','CAPEX-Stone Chips 12MM (280x2=560sqft)','Others','SFT'),
('Capex-Zinc Sheet','Capex-Zinc Sheet','Others','NOS'),
('CASTOR OIL 1L','CASTOR OIL 1L','Others','PKT'),
('Chalcan 1kg','Chalcan 1kg','Fertilizers','KGS'),
('CHANDINI (RZ 12-79) SEEDS','CHANDINI (RZ 12-79) SEEDS','Seeds','NOS'),
('CNO3 (FG) KG','CNO3 (FG) KG','Fertilizers','KGS'),
('CNO3 FOLIAR KG','CNO3 FOLIAR KG','Fertilizers','KGS'),
('CNO3 KEMIRA KG','CNO3 KEMIRA KG','Fertilizers','KGS'),
('COCONUT PITH COMPOST','COCONUT PITH COMPOST','Fertilizers','KGS'),
('COMPOST','COMPOST','Fertilizers','KGS'),
('CONFIDOR 100ML','CONFIDOR 100ML','Pesticides','BTL'),
('CONFIDOR 50ML','CONFIDOR 50ML','Pesticides','BTL'),
('CONFIDOR SUPER 100ML','CONFIDOR SUPER 100ML','Pesticides','BTL'),
('CONFIDOR SUPER 50ML','CONFIDOR SUPER 50ML','Pesticides','BTL'),
('CONTAF 100ML','CONTAF 100ML','Pesticides','BTL'),
('Coragen 10 ml','Coragen 10 ml','Pesticides','BTL'),
('COSAMIL DF 1Kg','COSAMIL DF 1Kg','Fertilizers','KGS'),
('Cotton Knitted Hand Gloves','Cotton Knitted Hand Gloves','Others','PAIR'),
('CRATES - BABY CORN','CRATES - BABY CORN','Others','NOS'),
('CURZATE 100G','CURZATE 100G','Pesticides','PKT'),
('CURZATE 300G','CURZATE 300G','Pesticides','GMS'),
('DAP','DAP','Fertilizers','KGS'),
('DAP IPL','DAP IPL','Fertilizers','KGS'),
('DAP Spic','DAP Spic','Fertilizers','KGS'),
('DECIS 250ML','DECIS 250ML','Pesticides','BTL'),
('DECIS 500ML','DECIS 500ML','Pesticides','BTL'),
('DECIS EC100 100ML','DECIS EC100 100ML','Pesticides','BTL'),
('DECIS EC100 50ML','DECIS EC100 50ML','Pesticides','BTL'),
('Delegate 100ml','Delegate 100ml','Pesticides','BTL'),
('Delfin 50g','Delfin 50g','Pesticides','PKT'),
('DELTA LINER NO','DELTA LINER NO','Others','NOS'),
('DELTA TRAP NO','DELTA TRAP NO','Others','NOS'),
('DICOFOL 250ML','DICOFOL 250ML','Pesticides','BTL'),
('DRIP CLEAN 250ML','DRIP CLEAN 250ML','Pesticides','BTL'),
('Drip-Ball Valve & Flush Valve','Drip-Ball Valve & Flush Valve','Others','EACH'),
('Drip-Filter','Drip-Filter','Others','NOS'),
('DRIP-KB DRIP 125MICRON PREPUNCH-1.5Ft','DRIP-KB DRIP 125MICRON PREPUNCH-1.5Ft','Others','KGS'),
('DRIP-KB GROMMET - 16 mm','DRIP-KB GROMMET - 16 mm','Others','NOS'),
('DRIP-KB JOINER - 16mm','DRIP-KB JOINER - 16mm','Others','NOS'),
('DRIP-KB TALEE OH - 16mm','DRIP-KB TALEE OH - 16mm','Others','NOS'),
('DRIP-LATERAL - 16mm','DRIP-LATERAL - 16mm','Others','MTRS'),
('Drip-Main Pipe','Drip-Main Pipe','Others','MTRS'),
('DRIP-MAIN PIPE 75MM','DRIP-MAIN PIPE 75MM','Others','MTRS'),
('Drip-Online Lateral','Drip-Online Lateral','Others','MTRS'),
('Drip-Online Lateral Joiner','Drip-Online Lateral Joiner','Others','NOS'),
('Drip-Plain Lateral','Drip-Plain Lateral','Others','MTRS'),
('Drip-Plain to Online Lateral Joiner','Drip-Plain to Online Lateral Joiner','Others','NOS'),
('Drip-PVCFittings Acc & Instl.','Drip-PVCFittings Acc & Instl.','Others','Per Plot'),
('Drip-Sand Filter','Drip-Sand Filter','Others','EACH'),
('Drip-Start End & Grommat','Drip-Start End & Grommat','Others','NOS'),
('Drip-Sub Main Pipe','Drip-Sub Main Pipe','Others','MTRS'),
('Drip-Ventury Main fold + Assembly','Drip-Ventury Main fold + Assembly','Others','SET'),
('DYNAMITE 100ML','DYNAMITE 100ML','Pesticides','BTL'),
('DYNAMITE 50ML','DYNAMITE 50ML','Pesticides','BTL'),
('ECOHUME 500ML','ECOHUME 500ML','Pesticides','BTL'),
('ECONEEM PLUS 1000ML','ECONEEM PLUS 1000ML','Pesticides','BTL'),
('ECOSOM-TH 1L','ECOSOM-TH 1L','Pesticides','BTL'),
('ECOSOM-TV 1L','ECOSOM-TV 1L','Pesticides','BTL'),
('ENTOMO PATHOGEIC NEMATODE','ENTOMO PATHOGEIC NEMATODE','Others','PKT'),
('FAME 10ML','FAME 10ML','Pesticides','BTL'),
('FAME 50ML','FAME 50ML','Pesticides','BTL'),
('FANTAC 100ML','FANTAC 100ML','Pesticides','BTL'),
('FANTAC 50ML','FANTAC 50ML','Pesticides','BTL'),
('FENNEL TRAPS','FENNEL TRAPS','Others','NOS'),
('FERTIBOR 250G','FERTIBOR 250G','Fertilizers','GMS'),
('FERTIBOR KG','FERTIBOR KG','Others','KGS'),
('Fitrest 50g','Fitrest 50g','Pesticides','PKT'),
('FORTENZA DUO 32ml','FORTENZA DUO 32ml','Pesticides','BTL'),
('FRUIT FLY LURES NO','FRUIT FLY LURES NO','Others','NOS'),
('FRUIT FLY TRAPS NO','FRUIT FLY TRAPS NO','Others','NOS'),
('GAUCHO 100ML','GAUCHO 100ML','Pesticides','BTL'),
('GAUCHO 50ML','GAUCHO 50ML','Pesticides','BTL'),
('GAUCHO FS 9ML','GAUCHO FS 9ML','Pesticides','BTL'),
('GENARAL LIQUID 1000ML','GENARAL LIQUID 1000ML','Pesticides','BTL'),
('GI WIRE 12 GAUGE KG','GI WIRE 12 GAUGE KG','Others','KGS'),
('GI WIRE 14 GAUGE KG','GI WIRE 14 GAUGE KG','Others','KGS'),
('GI WIRE 16 GAUGE KG','GI WIRE 16 GAUGE KG','Others','KGS'),
('GI WIRE 18 GAUGE KG','GI WIRE 18 GAUGE KG','Others','KGS'),
('GLYSAAN 1L','GLYSAAN 1L','Pesticides','LTR'),
('HELI LURES','HELI LURES','Others','NOS'),
('HUMIFOL 1L','HUMIFOL 1L','Pesticides','BTL'),
('HUMISTAR 500ML','HUMISTAR 500ML','Pesticides','BTL'),
('Indtron 1L','Indtron 1L','Pesticides','LTR'),
('Isabion 1l','Isabion 1l','Pesticides','BTL'),
('Isabion 250ml','Isabion 250ml','Pesticides','BTL'),
('JAGGERY 1Kg','JAGGERY 1Kg','Others','KGS'),
('JALAPENO SEEDLING 1No.','JALAPENO SEEDLING 1No.','Seeds','NOS'),
('JALAPENO SEEDLING-MITLA','JALAPENO SEEDLING-MITLA','Seeds','NOS'),
('JALAPENO-IMPERIAL SEEDS 1Kg','JALAPENO-IMPERIAL SEEDS 1Kg','Seeds','KGS'),
('JALAPENO-JALA-X SEEDS 1Kg','JALAPENO-JALA-X SEEDS 1Kg','Seeds','KGS'),
('JALAPENO-MITLA','JALAPENO-MITLA','Seeds','KGS'),
('JALAPENO-SOLEIL (PEP-4) seeds 1Kg','JALAPENO-SOLEIL (PEP-4) seeds 1Kg','Seeds','KGS'),
('Jatayu 100g','Jatayu 100g','Pesticides','PKT'),
('Jatayu 250g','Jatayu 250g','Pesticides','PKT'),
('JUTE THREAD KG','JUTE THREAD KG','Others','KGS'),
('KARATE 250ML','KARATE 250ML','Pesticides','BTL'),
('KARATE 500ML','KARATE 500ML','Pesticides','BTL'),
('KAVACH 100G','KAVACH 100G','Pesticides','PKT'),
('KAVACH 250G','KAVACH 250G','Pesticides','PKT'),
('Keerthi','Keerthi','Seeds','NOS'),
('KEICITE 500ML','KEICITE 500ML','Pesticides','BTL'),
('KEMIRA POTASSIUM NITRATE KG','KEMIRA POTASSIUM NITRATE KG','Fertilizers','KGS'),
('LAB-Nitric Acid 70% 2.5 ltr','LAB-Nitric Acid 70% 2.5 ltr','Others','CAN'),
('LANNATE 100G','LANNATE 100G','Pesticides','PKT'),
('LANNATE 250G','LANNATE 250G','Pesticides','PKT'),
('Larvin 100g','Larvin 100g','Pesticides','PKT'),
('LIBRA COMBI-2 100G','LIBRA COMBI-2 100G','Fertilizers','PKT'),
('LM OIL 100ML','LM OIL 100ML','Pesticides','BTL'),
('Luna Experience 100ml','Luna Experience 100ml','Pesticides','BTL'),
('LURE','LURE','Others','NOS'),
('MAIZE SEEDS','MAIZE SEEDS','Seeds','KGS'),
('MANIK 100g','MANIK 100g','Others','PKT'),
('MANIK 50G','MANIK 50G','Pesticides','PKT'),
('MANZATE 100G','MANZATE 100G','Pesticides','PKT'),
('MANZATE 250G','MANZATE 250G','Pesticides','PKT'),
('MANZATE 500G','MANZATE 500G','Pesticides','PKT'),
('MAP FOLIAR KG','MAP FOLIAR KG','Fertilizers','KGS'),
('MAPLE EM 1000ML','MAPLE EM 1000ML','Pesticides','BTL'),
('MATADOR 250ML','MATADOR 250ML','Pesticides','BTL'),
('MGSO4 KG','MGSO4 KG','Fertilizers','KGS'),
('MICMOLL-V 250g','MICMOLL-V 250g','Fertilizers','GMS'),
('MICNELF 500G','MICNELF 500G','Pesticides','PKT'),
('MKP FOLIAR KG','MKP FOLIAR KG','Fertilizers','KGS'),
('MONITOR (Th) 250g','MONITOR (Tv)','Pesticides','PKT'),
('MONITOR 15g','MONITOR 15g','Pesticides','GMS'),
('MONO POTASSIUM PHOSPHATE (MKP)KG','MONO POTASSIUM PHOSPHATE (MKP)KG','Fertilizers','KGS'),
('MONOFILAMENT YARN 1SPOOL','MONOFILAMENT YARN 1SPOOL','Others','BDL'),
('MONOSAAN 500ML','MONOSAAN 500ML','Pesticides','BTL'),
('MOP','MOP','Fertilizers','KGS'),
('MOP (ORGANIC)','MOP (ORGANIC)','Fertilizers','KGS'),
('Movento energy 100ml','Movento energy 100ml','Pesticides','BTL'),
('Movento OD 250ml','Movento OD 250ml','Pesticides','BTL'),
('MULCH FILM KG','MULCH FILM KG','Others','KGS'),
('MULTI FLOWER 500ml','MULTI FLOWER 500ml','Fertilizers','BTL'),
('MYCOZONE 100g','MYCOZONE 100g','Pesticides','PKT'),
('NATIVO 50gm','NATIVO 50gm','Pesticides','PKT'),
('NEEMAZAL 1000ML','NEEMAZAL 1000ML','Pesticides','BTL'),
('NEEMAZAL 250ML','NEEMAZAL 250ML','Pesticides','BTL'),
('NEEMAZAL 500ML','NEEMAZAL 500ML','Pesticides','BTL'),
('Neemazal F 250ml','Neemazal F 250ml','Pesticides','BTL'),
('NEEMCAKE','NEEMCAKE','Fertilizers','KGS'),
('Nitric Acid Kg','Nitric Acid Kg','Pesticides','KGS'),
('NO MATE LURES','NO MATE LURES','Others','NOS'),
('NO MATE TRAPS','NO MATE TRAPS','Others','NOS'),
('OBERON 100ML','OBERON 100ML','Pesticides','BTL'),
('OBERON 50ML','OBERON 50ML','Pesticides','BTL'),
('ODIN 100G','ODIN 100G','Pesticides','PKT'),
('ODIN 200G','ODIN 200G','Pesticides','GMS'),
('PACEILO 1L','PACEILO 1L','Pesticides','BTL'),
('PETAL 10G','PETAL 10G','Others','PKT'),
('PHOSPHATE SALUBILIZER BACTERIA 1L','PHOSPHATE SALUBILIZER BACTERIA 1L','Pesticides','BTL'),
('PHOSPHATE SALUBILIZER BACTERIA 200G','PHOSPHATE SALUBILIZER BACTERIA 200G','Others','PKT'),
('PHOSPHORIC ACID','PHOSPHORIC ACID','Others','KGS'),
('Piripiri Bull Horn 1Kg','Piripiri Bull Horn 1Kg','Seeds','KGS'),
('Piripiri Demon 1Kg','Piripiri Demon 1Kg','Seeds','KGS'),
('Piripiri seedling 1No','Piripiri seedling 1No','Seeds','NOS'),
('PK40-500ML','PK40-500ML','Pesticides','BTL'),
('PLASTIC BAGS','PLASTIC BAGS','Others','NOS'),
('PLASTIC BAGS NETTED','PLASTIC BAGS NETTED','Others','NOS'),
('PLASTIC TUB','PLASTIC TUB','Others','NOS'),
('PLASTIC WIRE KG','PLASTIC WIRE KG','Others','KGS'),
('POLYMER TAPE Kg','POLYMER TAPE Kg','Others','KGS'),
('POTASSIUM NITRATE FOLIAR KG','POTASSIUM NITRATE FOLIAR KG','Fertilizers','KGS'),
('POWDERYCARE 500g','POWDERYCARE 500g','Pesticides','PKT'),
('PREPARE 1000L','PREPARE 1000L','Pesticides','BTL'),
('PRIDE 100G','PRIDE 100G','Pesticides','BTL'),
('Profiler 250g','Profiler 250g','Pesticides','PKT'),
('PROMAX','PROMAX','Fertilizers','KGS'),
('PROTEIN BAIT 1000ML','PROTEIN BAIT 1000ML','Pesticides','BTL'),
('PROTEIN BAIT 500ML','PROTEIN BAIT 500ML','Pesticides','BTL'),
('RED PAPRIKA','RED PAPRIKA','Seeds','GMS'),
('RED PAPRIKA SEEDLING 1No.','RED PAPRIKA SEEDLING 1No.','Seeds','NOS'),
('REXOLIN 100g','REXOLIN 100g','Pesticides','PKT'),
('REXOLIN 250g','REXOLIN 250g','Pesticides','PKT'),
('RIDOMIL 100g','RIDOMIL 100g','Pesticides','PKT'),
('ROCK PHOSPHATE KG','ROCK PHOSPHATE KG','Fertilizers','KGS'),
('ROKO 100g','ROKO 100g','Pesticides','PKT'),
('ROKO 1Kg','ROKO 1Kg','Pesticides','PKT'),
('SAFETY DRESS SET','SAFETY DRESS SET','Others','SET'),
('SALT 1KG','SALT 1KG','Others','KGS'),
('SECTIN 100G','SECTIN 100G','Pesticides','PKT'),
('SECTIN 250G','SECTIN 250G','Pesticides','PKT'),
('SECTIN 600G','SECTIN 600G','Pesticides','PKT'),
('SECURE','SECURE','Seeds','NOS'),
('SHAKTHI','SHAKTHI','Seeds','NOS'),
('SHEATHGUARD 1L','SHEATHGUARD 1L','Pesticides','BTL'),
('SHUBODAYA KG','SHUBODAYA KG','Others','KGS'),
('SILWET-100ML','SILWET-100ML','Pesticides','BTL'),
('SOLANIA NET','SOLANIA NET','Others','MTRS'),
('SOLOMON 100 ML','SOLOMON 100 ML','Pesticides','BTL'),
('SOLUBOR 250g','SOLUBOR 250g','Fertilizers','PKT'),
('SOLUBOR KG','SOLUBOR KG','Fertilizers','KGS'),
('SOMGUARD SILICOP 500ml','SOMGUARD SILICOP 500ml','Pesticides','BTL'),
('SPARTA','SPARTA','Seeds','NOS'),
('SPIC CYTOZYME 250 ML','SPIC CYTOZYME 250 ML','Pesticides','BTL'),
('SPINOSAD 75ML','SPINOSAD 75ML','Pesticides','BTL'),
('SPINTOR 75 ML','SPINTOR 75 ML','Pesticides','BTL'),
('SPINTOR 7ml','Spintor 7ml','Pesticides','PKT'),
('SPODO LURE','SPODO LURE','Others','NOS'),
('SPRAYER OMEGA','SPRAYER OMEGA','Others','NOS'),
('SPRAYER PUMP','SPRAYER PUMP','Others','NOS'),
('SPRAYER-AGRI MATE 2L','SPRAYER-AGRI MATE 2L','Others','NOS'),
('SPRAYER-ASPEE BATTERY OPERATED','SPRAYER-ASPEE BATTERY OPERATED','Others','NOS'),
('SPRAYER-ASPEE BOLO','SPRAYER-ASPEE BOLO','Others','NOS'),
('SPRAYER-KNAPSACK','SPRAYER-KNAPSACK','Others','NOS'),
('SPREADMAX 100ML','SPREADMAX 100ML','Others','BTL'),
('SPREADMAX 5ML','SPREADMAX 5ML','Others','BTL'),
('STARTHENE 250G','STARTHENE 250G','Pesticides','PKT'),
('SU YASH - OME 500ml','SU YASH - OME 500ml','Fertilizers','BTL'),
('SUDOZONE 500g','SUDOZONE 500G','Pesticides','PKT'),
('SULPHUR 500g','SULPHUR 500g','Pesticides','PKT'),
('SWEET BANANA PEPPER SEEDLING 1No.','SWEET BANANA PEPPER SEEDLING 1No.','Seeds','NOS'),
('SWEET BANANA PEPPER-Ed 50g','SWEET BANANA PEPPER-Ed 50g','Seeds','PKT'),
('TAGMEC 100 ML','TAGMEC 100 ML','Pesticides','BTL'),
('TOPAS 100ML','TOPAS 100ML','Pesticides','BTL'),
('TRACEL 500g','TRACEL 500g','Pesticides','PKT'),
('TRACER 75ML','TRACER 75ML','Pesticides','BTL'),
('TRAP NO','TRAP NO','Others','NOS'),
('TRICODERMA KG','TRICODERMA KG','Others','KGS'),
('UREA','UREA','Fertilizers','KGS'),
('Urea 45 kg','Urea 45 kg','Fertilizers','KGS'),
('VANGUARD 250ml','VANGUARD 250ml','Pesticides','BTL'),
('VARAM 250ml','VARAM 250ml','Pesticides','BTL'),
('VERMI-COMPOST','VERMI-COMPOST','Fertilizers','KGS'),
('Verno-Bt Zyme 500ml','Verno-Bt Zyme 500ml','Pesticides','BTL'),
('VERTICILLIUM CHLAMYDOSPORIUM 1L','VERTICILLIUM CHLAMYDOSPORIUM 1L','Pesticides','LTR'),
('VERTICILLIUM LECANII 1L','VERTICILLIUM LECANII 1L','Pesticides','BTL'),
('VERTINA SEEDS','VERTINA SEEDS','Seeds','NOS'),
('VIVA 666 1L','VIVA 666 1L','Pesticides','BTL'),
('VLASSTER G','VLASSTER G','Seeds','GMS'),
('V-MEC 100ml','V-MEC 100ml','Pesticides','BTL'),
('WAPKIL 50G','WAPKIL 50G','Pesticides','PKT'),
('Water lock 250g','Water lock 250g','Others','PKT'),
('WHITE MOP','WHITE MOP','Fertilizers','KGS'),
('YELLOW BOARD','YELLOW BOARD','Others','NOS'),
('YORKER 250g','YORKER 250g','Pesticides','PKT'),
('Zampro 400ml','Zampro 400ml','Pesticides','BTL'),
('ZINC PHASPETE 10G','ZINC PHASPETE 10G','Pesticides','PKT'),
('ZINC SULPHATE KG','ZINC SULPHATE KG','Fertilizers','KGS'),
('ZINK SUPER POWER KG','ZINK SUPER POWER KG','Fertilizers','KGS')
go

-- April 5 2019
ALTER TABLE [dbo].[SqliteActionBatch]
ADD
	[NumberOfIssueReturns] BIGINT NOT NULL DEFAULT 0,
	[NumberOfIssueReturnsSaved] BIGINT NOT NULL DEFAULT 0
GO

CREATE TABLE [dbo].[SqliteIssueReturn]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[BatchId] BIGINT NOT NULL REFERENCES dbo.SqliteActionBatch,
	[EmployeeId] BIGINT NOT NULL,

	[EntityId] BIGINT NOT NULL,
	[EntityName] NVARCHAR(50) NOT NULL,
	[AgreementId] BIGINT NOT NULL DEFAULT 0,
	[Agreement] NVARCHAR(50) NOT NULL,
	[TranType] NVARCHAR(50) NOT NULL,
	[ItemId] BIGINT NOT NULL,
	[ItemCode] NVARCHAR(100) NOT NULL,
	[Quantity] INT NOT NULL,
	[IssueReturnDate] DATETIME2 NOT NULL,

	[IsProcessed] BIT NOT NULL DEFAULT 0,  
	[IssueReturnId] BIGINT NOT NULL DEFAULT 0,
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
)
GO

-- set up data in code Table for CustomerType to be only Farmer.

-- set up activity type
update dbo.CodeTable
set IsActive = 0
where codeType = 'ActivityType'

insert into dbo.codeTable
(CodeType, CodeName, CodeValue, DisplaySequence)
values
('ActivityType', '', 'Input Issue', 10),
('ActivityType', '', 'Input Return', 20)
go


CREATE PROCEDURE [dbo].[ProcessSqliteIssueReturnData]
	@batchId BIGINT
AS
BEGIN

	-- if batch is already processed - return	
	IF NOT EXISTS(SELECT 1 FROM dbo.SqliteActionBatch 
			WHERE Id = @batchId AND NumberOfIssueReturnsSaved > 0
			AND BatchProcessed = 0)
	BEGIN
		RETURN;
	END

	-- First create required records in Day Table
	INSERT INTO dbo.[Day]
	([DATE])
	SELECT DISTINCT CAST(e.[IssueReturnDate] AS [Date])
	FROM dbo.SqliteIssueReturn e
	LEFT JOIN dbo.[Day] d on CAST(e.[IssueReturnDate] AS [Date]) = d.[DATE]
	WHERE e.BatchId = @batchId
	AND d.[DATE] IS NULL

	-- select current max entity Id
	DECLARE @lastMaxId BIGINT
	SELECT @lastMaxId = ISNULL(MAX(Id),0) FROM dbo.IssueReturn

	-- Create Input/Issue Records
	INSERT INTO dbo.[IssueReturn]
	([EmployeeId], [DayId], [EntityAgreementId], [ItemMasterId],
	[TransactionDate], [TransactionType], [Quantity], [ActivityId],
	[SqliteIssueReturnId])

	SELECT 
	sqe.[EmployeeId], d.[Id] AS [DayId], sqe.[AgreementId], sqe.[ItemId],
	CAST(sqe.[IssueReturnDate] AS [Date]), sqe.[TranType], sqe.[Quantity], 0,
	sqe.[Id]

	FROM dbo.SqliteIssueReturn sqe
	INNER JOIN dbo.[Day] d ON d.[DATE] = CAST(sqe.[IssueReturnDate] AS [Date])
	WHERE sqe.BatchId = @batchId AND sqe.IsProcessed = 0
	ORDER BY sqe.Id

	-- now we need to update the id in SqliteIssueReturn table
	UPDATE dbo.SqliteIssueReturn
	SET IssueReturnId = e.Id,
	IsProcessed = 1,
	DateUpdated = SYSUTCDATETIME()
	FROM dbo.SqliteIssueReturn se
	INNER JOIN dbo.[IssueReturn] e on se.Id = e.SqliteIssueReturnId
	AND se.BatchId = @batchId
	AND e.Id > @lastMaxId
END
GO

ALTER TABLE [dbo].[FeatureControl]
ADD	[IssueReturnFeature] BIT NOT NULL DEFAULT 0
GO

ALTER TABLE [dbo].[EmployeeMaster]
ALTER COLUMN [Staff Code] [nvarchar](255) NULL
GO

-- April 11 2019
ALTER TABLE [dbo].[WorkFlowSchedule]
ADD	[PhoneDataEntryPage] NVARCHAR(50) NOT NULL DEFAULT ''
GO

UPDATE dbo.WorkFlowSchedule
SET PhoneDataEntryPage = 'SowingWorkflowEntryPage'
WHERE PHASE = 'Sowing Confirmation'

UPDATE dbo.WorkFlowSchedule
SET PhoneDataEntryPage = 'CommonWorkflowEntryPage'
WHERE PHASE = 'Germination'

UPDATE dbo.WorkFlowSchedule
SET PhoneDataEntryPage = 'CommonWorkflowEntryPage'
WHERE PHASE = 'Weeding'

UPDATE dbo.WorkFlowSchedule
SET PhoneDataEntryPage = 'CommonWorkflowEntryPage'
WHERE PHASE = 'Staking'

UPDATE dbo.WorkFlowSchedule
SET PhoneDataEntryPage = 'FirstHarvestWorkflowEntryPage'
WHERE PHASE = 'First Harvest'


ALTER TABLE [dbo].[ItemMaster]
ADD	[Classification] NVARCHAR(10) NOT NULL DEFAULT ''
GO

UPDATE dbo.ItemMaster 
SET Classification = 'Sowing'
WHERE itemCode in ('AJAX', 'AJAX UNTREATED', 'ANAXO(N)', 'CALYPSO SEEDS G', 'Keerthi', 'SECURE', 
 'SHAKTHI', 'SPARTA', 'VERTINA SEEDS', 'VLASSTER G')
GO

UPDATE dbo.WorkFlowSchedule
SET Phase = 'Sowing'
WHERE Phase = 'Sowing Confirmation'
GO

-- April 18 2019
-- April 5 2019
ALTER TABLE [dbo].[SqliteActionBatch]
ADD
	[NumberOfWorkFlow] BIGINT NOT NULL DEFAULT 0,
	[NumberOfWorkFlowSaved] BIGINT NOT NULL DEFAULT 0
GO

CREATE TABLE [dbo].[SqliteEntityWorkFlowV2]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
	[BatchId] BIGINT NOT NULL References dbo.SqliteActionBatch,
    [EmployeeId] BIGINT NOT NULL,
	[EntityId] BIGINT NOT NULL DEFAULT 0,
	[EntityType] NVARCHAR(50) NOT NULL, 
	[EntityName] NVARCHAR(50) NOT NULL, 
	[AgreementId] BIGINT NOT NULL DEFAULT 0,
	[Agreement] NVARCHAR(50) NOT NULL,

	[Phase] NVARCHAR(50) NOT NULL,
	[IsStarted] BIT NOT NULL DEFAULT 0,
	[Date] DATE NOT NULL,
	[MaterialType] NVARCHAR(50),
	[MaterialQuantity] INT NOT NULL DEFAULT 0,
	[GapFillingRequired] BIT NOT NULL DEFAULT 0,
	[GapFillingSeedQuantity] INT NOT NULL DEFAULT 0,
	[LaborCount] INT NOT NULL DEFAULT 0,
	[PercentCompleted] INT NOT NULL DEFAULT 0,

	[IsProcessed] BIT NOT NULL DEFAULT 0,
	[EntityWorkFlowId] BIGINT NOT NULL DEFAULT 0,
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[Timestamp] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
)
GO

-- 19.4.2019
ALTER TABLE [dbo].[EntityWorkFlow]
ADD 
	[AgreementId] BIGINT NOT NULL DEFAULT 0,
	[Agreement] NVARCHAR(50) NOT NULL DEFAULT ''
GO

ALTER TABLE [dbo].[EntityWorkFlowDetail]
ADD
	[IsStarted] BIT NOT NULL DEFAULT 0,
	[WorkFlowDate] DATE,
	[MaterialType] NVARCHAR(50),
	[MaterialQuantity] INT NOT NULL DEFAULT 0,
	[GapFillingRequired] BIT NOT NULL DEFAULT 0,
	[GapFillingSeedQuantity] INT NOT NULL DEFAULT 0,
	[LaborCount] INT NOT NULL DEFAULT 0,
	[PercentCompleted] INT NOT NULL DEFAULT 0,
	[BatchId] BIGINT NOT NULL DEFAULT 0
GO


CREATE PROCEDURE [dbo].[ProcessSqliteEntityWorkFlowDataV2]
	@batchId BIGINT
AS
BEGIN
   /*
    Conditions taken care of:
	a) Can't have more than one open work flow for an entity
	b) Duplicate phase activity records are ignored

	a can happen in following scenario
	   - Downloaded entities on phone
	   - Created a new workflow for an entity
	   - Uploaded the batch
	   - (batch is either under process or I did not download again)
	   - Create another start work flow activity for same entity
	   - Upload to server

	b can happen in similar scenario as above

	In the same batch, I can create multiple activities for same entity
	(duplicate activities for same entity in same batch are ignored)

	If we receive duplicates subsequently, it won't update the detail table
	as the phase is marked as complete.
	  
   */

	DECLARE @entityWorkFlow TABLE 
	(
		[Id] BIGINT,
		[Phase] NVARCHAR(50) NOT NULL,
		[PhaseStartDate] DATE NOT NULL,
		[PhaseEndDate] DATE NOT NULL
	)

	DECLARE @sqliteEntityWorkFlow TABLE 
	(ID BIGINT, 
	 EmployeeId BIGINT,
	 EmployeeCode NVARCHAR(10),
	 [HQCode] NVARCHAR(10),
	 [Date] DATE
	)

	IF NOT EXISTS(SELECT 1 FROM dbo.SqliteActionBatch 
			WHERE Id = @batchId AND NumberOfWorkFlowSaved > 0
			AND BatchProcessed = 0)
	BEGIN
		RETURN;
	END


	---- if there are no unprocessed entries - return
	--IF NOT EXISTS(SELECT 1 FROM @sqliteEntityWorkFlow)
	--BEGIN
	--	RETURN;
	--END

	-- fill Entity Id - if zero
	UPDATE dbo.SqliteEntityWorkFlowV2
	SET EntityId = e.Id
	FROM dbo.SqliteEntityWorkFlowV2 sewf
	INNER JOIN dbo.Entity e on sewf.EntityType = e.EntityType
	AND sewf.EntityName = e.EntityName
	AND sewf.EntityId = 0
	AND sewf.BatchId = @batchId
	
	-- For one entity, we will process only one row
	-- so refresh @sqliteEntityWorkFlow by removing duplicates
	-- Also
	-- Now in @sqliteEntityWorkFlow table
	-- fill EmployeeId, EmployeeCode and Date
	-- this is done so that we don't have to join the tables two times
	-- in following queries.
	;WITH singleRecCTE(Id, rownum)
	AS
	(
		SELECT sewf.Id,
		ROW_NUMBER() Over (Partition By sewf.EntityId ORDER BY sewf.Id)
		FROM dbo.SqliteEntityWorkFlowV2 sewf
		WHERE BatchId = @batchId
	)
	INSERT INTO @sqliteEntityWorkFlow 
	(ID, EmployeeId, EmployeeCode, [Date], HQCode)
	SELECT s2.Id, s2.EmployeeId, te.EmployeeCode, s2.[Date], sp.[HQCode]
	FROM singleRecCTE cte
	INNER JOIN dbo.SqliteEntityWorkFlowV2 s2 on cte.Id = s2.Id
	INNER JOIN dbo.TenantEmployee te on s2.EmployeeId = te.Id
	INNER JOIN dbo.SalesPerson sp on te.EmployeeCode = sp.StaffCode
	WHERE cte.rownum = 1


	-- Create a in memory table of work flow schedule
	-- this will have an additional column indicating previous phase
	-- Work flow schedule is a small table with say 5 to 10 rows.
	DECLARE @WorkFlowSchedule TABLE
	(
		[Sequence] INT NOT NULL,
		[Phase] NVARCHAR(50) NOT NULL,
		[TargetStartAtDay] INT NOT NULL,
		[TargetEndAtDay] INT NOT NULL,
		[PrevPhase] NVARCHAR(50) NOT NULL
	)

	;with schCTE([Sequence], Phase, TargetStartAtDay, TargetEndAtDay, rownum)
	AS
	(
		SELECT [Sequence], Phase,  TargetStartAtDay, TargetEndAtDay,
		ROW_NUMBER() OVER (Order By [Sequence])
		FROM dbo.[WorkFlowSchedule]
		WHERE IsActive = 1
	)
	INSERT INTO @WorkFlowSchedule
	([Sequence], [Phase], [TargetStartAtDay], [TargetEndAtDay], [PrevPhase])
	SELECT [Sequence], Phase,
	TargetStartAtDay, TargetEndAtDay,
	ISNULL((SELECT Phase FROM schCTE WHERE rownum = p.rownum-1), '') PrevPhase
	FROM schCTE p


	-- Select first step in workflow
	DECLARE @firstStep NVARCHAR(50)
	SELECT TOP 1 @firstStep = Phase
	FROM @WorkFlowSchedule
	ORDER BY [Sequence]

	-- INSERT NEW Rows in EntityWorkFlow 
	INSERT into dbo.EntityWorkFlow
	(EmployeeId, [EmployeeCode], HQCode, EntityId, EntityName, CurrentPhase, [CurrentPhaseStartDate],
	[CurrentPhaseEndDate], [InitiationDate], [IsComplete],
	[AgreementId], [Agreement])
	SELECT mem.EmployeeId, mem.EmployeeCode, mem.HQCode, sewf.EntityId, sewf.EntityName, '', '2000-01-01',
	'2000-01-01', mem.[Date], 0, 
	sewf.AgreementId, sewf.Agreement
	FROM dbo.SqliteEntityWorkFlowV2 sewf
	INNER JOIN @sqliteEntityWorkFlow mem ON mem.ID = sewf.Id
	WHERE sewf.IsProcessed = 0
	AND sewf.Phase = @firstStep
	-- there can be only one open work flow for an entity
	AND NOT EXISTS (SELECT 1 FROM dbo.EntityWorkFlow ewf2 
				WHERE ewf2.IsComplete = 0
				AND ewf2.EntityId = sewf.EntityId
				AND ewf2.AgreementId = sewf.AgreementId
				)


	-- now create detail entries for newly created work flow
	INSERT INTO dbo.EntityWorkFlowDetail
	(EntityWorkFlowId, [Sequence], [Phase], 
	[PlannedStartDate], [PlannedEndDate], 
	[PrevPhase])
	SELECT wf.Id, sch.[Sequence], sch.Phase, 
	DATEADD(d, sch.TargetStartAtDay, wf.InitiationDate), DATEADD(d, sch.TargetEndAtDay, wf.InitiationDate),
	sch.PrevPhase
	FROM dbo.EntityWorkFlow wf
	INNER JOIN @WorkFlowSchedule sch ON 1 = 1
	AND wf.CurrentPhase = ''


	DECLARE @updateTime DATETIME2 = SYSUTCDATETIME()

	DECLARE @updatedWorkFlowId TABLE 
	( Id BIGINT, ParentId BIGINT, PhaseDate DATE, Phase NVARCHAR(50), BatchId BIGINT )

	-- Now we have parent/child entries for all (including new) work flow requests
	-- Update the status in Detail table
	-- This will output the Ids of parent table (EntityWorkFlow), for which details 
	-- have been updated.
	UPDATE dbo.EntityWorkFlowDetail
	SET ActivityId = 0, -- this column should be dropped
	IsComplete = 1,
	ActualDate = sewf.[Date],
	[Timestamp] = @updateTime,
	--[PrevPhaseActualDate] = (
	--							SELECT ActualDate 
	--							FROM dbo.EntityWorkFlowDetail d2 
	--							WHERE d2.EntityWorkFlowId = wfd.EntityWorkFlowId 
	--								AND Phase = wfd.PrevPhase
	--						),
	PhaseCompleteStatus = CASE WHEN sewf.[Date] < wfd.PlannedStartDate THEN 'Early' 
							   WHEN sewf.[Date] > wfd.PlannedEndDate THEN 'Late'
							   WHEN sewf.[Date] >= wfd.PlannedStartDate AND sewf.[Date] <= wfd.PlannedEndDate THEN 'OnSchedule'
							   ELSE ''
							END,
	EmployeeId = mem.EmployeeId,
	-- 19.4.19
	IsStarted = sewf.IsStarted,
	WorkFlowDate = sewf.[Date],
	MaterialType = sewf.MaterialType,
	MaterialQuantity = sewf.MaterialQuantity,
	GapFillingRequired = sewf.GapFillingRequired,
	GapFillingSeedQuantity = sewf.GapFillingSeedQuantity,
	LaborCount = sewf.LaborCount,
	PercentCompleted = sewf.PercentCompleted,
	BatchId = sewf.BatchId
	OUTPUT inserted.Id, inserted.EntityWorkFlowId, inserted.ActualDate, inserted.Phase, inserted.BatchId INTO @updatedWorkFlowId
	FROM dbo.EntityWorkFlowDetail wfd
	INNER JOIN dbo.EntityWorkFlow wf on wfd.EntityWorkFlowId = wf.Id
		AND wf.IsComplete = 0
		AND wfd.IsComplete = 0
	INNER JOIN dbo.SqliteEntityWorkFlowV2 sewf ON sewf.EntityId = wf.EntityId
	AND wfd.Phase = sewf.Phase
	INNER JOIN @sqliteEntityWorkFlow mem ON mem.ID = sewf.Id

	-- put the prev phase actual date, in next phase row of detail table
	UPDATE dbo.EntityWorkFlowDetail
	SET PrevPhaseActualDate = u.PhaseDate
	FROM dbo.EntityWorkFlowDetail d
	INNER JOIN @updatedWorkFlowId u on d.EntityWorkFlowId = u.ParentId
	AND d.PrevPhase = u.Phase

	-- Find out current phase that need to be updated in parent table
	;WITH updateRecCTE(Id, [Sequence], Phase, PlannedStartDate, PlannedEndDate, rownumber)
	AS
	(
		SELECT uwf.ParentId, wfd.[Sequence], wfd.[Phase], wfd.PlannedStartDate, wfd.PlannedEndDate,
		ROW_NUMBER() OVER (PARTITION BY uwf.ParentId Order By wfd.[Sequence])
		FROM @updatedWorkFlowId uwf
		INNER JOIN dbo.EntityWorkFlowDetail wfd on uwf.ParentId = wfd.EntityWorkFlowId
		AND wfd.IsComplete = 0
	)
	INSERT INTO @entityWorkFlow
	(Id, Phase, PhaseStartDate, PhaseEndDate)
	SELECT Id, Phase, PlannedStartDate, PlannedEndDate FROM updateRecCTE
	WHERE rownumber = 1
	

	-- Now update the status values in Parent table
	-- (this only updates the rows when atleast there is one pending stage)
	UPDATE dbo.EntityWorkFlow
	SET CurrentPhase = memWf.Phase,
	CurrentPhaseStartDate = memWf.PhaseStartDate,
	CurrentPhaseEndDate = memWf.PhaseEndDate,
	[Timestamp] = @updateTime
	FROM dbo.EntityWorkFlow wf
	INNER JOIN @entityWorkFlow memWf ON wf.Id = memWf.Id

	-- Now mark the rows as complete where phase is marked as complete in Detail table
	-- (this will happen only when all phases are complete, otherwise previous sql
	--  will have already set the phase in parent to next available phase)
	UPDATE dbo.EntityWorkFlow
	SET IsComplete = 1,
	[Timestamp] = @updateTime
	FROM dbo.EntityWorkFlow wf
	INNER JOIN @updatedWorkFlowId uwf on wf.Id = uwf.ParentId
	INNER JOIN dbo.EntityWorkFlowDetail wfd on wfd.EntityWorkFlowId = wf.Id
	AND wfd.Phase = wf.CurrentPhase
	AND wfd.IsComplete = 1

	-- Now mark the status in SqliteEntityWorkFlow table
	UPDATE dbo.SqliteEntityWorkFlowV2
	SET IsProcessed = 1,
	[Timestamp] = @updateTime,
	EntityWorkFlowId = uwf.ParentId
	FROM dbo.SqliteEntityWorkFlowV2 sewf
	INNER JOIN dbo.EntityWorkFlow ewf on ewf.AgreementId = sewf.AgreementId
	INNER JOIN @updatedWorkFlowId uwf on uwf.ParentId = ewf.Id
	AND uwf.BatchId = sewf.BatchId
END
GO

-- this is todo in database
--ALTER TABLE [dbo].[Entity]
--DROP COLUMN 
--    --For dealers who have been added as customers and once approved need not be shown in entity
--	[IsApproved],
--	[ApproveDate],
--	[ApproveRef],
--	[ApproveNotes],
--	[ApprovedBy]
--GO

ALTER TABLE [dbo].[Entity]
ADD [UpdatedBy] NVARCHAR(50) NOT NULL DEFAULT ''
GO

CREATE UNIQUE INDEX IX_EntityAgreement_AgreementNumber
ON dbo.EntityAgreement (AgreementNumber)
WHERE AgreementNumber <> ''
GO

CREATE UNIQUE INDEX IX_EntityAgreement_EntityId
ON dbo.EntityAgreement (EntityId)
GO

-- 20.4.2019
ALTER TABLE [dbo].[SqliteEntityWorkFlowV2]
ADD
	[FieldVisitDate] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
GO

ALTER PROCEDURE [dbo].[ProcessSqliteEntityWorkFlowDataV2]
	@batchId BIGINT
AS
BEGIN
   /*
    Conditions taken care of:
	a) Can't have more than one open work flow for an entity
	b) Duplicate phase activity records are ignored

	a can happen in following scenario
	   - Downloaded entities on phone
	   - Created a new workflow for an entity
	   - Uploaded the batch
	   - (batch is either under process or I did not download again)
	   - Create another start work flow activity for same entity
	   - Upload to server

	b can happen in similar scenario as above

	In the same batch, I can create multiple activities for same entity
	(duplicate activities for same entity in same batch are ignored)

	If we receive duplicates subsequently, it won't update the detail table
	as the phase is marked as complete.
	  
   */

	DECLARE @entityWorkFlow TABLE 
	(
		[Id] BIGINT,
		[Phase] NVARCHAR(50) NOT NULL,
		[PhaseStartDate] DATE NOT NULL,
		[PhaseEndDate] DATE NOT NULL
	)

	DECLARE @sqliteEntityWorkFlow TABLE 
	(ID BIGINT, 
	 EmployeeId BIGINT,
	 EmployeeCode NVARCHAR(10),
	 [HQCode] NVARCHAR(10),
	 [Date] DATE
	)

	IF NOT EXISTS(SELECT 1 FROM dbo.SqliteActionBatch 
			WHERE Id = @batchId AND NumberOfWorkFlowSaved > 0
			AND BatchProcessed = 0)
	BEGIN
		RETURN;
	END


	---- if there are no unprocessed entries - return
	--IF NOT EXISTS(SELECT 1 FROM @sqliteEntityWorkFlow)
	--BEGIN
	--	RETURN;
	--END

	-- fill Entity Id - if zero
	UPDATE dbo.SqliteEntityWorkFlowV2
	SET EntityId = e.Id
	FROM dbo.SqliteEntityWorkFlowV2 sewf
	INNER JOIN dbo.Entity e on sewf.EntityType = e.EntityType
	AND sewf.EntityName = e.EntityName
	AND sewf.EntityId = 0
	AND sewf.BatchId = @batchId
	
	-- For one entity, we will process only one row
	-- so refresh @sqliteEntityWorkFlow by removing duplicates
	-- Also
	-- Now in @sqliteEntityWorkFlow table
	-- fill EmployeeId, EmployeeCode and Date
	-- this is done so that we don't have to join the tables two times
	-- in following queries.
	;WITH singleRecCTE(Id, rownum)
	AS
	(
		SELECT sewf.Id,
		ROW_NUMBER() Over (Partition By sewf.EntityId ORDER BY sewf.Id)
		FROM dbo.SqliteEntityWorkFlowV2 sewf
		WHERE BatchId = @batchId
	)
	INSERT INTO @sqliteEntityWorkFlow 
	(ID, EmployeeId, EmployeeCode, [Date], HQCode)
	SELECT s2.Id, s2.EmployeeId, te.EmployeeCode, s2.[Date], sp.[HQCode]
	FROM singleRecCTE cte
	INNER JOIN dbo.SqliteEntityWorkFlowV2 s2 on cte.Id = s2.Id
	INNER JOIN dbo.TenantEmployee te on s2.EmployeeId = te.Id
	INNER JOIN dbo.SalesPerson sp on te.EmployeeCode = sp.StaffCode
	WHERE cte.rownum = 1


	-- Create a in memory table of work flow schedule
	-- this will have an additional column indicating previous phase
	-- Work flow schedule is a small table with say 5 to 10 rows.
	DECLARE @WorkFlowSchedule TABLE
	(
		[Sequence] INT NOT NULL,
		[Phase] NVARCHAR(50) NOT NULL,
		[TargetStartAtDay] INT NOT NULL,
		[TargetEndAtDay] INT NOT NULL,
		[PrevPhase] NVARCHAR(50) NOT NULL
	)

	;with schCTE([Sequence], Phase, TargetStartAtDay, TargetEndAtDay, rownum)
	AS
	(
		SELECT [Sequence], Phase,  TargetStartAtDay, TargetEndAtDay,
		ROW_NUMBER() OVER (Order By [Sequence])
		FROM dbo.[WorkFlowSchedule]
		WHERE IsActive = 1
	)
	INSERT INTO @WorkFlowSchedule
	([Sequence], [Phase], [TargetStartAtDay], [TargetEndAtDay], [PrevPhase])
	SELECT [Sequence], Phase,
	TargetStartAtDay, TargetEndAtDay,
	ISNULL((SELECT Phase FROM schCTE WHERE rownum = p.rownum-1), '') PrevPhase
	FROM schCTE p


	-- Select first step in workflow
	DECLARE @firstStep NVARCHAR(50)
	SELECT TOP 1 @firstStep = Phase
	FROM @WorkFlowSchedule
	ORDER BY [Sequence]

	-- INSERT NEW Rows in EntityWorkFlow 
	INSERT into dbo.EntityWorkFlow
	(EmployeeId, [EmployeeCode], HQCode, EntityId, EntityName, CurrentPhase, [CurrentPhaseStartDate],
	[CurrentPhaseEndDate], [InitiationDate], [IsComplete],
	[AgreementId], [Agreement])
	SELECT mem.EmployeeId, mem.EmployeeCode, mem.HQCode, sewf.EntityId, sewf.EntityName, '', '2000-01-01',
	'2000-01-01', mem.[Date], 0, 
	sewf.AgreementId, sewf.Agreement
	FROM dbo.SqliteEntityWorkFlowV2 sewf
	INNER JOIN @sqliteEntityWorkFlow mem ON mem.ID = sewf.Id
	WHERE sewf.IsProcessed = 0
	AND sewf.Phase = @firstStep
	-- there can be only one open work flow for an entity
	AND NOT EXISTS (SELECT 1 FROM dbo.EntityWorkFlow ewf2 
				WHERE ewf2.IsComplete = 0
				AND ewf2.EntityId = sewf.EntityId
				AND ewf2.AgreementId = sewf.AgreementId
				)


	-- now create detail entries for newly created work flow
	INSERT INTO dbo.EntityWorkFlowDetail
	(EntityWorkFlowId, [Sequence], [Phase], 
	[PlannedStartDate], [PlannedEndDate], 
	[PrevPhase])
	SELECT wf.Id, sch.[Sequence], sch.Phase, 
	DATEADD(d, sch.TargetStartAtDay, wf.InitiationDate), DATEADD(d, sch.TargetEndAtDay, wf.InitiationDate),
	sch.PrevPhase
	FROM dbo.EntityWorkFlow wf
	INNER JOIN @WorkFlowSchedule sch ON 1 = 1
	AND wf.CurrentPhase = ''


	DECLARE @updateTime DATETIME2 = SYSUTCDATETIME()

	DECLARE @updatedWorkFlowId TABLE 
	( Id BIGINT, ParentId BIGINT, PhaseDate DATE, Phase NVARCHAR(50), BatchId BIGINT )

	-- Now we have parent/child entries for all (including new) work flow requests
	-- Update the status in Detail table
	-- This will output the Ids of parent table (EntityWorkFlow), for which details 
	-- have been updated.
	UPDATE dbo.EntityWorkFlowDetail
	SET ActivityId = 0, -- this column should be dropped
	IsComplete = 1,
	ActualDate = sewf.[FieldVisitDate],
	[Timestamp] = @updateTime,
	--[PrevPhaseActualDate] = (
	--							SELECT ActualDate 
	--							FROM dbo.EntityWorkFlowDetail d2 
	--							WHERE d2.EntityWorkFlowId = wfd.EntityWorkFlowId 
	--								AND Phase = wfd.PrevPhase
	--						),
	PhaseCompleteStatus = CASE WHEN sewf.[Date] < wfd.PlannedStartDate THEN 'Early' 
							   WHEN sewf.[Date] > wfd.PlannedEndDate THEN 'Late'
							   WHEN sewf.[Date] >= wfd.PlannedStartDate AND sewf.[Date] <= wfd.PlannedEndDate THEN 'OnSchedule'
							   ELSE ''
							END,
	EmployeeId = mem.EmployeeId,
	-- 19.4.19
	IsStarted = sewf.IsStarted,
	WorkFlowDate = sewf.[Date],
	MaterialType = sewf.MaterialType,
	MaterialQuantity = sewf.MaterialQuantity,
	GapFillingRequired = sewf.GapFillingRequired,
	GapFillingSeedQuantity = sewf.GapFillingSeedQuantity,
	LaborCount = sewf.LaborCount,
	PercentCompleted = sewf.PercentCompleted,
	BatchId = sewf.BatchId
	OUTPUT inserted.Id, inserted.EntityWorkFlowId, inserted.ActualDate, 
		inserted.Phase, inserted.BatchId
	INTO @updatedWorkFlowId
	FROM dbo.EntityWorkFlowDetail wfd
	INNER JOIN dbo.EntityWorkFlow wf on wfd.EntityWorkFlowId = wf.Id
		AND wf.IsComplete = 0
		AND wfd.IsComplete = 0
	INNER JOIN dbo.SqliteEntityWorkFlowV2 sewf ON sewf.EntityId = wf.EntityId
	AND wfd.Phase = sewf.Phase
	INNER JOIN @sqliteEntityWorkFlow mem ON mem.ID = sewf.Id

	-- put the prev phase actual date, in next phase row of detail table
	UPDATE dbo.EntityWorkFlowDetail
	SET PrevPhaseActualDate = u.PhaseDate
	FROM dbo.EntityWorkFlowDetail d
	INNER JOIN @updatedWorkFlowId u on d.EntityWorkFlowId = u.ParentId
	AND d.PrevPhase = u.Phase

	-- Find out current phase that need to be updated in parent table
	;WITH updateRecCTE(Id, [Sequence], Phase, PlannedStartDate, PlannedEndDate, rownumber)
	AS
	(
		SELECT uwf.ParentId, wfd.[Sequence], wfd.[Phase], wfd.PlannedStartDate, wfd.PlannedEndDate,
		ROW_NUMBER() OVER (PARTITION BY uwf.ParentId Order By wfd.[Sequence])
		FROM @updatedWorkFlowId uwf
		INNER JOIN dbo.EntityWorkFlowDetail wfd on uwf.ParentId = wfd.EntityWorkFlowId
		AND wfd.IsComplete = 0
	)
	INSERT INTO @entityWorkFlow
	(Id, Phase, PhaseStartDate, PhaseEndDate)
	SELECT Id, Phase, PlannedStartDate, PlannedEndDate FROM updateRecCTE
	WHERE rownumber = 1
	

	-- Now update the status values in Parent table
	-- (this only updates the rows when atleast there is one pending stage)
	UPDATE dbo.EntityWorkFlow
	SET CurrentPhase = memWf.Phase,
	CurrentPhaseStartDate = memWf.PhaseStartDate,
	CurrentPhaseEndDate = memWf.PhaseEndDate,
	[Timestamp] = @updateTime
	FROM dbo.EntityWorkFlow wf
	INNER JOIN @entityWorkFlow memWf ON wf.Id = memWf.Id

	-- Now mark the rows as complete where phase is marked as complete in Detail table
	-- (this will happen only when all phases are complete, otherwise previous sql
	--  will have already set the phase in parent to next available phase)
	UPDATE dbo.EntityWorkFlow
	SET IsComplete = 1,
	[Timestamp] = @updateTime
	FROM dbo.EntityWorkFlow wf
	INNER JOIN @updatedWorkFlowId uwf on wf.Id = uwf.ParentId
	INNER JOIN dbo.EntityWorkFlowDetail wfd on wfd.EntityWorkFlowId = wf.Id
	AND wfd.Phase = wf.CurrentPhase
	AND wfd.IsComplete = 1

	-- Now mark the status in SqliteEntityWorkFlow table
	UPDATE dbo.SqliteEntityWorkFlowV2
	SET IsProcessed = 1,
	[Timestamp] = @updateTime,
	EntityWorkFlowId = uwf.ParentId
	FROM dbo.SqliteEntityWorkFlowV2 sewf
	INNER JOIN dbo.EntityWorkFlow ewf on ewf.AgreementId = sewf.AgreementId
	INNER JOIN @updatedWorkFlowId uwf on uwf.ParentId = ewf.Id
	AND uwf.BatchId = sewf.BatchId
END
GO


ALTER TABLE [dbo].[SqliteIssueReturn]
ADD	[ActivityId] NVARCHAR(50) NOT NULL DEFAULT ''
GO

ALTER TABLE [dbo].[SqliteEntityWorkFlowV2]
ADD	[ActivityId] NVARCHAR(50) NOT NULL DEFAULT ''
GO

ALTER PROCEDURE [dbo].[ProcessSqliteEntityWorkFlowDataV2]
	@batchId BIGINT
AS
BEGIN
   /*
    Conditions taken care of:
	a) Can't have more than one open work flow for an entity
	b) Duplicate phase activity records are ignored

	a can happen in following scenario
	   - Downloaded entities on phone
	   - Created a new workflow for an entity
	   - Uploaded the batch
	   - (batch is either under process or I did not download again)
	   - Create another start work flow activity for same entity
	   - Upload to server

	b can happen in similar scenario as above

	In the same batch, I can create multiple activities for same entity
	(duplicate activities for same entity in same batch are ignored)

	If we receive duplicates subsequently, it won't update the detail table
	as the phase is marked as complete.
	  
   */

	DECLARE @entityWorkFlow TABLE 
	(
		[Id] BIGINT,
		[Phase] NVARCHAR(50) NOT NULL,
		[PhaseStartDate] DATE NOT NULL,
		[PhaseEndDate] DATE NOT NULL
	)

	DECLARE @sqliteEntityWorkFlow TABLE 
	(ID BIGINT, 
	 EmployeeId BIGINT,
	 EmployeeCode NVARCHAR(10),
	 [HQCode] NVARCHAR(10),
	 [Date] DATE
	)

	IF NOT EXISTS(SELECT 1 FROM dbo.SqliteActionBatch 
			WHERE Id = @batchId AND NumberOfWorkFlowSaved > 0
			AND BatchProcessed = 0)
	BEGIN
		RETURN;
	END


	---- if there are no unprocessed entries - return
	--IF NOT EXISTS(SELECT 1 FROM @sqliteEntityWorkFlow)
	--BEGIN
	--	RETURN;
	--END

	-- fill Entity Id - if zero
	UPDATE dbo.SqliteEntityWorkFlowV2
	SET EntityId = e.Id
	FROM dbo.SqliteEntityWorkFlowV2 sewf
	INNER JOIN dbo.Entity e on sewf.EntityType = e.EntityType
	AND sewf.EntityName = e.EntityName
	AND sewf.EntityId = 0
	AND sewf.BatchId = @batchId
	
	-- For one entity, we will process only one row
	-- so refresh @sqliteEntityWorkFlow by removing duplicates
	-- Also
	-- Now in @sqliteEntityWorkFlow table
	-- fill EmployeeId, EmployeeCode and Date
	-- this is done so that we don't have to join the tables two times
	-- in following queries.
	;WITH singleRecCTE(Id, rownum)
	AS
	(
		SELECT sewf.Id,
		ROW_NUMBER() Over (Partition By sewf.EntityId ORDER BY sewf.Id)
		FROM dbo.SqliteEntityWorkFlowV2 sewf
		WHERE BatchId = @batchId
	)
	INSERT INTO @sqliteEntityWorkFlow 
	(ID, EmployeeId, EmployeeCode, [Date], HQCode)
	SELECT s2.Id, s2.EmployeeId, te.EmployeeCode, s2.[Date], sp.[HQCode]
	FROM singleRecCTE cte
	INNER JOIN dbo.SqliteEntityWorkFlowV2 s2 on cte.Id = s2.Id
	INNER JOIN dbo.TenantEmployee te on s2.EmployeeId = te.Id
	INNER JOIN dbo.SalesPerson sp on te.EmployeeCode = sp.StaffCode
	WHERE cte.rownum = 1


	-- Create a in memory table of work flow schedule
	-- this will have an additional column indicating previous phase
	-- Work flow schedule is a small table with say 5 to 10 rows.
	DECLARE @WorkFlowSchedule TABLE
	(
		[Sequence] INT NOT NULL,
		[Phase] NVARCHAR(50) NOT NULL,
		[TargetStartAtDay] INT NOT NULL,
		[TargetEndAtDay] INT NOT NULL,
		[PrevPhase] NVARCHAR(50) NOT NULL
	)

	;with schCTE([Sequence], Phase, TargetStartAtDay, TargetEndAtDay, rownum)
	AS
	(
		SELECT [Sequence], Phase,  TargetStartAtDay, TargetEndAtDay,
		ROW_NUMBER() OVER (Order By [Sequence])
		FROM dbo.[WorkFlowSchedule]
		WHERE IsActive = 1
	)
	INSERT INTO @WorkFlowSchedule
	([Sequence], [Phase], [TargetStartAtDay], [TargetEndAtDay], [PrevPhase])
	SELECT [Sequence], Phase,
	TargetStartAtDay, TargetEndAtDay,
	ISNULL((SELECT Phase FROM schCTE WHERE rownum = p.rownum-1), '') PrevPhase
	FROM schCTE p


	-- Select first step in workflow
	DECLARE @firstStep NVARCHAR(50)
	SELECT TOP 1 @firstStep = Phase
	FROM @WorkFlowSchedule
	ORDER BY [Sequence]

	-- INSERT NEW Rows in EntityWorkFlow 
	INSERT into dbo.EntityWorkFlow
	(EmployeeId, [EmployeeCode], HQCode, EntityId, EntityName, CurrentPhase, [CurrentPhaseStartDate],
	[CurrentPhaseEndDate], [InitiationDate], [IsComplete],
	[AgreementId], [Agreement])
	SELECT mem.EmployeeId, mem.EmployeeCode, mem.HQCode, sewf.EntityId, sewf.EntityName, '', '2000-01-01',
	'2000-01-01', mem.[Date], 0, 
	sewf.AgreementId, sewf.Agreement
	FROM dbo.SqliteEntityWorkFlowV2 sewf
	INNER JOIN @sqliteEntityWorkFlow mem ON mem.ID = sewf.Id
	WHERE sewf.IsProcessed = 0
	AND sewf.Phase = @firstStep
	-- there can be only one open work flow for an entity
	AND NOT EXISTS (SELECT 1 FROM dbo.EntityWorkFlow ewf2 
				WHERE ewf2.IsComplete = 0
				AND ewf2.EntityId = sewf.EntityId
				AND ewf2.AgreementId = sewf.AgreementId
				)


	-- now create detail entries for newly created work flow
	INSERT INTO dbo.EntityWorkFlowDetail
	(EntityWorkFlowId, [Sequence], [Phase], 
	[PlannedStartDate], [PlannedEndDate], 
	[PrevPhase])
	SELECT wf.Id, sch.[Sequence], sch.Phase, 
	DATEADD(d, sch.TargetStartAtDay, wf.InitiationDate), DATEADD(d, sch.TargetEndAtDay, wf.InitiationDate),
	sch.PrevPhase
	FROM dbo.EntityWorkFlow wf
	INNER JOIN @WorkFlowSchedule sch ON 1 = 1
	AND wf.CurrentPhase = ''


	DECLARE @updateTime DATETIME2 = SYSUTCDATETIME()

	DECLARE @updatedWorkFlowId TABLE 
	( Id BIGINT, ParentId BIGINT, PhaseDate DATE, Phase NVARCHAR(50), BatchId BIGINT )

	-- Now we have parent/child entries for all (including new) work flow requests
	-- Update the status in Detail table
	-- This will output the Ids of parent table (EntityWorkFlow), for which details 
	-- have been updated.
	UPDATE dbo.EntityWorkFlowDetail
	SET ActivityId = sqa.ActivityId,
	IsComplete = 1,
	ActualDate = sewf.[FieldVisitDate],
	[Timestamp] = @updateTime,
	--[PrevPhaseActualDate] = (
	--							SELECT ActualDate 
	--							FROM dbo.EntityWorkFlowDetail d2 
	--							WHERE d2.EntityWorkFlowId = wfd.EntityWorkFlowId 
	--								AND Phase = wfd.PrevPhase
	--						),
	PhaseCompleteStatus = CASE WHEN sewf.[Date] < wfd.PlannedStartDate THEN 'Early' 
							   WHEN sewf.[Date] > wfd.PlannedEndDate THEN 'Late'
							   WHEN sewf.[Date] >= wfd.PlannedStartDate AND sewf.[Date] <= wfd.PlannedEndDate THEN 'OnSchedule'
							   ELSE ''
							END,
	EmployeeId = mem.EmployeeId,
	-- 19.4.19
	IsStarted = sewf.IsStarted,
	WorkFlowDate = sewf.[Date],
	MaterialType = sewf.MaterialType,
	MaterialQuantity = sewf.MaterialQuantity,
	GapFillingRequired = sewf.GapFillingRequired,
	GapFillingSeedQuantity = sewf.GapFillingSeedQuantity,
	LaborCount = sewf.LaborCount,
	PercentCompleted = sewf.PercentCompleted,
	BatchId = sewf.BatchId
	OUTPUT inserted.Id, inserted.EntityWorkFlowId, inserted.ActualDate, 
		inserted.Phase, inserted.BatchId
	INTO @updatedWorkFlowId
	FROM dbo.EntityWorkFlowDetail wfd
	INNER JOIN dbo.EntityWorkFlow wf on wfd.EntityWorkFlowId = wf.Id
		AND wf.IsComplete = 0
		AND wfd.IsComplete = 0
	INNER JOIN dbo.SqliteEntityWorkFlowV2 sewf ON sewf.EntityId = wf.EntityId
	AND wfd.Phase = sewf.Phase
	INNER JOIN @sqliteEntityWorkFlow mem ON mem.ID = sewf.Id
	-- this join is to get activity Id
	INNER JOIN dbo.SqliteAction sqa on sqa.[At] = sewf.FieldVisitDate
	AND sqa.EmployeeId = mem.EmployeeId
	AND sqa.PhoneDbId = sewf.ActivityId


	-- put the prev phase actual date, in next phase row of detail table
	UPDATE dbo.EntityWorkFlowDetail
	SET PrevPhaseActualDate = u.PhaseDate
	FROM dbo.EntityWorkFlowDetail d
	INNER JOIN @updatedWorkFlowId u on d.EntityWorkFlowId = u.ParentId
	AND d.PrevPhase = u.Phase

	-- Find out current phase that need to be updated in parent table
	;WITH updateRecCTE(Id, [Sequence], Phase, PlannedStartDate, PlannedEndDate, rownumber)
	AS
	(
		SELECT uwf.ParentId, wfd.[Sequence], wfd.[Phase], wfd.PlannedStartDate, wfd.PlannedEndDate,
		ROW_NUMBER() OVER (PARTITION BY uwf.ParentId Order By wfd.[Sequence])
		FROM @updatedWorkFlowId uwf
		INNER JOIN dbo.EntityWorkFlowDetail wfd on uwf.ParentId = wfd.EntityWorkFlowId
		AND wfd.IsComplete = 0
	)
	INSERT INTO @entityWorkFlow
	(Id, Phase, PhaseStartDate, PhaseEndDate)
	SELECT Id, Phase, PlannedStartDate, PlannedEndDate FROM updateRecCTE
	WHERE rownumber = 1
	

	-- Now update the status values in Parent table
	-- (this only updates the rows when atleast there is one pending stage)
	UPDATE dbo.EntityWorkFlow
	SET CurrentPhase = memWf.Phase,
	CurrentPhaseStartDate = memWf.PhaseStartDate,
	CurrentPhaseEndDate = memWf.PhaseEndDate,
	[Timestamp] = @updateTime
	FROM dbo.EntityWorkFlow wf
	INNER JOIN @entityWorkFlow memWf ON wf.Id = memWf.Id

	-- Now mark the rows as complete where phase is marked as complete in Detail table
	-- (this will happen only when all phases are complete, otherwise previous sql
	--  will have already set the phase in parent to next available phase)
	UPDATE dbo.EntityWorkFlow
	SET IsComplete = 1,
	[Timestamp] = @updateTime
	FROM dbo.EntityWorkFlow wf
	INNER JOIN @updatedWorkFlowId uwf on wf.Id = uwf.ParentId
	INNER JOIN dbo.EntityWorkFlowDetail wfd on wfd.EntityWorkFlowId = wf.Id
	AND wfd.Phase = wf.CurrentPhase
	AND wfd.IsComplete = 1

	-- Now mark the status in SqliteEntityWorkFlow table
	UPDATE dbo.SqliteEntityWorkFlowV2
	SET IsProcessed = 1,
	[Timestamp] = @updateTime,
	EntityWorkFlowId = uwf.ParentId
	FROM dbo.SqliteEntityWorkFlowV2 sewf
	INNER JOIN dbo.EntityWorkFlow ewf on ewf.AgreementId = sewf.AgreementId
	INNER JOIN @updatedWorkFlowId uwf on uwf.ParentId = ewf.Id
	AND uwf.BatchId = sewf.BatchId
END
GO

ALTER PROCEDURE [dbo].[ProcessSqliteIssueReturnData]
	@batchId BIGINT
AS
BEGIN

	-- if batch is already processed - return	
	IF NOT EXISTS(SELECT 1 FROM dbo.SqliteActionBatch 
			WHERE Id = @batchId AND NumberOfIssueReturnsSaved > 0
			AND BatchProcessed = 0)
	BEGIN
		RETURN;
	END

	-- First create required records in Day Table
	INSERT INTO dbo.[Day]
	([DATE])
	SELECT DISTINCT CAST(e.[IssueReturnDate] AS [Date])
	FROM dbo.SqliteIssueReturn e
	LEFT JOIN dbo.[Day] d on CAST(e.[IssueReturnDate] AS [Date]) = d.[DATE]
	WHERE e.BatchId = @batchId
	AND d.[DATE] IS NULL

	-- select current max entity Id
	DECLARE @lastMaxId BIGINT
	SELECT @lastMaxId = ISNULL(MAX(Id),0) FROM dbo.IssueReturn

	-- Create Input/Issue Records
	INSERT INTO dbo.[IssueReturn]
	([EmployeeId], [DayId], [EntityAgreementId], [ItemMasterId],
	[TransactionDate], [TransactionType], [Quantity], [ActivityId],
	[SqliteIssueReturnId])

	SELECT 
	sqe.[EmployeeId], d.[Id] AS [DayId], sqe.[AgreementId], sqe.[ItemId],
	CAST(sqe.[IssueReturnDate] AS [Date]), sqe.[TranType], sqe.[Quantity], sqa.ActivityId,
	sqe.[Id]

	FROM dbo.SqliteIssueReturn sqe
	INNER JOIN dbo.[Day] d ON d.[DATE] = CAST(sqe.[IssueReturnDate] AS [Date])
	-- this join is to get activity Id
	INNER JOIN dbo.SqliteAction sqa on sqa.EmployeeId = sqe.EmployeeId
			AND sqa.[At] = sqe.IssueReturnDate
			AND sqa.PhoneDbId = sqe.ActivityId
	WHERE sqe.BatchId = @batchId AND sqe.IsProcessed = 0
	ORDER BY sqe.Id

	-- now we need to update the id in SqliteIssueReturn table
	UPDATE dbo.SqliteIssueReturn
	SET IssueReturnId = e.Id,
	IsProcessed = 1,
	DateUpdated = SYSUTCDATETIME()
	FROM dbo.SqliteIssueReturn se
	INNER JOIN dbo.[IssueReturn] e on se.Id = e.SqliteIssueReturnId
	AND se.BatchId = @batchId
	AND e.Id > @lastMaxId
END
GO

DROP INDEX [IX_SqliteAction_EmpIdActionName] ON dbo.SqliteAction
GO

CREATE /*UNIQUE*/ INDEX [IX_SqliteAction_EmpIdActionName]
	ON [dbo].[SqliteAction]
	(EmployeeId, [AT], ActivityTrackingType)
GO

CREATE TABLE [dbo].[ActivityType]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[ActivityName] NVARCHAR(50) NOT NULL, 
	[DateCreated] [DATETIME2] NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] [DateTime2] NOT NULL DEFAULT SYSUTCDATETIME()
)
GO

ALTER PROCEDURE [dbo].[AddActivityData]
	@employeeDayId BIGINT,
	@activityDateTime DateTime2,
	@clientName NVARCHAR(50),
	@clientPhone NVARCHAR(20),
	@clientType NVARCHAR(50),
	@activityType NVARCHAR(50),
	@comments NVARCHAR(2048),
	@clientCode NVARCHAR(50),
	@activityAmount DECIMAL(19,2),
	@atBusiness BIT,
	@imageCount INT,
	@contactCount INT,
	@activityId BIGINT OUTPUT
AS
BEGIN
   -- Records tracking activity
   SET @activityId = 0
	-- check if a record already exist in Employee Day table
	IF EXISTS(SELECT 1 FROM dbo.EmployeeDay WHERE Id = @employeeDayId)
	BEGIN
		INSERT into dbo.Activity
		(EmployeeDayId, ClientName, ClientPhone, ClientType, ActivityType, Comments, ImageCount, [At], ClientCode, ActivityAmount, AtBusiness, ContactCount)
		VALUES
		(@employeeDayId, @clientName, @clientPhone, @clientType, @activityType, @comments, @imageCount, @activityDateTime, @clientCode, @activityAmount, @atBusiness, @contactCount)

		SET @activityId = SCOPE_IDENTITY()

		-- create a row in Activity Type if not already there
		IF NOT EXISTS(SELECT 1 FROM dbo.ActivityType WHERE ActivityName = @activityType)
		BEGIN
		    INSERT INTO dbo.ActivityType
			(ActivityName)
			VALUES
			(@activityType)
		END

		-- keep count of total activities for exec crm application
		UPDATE dbo.EmployeeDay
		SET TotalActivityCount = TotalActivityCount + 1
		WHERE Id = @employeeDayId
	END
END
GO

-- April 24 2019
-- We will need to reduce the list on seeds to only Gherkin on Input Issue/Return. Data Setup
Update dbo.ItemMaster
set IsActive = 0
where Category = 'Seeds'
and Classification <> 'Sowing'

-- 
-- table modified on 24.4.19 - added entityId
drop table dbo.IssueReturn
go
CREATE TABLE [dbo].[IssueReturn]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[EmployeeId] BIGINT NOT NULL REFERENCES TenantEmployee,
	[DayId] BIGINT NOT NULL REFERENCES dbo.[Day],
	[EntityId] BIGINT NOT NULL REFERENCES dbo.Entity,
	[EntityAgreementId] BIGINT NULL REFERENCES dbo.EntityAgreement,
	[ItemMasterId] BIGINT NOT NULL REFERENCES dbo.ItemMaster,
	[TransactionDate] DATE NOT NULL,
	[TransactionType] NVARCHAR(50) NOT NULL, -- Issue/Return/Abandoned
	[Quantity] INT NOT NULL,
	[ActivityId] BIGINT NOT NULL,
	[SqliteIssueReturnId] BIGINT NOT NULL DEFAULT 0,
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
)
GO

ALTER TABLE [dbo].[SqliteIssueReturn]
ADD	[SqliteEntityId]  NVARCHAR(50) NOT NULL DEFAULT ''
GO

ALTER PROCEDURE [dbo].[ProcessSqliteIssueReturnData]
	@batchId BIGINT
AS
BEGIN

	-- if batch is already processed - return	
	IF NOT EXISTS(SELECT 1 FROM dbo.SqliteActionBatch 
			WHERE Id = @batchId AND NumberOfIssueReturnsSaved > 0
			AND BatchProcessed = 0)
	BEGIN
		RETURN;
	END

	-- First create required records in Day Table
	INSERT INTO dbo.[Day]
	([DATE])
	SELECT DISTINCT CAST(e.[IssueReturnDate] AS [Date])
	FROM dbo.SqliteIssueReturn e
	LEFT JOIN dbo.[Day] d on CAST(e.[IssueReturnDate] AS [Date]) = d.[DATE]
	WHERE e.BatchId = @batchId
	AND d.[DATE] IS NULL

	-- SqliteIssueReturn can have issues/returns for entities, that were
	-- created on phone and not uploaded.  For such records, we have to first
	-- fill in entity Id
	UPDATE dbo.SqliteIssueReturn
	SET EntityId = se.EntityId
	FROM dbo.SqliteIssueReturn sqe
	INNER JOIN dbo.SqliteEntity se on sqe.SqliteEntityId = se.PhoneDbId
	AND se.BatchId <= @batchId -- entity has to come in same batch or before
	AND sqe.EntityId = 0
	AND se.EntityId > 0
	AND se.IsProcessed = 1

	-- select current max entity Id
	DECLARE @lastMaxId BIGINT
	SELECT @lastMaxId = ISNULL(MAX(Id),0) FROM dbo.IssueReturn

	-- Create Input/Issue Records
	INSERT INTO dbo.[IssueReturn]
	([EmployeeId], [DayId], [EntityId], 
	[EntityAgreementId], 
	[ItemMasterId],
	[TransactionDate], [TransactionType], [Quantity], [ActivityId],
	[SqliteIssueReturnId])

	SELECT 
	sqe.[EmployeeId], d.[Id] AS [DayId], sqe.EntityId, 
	CASE WHEN sqe.[AgreementId] = 0 THEN NULL ELSE sqe.[AgreementId] END,
	sqe.[ItemId],
	CAST(sqe.[IssueReturnDate] AS [Date]), sqe.[TranType], sqe.[Quantity], sqa.ActivityId,
	sqe.[Id]

	FROM dbo.SqliteIssueReturn sqe
	INNER JOIN dbo.[Day] d ON d.[DATE] = CAST(sqe.[IssueReturnDate] AS [Date])
	-- this join is to get activity Id
	INNER JOIN dbo.SqliteAction sqa on sqa.EmployeeId = sqe.EmployeeId
			AND sqa.[At] = sqe.IssueReturnDate
			AND sqa.PhoneDbId = sqe.ActivityId
	WHERE sqe.BatchId = @batchId AND sqe.IsProcessed = 0
	AND sqe.EntityId > 0
	ORDER BY sqe.Id

	-- now we need to update the id in SqliteIssueReturn table
	UPDATE dbo.SqliteIssueReturn
	SET IssueReturnId = e.Id,
	IsProcessed = 1,
	DateUpdated = SYSUTCDATETIME()
	FROM dbo.SqliteIssueReturn se
	INNER JOIN dbo.[IssueReturn] e on se.Id = e.SqliteIssueReturnId
	AND se.BatchId = @batchId
	AND e.Id > @lastMaxId
END
GO

-- 25.4.2019
ALTER TABLE [dbo].[FeatureControl]
ADD	
	[FieldActivityReportFeature] BIT NOT NULL DEFAULT 0,
	[EntityProgressReportFeature] BIT NOT NULL DEFAULT 0,
	[AbsenteeReportFeature] BIT NOT NULL DEFAULT 0,
	[AppSignUpReportFeature] BIT  NOT NULL DEFAULT 0,
	[AppSignInReportFeature] BIT  NOT NULL DEFAULT 0,
	[ActivityReportFeature] BIT  NOT NULL DEFAULT 0,
	[ActivityByTypeReportFeature] BIT  NOT NULL DEFAULT 0,
	[MAPFeature] BIT  NOT NULL DEFAULT 0
GO

-- 28.4.2019
ALTER PROCEDURE [dbo].[ClearEmployeeData]
	@employeeId BIGINT
AS
BEGIN
	BEGIN TRY

		BEGIN TRANSACTION

		-- Delete Activity Data
		DECLARE @activity Table ( Id BIGINT )
		DECLARE @image TABLE (Id BIGINT)

		-- first find out the activity ids that need to be deleted
		INSERT INTO @activity (Id)
		SELECT a.id from dbo.Activity a
		INNER JOIN dbo.EmployeeDay ed on ed.Id = a.employeeDayId
		and ed.TenantEmployeeId = @employeeId

		-- delete activity images
		-- store image Ids first - delete images at the end, as we need to delete images for payment as well;
		INSERT INTO @image (Id)
		SELECT ImageId FROM dbo.ActivityImage ai 
					 INNER JOIN @activity a on ai.ActivityId = a.Id

		print 'ActivityImage'
		DELETE FROM dbo.ActivityImage
		WHERE ActivityId IN (SELECT id FROM @activity)

		print 'ActivityContact'
		DELETE FROM dbo.ActivityContact WHERE ActivityId in (SELECT ID FROM @activity)


		print 'SqliteEntityWorkFlow'
		-- this table is no longer used;
		DELETE FROM dbo.SqliteEntityWorkFlow WHERE  ActivityId in (SELECT ID FROM @activity)

		print 'SqliteEntityWorkFlowV2'
		DELETE FROM dbo.SqliteEntityWorkFlowV2 WHERE EmployeeId = @employeeId


		print 'Activity'
		DELETE from dbo.Activity WHERE id in (SELECT Id from @activity)

		-----------------
		print 'dbo.DistanceCalcErrorLog'
		DELETE from dbo.DistanceCalcErrorLog
		WHERE id in
		(SELECT l.id from dbo.distanceCalcErrorLog l
		INNER JOIN dbo.Tracking t on l.TrackingId = t.Id
		INNER JOIN dbo.EmployeeDay ed on ed.Id = t.employeeDayId
		and ed.TenantEmployeeId = @employeeId)

		print 'dbo.Tracking'
		DELETE from dbo.Tracking
		WHERE id in
		(SELECT t.id from dbo.Tracking t
		INNER JOIN dbo.EmployeeDay ed on ed.Id = t.employeeDayId
		and ed.TenantEmployeeId = @employeeId)

		print 'employeeDay'
		DELETE from dbo.employeeDay WHERE TenantEmployeeId = @employeeId

		print 'imei'
		DELETE from dbo.Imei WHERE TenantEmployeeId = @employeeId

		--DELETE FROM dbo.SqliteAction WHERE EmployeeId = @employeeId
		--DELETE FROM dbo.SqliteActionBatch WHERE EmployeeId = @employeeId

		-- delete expense Data as well
		print 'SqliteExpenseImage'
		DELETE FROM dbo.SqliteExpenseImage WHERE SqliteExpenseId in (SELECT Id FROM dbo.SqliteExpense WHERE EmployeeId = @employeeId)
		print 'SqliteExpense'
		DELETE FROM dbo.SqliteExpense WHERE EmployeeId = @employeeId

		-- Delete SqliteOrder data
		print 'SqliteOrderItem'
		DELETE FROM dbo.SqliteOrderItem WHERE SqliteOrderId IN (SELECT Id FROM dbo.[SqliteOrder] WHERE EmployeeId = @employeeId)
		print 'SqliteOrderImage'
		DELETE FROM dbo.SqliteOrderImage WHERE SqliteOrderId IN (SELECT Id FROM dbo.[SqliteOrder] WHERE EmployeeId = @employeeId)
		print '[SqliteOrder]'
		DELETE FROM dbo.[SqliteOrder] WHERE EmployeeId = @employeeId

		-- delete SqlLiteAction Data as well
		print 'SqliteActionImage'
		DELETE FROM dbo.SqliteActionImage where SqliteActionId in (SELECT id FROM dbo.SqliteAction WHERE EmployeeId = @employeeId)
		print 'SqliteActionContact'
		DELETE FROM dbo.SqliteActionContact WHERE SqliteActionId in (SELECT id FROM dbo.SqliteAction WHERE EmployeeId = @employeeId)
		print 'SqliteActionLocation'
		DELETE FROM dbo.SqliteActionLocation WHERE SqliteActionId in (SELECT id FROM dbo.SqliteAction WHERE EmployeeId = @employeeId)
		print 'SqliteAction'
		DELETE FROM dbo.SqliteAction WHERE EmployeeId = @employeeId
		print 'SqliteActionDup'
		DELETE FROM dbo.SqliteActionDup WHERE EmployeeId = @employeeId

		-- delete SqlitePayment data as well
		print 'SqlitePaymentImage'
		DELETE FROM dbo.SqlitePaymentImage WHERE SqlitePaymentId in (SELECT Id FROM dbo.SqlitePayment WHERE EmployeeId = @employeeId)
		print 'SqlitePayment'
		DELETE FROM dbo.SqlitePayment WHERE EmployeeId = @employeeId

		-- Delete SqliteReturnOrder data
		print 'SqliteReturnOrderItem'
		DELETE FROM dbo.SqliteReturnOrderItem WHERE SqliteReturnOrderId IN (SELECT Id FROM dbo.[SqliteReturnOrder] WHERE EmployeeId = @employeeId)
		print '[SqliteReturnOrder]'
		DELETE FROM dbo.[SqliteReturnOrder] WHERE EmployeeId = @employeeId

		-- Delete SqliteEntity data
		DECLARE @SqliteEntity TABLE (Id BIGINT)
		INSERT INTO @SqliteEntity SELECT Id FROM dbo.[SqliteEntity] WHERE EmployeeId = @employeeId
		print '[SqliteEntityContact]'
		DELETE FROM dbo.[SqliteEntityContact] WHERE [SqliteEntityId] IN (SELECT Id FROM @SqliteEntity)
		print '[SqliteEntityCrop]'
		DELETE FROM dbo.[SqliteEntityCrop] WHERE [SqliteEntityId] IN (SELECT Id FROM @SqliteEntity)
		print '[SqliteEntityImage]'
		DELETE FROM dbo.[SqliteEntityImage] WHERE [SqliteEntityId] IN (SELECT Id FROM @SqliteEntity)
		print '[SqliteEntityLocation]'
		DELETE FROM dbo.[SqliteEntityLocation] WHERE [SqliteEntityId] IN (SELECT Id FROM @SqliteEntity)
		print '[SqliteEntity]'
		DELETE FROM dbo.[SqliteEntity] WHERE EmployeeId = @employeeId

		-- Delete SqliteLeave data
		print '[SqliteLeave]'
		DELETE FROM dbo.[SqliteLeave] WHERE EmployeeId = @employeeId

		-- Delete SqliteCancelledLeave data
		print '[SqliteCancelledLeave]'
		DELETE FROM dbo.[SqliteCancelledLeave] WHERE EmployeeId = @employeeId
		
		-- 
		print '[SqliteIssueReturn]'
		DELETE FROM dbo.[SqliteIssueReturn] WHERE EmployeeId = @employeeId

		-- Delete Device Log
		print '[SqliteDeviceLog]'
		DELETE FROM dbo.[SqliteDeviceLog] WHERE EmployeeId = @employeeId
		print 'SqliteActionBatch'
		DELETE FROM dbo.SqliteActionBatch WHERE EmployeeId = @employeeId

		-- store image ids first for processed expense data
		INSERT INTO @image (id)
		SELECT ImageId 
		FROM dbo.ExpenseItemImage WHERE ExpenseItemId IN 
		(SELECT ei.id FROM dbo.ExpenseItem ei
		INNER JOIN dbo.Expense e on ei.ExpenseId = e.Id
		AND e.EmployeeId = @employeeId)

		print 'ExpenseItemImage'
		DELETE FROM dbo.ExpenseItemImage WHERE ExpenseItemId IN 
		(SELECT ei.id FROM dbo.ExpenseItem ei INNER JOIN dbo.Expense e on ei.ExpenseId = e.Id AND e.EmployeeId = @employeeId)

		print 'ExpenseItem'
		DELETE FROM dbo.ExpenseItem WHERE ExpenseId in (SELECT Id FROM dbo.Expense WHERE EmployeeId = @employeeId)
		print 'ExpenseApproval'
		DELETE FROM dbo.ExpenseApproval WHERE ExpenseId in (SELECT Id FROM dbo.Expense WHERE EmployeeId = @employeeId)
		print 'Expense'
		DELETE FROM dbo.Expense WHERE EmployeeId = @employeeId

		-- Delete order Data
		print 'OrderItem'
		DELETE FROM dbo.OrderItem WHERE OrderId IN (SELECT Id FROM dbo.[Order] WHERE EmployeeId = @employeeId)

		INSERT INTO @image (id)
		SELECT oim.ImageId
		FROM dbo.OrderImage oim
		INNER JOIN dbo.[Order] o on o.Id = oim.OrderId
		AND o.EmployeeId = @employeeId

		print 'OrderImage'
		DELETE FROM dbo.OrderImage WHERE OrderId IN (SELECT Id FROM dbo.[Order] WHERE EmployeeId = @employeeId)

		print '[Order]'
		DELETE FROM dbo.[Order] WHERE EmployeeId = @employeeId


		-- Delete Return Order Data
		print 'ReturnOrderItem'
		DELETE FROM dbo.ReturnOrderItem WHERE ReturnOrderId IN (SELECT Id FROM dbo.[ReturnOrder] WHERE EmployeeId = @employeeId)
		print '[ReturnOrder]'
		DELETE FROM dbo.[ReturnOrder] WHERE EmployeeId = @employeeId

		-- DELETE Workflow data
		print 'EntityWorkFlowDetail'
		DELETE FROM dbo.EntityWorkFlowDetail
		WHERE ID IN (SELECT d.ID FROM dbo.EntityWorkFlowDetail d
		             INNER JOIN dbo.EntityWorkFlow p on d.EntityWorkFlowId = p.Id
					 AND p.EmployeeId = @employeeId)

		print 'EntityWorkFlow'
		DELETE FROM dbo.EntityWorkFlow WHERE EmployeeId = @employeeId

		print 'Issue/Return'
		DELETE FROM dbo.[IssueReturn] WHERE EmployeeId = @employeeId


		-- Delete Entity data
		DECLARE @Entity TABLE (Id BIGINT)
		INSERT INTO @Entity SELECT Id FROM dbo.[Entity] WHERE EmployeeId = @employeeId

		INSERT INTO @image (id)
		SELECT oim.ImageId
		FROM dbo.EntityImage oim
		INNER JOIN dbo.[Entity] o on o.Id = oim.EntityId
		AND o.EmployeeId = @employeeId

		print '[EntityContact]'
		DELETE FROM dbo.[EntityContact] WHERE [EntityId] IN (SELECT Id FROM @Entity)
		print '[EntityCrop]'
		DELETE FROM dbo.[EntityCrop] WHERE [EntityId] IN (SELECT Id FROM @Entity)
		print 'EntityImage'
		DELETE FROM dbo.EntityImage WHERE EntityId IN (SELECT Id FROM @Entity)
		print 'EntityAgreement'
		-- clear foreign key reference first
		UPDATE dbo.[IssueReturn] SET EntityAgreementId = NULL
		WHERE EntityAgreementId IN 
		(
		   SELECT ID FROM dbo.EntityAgreement WHERE EntityId IN (SELECT Id FROM @Entity)
		)

		DELETE FROM dbo.EntityAgreement WHERE EntityId IN (SELECT Id FROM @Entity)

		-- User 1 has created an entity
		-- User 2 has created a workflow based on this entity
		-- Question: Should we delete the workflow created by user 2 on this entity
		-- Answer: Basic use of this script is to delete the data that is created by test users
		--         during testing on live site.  So the answer is yes.


		print 'EntityWorkFlowDetail - again'
		DELETE FROM dbo.EntityWorkFlowDetail
		WHERE ID IN (SELECT d.ID FROM dbo.EntityWorkFlowDetail d
		             INNER JOIN dbo.EntityWorkFlow p on d.EntityWorkFlowId = p.Id
					 AND p.EntityId in (SELECT Id FROM @Entity))

		print 'EntityWorkFlow - again'
		DELETE FROM dbo.EntityWorkFlow WHERE EntityId in (SELECT Id FROM @Entity)

		print 'IssueReturn - again'
		DELETE FROM dbo.[IssueReturn] WHERE EntityId in (SELECT Id FROM @Entity)

		print '[Entity]'
		DELETE FROM dbo.[Entity] WHERE ID in (SELECT Id from @Entity)

		--
		-- Delete Payment Data
		--
		INSERT INTO @image (id)
		SELECT pim.ImageId
		FROM dbo.PaymentImage pim
		INNER JOIN dbo.Payment p on p.Id = pim.PaymentId
		AND p.EmployeeId = @employeeId

		print 'PaymentImage'
		DELETE FROM dbo.PaymentImage WHERE PaymentId IN (SELECT Id FROM dbo.[Payment] WHERE EmployeeId = @employeeId)
		print '[Payment]'
		DELETE FROM dbo.[Payment] WHERE EmployeeId = @employeeId

		-- DELETE IMAGES
		DELETE FROM dbo.[Image]
		WHERE Id in (SELECT Id FROM @image)

		-- CLEAR DEVICE LOG
		print 'SqliteDeviceLog'
		DELETE FROM dbo.SqliteDeviceLog WHERE EmployeeId = @employeeId

		print 'TenantEmployee'
		DELETE from dbo.TenantEmployee WHERE id = @employeeId
		COMMIT
	END TRY

	BEGIN CATCH
		PRINT 'Inside Catch...'
		PRINT ERROR_MESSAGE()
		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:ClearEmployeeData', ERROR_MESSAGE()

		ROLLBACK TRANSACTION
		throw;

	END CATCH
END	
GO

update dbo.WorkFlowSchedule set IsActive = 0 
WHERE Phase = 'Germination'
GO

update dbo.CodeTable set IsActive = 0
WHERE CodeType = 'CropType' and CodeName <> 'Gherkins'
GO

-- 30.4.2019
DELETE FROM dbo.CodeTable WHERE CodeType = 'Department'
GO
DELETE FROM dbo.CodeTable WHERE CodeType = 'Designation'
GO
INSERT INTO dbo.CodeTable
(CodeType, CodeName, CodeValue, DisplaySequence)
VALUES
('Department', 'AGRI-ACCOUNTS', 'AGRI-ACCOUNTS', 10),
('Department', 'AGRI-OPERATIONS','AGRI-OPERATIONS', 20),
('Department', 'AGRI-PROCUREMENT','AGRI-PROCUREMENT', 30),
('Department', 'AGRI-STORES','AGRI-STORES', 40),
('Department', 'AGRI-STORES&PROCUREMENT','AGRI-STORES&PROCUREMENT', 50),
('Department', 'AGRI-SUST DVPT','AGRI-SUST DVPT', 60)
GO

INSERT INTO dbo.CodeTable
(CodeType, CodeName, CodeValue, DisplaySequence)
VALUES
('Designation', 'ASSISTANT', 'ASSISTANT', 10),
('Designation', 'ASSISTANT MANAGER','ASSISTANT MANAGER', 20),
('Designation', 'ASST. GENERAL MANAGER','ASST. GENERAL MANAGER', 30),
('Designation', 'EXECUTIVE','EXECUTIVE', 40),
('Designation', 'GENERAL MANAGER','GENERAL MANAGER', 50),
('Designation', 'MANAGER','MANAGER', 60),
('Designation', 'OFFICER','OFFICER', 70),
('Designation', 'ON-CONTRACT','ON-CONTRACT', 80),
('Designation', 'SR. EXECUTIVE','SR. EXECUTIVE', 90),
('Designation', 'SR. MANAGER','SR. MANAGER', 100),
('Designation', 'SUPERVISOR','SUPERVISOR',110)
GO

-- 01.05.2019
update dbo.CodeTable
set IsActive = 0
where CodeType = 'CustomerType' and CodeValue <> 'Farmer'
GO
