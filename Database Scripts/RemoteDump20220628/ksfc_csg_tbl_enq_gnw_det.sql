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
-- Table structure for table `tbl_enq_gnw_det`
--

DROP TABLE IF EXISTS `tbl_enq_gnw_det`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_enq_gnw_det` (
  `enq_guarnw_id` int NOT NULL AUTO_INCREMENT,
  `enqtemp_id` int DEFAULT NULL,
  `promoter_code` bigint NOT NULL,
  `guar_immov` decimal(10,1) DEFAULT NULL,
  `guar_mov` decimal(10,1) DEFAULT NULL,
  `guar_liab` decimal(10,1) DEFAULT NULL,
  `guar_nw` decimal(10,1) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `modified_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  PRIMARY KEY (`enq_guarnw_id`),
  KEY `fk_tbl_enq_gnw_det_tbl_enq_temptab` (`enqtemp_id`),
  KEY `fk_tbl_enq_gnw_det_tbl_prom_cdtab` (`promoter_code`),
  CONSTRAINT `fk_tbl_enq_gnw_det_tbl_enq_temptab` FOREIGN KEY (`enqtemp_id`) REFERENCES `tbl_enq_temptab` (`enqtemp_id`),
  CONSTRAINT `fk_tbl_enq_gnw_det_tbl_prom_cdtab` FOREIGN KEY (`promoter_code`) REFERENCES `tbl_prom_cdtab` (`promoter_code`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_enq_gnw_det`
--

LOCK TABLES `tbl_enq_gnw_det` WRITE;
/*!40000 ALTER TABLE `tbl_enq_gnw_det` DISABLE KEYS */;
INSERT INTO `tbl_enq_gnw_det` VALUES (1,7,5,7.0,0.0,0.0,54.0,_binary '',_binary '\0',NULL,'string','2022-01-19 10:42:54','2022-01-19 10:40:03','c310c5f8-55bf-42a8-a4db-f7b0a2d39dbc'),(2,92,73,NULL,2045.0,234.0,1811.0,_binary '\0',_binary '',NULL,NULL,'2022-04-05 03:37:44','2022-04-05 03:40:15','6c8860eb-8cb1-400f-87db-6837bf49284b'),(3,92,73,NULL,2045.0,234.0,1811.0,_binary '\0',_binary '',NULL,NULL,'2022-04-05 03:40:42','2022-04-05 03:41:26','303c5114-cea5-4b39-abcf-084fe2e3cb31'),(4,92,73,NULL,2045.0,234.0,1811.0,_binary '\0',_binary '',NULL,NULL,'2022-04-05 03:41:35',NULL,'a2c3602a-ff2b-4798-b3b5-e8b67be19ec1'),(5,92,73,NULL,3000.0,300.0,2700.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 07:29:41',NULL,'33ce981a-0407-4107-ba66-70fe4bf35c8e'),(6,92,73,NULL,3000.0,300.0,2700.0,_binary '\0',_binary '',NULL,'CMEPS9748B','2022-04-07 07:59:09','2022-04-07 08:12:08','f716c21f-2ddd-4ea7-b2c3-3dd8893c1ec7'),(7,92,73,NULL,4000.0,300.0,3700.0,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-04-07 08:12:08','2022-04-07 08:14:58','099753a2-371b-43e2-b01d-2d2901b0334c'),(8,92,73,NULL,1000.0,200.0,800.0,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-04-07 08:14:58','2022-04-07 08:18:55','9fccc1a4-76f6-45a0-a066-57e65b7bb00e'),(9,92,73,NULL,1000.0,200.0,800.0,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-04-07 08:18:55','2022-04-07 08:25:15','0c9bdf9e-9e2f-42e4-ab6d-16fcba8f6a65'),(10,92,73,NULL,2000.0,300.0,1700.0,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-04-07 08:25:15',NULL,'7611dbc4-a457-4e85-b2b3-f43190b3b3ff'),(11,94,76,NULL,2000.0,200.0,1800.0,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-04-08 06:33:20',NULL,'fa678444-f415-47aa-aba2-70e163aede0e'),(12,95,79,NULL,5000.0,3400.0,1600.0,_binary '',_binary '\0',NULL,NULL,'2022-04-08 10:04:29',NULL,'400ecc61-2261-41d9-9e06-9f40dd151800'),(13,99,88,NULL,1.0,123454.0,-123453.0,_binary '',_binary '\0','AJNPB1985K',NULL,'2022-04-21 06:30:30',NULL,'7b9c296f-4219-4e25-bf4a-d1336fa580c6'),(14,104,90,NULL,3000000.0,1000000.0,2000000.0,_binary '',_binary '\0','BADPS0712F',NULL,'2022-04-22 11:00:22',NULL,'facc4f8f-d1c8-40c2-8259-f3ede27bed8d');
/*!40000 ALTER TABLE `tbl_enq_gnw_det` ENABLE KEYS */;
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

-- Dump completed on 2022-06-28 19:19:06
