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
-- Table structure for table `tbl_empchair_det`
--

DROP TABLE IF EXISTS `tbl_empchair_det`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_empchair_det` (
  `empchair_id` int NOT NULL AUTO_INCREMENT,
  `emp_id` varchar(8) DEFAULT NULL,
  `offc_cd` tinyint NOT NULL,
  `tges_code` varchar(5) NOT NULL,
  `chair_code` int NOT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `modified_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  PRIMARY KEY (`empchair_id`),
  KEY `fk_tbl_empchair_det_tbl_trg_employee` (`emp_id`),
  KEY `fk_tbl_empchair_det_OFFC_CDTAB` (`offc_cd`),
  KEY `fk_tbl_empchair_det_tbl_trg_emp_grade` (`tges_code`),
  KEY `fk_tbl_empchair_det_tbl_chair_cdtab` (`chair_code`),
  CONSTRAINT `fk_tbl_empchair_det_OFFC_CDTAB` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`),
  CONSTRAINT `fk_tbl_empchair_det_tbl_chair_cdtab` FOREIGN KEY (`chair_code`) REFERENCES `tbl_chair_cdtab` (`chair_code`),
  CONSTRAINT `fk_tbl_empchair_det_tbl_trg_emp_grade` FOREIGN KEY (`tges_code`) REFERENCES `tbl_trg_emp_grade` (`tges_code`),
  CONSTRAINT `fk_tbl_empchair_det_tbl_trg_employee` FOREIGN KEY (`emp_id`) REFERENCES `tbl_trg_employee` (`tey_ticket_num`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_empchair_det`
--

LOCK TABLES `tbl_empchair_det` WRITE;
/*!40000 ALTER TABLE `tbl_empchair_det` DISABLE KEYS */;
INSERT INTO `tbl_empchair_det` VALUES (1,'0001',11,'A32',1,NULL,NULL,NULL,NULL,NULL,NULL,'0');
/*!40000 ALTER TABLE `tbl_empchair_det` ENABLE KEYS */;
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

-- Dump completed on 2022-06-28 19:19:51
