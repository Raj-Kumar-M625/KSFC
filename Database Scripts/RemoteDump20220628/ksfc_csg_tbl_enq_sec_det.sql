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
-- Table structure for table `tbl_enq_sec_det`
--

DROP TABLE IF EXISTS `tbl_enq_sec_det`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_enq_sec_det` (
  `enq_sec_id` int NOT NULL AUTO_INCREMENT,
  `enqtemp_id` int DEFAULT NULL,
  `sec_code` mediumint DEFAULT NULL,
  `sec_cd` smallint NOT NULL,
  `enq_sec_desc` varchar(300) DEFAULT NULL,
  `enq_sec_value` decimal(10,1) DEFAULT NULL,
  `enq_sec_name` varchar(100) DEFAULT NULL,
  `promrel_cd` int NOT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `modified_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  PRIMARY KEY (`enq_sec_id`),
  KEY `fk_tbl_enq_sec_det_tbl_enq_temptab` (`enqtemp_id`),
  KEY `fk_tbl_enq_sec_det_tbl_pjsec_cdtab` (`sec_code`),
  KEY `fk_tbl_enq_sec_det_tbl_sec_cdtab` (`sec_cd`),
  KEY `fk_tbl_enq_sec_det_tbl_promrel_cdtab` (`promrel_cd`),
  CONSTRAINT `fk_tbl_enq_sec_det_tbl_enq_temptab` FOREIGN KEY (`enqtemp_id`) REFERENCES `tbl_enq_temptab` (`enqtemp_id`),
  CONSTRAINT `fk_tbl_enq_sec_det_tbl_pjsec_cdtab` FOREIGN KEY (`sec_code`) REFERENCES `tbl_pjsec_cdtab` (`sec_code`),
  CONSTRAINT `fk_tbl_enq_sec_det_tbl_promrel_cdtab` FOREIGN KEY (`promrel_cd`) REFERENCES `tbl_promrel_cdtab` (`promrel_cd`),
  CONSTRAINT `fk_tbl_enq_sec_det_tbl_sec_cdtab` FOREIGN KEY (`sec_cd`) REFERENCES `tbl_sec_cdtab` (`sec_cd`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_enq_sec_det`
--

LOCK TABLES `tbl_enq_sec_det` WRITE;
/*!40000 ALTER TABLE `tbl_enq_sec_det` DISABLE KEYS */;
INSERT INTO `tbl_enq_sec_det` VALUES (5,1,1,1,'string',1.0,'string',1,_binary '',_binary '\0',NULL,NULL,'2022-01-20 13:06:30',NULL,'9d6be695-ab52-414b-beca-aeda6f09390d'),(6,7,1,1,'string',1.0,'string',1,_binary '',_binary '\0',NULL,NULL,'2022-01-20 13:06:47',NULL,'6ca080c7-19a9-4856-bf70-50ff7c13ad03'),(7,82,1,1,'test',1200.0,'test',1,_binary '',_binary '\0',NULL,NULL,'2022-03-29 09:57:28',NULL,'f957a111-b65d-4550-95b5-a12ac11f6d6b'),(8,94,1,1,'test',4000.0,'test',1,_binary '',_binary '\0',NULL,NULL,'2022-04-08 06:58:49',NULL,'8ab57918-3c86-418d-9d8c-e775ba399e7f'),(9,99,1,1,'sda',1234.0,'tetts',1,_binary '',_binary '\0',NULL,NULL,'2022-04-21 04:53:32',NULL,'0b3c79cb-79a3-409d-b066-d9775b4762b1');
/*!40000 ALTER TABLE `tbl_enq_sec_det` ENABLE KEYS */;
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

-- Dump completed on 2022-06-28 19:20:22
