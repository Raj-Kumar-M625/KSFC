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
-- Table structure for table `tbl_role_cdtab`
--

DROP TABLE IF EXISTS `tbl_role_cdtab`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_role_cdtab` (
  `role_id` int NOT NULL AUTO_INCREMENT,
  `role_desc` varchar(100) NOT NULL,
  `module_id` int NOT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `modified_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  PRIMARY KEY (`role_id`),
  KEY `fk_tbl_role_cdtab_tbl_module_cdtab` (`module_id`),
  CONSTRAINT `fk_tbl_role_cdtab_tbl_module_cdtab` FOREIGN KEY (`module_id`) REFERENCES `tbl_module_cdtab` (`module_id`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_role_cdtab`
--

LOCK TABLES `tbl_role_cdtab` WRITE;
/*!40000 ALTER TABLE `tbl_role_cdtab` DISABLE KEYS */;
INSERT INTO `tbl_role_cdtab` VALUES (1,'Enquiry Ackowledgment',1,_binary '',_binary '\0','1',NULL,'2022-05-26 11:54:44',NULL,NULL),(2,'Enquiry Acceptance',1,_binary '',_binary '\0','1',NULL,'2022-05-26 11:54:44',NULL,NULL),(3,'Loan Application Acknowledgment',1,_binary '',_binary '\0','1',NULL,'2022-05-26 11:54:44',NULL,NULL),(4,'Loan Application Acceptance',1,_binary '',_binary '\0','1',NULL,'2022-05-26 11:54:44',NULL,NULL),(5,'Assign Role',10,_binary '',_binary '\0','1',NULL,'2022-05-26 11:54:44',NULL,NULL),(6,'Creation of KSFC User',10,_binary '',_binary '\0','1',NULL,'2022-05-26 11:54:44',NULL,NULL),(7,'Appraisal Acknowledgment',2,_binary '',_binary '\0','1',NULL,'2022-05-26 11:54:44',NULL,NULL),(8,'Appraisal Entry',2,_binary '',_binary '\0','1',NULL,'2022-05-26 11:54:44',NULL,NULL),(9,'Appraisal Approval',2,_binary '',_binary '\0','1',NULL,'2022-05-26 11:54:44',NULL,NULL),(10,'Loan Sanctioning Authority',2,_binary '',_binary '\0','1',NULL,'2022-05-26 11:54:44',NULL,NULL),(11,'IDM User',3,_binary '',_binary '\0','1',NULL,'2022-05-26 11:54:44',NULL,NULL),(12,'Legal Documentation',3,_binary '',_binary '\0','1',NULL,'2022-05-26 11:54:44',NULL,NULL),(13,'Inspection Activity',3,_binary '',_binary '\0','1',NULL,'2022-05-26 11:54:44',NULL,NULL),(14,'Disbursement Activity',3,_binary '',_binary '\0','1',NULL,'2022-05-26 11:54:44',NULL,NULL),(15,'Receipt Proposal Creation',6,_binary '',_binary '\0','1',NULL,'2022-05-26 11:54:44',NULL,NULL),(16,'Receipt Proposal Approval',6,_binary '',_binary '\0','1',NULL,'2022-05-26 11:54:44',NULL,NULL),(17,'Disbursement Proposal Creation',3,_binary '',_binary '\0','1',NULL,'2022-05-26 11:54:44',NULL,NULL),(18,'Disbursement Proposal Approval',3,_binary '',_binary '\0','1',NULL,'2022-05-26 11:54:44',NULL,NULL),(19,'Payment Proposal Creation',6,_binary '',_binary '\0','1',NULL,'2022-05-26 11:54:44',NULL,NULL),(20,'Payment Proposal Approval',6,_binary '',_binary '\0','1',NULL,'2022-05-26 11:54:44',NULL,NULL);
/*!40000 ALTER TABLE `tbl_role_cdtab` ENABLE KEYS */;
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

-- Dump completed on 2022-06-28 19:18:56
