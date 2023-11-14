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
-- Table structure for table `tbl_enq_pjfin_det`
--

DROP TABLE IF EXISTS `tbl_enq_pjfin_det`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_enq_pjfin_det` (
  `enq_pjfin_id` int NOT NULL AUTO_INCREMENT,
  `enqtemp_id` int DEFAULT NULL,
  `finyear_code` mediumint DEFAULT NULL,
  `fincomp_cd` int NOT NULL,
  `enq_pjfinamt` decimal(10,1) DEFAULT NULL,
  `wh_pjprov` tinyint(1) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `modified_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  PRIMARY KEY (`enq_pjfin_id`),
  KEY `fk_tbl_enq_pjfin_det_tbl_enq_temptab` (`enqtemp_id`),
  KEY `fk_tbl_enq_pjfin_det_tbl_finyear_cdtab` (`finyear_code`),
  KEY `fk_tbl_enq_pjfin_det_tbl_fincomp_cdtab` (`fincomp_cd`),
  CONSTRAINT `fk_tbl_enq_pjfin_det_tbl_enq_temptab` FOREIGN KEY (`enqtemp_id`) REFERENCES `tbl_enq_temptab` (`enqtemp_id`),
  CONSTRAINT `fk_tbl_enq_pjfin_det_tbl_fincomp_cdtab` FOREIGN KEY (`fincomp_cd`) REFERENCES `tbl_fincomp_cdtab` (`fincomp_cd`),
  CONSTRAINT `fk_tbl_enq_pjfin_det_tbl_finyear_cdtab` FOREIGN KEY (`finyear_code`) REFERENCES `tbl_finyear_cdtab` (`finyear_code`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_enq_pjfin_det`
--

LOCK TABLES `tbl_enq_pjfin_det` WRITE;
/*!40000 ALTER TABLE `tbl_enq_pjfin_det` DISABLE KEYS */;
INSERT INTO `tbl_enq_pjfin_det` VALUES (1,7,1,1,100.0,21,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(2,82,1,1,244.0,1,_binary '',_binary '\0',NULL,NULL,'2022-03-28 13:49:29',NULL,'f32fbca1-0d26-461e-a1bb-46bdd5713321'),(3,94,1,1,10000.0,1,_binary '',_binary '\0',NULL,NULL,'2022-04-08 06:35:23',NULL,'2555c2d4-f0bf-4991-bb35-9e945c8c3426'),(4,99,1,1,12.0,0,_binary '',_binary '\0',NULL,NULL,'2022-04-21 04:43:03',NULL,'3f1c27db-cacb-49c8-b07d-75b21144640a'),(5,99,1,1,12.0,0,_binary '',_binary '\0',NULL,NULL,'2022-04-21 04:43:03',NULL,'5154a092-448e-4286-8a3e-f9a4767fa323');
/*!40000 ALTER TABLE `tbl_enq_pjfin_det` ENABLE KEYS */;
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

-- Dump completed on 2022-06-28 19:18:02
