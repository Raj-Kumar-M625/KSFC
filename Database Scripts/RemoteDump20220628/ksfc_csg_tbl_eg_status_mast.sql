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
-- Table structure for table `tbl_eg_status_mast`
--

DROP TABLE IF EXISTS `tbl_eg_status_mast`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_eg_status_mast` (
  `esm_code` int NOT NULL AUTO_INCREMENT,
  `esm_description` varchar(50) NOT NULL,
  `esm_flag` int NOT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `modified_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  PRIMARY KEY (`esm_code`)
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_eg_status_mast`
--

LOCK TABLES `tbl_eg_status_mast` WRITE;
/*!40000 ALTER TABLE `tbl_eg_status_mast` DISABLE KEYS */;
INSERT INTO `tbl_eg_status_mast` VALUES (1,'Check List Issued',1,_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(2,'Information Received',1,_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(3,'Referred to Legal Opinion',1,_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(4,'Received Legal Opinion',1,_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(5,'Referred for Security Inspection',1,_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(6,'Received Security Inspection Report',1,_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(7,'Placed before Project Clearence Committee',1,_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(8,'Referred for Pre-Appraisal Opinion',1,_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(9,'Received Pre-Appraisal Opinion',1,_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(10,'Proposal Deferred',1,_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(11,'Proposal Closed',1,_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(12,'Re-Placed before Project Clearence Committee',1,_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(13,'Proposal Rejected',1,_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(14,'Proposal Cleared by Project Clearence Committee',1,_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(15,'Loan Application Issued ',1,_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(16,'Loan Application Accepted',1,_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(17,'Application sent for Appraisal',1,_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(18,'Loan Application taken up for Appraisal',0,_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(19,'Referred for Expert Opinion from External Agency',2,_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(20,'Expert Opinion received from External Agency',2,_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(21,'Referred for Rating from External Rating Agency',2,_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(22,'Rating received from External Rating Agency',2,_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(23,'Loan Rejected',0,_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(24,'Loan Sanctioned',0,_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(25,'Proposal taken back by Entrepreneur',1,_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(26,'Enquiry Submitted',1,_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(27,'Enquiry Acknowledged',1,_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(28,'Enquiry under Scrutiny',1,_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(29,'Enquiry Accepted',1,_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL);
/*!40000 ALTER TABLE `tbl_eg_status_mast` ENABLE KEYS */;
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

-- Dump completed on 2022-06-28 19:19:53
