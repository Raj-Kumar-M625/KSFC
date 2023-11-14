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
-- Table structure for table `tbl_enq_document`
--

DROP TABLE IF EXISTS `tbl_enq_document`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_enq_document` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `EnquiryId` int NOT NULL,
  `Name` varchar(150) DEFAULT NULL,
  `FilePath` varchar(200) DEFAULT NULL,
  `Description` varchar(450) DEFAULT NULL,
  `Process` varchar(45) DEFAULT NULL,
  `DocSection` varchar(45) DEFAULT NULL,
  `FileType` varchar(45) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(45) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `modified_by` varchar(45) DEFAULT NULL,
  `unique_id` varchar(150) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `id_UNIQUE` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=32 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_enq_document`
--

LOCK TABLES `tbl_enq_document` WRITE;
/*!40000 ALTER TABLE `tbl_enq_document` DISABLE KEYS */;
INSERT INTO `tbl_enq_document` VALUES (1,1,'1_GD1_17Jan2022','D:\\CSG\\LiveKsfc\\API\\Documents\\KAR.KSFC.API\\GeneralDocuments\\1_GD1_17Jan2022.Pdf','General Document 1','EnquirySubmission','GeneralDocument1','Pdf',_binary '',_binary '\0',NULL,'2022-01-17 10:00:39',NULL,NULL,'3058b7de-ed8c-45ca-910f-1c7e60805455'),(2,0,'0_GD1_17Feb2022_38_28.Pdf','D:\\CSG\\LiveKsfc\\API\\KAR.KSFC.API\\Documents\\GeneralDocuments\\0_GD1_17Feb2022_38_28.Pdf','GeneralDocument1','EnquirySubmission','GeneralDocument1','Pdf',_binary '',_binary '\0',NULL,'2022-02-17 05:08:29',NULL,NULL,'178480e9-955b-41e9-8b55-15a338e974d2'),(3,0,'0_GD2_17Feb2022_00_46.Pdf','D:\\CSG\\LiveKsfc\\API\\KAR.KSFC.API\\Documents\\GeneralDocuments\\0_GD2_17Feb2022_00_46.Pdf','GeneralDocument2','EnquirySubmission','GeneralDocument2','Pdf',_binary '',_binary '\0',NULL,'2022-02-17 06:30:46',NULL,NULL,'d4b5b510-6c61-46f1-8558-5c4bb81a0ddd'),(4,82,'82_TD2_28Mar2022_25_55.Pdf','D:\\CSG\\KSFC\\DOCUMENTS\\TechnicalDocuments\\82_TD2_28Mar2022_25_55.Pdf','TechnicalDocument2','EnquirySubmission','TechnicalDocument2','Pdf',_binary '',_binary '\0',NULL,'2022-03-28 13:55:57',NULL,NULL,'2d2d21f6-3636-47d9-bd98-ee374df8fb2f'),(5,82,'82_TD1_28Mar2022_26_36.Pdf','D:\\CSG\\KSFC\\DOCUMENTS\\TechnicalDocuments\\82_TD1_28Mar2022_26_36.Pdf','TechnicalDocument1','EnquirySubmission','TechnicalDocument1','Pdf',_binary '',_binary '\0',NULL,'2022-03-28 13:56:37',NULL,NULL,'865db399-0fea-4369-b44b-fcaf1b93aaf3'),(6,82,'82_GD1_29Mar2022_26_25.Pdf','D:\\CSG\\KSFC\\DOCUMENTS\\GeneralDocuments\\82_GD1_29Mar2022_26_25.Pdf','GeneralDocument1','EnquirySubmission','GeneralDocument1','Pdf',_binary '',_binary '\0',NULL,'2022-03-29 09:56:25',NULL,NULL,'7498c547-3e19-4bd5-94d9-435726d25d0c'),(7,82,'82_GD2_29Mar2022_26_32.Pdf','D:\\CSG\\KSFC\\DOCUMENTS\\GeneralDocuments\\82_GD2_29Mar2022_26_32.Pdf','GeneralDocument2','EnquirySubmission','GeneralDocument2','Pdf',_binary '',_binary '\0',NULL,'2022-03-29 09:56:32',NULL,NULL,'1f7aabf4-38c9-418d-bc6e-558f1c03097b'),(8,82,'82_FD1_29Mar2022_27_07.Pdf','D:\\CSG\\KSFC\\DOCUMENTS\\FinancialDocuments\\82_FD1_29Mar2022_27_07.Pdf','FinancialDocument1','EnquirySubmission','FinancialDocument1','Pdf',_binary '',_binary '\0',NULL,'2022-03-29 09:57:08',NULL,NULL,'e501b7a1-8d8e-4aa0-b6a2-b844602f8899'),(9,82,'82_FD2_29Mar2022_27_09.Pdf','D:\\CSG\\KSFC\\DOCUMENTS\\FinancialDocuments\\82_FD2_29Mar2022_27_09.Pdf','FinancialDocument2','EnquirySubmission','FinancialDocument2','Pdf',_binary '',_binary '\0',NULL,'2022-03-29 09:57:10',NULL,NULL,'92aee83a-be7b-48c7-9414-94ec76ced0e9'),(10,82,'82_LD1_29Mar2022_27_20.Pdf','D:\\CSG\\KSFC\\DOCUMENTS\\LegalDocuments\\82_LD1_29Mar2022_27_20.Pdf','LegalDocument1','EnquirySubmission','LegalDocument1','Pdf',_binary '',_binary '\0',NULL,'2022-03-29 09:57:20',NULL,NULL,'b7b569c7-083c-46ea-ae18-3ed8a52bfdfd'),(11,82,'82_LD2_29Mar2022_27_22.Pdf','D:\\CSG\\KSFC\\DOCUMENTS\\LegalDocuments\\82_LD2_29Mar2022_27_22.Pdf','LegalDocument2','EnquirySubmission','LegalDocument2','Pdf',_binary '',_binary '\0',NULL,'2022-03-29 09:57:22',NULL,NULL,'06f4e657-5190-48fc-8c17-0d210d9c774e'),(12,94,'94_GD1_08Apr2022_26_21.Pdf','E:\\CSG\\KSFC\\DOCUMENTS\\GeneralDocuments\\94_GD1_08Apr2022_26_21.Pdf','GeneralDocument1','EnquirySubmission','GeneralDocument1','Pdf',_binary '',_binary '\0',NULL,'2022-04-08 06:56:21',NULL,NULL,'c9ff36e7-40b3-4f06-a054-1f7a7169dcee'),(13,94,'94_GD2_08Apr2022_26_33.Pdf','E:\\CSG\\KSFC\\DOCUMENTS\\GeneralDocuments\\94_GD2_08Apr2022_26_33.Pdf','GeneralDocument2','EnquirySubmission','GeneralDocument2','Pdf',_binary '',_binary '\0',NULL,'2022-04-08 06:56:33',NULL,NULL,'86ad8afa-8428-4151-814c-9377525a5321'),(14,94,'94_TD1_08Apr2022_26_46.Pdf','E:\\CSG\\KSFC\\DOCUMENTS\\TechnicalDocuments\\94_TD1_08Apr2022_26_46.Pdf','TechnicalDocument1','EnquirySubmission','TechnicalDocument1','Pdf',_binary '',_binary '\0',NULL,'2022-04-08 06:56:46',NULL,NULL,'76698f9b-0ba6-44d9-a469-a16beed82fcc'),(15,94,'94_TD2_08Apr2022_26_55.Pdf','E:\\CSG\\KSFC\\DOCUMENTS\\TechnicalDocuments\\94_TD2_08Apr2022_26_55.Pdf','TechnicalDocument2','EnquirySubmission','TechnicalDocument2','Pdf',_binary '',_binary '\0',NULL,'2022-04-08 06:56:56',NULL,NULL,'42ac93d8-404f-491b-999f-47bde3250ae4'),(16,94,'94_FD1_08Apr2022_27_06.Pdf','E:\\CSG\\KSFC\\DOCUMENTS\\FinancialDocuments\\94_FD1_08Apr2022_27_06.Pdf','FinancialDocument1','EnquirySubmission','FinancialDocument1','Pdf',_binary '',_binary '\0',NULL,'2022-04-08 06:57:07',NULL,NULL,'a50377bd-052e-4cfe-bc66-d16e8ee2e276'),(17,94,'94_FD2_08Apr2022_27_14.Pdf','E:\\CSG\\KSFC\\DOCUMENTS\\FinancialDocuments\\94_FD2_08Apr2022_27_14.Pdf','FinancialDocument2','EnquirySubmission','FinancialDocument2','Pdf',_binary '',_binary '\0',NULL,'2022-04-08 06:57:14',NULL,NULL,'585f3964-0176-4331-af28-a9a4eaf1a62d'),(18,94,'94_LD1_08Apr2022_28_25.Pdf','E:\\CSG\\KSFC\\DOCUMENTS\\LegalDocuments\\94_LD1_08Apr2022_28_25.Pdf','LegalDocument1','EnquirySubmission','LegalDocument1','Pdf',_binary '',_binary '\0',NULL,'2022-04-08 06:58:25',NULL,NULL,'6c4878b5-d9fd-442e-9da3-4d963751ecbc'),(19,94,'94_LD2_08Apr2022_28_34.Pdf','E:\\CSG\\KSFC\\DOCUMENTS\\LegalDocuments\\94_LD2_08Apr2022_28_34.Pdf','LegalDocument2','EnquirySubmission','LegalDocument2','Pdf',_binary '',_binary '\0',NULL,'2022-04-08 06:58:35',NULL,NULL,'a565ac4d-7fc4-4549-80cd-580bdb5d54e8'),(20,95,'95_GD1_08Apr2022_45_45.Pdf','E:\\CSG\\KSFC\\DOCUMENTS\\GeneralDocuments\\95_GD1_08Apr2022_45_45.Pdf','GeneralDocument1','EnquirySubmission','GeneralDocument1','Pdf',_binary '',_binary '\0',NULL,'2022-04-08 10:15:46',NULL,NULL,'3207da7b-2d39-4a55-9072-e7121d6331ff'),(21,101,'101_GD1_21Apr2022_45_58.Pdf','E:\\CSG\\KSFC\\DOCUMENTS\\GeneralDocuments\\101_GD1_21Apr2022_45_58.Pdf','GeneralDocument1','EnquirySubmission','GeneralDocument1','Pdf',_binary '\0',_binary '',NULL,'2022-04-21 04:15:59','2022-04-21 04:16:21',NULL,'51734153-d889-41f4-a3e3-007761fe4103'),(22,101,'101_GD1_21Apr2022_46_50.Pdf','E:\\CSG\\KSFC\\DOCUMENTS\\GeneralDocuments\\101_GD1_21Apr2022_46_50.Pdf','GeneralDocument1','EnquirySubmission','GeneralDocument1','Pdf',_binary '',_binary '\0',NULL,'2022-04-21 04:16:50',NULL,NULL,'6182a51b-8d73-4e9f-91e2-ed2193919387'),(23,99,'99_GD1_21Apr2022_21_01.Pdf','E:\\CSG\\KSFC\\DOCUMENTS\\GeneralDocuments\\99_GD1_21Apr2022_21_01.Pdf','GeneralDocument1','EnquirySubmission','GeneralDocument1','Pdf',_binary '',_binary '\0',NULL,'2022-04-21 04:51:01',NULL,NULL,'1394a5e7-9d5c-40ea-85b3-288644d8e556'),(24,99,'99_GD2_21Apr2022_21_05.Pdf','E:\\CSG\\KSFC\\DOCUMENTS\\GeneralDocuments\\99_GD2_21Apr2022_21_05.Pdf','GeneralDocument2','EnquirySubmission','GeneralDocument2','Pdf',_binary '',_binary '\0',NULL,'2022-04-21 04:51:06',NULL,NULL,'09507182-bbd1-4527-9579-d939057bc2b0'),(25,99,'99_TD1_21Apr2022_21_20.Pdf','E:\\CSG\\KSFC\\DOCUMENTS\\TechnicalDocuments\\99_TD1_21Apr2022_21_20.Pdf','TechnicalDocument1','EnquirySubmission','TechnicalDocument1','Pdf',_binary '',_binary '\0',NULL,'2022-04-21 04:51:20',NULL,NULL,'6e39c554-4951-4a44-a09d-55131599d345'),(26,99,'99_TD2_21Apr2022_21_30.Pdf','E:\\CSG\\KSFC\\DOCUMENTS\\TechnicalDocuments\\99_TD2_21Apr2022_21_30.Pdf','TechnicalDocument2','EnquirySubmission','TechnicalDocument2','Pdf',_binary '',_binary '\0',NULL,'2022-04-21 04:51:31',NULL,NULL,'e2f7d722-91d0-47fe-af7d-967136bd7d40'),(27,99,'99_FD1_21Apr2022_22_21.Pdf','E:\\CSG\\KSFC\\DOCUMENTS\\FinancialDocuments\\99_FD1_21Apr2022_22_21.Pdf','FinancialDocument1','EnquirySubmission','FinancialDocument1','Pdf',_binary '',_binary '\0',NULL,'2022-04-21 04:52:22',NULL,NULL,'378092db-a82a-45a9-bb94-8c7c03b76776'),(28,99,'99_FD2_21Apr2022_22_28.Pdf','E:\\CSG\\KSFC\\DOCUMENTS\\FinancialDocuments\\99_FD2_21Apr2022_22_28.Pdf','FinancialDocument2','EnquirySubmission','FinancialDocument2','Pdf',_binary '',_binary '\0',NULL,'2022-04-21 04:52:28',NULL,NULL,'1ceb0a9c-471a-4ebb-96af-0303911800c1'),(29,99,'99_LD1_21Apr2022_23_04.Pdf','E:\\CSG\\KSFC\\DOCUMENTS\\LegalDocuments\\99_LD1_21Apr2022_23_04.Pdf','LegalDocument1','EnquirySubmission','LegalDocument1','Pdf',_binary '',_binary '\0',NULL,'2022-04-21 04:53:05',NULL,NULL,'3c1324e1-14ff-4f42-8222-e55f01584d9d'),(30,99,'99_LD2_21Apr2022_23_12.Pdf','E:\\CSG\\KSFC\\DOCUMENTS\\LegalDocuments\\99_LD2_21Apr2022_23_12.Pdf','LegalDocument2','EnquirySubmission','LegalDocument2','Pdf',_binary '',_binary '\0',NULL,'2022-04-21 04:53:12',NULL,NULL,'6d449a6d-451d-42d0-93e7-7daaad758098'),(31,102,'102_GD1_21Apr2022_37_16.Pdf','E:\\CSG\\KSFC\\DOCUMENTS\\GeneralDocuments\\102_GD1_21Apr2022_37_16.Pdf','GeneralDocument1','EnquirySubmission','GeneralDocument1','Pdf',_binary '\0',_binary '',NULL,'2022-04-21 08:07:16','2022-04-21 08:07:23',NULL,'72d748d9-62ea-4fcc-8a1d-2ebc3c6ae49c');
/*!40000 ALTER TABLE `tbl_enq_document` ENABLE KEYS */;
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

-- Dump completed on 2022-06-28 19:17:46
