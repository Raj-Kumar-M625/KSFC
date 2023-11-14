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
-- Table structure for table `kzn_cdtab`
--

DROP TABLE IF EXISTS `kzn_cdtab`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `kzn_cdtab` (
  `KZN_CD` tinyint NOT NULL COMMENT 'Circle Code',
  `KZN_NAM` varchar(20) DEFAULT NULL COMMENT 'Circle Name',
  `KZN_ADR1` varchar(30) DEFAULT NULL COMMENT 'Address',
  `KZN_ADR2` varchar(30) DEFAULT NULL COMMENT 'Address',
  `KZN_ADR3` varchar(30) DEFAULT NULL COMMENT 'Address',
  `KZN_PIN` int DEFAULT NULL COMMENT 'Pincode',
  `KZN_TEL` bigint DEFAULT NULL COMMENT 'Telephone',
  `KZN_TLX` varchar(20) DEFAULT NULL COMMENT 'Telex',
  `KZN_FAX` varchar(15) DEFAULT NULL COMMENT 'fax',
  `KZN_FLAG` tinyint(1) DEFAULT NULL COMMENT 'isActive/Inactive',
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `modified_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  PRIMARY KEY (`KZN_CD`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='Circle details';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `kzn_cdtab`
--

LOCK TABLES `kzn_cdtab` WRITE;
/*!40000 ALTER TABLE `kzn_cdtab` DISABLE KEYS */;
INSERT INTO `kzn_cdtab` VALUES (1,'Bangalore Circle',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,_binary '',_binary '\0','1',NULL,'2022-05-25 10:47:50',NULL,NULL),(2,'Gulbarga Circle',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,_binary '',_binary '\0','1',NULL,'2022-05-25 10:47:50',NULL,NULL),(3,'Dharwad Circle',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,_binary '',_binary '\0','1',NULL,'2022-05-25 10:47:50',NULL,NULL),(4,'Belgavi Circle',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,_binary '',_binary '\0','1',NULL,'2022-05-25 10:47:50',NULL,NULL);
/*!40000 ALTER TABLE `kzn_cdtab` ENABLE KEYS */;
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

-- Dump completed on 2022-06-28 19:19:33
