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
-- Table structure for table `tbl_enq_passet_det`
--

DROP TABLE IF EXISTS `tbl_enq_passet_det`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_enq_passet_det` (
  `enq_promasset_id` int NOT NULL AUTO_INCREMENT,
  `enqtemp_id` int DEFAULT NULL,
  `promoter_code` bigint NOT NULL,
  `assetcat_cd` int NOT NULL,
  `assettype_cd` int NOT NULL,
  `enq_asset_desc` varchar(500) DEFAULT NULL,
  `enq_asset_value` decimal(10,1) DEFAULT NULL,
  `enq_asset_siteno` varchar(50) DEFAULT NULL,
  `enq_asset_addr` varchar(200) DEFAULT NULL,
  `enq_asset_dim` varchar(100) DEFAULT NULL,
  `enq_asset_area` decimal(10,1) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `modified_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  PRIMARY KEY (`enq_promasset_id`),
  KEY `fk_tbl_enq_passet_det_tbl_enq_temptab` (`enqtemp_id`),
  KEY `fk_tbl_enq_passet_det_tbl_prom_cdtab` (`promoter_code`),
  KEY `fk_tbl_enq_passet_det_tbl_assetcat_cdtab` (`assetcat_cd`),
  KEY `fk_tbl_enq_passet_det_tbl_assettype_cdtab` (`assettype_cd`),
  CONSTRAINT `fk_tbl_enq_passet_det_tbl_assetcat_cdtab` FOREIGN KEY (`assetcat_cd`) REFERENCES `tbl_assetcat_cdtab` (`assetcat_cd`),
  CONSTRAINT `fk_tbl_enq_passet_det_tbl_assettype_cdtab` FOREIGN KEY (`assettype_cd`) REFERENCES `tbl_assettype_cdtab` (`assettype_cd`),
  CONSTRAINT `fk_tbl_enq_passet_det_tbl_enq_temptab` FOREIGN KEY (`enqtemp_id`) REFERENCES `tbl_enq_temptab` (`enqtemp_id`),
  CONSTRAINT `fk_tbl_enq_passet_det_tbl_prom_cdtab` FOREIGN KEY (`promoter_code`) REFERENCES `tbl_prom_cdtab` (`promoter_code`)
) ENGINE=InnoDB AUTO_INCREMENT=97 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_enq_passet_det`
--

LOCK TABLES `tbl_enq_passet_det` WRITE;
/*!40000 ALTER TABLE `tbl_enq_passet_det` DISABLE KEYS */;
INSERT INTO `tbl_enq_passet_det` VALUES (1,33,1,1,1,'dsds',2323.0,'434','2323','343',434.0,_binary '',_binary '\0',NULL,NULL,'2022-03-05 11:01:50',NULL,'926c2b5f-a1c8-494a-a78a-3125cc7fe313'),(2,7,4,1,1,'string',123.0,'string','string','string',500.0,_binary '',_binary '\0',NULL,NULL,'2022-01-19 05:27:48',NULL,'c896ae1a-3a42-40c9-99e5-90d65b7ac0d2'),(4,55,1,1,1,'Test',2323.0,'1284','Test','12',12.0,_binary '',_binary '\0',NULL,NULL,'2022-03-21 10:04:59',NULL,'181ff44b-66da-4a11-a7e9-6dd41f62149e'),(5,56,1,1,1,'test',2323.0,'1212','Test','12',23.0,_binary '',_binary '\0',NULL,NULL,'2022-03-21 10:21:24',NULL,'47fbe470-9d7a-4981-a67c-3529dfa4bc3c'),(6,57,1,1,1,'test',34.0,'1284','Test','12',12.0,_binary '\0',_binary '',NULL,'cmeps9748b','2022-03-21 10:35:04','2022-03-21 10:42:59','77528208-2c21-4889-b0aa-ecbf318024a9'),(7,57,1,1,1,'test',34.0,'1284','Test','12',12.0,_binary '',_binary '\0','cmeps9748b',NULL,'2022-03-21 10:42:59',NULL,'681aba00-94ee-4275-94ce-8ba45318d8ec'),(8,58,1,1,1,'test',2323.0,'1284','Test','12',12.0,_binary '',_binary '\0',NULL,NULL,'2022-03-21 10:55:07',NULL,'1b5f2e7a-70e6-4134-9d46-7a4d5916eb1b'),(9,59,1,1,1,'tets',2323.0,'1284','Test','12',12.0,_binary '',_binary '\0',NULL,NULL,'2022-03-21 11:38:36',NULL,'b1eace9d-24d6-46c8-b72a-2c41600692ba'),(13,64,1,1,1,'tet',34.0,'1284','Test','12',12.0,_binary '',_binary '\0',NULL,NULL,'2022-03-21 11:53:39',NULL,'1fa1cda5-5144-4d52-a009-1c04dae3acd5'),(14,65,1,1,1,'test',2323.0,'1284','Test','12',12.0,_binary '',_binary '\0',NULL,NULL,'2022-03-21 11:58:59',NULL,'cc546495-17bd-4f78-832d-27ee7c349355'),(15,66,39,1,1,'test',2323.0,'1284','Test','12',12.0,_binary '\0',_binary '',NULL,'cmeps9748b','2022-03-21 12:27:24','2022-03-21 12:28:21','d0a9c687-a6fb-4491-b55a-97b2c26acf22'),(16,66,39,1,1,'test',2323.0,'1284','Test','12',12.0,_binary '\0',_binary '','cmeps9748b','cmeps9748b','2022-03-21 12:28:21','2022-03-21 12:36:32','ad40be19-deb2-4a56-8c56-7b21dd3857c7'),(17,66,39,1,1,'test',2323.0,'1284','Test','12',12.0,_binary '',_binary '\0','cmeps9748b',NULL,'2022-03-21 12:36:32',NULL,'db235d63-b20b-4f2f-9f0a-b6e0b77e81f7'),(18,67,41,1,1,'test',4545.0,'1284','Test','12',12.0,_binary '\0',_binary '',NULL,'cmeps9748b','2022-03-21 13:44:54','2022-03-21 13:54:01','8b972b77-e895-43bc-9a7c-ff015bc9abab'),(19,67,41,1,1,'test',4545.0,'1284','Test','12',12.0,_binary '',_binary '\0','cmeps9748b',NULL,'2022-03-21 13:54:01',NULL,'a1a9a84a-b1de-454d-b217-d2b31ba1052c'),(20,68,44,1,1,'test',4545.0,'1284','Test','12',12.0,_binary '\0',_binary '',NULL,'cmeps9748b','2022-03-21 14:04:05','2022-03-21 14:04:48','2c935221-9340-45aa-b45b-a8ce3d6011d5'),(21,68,44,1,1,'test',4545.0,'1284','Test','12',12.0,_binary '\0',_binary '','cmeps9748b','cmeps9748b','2022-03-21 14:04:48','2022-03-22 04:38:28','1c273a4d-8703-4cf0-9675-8eb29903eaf5'),(22,68,21,1,1,'test',4545.0,'1284','Test','12',12.0,_binary '\0',_binary '','cmeps9748b','cmeps9748b','2022-03-22 04:38:28','2022-03-22 05:09:39','ba0fdb9f-8eba-4942-86d8-a5d1a9ea45b6'),(23,68,21,1,1,'test',4545.0,'1284','Test','12',12.0,_binary '\0',_binary '','cmeps9748b','cmeps9748b','2022-03-22 05:09:39','2022-03-22 05:12:36','19007f36-8d46-463e-9543-db25489e83e3'),(24,68,21,1,1,'test',4545.0,'1284','Test','12',12.0,_binary '',_binary '\0','cmeps9748b',NULL,'2022-03-22 05:12:36',NULL,'ebcb72de-1e4c-4af7-8c55-5af943958073'),(26,82,47,1,1,'test',1233.0,'434','434','343',434.0,_binary '\0',_binary '',NULL,'CMEPS9748B','2022-03-25 07:37:53','2022-03-29 10:23:42','bb108631-770a-48c9-99b3-0798f98d486f'),(27,82,47,1,1,'test',1233.0,'434','434','343',434.0,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-03-29 10:23:42',NULL,'6e328a2d-e118-48f3-83f6-cc004a2e5d00'),(36,84,57,1,1,'test',43434.0,'test','test','343',434.0,_binary '',_binary '\0',NULL,NULL,'2022-03-30 13:41:06',NULL,'b92acfd8-a09d-4a91-8b80-2c662726d69f'),(37,85,58,1,1,'test',43434.0,'434','434','343',434.0,_binary '',_binary '\0',NULL,NULL,'2022-03-30 13:47:52',NULL,'370a8690-0bae-4e09-92ab-88c3139dc65a'),(38,86,59,1,1,'test',43434.0,'434','434','343',434.0,_binary '',_binary '\0',NULL,NULL,'2022-03-30 14:02:19',NULL,'dc0dbf69-8676-4489-9d16-eddedc426d2e'),(39,91,68,1,1,'test',10000.0,'434','434','343',434.0,_binary '\0',_binary '',NULL,'CMEPS9748B','2022-03-30 21:52:23','2022-03-30 21:56:00','9ae21bc2-1c28-45df-bffc-c94ad99cb92e'),(40,91,69,1,1,'test',2000.0,'434','434','343',434.0,_binary '\0',_binary '',NULL,'CMEPS9748B','2022-03-30 21:52:23','2022-03-30 21:56:00','90f50dc6-df2f-4681-9fae-643efdf80ce1'),(41,91,68,1,1,'test',10000.0,'434','434','343',434.0,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-03-30 21:56:00','2022-03-30 22:19:43','c71f08aa-7903-4425-84d2-5853ac36a9f6'),(42,91,69,1,1,'test',2000.0,'434','434','343',434.0,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-03-30 21:56:00','2022-03-30 22:19:43','0d72c67d-90ff-44b2-b1ae-2f6cfe40ca60'),(45,91,69,1,1,'test',3000.0,'434','434','343',434.0,_binary '\0',_binary '',NULL,'CMEPS9748B','2022-03-30 22:00:30','2022-03-30 22:19:43','f611d678-d5bb-4587-9c32-3a11800bb36f'),(46,91,68,1,1,'test',40000.0,'1000','434','343',434.0,_binary '\0',_binary '',NULL,'CMEPS9748B','2022-03-30 22:00:30','2022-03-30 22:19:43','bf444b84-c8b2-4b54-93a0-e9e248c09538'),(47,91,68,1,1,'test',10000.0,'434','434','343',434.0,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-03-30 22:19:43','2022-03-30 22:23:39','0c278752-d4d8-4793-b189-be3cfd5106ac'),(48,91,69,1,1,'test',2000.0,'434','434','343',434.0,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-03-30 22:19:43','2022-03-30 22:23:39','35431232-2b4a-4071-8869-a5aefb8f422e'),(49,91,69,1,1,'test',3000.0,'434','434','343',434.0,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-03-30 22:19:43','2022-03-30 22:23:39','f34c5ca8-de73-4290-9dc7-0b27f5585e83'),(50,91,68,1,1,'test',40000.0,'1000','434','343',434.0,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-03-30 22:19:43','2022-03-30 22:23:39','773c61f3-6dd7-4a87-b205-5212e8326350'),(61,91,68,1,1,'test',10000.0,'434','434','343',434.0,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-03-30 22:23:39',NULL,'bcde5a99-3b13-4072-8f1a-93e1377d1ba4'),(62,91,69,1,1,'test',2000.0,'434','434','343',434.0,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-03-30 22:23:39',NULL,'66eb7ed3-1b7a-40b7-91fd-3e00d4178255'),(63,91,69,1,1,'test',3000.0,'434','434','343',434.0,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-03-30 22:23:39',NULL,'e3fced0c-4edb-4c93-aa9c-fc1c963209c5'),(64,91,68,1,1,'test',40000.0,'1000','434','343',434.0,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-03-30 22:23:39',NULL,'cd13afc0-e15c-481a-865b-0da92ff9ff28'),(65,91,68,1,1,'test',99999.0,'434','434','343',434.0,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-03-30 22:23:39',NULL,'601f20a3-d00c-4faf-b706-ad826f940b8e'),(66,92,72,1,1,'test',100000.0,'434','434','343',434.0,_binary '\0',_binary '',NULL,'CMEPS9748B','2022-03-30 23:22:43','2022-03-30 23:23:46','cba6b05f-aec2-41e7-a8e2-e7b1d8dfe792'),(67,92,72,1,1,'test',1234.0,'434','434','343',434.0,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-03-30 23:23:46','2022-03-30 23:24:31','364bc587-8511-4c21-868f-b167e974b048'),(68,92,72,1,1,'test',100000.0,'434','434','343',434.0,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-03-30 23:23:46','2022-03-30 23:24:31','b0c7d38d-fa47-4906-baa8-4cc369100c1f'),(69,92,72,1,1,'test',1234.0,'434','434','343',434.0,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-03-30 23:24:31','2022-03-30 23:27:14','28a5d10e-5c62-41ee-a460-7f1763f479f0'),(70,92,72,1,1,'test',100000.0,'434','434','343',434.0,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-03-30 23:24:31','2022-03-30 23:27:14','6939aa70-1f06-4c56-88d4-7100c098b57c'),(71,92,72,1,1,'test',1234.0,'434','434','343',434.0,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-03-30 23:27:14','2022-03-30 23:28:15','b81f4a39-6e00-4c67-a0b9-0c658a4c30d5'),(72,92,72,1,1,'test',100000.0,'434','434','343',434.0,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-03-30 23:27:14','2022-03-30 23:28:15','b4d60241-9de2-4a2d-b2ec-1f7282b10a9c'),(73,92,72,1,1,'test',1234.0,'434','434','343',434.0,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-03-30 23:28:15','2022-04-05 03:26:18','9f53c6b8-d447-4b2a-ad0c-360a76de7a76'),(74,92,72,1,1,'test',1234.0,'434','434','343',434.0,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-04-05 03:26:19','2022-04-05 03:36:52','b4c3c77b-6529-42af-80c5-13adec1e2bd0'),(75,92,72,1,1,'test',1234.0,'434','434','343',434.0,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-04-05 03:36:52','2022-04-05 03:38:16','8a29eb8f-c169-4379-b1c8-5bc1a3d57d8c'),(76,92,72,1,1,'test',1234.0,'434','434','343',434.0,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-04-05 03:38:16','2022-04-05 03:41:00','ac665b66-b3f9-4107-ae40-baef60ef1377'),(77,92,72,1,1,'test',1234.0,'434','434','343',434.0,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-04-05 03:41:00','2022-04-06 12:14:01','98d46f84-716a-47f9-b2ae-21e8a199d6f7'),(78,92,72,1,1,'test',1234.0,'434','434','343',434.0,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-04-06 12:14:01','2022-04-07 07:58:09','13a80405-895f-40d3-b28e-a6104d863e30'),(79,92,72,1,1,'test',1000.0,'434','434','343',434.0,_binary '\0',_binary '',NULL,'CMEPS9748B','2022-04-07 07:29:29','2022-04-07 07:58:09','694ccbb9-fa8d-49ef-9629-c4aaf92c60ed'),(80,92,72,1,1,'test',1000.0,'434','434','343',434.0,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-04-07 07:58:09','2022-04-07 08:02:37','a6c01645-4dfe-4ea7-9a48-caa4df23a446'),(81,92,72,1,1,'test',1000.0,'434','434','343',434.0,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-04-07 07:58:09','2022-04-07 08:02:37','4ab150be-b19e-49cd-a729-fe66f2bded24'),(82,92,72,1,1,'test',1000.0,'434','434','343',434.0,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-04-07 08:02:37','2022-04-07 08:10:26','423936d1-6f61-4b2d-8eed-c75895b42d16'),(83,92,72,1,1,'test',1000.0,'434','434','343',434.0,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-04-07 08:02:37','2022-04-07 08:10:26','360bed7b-d683-4400-9eb7-7503417b3078'),(84,92,72,1,1,'test',1000.0,'434','434','343',434.0,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-04-07 08:10:26','2022-04-07 08:11:47','40db7033-cf8c-4d9d-9c8e-532c07b7e695'),(85,92,72,1,1,'test',1000.0,'434','434','343',434.0,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-04-07 08:10:26','2022-04-07 08:11:47','7759133c-6250-40b9-b20d-08f6e77f38e2'),(86,92,72,1,1,'test',1000.0,'434','434','343',434.0,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-04-07 08:11:47','2022-04-07 08:13:09','49b45ab3-120a-4695-808a-a757f4fc4b2f'),(87,92,72,1,1,'test',2000.0,'434','434','343',434.0,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-04-07 08:11:47','2022-04-07 08:13:09','e8667a7a-93a0-4169-b9c1-7ea5ec8a664e'),(88,92,72,1,1,'test',1000.0,'434','434','343',434.0,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-04-07 08:13:09','2022-04-07 08:14:54','47d71ebc-ab43-46cf-8c77-7fd01b5e5442'),(89,92,72,1,1,'test',2000.0,'434','434','343',434.0,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-04-07 08:13:09','2022-04-07 08:14:54','b49238b4-60a5-4be4-acef-644f9905a8a8'),(90,92,72,1,1,'test',1000.0,'434','434','343',434.0,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-04-07 08:14:54','2022-04-07 08:25:01','0429cf01-980f-4787-bafb-74b28278e8a0'),(91,92,72,1,1,'test',2000.0,'434','434','343',434.0,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-04-07 08:25:01',NULL,'51f2ab24-b285-4b06-9813-b6b840d79a17'),(92,94,75,1,1,'test',1000.0,'3434','3434 NOIDA','545',454.0,_binary '',_binary '\0',NULL,NULL,'2022-04-08 06:33:19',NULL,'71ea2bc9-c2d9-4aef-9420-27575b8aa8c4'),(93,95,77,1,1,'asset description',23450.0,'23','34','2345',1200.0,_binary '',_binary '\0',NULL,NULL,'2022-04-08 10:04:28',NULL,'1ef1aae8-0422-4892-99ed-6c5c05b77203'),(94,95,78,1,1,'descr',5600.0,'45','34','2345',1200.0,_binary '',_binary '\0',NULL,NULL,'2022-04-08 10:04:28',NULL,'3553bcc3-ac44-46e4-b482-6e25fd15975c'),(95,99,87,1,1,'tEST',12345678.0,'1','1','11',1.0,_binary '',_binary '\0',NULL,NULL,'2022-04-21 06:30:29',NULL,'4e5d9c31-cf99-47e1-9113-3eca92f67ccb'),(96,104,89,1,1,'Yeshwantpur',2500000.0,'1st  Floor','TTMC, BMTC Building','100',100.0,_binary '',_binary '\0',NULL,NULL,'2022-04-22 11:00:21',NULL,'9c72e937-331d-4378-b38b-3de5b0870531');
/*!40000 ALTER TABLE `tbl_enq_passet_det` ENABLE KEYS */;
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

-- Dump completed on 2022-06-28 19:17:50
