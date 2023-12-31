﻿START TRANSACTION;

-- Loan Accounting Module --- Added by Gagana


Insert into tbl_allc_cdtab (allc_cd,allc_dets,allc_flg,is_active,is_deleted) values 
(1,'Towards Land',1,b'1', b'0'),
 (2,'Towards Building ',1,b'1', b'0'),
 (3,'Towards Furniture',1,b'1', b'0'),
(4,'Towards Indeginous Machinery',1,b'1', b'0'),
(5,' Towards Imported Machniery',1,b'1', b'0'),
(6,'Interest on Previously Disbursed Loan',1,b'1', b'0'),
(7,'Towards Indigenous Plant And Machinery(Second Hand)',1,b'1', b'0'),
(8,'Towards Contingency on Plant And Machinery(Second Hand)',1,b'1', b'0'),
(9,'Towards Imported Plant And Machinary(New)',1,b'1', b'0'),
(10,'Towards Contingency on Plant And Machinery (New)',1,b'1', b'0'),
(11,'Towards Imported Plant And Machinery (Second Hand)',1,b'1', b'0'),
(12,'Towards Contingency on Imported Plant And Machinery (Second Hand)',1,b'1', b'0'),
(13,'Towards Furniture And Fixtures',1,b'1', b'0'),
(14,'Towards Contingency on Furniture And Fixtures',1,b'1', b'0'),
(15,'Towards Technical Knowhow Fee',1,b'1', b'0'),
(16,'Towards Interest During Implementation',1,b'1', b'0'),
(17,'Customs Duty',1,b'1', b'0'),
(18,'D.G Set',1,b'1', b'0'),
(19,'Q.C Equipments',1,b'1', b'0'),
(20,'Electro Medical Equipments',1,b'1', b'0'),
(21,'Towards Chassis',2,b'1', b'0'),
(22,'Erection And Commissioning',1,b'1', b'0'),
(23,'Power Wiring',1,b'1', b'0'),
(24,'Miscellaneous Fixed Assets',9,b'1', b'0'),
(25,'Computer/Office Equipments',1,b'1', b'0'),
(26,'Soft Loan',1,b'1', b'0'),
(27,'Seed Capital',1,b'1', b'0'),
(28,'Soft Seed Capital',1,b'1', b'0'),
(29,'Working Capital',1,b'1', b'0'),
(30,'Bridge Loan',1,b'1', b'0'),
(31,'Bridge Loan-Central Subsidy',1,b'1', b'0'),
(32,'NEF',1,b'1', b'0'),
(33,'Subsidy For Physically Handicapped',1,b'1', b'0'),
(34,'RSR-Repairs&Maintenance',1,b'1', b'0'),
(35,'RSR-W.C Margin',1,b'1', b'0'),
(36,'RSR-Pressing Creditors',1,b'1', b'0'),
(37,'RSR-Principal Dues',1,b'1', b'0'),
(38,'RSR-Statutory Loss',1,b'1', b'0'),
(39,'RSR-Cash Loss',1,b'1', b'0'),
(40,'RSR-Margin Money',1,b'1', b'0'),
(41,'RSR-Additional Capital Support',1,b'1', b'0'),
(42,'RSR-Contingency',1,b'1', b'0'),
(43,'Towards Additional Building',1,b'1', b'0'),
(44,'Towards Addlitional Machinery',1,b'1', b'0'),
(99,'Others',9,b'1', b'0'),
(45,'Towards Body Building',2,b'1', b'0'),
(76,'Towards Market Related Expenditure under AMARA scheme',1,b'1', b'0'),
(77,'Towards Advertisement Expenditure',1,b'1', b'0'),
(78,'Towards Market Research expenditure',1,b'1', b'0'),
(79,'Towards Sales Promotion Expenditure',1,b'1', b'0'),
(80,'Towards Investment in Show Rooms',1,b'1', b'0'),
(81,'Towards Investment in Sales Distribution Network',1,b'1', b'0'),
(82,'Towards Training & Other Related Expenditure',1,b'1', b'0'),
(46,'Towards L.C. Requirements',1,b'1', b'0'),
(47,'Towards F.L.C  Requirements',1,b'1', b'0'),
(48,'Towards Intangible Assets',1,b'1', b'0');


COMMIT;