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
-- Table structure for table `tbl_trg_employee`
--

DROP TABLE IF EXISTS `tbl_trg_employee`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_trg_employee` (
  `tey_unit_code` varchar(10) DEFAULT NULL,
  `tey_ticket_num` varchar(8) NOT NULL,
  `tey_staftype_code` varchar(8) DEFAULT NULL,
  `tey_grade_code` varchar(5) DEFAULT NULL,
  `tey_name` varchar(60) NOT NULL,
  `tey_sex` varchar(1) NOT NULL,
  `tey_mode_of_pay` varchar(1) DEFAULT NULL,
  `tey_dept_code` varchar(5) DEFAULT NULL,
  `tey_alias_name` varchar(30) DEFAULT NULL,
  `tey_delete_status` varchar(2) DEFAULT NULL,
  `tey_join_date` date DEFAULT NULL,
  `tey_emp_type` varchar(2) DEFAULT NULL,
  `tey_work_area` varchar(5) DEFAULT NULL,
  `tey_pan_num` varchar(30) DEFAULT NULL,
  `tey_pf_num` varchar(30) DEFAULT NULL,
  `tey_separation_type` varchar(5) DEFAULT NULL,
  `tey_lastdate_increment` date DEFAULT NULL,
  `tey_lastdate_promotion` date DEFAULT NULL,
  `tey_separation_date` date DEFAULT NULL,
  `tey_father_husband_name` varchar(60) DEFAULT NULL,
  `tey_birth_date` date DEFAULT NULL,
  `tey_blood_group` varchar(3) DEFAULT NULL,
  `tey_marital_status` varchar(1) DEFAULT NULL,
  `tey_eye_sight` varchar(1) DEFAULT NULL,
  `tey_colour_blindness` varchar(1) DEFAULT NULL,
  `tey_whether_handicap` varchar(1) DEFAULT NULL,
  `tey_present_address1` varchar(60) DEFAULT NULL,
  `tey_present_address2` varchar(60) DEFAULT NULL,
  `tey_present_city` varchar(30) DEFAULT NULL,
  `tey_present_state` varchar(30) DEFAULT NULL,
  `tey_present_zip` varchar(30) DEFAULT NULL,
  `tey_permanent_address1` varchar(60) DEFAULT NULL,
  `tey_permanent_address2` varchar(60) DEFAULT NULL,
  `tey_permanent_city` varchar(30) DEFAULT NULL,
  `tey_permanent_state` varchar(30) DEFAULT NULL,
  `tey_permanent_zip` varchar(30) DEFAULT NULL,
  `tey_footware_size` int DEFAULT NULL,
  `tey_next_increment_date` date DEFAULT NULL,
  `tey_vpf_percent` decimal(5,2) DEFAULT NULL,
  `tey_spouse_name` varchar(60) DEFAULT NULL,
  `tey_present_phone` int DEFAULT NULL,
  `tey_present_email` varchar(30) DEFAULT NULL,
  `tey_permanent_phone` int DEFAULT NULL,
  `tey_permanent_email` varchar(30) DEFAULT NULL,
  `tey_pay_status` varchar(1) DEFAULT NULL,
  `tey_employee_status` varchar(1) DEFAULT NULL,
  `tey_super_user` varchar(1) DEFAULT NULL,
  `tey_entry_basic` decimal(14,2) DEFAULT NULL,
  `tey_current_basic` decimal(14,2) DEFAULT NULL,
  `tey_nationality` varchar(30) DEFAULT NULL,
  `tey_religion_code` varchar(5) DEFAULT NULL,
  `tey_caste_code` varchar(5) DEFAULT NULL,
  `tey_category_code` varchar(5) DEFAULT NULL,
  `tey_current_unit` varchar(10) DEFAULT NULL,
  `tey_home_state` varchar(5) DEFAULT NULL,
  `tey_home_city` varchar(30) DEFAULT NULL,
  `tey_mother_tongue` varchar(5) DEFAULT NULL,
  `tey_ajoin_date` date DEFAULT NULL,
  `tey_separation_ref` date DEFAULT NULL,
  `tey_confirm_date` date DEFAULT NULL,
  `emp_mobile_no` bigint DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `modified_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  PRIMARY KEY (`tey_ticket_num`),
  UNIQUE KEY `uk_emp_mobile_no` (`emp_mobile_no`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_trg_employee`
--

LOCK TABLES `tbl_trg_employee` WRITE;
/*!40000 ALTER TABLE `tbl_trg_employee` DISABLE KEYS */;
INSERT INTO `tbl_trg_employee` VALUES ('0002','00000001',NULL,NULL,'Abhishek','M',NULL,NULL,NULL,NULL,NULL,NULL,NULL,'FTGPK9863y',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'softashis45@gamail.com',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'1',NULL,NULL,NULL,NULL,NULL,NULL,9120276336,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),('0003','0000001',NULL,NULL,'Abhishek','M',NULL,NULL,NULL,NULL,NULL,NULL,NULL,'FTGPK9863g',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'softashis43@gamail.com',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'1',NULL,NULL,NULL,NULL,NULL,NULL,9120276334,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),('0001','0001',NULL,NULL,'Ashish','M',NULL,NULL,NULL,NULL,NULL,NULL,NULL,'FTGPK9863P',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'softashish3@gamail.com',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'1',NULL,NULL,NULL,NULL,NULL,NULL,9120276335,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `tbl_trg_employee` ENABLE KEYS */;
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

-- Dump completed on 2022-06-28 19:20:24
