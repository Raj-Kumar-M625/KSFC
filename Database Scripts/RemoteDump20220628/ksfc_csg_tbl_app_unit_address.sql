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
-- Table structure for table `tbl_app_unit_address`
--

DROP TABLE IF EXISTS `tbl_app_unit_address`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_app_unit_address` (
  `ut_address_rowid` int NOT NULL AUTO_INCREMENT,
  `ut_cd` int NOT NULL,
  `eg_no` bigint DEFAULT NULL,
  `addtype_cd` int NOT NULL,
  `ut_address` varchar(200) DEFAULT NULL,
  `ut_area` varchar(100) DEFAULT NULL,
  `ut_city` varchar(100) DEFAULT NULL,
  `ut_pincode` int DEFAULT NULL,
  `ut_telephone` int DEFAULT NULL,
  `ut_mobile` int DEFAULT NULL,
  `ut_alt_mobile` int DEFAULT NULL,
  `ut_email` varchar(50) DEFAULT NULL,
  `ut_alt_email` varchar(50) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) DEFAULT NULL,
  `modified_by` varchar(50) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `ut_dist_cd` tinyint DEFAULT NULL,
  `ut_tlq_cd` int DEFAULT NULL,
  `ut_hob_cd` int DEFAULT NULL,
  `ut_vil_cd` int DEFAULT NULL,
  `ut_website` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`ut_address_rowid`),
  KEY `FK_tbl_app_unit_address_tbl_app_unit_details` (`ut_cd`),
  KEY `FK_tbl_app_unit_address_tbl_address_cdtab` (`addtype_cd`),
  KEY `fk_tbl_app_unit_address_dist_cdtab` (`ut_dist_cd`),
  KEY `fk_tbl_app_unit_address_tlq_cdtab` (`ut_tlq_cd`),
  KEY `fk_tbl_app_unit_address_hob_cdtab` (`ut_hob_cd`),
  KEY `fk_tbl_app_unit_address_vil_cdtab` (`ut_vil_cd`),
  KEY `fk_tbl_app_unit_address_tbl_eg_num_mast` (`eg_no`),
  CONSTRAINT `fk_tbl_app_unit_address_dist_cdtab` FOREIGN KEY (`ut_dist_cd`) REFERENCES `dist_cdtab` (`DIST_CD`),
  CONSTRAINT `fk_tbl_app_unit_address_hob_cdtab` FOREIGN KEY (`ut_hob_cd`) REFERENCES `hob_cdtab` (`HOB_CD`),
  CONSTRAINT `FK_tbl_app_unit_address_tbl_address_cdtab` FOREIGN KEY (`addtype_cd`) REFERENCES `tbl_address_cdtab` (`addtype_cd`),
  CONSTRAINT `fk_tbl_app_unit_address_tbl_eg_num_mast` FOREIGN KEY (`eg_no`) REFERENCES `tbl_eg_num_mast` (`eg_no`),
  CONSTRAINT `fk_tbl_app_unit_address_tbl_unit_mast` FOREIGN KEY (`ut_cd`) REFERENCES `tbl_unit_mast` (`ut_cd`),
  CONSTRAINT `fk_tbl_app_unit_address_tlq_cdtab` FOREIGN KEY (`ut_tlq_cd`) REFERENCES `tlq_cdtab` (`TLQ_CD`),
  CONSTRAINT `fk_tbl_app_unit_address_vil_cdtab` FOREIGN KEY (`ut_vil_cd`) REFERENCES `vil_cdtab` (`VIL_CD`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_app_unit_address`
--

LOCK TABLES `tbl_app_unit_address` WRITE;
/*!40000 ALTER TABLE `tbl_app_unit_address` DISABLE KEYS */;
/*!40000 ALTER TABLE `tbl_app_unit_address` ENABLE KEYS */;
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

-- Dump completed on 2022-06-28 19:18:59
