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
-- Table structure for table `tlq_cdtab`
--

DROP TABLE IF EXISTS `tlq_cdtab`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tlq_cdtab` (
  `TLQ_CD` int NOT NULL,
  `TLQ_NAM` varchar(25) DEFAULT NULL COMMENT 'Taluka Name',
  `DIST_CD` tinyint DEFAULT NULL COMMENT 'District Code Ref: dist_cdtab (dist_cd)',
  `TLQ_INDZONE` tinyint DEFAULT NULL,
  `TLQ_NAME_KANNADA` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `TLQ_LGDCODE` int DEFAULT NULL,
  `TLQ_BHOOMICODE` int DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `modified_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  PRIMARY KEY (`TLQ_CD`),
  KEY `fk_TLQ_CDTAB_DIST_CDTAB` (`DIST_CD`),
  CONSTRAINT `fk_TLQ_CDTAB_DIST_CDTAB` FOREIGN KEY (`DIST_CD`) REFERENCES `dist_cdtab` (`DIST_CD`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='Taluk Details';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tlq_cdtab`
--

LOCK TABLES `tlq_cdtab` WRITE;
/*!40000 ALTER TABLE `tlq_cdtab` DISABLE KEYS */;
INSERT INTO `tlq_cdtab` VALUES (1,'Taluka 1',1,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(2,'Taluka 2',1,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(3,'Taluka 3',2,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(4,'Taluka 4',2,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(5,'Taluka 1',3,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(6,'Taluka 1',4,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(7,'Taluka 1',5,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(8,'Taluka 1',6,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(9,'Taluka 1',7,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(10,'Taluka 1',8,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(19,'Taluka 1',9,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(20,'Taluka 1',10,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(21,'Taluka 1',11,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(22,'Taluka 1',12,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(23,'Taluka 1',13,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(24,'Taluka 1',14,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(25,'Taluka 1',15,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(26,'Taluka 1',16,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(27,'Taluka 1',17,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(28,'Taluka 1',18,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(29,'Taluka 1',19,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(30,'Taluka 1',20,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(32,'Taluka 1',22,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(33,'Taluka 1',23,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(34,'Taluka 1',24,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(35,'Taluka 1',25,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(36,'Taluka 1',26,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(37,'Taluka 1',27,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(38,'Taluka 1',28,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(39,'Taluka 1',29,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(40,'Taluka 1',30,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(41,'Taluka 1',31,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `tlq_cdtab` ENABLE KEYS */;
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

-- Dump completed on 2022-06-28 19:19:57
