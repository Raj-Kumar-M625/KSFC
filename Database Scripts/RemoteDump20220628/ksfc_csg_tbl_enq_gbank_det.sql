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
-- Table structure for table `tbl_enq_gbank_det`
--

DROP TABLE IF EXISTS `tbl_enq_gbank_det`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_enq_gbank_det` (
  `enq_guarbank_id` int NOT NULL AUTO_INCREMENT,
  `enqtemp_id` int DEFAULT NULL,
  `promoter_code` bigint NOT NULL,
  `guar_acctype` varchar(20) DEFAULT NULL,
  `guar_bankaccno` varchar(50) NOT NULL,
  `guar_ifsc` varchar(11) NOT NULL,
  `guar_acc_name` varchar(100) NOT NULL,
  `guar_bankname` varchar(100) DEFAULT NULL,
  `guar_bankbr` varchar(100) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `modified_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  PRIMARY KEY (`enq_guarbank_id`),
  KEY `fk_tbl_enq_gbank_det_tbl_enq_temptab` (`enqtemp_id`),
  KEY `fk_tbl_enq_gbank_det_tbl_prom_cdtab` (`promoter_code`),
  CONSTRAINT `fk_tbl_enq_gbank_det_tbl_enq_temptab` FOREIGN KEY (`enqtemp_id`) REFERENCES `tbl_enq_temptab` (`enqtemp_id`),
  CONSTRAINT `fk_tbl_enq_gbank_det_tbl_prom_cdtab` FOREIGN KEY (`promoter_code`) REFERENCES `tbl_prom_cdtab` (`promoter_code`)
) ENGINE=InnoDB AUTO_INCREMENT=40 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_enq_gbank_det`
--

LOCK TABLES `tbl_enq_gbank_det` WRITE;
/*!40000 ALTER TABLE `tbl_enq_gbank_det` DISABLE KEYS */;
INSERT INTO `tbl_enq_gbank_det` VALUES (1,7,5,'string','string','string','string','string','string',_binary '',_binary '\0',NULL,NULL,'2022-01-19 08:11:50',NULL,'e7ae68a7-5124-4b67-881e-e40025a2b667'),(3,14,7,'string','string','string','string','string','string',_binary '',_binary '\0',NULL,NULL,'2022-01-27 06:40:31',NULL,'e8959075-ac73-4d5f-a52a-e790096885b2'),(7,64,36,NULL,'43434','ICICI00978','CSSSG','HDFC',NULL,_binary '',_binary '\0',NULL,NULL,'2022-03-21 11:54:31',NULL,'bfef8552-81c8-492a-bd29-57452cc31d93'),(8,65,38,NULL,'232323','ICICI00978','CSSSG','HDFC',NULL,_binary '',_binary '\0',NULL,NULL,'2022-03-21 11:59:05',NULL,'794141c3-0890-4536-8480-3a1be605ac2a'),(9,66,40,NULL,'12121','ICICI00978','CSSSG','HDFC',NULL,_binary '\0',_binary '',NULL,NULL,'2022-03-21 12:27:24','2022-03-21 12:28:22','412b584b-9716-443c-ab4f-8adb1ac2d118'),(11,66,21,NULL,'12121','ICICI00978','CSSSG','HDFC',NULL,_binary '',_binary '\0',NULL,NULL,'2022-03-21 12:36:36',NULL,'85d50db9-1427-478c-8be8-4bd653f0b448'),(12,67,43,NULL,'32323','ICICI00978','CSSSG','HDFC',NULL,_binary '\0',_binary '',NULL,NULL,'2022-03-21 13:44:54','2022-03-21 13:54:12','ec0ea0dd-5a11-411a-a829-46af53a5db14'),(13,67,21,NULL,'32323','ICICI00978','CSSSG','HDFC',NULL,_binary '',_binary '\0',NULL,NULL,'2022-03-21 13:54:12',NULL,'190dd092-754a-4e4c-a2f0-52357c6648c2'),(14,68,45,NULL,'2323','ICICI00978','CSSSG','HDFC',NULL,_binary '\0',_binary '',NULL,NULL,'2022-03-21 14:04:10','2022-03-21 14:04:56','723a1728-19f6-461c-9d4f-da0a22808ba5'),(15,68,46,NULL,'12','ICICI00978','CSSSG','HDFC',NULL,_binary '\0',_binary '',NULL,NULL,'2022-03-21 14:04:10','2022-03-21 14:04:56','23508fa5-146d-4565-8400-79161757c886'),(16,68,21,NULL,'2323','ICICI00978','CSSSG','HDFC',NULL,_binary '\0',_binary '',NULL,'cmeps9748b','2022-03-21 14:04:56','2022-03-22 05:09:56','b8fa3547-f87f-48d1-bf33-00edab36386f'),(17,68,21,NULL,'2323','ICICI00978','CSSSG','HDFC',NULL,_binary '\0',_binary '','cmeps9748b','cmeps9748b','2022-03-22 05:09:56','2022-03-22 05:12:39','baa13215-31a7-4337-b2f7-ac1dde04a25b'),(18,68,21,NULL,'2323','ICICI00978','CSSSG','HDFC',NULL,_binary '\0',_binary '','cmeps9748b','cmeps9748b','2022-03-22 05:12:39','2022-03-23 03:15:56','772644af-e47b-42a7-8607-93de8dbb14d4'),(19,68,21,NULL,'2323','ICICI00978','CSSSG','HDFC',NULL,_binary '',_binary '\0','cmeps9748b',NULL,'2022-03-23 03:15:56',NULL,'d0b77879-5511-4b28-a07d-b288c4140418'),(20,82,48,NULL,'32434','dsdd','dsdsd','Anuj Kumar',NULL,_binary '\0',_binary '','cmeps9748b','CMEPS9748B','2022-03-25 07:37:54','2022-03-29 10:23:43','0bc997a5-e0c8-4763-b194-a40c5116f435'),(21,82,21,NULL,'32434','dsdd','dsdsd','Anuj Kumar',NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-03-29 10:23:43',NULL,'8b158944-ef0d-4de7-9933-5d9d9e5daebc'),(30,86,60,NULL,'343434','ICICICIC','6767','Anuj Kumar',NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-03-30 14:02:25',NULL,'c3b1c877-1c8b-4768-849c-9b1086fa6e18'),(31,91,70,NULL,'5353','ICICICIC','6767','Anuj Kumar',NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-03-30 21:34:30',NULL,'5a924532-ddff-4bb0-a59d-2cfd457fcbaa'),(32,91,71,NULL,'3434','sdsdsds','6767','Anuj Kumar',NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-03-30 21:35:00',NULL,'90c872dd-996f-40c3-8065-33458903bb40'),(33,92,73,NULL,'43434','677767','6767','Anuj Kumar',NULL,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-03-30 23:11:29','2022-03-30 23:12:00','103856da-ddc5-42a6-85ec-a077dfcf08cf'),(34,92,73,NULL,'43434','677767','6767','Anuj Kumar',NULL,_binary '\0',_binary '','CMEPS9748B','CMEPS9748B','2022-03-30 23:21:10','2022-03-30 23:22:18','b38485d3-0cfb-4e8c-a6ff-3ae4baccd680'),(35,92,73,NULL,'43434','677767','6767','Anuj Kumar',NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-03-30 23:22:19',NULL,'58647c84-8889-424e-8b35-c1ef1d29255c'),(36,94,76,NULL,'676767','HDFC001','abhishek','hdfc',NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-04-08 06:32:34',NULL,'2436627b-3182-487e-8fec-ca03c14782ce'),(37,95,79,NULL,'7687433','sbi4324324','suraj kumar','sbi',NULL,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-04-08 10:01:01',NULL,'345ca804-b375-41f1-a81e-2c196a4f2357'),(38,99,88,NULL,'2','dsad2','ici','ici',NULL,_binary '',_binary '\0','AJNPB1985K',NULL,'2022-04-21 06:29:56',NULL,'c1245dc9-7a2b-4223-b001-ac2116f4bdd0'),(39,104,90,NULL,'06620263333','ICIC0000849','Meghana','ICICI BANK',NULL,_binary '',_binary '\0','BADPS0712F',NULL,'2022-04-22 10:57:53',NULL,'df166a8e-059c-440b-966a-61e212004a72');
/*!40000 ALTER TABLE `tbl_enq_gbank_det` ENABLE KEYS */;
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

-- Dump completed on 2022-06-28 19:18:52
