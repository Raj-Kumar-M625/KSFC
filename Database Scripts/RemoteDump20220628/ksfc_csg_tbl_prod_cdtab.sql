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
-- Table structure for table `tbl_prod_cdtab`
--

DROP TABLE IF EXISTS `tbl_prod_cdtab`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_prod_cdtab` (
  `prod_ind` int NOT NULL,
  `prod_cd` int NOT NULL,
  `prod_dets` varchar(80) NOT NULL,
  `dept` varchar(6) DEFAULT NULL,
  `prod_ncd` int DEFAULT NULL,
  `prod_ndt` varchar(50) DEFAULT NULL,
  `prof_flg` int DEFAULT NULL,
  `id` int NOT NULL AUTO_INCREMENT,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `unique_id` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `created_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `modified_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_tbl_prod_cdtab_tbl_ind_cdtab` (`prod_ind`),
  CONSTRAINT `fk_tbl_prod_cdtab_tbl_ind_cdtab` FOREIGN KEY (`prod_ind`) REFERENCES `tbl_ind_cdtab` (`ind_cd`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_prod_cdtab`
--

LOCK TABLES `tbl_prod_cdtab` WRITE;
/*!40000 ALTER TABLE `tbl_prod_cdtab` DISABLE KEYS */;
INSERT INTO `tbl_prod_cdtab` VALUES (1,1,'Test',NULL,1,'1',1,1,_binary '',_binary '\0',NULL,NULL,NULL),(6,600000,'FOOD MANUFACTURING INDUSTRIES(EXCEPT BEVERAGE',NULL,NULL,NULL,1,2,_binary '',_binary '\0',NULL,'1',NULL),(6,601020,'COCONUT OIL',NULL,NULL,NULL,1,3,_binary '',_binary '\0',NULL,'1',NULL),(4,400000,'STONE QUARRYING CLAY & SAND PITS',NULL,NULL,NULL,1,4,_binary '',_binary '\0',NULL,'1',NULL),(7,701010,'BEER',NULL,NULL,NULL,1,5,_binary '',_binary '\0',NULL,'1',NULL),(7,701040,'WHISKY',NULL,NULL,NULL,1,6,_binary '',_binary '\0',NULL,'1',NULL),(8,801000,'TOBACCO CURING & PROCESSING',NULL,NULL,NULL,1,7,_binary '',_binary '\0',NULL,'1',NULL),(9,900000,'TEXTILES',NULL,NULL,NULL,1,8,_binary '',_binary '\0',NULL,'1',NULL),(9,901050,'SURGICAL COTTON',NULL,NULL,NULL,1,9,_binary '',_binary '\0',NULL,'1',NULL),(9,905010,'WOOLEN SWEATERS',NULL,NULL,NULL,1,10,_binary '',_binary '\0',NULL,'1',NULL),(12,1200000,'FURNITURE & FIXTURES ',NULL,NULL,NULL,1,11,_binary '',_binary '\0',NULL,'1',NULL),(11,1111000,'SERVICES AND OTHERS',NULL,NULL,NULL,1,12,_binary '',_binary '\0',NULL,'1',NULL),(16,1606010,'RUBBER SHOES',NULL,NULL,NULL,1,13,_binary '',_binary '\0',NULL,'1',NULL);
/*!40000 ALTER TABLE `tbl_prod_cdtab` ENABLE KEYS */;
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

-- Dump completed on 2022-06-28 19:18:16
