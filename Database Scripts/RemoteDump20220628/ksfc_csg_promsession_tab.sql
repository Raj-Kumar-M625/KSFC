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
-- Table structure for table `promsession_tab`
--

DROP TABLE IF EXISTS `promsession_tab`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `promsession_tab` (
  `PROMSESSION_ID` int NOT NULL AUTO_INCREMENT,
  `PROM_PAN` varchar(10) DEFAULT NULL COMMENT 'Employee ID',
  `LOGIN_DATE_TIME` datetime(6) DEFAULT NULL COMMENT 'Date and Time of Login',
  `LOGOUT_DATE_TIME` datetime(6) DEFAULT NULL COMMENT 'Date and Time of Logout',
  `IPADRESS` varchar(200) DEFAULT NULL COMMENT 'To track device ip	',
  `ACCESSTOKEN` varchar(2000) DEFAULT NULL,
  `REFRESHTOKEN` varchar(2000) DEFAULT NULL,
  `ACCESSTOKENREVOKE` tinyint(1) DEFAULT NULL,
  `REFRESHTOKENREVOKE` tinyint(1) DEFAULT NULL,
  `ACCESSTOKENEXPIRYDATETIME` datetime(6) DEFAULT NULL,
  `REFRESHTOKENEXPIRYDATETIME` datetime(6) DEFAULT NULL,
  `SESSION_STATUS` tinyint(1) DEFAULT NULL COMMENT 'Status of Session',
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `modified_by` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  PRIMARY KEY (`PROMSESSION_ID`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='Promoter Session Details';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `promsession_tab`
--

LOCK TABLES `promsession_tab` WRITE;
/*!40000 ALTER TABLE `promsession_tab` DISABLE KEYS */;
INSERT INTO `promsession_tab` VALUES (1,'CMEPS9748B','2022-06-01 18:53:41.575424','2022-05-06 13:11:56.723877','::1','eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwYW4iOiJDTUVQUzk3NDhCIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQ3VzdG9tZXIiLCJJcEFkZHJlc3MiOiI6OjEiLCJleHAiOjE2NTQwOTA3MjEsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjQ0Mzc5IiwiYXVkIjoiVXNlciJ9.6RIsN-u4wnYa26ggaO6CVtG5s32dkAebRbYzAXAItzQ','Mu22+h7P6xOUItiSTShEiQwpwphRYR6zHt1wzD05Cl4=',0,0,'2022-06-01 19:08:41.575424','2022-06-01 19:53:41.575438',1,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(2,'cmeps9767c','2022-03-18 11:33:54.608653',NULL,'127.0.0.1','eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJQYW4gTnVtYmVyIjoiY21lcHM5NzY3YyIsIlJvbGUiOiJDdXN0b21lciIsIklwQWRkcmVzcyI6IjEyNy4wLjAuMSIsImV4cCI6MTY0NzU4NDMzNCwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNzkiLCJhdWQiOiJVc2VyIn0.TUXf968ml-tVQiqjmO2LDGQNwHLzo91zem0ZkuavBtM','r50+a4+5O4u0pobZ4x6FGp5Gw9Q1sgfVOUy0gmKrd08=',0,0,'2022-03-18 11:48:54.608679','2022-03-18 12:33:54.608711',1,NULL,_binary '\0',NULL,NULL,NULL,NULL,NULL),(3,'cmeps9749b','2022-03-25 10:36:39.672704','2022-03-25 10:40:07.207548','::1','eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwYW4iOiJjbWVwczk3NDliIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQ3VzdG9tZXIiLCJJcEFkZHJlc3MiOiI6OjEiLCJleHAiOjE2NDgxODU2OTksImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjQ0Mzc5IiwiYXVkIjoiVXNlciJ9.gcNUybwMj9qhJOuDC-ZeSJ6ToSv1Ca7U_pYlcn7b45M','mXN2ONGvIEP7JwgXG9Nba5EfNjrHFdVnf+lCNrbtCm0=',0,0,'2022-03-25 10:51:39.672704','2022-03-25 11:36:39.672718',0,NULL,_binary '\0',NULL,NULL,NULL,NULL,NULL),(4,'cmeps9748q','2022-04-04 14:59:07.964680','2022-04-04 14:56:06.020138','127.0.0.1','eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwYW4iOiJDTUVQUzk3NDhRIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQ3VzdG9tZXIiLCJJcEFkZHJlc3MiOiIxMjcuMC4wLjEiLCJleHAiOjE2NDkwNjU0NDcsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjQ0Mzc5IiwiYXVkIjoiVXNlciJ9.L-7vE_GxUVXfYFKyJLEnurnDuDx0y-6IW8rRYbe3KPE','tShYchfbdU+iMSgU9WbDNQUDUPMQUuTEEblHbiSTneU=',0,0,'2022-04-04 15:14:07.964680','2022-04-04 15:59:07.964686',1,NULL,_binary '\0',NULL,NULL,NULL,NULL,NULL),(5,'AJNPB1985K','2022-04-21 15:31:39.836561',NULL,'138.91.108.27','eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwYW4iOiJBSk5QQjE5ODVLIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQ3VzdG9tZXIiLCJJcEFkZHJlc3MiOiIxMzguOTEuMTA4LjI3IiwiZXhwIjoxNjUwNTM2MTk5LCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDM3OSIsImF1ZCI6IlVzZXIifQ.5slr839k3ICXDWa71bYBSkqOxs-i0sNQhOw4eVzu-2E','vT3KxlWIr66SEk2G66wTnsltUQ0XvB3tCI11gSAOnFo=',0,0,'2022-04-21 15:46:39.836711','2022-04-21 16:31:39.836873',1,NULL,NULL,NULL,NULL,NULL,NULL,NULL),(6,'FTGPK9863P','2022-06-02 11:14:05.600504','2022-06-02 11:14:40.578574','10.96.158.118','eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwYW4iOiJGVEdQSzk4NjNQIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQ3VzdG9tZXIiLCJJcEFkZHJlc3MiOiIxMC45Ni4xNTguMTE4IiwiZXhwIjoxNjU0MTQ5NTQ1LCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDM3OSIsImF1ZCI6IlVzZXIifQ.rWHL7z7f0pOq_upeONCZaPqmmDbKoMfk5tNtDgPjIAc','gQt3pYAVXzjulow/KGSl5cQsStWN5rAj9mvTQBNqX8w=',0,0,'2022-06-02 11:29:05.600919','2022-06-02 12:14:05.601076',0,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(7,'BADPS0712F','2022-04-25 15:00:47.099182','2022-04-22 15:37:41.750201','49.204.72.2','eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwYW4iOiJCQURQUzA3MTJGIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQ3VzdG9tZXIiLCJJcEFkZHJlc3MiOiI0OS4yMDQuNzIuMiIsImV4cCI6MTY1MDg3OTk0NiwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNzkiLCJhdWQiOiJVc2VyIn0.cxPIp0yEhh2kOaWaBgfwjfct43I8z93kQG3yAGJ2tVI','ypVbTtYnNGaRbWcoEj59lYOS2LUdZZuOtn4iAIXZXrQ=',0,0,'2022-04-25 15:15:47.099306','2022-04-25 16:00:47.099440',1,NULL,NULL,NULL,NULL,NULL,NULL,NULL),(8,'AWPPM8980Q','2022-04-22 18:38:16.019576','2022-04-22 18:39:47.063412','117.216.129.30','eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwYW4iOiJBV1BQTTg5ODBRIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQ3VzdG9tZXIiLCJJcEFkZHJlc3MiOiIxMTcuMjE2LjEyOS4zMCIsImV4cCI6MTY1MDYzMzc5NSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNzkiLCJhdWQiOiJVc2VyIn0.wGWfF-vTD_aWTCOyXgGxjeOpqWqKHfBIbFtwkP4KH8M','i8Ph+PxajnHT8b6WqlwvTsdrNEcy2ekSaUssw4MnuhQ=',0,0,'2022-04-22 18:53:16.019722','2022-04-22 19:38:16.019883',0,NULL,NULL,NULL,NULL,NULL,NULL,NULL),(9,'ACSPV4007E','2022-04-25 13:05:36.136317',NULL,'164.100.133.226','eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwYW4iOiJBQ1NQVjQwMDdFIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQ3VzdG9tZXIiLCJJcEFkZHJlc3MiOiIxNjQuMTAwLjEzMy4yMjYiLCJleHAiOjE2NTA4NzMwMzYsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjQ0Mzc5IiwiYXVkIjpbIlVzZXIiLCJVc2VyIl19.w_iUlg1Ox0cbs3DJFGfCcWHig7NBmhP6G70KQSNVEzg','f9tBQt7qDFBK76lrRP4VxYmFYmzemHiCQhneEj0ynb0=',0,0,'2022-04-25 13:20:36.136318','2022-04-25 14:05:36.136329',1,NULL,NULL,NULL,NULL,NULL,NULL,NULL),(10,'AELPM6415Q','2022-04-25 15:49:48.672869',NULL,'164.100.133.226','eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwYW4iOiJBRUxQTTY0MTVRIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQ3VzdG9tZXIiLCJJcEFkZHJlc3MiOiIxNjQuMTAwLjEzMy4yMjYiLCJleHAiOjE2NTA4ODI4ODgsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjQ0Mzc5IiwiYXVkIjpbIlVzZXIiLCJVc2VyIl19.ZZ8AbIt6ydb93-ZfQmNmCJhQZykjKljP5oTpWGPBXsQ','Mb2T2vIQOkf9/PwPE/jWgalig/T4CNkHc9dRF70L1k0=',0,0,'2022-04-25 16:04:48.672869','2022-04-25 16:49:48.672881',1,NULL,NULL,NULL,NULL,NULL,NULL,NULL),(11,'BICPT9577J','2022-05-05 15:19:55.980755',NULL,'10.96.158.118','eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwYW4iOiJCSUNQVDk1NzdKIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQ3VzdG9tZXIiLCJJcEFkZHJlc3MiOiIxMC45Ni4xNTguMTE4IiwiZXhwIjoxNjUxNzQ1MDk1LCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDM3OSIsImF1ZCI6IlVzZXIifQ.TNPrjIuQl3MihvSnse6TXPZSm71V6IoOuFWHyaJfpGY','BJ0NFWT2l6J8E9J3fFJzIjytvx0RaEHavuw/4dB1vQc=',0,0,'2022-05-05 15:34:55.980901','2022-05-05 16:19:55.981065',1,NULL,NULL,NULL,NULL,NULL,NULL,NULL),(12,'CMEPS9748W','2022-05-06 13:13:19.632817',NULL,'10.96.158.118','eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwYW4iOiJDTUVQUzk3NDhXIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQ3VzdG9tZXIiLCJJcEFkZHJlc3MiOiIxMC45Ni4xNTguMTE4IiwiZXhwIjoxNjUxODIzODk5LCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDM3OSIsImF1ZCI6IlVzZXIifQ.lYxXJDGubHN1RcRA93d_X81IZfiIIa2qfpSfsGRnWSg','cTulaGBr8lphooZq098jZAsgFbGvyf3m3I9aOi0DOT4=',0,0,'2022-05-06 13:28:19.632817','2022-05-06 14:13:19.632830',1,NULL,NULL,NULL,NULL,NULL,NULL,NULL),(13,'CMEPS9748L','2022-05-06 14:17:17.739988','2022-05-06 14:18:52.903376','10.96.158.118','eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwYW4iOiJDTUVQUzk3NDhMIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQ3VzdG9tZXIiLCJJcEFkZHJlc3MiOiIxMC45Ni4xNTguMTE4IiwiZXhwIjoxNjUxODI3NzM3LCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDM3OSIsImF1ZCI6IlVzZXIifQ.rO-u7l6741Iped8amaE588XrPHOY9JSkUruAvru4mYs','Cci/rNE5WXfM5eYrEzp1MdSyHFJql/go8ayFEffwutE=',0,0,'2022-05-06 14:32:17.739988','2022-05-06 15:17:17.739996',0,NULL,NULL,NULL,NULL,NULL,NULL,NULL),(14,'KXMPS4353S','2022-05-06 17:58:40.348448','2022-05-06 17:58:55.302115','10.96.158.118','eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwYW4iOiJLWE1QUzQzNTNTIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQ3VzdG9tZXIiLCJJcEFkZHJlc3MiOiIxMC45Ni4xNTguMTE4IiwiZXhwIjoxNjUxODQxMDIwLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDM3OSIsImF1ZCI6IlVzZXIifQ.HR0NTf96ud464pBkrioD5Xzx1yBDaCZj24r1jktIZt4','crTHVR0AK4KBf6TwImMN0Dywtb40uq9RzWfHsdScYpc=',0,0,'2022-05-06 18:13:40.348568','2022-05-06 18:58:40.348725',0,NULL,NULL,NULL,NULL,NULL,NULL,NULL),(15,'CMEPS9748I','2022-05-09 16:49:52.836195','2022-05-09 15:38:06.415564','172.23.175.205','eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwYW4iOiJDTUVQUzk3NDhJIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQ3VzdG9tZXIiLCJJcEFkZHJlc3MiOiIxNzIuMjMuMTc1LjIwNSIsImV4cCI6MTY1MjA5NjA5MiwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNzkiLCJhdWQiOiJVc2VyIn0.qIUw618pOq9phqQeIyloJ40hTuJROjUc-7ixnoLuTDA','Q3MVRvZa7boi5/hUyobTXut+yp454ji3tTQlcLccCVI=',0,0,'2022-05-09 17:04:52.836343','2022-05-09 17:49:52.836500',1,NULL,NULL,NULL,NULL,NULL,NULL,NULL),(16,'KEMPS1431E','2022-05-12 12:21:59.512901',NULL,'172.23.175.205','eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwYW4iOiJLRU1QUzE0MzFFIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQ3VzdG9tZXIiLCJJcEFkZHJlc3MiOiIxNzIuMjMuMTc1LjIwNSIsImV4cCI6MTY1MjMzOTIxOSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNzkiLCJhdWQiOiJVc2VyIn0.1P2FS9tu2ufZhHU_wnKPKjSN8rw6GYPS_1rf18vgkUM','CJEy8PnQ8aAhhPajwCTWl0psQMBDrfbnO1hO5NR/VR0=',0,0,'2022-05-12 12:36:59.513051','2022-05-12 13:21:59.513214',1,NULL,NULL,NULL,NULL,NULL,NULL,NULL),(17,'KERPS1432M','2022-05-13 12:02:23.441865','2022-05-13 12:04:14.382010','10.96.158.118','eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwYW4iOiJLRVJQUzE0MzJNIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQ3VzdG9tZXIiLCJJcEFkZHJlc3MiOiIxMC45Ni4xNTguMTE4IiwiZXhwIjoxNjUyNDI0NDQzLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDM3OSIsImF1ZCI6IlVzZXIifQ.YBZdDKBOcbuWiN5wW3wwhll_VqVQE5maFxhWN1wrqtw','3lUsuJkIl1Q17Co2di/xCcFcYFyLcfGZZy8Yv6JAUaU=',0,0,'2022-05-13 12:17:23.442010','2022-05-13 13:02:23.442169',0,NULL,NULL,NULL,NULL,NULL,NULL,NULL),(18,'KXSPS1431E','2022-05-27 11:21:56.748465','2022-05-24 18:43:52.423786','10.96.158.118','eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwYW4iOiJLWFNQUzE0MzFFIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQ3VzdG9tZXIiLCJJcEFkZHJlc3MiOiIxMC45Ni4xNTguMTE4IiwiZXhwIjoxNjUzNjMxNjE2LCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDM3OSIsImF1ZCI6IlVzZXIifQ.U5lbsH1UxE6kMHVcBAw-K_TIcyBL9mJdXvvY18R-JlI','fZ7aHcJt6cauHj4VsD5ypUugfh3N2YhosPHBIHgD6w4=',0,0,'2022-05-27 11:36:56.748884','2022-05-27 12:21:56.749042',1,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(19,'OOSPS1480G','2022-06-06 11:31:31.336044','2022-06-04 18:17:52.035022','10.96.158.118','eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwYW4iOiJPT1NQUzE0ODBHIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQ3VzdG9tZXIiLCJJcEFkZHJlc3MiOiIxMC45Ni4xNTguMTE4IiwiZXhwIjoxNjU0NDk2MTkxLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDM3OSIsImF1ZCI6IlVzZXIifQ.EdrmrX6MwfV5YQE5w3JnHY6_cww4P6CBnJAhMF7K6eo','jpC1BKrEepPlfnM44at8bJ0mRgtb/u81ov3aAM2Z51E=',0,0,'2022-06-06 11:46:31.336395','2022-06-06 12:31:31.336540',1,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(20,'FMHPM5207B','2022-06-04 15:51:03.085280',NULL,'10.96.158.118','eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwYW4iOiJGTUhQTTUyMDdCIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQ3VzdG9tZXIiLCJJcEFkZHJlc3MiOiIxMC45Ni4xNTguMTE4IiwiZXhwIjoxNjU0MzM4OTYzLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDM3OSIsImF1ZCI6WyJVc2VyIiwiVXNlciJdfQ.IoyBHpnWdr8ipjSvg3kKnmBimjUrTUGiTMfqf8ZEuGM','1uObAHxrt/05udyYHdcP3KdInqrfqRAIcnMAn3hC19E=',0,0,'2022-06-04 16:06:03.085281','2022-06-04 16:51:03.085293',1,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `promsession_tab` ENABLE KEYS */;
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

-- Dump completed on 2022-06-28 19:19:59
