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
-- Table structure for table `tbl_enq_pbank_det`
--

DROP TABLE IF EXISTS `tbl_enq_pbank_det`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_enq_pbank_det` (
  `enq_prombank_id` int NOT NULL AUTO_INCREMENT,
  `enqtemp_id` int DEFAULT NULL,
  `promoter_code` bigint NOT NULL,
  `prom_acctype` varchar(20) DEFAULT NULL,
  `prom_bankaccno` varchar(50) NOT NULL,
  `prom_ifsc` varchar(11) NOT NULL,
  `prom_acc_name` varchar(100) NOT NULL,
  `prom_bankname` varchar(100) DEFAULT NULL,
  `prom_bankbr` varchar(100) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `modified_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  PRIMARY KEY (`enq_prombank_id`),
  KEY `fk_tbl_enq_pbank_det_tbl_enq_temptab` (`enqtemp_id`),
  KEY `fk_tbl_enq_pbank_det_tbl_prom_cdtab` (`promoter_code`),
  CONSTRAINT `fk_tbl_enq_pbank_det_tbl_enq_temptab` FOREIGN KEY (`enqtemp_id`) REFERENCES `tbl_enq_temptab` (`enqtemp_id`),
  CONSTRAINT `fk_tbl_enq_pbank_det_tbl_prom_cdtab` FOREIGN KEY (`promoter_code`) REFERENCES `tbl_prom_cdtab` (`promoter_code`)
) ENGINE=InnoDB AUTO_INCREMENT=92 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_enq_pbank_det`
--

LOCK TABLES `tbl_enq_pbank_det` WRITE;
/*!40000 ALTER TABLE `tbl_enq_pbank_det` DISABLE KEYS */;
INSERT INTO `tbl_enq_pbank_det` VALUES (1,7,4,'string','string','string','string','string','string',_binary '',_binary '\0',NULL,NULL,'2022-01-18 13:55:14',NULL,'623a9160-5d1b-4c30-beb0-87b55d7f347c'),(7,27,12,NULL,'sasas','sasas','sas','Anuj Kumar',NULL,_binary '',_binary '\0',NULL,NULL,'2022-03-05 07:55:06',NULL,'c5a0d60c-2d2a-4d42-b238-ba890b4e8674'),(8,28,13,NULL,'3232','232323','Hdfc','Anuj Kumar',NULL,_binary '',_binary '\0',NULL,NULL,'2022-03-05 08:10:24',NULL,'e4afaa3a-c9f6-4f2a-b8d1-7056528d2215'),(10,29,14,NULL,'sasa','sasas','asas','Anuj Kumar',NULL,_binary '\0',_binary '',NULL,NULL,'2022-03-05 08:23:30','2022-03-05 08:24:54','73ee1d19-670d-4855-83ed-3061109e4109'),(15,31,17,NULL,'sasa','sas','sasa','sas',NULL,_binary '\0',_binary '',NULL,NULL,'2022-03-05 10:24:17','2022-03-05 10:27:52','2830d84f-378a-4e97-9128-289735f86899'),(17,32,18,NULL,'232323','saasas','2323','Anuj Kumar',NULL,_binary '\0',_binary '',NULL,NULL,'2022-03-05 10:32:43','2022-03-05 10:52:35','6b701bb9-c010-45d1-b18f-8606172a5c74'),(19,33,19,NULL,'4343','43434','Hdfc','Anuj Kumar',NULL,_binary '',_binary '\0',NULL,NULL,'2022-03-05 11:01:05',NULL,'8dcb2609-830b-49b5-a7c1-5687c4d8b4e2'),(20,34,20,NULL,'ewe','ewew','Hdfc','Anuj Kumar',NULL,_binary '',_binary '\0',NULL,NULL,'2022-03-05 11:06:22',NULL,'cb555a74-bf9f-4a7e-a69b-d6cc7bdb52b9'),(22,54,22,NULL,'121212','iiiiii','Abhishek Singh Promoter','HDFC',NULL,_binary '',_binary '\0',NULL,NULL,'2022-03-21 07:52:31',NULL,'23ff86c0-ea9e-40e6-b581-81756f6a717f'),(23,55,23,NULL,'4343','dsds','Abhishek Singh Promoter','HDFC',NULL,_binary '',_binary '\0',NULL,NULL,'2022-03-21 10:03:34',NULL,'9c5e96f5-3c9b-4fab-8746-0c16f9b4d14e'),(24,56,24,NULL,'3232','123456','Abhishek Singh Promoter','HDFC',NULL,_binary '',_binary '\0',NULL,NULL,'2022-03-21 10:21:24',NULL,'7bc47d80-8ba3-48d5-adfb-51e123fa2604'),(25,57,25,NULL,'21212','dsds','Abhishek Singh Promoter','HDFC',NULL,_binary '\0',_binary '',NULL,'cmeps9748b','2022-03-21 10:35:04','2022-03-21 10:42:58','461561d2-21e0-465f-b043-0554ca96fc3a'),(27,58,26,NULL,'1212','123456','Abhishek Singh Promoter','HDFC',NULL,_binary '',_binary '\0',NULL,NULL,'2022-03-21 10:54:52',NULL,'66bf4743-d436-4b55-9e8a-65a4e8841253'),(28,59,27,NULL,'1212','123456','Abhishek Singh Promoter','HDFC',NULL,_binary '',_binary '\0',NULL,NULL,'2022-03-21 11:38:36',NULL,'7748ad82-37f4-4145-b292-8cc2dfce1cfa'),(33,64,35,NULL,'21212','123456','Abhishek Singh Promoter','HDFC',NULL,_binary '',_binary '\0',NULL,NULL,'2022-03-21 11:53:28',NULL,'e3ef5813-73a4-48e1-9793-47f385110e17'),(34,65,37,NULL,'121212','123456','Abhishek Singh Promoter','HDFC',NULL,_binary '',_binary '\0',NULL,NULL,'2022-03-21 11:58:59',NULL,'741b4522-9333-4a25-b8c4-4c3d52e6c7cb'),(35,66,39,NULL,'1212','dsds','Abhishek Singh Promoter','HDFC',NULL,_binary '\0',_binary '',NULL,'cmeps9748b','2022-03-21 12:27:24','2022-03-21 12:28:21','1ce799cb-1296-4374-99d3-cc526a12a419'),(38,66,21,NULL,'1212','dsds','Abhishek Singh Promoter','HDFC',NULL,_binary '\0',_binary '','cmeps9748b','cmeps9748b','2022-03-21 12:36:04','2022-03-21 12:37:54','56b87781-8af9-4557-9365-cd5a1b95a874'),(39,66,21,NULL,'1212','dsds','Abhishek Singh Promoter','HDFC',NULL,_binary '\0',_binary '','cmeps9748b','cmeps9748b','2022-03-21 12:37:54','2022-03-21 12:38:21','1a158fe1-91ab-4cd0-82f7-9390bc8526ca'),(40,66,21,NULL,'1212','dsds','Abhishek Singh Promoter','HDFC',NULL,_binary '\0',_binary '','cmeps9748b','cmeps9748b','2022-03-21 12:38:21','2022-03-21 12:39:02','ce12356c-3448-4c11-895e-2c2d1c592cbf'),(41,66,21,NULL,'1212','dsds','Abhishek Singh Promoter','HDFC',NULL,_binary '\0',_binary '','cmeps9748b',NULL,'2022-03-21 12:39:29','2022-03-21 12:43:50','c7799123-0fbd-47c3-818d-b847f38cdcce'),(42,66,21,NULL,'1212','dsds','Abhishek Singh Promoter','HDFC',NULL,_binary '\0',_binary '',NULL,NULL,'2022-03-21 12:44:04','2022-03-21 12:51:55','07e5da25-a2e5-4ecc-8272-3b5dabc8e7b8'),(44,66,21,NULL,'1212','dsds','Abhishek Singh Promoter','HDFC',NULL,_binary '\0',_binary '',NULL,NULL,'2022-03-21 12:53:55','2022-03-21 12:54:06','c239843a-bbef-4138-8686-ef96e0a3da67'),(45,66,21,NULL,'1212','dsds','Abhishek Singh Promoter','HDFC',NULL,_binary '',_binary '\0',NULL,NULL,'2022-03-21 12:54:24',NULL,'fbbc6a8d-9ab3-41a3-aa4b-09e682b4526f'),(46,67,41,NULL,'12121','123456','Abhishek Singh Promoter','HDFC',NULL,_binary '\0',_binary '',NULL,'cmeps9748b','2022-03-21 13:44:46','2022-03-21 13:45:26','00bc9fe2-3d60-4b82-bda7-605afe521634'),(47,67,42,NULL,'1212','123456','Abhishek Singh Promoter','HDFC',NULL,_binary '\0',_binary '',NULL,'cmeps9748b','2022-03-21 13:44:46','2022-03-21 13:45:26','d3faef98-b378-436c-9010-d9974e92c5f0'),(48,67,21,NULL,'12121','123456','Abhishek Singh Promoter','HDFC',NULL,_binary '\0',_binary '','cmeps9748b','cmeps9748b','2022-03-21 13:45:54','2022-03-21 13:52:55','ec69f788-bdb7-4013-aa7c-ff64bb328e4d'),(49,67,21,NULL,'12121','123456','Abhishek Singh Promoter','HDFC',NULL,_binary '',_binary '\0','cmeps9748b',NULL,'2022-03-21 13:53:08',NULL,'7768e025-a86e-4450-90fc-930a4c55d756'),(50,67,21,NULL,'1212','123456','Abhishek Singh Promoter','HDFC',NULL,_binary '',_binary '\0','cmeps9748b',NULL,'2022-03-21 13:53:21',NULL,'b180a7fe-ba5a-4cbd-8dcc-0cb8c9b20239'),(51,68,44,NULL,'2121','ICICCIC','Abhishek Singh Promoter','HDFC',NULL,_binary '\0',_binary '',NULL,'cmeps9748b','2022-03-21 14:04:03','2022-03-21 14:04:43','f0b3662b-85e4-4285-8200-daaa97478b56'),(52,68,21,NULL,'2121','ICICCIC','Abhishek Singh Promoter','HDFC',NULL,_binary '\0',_binary '','cmeps9748b','cmeps9748b','2022-03-21 14:04:46','2022-03-22 04:38:25','f8ffc80d-b219-4227-9de6-b83e3bbb6d17'),(53,68,21,NULL,'2121','ICICCIC','Abhishek Singh Promoter','HDFC',NULL,_binary '\0',_binary '','cmeps9748b','cmeps9748b','2022-03-22 04:38:26','2022-03-22 05:09:38','635a66f8-93f8-496d-b5dd-0805a3d569a9'),(54,68,21,NULL,'2121','ICICCIC','Abhishek Singh Promoter','HDFC',NULL,_binary '\0',_binary '','cmeps9748b','cmeps9748b','2022-03-22 05:09:38','2022-03-22 05:12:36','50720528-bbbe-41e7-a69a-41fe9e4c2bc3'),(55,68,21,NULL,'2121','ICICCIC','Abhishek Singh Promoter','HDFC',NULL,_binary '\0',_binary '','cmeps9748b','cmeps9748b','2022-03-22 05:12:36','2022-03-23 03:15:48','7a8d631f-c790-4432-834e-587b956c3944'),(56,68,29,NULL,'2121','ICICCIC','Abhishek Singh Promoter','HDFC',NULL,_binary '',_binary '\0','cmeps9748b',NULL,'2022-03-23 03:15:48',NULL,'c724f05d-bf8c-4d8b-bbc4-87011ad67d49'),(57,82,47,NULL,'456211','ICIC','ghghgh','hdfc',NULL,_binary '\0',_binary '','cmeps9748b','CMEPS9748B','2022-03-25 07:37:53','2022-03-30 13:36:58','8eace457-c775-4557-afa4-6630b7fd9dfc'),(58,82,47,NULL,'456211','ICIC','ghghgh','hdfc',NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-03-30 13:36:58',NULL,'77e3b451-f280-4d76-99bf-bb5e85d9f20e'),(59,84,57,NULL,'12212','dsdsd','212','12',NULL,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-03-30 13:38:40','2022-03-30 13:40:41','4aa45bb0-6ff0-4857-98b1-2524282ba3f9'),(60,84,47,NULL,'12212','dsdsd','212','12',NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-03-30 13:40:41',NULL,'c76f3b8a-7590-4902-ab18-e6d6ed1f0a56'),(61,85,58,NULL,'123456','ICICICICI','sasa','10',NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-03-30 13:46:11',NULL,'3504d809-7d31-497e-9758-8ba264567d6d'),(62,86,59,NULL,'22222','ICICICICI','Hdfc','Anuj Kumar',NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-03-30 14:02:01',NULL,'b1f34648-dfc7-427a-94b6-1aef06227571'),(63,87,61,NULL,'12345','ICICICICI','Hdfc','HDFC',NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-03-30 19:20:19',NULL,'e73a58e1-c0a4-49d1-bd4d-ab6c6a4998f2'),(64,87,62,NULL,'12345','ICICICICI','Hdfc','HDFC',NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-03-30 19:24:59',NULL,'a78143db-d07d-4e31-9465-7ec41cbb76b6'),(65,88,63,NULL,'12345678','ICICICCI','Abhishek','Hdfc',NULL,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-03-30 20:03:46','2022-03-30 20:05:26','f5f12724-ccce-4278-b4c5-cf6da7431b9f'),(66,89,64,NULL,'1234567','ICICICICI','Hdfc','HDFC',NULL,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-03-30 20:41:07','2022-03-30 20:43:03','f941dd88-0fb9-4e9a-9dcf-a7169393c42a'),(67,89,65,NULL,'123456','ICICICICI','Hdfc','HDFC',NULL,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-03-30 20:42:08','2022-03-30 20:43:03','7421f499-95fe-41ce-81c1-8d3ed27148b6'),(68,89,65,NULL,'123456','ICICICICI','Hdfc','HDFC',NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-03-30 20:43:03',NULL,'59c3cf59-8f2c-4a14-890f-c668ef1e2140'),(69,89,64,NULL,'1234567','ICICICICI','Hdfc','HDFC',NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-03-30 20:43:03',NULL,'cbce7795-19df-432c-ae85-1aa2f2d6bfdf'),(70,90,66,NULL,'333333','ICICICICI','fdfdf','HDFC',NULL,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-03-30 20:58:03','2022-03-30 20:59:09','aac3d42f-158b-4954-89fa-66550ac9e6c3'),(71,90,67,NULL,'45678','ICICICICI','Hdfc','HDFC',NULL,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-03-30 20:58:42','2022-03-30 20:59:09','33bf96d0-f70f-4aaf-b253-28724ee5a21c'),(72,90,66,NULL,'333333','ICICICICI','fdfdf','HDFC',NULL,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-03-30 20:59:09','2022-03-30 20:59:48','77f025ce-c9c7-4225-a842-5bdf9ac011f0'),(73,90,66,NULL,'333333','ICICICICI','fdfdf','HDFC',NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-03-30 20:59:48',NULL,'645c747c-d931-4ca4-bc3b-749f1db945be'),(74,91,68,NULL,'123456','ICICICICI','abbb','HDFC',NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-03-30 21:32:53',NULL,'cbef6eab-6941-400a-bbfd-7cc3c54f0512'),(75,91,69,NULL,'345','ICICICICI','gggg','gggg',NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-03-30 21:33:31',NULL,'fe7d9d32-df8a-4904-9e76-31e9e62c8357'),(76,92,72,NULL,'323232','ICICICICI','dddd','ddddd',NULL,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-03-30 23:09:17','2022-03-30 23:09:37','79c3ce59-bb85-4bce-a80a-65ed5b8f4e33'),(77,92,72,NULL,'323232','ICICICICI','dddd','ddddd',NULL,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-03-30 23:09:37','2022-03-30 23:12:23','c71c6b63-83e1-41cd-a1f0-605d0492fa27'),(78,92,72,NULL,'323232','ICICICICI','dddd','ddddd',NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-03-30 23:12:23',NULL,'c789b02f-9ea3-443f-99e0-d6251bd80bc7'),(79,92,74,NULL,'134567','ICICCIC','hhhhh','HDFC',NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-04-03 19:32:44',NULL,'c7a9a6e4-fd35-41c1-94c3-ea8dcf8fba0d'),(80,94,75,NULL,'3455','iciciic','HDFC','HDFC',NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-04-08 06:31:05',NULL,'54c7a99b-497c-4803-a7fd-5f95688f08fc'),(81,95,77,NULL,'46879647','SBI534543','Abhishek singh','SBI',NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-04-08 09:52:12',NULL,'62d1061b-771b-4c07-97bb-02b69dcc1d71'),(82,95,78,NULL,'878946','sbi654687','kumar ','SBI',NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-04-08 09:54:13',NULL,'74efdb2b-f8fd-4fc6-9f80-3292417b268a'),(90,99,87,NULL,'12','12','Test','ICIC',NULL,_binary '',_binary '\0','AJNPB1985K',NULL,'2022-04-21 06:28:56',NULL,'ab66c0b8-5751-43f9-a7aa-9f0f68be66e1'),(91,104,89,NULL,'05510151111','ICIC0000938','Kokila V','icici bank',NULL,_binary '',_binary '\0','BADPS0712F',NULL,'2022-04-22 10:47:05',NULL,'eba91143-e9f7-4f17-b90a-72d4c6184dbb');
/*!40000 ALTER TABLE `tbl_enq_pbank_det` ENABLE KEYS */;
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

-- Dump completed on 2022-06-28 19:20:15
