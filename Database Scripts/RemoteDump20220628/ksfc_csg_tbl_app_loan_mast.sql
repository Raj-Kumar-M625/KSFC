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
-- Table structure for table `tbl_app_loan_mast`
--

DROP TABLE IF EXISTS `tbl_app_loan_mast`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_app_loan_mast` (
  `ln_mast_id` bigint NOT NULL AUTO_INCREMENT,
  `ln_offc` tinyint DEFAULT NULL,
  `ln_unit` int DEFAULT NULL,
  `ln_sno` int DEFAULT NULL,
  `ln_no` bigint DEFAULT NULL,
  `ln_ty` int DEFAULT NULL,
  `ln_san_amt` decimal(15,2) DEFAULT NULL,
  `ln_san_dt` date DEFAULT NULL,
  `ln_schm` int DEFAULT NULL,
  `ln_stat` int DEFAULT NULL,
  `ln_intr_low` decimal(10,2) DEFAULT NULL,
  `ln_intr_high` decimal(10,2) DEFAULT NULL,
  `ln_int_reb` decimal(10,2) DEFAULT NULL,
  `ln_pmode` int DEFAULT NULL,
  `ln_imode` int DEFAULT NULL,
  `ln_mort_prd` int DEFAULT NULL,
  `unit_id` int DEFAULT NULL,
  `ln_pur_ty` int DEFAULT NULL,
  `ln_sub` int DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) DEFAULT NULL,
  `modified_by` varchar(50) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`ln_mast_id`),
  UNIQUE KEY `ln_no` (`ln_no`),
  KEY `FK_tbl_app_loan_mast_offc_cdtab` (`ln_offc`),
  KEY `fk_tbl_app_loan_mast_tbl_unit_mast` (`ln_unit`),
  KEY `fk_tbl_app_loan_mast_tbl_loan_type_cdtab` (`ln_ty`),
  KEY `fk_tbl_app_loan_mast_tbl_scheme_cdtab` (`ln_schm`),
  CONSTRAINT `FK_tbl_app_loan_mast_offc_cdtab` FOREIGN KEY (`ln_offc`) REFERENCES `offc_cdtab` (`OFFC_CD`),
  CONSTRAINT `fk_tbl_app_loan_mast_tbl_loan_type_cdtab` FOREIGN KEY (`ln_ty`) REFERENCES `tbl_loan_type_cdtab` (`loan_type`),
  CONSTRAINT `fk_tbl_app_loan_mast_tbl_scheme_cdtab` FOREIGN KEY (`ln_schm`) REFERENCES `tbl_scheme_cdtab` (`scheme_cd`),
  CONSTRAINT `fk_tbl_app_loan_mast_tbl_unit_mast` FOREIGN KEY (`ln_unit`) REFERENCES `tbl_unit_mast` (`ut_cd`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_app_loan_mast`
--

LOCK TABLES `tbl_app_loan_mast` WRITE;
/*!40000 ALTER TABLE `tbl_app_loan_mast` DISABLE KEYS */;
INSERT INTO `tbl_app_loan_mast` VALUES (1,11,1,11,98765432101,1,100000.00,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,NULL,NULL,'0');
/*!40000 ALTER TABLE `tbl_app_loan_mast` ENABLE KEYS */;
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

-- Dump completed on 2022-06-28 19:20:09
