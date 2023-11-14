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
-- Table structure for table `tbl_enq_basic_det`
--

DROP TABLE IF EXISTS `tbl_enq_basic_det`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_enq_basic_det` (
  `enq_bdet_id` int NOT NULL AUTO_INCREMENT,
  `enqtemp_id` int DEFAULT NULL,
  `enq_appl_name` varchar(100) DEFAULT NULL,
  `enq_address` varchar(200) DEFAULT NULL,
  `enq_place` varchar(100) DEFAULT NULL,
  `enq_pincode` int DEFAULT NULL,
  `enq_email` varchar(50) DEFAULT NULL,
  `addl_loan` int DEFAULT NULL,
  `unit_name` varchar(150) DEFAULT NULL,
  `enq_repay_period` int DEFAULT NULL,
  `enq_loanamt` int DEFAULT NULL,
  `const_cd` int DEFAULT NULL,
  `purp_cd` int DEFAULT NULL,
  `size_cd` int NOT NULL,
  `prod_cd` int NOT NULL,
  `vil_cd` int NOT NULL,
  `prem_cd` int DEFAULT NULL,
  `offc_cd` tinyint NOT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `modified_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `ind_cd` int DEFAULT NULL,
  PRIMARY KEY (`enq_bdet_id`),
  KEY `fk_tbl_enq_basic_det_tbl_cnst_cdtab` (`const_cd`),
  KEY `fk_tbl_enq_basic_det_tbl_purp_cdtab` (`purp_cd`),
  KEY `fk_tbl_enq_basic_det_VIL_CDTAB` (`vil_cd`),
  KEY `fk_tbl_enq_basic_det_tbl_prem_cdtab` (`prem_cd`),
  KEY `fk_tbl_enq_basic_det_OFFC_CDTAB` (`offc_cd`),
  KEY `fk_tbl_enq_basic_det_tbl_size_cdtab` (`size_cd`),
  KEY `fk_tbl_enq_basic_det_tbl_enq_temptab_idx` (`enqtemp_id`),
  CONSTRAINT `fk_tbl_enq_basic_det_OFFC_CDTAB` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`),
  CONSTRAINT `fk_tbl_enq_basic_det_tbl_cnst_cdtab` FOREIGN KEY (`const_cd`) REFERENCES `tbl_cnst_cdtab` (`cnst_cd`),
  CONSTRAINT `fk_tbl_enq_basic_det_tbl_enq_temptab` FOREIGN KEY (`enqtemp_id`) REFERENCES `tbl_enq_temptab` (`enqtemp_id`),
  CONSTRAINT `fk_tbl_enq_basic_det_tbl_prem_cdtab` FOREIGN KEY (`prem_cd`) REFERENCES `tbl_prem_cdtab` (`prem_cd`),
  CONSTRAINT `fk_tbl_enq_basic_det_tbl_purp_cdtab` FOREIGN KEY (`purp_cd`) REFERENCES `tbl_purp_cdtab` (`purp_cd`),
  CONSTRAINT `fk_tbl_enq_basic_det_tbl_size_cdtab` FOREIGN KEY (`size_cd`) REFERENCES `tbl_size_cdtab` (`size_cd`),
  CONSTRAINT `fk_tbl_enq_basic_det_VIL_CDTAB` FOREIGN KEY (`vil_cd`) REFERENCES `vil_cdtab` (`VIL_CD`)
) ENGINE=InnoDB AUTO_INCREMENT=47 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_enq_basic_det`
--

