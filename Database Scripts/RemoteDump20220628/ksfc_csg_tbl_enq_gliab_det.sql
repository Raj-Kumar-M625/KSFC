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
-- Table structure for table `tbl_enq_gliab_det`
--

DROP TABLE IF EXISTS `tbl_enq_gliab_det`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_enq_gliab_det` (
  `enq_guarliab_id` int NOT NULL AUTO_INCREMENT,
  `enqtemp_id` int DEFAULT NULL,
  `promoter_code` bigint NOT NULL,
  `guar_liab_desc` varchar(500) DEFAULT NULL,
  `guar_liab_value` decimal(10,1) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `modified_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  PRIMARY KEY (`enq_guarliab_id`),
  KEY `fk_tbl_enq_gliab_det_tbl_enq_temptab` (`enqtemp_id`),
  KEY `fk_tbl_enq_gliab_det_tbl_prom_cdtab` (`promoter_code`),
  CONSTRAINT `fk_tbl_enq_gliab_det_tbl_enq_temptab` FOREIGN KEY (`enqtemp_id`) REFERENCES `tbl_enq_temptab` (`enqtemp_id`),
  CONSTRAINT `fk_tbl_enq_gliab_det_tbl_prom_cdtab` FOREIGN KEY (`promoter_code`) REFERENCES `tbl_prom_cdtab` (`promoter_code`)
) ENGINE=InnoDB AUTO_INCREMENT=74 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_enq_gliab_det`
--

LOCK TABLES `tbl_enq_gliab_det` WRITE;
/*!40000 ALTER TABLE `tbl_enq_gliab_det` DISABLE KEYS */;
INSERT INTO `tbl_enq_gliab_det` VALUES (2,7,5,'Dev123',12.0,_binary '',_binary '\0',NULL,NULL,'2022-01-19 10:21:19',NULL,'944370e2-f9d7-448f-a55d-2f583070fc2a'),(8,66,40,'test',12.0,_binary '\0',_binary '',NULL,NULL,'2022-03-21 12:27:47','2022-03-21 12:29:01','5c295c87-29eb-4cc1-90a8-53067925b04d'),(9,66,40,'test',12.0,_binary '\0',_binary '',NULL,NULL,'2022-03-21 12:29:01','2022-03-21 12:36:52','3b708836-840a-40db-950e-4d82bb9263ef'),(10,66,40,'test',12.0,_binary '',_binary '\0',NULL,NULL,'2022-03-21 12:36:52',NULL,'b710a2c1-305d-410d-b6b9-88c048253364'),(11,67,43,'trest',12.0,_binary '\0',_binary '',NULL,NULL,'2022-03-21 13:44:56','2022-03-21 13:54:33','9c7458a1-1b7b-4e79-8804-cb596bd9511a'),(12,67,43,'trest',12.0,_binary '',_binary '\0',NULL,NULL,'2022-03-21 13:54:33',NULL,'9fa96565-1445-4e12-b591-5bb7a2e079d5'),(13,68,45,'test',12.0,_binary '\0',_binary '',NULL,NULL,'2022-03-21 14:04:22','2022-03-21 14:05:14','e464a8c3-6454-4633-8544-95d5737048d0'),(14,68,45,'te',5656.0,_binary '\0',_binary '',NULL,NULL,'2022-03-21 14:04:22','2022-03-21 14:05:14','21c7e6db-3b14-4b39-a844-e880f48a5faa'),(15,68,45,'test',12.0,_binary '\0',_binary '',NULL,NULL,'2022-03-21 14:05:14','2022-03-22 04:39:21','b286ba07-caf2-4e3a-9c0f-b3bb436956c0'),(16,68,45,'te',5656.0,_binary '\0',_binary '',NULL,NULL,'2022-03-21 14:05:14','2022-03-22 04:39:21','dc3bb77c-7017-4472-89c7-9ecaf6f1043e'),(17,68,21,'test',12.0,_binary '\0',_binary '',NULL,NULL,'2022-03-22 04:39:21','2022-03-22 05:10:05','2f80e187-1104-4141-b622-90f420a77c69'),(18,68,21,'te',5656.0,_binary '\0',_binary '',NULL,NULL,'2022-03-22 04:39:21','2022-03-22 05:10:05','9fc3398c-deb9-4653-aae8-743f536fd600'),(19,68,21,'test',12.0,_binary '\0',_binary '',NULL,NULL,'2022-03-22 05:10:05','2022-03-22 05:12:39','5f50df8e-74f6-4554-9e65-aa3ff08dd5ae'),(20,68,21,'te',5656.0,_binary '\0',_binary '',NULL,NULL,'2022-03-22 05:10:05','2022-03-22 05:12:39','3629a82c-4bff-4d52-8292-ad4529e0649a'),(21,68,21,'test',12.0,_binary '\0',_binary '',NULL,NULL,'2022-03-22 05:12:39','2022-03-23 03:15:56','6de9bdf4-6050-43f7-9ff2-d1ee0e27febb'),(22,68,21,'te',5656.0,_binary '\0',_binary '',NULL,NULL,'2022-03-22 05:12:39','2022-03-23 03:15:56','0bb60220-2fd8-4247-8f26-ed21c2ada5ad'),(23,68,21,'test',12.0,_binary '',_binary '\0',NULL,NULL,'2022-03-23 03:15:56',NULL,'423a2b64-28b2-4207-9544-29cea18c4a69'),(24,68,21,'te',5656.0,_binary '',_binary '\0',NULL,NULL,'2022-03-23 03:15:56',NULL,'3d722464-053f-4d2b-bcdc-c853b2a6d1ae'),(25,82,21,'test',3434.0,_binary '\0',_binary '',NULL,NULL,'2022-03-25 07:37:54','2022-03-29 10:23:43','e4530d9a-cda0-4b0c-a85f-73cece6810fc'),(26,82,21,'test',3434.0,_binary '',_binary '\0',NULL,NULL,'2022-03-29 10:23:43',NULL,'16a21bfe-d396-4984-b6fd-c7436d61ff1f'),(34,86,60,'test',100.0,_binary '',_binary '\0',NULL,NULL,'2022-03-30 14:02:32',NULL,'dc8f07c3-085a-417c-8f93-8d194c3b0f2a'),(35,91,70,'test',100.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 21:56:01','2022-03-30 21:58:20','52b93be4-ec03-40d6-98d9-57d7ef87ae73'),(36,91,70,'test',100.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 21:58:20','2022-03-30 21:59:05','30acff1f-1af4-4210-a6d4-5d7034a08434'),(37,91,70,'test',100.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 21:59:05','2022-03-30 22:00:33','4f8dd391-18e9-42f7-9488-a8b23e8f6e87'),(38,91,70,'test',100.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 22:00:33','2022-03-30 22:01:43','72ba4718-6339-4ec3-8938-b000cfbcd7f9'),(39,91,70,'test',100.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 22:01:43','2022-03-30 22:19:55','b7a49e40-cd82-4922-afd8-d8b0a85640e3'),(40,91,70,'test',100.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 22:19:55','2022-03-30 22:21:50','07b451e4-8799-48cd-acc2-a03e9bcdf990'),(41,91,70,'test',100.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 22:21:50','2022-03-30 22:23:40','0d9f2b22-0a86-4506-a04c-9379d4747e15'),(42,91,70,'test',100.0,_binary '',_binary '\0',NULL,NULL,'2022-03-30 22:23:40',NULL,'1d9b449a-bd91-4916-8b21-541c9e9187bb'),(43,92,73,'test',20000.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 23:22:43','2022-03-30 23:23:46','8ffb0ae8-86fb-4ba9-9f51-d774f4e54d28'),(44,92,73,'test',20000.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 23:23:46','2022-03-30 23:24:31','689c6114-20f7-4ef8-a3fa-393afdf4c0d0'),(45,92,73,'test',20000.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 23:24:31','2022-03-30 23:27:31','38241719-9ad2-4273-b814-7e47adced3e1'),(46,92,73,'test',20000.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 23:27:31','2022-03-30 23:28:22','f9c5a43e-4e96-47f6-a9f1-851ab729009f'),(47,92,73,'test',34.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 23:28:22','2022-04-05 03:37:33','f1562596-0232-4095-96f4-52b6733d88d7'),(48,92,73,'test',20000.0,_binary '\0',_binary '',NULL,NULL,'2022-03-30 23:28:22','2022-04-05 03:37:33','d082e6f5-85cc-4a63-bdf4-d6de95adb152'),(49,92,73,'test',34.0,_binary '\0',_binary '',NULL,NULL,'2022-04-05 03:37:33','2022-04-05 03:39:53','e8a59838-584d-4531-a0e6-9ee5adff404a'),(50,92,73,'test',200.0,_binary '\0',_binary '',NULL,NULL,'2022-04-05 03:37:33','2022-04-05 03:39:53','3b87cefc-743f-4428-bfb9-030fcf768164'),(51,92,73,'test',34.0,_binary '\0',_binary '',NULL,NULL,'2022-04-05 03:39:53','2022-04-05 03:41:20','4325bad3-36c3-4580-96f0-6ad413bc0ca4'),(52,92,73,'test',200.0,_binary '\0',_binary '',NULL,NULL,'2022-04-05 03:39:53','2022-04-05 03:41:20','1e92f3aa-38ef-43d6-8d1a-833b387cc051'),(53,92,73,'test',34.0,_binary '\0',_binary '',NULL,NULL,'2022-04-05 03:41:20','2022-04-07 07:29:38','860a56fa-1d5b-4463-8c6a-523c71d993b5'),(54,92,73,'test',200.0,_binary '\0',_binary '',NULL,NULL,'2022-04-05 03:41:20','2022-04-07 07:29:38','256ee8c0-d9d8-4537-b88c-87ebd0941457'),(55,92,73,'test',200.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 07:29:38','2022-04-07 07:59:04','c8eaa602-f1fb-4ce1-8f0b-bad569b819ac'),(56,92,73,'test',100.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 07:29:38','2022-04-07 07:59:04','6afe2a42-b28e-4c9f-a1ec-fd8d6682c847'),(57,92,73,'test',200.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 07:59:04','2022-04-07 08:03:22','953e2b76-8891-42df-a6d8-b1d4f762ba62'),(58,92,73,'test',100.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 07:59:04','2022-04-07 08:03:22','5e4738c6-573d-4e51-836d-362d282b2480'),(59,92,73,'test',200.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 08:03:22','2022-04-07 08:10:38','399df688-a040-4f0e-8f15-4cc0d2cc2190'),(60,92,73,'test',100.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 08:03:22','2022-04-07 08:10:38','e2331474-9e3d-49e6-a361-a8c5c26a534e'),(61,92,73,'test',200.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 08:10:38','2022-04-07 08:12:08','05fdc440-8eca-49f6-8a58-693260faa72e'),(62,92,73,'test',100.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 08:10:38','2022-04-07 08:12:08','f1bcfb6b-86fd-4473-86a4-fa399c2503ad'),(63,92,73,'test',200.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 08:12:08','2022-04-07 08:13:38','861754d6-4222-40d3-be4c-deb2db113987'),(64,92,73,'test',100.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 08:12:08','2022-04-07 08:13:38','6ff02d62-90c2-4c51-bb3e-4974d78e46c1'),(65,92,73,'test',200.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 08:13:38','2022-04-07 08:14:58','38ba48f1-6a92-471d-93b5-b45d44f94bad'),(66,92,73,'test',100.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 08:13:38','2022-04-07 08:14:58','4b4cb655-7d4d-4822-88e6-5a84d93a47af'),(67,92,73,'test',200.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 08:14:58','2022-04-07 08:18:55','30ff97ad-6445-4be9-83cb-97d9fae1af7d'),(68,92,73,'test',200.0,_binary '\0',_binary '',NULL,NULL,'2022-04-07 08:18:55','2022-04-07 08:25:15','4bad7deb-271f-4ab7-bb36-2d398a757b66'),(69,92,73,'test',300.0,_binary '',_binary '\0',NULL,NULL,'2022-04-07 08:25:15',NULL,'d512c64e-0a93-45d1-ab38-a0c6c5fd4ecc'),(70,94,76,'tet',200.0,_binary '',_binary '\0',NULL,NULL,'2022-04-08 06:33:20',NULL,'408c6bad-d59a-49ec-ac6f-e7c3af222279'),(71,95,79,'liabilit desc',3400.0,_binary '',_binary '\0',NULL,NULL,'2022-04-08 10:04:29',NULL,'b8ef7a2e-02e7-41ef-85e2-93b981b4ef59'),(72,99,88,'123',123454.0,_binary '',_binary '\0',NULL,NULL,'2022-04-21 06:30:30',NULL,'ce9ef5a7-433b-4669-b1d2-ee538fb4246d'),(73,104,90,'bank loan ',1000000.0,_binary '',_binary '\0',NULL,NULL,'2022-04-22 11:00:22',NULL,'6cbf1aa5-7950-4d58-a788-258c649fc24a');
/*!40000 ALTER TABLE `tbl_enq_gliab_det` ENABLE KEYS */;
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

-- Dump completed on 2022-06-28 19:18:24
