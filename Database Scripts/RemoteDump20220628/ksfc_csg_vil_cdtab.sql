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
-- Table structure for table `vil_cdtab`
--

DROP TABLE IF EXISTS `vil_cdtab`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `vil_cdtab` (
  `VIL_CD` int NOT NULL AUTO_INCREMENT,
  `VIL_NAM` varchar(25) DEFAULT NULL COMMENT 'Village Name',
  `HOB_CD` int DEFAULT NULL COMMENT 'Hobli Code Ref: hob_cdtab (hob_cd)',
  `VIL_NAME_KANNADA` varchar(50) DEFAULT NULL COMMENT 'Village Name in Kannada',
  `VIL_LGDCODE` decimal(20,0) DEFAULT NULL COMMENT 'LGD code from KSRSAC',
  `VIL_BHOOMICODE` decimal(20,0) DEFAULT NULL COMMENT 'LGD code from Bhoomi Project',
  `CONSTMLA_CD` smallint DEFAULT NULL COMMENT 'MLA Constituency Code Ref: constmla_cdtab (constmla_cd)',
  `CONSTMP_CD` tinyint DEFAULT NULL COMMENT 'MP Constiutency Code Ref: constmp_cdtab (constmp_cd)',
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `modified_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  PRIMARY KEY (`VIL_CD`),
  KEY `fk_VIL_CDTAB_CONSTMLA_CDTAB` (`CONSTMLA_CD`),
  KEY `fk_VIL_CDTAB_CONSTMP_CDTAB` (`CONSTMP_CD`),
  CONSTRAINT `fk_VIL_CDTAB_CONSTMLA_CDTAB` FOREIGN KEY (`CONSTMLA_CD`) REFERENCES `constmla_cdtab` (`CONSTMLA_CD`),
  CONSTRAINT `fk_VIL_CDTAB_CONSTMP_CDTAB` FOREIGN KEY (`CONSTMP_CD`) REFERENCES `constmp_cdtab` (`CONSTMP_CD`)
) ENGINE=InnoDB AUTO_INCREMENT=42 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='Village Details';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `vil_cdtab`
--

LOCK TABLES `vil_cdtab` WRITE;
/*!40000 ALTER TABLE `vil_cdtab` DISABLE KEYS */;
INSERT INTO `vil_cdtab` VALUES (1,'Village 1',1,NULL,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(2,'Village 2',2,NULL,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(3,'Village 3',3,NULL,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(4,'Village 4',4,NULL,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(5,'Village 1',5,NULL,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(6,'Village 1',6,NULL,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(7,'Village 1',7,NULL,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(8,'Village 1',8,NULL,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(9,'Village 1',9,NULL,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(10,'Village 1',10,NULL,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(19,'Village 1',19,NULL,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(20,'Village 1',20,NULL,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(21,'Village 1',21,NULL,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(22,'Village 1',22,NULL,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(23,'Village 1',23,NULL,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(24,'Village 1',24,NULL,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(25,'Village 1',25,NULL,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(26,'Village 1',26,NULL,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(27,'Village 1',27,NULL,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(28,'Village 1',28,NULL,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(29,'Village 1',29,NULL,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(30,'Village 1',30,NULL,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(32,'Village 1',32,NULL,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(33,'Village 1',33,NULL,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(34,'Village 1',34,NULL,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(35,'Village 1',35,NULL,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(36,'Village 1',36,NULL,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(37,'Village 1',37,NULL,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(38,'Village 1',38,NULL,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(39,'Village 1',39,NULL,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(40,'Village 1',40,NULL,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(41,'Village 1',41,NULL,NULL,NULL,NULL,NULL,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `vil_cdtab` ENABLE KEYS */;
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

-- Dump completed on 2022-06-28 19:18:12
