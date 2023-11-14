-- MySQL dump 10.13  Distrib 8.0.29, for Win64 (x86_64)
--
-- Host: ksfccsgdb.c3d9d6stfnzl.ap-south-1.rds.amazonaws.com    Database: ksfc_csg
-- ------------------------------------------------------
-- Server version	8.0.28

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
SET @MYSQLDUMP_TEMP_LOG_BIN = @@SESSION.SQL_LOG_BIN;
SET @@SESSION.SQL_LOG_BIN= 0;

--
-- GTID state at the beginning of the backup 
--

SET @@GLOBAL.GTID_PURGED=/*!80000 '+'*/ '';

--
-- Table structure for table `tbl_eqry_det`
--

DROP TABLE IF EXISTS `tbl_eqry_det`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_eqry_det` (
  `eqry_det_rowid` int NOT NULL AUTO_INCREMENT,
  `enq_ref_no` int DEFAULT NULL,
  `enq_appl_name` varchar(100) DEFAULT NULL,
  `enq_address` varchar(200) DEFAULT NULL,
  `enq_state` varchar(100) DEFAULT NULL,
  `enq_district` varchar(100) DEFAULT NULL,
  `enq_pincode` int DEFAULT NULL,
  `enq_email` varchar(50) DEFAULT NULL,
  `unit_name` varchar(150) DEFAULT NULL,
  `dist_cd` tinyint DEFAULT NULL,
  `hob_cd` int DEFAULT NULL,
  `tlq_cd` int DEFAULT NULL,
  `vil_cd` int DEFAULT NULL,
  `unit_addresss` varchar(200) DEFAULT NULL,
  `pincode_cd` int DEFAULT NULL,
  `purp_cd` int DEFAULT NULL,
  `enq_repay_period` int DEFAULT NULL,
  `enq_loanamt` int DEFAULT NULL,
  `const_cd` tinyint DEFAULT NULL,
  `size_cd` int DEFAULT NULL,
  `prod_cd` int DEFAULT NULL,
  `ind_cd` int DEFAULT NULL,
  `offc_cd` tinyint DEFAULT NULL,
  `is_active` tinyint DEFAULT NULL,
  `is_deleted` tinyint DEFAULT NULL,
  `created_by` varchar(50) DEFAULT NULL,
  `modified_by` varchar(50) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `notes` varchar(1000) DEFAULT NULL,
  PRIMARY KEY (`eqry_det_rowid`),
  KEY `FK_tbl_eqry_det_tbl_enq_temptab` (`enq_ref_no`),
  KEY `FK_tbl_eqry_det_tbl_dist_cdtab` (`dist_cd`),
  KEY `fk_tbl_eqry_det_tbl_hob_cdtab` (`hob_cd`),
  KEY `FK_tbl_eqry_det_tbl_tlq_cdtab` (`tlq_cd`),
  KEY `FK_tbl_eqry_det_tbl_vil_cdtab` (`vil_cd`),
  KEY `FK_tbl_eqry_det_tbl_purp_cdtab` (`purp_cd`),
  KEY `FK_tbl_eqry_det_tbl_const_cdtab` (`const_cd`),
  KEY `FK_tbl_eqry_det_tbl_size_cdtabl` (`size_cd`),
  KEY `FK__tbl_eqry_det_tbl_offc_cdtab` (`offc_cd`),
  CONSTRAINT `FK__tbl_eqry_det_tbl_offc_cdtab` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`),
  CONSTRAINT `FK_tbl_eqry_det_tbl_const_cdtab` FOREIGN KEY (`const_cd`) REFERENCES `cnst_cdtab` (`CNST_CD`),
  CONSTRAINT `FK_tbl_eqry_det_tbl_dist_cdtab` FOREIGN KEY (`dist_cd`) REFERENCES `dist_cdtab` (`DIST_CD`),
  CONSTRAINT `FK_tbl_eqry_det_tbl_enq_temptab` FOREIGN KEY (`enq_ref_no`) REFERENCES `tbl_enq_temptab` (`enqtemp_id`),
  CONSTRAINT `fk_tbl_eqry_det_tbl_hob_cdtab` FOREIGN KEY (`hob_cd`) REFERENCES `hob_cdtab` (`HOB_CD`),
  CONSTRAINT `FK_tbl_eqry_det_tbl_purp_cdtab` FOREIGN KEY (`purp_cd`) REFERENCES `tbl_purp_cdtab` (`purp_cd`),
  CONSTRAINT `FK_tbl_eqry_det_tbl_size_cdtabl` FOREIGN KEY (`size_cd`) REFERENCES `tbl_size_cdtab` (`size_cd`),
  CONSTRAINT `FK_tbl_eqry_det_tbl_tlq_cdtab` FOREIGN KEY (`tlq_cd`) REFERENCES `tlq_cdtab` (`TLQ_CD`),
  CONSTRAINT `FK_tbl_eqry_det_tbl_vil_cdtab` FOREIGN KEY (`vil_cd`) REFERENCES `vil_cdtab` (`VIL_CD`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_eqry_det`
--

LOCK TABLES `tbl_eqry_det` WRITE;
/*!40000 ALTER TABLE `tbl_eqry_det` DISABLE KEYS */;
INSERT INTO `tbl_eqry_det` VALUES (1,108,'abc','RT Nagar','1','1',560043,'abc@gmail.com','abc',1,1,1,1,'RT Nagar',560043,1,2,6,NULL,1,1,1,11,1,0,'OOSPS1480G',NULL,'2022-05-23 12:09:37',NULL,'abc'),(2,109,'vasu','#45 5thcross 1st min bangalore','9','6',560022,'vasu@gmail.com','vishal',17,27,27,27,'#67 6th cross 8th main bangalore',560043,2,36,43,NULL,2,1,15,43,1,0,'KXSPS1431E','KXSPS1431E','2022-05-24 11:30:23','2022-05-24 11:31:58','generate note'),(3,110,'Abhishek','test','12','265',581102,'abc@gamil.com','Abhishek',2,3,3,3,'test',111111,2,12,12,NULL,2,600000,6,26,1,0,'CMEPS9748B',NULL,'2022-06-01 12:56:21',NULL,'test'),(4,111,'latha','nagar','12','275',572103,'latha789@gmail.com','abc',18,28,28,28,'nagar',573219,1,9,15,NULL,2,400000,4,50,1,0,'OOSPS1480G',NULL,'2022-06-02 11:16:46',NULL,'abc'),(5,112,'shivakumar','wedcfrvgtbnhjm','12','263',582211,'neelavarna2022@gamil.com','Neelavrana enterprises',25,35,35,35,'dfvgbhunjimk',582114,1,24,55,NULL,1,1606010,16,72,1,0,'FMHPM5207B','FMHPM5207B','2022-06-04 10:07:14','2022-06-04 10:09:26','dfghnjmk,l');
/*!40000 ALTER TABLE `tbl_eqry_det` ENABLE KEYS */;
UNLOCK TABLES;
SET @@SESSION.SQL_LOG_BIN = @MYSQLDUMP_TEMP_LOG_BIN;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2022-06-28 19:20:16
