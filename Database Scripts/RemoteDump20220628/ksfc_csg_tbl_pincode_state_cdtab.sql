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
-- Table structure for table `tbl_pincode_state_cdtab`
--

DROP TABLE IF EXISTS `tbl_pincode_state_cdtab`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_pincode_state_cdtab` (
  `pincode_state_cd` int NOT NULL AUTO_INCREMENT,
  `pincode_state_desc` varchar(200) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) DEFAULT NULL,
  `modified_by` varchar(50) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  PRIMARY KEY (`pincode_state_cd`)
) ENGINE=InnoDB AUTO_INCREMENT=37 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_pincode_state_cdtab`
--

LOCK TABLES `tbl_pincode_state_cdtab` WRITE;
/*!40000 ALTER TABLE `tbl_pincode_state_cdtab` DISABLE KEYS */;
INSERT INTO `tbl_pincode_state_cdtab` VALUES (1,'ANDHRA PRADESH',_binary '',_binary '\0','1',NULL,'2022-05-26 11:27:13',NULL),(2,'ARUNACHAL PRADESH',_binary '',_binary '\0','1',NULL,'2022-05-26 11:27:13',NULL),(3,'ASSAM',_binary '',_binary '\0','1',NULL,'2022-05-26 11:27:13',NULL),(4,'BIHAR',_binary '',_binary '\0','1',NULL,'2022-05-26 11:27:13',NULL),(5,'CHHATTISGARH',_binary '',_binary '\0','1',NULL,'2022-05-26 11:27:13',NULL),(6,'GOA',_binary '',_binary '\0','1',NULL,'2022-05-26 11:27:13',NULL),(7,'GUJARAT',_binary '',_binary '\0','1',NULL,'2022-05-26 11:27:13',NULL),(8,'HARYANA',_binary '',_binary '\0','1',NULL,'2022-05-26 11:27:13',NULL),(9,'HIMACHAL PRADESH',_binary '',_binary '\0','1',NULL,'2022-05-26 11:27:13',NULL),(10,'JAMMU AND KASHMIR',_binary '',_binary '\0','1',NULL,'2022-05-26 11:27:13',NULL),(11,'JHARKHAND',_binary '',_binary '\0','1',NULL,'2022-05-26 11:27:13',NULL),(12,'KARNATAKA',_binary '',_binary '\0','1',NULL,'2022-05-26 11:27:13',NULL),(13,'KERALA',_binary '',_binary '\0','1',NULL,'2022-05-26 11:27:13',NULL),(14,'MADHYA PRADESH',_binary '',_binary '\0','1',NULL,'2022-05-26 11:27:13',NULL),(15,'MAHARASHTRA',_binary '',_binary '\0','1',NULL,'2022-05-26 11:27:13',NULL),(16,'MANIPUR',_binary '',_binary '\0','1',NULL,'2022-05-26 11:27:13',NULL),(17,'MEGHALAYA',_binary '',_binary '\0','1',NULL,'2022-05-26 11:27:13',NULL),(18,'MIZORAM',_binary '',_binary '\0','1',NULL,'2022-05-26 11:27:13',NULL),(19,'NAGALAND',_binary '',_binary '\0','1',NULL,'2022-05-26 11:27:13',NULL),(20,'ODISHA',_binary '',_binary '\0','1',NULL,'2022-05-26 11:27:13',NULL),(21,'PUNJAB',_binary '',_binary '\0','1',NULL,'2022-05-26 11:27:13',NULL),(22,'RAJASTHAN',_binary '',_binary '\0','1',NULL,'2022-05-26 11:27:13',NULL),(23,'SIKKIM',_binary '',_binary '\0','1',NULL,'2022-05-26 11:27:13',NULL),(24,'TAMIL NADU',_binary '',_binary '\0','1',NULL,'2022-05-26 11:27:13',NULL),(25,'TELANGANA',_binary '',_binary '\0','1',NULL,'2022-05-26 11:27:13',NULL),(26,'TRIPURA',_binary '',_binary '\0','1',NULL,'2022-05-26 11:27:13',NULL),(27,'UTTAR PRADESH',_binary '',_binary '\0','1',NULL,'2022-05-26 11:27:13',NULL),(28,'UTTARAKHAND',_binary '',_binary '\0','1',NULL,'2022-05-26 11:27:13',NULL),(29,'WEST BENGAL',_binary '',_binary '\0','1',NULL,'2022-05-26 11:27:13',NULL),(30,'ANDAMAN AND NICOBAR ISLANDS',_binary '',_binary '\0','1',NULL,'2022-05-26 11:27:13',NULL),(31,'CHANDIGARH',_binary '',_binary '\0','1',NULL,'2022-05-26 11:27:13',NULL),(32,'DELHI',_binary '',_binary '\0','1',NULL,'2022-05-26 11:27:13',NULL),(33,'LADAKH',_binary '',_binary '\0','1',NULL,'2022-05-26 11:27:13',NULL),(34,'LAKSHADWEEP',_binary '',_binary '\0','1',NULL,'2022-05-26 11:27:13',NULL),(35,'PUDUCHERRY',_binary '',_binary '\0','1',NULL,'2022-05-26 11:27:13',NULL),(36,'THE DADRA AND NAGAR HAVELI AND DAMAN AND DIU',_binary '',_binary '\0','1',NULL,'2022-05-26 11:27:13',NULL);
/*!40000 ALTER TABLE `tbl_pincode_state_cdtab` ENABLE KEYS */;
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

-- Dump completed on 2022-06-28 19:20:23
