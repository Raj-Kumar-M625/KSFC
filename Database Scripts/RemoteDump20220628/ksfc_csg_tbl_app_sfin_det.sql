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
-- Table structure for table `tbl_app_sfin_det`
--

DROP TABLE IF EXISTS `tbl_app_sfin_det`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_app_sfin_det` (
  `app_sisfin_id` int NOT NULL AUTO_INCREMENT,
  `app_sis_id` int NOT NULL,
  `finyear_code` mediumint DEFAULT NULL,
  `fincomp_cd` int NOT NULL,
  `enq_finamt` decimal(10,2) DEFAULT NULL,
  `wh_prov` tinyint(1) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) DEFAULT NULL,
  `modified_by` varchar(50) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  PRIMARY KEY (`app_sisfin_id`),
  KEY `fk_tbl_app_sfin_det_tbl_app_sis_det` (`app_sis_id`),
  KEY `fk_tbl_app_sfin_det_tbl_finyear_cdtab` (`finyear_code`),
  KEY `fk_tbl_app_sfin_det_tbl_fincomp_cdtab` (`fincomp_cd`),
  CONSTRAINT `fk_tbl_app_sfin_det_tbl_app_sis_det` FOREIGN KEY (`app_sis_id`) REFERENCES `tbl_app_sis_det` (`app_sis_id`),
  CONSTRAINT `fk_tbl_app_sfin_det_tbl_fincomp_cdtab` FOREIGN KEY (`fincomp_cd`) REFERENCES `tbl_fincomp_cdtab` (`fincomp_cd`),
  CONSTRAINT `fk_tbl_app_sfin_det_tbl_finyear_cdtab` FOREIGN KEY (`finyear_code`) REFERENCES `tbl_finyear_cdtab` (`finyear_code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_app_sfin_det`
--

LOCK TABLES `tbl_app_sfin_det` WRITE;
/*!40000 ALTER TABLE `tbl_app_sfin_det` DISABLE KEYS */;
/*!40000 ALTER TABLE `tbl_app_sfin_det` ENABLE KEYS */;
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

-- Dump completed on 2022-06-28 19:18:57
