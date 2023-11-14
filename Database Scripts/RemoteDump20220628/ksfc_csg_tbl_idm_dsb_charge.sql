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
-- Table structure for table `tbl_idm_dsb_charge`
--

DROP TABLE IF EXISTS `tbl_idm_dsb_charge`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_idm_dsb_charge` (
  `idm_dsb_charge_id` int NOT NULL AUTO_INCREMENT,
  `loan_acc` bigint DEFAULT NULL,
  `loan_sub` int DEFAULT NULL,
  `offc_cd` tinyint DEFAULT NULL,
  `security_cd` int DEFAULT NULL,
  `charge_type_cd` int DEFAULT NULL,
  `security_value` decimal(10,0) DEFAULT NULL,
  `noc_issue_by` varchar(100) DEFAULT NULL,
  `noc_isssue_to` varchar(100) DEFAULT NULL,
  `noc_date` date DEFAULT NULL,
  `auth_letter_by` varchar(100) DEFAULT NULL,
  `auth_letter_date` date DEFAULT NULL,
  `board_resolution_date` date DEFAULT NULL,
  `moe_insured_date` date DEFAULT NULL,
  `request_ltr_no` varchar(50) DEFAULT NULL,
  `request_ltr_date` date DEFAULT NULL,
  `bank_ifsc_cd` varchar(11) DEFAULT NULL,
  `bank_request_ltr_no` varchar(50) DEFAULT NULL,
  `bank_request_ltr_date` date DEFAULT NULL,
  `charge_purpose` varchar(100) DEFAULT NULL,
  `charge_details` varchar(500) DEFAULT NULL,
  `charge_conditions` varchar(500) DEFAULT NULL,
  `upload_document` varchar(300) DEFAULT NULL,
  `approved_emp_id` varchar(8) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) DEFAULT NULL,
  `modified_by` varchar(50) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  PRIMARY KEY (`idm_dsb_charge_id`),
  KEY `fk_tbl_idm_dsb_charge_offc_cdtab` (`offc_cd`),
  KEY `fk_tbl_idm_dsb_charge_tbl_app_loan_mast` (`loan_acc`),
  KEY `fk_tbl_idm_dsb_charge_tbl_security_refno_mast` (`security_cd`),
  KEY `fk_tbl_idm_dsb_charge_tbl_trg_employee` (`approved_emp_id`),
  KEY `fk_tbl_idm_dsb_charge_tbl_ifsc_master` (`bank_ifsc_cd`),
  CONSTRAINT `fk_tbl_idm_dsb_charge_offc_cdtab` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`),
  CONSTRAINT `fk_tbl_idm_dsb_charge_tbl_app_loan_mast` FOREIGN KEY (`loan_acc`) REFERENCES `tbl_app_loan_mast` (`ln_no`),
  CONSTRAINT `fk_tbl_idm_dsb_charge_tbl_ifsc_master` FOREIGN KEY (`bank_ifsc_cd`) REFERENCES `tbl_ifsc_master` (`ifsc_cd`),
  CONSTRAINT `fk_tbl_idm_dsb_charge_tbl_security_refno_mast` FOREIGN KEY (`security_cd`) REFERENCES `tbl_security_refno_mast` (`security_cd`),
  CONSTRAINT `fk_tbl_idm_dsb_charge_tbl_trg_employee` FOREIGN KEY (`approved_emp_id`) REFERENCES `tbl_trg_employee` (`tey_ticket_num`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_idm_dsb_charge`
--

LOCK TABLES `tbl_idm_dsb_charge` WRITE;
/*!40000 ALTER TABLE `tbl_idm_dsb_charge` DISABLE KEYS */;
INSERT INTO `tbl_idm_dsb_charge` VALUES (1,98765432101,1,11,1,1,100000,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `tbl_idm_dsb_charge` ENABLE KEYS */;
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

-- Dump completed on 2022-06-28 19:17:59
