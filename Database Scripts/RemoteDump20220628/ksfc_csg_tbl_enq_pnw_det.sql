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
-- Table structure for table `tbl_enq_pnw_det`
--

DROP TABLE IF EXISTS `tbl_enq_pnw_det`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_enq_pnw_det` (
  `enq_promnw_id` int NOT NULL AUTO_INCREMENT,
  `enqtemp_id` int DEFAULT NULL,
  `promoter_code` bigint NOT NULL,
  `enq_immov` decimal(10,1) DEFAULT NULL,
  `enq_mov` decimal(10,1) DEFAULT NULL,
  `enq_liab` decimal(10,1) DEFAULT NULL,
  `enq_nw` decimal(10,1) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `modified_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  PRIMARY KEY (`enq_promnw_id`),
  KEY `fk_tbl_enq_pnw_det_tbl_enq_temptab` (`enqtemp_id`),
  KEY `fk_tbl_enq_pnw_det_tbl_prom_cdtab` (`promoter_code`),
  CONSTRAINT `fk_tbl_enq_pnw_det_tbl_enq_temptab` FOREIGN KEY (`enqtemp_id`) REFERENCES `tbl_enq_temptab` (`enqtemp_id`),
  CONSTRAINT `fk_tbl_enq_pnw_det_tbl_prom_cdtab` FOREIGN KEY (`promoter_code`) REFERENCES `tbl_prom_cdtab` (`promoter_code`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_enq_pnw_det`
--

LOCK TABLES `tbl_enq_pnw_det` WRITE;
/*!40000 ALTER TABLE `tbl_enq_pnw_det` DISABLE KEYS */;
INSERT INTO `tbl_enq_pnw_det` VALUES (1,7,4,1.0,1.0,2.0,23.0,_binary '',_binary '',NULL,NULL,'2022-01-19 05:58:07',NULL,'a4a5daf1-b809-43a5-b690-f122a4ae0643'),(2,92,72,NULL,1234.0,912.0,322.0,_binary '\0',_binary '',NULL,NULL,'2022-04-05 03:37:18','2022-04-05 03:41:05','86bbd4ff-7c54-44b1-bb27-6da806e4f98b'),(3,92,72,NULL,1234.0,912.0,322.0,_binary '\0',_binary '',NULL,NULL,'2022-04-05 03:41:16',NULL,'6581e0c1-bcf6-410f-9642-604c36b2c717'),(4,92,72,NULL,1000.0,200.0,800.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 07:29:37',NULL,'75b9aa30-5f2b-4447-82e0-6d4dd4546511'),(5,92,72,NULL,2000.0,200.0,1800.0,_binary '\0',_binary '',NULL,'CMEPS9748B','2022-04-07 07:58:18','2022-04-07 08:12:04','2a6285f0-c60e-4fbd-a3b1-2ec1a298ae31'),(6,92,72,NULL,3000.0,200.0,2800.0,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-04-07 08:12:06','2022-04-07 08:13:38','ddddba56-0bf4-48df-8055-0003471df601'),(7,92,72,NULL,3000.0,300.0,2700.0,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-04-07 08:13:38','2022-04-07 08:14:58','338a2968-f767-48ef-a2a6-93b1094c565f'),(8,92,72,NULL,1000.0,100.0,900.0,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-04-07 08:14:58','2022-04-07 08:18:46','ec90b16c-f456-44ca-b3b1-dab1712c8d05'),(9,92,72,NULL,2000.0,100.0,1900.0,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-04-07 08:18:46','2022-04-07 08:25:10','6342ace4-7d2f-4c77-bb75-180273613385'),(10,92,72,NULL,2000.0,200.0,1800.0,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-04-07 08:25:10',NULL,'762737d6-b976-4642-97f9-b285d0c3a8b2'),(11,94,75,NULL,1000.0,100.0,900.0,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-04-08 06:33:20',NULL,'8b6bbbfa-ea85-4361-a9ed-92ae7d434a11'),(12,95,77,NULL,23450.0,4200.0,19250.0,_binary '',_binary '\0',NULL,NULL,'2022-04-08 10:04:29',NULL,'9e60fcdc-4790-42da-8bd7-66c34b0a8bc7'),(13,95,78,NULL,5600.0,2300.0,3300.0,_binary '',_binary '\0',NULL,NULL,'2022-04-08 10:04:29',NULL,'d088c701-d633-4fe1-a1a5-079a2b64ae8e');
/*!40000 ALTER TABLE `tbl_enq_pnw_det` ENABLE KEYS */;
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

-- Dump completed on 2022-06-28 19:20:08