LOCK TABLES `tbl_enq_basic_det` WRITE;
/*!40000 ALTER TABLE `tbl_enq_basic_det` DISABLE KEYS */;
INSERT INTO `tbl_enq_basic_det` VALUES (1,16,'string','string','string',0,'string',0,'string',0,0,1,1,1,1,1,1,1,_binary '',_binary '\0',NULL,NULL,'2022-01-28 13:26:54',NULL,'6462de0d-54a7-4e7a-9143-2311865fc110',NULL),(2,17,'Abhishek','test','test',841235,'fdfdfd@gmail.com',NULL,'fdfdfd',60,12000,NULL,1,1,0,1,1,1,_binary '',_binary '\0',NULL,NULL,'2022-02-08 07:22:31',NULL,'61f15c6e-6020-4e43-b4e8-4b0a1528ddab',NULL),(3,18,'Abhishek','test','fdfdfdf',841235,'fdfdfd@gmail.com',NULL,'fdfdfd',60,1200000,NULL,1,1,0,1,1,1,_binary '',_binary '\0',NULL,NULL,'2022-02-08 09:03:58',NULL,'26e4fcdc-b9b6-445e-9836-b1dbde3148ff',NULL),(4,19,'Abhishek','Test','Test',841235,'a@gmail.com',NULL,'Enquiry 1',60,1200000,NULL,1,1,0,1,1,1,_binary '',_binary '\0',NULL,NULL,'2022-02-09 11:21:27',NULL,'16340962-a3c7-42d0-9237-4fd5b1cd3f28',NULL),(5,20,'Abhishek','Test','Test',841235,'fdfdfd@gmail.com',NULL,'Enquiry 2',60,1200000,NULL,1,1,0,1,1,1,_binary '',_binary '\0',NULL,NULL,'2022-02-09 12:40:51',NULL,'cb9035b9-a53d-4fa4-a465-fa99d429b339',NULL),(6,21,'Abhishek','Test Address','Noida',841235,'fdfdfd@gmail.com',NULL,'Test Unit',60,1200000,NULL,1,1,0,1,1,1,_binary '',_binary '\0',NULL,NULL,'2022-02-14 13:29:55',NULL,'de3d3dd0-b8b0-421f-9eb8-7b7244250502',NULL),(7,22,'Abhishek','Test','Test',841235,'a23@gmail.com',NULL,'Test 3 Unit',60,1200000,NULL,1,1,0,1,1,1,_binary '',_binary '\0',NULL,NULL,'2022-02-15 06:10:02',NULL,'2b3d8fd1-7f59-4ab3-ac20-dbcb3688aa70',NULL),(8,23,'Abhishek','Test','Terst',841235,'a@gmail.com',NULL,'Test 3 Unit',60,45454,NULL,1,1,0,1,1,1,_binary '',_binary '\0',NULL,NULL,'2022-02-15 07:24:43',NULL,'344c722f-6d8c-4be0-a394-7b9daf5c62c9',NULL),(10,30,'Anuj Kumar','Bangalore\r\nBangalore','test',560063,'anujkr502@outlook.com',NULL,'Anuj Kumar',232,3232,NULL,1,1,0,1,1,1,_binary '',_binary '\0',NULL,NULL,'2022-03-05 09:58:20',NULL,'ec3aaa22-f4fa-4668-8f41-15a77975968c',NULL),(11,35,'Abhishek','Noida Delhi','noida',201109,'abc@gmail.com',NULL,'Unit test',23,1200000,NULL,1,1,0,1,1,1,_binary '',_binary '\0',NULL,NULL,'2022-03-09 12:46:14',NULL,'38a38c41-1f73-402f-9051-09306c7f0a6f',NULL),(12,35,'Abhishek','Noida Delhi','noida',201109,'abc@gmail.com',NULL,'Unit test',23,1200000,NULL,1,1,0,1,1,1,_binary '',_binary '\0',NULL,NULL,'2022-03-09 12:47:31',NULL,'838c40f0-792e-435b-a015-b5671157c4b3',NULL),(13,35,'Abhishek','test','fdfdfdf',841235,'fdfdfd@gmail.com',NULL,'fdfdfd',60,1200000,NULL,1,1,0,1,1,1,_binary '',_binary '\0',NULL,NULL,'2022-03-09 12:54:55',NULL,'c0e0afeb-a592-4a6c-8935-a1efa7df47f0',NULL),(14,35,'Abhishek','Noida Delhi','noida',201109,'abc@gmail.com',NULL,'Unit test',23,1200000,NULL,1,1,0,1,1,1,_binary '',_binary '\0',NULL,NULL,'2022-03-09 12:57:26',NULL,'07c924b7-7379-483f-a8ea-4006b338a6d1',NULL),(15,35,'Abhishek','dsd','fdfd',841235,'fdfdfd@gmail.com',NULL,'fdfdfd',60,1200000,NULL,1,1,0,1,1,1,_binary '',_binary '\0',NULL,NULL,'2022-03-09 13:00:56',NULL,'94cb2802-9302-42e2-ab94-cd1874e196b3',NULL),(16,35,'Abhishek','dsd','fdfd',841235,'fdfdfd@gmail.com',NULL,'fdfdfd',60,1200000,NULL,1,1,0,1,1,1,_binary '',_binary '\0',NULL,NULL,'2022-03-09 13:01:21',NULL,'52b3365f-7149-42e0-aafc-743b2463e0dc',NULL),(17,35,'Abhishek','dsd','fdfd',841235,'fdfdfd@gmail.com',NULL,'fdfdfd',60,1200000,NULL,1,1,0,1,1,1,_binary '',_binary '\0',NULL,NULL,'2022-03-09 13:01:44',NULL,'9a0d1dd0-a9b7-4154-9b41-a9ad5ad73700',NULL),(18,35,'Abhishek','dsd','fdfd',841235,'fdfdfd@gmail.com',NULL,'fdfdfd',60,1200000,NULL,1,1,0,1,1,1,_binary '',_binary '\0',NULL,NULL,'2022-03-09 13:02:25',NULL,'2bf6c5b5-4134-4f24-833f-5c5b40639857',NULL),(19,35,'Abhishek','dsd','fdfd',841235,'fdfdfd@gmail.com',NULL,'fdfdfd',60,1200000,NULL,1,1,0,1,1,1,_binary '',_binary '\0',NULL,NULL,'2022-03-09 13:02:27',NULL,'749a2d53-8e6f-48de-90a6-9b95342df545',NULL),(20,35,'Abhishek','dsd','fdfd',841235,'fdfdfd@gmail.com',NULL,'fdfdfd',60,1200000,NULL,1,1,0,1,1,1,_binary '',_binary '\0',NULL,NULL,'2022-03-09 13:02:31',NULL,'02eb6464-5357-42d8-bf0f-9836b3825220',NULL),(21,35,'Abhishek','dsd','fdfd',841235,'fdfdfd@gmail.com',NULL,'fdfdfd',60,1200000,NULL,1,1,0,1,1,1,_binary '',_binary '\0',NULL,NULL,'2022-03-09 13:02:42',NULL,'9ee1c937-b60d-40fa-ba26-af9962c5a9e8',NULL),(22,35,'Abhishek kumar singh','dsd','fdfd',841235,'fdfdfd@gmail.com',NULL,'fdfdfd',60,1200000,NULL,1,1,0,1,1,1,_binary '',_binary '\0',NULL,NULL,'2022-03-09 13:02:53',NULL,'59e55828-bd61-4286-9ae2-94269e3ca36f',NULL),(23,36,'Abhishek','Abhisehk','Noida',841235,'a@gmail.com',NULL,'fdfdfd',60,1200000,NULL,1,1,0,1,1,1,_binary '',_binary '\0',NULL,NULL,'2022-03-09 13:11:52',NULL,'811bdf84-70e4-4297-8b16-a4ad32503192',NULL),(24,37,'Abhishek','dsd','dsd',222222,'abcd@gmail.com',NULL,'Noida',4343,343,NULL,1,1,0,1,1,1,_binary '',_binary '\0',NULL,NULL,'2022-03-10 08:03:11',NULL,'0703c1c5-96ca-4d3a-ae57-9402b57d2a82',NULL),(25,38,'Abhishek Singh','Delhi','Noida',201009,'abcd@gmail.com',NULL,'Noida',323232,12222,NULL,1,1,0,1,1,1,_binary '',_binary '\0',NULL,NULL,'2022-03-10 08:11:44','2022-03-10 08:13:00','f10554d6-d30f-4e48-9707-b6c71252bc8a',NULL),(26,39,'Sonal Singh','sasa','sas',3434,'abcd@gmail.com',NULL,'Noida',434,3434,NULL,1,1,0,1,1,1,_binary '',_binary '\0',NULL,NULL,'2022-03-10 08:33:22','2022-03-10 08:34:42','0b8549b3-59d4-4da9-a36f-8cb576e670f2',NULL),(27,40,'Sonal Singh','Test','Noida',3434,'abcd@gmail.com',NULL,'Noida',343,3434,NULL,1,1,0,1,1,1,_binary '',_binary '\0',NULL,NULL,'2022-03-10 08:41:52','2022-03-10 08:43:49','d1763cd7-0d43-40b9-a3a9-5085cb052103',NULL),(28,41,'Sonal Singh','Address','Noida',3434,'abcd@gmail.com',NULL,'Noida',212,21212,NULL,1,1,0,1,1,1,_binary '',_binary '\0',NULL,NULL,'2022-03-10 08:53:25','2022-03-10 08:53:46','d0724db9-5542-4475-adfb-b1df3da2426a',NULL),(29,42,'Sonal ','Test','Noida',4343,'abcd@gmail.com',NULL,'Noida',34,23,NULL,1,1,0,1,1,1,_binary '',_binary '\0',NULL,NULL,'2022-03-10 09:03:21','2022-03-10 09:05:48','cbea997a-8561-49e2-a2bb-73e5528230b9',NULL),(30,43,'Kreetika','Noida','Noida',841235,'fdfdfd@gmail.com',NULL,'fdfdfd',60,2323,NULL,1,1,0,1,1,1,_binary '',_binary '\0',NULL,NULL,'2022-03-10 09:18:02','2022-03-10 09:18:32','17bc77b3-5be2-43a3-b39b-9f8619d95773',NULL),(31,44,'Abhishek kumar singh','test','Noida',841235,'fdfdfd@gmail.com',NULL,'Test 3 Unit',60,1200000,NULL,1,1,0,1,1,1,_binary '',_binary '\0',NULL,NULL,'2022-03-10 09:41:14','2022-03-10 09:43:15','4d8c0cd8-89c5-4a8c-b8fa-fac81805dce7',NULL),(32,45,'ABhishek','Noida-Address','Noida',841235,'a@gmail.com',NULL,'Test 3 Unit',60,1200000,NULL,1,1,0,1,1,1,_binary '',_binary '\0',NULL,NULL,'2022-03-11 06:56:31','2022-03-11 11:23:42','3ff68d43-6ee8-42e5-8f1e-470943b6518a',NULL),(33,46,'Abhishek singh','test','Noida',841235,'a@gmail.com',NULL,'Test 3 Unit',60,1200000,NULL,1,1,0,4,1,1,_binary '',_binary '\0',NULL,NULL,'2022-03-15 02:40:24','2022-03-15 07:44:38','e3cfe124-d2ba-4ed6-92a9-17a500f5f8fe',NULL),(34,47,'Srinivas Singh','Delhi','Noida',841235,'a23@gmail.com',NULL,'Test 3 Unit',60,1200000,NULL,1,1,0,1,1,1,_binary '',_binary '\0',NULL,NULL,'2022-03-15 08:15:32','2022-03-17 12:48:06','faf88519-e337-4d50-83e0-661792c2a1d8',NULL),(35,53,'Abhishek singh','Delhi karol bag','Noida',841235,'abc@gmail.com',NULL,'Test 3 Unit',60,1200000,NULL,1,1,0,1,1,1,_binary '',_binary '\0',NULL,NULL,'2022-03-18 06:12:58','2022-03-18 06:26:37','8b59279e-22d4-4905-9b7f-15e478ac1078',NULL),(36,68,'Abhishek kumar singh','test','Noida',841235,'fdfdfd@gmail.com',NULL,'fdfdfd',60,1200000,NULL,1,1,0,1,1,1,_binary '',_binary '\0',NULL,NULL,'2022-03-22 05:11:47','2022-03-22 05:12:30','c590f0f2-4e99-4eb1-9c93-17190b849279',NULL),(37,82,'Abhishek Singh','Test','Noida',560063,'anujkr502@outlook.com',NULL,'test',60,1200000,NULL,1,1,0,1,1,1,_binary '',_binary '\0',NULL,NULL,'2022-03-28 05:20:03',NULL,'62c2b436-1adf-4f45-8cb3-a5ffe05de739',NULL),(38,92,'Abhishek singh','test','noida',841235,'anujkr502@outlook.com',NULL,'Anuj Kumar',2323,60000,NULL,1,1,0,1,1,1,_binary '',_binary '\0',NULL,NULL,'2022-03-31 00:57:54','2022-04-06 12:13:30','ee3d9b1d-afd0-480d-a1b4-e74a603862b5',NULL),(39,94,'Abhishek Singh','Test Address','Noida',821235,'abc@gmail.com',NULL,'test unit',60,200000,NULL,1,1,0,1,1,1,_binary '',_binary '\0',NULL,NULL,'2022-04-08 06:29:53',NULL,'841a5c90-a7ce-414b-8f01-b7b546e4496d',NULL),(40,95,'Abhishek','JP Nagar, ','Mandya',560040,'abhishek.singh@gmail.com',NULL,'JP Enterprises',48,23,NULL,1,1,0,1,1,1,_binary '',_binary '\0',NULL,NULL,'2022-04-08 09:28:35',NULL,'5a3023f2-87c8-4fac-b30d-21d4833d7d3c',NULL),(41,99,'Manik','Test','Test',500050,'test@gmail.com',NULL,'Prop',12,12,NULL,2,1,0,1,1,12,_binary '',_binary '\0','AJNPB1985K',NULL,'2022-04-21 03:43:57',NULL,'a4aeca4d-167b-4549-af2f-ebffe412380b',6),(42,101,'Ashish','Lucknow','Lalganj',229206,'test@gmail.com',NULL,'Lalganj',44,33,NULL,1,1,0,1,1,1,_binary '',_binary '\0','FTGPK9863P',NULL,'2022-04-21 04:15:46',NULL,'8a148335-162e-43b5-989f-13becc693872',9),(43,104,'V Kokila','1st  Floor, TTMC, BMTC Building\r\nYestwantpur','Bengaluru',560022,'v.kokila@gmail.com',NULL,'Kokila Enterprises',60,50,NULL,1,19,0,1,1,13,_binary '',_binary '\0','BADPS0712F','BADPS0712F','2022-04-22 09:41:37','2022-04-22 09:52:15','b5d7d6b3-3881-46bc-b24a-3776ba0ac16c',6),(44,110,'Abhishek','test',NULL,581102,'abc@gamil.com',NULL,'Abhishek',12,12,NULL,2,2,600000,3,NULL,26,_binary '',_binary '\0','CMEPS9748B',NULL,'2022-06-01 12:56:22',NULL,'44d02fa6-9489-4d7d-b1d4-e47e21421d1a',6),(45,111,'latha','nagar',NULL,572103,'latha789@gmail.com',NULL,'abc',9,15,NULL,1,2,400000,28,NULL,50,_binary '',_binary '\0','OOSPS1480G',NULL,'2022-06-02 11:16:47',NULL,'146692f3-6200-46cd-8336-c0067905e2cb',4),(46,112,'shivakumar','wedcfrvgtbnhjm',NULL,582211,'sfdgfkjdh@gamil.com',NULL,'Neelavrana enterprises',24,55,NULL,1,1,701040,35,NULL,72,_binary '',_binary '\0','FMHPM5207B',NULL,'2022-06-04 10:07:14',NULL,'b738ee82-b9df-4863-8356-6d22bf3f72e8',7);
/*!40000 ALTER TABLE `tbl_enq_basic_det` ENABLE KEYS */;
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

-- Dump completed on 2022-06-28 19:18:27
