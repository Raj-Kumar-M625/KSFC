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
-- Table structure for table `tbl_trg_emp_grade`
--

DROP TABLE IF EXISTS `tbl_trg_emp_grade`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_trg_emp_grade` (
  `tges_code` varchar(5) NOT NULL,
  `tges_desc` varchar(30) NOT NULL,
  `tegs_order` decimal(3,1) NOT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `modified_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  PRIMARY KEY (`tges_code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_trg_emp_grade`
--

LOCK TABLES `tbl_trg_emp_grade` WRITE;
/*!40000 ALTER TABLE `tbl_trg_emp_grade` DISABLE KEYS */;
INSERT INTO `tbl_trg_emp_grade` VALUES ('A00','All',0.0,_binary '',_binary '\0','1',NULL,'2022-05-25 11:46:04',NULL,NULL),('A01','EXECUTIVE DIRECTOR',1.0,_binary '',_binary '\0','1',NULL,'2022-05-25 11:46:04',NULL,NULL),('A02','GENERAL MANAGER',3.0,_binary '',_binary '\0','1',NULL,'2022-05-25 11:46:04',NULL,NULL),('A02.1','General Manager-AP',3.0,_binary '',_binary '\0','1',NULL,'2022-05-25 11:46:04',NULL,NULL),('A03','DY. GEN. MANAGER',6.0,_binary '',_binary '\0','1',NULL,'2022-05-25 11:46:04',NULL,NULL),('A03.1','Dy.Gen.Manager-AP ',6.0,_binary '',_binary '\0','1',NULL,'2022-05-25 11:46:04',NULL,NULL),('A04','ASST.GEN.MANAGER',9.0,_binary '',_binary '\0','1',NULL,'2022-05-25 11:46:04',NULL,NULL),('A04.0','Asst.Gen.Manager-AP',9.0,_binary '',_binary '\0','1',NULL,'2022-05-25 11:46:04',NULL,NULL),('A05','MANAGER(F&A)',12.0,_binary '',_binary '\0','1',NULL,'2022-05-25 11:46:04',NULL,NULL),('A06','MANAGER(TECH)',12.0,_binary '',_binary '\0','1',NULL,'2022-05-25 11:46:04',NULL,NULL),('A07','MANAGER(LEGAL)',12.0,_binary '',_binary '\0','1',NULL,'2022-05-25 11:46:04',NULL,NULL),('A10.4','DY.MANAGER(F&A)PP(PSP)',16.0,_binary '',_binary '\0','1',NULL,'2022-05-25 11:46:04',NULL,NULL),('A10.5','DY.MANAGER(F&A)(PSP)',16.0,_binary '',_binary '\0','1',NULL,'2022-05-25 11:46:04',NULL,NULL),('A16','Managing Dir.',0.5,_binary '',_binary '\0','1',NULL,'2022-05-25 11:46:04',NULL,NULL),('A17','Dy.Chf.Exe.offr',0.6,_binary '',_binary '\0','1',NULL,'2022-05-25 11:46:04',NULL,NULL),('A18','Dy.General Manager(Legal)',6.0,_binary '',_binary '\0','1',NULL,'2022-05-25 11:46:04',NULL,NULL),('A30.1','SR.MANAGER(F&A)-PP',11.0,_binary '',_binary '\0','1',NULL,'2022-05-25 11:46:04',NULL,NULL),('A30.2','SR.MANAGER (F&A) -APP',11.0,_binary '',_binary '\0','1',NULL,'2022-05-25 11:46:04',NULL,NULL),('A31','SR.MANAGER (TECH)',11.0,_binary '',_binary '\0','1',NULL,'2022-05-25 11:46:04',NULL,NULL),('A31.1','SR.MANAGER (TECH) -PP',11.0,_binary '',_binary '\0','1',NULL,'2022-05-25 11:46:04',NULL,NULL),('A31.2','SR.MANAGER (TECH) -APP',11.0,_binary '',_binary '\0','1',NULL,'2022-05-25 11:46:04',NULL,NULL),('A32','SR.MANAGER (LEGAL)',11.0,_binary '',_binary '\0','1',NULL,'2022-05-25 11:46:04',NULL,NULL),('A32.1','SR.MANAGER(LEGAL)PP',11.0,_binary '',_binary '\0','1',NULL,'2022-05-25 11:46:04',NULL,NULL),('A32.2','SR.MANAGER (LEGAL) -APP',11.0,_binary '',_binary '\0','1',NULL,'2022-05-25 11:46:04',NULL,NULL);
/*!40000 ALTER TABLE `tbl_trg_emp_grade` ENABLE KEYS */;
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

-- Dump completed on 2022-06-28 19:19:28
