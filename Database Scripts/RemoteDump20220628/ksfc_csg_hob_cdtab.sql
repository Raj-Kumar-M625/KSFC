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
-- Table structure for table `hob_cdtab`
--

DROP TABLE IF EXISTS `hob_cdtab`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `hob_cdtab` (
  `HOB_CD` int NOT NULL COMMENT 'Hobli Code',
  `HOB_NAM` varchar(25) DEFAULT NULL COMMENT 'Hobli Name',
  `TLQ_CD` int DEFAULT NULL COMMENT 'Taluka Code Ref: tlq_cdtab (tlq_cd)',
  `HOB_NAME_KANNADA` varchar(50) DEFAULT NULL COMMENT 'Hobli Name in Kannada',
  `HOB_LGDCODE` int DEFAULT NULL,
  `HOB_BHOOMICODE` int DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `modified_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  PRIMARY KEY (`HOB_CD`),
  KEY `fk_HOB_CDTAB_TLQ_CDTAB` (`TLQ_CD`),
  CONSTRAINT `fk_HOB_CDTAB_TLQ_CDTAB` FOREIGN KEY (`TLQ_CD`) REFERENCES `tlq_cdtab` (`TLQ_CD`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `hob_cdtab`
--

LOCK TABLES `hob_cdtab` WRITE;
/*!40000 ALTER TABLE `hob_cdtab` DISABLE KEYS */;
INSERT INTO `hob_cdtab` VALUES (1,'Hobli 1',1,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(2,'Hobli 2',2,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(3,'Hobli 3',3,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(4,'Hobli 4',4,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(5,'Hobli 1',5,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(6,'Hobli 1',6,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(7,'Hobli 1',7,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(8,'Hobli 1',8,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(9,'Hobli 1',9,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(10,'Hobli 1',10,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(19,'Hobli 1',19,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(20,'Hobli 1',20,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(21,'Hobli 1',21,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(22,'Hobli 1',22,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(23,'Hobli 1',23,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(24,'Hobli 1',24,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(25,'Hobli 1',25,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(26,'Hobli 1',26,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(27,'Hobli 1',27,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(28,'Hobli 1',28,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(29,'Hobli 1',29,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(30,'Hobli 1',30,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(32,'Hobli 1',32,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(33,'Hobli 1',33,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(34,'Hobli 1',34,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(35,'Hobli 1',35,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(36,'Hobli 1',36,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(37,'Hobli 1',37,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(38,'Hobli 1',38,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(39,'Hobli 1',39,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(40,'Hobli 1',40,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(41,'Hobli 1',41,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `hob_cdtab` ENABLE KEYS */;
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

-- Dump completed on 2022-06-28 19:19:40
