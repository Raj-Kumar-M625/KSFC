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
-- Table structure for table `tbl_enq_pjmf_det`
--

DROP TABLE IF EXISTS `tbl_enq_pjmf_det`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_enq_pjmf_det` (
  `enq_pjmf_id` int NOT NULL AUTO_INCREMENT,
  `enqtemp_id` int DEFAULT NULL,
  `pjmf_cd` int NOT NULL,
  `mfcat_cd` int NOT NULL,
  `enq_pjmf_value` decimal(10,1) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `modified_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  PRIMARY KEY (`enq_pjmf_id`),
  KEY `fk_tbl_enq_pjmf_det_tbl_enq_temptab` (`enqtemp_id`),
  KEY `fk_tbl_enq_pjmf_det_tbl_pjmfcat_cdtab` (`mfcat_cd`),
  KEY `fk_tbl_enq_pjmf_det_tbl_pjmf_cdtab` (`pjmf_cd`),
  CONSTRAINT `fk_tbl_enq_pjmf_det_tbl_enq_temptab` FOREIGN KEY (`enqtemp_id`) REFERENCES `tbl_enq_temptab` (`enqtemp_id`),
  CONSTRAINT `fk_tbl_enq_pjmf_det_tbl_pjmf_cdtab` FOREIGN KEY (`pjmf_cd`) REFERENCES `tbl_pjmf_cdtab` (`pjmf_cd`),
  CONSTRAINT `fk_tbl_enq_pjmf_det_tbl_pjmfcat_cdtab` FOREIGN KEY (`mfcat_cd`) REFERENCES `tbl_pjmfcat_cdtab` (`mfcat_cd`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_enq_pjmf_det`
--

LOCK TABLES `tbl_enq_pjmf_det` WRITE;
/*!40000 ALTER TABLE `tbl_enq_pjmf_det` DISABLE KEYS */;
INSERT INTO `tbl_enq_pjmf_det` VALUES (1,82,1,1,200.0,_binary '',_binary '\0',NULL,NULL,'2022-03-28 13:49:28',NULL,'a20473f0-c0cb-47dc-a56f-83c53dae54f6'),(2,94,1,1,1000.0,_binary '',_binary '\0',NULL,NULL,'2022-04-08 06:35:23',NULL,'f8cf81e6-176a-4080-b885-f723ca79eb50'),(3,99,1,1,123.0,_binary '',_binary '\0',NULL,NULL,'2022-04-21 04:43:01',NULL,'771aaa73-5a57-4ef0-ad71-f99b9766ac13');
/*!40000 ALTER TABLE `tbl_enq_pjmf_det` ENABLE KEYS */;
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

-- Dump completed on 2022-06-28 19:18:30
