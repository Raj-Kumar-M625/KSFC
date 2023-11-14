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
-- Table structure for table `tbl_enq_pliab_det`
--

DROP TABLE IF EXISTS `tbl_enq_pliab_det`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_enq_pliab_det` (
  `enq_promliab_id` int NOT NULL AUTO_INCREMENT,
  `enqtemp_id` int DEFAULT NULL,
  `promoter_code` bigint NOT NULL,
  `enq_liab_desc` varchar(500) DEFAULT NULL,
  `enq_liab_value` decimal(10,1) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `modified_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  PRIMARY KEY (`enq_promliab_id`),
  KEY `fk_tbl_enq_pliab_det_tbl_enq_temptab` (`enqtemp_id`),
  KEY `fk_tbl_enq_pliab_det_tbl_prom_cdtab` (`promoter_code`),
  CONSTRAINT `fk_tbl_enq_pliab_det_tbl_enq_temptab` FOREIGN KEY (`enqtemp_id`) REFERENCES `tbl_enq_temptab` (`enqtemp_id`),
  CONSTRAINT `fk_tbl_enq_pliab_det_tbl_prom_cdtab` FOREIGN KEY (`promoter_code`) REFERENCES `tbl_prom_cdtab` (`promoter_code`)
) ENGINE=InnoDB AUTO_INCREMENT=83 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_enq_pliab_det`
--

LOCK TABLES `tbl_enq_pliab_det` WRITE;
/*!40000 ALTER TABLE `tbl_enq_pliab_det` DISABLE KEYS */;
INSERT INTO `tbl_enq_pliab_det` VALUES (1,7,4,'test',150000.0,_binary '',_binary '\0',NULL,NULL,'2022-01-19 05:36:57',NULL,'0ad9554a-6dee-4560-9eee-5fface7e655a'),(2,56,1,'test',1234.0,_binary '',_binary '\0',NULL,NULL,'2022-03-21 10:21:24',NULL,'9a585c6c-c3b9-4a31-b051-c993a4e15400'),(3,57,1,'test',1234.0,_binary '\0',_binary '',NULL,NULL,'2022-03-21 10:35:04','2022-03-21 10:43:00','9db67a97-c745-44a9-92a3-7b14d87dce5f'),(4,57,1,'test',1234.0,_binary '',_binary '\0',NULL,NULL,'2022-03-21 10:43:00',NULL,'abf90567-8c20-4526-a65a-fcce215593eb'),(5,58,1,'test',1234.0,_binary '',_binary '\0',NULL,NULL,'2022-03-21 10:55:19',NULL,'dec44438-960f-4598-92df-f893a2583ded'),(6,59,1,'test',1234.0,_binary '',_binary '\0',NULL,NULL,'2022-03-21 11:38:36',NULL,'5c566935-37e3-4813-bbf9-16836176794a'),(9,65,1,'test',1234.0,_binary '',_binary '\0',NULL,NULL,'2022-03-21 11:58:59',NULL,'beaee853-9899-4f7c-a914-8636d89f9dbc'),(10,66,39,'test',1234.0,_binary '\0',_binary '',NULL,NULL,'2022-03-21 12:27:24','2022-03-21 12:28:21','0a3909ef-55c3-4eda-b38d-beef40d8b191'),(11,66,39,'test',1234.0,_binary '\0',_binary '',NULL,NULL,'2022-03-21 12:28:21','2022-03-21 12:36:34','39fa8096-d53b-4637-8f90-03c1d41c8c35'),(12,66,39,'test',1234.0,_binary '',_binary '\0',NULL,NULL,'2022-03-21 12:36:34',NULL,'5eb67829-743c-41ae-9365-73a989e0bd3e'),(13,67,41,'test',1234.0,_binary '\0',_binary '',NULL,NULL,'2022-03-21 13:44:54','2022-03-21 13:54:05','7f889347-e7ec-48a3-924f-da1d5fc3e98c'),(14,67,41,'test',1234.0,_binary '',_binary '\0',NULL,NULL,'2022-03-21 13:54:05',NULL,'b2c7e987-ac8c-4793-b7d0-6720a9324662'),(15,68,44,'test',1234.0,_binary '\0',_binary '',NULL,NULL,'2022-03-21 14:04:06','2022-03-21 14:04:52','48b4cec8-1513-441b-829b-e817c028d8fb'),(16,68,44,'test',1234.0,_binary '\0',_binary '',NULL,NULL,'2022-03-21 14:04:52','2022-03-22 04:38:30','9ff9ee35-754f-4ea8-8b91-6336ef1aa11e'),(17,68,21,'test',1234.0,_binary '\0',_binary '',NULL,NULL,'2022-03-22 04:38:30','2022-03-22 05:09:41','109e1881-87d4-4834-945c-d150eff0b61d'),(18,68,21,'test',1234.0,_binary '\0',_binary '',NULL,NULL,'2022-03-22 05:09:41','2022-03-22 05:12:36','5cc521db-a1d3-45b2-b2a2-36a0c86d7ad9'),(19,68,21,'test',1234.0,_binary '\0',_binary '',NULL,NULL,'2022-03-22 05:12:36','2022-03-23 03:15:49','9ee90591-d7d8-4542-9bc6-d7608d565c9e'),(20,68,21,'test',1234.0,_binary '',_binary '\0',NULL,NULL,'2022-03-23 03:15:49',NULL,'fff0689d-0ca0-412e-9476-826a5f5bc5bd'),(21,82,47,'test',232323.0,_binary '\0',_binary '',NULL,NULL,'2022-03-25 07:37:53','2022-03-29 10:23:42','4a9f3400-a0f2-43c0-be99-96413f466d91'),(22,82,47,'test',232323.0,_binary '',_binary '\0',NULL,NULL,'2022-03-29 10:23:42',NULL,'c7bc5078-16aa-4c61-929a-91def6858e88'),(31,84,57,'test',1000.0,_binary '',_binary '\0',NULL,NULL,'2022-03-30 13:41:12',NULL,'0f35dc85-047b-4304-936a-c1c3210a7234'),(32,86,59,'test',4545.0,_binary '',_binary '\0',NULL,NULL,'2022-03-30 14:02:19',NULL,'9348b9ee-79e0-4f73-ba31-2fc52feb6993'),(33,91,68,'test',1000.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 21:52:24','2022-03-30 21:58:12','a9acb495-3aad-4e72-9b70-16383d90ed66'),(34,91,69,'test',100.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 21:52:24','2022-03-30 21:58:12','a62681c5-35c3-4f11-a754-a731df0189ce'),(38,91,68,'test',1000.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 21:58:12','2022-03-30 22:00:33','28745fe9-608e-4519-8533-ea03207fced5'),(39,91,69,'test',100.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 21:58:12','2022-03-30 22:00:33','3d442a0e-8d06-40fa-9dd6-6e0a0f286012'),(40,91,68,'test',1000.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 22:00:33','2022-03-30 22:19:47','e3b37d65-d1a6-4ee5-b795-91790aa3b8ea'),(41,91,69,'test',100.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 22:00:33','2022-03-30 22:19:47','17a99dac-243e-4242-854c-5309be72722b'),(42,91,68,'test',1000.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 22:19:47','2022-03-30 22:21:50','940d3c03-54e9-45ca-a94c-f7d60fcb36f8'),(43,91,69,'test',100.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 22:19:47','2022-03-30 22:21:50','abb6081a-0f92-468d-a21c-aeb5b673bc39'),(44,91,68,'test',1000.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 22:21:50','2022-03-30 22:23:40','d77d21a5-27f7-41ba-ae6f-706cae635607'),(45,91,69,'test',100.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 22:21:50','2022-03-30 22:23:40','3dff6b3d-578d-44c7-9438-fbcd346b8033'),(46,91,68,'test',1000.0,_binary '',_binary '\0',NULL,NULL,'2022-03-30 22:23:40',NULL,'2352d8b2-e9d9-43c7-bc58-2651269f11cf'),(47,91,69,'test',100.0,_binary '',_binary '\0',NULL,NULL,'2022-03-30 22:23:40',NULL,'1a83362a-bb68-46ef-ac7d-33530ebbd857'),(48,92,72,'test',1009.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 23:22:43','2022-03-30 23:27:26','e4d3b743-275f-4877-b786-4cb96ad2635b'),(51,92,72,'test',456.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 23:27:26','2022-03-30 23:28:22','707c52af-5788-4dee-ba97-2ee4d7c1d3ff'),(52,92,72,'test',1009.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 23:27:26','2022-03-30 23:28:22','e3a22f18-c7b1-41f3-8cca-83c111225cc1'),(53,92,72,'test',456.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 23:28:22','2022-04-05 03:36:52','f5cc893a-fb08-4ff3-b906-1de350250de5'),(54,92,72,'test',456.0,_binary '\0',_binary '',NULL,NULL,'2022-04-05 03:26:19','2022-04-05 03:36:52','5351ee1b-d2d9-4856-8fb9-82fc423d868c'),(55,92,72,'test',456.0,_binary '\0',_binary '',NULL,NULL,'2022-04-05 03:36:52','2022-04-05 03:38:16','6fc60cd7-3509-49cc-828a-eed1d0179834'),(56,92,72,'test',456.0,_binary '\0',_binary '',NULL,NULL,'2022-04-05 03:36:52','2022-04-05 03:38:16','ccea8b1d-9e78-487a-89e8-b9de3be23783'),(57,92,72,'test',456.0,_binary '\0',_binary '',NULL,NULL,'2022-04-05 03:38:16','2022-04-05 03:41:00','8d779e61-c117-4fa2-834e-5a734692f7e8'),(58,92,72,'test',456.0,_binary '\0',_binary '',NULL,NULL,'2022-04-05 03:38:16','2022-04-05 03:41:00','6dc34a3b-e9f8-430f-8546-cc37198b4940'),(59,92,72,'test',456.0,_binary '\0',_binary '',NULL,NULL,'2022-04-05 03:41:00','2022-04-06 12:14:01','c16a5336-880e-4c42-84e0-1fa12458d240'),(60,92,72,'test',456.0,_binary '\0',_binary '',NULL,NULL,'2022-04-05 03:41:00','2022-04-06 12:14:01','ae8ca020-4920-4b37-ab78-18d7f9251050'),(61,92,72,'test',456.0,_binary '\0',_binary '',NULL,NULL,'2022-04-06 12:14:01','2022-04-07 07:58:16','303a8379-d69b-4bc8-9d1b-012ed67153c6'),(62,92,72,'test',456.0,_binary '\0',_binary '',NULL,NULL,'2022-04-06 12:14:01','2022-04-07 07:58:16','c657a66a-2e8c-4f17-b29e-e44c6807431a'),(63,92,72,'test',100.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 07:29:29','2022-04-07 07:58:16','cd801099-b8fa-448c-b729-a21bb6148167'),(64,92,72,'test',100.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 07:29:29','2022-04-07 07:58:16','2f008a38-7b54-4c86-9b3f-0000872aba8d'),(65,92,72,'test',100.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 07:58:16','2022-04-07 08:02:37','76c19131-765b-4cf7-95d1-d516aff24df8'),(66,92,72,'test',100.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 07:58:16','2022-04-07 08:02:37','e2517995-696e-49b8-b9f6-0bdc2398c3b5'),(67,92,72,'test',100.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 08:02:37','2022-04-07 08:10:26','c1711e4d-8c17-4922-a462-89a35ad45e1f'),(68,92,72,'test',100.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 08:02:37','2022-04-07 08:10:26','a0504186-e6fa-40aa-ae02-a638dc8ad574'),(69,92,72,'test',100.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 08:10:26','2022-04-07 08:11:47','ee50b765-b78d-44bb-bc83-15dc008e875e'),(70,92,72,'test',100.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 08:10:26','2022-04-07 08:11:47','2fda552b-6d68-45d5-8b55-0d2b7439bdd6'),(71,92,72,'test',100.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 08:11:47','2022-04-07 08:13:25','3be2af1c-98e6-498b-9e11-8cc087eaefb9'),(72,92,72,'test',100.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 08:11:47','2022-04-07 08:13:25','de8577b5-1f34-4018-bf9d-f91da2de694a'),(73,92,72,'test',100.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 08:13:25','2022-04-07 08:14:58','2a0d9c4c-2e7b-4845-9b85-8ab7578d3078'),(74,92,72,'test',200.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 08:13:25','2022-04-07 08:14:58','e0d5f683-6610-4022-ba63-0cebb9b9130d'),(75,92,72,'test',100.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 08:14:58','2022-04-07 08:18:46','a8e36932-4f2c-42c4-b1f1-dfa68c8e64cf'),(76,92,72,'test',100.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 08:18:46','2022-04-07 08:25:10','7866537c-632a-4714-9465-e28486caf28c'),(77,92,72,'test',200.0,_binary '',_binary '\0',NULL,NULL,'2022-04-07 08:25:10',NULL,'9080da3d-de5f-4915-953b-c93ca0d66262'),(78,94,75,'test',100.0,_binary '',_binary '\0',NULL,NULL,'2022-04-08 06:33:19',NULL,'247ba50c-d7c9-406e-a2c8-e0471b720efc'),(79,95,78,'liability desc',2300.0,_binary '',_binary '\0',NULL,NULL,'2022-04-08 10:04:28',NULL,'9dc7a6cb-30d3-4fe7-947f-bbaa43d78204'),(80,95,77,'liability desc 2',4200.0,_binary '',_binary '\0',NULL,NULL,'2022-04-08 10:04:28',NULL,'fc9cf177-6c10-431a-8662-3aa825e0a9bc'),(81,99,87,'REST',12345.0,_binary '',_binary '\0',NULL,NULL,'2022-04-21 06:30:29',NULL,'c3a4ea60-727c-4269-815e-01e1032fbd69'),(82,104,89,'Bank Loan',1000000.0,_binary '',_binary '\0',NULL,NULL,'2022-04-22 11:00:22',NULL,'db339636-3aaa-478b-9379-ef14b6a801c3');
/*!40000 ALTER TABLE `tbl_enq_pliab_det` ENABLE KEYS */;
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

-- Dump completed on 2022-06-28 19:18:42
