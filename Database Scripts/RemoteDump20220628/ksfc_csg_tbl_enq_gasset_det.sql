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
-- Table structure for table `tbl_enq_gasset_det`
--

DROP TABLE IF EXISTS `tbl_enq_gasset_det`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_enq_gasset_det` (
  `enq_guarasset_id` int NOT NULL AUTO_INCREMENT,
  `enqtemp_id` int DEFAULT NULL,
  `promoter_code` bigint NOT NULL,
  `assetcat_cd` int NOT NULL,
  `assettype_cd` int NOT NULL,
  `guar_asset_desc` varchar(500) DEFAULT NULL,
  `guar_asset_value` decimal(10,1) DEFAULT NULL,
  `guar_asset_siteno` varchar(50) DEFAULT NULL,
  `guar_asset_addr` varchar(200) DEFAULT NULL,
  `guar_asset_dim` varchar(100) DEFAULT NULL,
  `guar_asset_area` decimal(10,1) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `modified_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  PRIMARY KEY (`enq_guarasset_id`),
  KEY `fk_tbl_enq_gasset_det_tbl_enq_temptab` (`enqtemp_id`),
  KEY `fk_tbl_enq_gasset_det_tbl_prom_cdtab` (`promoter_code`),
  KEY `fk_tbl_enq_gasset_det_tbl_assetcat_cdtab` (`assetcat_cd`),
  KEY `fk_tbl_enq_gasset_det_tbl_assettype_cdtab` (`assettype_cd`),
  CONSTRAINT `fk_tbl_enq_gasset_det_tbl_assetcat_cdtab` FOREIGN KEY (`assetcat_cd`) REFERENCES `tbl_assetcat_cdtab` (`assetcat_cd`),
  CONSTRAINT `fk_tbl_enq_gasset_det_tbl_assettype_cdtab` FOREIGN KEY (`assettype_cd`) REFERENCES `tbl_assettype_cdtab` (`assettype_cd`),
  CONSTRAINT `fk_tbl_enq_gasset_det_tbl_enq_temptab` FOREIGN KEY (`enqtemp_id`) REFERENCES `tbl_enq_temptab` (`enqtemp_id`),
  CONSTRAINT `fk_tbl_enq_gasset_det_tbl_prom_cdtab` FOREIGN KEY (`promoter_code`) REFERENCES `tbl_prom_cdtab` (`promoter_code`)
) ENGINE=InnoDB AUTO_INCREMENT=77 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_enq_gasset_det`
--

LOCK TABLES `tbl_enq_gasset_det` WRITE;
/*!40000 ALTER TABLE `tbl_enq_gasset_det` DISABLE KEYS */;
INSERT INTO `tbl_enq_gasset_det` VALUES (1,7,5,1,1,'string',23.0,'string','string','string',0.0,_binary '',_binary '\0',NULL,NULL,'2022-01-19 09:54:11',NULL,'27231ae6-6752-41c8-b92d-bff81755c1c3'),(8,66,40,1,1,'test',1233.0,NULL,'1284','vffvfvf',545.0,_binary '\0',_binary '',NULL,NULL,'2022-03-21 12:27:36','2022-03-21 12:28:44','e880ea88-1a8b-4569-9eb2-0d5b0269d7d3'),(9,66,40,1,1,'test',1233.0,NULL,'1284','vffvfvf',545.0,_binary '\0',_binary '',NULL,NULL,'2022-03-21 12:28:44','2022-03-21 12:36:48','5c497a3e-f7c7-4842-904f-af80f702fb05'),(10,66,40,1,1,'test',1233.0,NULL,'1284','vffvfvf',545.0,_binary '',_binary '\0',NULL,NULL,'2022-03-21 12:36:48',NULL,'acce7122-1437-4f11-b66c-beb7fbd878ce'),(11,67,43,1,1,'test',5656.0,NULL,'1284','vffvfvf',545.0,_binary '\0',_binary '',NULL,NULL,'2022-03-21 13:44:55','2022-03-21 13:54:30','a7ca2759-e3c2-443b-a7cf-77b4ef9c5b83'),(12,67,43,1,1,'test',5656.0,NULL,'1284','vffvfvf',545.0,_binary '',_binary '\0',NULL,NULL,'2022-03-21 13:54:30',NULL,'4cbe6796-5997-409a-8975-46e618e8e503'),(13,68,45,1,1,'test',1233.0,NULL,'1284','vffvfvf',545.0,_binary '\0',_binary '',NULL,NULL,'2022-03-21 14:04:16','2022-03-21 14:05:09','fe042ed7-e55c-485b-85ac-5bb8898f472c'),(14,68,45,1,1,'test',1233.0,NULL,'1284','vffvfvf',545.0,_binary '\0',_binary '',NULL,NULL,'2022-03-21 14:04:16','2022-03-21 14:05:09','538884f2-755c-4a6b-9ae7-eaa6f2add0d7'),(15,68,45,1,1,'test',1233.0,NULL,'1284','vffvfvf',545.0,_binary '\0',_binary '',NULL,NULL,'2022-03-21 14:05:09','2022-03-22 04:39:16','57687d35-fc08-4b73-a769-9d117bb3c71a'),(16,68,45,1,1,'test',1233.0,NULL,'1284','vffvfvf',545.0,_binary '\0',_binary '',NULL,NULL,'2022-03-21 14:05:09','2022-03-22 04:39:16','74080520-9cee-40fb-a002-96ace369893d'),(17,68,21,1,1,'test',1233.0,NULL,'1284','vffvfvf',545.0,_binary '\0',_binary '',NULL,NULL,'2022-03-22 04:39:16','2022-03-22 05:10:01','f000fc14-876f-4276-ad0c-5c2d11aecbb3'),(18,68,21,1,1,'test',1233.0,NULL,'1284','vffvfvf',545.0,_binary '\0',_binary '',NULL,NULL,'2022-03-22 04:39:16','2022-03-22 05:10:01','51ddb1a2-2084-46a1-add8-466077ecf79e'),(19,68,21,1,1,'test',1233.0,NULL,'1284','vffvfvf',545.0,_binary '\0',_binary '',NULL,NULL,'2022-03-22 05:10:01','2022-03-22 05:12:39','e303809a-8b86-48c0-a3fc-0ac1263d213e'),(20,68,21,1,1,'test',1233.0,NULL,'1284','vffvfvf',545.0,_binary '\0',_binary '',NULL,NULL,'2022-03-22 05:10:01','2022-03-22 05:12:39','3c9da406-8e59-446a-98af-6037a54e7765'),(21,68,21,1,1,'test',1233.0,NULL,'1284','vffvfvf',545.0,_binary '\0',_binary '',NULL,NULL,'2022-03-22 05:12:39','2022-03-23 03:15:56','bbf7d52e-bf53-4af0-80e4-f4943567a5b6'),(22,68,21,1,1,'test',1233.0,NULL,'1284','vffvfvf',545.0,_binary '\0',_binary '',NULL,NULL,'2022-03-22 05:12:39','2022-03-23 03:15:56','e1d0727c-efcd-439f-b982-5c868b82e1ec'),(23,68,21,1,1,'test',1233.0,NULL,'1284','vffvfvf',545.0,_binary '',_binary '\0',NULL,NULL,'2022-03-23 03:15:56',NULL,'6013139b-1a81-406d-a449-6b1009a85d87'),(24,68,21,1,1,'test',1233.0,NULL,'1284','vffvfvf',545.0,_binary '',_binary '\0',NULL,NULL,'2022-03-23 03:15:56',NULL,'ea995038-d98a-4819-88ff-cd57ea65da9c'),(25,82,21,1,1,'test',1212.0,NULL,'12121','343',3434.0,_binary '\0',_binary '',NULL,NULL,'2022-03-25 07:37:54','2022-03-29 10:23:43','b5d92c4b-3743-4a9e-98b3-1bca510fd3f7'),(26,82,21,1,1,'test',1212.0,NULL,'12121','343',3434.0,_binary '',_binary '\0',NULL,NULL,'2022-03-29 10:23:43',NULL,'5c2605c9-0d64-4c36-b33c-dffa4ddff19e'),(34,86,60,1,1,'test',1212.0,NULL,'12121','343',3434.0,_binary '',_binary '\0',NULL,NULL,'2022-03-30 14:02:29',NULL,'19e48162-3d89-4414-8fdc-bdd17cdad3c6'),(35,91,70,1,1,'test',1000.0,NULL,'12121','343',3434.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 21:56:01','2022-03-30 21:58:20','1db9ecb4-5ab4-46d8-9ffa-d8780df627b5'),(36,91,70,1,1,'test',1000.0,NULL,'12121','343',3434.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 21:58:20','2022-03-30 21:59:05','d55b7d48-16be-4da9-9dc5-e6617a0c378d'),(37,91,70,1,1,'test',1000.0,NULL,'12121','343',3434.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 21:59:05','2022-03-30 22:00:33','ac2458d1-37dd-4e1e-9ef3-2a1453688809'),(38,91,70,1,1,'test',1000.0,NULL,'12121','343',3434.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 22:00:33','2022-03-30 22:19:52','af791bad-40a2-414c-a757-8b803e6cb9c2'),(39,91,70,1,1,'test',2000.0,NULL,'12121','343',3434.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 22:01:43','2022-03-30 22:19:52','6ebf8613-97c0-4537-86ad-a88b3dbfd507'),(40,91,70,1,1,'test',1000.0,NULL,'12121','343',3434.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 22:19:52','2022-03-30 22:21:50','0c41f326-798b-4e2a-83ee-4bda451fee7a'),(41,91,70,1,1,'test',2000.0,NULL,'12121','343',3434.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 22:19:52','2022-03-30 22:21:50','933aa749-7ebc-46fc-9bbf-b27855f4898a'),(42,91,70,1,1,'test',1000.0,NULL,'12121','343',3434.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 22:21:50','2022-03-30 22:23:40','203fc561-9b9b-4dfe-ab4f-b781522c0ae7'),(43,91,70,1,1,'test',2000.0,NULL,'12121','343',3434.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 22:21:50','2022-03-30 22:23:40','1451db93-bbc9-4e29-a465-d44b1ee999b6'),(44,91,70,1,1,'test',1000.0,NULL,'12121','343',3434.0,_binary '',_binary '\0',NULL,NULL,'2022-03-30 22:23:40',NULL,'deca3961-056b-4c0d-b148-33965d368a5f'),(45,91,70,1,1,'test',2000.0,NULL,'12121','343',3434.0,_binary '',_binary '\0',NULL,NULL,'2022-03-30 22:23:40',NULL,'ae1f603c-7018-423b-8383-1a4574086957'),(46,92,73,1,1,'test',2000.0,NULL,'test','343',3434.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 23:22:43','2022-03-30 23:23:46','1d175331-68a8-46b9-a494-b3d7035df773'),(47,92,73,1,1,'test',2000.0,NULL,'test','343',3434.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 23:23:46','2022-03-30 23:24:31','75895052-735c-430f-8788-970f97f8eed3'),(48,92,73,1,1,'test',2000.0,NULL,'test','343',3434.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 23:24:31','2022-03-30 23:27:30','9a0c7016-a65c-4acb-ad06-57471481d2cb'),(49,92,73,1,1,'test',2000.0,NULL,'test','343',3434.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 23:27:30','2022-03-30 23:28:22','7660c651-b26c-4b99-bc99-99cc37fba78d'),(50,92,73,1,1,'test',45.0,NULL,'12121','test',3434.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 23:28:22','2022-04-05 03:37:32','a7bb8b6c-2baa-4bed-bcf2-abd01356b5c1'),(51,92,73,1,1,'test',2000.0,NULL,'test','343',3434.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 23:28:22','2022-04-05 03:37:32','838b624b-a6eb-47da-9016-24f44d2ba2cb'),(52,92,73,1,1,'test',45.0,NULL,'12121','test',3434.0,_binary '\0',_binary '',NULL,NULL,'2022-04-05 03:37:32','2022-04-05 03:39:53','c185628f-a805-458e-bc24-04a1f96058c8'),(53,92,73,1,1,'test',2000.0,NULL,'test','343',3434.0,_binary '\0',_binary '',NULL,NULL,'2022-04-05 03:37:32','2022-04-05 03:39:53','3cd39d15-6d8a-4bed-9ce0-4e41227f8251'),(54,92,73,1,1,'test',45.0,NULL,'12121','test',3434.0,_binary '\0',_binary '',NULL,NULL,'2022-04-05 03:39:53','2022-04-05 03:41:20','be4796e2-b7c5-48b0-bc8b-034487a8a5ae'),(55,92,73,1,1,'test',2000.0,NULL,'test','343',3434.0,_binary '\0',_binary '',NULL,NULL,'2022-04-05 03:39:53','2022-04-05 03:41:20','2fbba538-b9da-451b-8fc1-ea25a95472aa'),(56,92,73,1,1,'test',45.0,NULL,'12121','test',3434.0,_binary '\0',_binary '',NULL,NULL,'2022-04-05 03:41:20','2022-04-07 07:29:37','2ebfaf56-08a7-4915-a7e3-f8dad8f9add1'),(57,92,73,1,1,'test',2000.0,NULL,'test','343',3434.0,_binary '\0',_binary '',NULL,NULL,'2022-04-05 03:41:20','2022-04-07 07:29:37','525ba366-3e64-413b-8fe6-3aadaadb0e8b'),(58,92,73,1,1,'test',2000.0,NULL,'test','343',3434.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 07:29:38','2022-04-07 07:59:03','b384835c-d887-4413-b93f-7c5066f37a3f'),(59,92,73,1,1,'test',1000.0,NULL,'12121','test',3434.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 07:29:38','2022-04-07 07:59:03','4d986189-917a-45d2-9e81-361b600705d9'),(60,92,73,1,1,'test',1000.0,NULL,'12121','test',3434.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 07:59:03','2022-04-07 08:03:22','56a9e6e9-fd00-4f0a-8ec6-afd0a990bac9'),(61,92,73,1,1,'test',2000.0,NULL,'test','343',3434.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 07:59:03','2022-04-07 08:03:22','1e85e384-eb0b-4444-8f96-4fceb18f7694'),(62,92,73,1,1,'test',1000.0,NULL,'12121','test',3434.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 08:03:22','2022-04-07 08:10:38','c0caed15-3059-44c1-92a5-87b7cf1c8cfb'),(63,92,73,1,1,'test',2000.0,NULL,'test','343',3434.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 08:03:22','2022-04-07 08:10:38','305e262f-1989-4d43-a932-d62fd9001dbb'),(64,92,73,1,1,'test',1000.0,NULL,'12121','test',3434.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 08:10:38','2022-04-07 08:12:08','ab152055-5b4c-4ae9-a9ba-43b5eacf51f9'),(65,92,73,1,1,'test',2000.0,NULL,'test','343',3434.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 08:10:38','2022-04-07 08:12:08','768cd4d1-494f-4f83-b0e5-68a1d7ff16de'),(66,92,73,1,1,'test',2000.0,NULL,'test','343',3434.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 08:12:08','2022-04-07 08:13:38','deb9a8f4-c0f3-4e36-994e-4d7fd03687f1'),(67,92,73,1,1,'test',2000.0,NULL,'12121','test',3434.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 08:12:08','2022-04-07 08:13:38','5fde58ac-9b1a-4947-a180-4c945320b939'),(68,92,73,1,1,'test',2000.0,NULL,'test','343',3434.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 08:13:38','2022-04-07 08:18:55','e87c267f-59f6-42d0-904c-47d35b502b9d'),(69,92,73,1,1,'test',2000.0,NULL,'12121','test',3434.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 08:13:38','2022-04-07 08:18:55','c6afe645-cb87-490c-bfe2-4d5e1497acb7'),(70,92,73,1,1,'test',1000.0,NULL,'test','343',3434.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 08:14:58','2022-04-07 08:18:55','c651738a-7595-489d-9e66-0dc18748ef43'),(71,92,73,1,1,'test',1000.0,NULL,'test','343',3434.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 08:18:55','2022-04-07 08:25:15','48c8db4d-a2dc-4fdd-886a-7f0c735a950c'),(72,92,73,1,1,'test',2000.0,NULL,'test','343',3434.0,_binary '',_binary '\0',NULL,NULL,'2022-04-07 08:25:15',NULL,'872b962d-8048-41fc-926c-30f38fbd475d'),(73,94,76,1,1,'test',2000.0,NULL,'2323','344',4343.0,_binary '',_binary '\0',NULL,NULL,'2022-04-08 06:33:20',NULL,'526f50a6-f519-4e8c-a91a-456bcfebc8bc'),(74,95,79,1,1,'desc',5000.0,NULL,'21','2100',2100.0,_binary '',_binary '\0',NULL,NULL,'2022-04-08 10:04:29',NULL,'cdbde2e9-5248-41a2-b496-83693f2ff50a'),(75,99,88,1,1,'1',1.0,NULL,'1','1',11.0,_binary '',_binary '\0',NULL,NULL,'2022-04-21 06:30:29',NULL,'f5ac5642-5d31-405e-82a9-ae72cf276519'),(76,104,90,1,1,'Packing Machine',3000000.0,NULL,'NO. 3','30',100.0,_binary '',_binary '\0',NULL,NULL,'2022-04-22 11:00:22',NULL,'881c7de3-0a32-4b77-b1b6-ed3d26c6a90e');
/*!40000 ALTER TABLE `tbl_enq_gasset_det` ENABLE KEYS */;
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

-- Dump completed on 2022-06-28 19:19:29
