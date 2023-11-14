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
-- Table structure for table `tbl_ind_cdtab`
--

DROP TABLE IF EXISTS `tbl_ind_cdtab`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_ind_cdtab` (
  `ind_cd` int NOT NULL AUTO_INCREMENT,
  `ind_dets` varchar(100) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `modified_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  PRIMARY KEY (`ind_cd`)
) ENGINE=InnoDB AUTO_INCREMENT=39 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_ind_cdtab`
--

LOCK TABLES `tbl_ind_cdtab` WRITE;
/*!40000 ALTER TABLE `tbl_ind_cdtab` DISABLE KEYS */;
INSERT INTO `tbl_ind_cdtab` VALUES (1,'COAL MINING',_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(2,'METAL MINING',_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(3,'CRUDE PETROLIUM & NATURAL GAS',_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(4,'STONE QUARRYING CLAY & SAND',_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(5,'OTHER NON-METALLIC MINING',_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(6,'FOOD MANUFACTURING INDS',_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(7,'BEVERAGE INDUSTRY',_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(8,'TABOCCO',_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(9,'TEXTILES',_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(10,'MFG FOOT WEAR, OTHER TEXTILES',_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(11,'WOOD & CORK',_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(12,'FURNITURE AND FIXTURES',_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(13,'PAPER AND PAPER PRODUCT',_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(14,'PRINTING PUBLISHING  INDUSTRY',_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(15,'LEATHER PRODUCTS',_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(16,'RUBBER AND RUBBER PRODUCT',_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(17,'CHEMICALS AND CHEMICAL PRODUCT',_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(18,'PETROLEUM PRODUCTS',_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(19,'NON-METALLIC MENERAL PRODUCT',_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(20,'BASIC METAL',_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(21,'METAL PRODUCT',_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(22,'MACHINERIES',_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(23,'ELECTRICAL',_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(24,'TRANSPORT EQUIPMENT',_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(25,'ELECTRICITY GAS & STEAM',_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(26,'MOTION  PICTURES',_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(27,'MISCELLANEOUS MFG INDUSTRY',_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(28,'HOTEL/CONSTRUCTION/EDUCATIONAL',_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(29,'TRANSPORT INDUSTRY',_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(30,'HOUSE BOATS',_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(31,'FISHING',_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(32,'INDUSTRIAL ESTATE',_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(33,'CONSULATANCY SERVICES',_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(34,'ELECTRONIC EQUIPMENTS',_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(35,'GENERATOR',_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(36,'COMPUTER AND COMPUTER SERVICES',_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(37,'PLASTIC GOODS',_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL),(38,'OTHER INDUSTRIES',_binary '',_binary '\0','1',NULL,'2022-04-12 00:00:00',NULL,NULL);
/*!40000 ALTER TABLE `tbl_ind_cdtab` ENABLE KEYS */;
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

-- Dump completed on 2022-06-28 19:18:50
