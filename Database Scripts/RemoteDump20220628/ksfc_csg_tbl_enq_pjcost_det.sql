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
-- Table structure for table `tbl_enq_pjcost_det`
--

DROP TABLE IF EXISTS `tbl_enq_pjcost_det`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_enq_pjcost_det` (
  `enq_pjcost_id` int NOT NULL AUTO_INCREMENT,
  `enqtemp_id` int NOT NULL,
  `pjcost_cd` int NOT NULL,
  `enq_pjcost_amt` decimal(10,1) DEFAULT NULL,
  `enq_pjcost_rem` varchar(100) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `modified_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  PRIMARY KEY (`enq_pjcost_id`),
  KEY `fk_tbl_enq_pjcost_det_tbl_enq_temptab` (`enqtemp_id`),
  KEY `fk_tbl_enq_pjcost_det_tbl_pjcost_cdtab` (`pjcost_cd`),
  CONSTRAINT `fk_tbl_enq_pjcost_det_tbl_enq_temptab` FOREIGN KEY (`enqtemp_id`) REFERENCES `tbl_enq_temptab` (`enqtemp_id`),
  CONSTRAINT `fk_tbl_enq_pjcost_det_tbl_pjcost_cdtab` FOREIGN KEY (`pjcost_cd`) REFERENCES `tbl_pjcost_cdtab` (`pjcost_cd`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_enq_pjcost_det`
--

LOCK TABLES `tbl_enq_pjcost_det` WRITE;
/*!40000 ALTER TABLE `tbl_enq_pjcost_det` DISABLE KEYS */;
INSERT INTO `tbl_enq_pjcost_det` VALUES (1,82,1,122.0,NULL,_binary '',_binary '\0',NULL,NULL,'2022-03-28 13:49:28',NULL,'c66ceba8-87e9-4795-8863-f7955825f156'),(2,94,1,10000.0,NULL,_binary '',_binary '\0',NULL,NULL,'2022-04-08 06:35:22',NULL,'6fb2c727-932f-49a2-997e-b21544ed0ced'),(3,99,1,1.0,NULL,_binary '',_binary '\0',NULL,NULL,'2022-04-21 04:42:59',NULL,'8e9e9f73-9b13-4deb-a5f6-1cb10b2e8e2e');
/*!40000 ALTER TABLE `tbl_enq_pjcost_det` ENABLE KEYS */;
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

-- Dump completed on 2022-06-28 19:17:48
