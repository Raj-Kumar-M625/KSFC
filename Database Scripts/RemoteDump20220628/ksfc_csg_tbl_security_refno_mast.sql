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
-- Table structure for table `tbl_security_refno_mast`
--

DROP TABLE IF EXISTS `tbl_security_refno_mast`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_security_refno_mast` (
  `sec_refno_mast_id` int NOT NULL AUTO_INCREMENT,
  `security_cd` int NOT NULL,
  `loan_acc` bigint NOT NULL,
  `loan_sub` int DEFAULT NULL,
  `offc_cd` tinyint DEFAULT NULL,
  `sec_cd` smallint DEFAULT NULL,
  `pjsec_cd` mediumint DEFAULT NULL,
  `security_details` varchar(200) DEFAULT NULL,
  `security_value` decimal(15,2) DEFAULT NULL,
  `sec_name_holder` varchar(200) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) DEFAULT NULL,
  `modified_by` varchar(50) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`sec_refno_mast_id`),
  UNIQUE KEY `security_cd` (`security_cd`),
  KEY `fk_tbl_security_refno_mast_tbl_app_loan_mast` (`loan_acc`),
  KEY `fk_tbl_security_refno_mast_offc_cdtab` (`offc_cd`),
  KEY `fk_tbl_security_refno_mast_tbl_sec_cdtab` (`sec_cd`),
  KEY `fk_tbl_security_refno_mast_tbl_pjsec_cdtab` (`pjsec_cd`),
  CONSTRAINT `fk_tbl_security_refno_mast_offc_cdtab` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`),
  CONSTRAINT `fk_tbl_security_refno_mast_tbl_app_loan_mast` FOREIGN KEY (`loan_acc`) REFERENCES `tbl_app_loan_mast` (`ln_no`),
  CONSTRAINT `fk_tbl_security_refno_mast_tbl_pjsec_cdtab` FOREIGN KEY (`pjsec_cd`) REFERENCES `tbl_pjsec_cdtab` (`sec_code`),
  CONSTRAINT `fk_tbl_security_refno_mast_tbl_sec_cdtab` FOREIGN KEY (`sec_cd`) REFERENCES `tbl_sec_cdtab` (`sec_cd`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_security_refno_mast`
--

LOCK TABLES `tbl_security_refno_mast` WRITE;
/*!40000 ALTER TABLE `tbl_security_refno_mast` DISABLE KEYS */;
INSERT INTO `tbl_security_refno_mast` VALUES (1,1,98765432101,1,11,1,NULL,'New test case of this UC ',100000.00,NULL,_binary '',NULL,NULL,NULL,NULL,NULL,''),(2,2,98765432101,2,11,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `tbl_security_refno_mast` ENABLE KEYS */;
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

-- Dump completed on 2022-06-28 19:19:10
