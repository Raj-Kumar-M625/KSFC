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
-- Table structure for table `tbl_enq_guar_det`
--

DROP TABLE IF EXISTS `tbl_enq_guar_det`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_enq_guar_det` (
  `enq_guar_id` int NOT NULL AUTO_INCREMENT,
  `enqtemp_id` int DEFAULT NULL,
  `promoter_code` bigint NOT NULL,
  `dom_cd` int NOT NULL,
  `enq_guarcibil` text,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `modified_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  PRIMARY KEY (`enq_guar_id`),
  KEY `fk_tbl_enq_guar_det_tbl_enq_temptab` (`enqtemp_id`),
  KEY `fk_tbl_enq_guar_det_tbl_prom_cdtab` (`promoter_code`),
  KEY `fk_tbl_enq_guar_det_tbl_domi_cdtab` (`dom_cd`),
  CONSTRAINT `fk_tbl_enq_guar_det_tbl_domi_cdtab` FOREIGN KEY (`dom_cd`) REFERENCES `tbl_domi_cdtab` (`dom_cd`),
  CONSTRAINT `fk_tbl_enq_guar_det_tbl_enq_temptab` FOREIGN KEY (`enqtemp_id`) REFERENCES `tbl_enq_temptab` (`enqtemp_id`),
  CONSTRAINT `fk_tbl_enq_guar_det_tbl_prom_cdtab` FOREIGN KEY (`promoter_code`) REFERENCES `tbl_prom_cdtab` (`promoter_code`)
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_enq_guar_det`
--

LOCK TABLES `tbl_enq_guar_det` WRITE;
/*!40000 ALTER TABLE `tbl_enq_guar_det` DISABLE KEYS */;
INSERT INTO `tbl_enq_guar_det` VALUES (1,7,5,1,'56',_binary '',_binary '\0',NULL,NULL,'2022-01-19 08:11:47',NULL,'34373bf5-a689-477e-a4a9-8afb0765c272'),(3,14,7,1,'string',_binary '',_binary '\0',NULL,NULL,'2022-01-27 06:40:20',NULL,'6053afd7-1ac7-45dc-a616-23d82512900a'),(4,64,36,1,NULL,_binary '',_binary '\0',NULL,NULL,'2022-03-21 11:54:30',NULL,'aa403278-599a-47f9-98cd-42a4f8075839'),(5,65,38,1,NULL,_binary '',_binary '\0',NULL,NULL,'2022-03-21 11:59:05',NULL,'e4c2f927-68a9-4ab9-a868-fc9ea79530bc'),(6,66,40,1,NULL,_binary '\0',_binary '',NULL,NULL,'2022-03-21 12:27:24','2022-03-21 12:28:22','195b492f-b08e-4cf4-ae81-8530aa028129'),(7,66,21,1,NULL,_binary '',_binary '\0',NULL,NULL,'2022-03-21 12:36:36',NULL,'b4e21787-82db-4b01-89ad-5ec0730c4a52'),(8,67,43,1,NULL,_binary '\0',_binary '',NULL,NULL,'2022-03-21 13:44:54','2022-03-21 13:54:12','05b6826d-1f4d-4b5d-914d-7eb7080bc5ec'),(9,67,21,1,NULL,_binary '',_binary '\0',NULL,NULL,'2022-03-21 13:54:12',NULL,'812ed5be-2030-4203-9789-931a7e053f8d'),(10,68,45,1,NULL,_binary '\0',_binary '',NULL,NULL,'2022-03-21 14:04:10','2022-03-21 14:04:56','3e7ffe0e-425c-45a4-baa6-4b6e0f302625'),(11,68,46,1,NULL,_binary '\0',_binary '',NULL,NULL,'2022-03-21 14:04:10','2022-03-21 14:04:56','6f94826b-82eb-4932-9f90-ca86ec8a0151'),(12,68,21,1,NULL,_binary '\0',_binary '',NULL,'cmeps9748b','2022-03-21 14:04:56','2022-03-22 05:09:56','cfa1ff45-295b-40ea-8e22-2ed3bec64a8d'),(13,68,21,1,NULL,_binary '\0',_binary '','cmeps9748b','cmeps9748b','2022-03-22 05:09:56','2022-03-22 05:12:39','611dbf43-3cee-4419-98bf-af947d3b2eed'),(14,68,21,1,NULL,_binary '\0',_binary '','cmeps9748b','cmeps9748b','2022-03-22 05:12:39','2022-03-23 03:15:56','b8ae27fa-25b8-42be-b9dd-a7a8a27213ab'),(15,68,21,1,NULL,_binary '',_binary '\0','cmeps9748b',NULL,'2022-03-23 03:15:56',NULL,'79d8a915-ec2f-4ce2-9c6f-4fe8be67e6d5'),(16,82,48,1,NULL,_binary '\0',_binary '','cmeps9748b','CMEPS9748B','2022-03-25 07:37:53','2022-03-29 10:23:43','b24a17a9-d0cc-498f-bb1f-54ba384b310e'),(17,82,21,1,NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-03-29 10:23:43',NULL,'c4a4fab8-26e9-444a-887a-505aa8fb25b7'),(18,86,60,1,NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-03-30 14:02:25',NULL,'eaac1d04-c145-424b-988d-1001eca62c97'),(19,91,70,1,NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-03-30 21:34:30',NULL,'a0bd55b1-24dc-4396-9b19-1755e5b61905'),(20,91,71,1,NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-03-30 21:35:00',NULL,'70102428-df7a-44c4-ac34-d3bbfe065337'),(21,92,73,1,NULL,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-03-30 23:11:29','2022-03-30 23:12:00','dc4bd7bf-1b12-4fb2-927a-c41982d00517'),(22,92,73,1,NULL,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-03-30 23:21:10','2022-03-30 23:22:18','11ad2f47-4ba5-43d3-9f88-127cca76927f'),(23,92,73,1,NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-03-30 23:22:19',NULL,'b490fb07-123e-4d2d-88ac-2cecfa9ca078'),(24,94,76,1,NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-04-08 06:32:34',NULL,'98b38a00-4a00-404f-b5db-e36ae348f90e'),(25,95,79,1,NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-04-08 10:01:01',NULL,'bdc16215-17d3-47d6-9503-2f5f0c012054'),(26,99,88,1,NULL,_binary '',_binary '\0','AJNPB1985K',NULL,'2022-04-21 06:29:56',NULL,'a4628a05-34b0-4441-b0dc-313a45a7a97b'),(27,104,90,1,NULL,_binary '',_binary '\0','BADPS0712F',NULL,'2022-04-22 10:57:53',NULL,'b7156aa7-99f7-461f-823f-078f3b697624');
/*!40000 ALTER TABLE `tbl_enq_guar_det` ENABLE KEYS */;
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

-- Dump completed on 2022-06-28 19:17:45
