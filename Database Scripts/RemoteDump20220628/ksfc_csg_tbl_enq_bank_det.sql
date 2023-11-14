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
-- Table structure for table `tbl_enq_bank_det`
--

DROP TABLE IF EXISTS `tbl_enq_bank_det`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_enq_bank_det` (
  `enq_bank_id` int NOT NULL AUTO_INCREMENT,
  `enqtemp_id` int DEFAULT NULL,
  `enq_acctype` varchar(20) DEFAULT NULL,
  `enq_bankaccno` varchar(50) NOT NULL,
  `enq_ifsc` varchar(11) NOT NULL,
  `enq_acc_name` varchar(100) NOT NULL,
  `enq_bankname` varchar(100) DEFAULT NULL,
  `enq_bankbr` varchar(100) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `modified_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  PRIMARY KEY (`enq_bank_id`),
  KEY `fk_tbl_enq_bank_det_tbl_enq_temptab` (`enqtemp_id`),
  CONSTRAINT `fk_tbl_enq_bank_det_tbl_enq_temptab` FOREIGN KEY (`enqtemp_id`) REFERENCES `tbl_enq_temptab` (`enqtemp_id`)
) ENGINE=InnoDB AUTO_INCREMENT=40 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_enq_bank_det`
--

LOCK TABLES `tbl_enq_bank_det` WRITE;
/*!40000 ALTER TABLE `tbl_enq_bank_det` DISABLE KEYS */;
INSERT INTO `tbl_enq_bank_det` VALUES (1,7,'1','123456','TEST','ASHISH','BOB','1',_binary '\0',NULL,NULL,NULL,'2022-01-18 10:23:42',NULL,'44159ad0-de15-445a-9487-a4f2665a5159'),(2,1,'Devfdd','12344566','asdf123','Pappu','pappu','string',_binary '\0',NULL,NULL,NULL,'2022-01-27 06:57:12',NULL,'dbd0e1b7-6b17-4da6-b25b-240cea7be1c2'),(3,1,'Devfdd1','12344566','asdf123','Pappu1','pappu2','string',_binary '\0',_binary '',NULL,NULL,'2022-01-27 07:41:15','2022-01-27 07:43:24','string'),(4,17,NULL,'1234567897899','12345678','HDFC Bank','HDFC','Noida',_binary '',_binary '\0',NULL,NULL,'2022-02-08 07:22:35',NULL,'5c4dd3ce-92b3-42ef-9d35-b88e35c49631'),(5,18,NULL,'1222222222','ICIC0001577','HDFC Bank','HDFC','Noida',_binary '',_binary '\0',NULL,NULL,'2022-02-08 09:03:58',NULL,'eef1a1be-ede7-44d0-96a3-6b20303b6213'),(6,19,NULL,'4343434343','ICIC0001577','HDFC Bank','HDFC','Noida',_binary '',_binary '\0',NULL,NULL,'2022-02-09 11:21:27',NULL,'8fccc015-09fe-44ce-9ab4-ccfc3bee23a5'),(7,20,NULL,'43434','ICIC0001577','HDFC Bank','HDFC','Noida',_binary '',_binary '\0',NULL,NULL,'2022-02-09 12:40:51',NULL,'0fc9e5b0-4ec9-4280-8830-df2bac0751eb'),(8,22,NULL,'44444444','ICIC0001577','HDFC Bank','HDFC','Noida',_binary '',_binary '\0',NULL,NULL,'2022-02-15 06:11:06',NULL,'2cb05085-c554-44a1-985f-59f5556ba896'),(9,23,NULL,'45454dfdfdf','ICIC0001577','HDFC Bank','HDFC','Noida',_binary '',_binary '\0',NULL,NULL,'2022-02-15 07:24:43',NULL,'cd4c1a90-73e8-4fb6-9d65-0e6c457d4077'),(10,30,NULL,'5454','454545','45454','Anuj Kumar','Anuj Kumar',_binary '',_binary '\0',NULL,NULL,'2022-03-05 09:58:39',NULL,'95635cb9-b563-466f-9fb3-989b55f20312'),(11,35,NULL,'343434','ICIC0001577','HDFC Bank','HDFC','Noida',_binary '',_binary '\0',NULL,NULL,'2022-03-09 12:46:15',NULL,'1fb79eda-3b7a-4a85-a7dd-5286416af3a0'),(12,35,NULL,'74384738','ICIC0001577','HDFC Bank','HDFC','Noida',_binary '',_binary '\0',NULL,NULL,'2022-03-09 12:54:56',NULL,'6b9e82f4-ed06-4fdf-80f1-89daedf8f9da'),(13,35,NULL,'232323','ICIC0001577','HDFC Bank','HDFC','Noida',_binary '',_binary '\0',NULL,NULL,'2022-03-09 13:00:56',NULL,'4738cde7-22f9-4085-abfe-78eef639ed35'),(14,35,NULL,'232323','ICIC0001577','HDFC Bank','HDFC','Noida',_binary '',_binary '\0',NULL,NULL,'2022-03-09 13:01:21',NULL,'83059198-040e-40c1-a019-ecf1921e7774'),(15,35,NULL,'232323','ICIC0001577','HDFC Bank','HDFC','Noida',_binary '',_binary '\0',NULL,NULL,'2022-03-09 13:01:44',NULL,'bf9aa1b2-f7d4-45a2-bc94-0fe7405101d9'),(16,35,NULL,'232323','ICIC0001577','HDFC Bank','HDFC','Noida',_binary '',_binary '\0',NULL,NULL,'2022-03-09 13:02:25',NULL,'d109ff00-7de0-4809-99c4-11fb7bb8fed4'),(17,35,NULL,'232323','ICIC0001577','HDFC Bank','HDFC','Noida',_binary '',_binary '\0',NULL,NULL,'2022-03-09 13:02:27',NULL,'9ec86be3-9cf0-4619-b595-53289ebcfe62'),(18,35,NULL,'232323','ICIC0001577','HDFC Bank','HDFC','Noida',_binary '',_binary '\0',NULL,NULL,'2022-03-09 13:02:31',NULL,'3d807e55-03c4-4a0e-866c-71c80b5f1313'),(19,35,NULL,'232323','ICIC0001577','HDFC Bank','HDFC','Noida',_binary '',_binary '\0',NULL,NULL,'2022-03-09 13:02:42',NULL,'bd6f6cb0-9cd7-40f3-9198-3d5d1ad8992b'),(20,35,NULL,'232323','ICIC0001577','HDFC Bank','HDFC','Noida',_binary '',_binary '\0',NULL,NULL,'2022-03-09 13:02:53',NULL,'710f531f-eeb6-4a90-9708-88932cad09c1'),(21,36,NULL,'54545','ICIC0001577','HDFC Bank','HDFC','Noida',_binary '',_binary '\0',NULL,NULL,'2022-03-09 13:12:50',NULL,'f07dff94-68cd-4db6-9d5e-e9587327a605'),(22,39,NULL,'323','IIIIIIIII','CCC','AA','BB',_binary '',_binary '\0',NULL,NULL,'2022-03-10 08:33:37',NULL,'9539663d-ca3f-4530-86a6-e9ab10591338'),(23,40,NULL,'43434','IIIIIIIII','CCC','AA','CC',_binary '',_binary '\0',NULL,NULL,'2022-03-10 08:41:58','2022-03-10 08:43:50',NULL),(24,41,NULL,'21212','IIIIIIIII','CCC','AA','BB',_binary '',_binary '\0',NULL,NULL,'2022-03-10 08:53:25','2022-03-10 08:53:46',NULL),(25,42,NULL,'232323','IIIIIIIII','adad','AA','BB',_binary '',_binary '\0',NULL,NULL,'2022-03-10 09:03:21','2022-03-10 09:05:48',NULL),(26,43,NULL,'232323','ICIC0001577','HDFC Bank','HDFC','Noida',_binary '',_binary '\0',NULL,NULL,'2022-03-10 09:18:03','2022-03-10 09:18:32',NULL),(27,44,NULL,'1111111','ICIC0001577','HDFC Bank','HDFC','Noida',_binary '',_binary '\0',NULL,NULL,'2022-03-10 09:41:14','2022-03-10 09:43:15',NULL),(28,45,NULL,'111111','ICIC0001577','HDFC Bank','HDFC','Noida',_binary '',_binary '\0',NULL,NULL,'2022-03-11 06:56:31','2022-03-11 11:24:27',NULL),(29,46,NULL,'5454545','ICIC0001577','HDFC Bank','HDFC','Noida',_binary '',_binary '\0',NULL,NULL,'2022-03-15 02:40:24','2022-03-15 07:44:52',NULL),(30,47,NULL,'121212','ICIC0001577','HDFC Bank','HDFC','Noida',_binary '',_binary '\0',NULL,NULL,'2022-03-15 08:15:36','2022-03-17 12:48:06',NULL),(31,53,NULL,'323232323','ICIC0001577','HDFC Bank','HDFC','Noida',_binary '',_binary '\0',NULL,NULL,'2022-03-18 06:12:59','2022-03-18 06:26:37',NULL),(32,68,NULL,'32323','ICIC0001577','HDFC Bank','HDFC','Noida',_binary '',_binary '\0',NULL,NULL,'2022-03-22 05:11:48','2022-03-22 05:12:30',NULL),(33,82,NULL,'121212121','dsdsds','Abbbbbb','HDFC','Noida',_binary '',_binary '\0',NULL,NULL,'2022-03-28 05:20:04',NULL,'4b9040ed-60f5-4729-86c9-72f43ca8b6ff'),(34,92,NULL,'122333','123333','HDCFC','abhishek','Noida',_binary '',_binary '\0',NULL,NULL,'2022-03-31 01:04:53','2022-04-06 12:13:49',NULL),(35,94,NULL,'234567','HDFC0001','hdfc','hdfc','noida',_binary '',_binary '\0',NULL,NULL,'2022-04-08 06:29:53',NULL,'4f813f68-d7e6-42dd-ba6f-91bda487313e'),(36,95,NULL,'45689787','SBI2324','Abhishek SIngh','SBI','Mandya',_binary '',_binary '\0',NULL,NULL,'2022-04-08 09:28:37',NULL,'28f49798-1be9-44df-ba6c-9e14fb23c2ad'),(37,99,NULL,'12451','iCICI124578','MNAIK','ICICIBAnk','ICICI',_binary '',_binary '\0',NULL,NULL,'2022-04-21 03:43:58',NULL,'ec1aef21-67cf-41de-907a-8cba29e9445a'),(38,101,NULL,'4315810305955019','ICICI000214','Ashish','Test','Lalganj',_binary '',_binary '\0',NULL,NULL,'2022-04-21 04:15:47',NULL,'af43b6a1-7806-4d1f-b9e9-f91caf03040d'),(39,104,NULL,'055101501111','ICIC0000938','Kokila V','icici bank','RR nagar',_binary '',_binary '\0',NULL,NULL,'2022-04-22 09:41:38','2022-04-22 09:52:16',NULL);
/*!40000 ALTER TABLE `tbl_enq_bank_det` ENABLE KEYS */;
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

-- Dump completed on 2022-06-28 19:19:38
