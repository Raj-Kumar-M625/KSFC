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
-- Table structure for table `tbl_enq_prom_det`
--

DROP TABLE IF EXISTS `tbl_enq_prom_det`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_enq_prom_det` (
  `enq_prom_id` int NOT NULL AUTO_INCREMENT,
  `enqtemp_id` int DEFAULT NULL,
  `promoter_code` bigint NOT NULL,
  `enq_prom_share` decimal(18,2) DEFAULT NULL,
  `enq_prom_exp` mediumint DEFAULT NULL,
  `enq_prom_expdet` varchar(500) DEFAULT NULL,
  `pdesig_cd` int NOT NULL,
  `dom_cd` int NOT NULL,
  `enq_cibil` text,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `modified_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  PRIMARY KEY (`enq_prom_id`),
  KEY `fk_tbl_enq_prom_det_tbl_enq_temptab` (`enqtemp_id`),
  KEY `fk_tbl_enq_prom_det_tbl_prom_cdtab` (`promoter_code`),
  KEY `fk_tbl_enq_prom_det_tbl_pdesig_cdtab` (`pdesig_cd`),
  KEY `fk_tbl_enq_prom_det_tbl_domi_cdtab` (`dom_cd`),
  CONSTRAINT `fk_tbl_enq_prom_det_tbl_domi_cdtab` FOREIGN KEY (`dom_cd`) REFERENCES `tbl_domi_cdtab` (`dom_cd`),
  CONSTRAINT `fk_tbl_enq_prom_det_tbl_enq_temptab` FOREIGN KEY (`enqtemp_id`) REFERENCES `tbl_enq_temptab` (`enqtemp_id`),
  CONSTRAINT `fk_tbl_enq_prom_det_tbl_pdesig_cdtab` FOREIGN KEY (`pdesig_cd`) REFERENCES `tbl_pdesig_cdtab` (`pdesig_cd`),
  CONSTRAINT `fk_tbl_enq_prom_det_tbl_prom_cdtab` FOREIGN KEY (`promoter_code`) REFERENCES `tbl_prom_cdtab` (`promoter_code`)
) ENGINE=InnoDB AUTO_INCREMENT=74 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_enq_prom_det`
--

LOCK TABLES `tbl_enq_prom_det` WRITE;
/*!40000 ALTER TABLE `tbl_enq_prom_det` DISABLE KEYS */;
INSERT INTO `tbl_enq_prom_det` VALUES (2,7,4,1.50,10,'string',1,1,'123',_binary '',_binary '\0',NULL,NULL,'2022-01-18 13:55:11',NULL,'2a98a516-7897-4b7a-8db5-f1f7eab77351'),(3,27,12,5.00,12,'fdfdfdfdf',1,1,NULL,_binary '',_binary '\0',NULL,NULL,'2022-03-05 07:55:00',NULL,'14ab77a9-141a-4ec6-b35e-d7d54f097ef1'),(4,28,13,2.00,12,'fdfdfdfdf',1,1,NULL,_binary '',_binary '\0',NULL,NULL,'2022-03-05 08:10:23',NULL,'6768ba90-aea5-484e-a6a0-23e2ebb69e7d'),(5,29,14,2.00,1212121,'fdfdfdfdf',1,1,NULL,_binary '\0',_binary '',NULL,NULL,'2022-03-05 08:23:30','2022-03-05 08:24:51','db533b2e-b426-48d6-8fe2-47ed01d6399d'),(7,31,17,2.00,12,'sa',1,1,NULL,_binary '\0',_binary '',NULL,NULL,'2022-03-05 10:24:16','2022-03-05 10:27:52','a3101008-b27b-406c-be6a-211007ca26b6'),(8,32,18,2.00,12,'dsds',1,1,NULL,_binary '\0',_binary '',NULL,NULL,'2022-03-05 10:32:42','2022-03-05 10:52:34','b7125d43-6ee8-4ed9-959a-46f7c32453ca'),(9,33,19,2.00,12,'fdfdfdfdf',1,1,NULL,_binary '',_binary '\0',NULL,NULL,'2022-03-05 11:01:05',NULL,'b26482ca-2aa4-48b8-a774-660be414737f'),(10,34,20,3.00,3,'fdfdfdfdf',1,1,NULL,_binary '',_binary '\0',NULL,NULL,'2022-03-05 11:06:22',NULL,'651fb3c4-0096-4fff-a052-01ae27002b4c'),(11,54,22,1.00,12,'12',1,1,NULL,_binary '',_binary '\0',NULL,NULL,'2022-03-21 07:52:28',NULL,'2d93432b-de60-45d3-a973-ce67ee7df508'),(12,55,23,1.00,12,'12',1,1,NULL,_binary '',_binary '\0',NULL,NULL,'2022-03-21 10:03:34',NULL,'1b31c806-e394-4208-a315-2cc867a70279'),(13,56,24,2.00,23,'sddsd',1,1,NULL,_binary '',_binary '\0',NULL,NULL,'2022-03-21 10:21:24',NULL,'5768c97a-3b3e-4210-bc34-e60ee16475ed'),(14,57,25,1.00,12,'sddsd',1,1,NULL,_binary '\0',_binary '',NULL,'cmeps9748b','2022-03-21 10:35:04','2022-03-21 10:42:58','f58552c1-5dd5-49bc-849e-25f94db4a3a6'),(15,58,26,1.00,12,'12',1,1,NULL,_binary '',_binary '\0',NULL,NULL,'2022-03-21 10:54:52',NULL,'1da69b26-d9d6-4b11-8a76-4d0c31076dc1'),(16,59,27,1.00,12,'sdsdsd',1,1,NULL,_binary '',_binary '\0',NULL,NULL,'2022-03-21 11:38:36',NULL,'7bd8a696-b4c4-4f7c-87de-2bd7ac13de83'),(17,64,35,10.00,12,'sddsd',1,1,NULL,_binary '',_binary '\0',NULL,NULL,'2022-03-21 11:53:25',NULL,'9ff07a33-7758-416e-8912-c05fee4c60e8'),(18,65,37,10.00,12,'sddsd',1,1,NULL,_binary '',_binary '\0',NULL,NULL,'2022-03-21 11:58:59',NULL,'92f092d3-ef40-42ad-93cc-8be48259f6cb'),(19,66,39,10.00,12,'sddsd',1,1,NULL,_binary '\0',_binary '',NULL,'cmeps9748b','2022-03-21 12:27:24','2022-03-21 12:28:21','667766d0-4439-46cd-9543-5c0d09db9b5c'),(20,66,21,10.00,12,'sddsd',1,1,NULL,_binary '\0',_binary '','cmeps9748b','cmeps9748b','2022-03-21 12:36:03','2022-03-21 12:37:54','53f827e6-28e0-45aa-8474-5dee2506d92b'),(21,66,21,10.00,12,'sddsd',1,1,NULL,_binary '\0',_binary '','cmeps9748b','cmeps9748b','2022-03-21 12:37:54','2022-03-21 12:38:21','2c049cd7-f045-4f0c-a98c-2e75f4b3caa4'),(22,66,21,10.00,12,'sddsd',1,1,NULL,_binary '\0',_binary '','cmeps9748b','cmeps9748b','2022-03-21 12:38:21','2022-03-21 12:38:56','fb168c4a-8ef6-439a-b61c-a3c3b8222b97'),(23,66,21,10.00,12,'sddsd',1,1,NULL,_binary '\0',_binary '','cmeps9748b',NULL,'2022-03-21 12:39:27','2022-03-21 12:43:48','51380712-90e3-4cff-a5c5-4d1cc0cdcab2'),(24,66,21,10.00,12,'sddsd',1,1,NULL,_binary '\0',_binary '',NULL,NULL,'2022-03-21 12:44:03','2022-03-21 12:51:52','00481322-4f73-46bf-989a-c77222b73a56'),(26,66,21,10.00,12,'sddsd',1,1,NULL,_binary '\0',_binary '',NULL,NULL,'2022-03-21 12:53:55','2022-03-21 12:54:05','1596b190-e17d-4862-97b6-53eab0bb3809'),(27,66,21,10.00,12,'sddsd',1,1,NULL,_binary '',_binary '\0',NULL,NULL,'2022-03-21 12:54:21',NULL,'69ed1cc6-9041-4121-90c5-65f052a585bb'),(28,67,41,12.00,12,'sddsd',1,1,NULL,_binary '\0',_binary '',NULL,'cmeps9748b','2022-03-21 13:44:46','2022-03-21 13:45:24','e402f97a-f5f3-46dc-9f34-03f9f1e75d02'),(29,67,42,10.00,12,'sddsd',1,1,NULL,_binary '\0',_binary '',NULL,'cmeps9748b','2022-03-21 13:44:46','2022-03-21 13:45:24','02cd1af7-40cd-48da-9817-2cf711d96419'),(30,67,21,12.00,12,'sddsd',1,1,NULL,_binary '\0',_binary '','cmeps9748b','cmeps9748b','2022-03-21 13:45:52','2022-03-21 13:52:55','95faaaf4-a5ff-4914-b940-a3560ec1164b'),(31,67,21,12.00,12,'sddsd',1,1,NULL,_binary '',_binary '\0','cmeps9748b',NULL,'2022-03-21 13:53:08',NULL,'35cc6fb0-3a6c-4cfa-8482-c7dca4c7ad05'),(32,67,21,10.00,12,'sddsd',1,1,NULL,_binary '',_binary '\0','cmeps9748b',NULL,'2022-03-21 13:53:19',NULL,'f04cd779-ff3b-40f7-a511-a02baf4d95c8'),(33,68,44,10.00,12,'sddsd',1,1,NULL,_binary '\0',_binary '',NULL,'cmeps9748b','2022-03-21 14:04:03','2022-03-21 14:04:43','a8715954-9d89-47dc-9514-def9cd7a58e9'),(34,68,21,10.00,12,'sddsd',1,1,NULL,_binary '\0',_binary '','cmeps9748b','cmeps9748b','2022-03-21 14:04:46','2022-03-22 04:38:25','10cfeb4c-9f7d-434c-bf15-c58ad1a5b86d'),(35,68,21,10.00,12,'sddsd',1,1,NULL,_binary '\0',_binary '','cmeps9748b','cmeps9748b','2022-03-22 04:38:26','2022-03-22 05:09:38','380bb455-82ba-4033-9dc2-7447d7549f3a'),(36,68,21,10.00,12,'sddsd',1,1,NULL,_binary '\0',_binary '','cmeps9748b','cmeps9748b','2022-03-22 05:09:38','2022-03-22 05:12:36','dffd1544-daf7-4e40-8ab2-389bc8befc86'),(37,68,21,10.00,12,'sddsd',1,1,NULL,_binary '\0',_binary '','cmeps9748b','cmeps9748b','2022-03-22 05:12:36','2022-03-23 03:15:48','7cb45613-fcee-4461-9e99-dcaf78560dd4'),(38,68,29,10.00,12,'sddsd',1,1,NULL,_binary '',_binary '\0','cmeps9748b',NULL,'2022-03-23 03:15:48',NULL,'6be97330-ccdb-45ca-9702-970173b82db6'),(39,82,47,54.00,12,'dsdsd',1,1,NULL,_binary '\0',_binary '','cmeps9748b','CMEPS9748B','2022-03-25 07:37:53','2022-03-30 13:36:58','a8de6ed8-43cc-424e-9363-2e9800560c45'),(40,82,47,54.00,12,'dsdsd',1,1,NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-03-30 13:36:58',NULL,'8498a48d-5ec6-4cf7-837b-f76e236e2756'),(41,84,57,54.00,12,'21',1,1,NULL,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-03-30 13:38:39','2022-03-30 13:40:41','72341be6-d663-432a-9eed-58cd642161d0'),(42,84,47,54.00,12,'21',1,1,NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-03-30 13:40:41',NULL,'0d1d5ce4-0aa9-496e-8c14-db1f16ab3aa7'),(43,85,58,54.00,10,'10',1,1,NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-03-30 13:46:11',NULL,'fbcff973-d9d2-4c6d-9dde-728a52c3ec45'),(44,86,59,54.00,12,'dsdsd',1,1,NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-03-30 14:02:01',NULL,'59b00db6-6847-48c8-82d2-a19767f76b22'),(45,87,61,54.00,12,'dsdsd',1,1,NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-03-30 19:20:19',NULL,'a4921c39-6bf9-4b58-8010-9a3f95cc760f'),(46,87,62,54.00,12,'dsdsd',1,1,NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-03-30 19:24:59',NULL,'aebdf609-f6e5-4282-bdf3-1f8fd784fefa'),(47,88,63,10.00,10,'Details',1,1,NULL,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-03-30 20:03:46','2022-03-30 20:05:26','8cdeeae5-bddf-4d22-a0d0-28949dbd3e80'),(48,89,64,54.00,212,'fdfdfdfdf',1,1,NULL,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-03-30 20:41:07','2022-03-30 20:43:03','acbafd1c-1d3e-493d-acae-212834cadd46'),(49,89,65,54.00,12,'fdfdfdfdf',1,1,NULL,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-03-30 20:42:08','2022-03-30 20:43:03','696adc3f-c802-40f8-8d2d-a09ccd47fc0b'),(50,89,65,54.00,12,'fdfdfdfdf',1,1,NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-03-30 20:43:03',NULL,'3325d2c2-4b0c-427a-8773-2ede3b9a0d1f'),(51,89,64,55.00,212,'fdfdfdfdf',1,1,NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-03-30 20:43:03',NULL,'8d92e0aa-5f0f-47de-a500-3663894dfe02'),(52,90,66,10.00,10,'wwww',1,1,NULL,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-03-30 20:58:03','2022-03-30 20:59:09','cf2acca1-65c1-40e9-b81b-8a9f477cd49b'),(53,90,67,20.00,45,'gggg',1,1,NULL,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-03-30 20:58:42','2022-03-30 20:59:09','56142329-0ab3-418a-b4c5-17653069433b'),(54,90,66,10.00,10,'wwww',1,1,NULL,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-03-30 20:59:09','2022-03-30 20:59:48','1b1f9b29-cabe-4db0-a29f-67c75d80676c'),(55,90,66,10.00,10,'wwww',1,1,NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-03-30 20:59:48',NULL,'f94e88b8-0fb9-4742-9831-729609e6b4fc'),(56,91,68,12.00,12,'fdfdfdfdf',1,1,NULL,_binary '\0',_binary '','CMEPS9748B',NULL,'2022-03-30 22:46:25',NULL,'4c8b09db-1061-4c80-9861-73c1e497a76c'),(57,91,69,12.00,23,'eee',1,1,NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-03-30 21:33:31',NULL,'5399d42d-6a6a-4b68-8574-efce58d8c50b'),(58,92,72,54.00,23,'fdfdfdfdf',1,1,NULL,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-03-30 23:09:17','2022-03-30 23:09:37','248e6bf7-e0b2-44c1-baf8-df1e4a067912'),(59,92,72,54.00,23,'fdfdfdfdf',1,1,NULL,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-03-30 23:09:37','2022-03-30 23:12:23','2629f0c4-e8c6-4235-afb3-d0b4e63a3006'),(60,92,72,54.00,23,'fdfdfdfdf',1,1,NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-03-30 23:12:23',NULL,'b026c0b6-47bd-4329-af26-c0c20583a3a7'),(61,92,74,10.00,12,'12',1,1,NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-04-03 19:32:44',NULL,'114e8734-e986-4f41-a1d4-f9a28d02c4c8'),(62,94,75,10.00,10,'TEST',1,1,NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-04-08 06:31:05',NULL,'b0726645-4328-4b3c-9761-f6ce92d44731'),(63,95,77,28.00,45,'exp details',1,1,NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-04-08 09:52:11',NULL,'a5fe0d42-2cf4-4d82-abe2-9b521b63e40c'),(64,95,78,34.00,34,'exp details',1,1,NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-04-08 09:54:13',NULL,'1064984c-ef10-486b-8e32-502a94825799'),(72,99,87,100.00,1,'1',1,1,NULL,_binary '',_binary '\0','AJNPB1985K',NULL,'2022-04-21 06:28:56',NULL,'60aa5bd0-d878-46cc-a8a4-2eabc10ec980'),(73,104,89,100.00,20,'CULINARY ARTS EXPERIENCE',3,1,NULL,_binary '',_binary '\0','BADPS0712F',NULL,'2022-04-22 10:47:05',NULL,'bead2a71-a5a3-4f29-8b5e-e832863f4e04');
/*!40000 ALTER TABLE `tbl_enq_prom_det` ENABLE KEYS */;
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

-- Dump completed on 2022-06-28 19:18:20
