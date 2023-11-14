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
-- Table structure for table `tbl_app_guarnator`
--

DROP TABLE IF EXISTS `tbl_app_guarnator`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_app_guarnator` (
  `app_guar_id` int NOT NULL AUTO_INCREMENT,
  `promoter_code` bigint NOT NULL,
  `eg_no` bigint DEFAULT NULL,
  `offc_cd` tinyint DEFAULT NULL,
  `ut_cd` int DEFAULT NULL,
  `ut_name` varchar(100) DEFAULT NULL,
  `guar_name` varchar(35) DEFAULT NULL,
  `guar_gender` text,
  `guar_dob` date DEFAULT NULL,
  `guar_age` decimal(10,0) DEFAULT NULL,
  `name_father_spouse` varchar(150) DEFAULT NULL,
  `pclas_cd` int DEFAULT NULL,
  `psubclas_cd` int DEFAULT NULL,
  `dom_cd` int DEFAULT NULL,
  `guar_passport` varchar(20) DEFAULT NULL,
  `guar_pan` varchar(10) DEFAULT NULL,
  `guar_mobile_no` decimal(10,0) DEFAULT NULL,
  `guar_email` varchar(50) DEFAULT NULL,
  `prom_tel_no` decimal(10,0) DEFAULT NULL,
  `prom_photo` varchar(300) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) DEFAULT NULL,
  `modified_by` varchar(50) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  PRIMARY KEY (`app_guar_id`),
  KEY `FK_tbl_app_guarnator_prom_cdtab` (`promoter_code`),
  KEY `fk_tbl_app_guarnator_offc_cdtab` (`offc_cd`),
  KEY `fk_tbl_app_guarnator_tbl_pclas_cdtab` (`pclas_cd`),
  KEY `fk_tbl_app_guarnator_tbl_psubclas_cdtab` (`psubclas_cd`),
  KEY `fk_tbl_app_guarnator_tbl_domi_cdtab` (`dom_cd`),
  KEY `FK_tbl_app_guarnator_tbl_unit_mast` (`ut_cd`),
  KEY `FK_tbl_app_guarnator_tbl_eg_num_mast` (`eg_no`),
  CONSTRAINT `fk_tbl_app_guarnator_offc_cdtab` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`),
  CONSTRAINT `FK_tbl_app_guarnator_prom_cdtab` FOREIGN KEY (`promoter_code`) REFERENCES `tbl_prom_cdtab` (`promoter_code`),
  CONSTRAINT `fk_tbl_app_guarnator_tbl_domi_cdtab` FOREIGN KEY (`dom_cd`) REFERENCES `tbl_domi_cdtab` (`dom_cd`),
  CONSTRAINT `FK_tbl_app_guarnator_tbl_eg_num_mast` FOREIGN KEY (`eg_no`) REFERENCES `tbl_eg_num_mast` (`eg_no`),
  CONSTRAINT `fk_tbl_app_guarnator_tbl_pclas_cdtab` FOREIGN KEY (`pclas_cd`) REFERENCES `tbl_pclas_cdtab` (`pclas_cd`),
  CONSTRAINT `fk_tbl_app_guarnator_tbl_psubclas_cdtab` FOREIGN KEY (`psubclas_cd`) REFERENCES `tbl_psubclas_cdtab` (`psubclas_cd`),
  CONSTRAINT `FK_tbl_app_guarnator_tbl_unit_mast` FOREIGN KEY (`ut_cd`) REFERENCES `tbl_unit_mast` (`ut_cd`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_app_guarnator`
--

LOCK TABLES `tbl_app_guarnator` WRITE;
/*!40000 ALTER TABLE `tbl_app_guarnator` DISABLE KEYS */;
/*!40000 ALTER TABLE `tbl_app_guarnator` ENABLE KEYS */;
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

-- Dump completed on 2022-06-28 19:17:43
