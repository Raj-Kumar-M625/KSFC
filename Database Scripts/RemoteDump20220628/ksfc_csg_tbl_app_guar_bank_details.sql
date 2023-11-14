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
-- Table structure for table `tbl_app_guar_bank_details`
--

DROP TABLE IF EXISTS `tbl_app_guar_bank_details`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_app_guar_bank_details` (
  `app_guar_bank_id` int NOT NULL AUTO_INCREMENT,
  `offc_cd` tinyint DEFAULT NULL,
  `ut_cd` int DEFAULT NULL,
  `promoter_code` bigint NOT NULL,
  `guar_ac_type` int DEFAULT NULL,
  `guar_bank_name` varchar(20) DEFAULT NULL,
  `guar_bank_branch` varchar(20) DEFAULT NULL,
  `guar_ac_no` decimal(12,0) DEFAULT NULL,
  `guar_ifsc_id` int NOT NULL,
  `guar_bank_address` varchar(500) DEFAULT NULL,
  `guar_bank_state` varchar(100) DEFAULT NULL,
  `guar_bank_district` varchar(100) DEFAULT NULL,
  `guar_bank_taluka` varchar(100) DEFAULT NULL,
  `guar_bank_pincode` decimal(6,0) DEFAULT NULL,
  `guar_bank_ac_name` varchar(100) DEFAULT NULL,
  `guar_primary_bank` tinyint(1) DEFAULT NULL,
  `guar_cibil_score` decimal(10,0) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) DEFAULT NULL,
  `modified_by` varchar(50) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `eg_no` bigint DEFAULT NULL,
  PRIMARY KEY (`app_guar_bank_id`),
  KEY `fk_tbl_app_guar_bank_details_offc_cdtab` (`offc_cd`),
  KEY `FK_tbl_app_guar_bank_details_prom_cdtab` (`promoter_code`),
  KEY `fk_tbl_app_guar_bank_details_tbl_ifsc_master` (`guar_ifsc_id`),
  KEY `FK_tbl_app_guar_bank_details_tbl_unit_mast` (`ut_cd`),
  KEY `fk_tbl_app_guar_bank_details_tbl_eg_num_mast` (`eg_no`),
  CONSTRAINT `fk_tbl_app_guar_bank_details_offc_cdtab` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`),
  CONSTRAINT `FK_tbl_app_guar_bank_details_prom_cdtab` FOREIGN KEY (`promoter_code`) REFERENCES `tbl_prom_cdtab` (`promoter_code`),
  CONSTRAINT `fk_tbl_app_guar_bank_details_tbl_eg_num_mast` FOREIGN KEY (`eg_no`) REFERENCES `tbl_eg_num_mast` (`eg_no`),
  CONSTRAINT `fk_tbl_app_guar_bank_details_tbl_ifsc_master` FOREIGN KEY (`guar_ifsc_id`) REFERENCES `tbl_ifsc_master` (`ifsc_rowid`),
  CONSTRAINT `FK_tbl_app_guar_bank_details_tbl_unit_mast` FOREIGN KEY (`ut_cd`) REFERENCES `tbl_unit_mast` (`ut_cd`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_app_guar_bank_details`
--

LOCK TABLES `tbl_app_guar_bank_details` WRITE;
/*!40000 ALTER TABLE `tbl_app_guar_bank_details` DISABLE KEYS */;
/*!40000 ALTER TABLE `tbl_app_guar_bank_details` ENABLE KEYS */;
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

-- Dump completed on 2022-06-28 19:17:57
