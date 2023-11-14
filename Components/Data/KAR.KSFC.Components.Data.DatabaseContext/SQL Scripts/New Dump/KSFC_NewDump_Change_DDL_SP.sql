USE `ksfc_qa`;
DROP procedure IF EXISTS `KSFC_NewDump_change_DDL_SP`;

DELIMITER $$
USE `ksfc_qa`$$
CREATE PROCEDURE `KSFC_NewDump_change_DDL_SP` ()
BEGIN
-- -----------------------Removing Foreign Key Constraints ----------------------------------
						
Begin
DECLARE DBName varchar(50) DEFAULT "ksfc_qa";

DECLARE exit handler for sqlexception
BEGIN
    ROLLBACK;
END;
DECLARE exit handler for sqlwarning
BEGIN
    ROLLBACK;
END;
START TRANSACTION;
ALTER TABLE tbl_app_team_det 
DROP FOREIGN KEY  FK_tbl_app_team_det_tbl_trg_employee;

ALTER TABLE tbl_empchair_det 
DROP FOREIGN KEY  fk_tbl_empchair_det_tbl_trg_employee;

ALTER TABLE tbl_empchairhist_det 
DROP FOREIGN KEY  fk_tbl_empchairhist_det_tbl_trg_employee;

ALTER TABLE tbl_empdesig_tab 
DROP FOREIGN KEY  fk_tbl_empdesig_tab_tbl_trg_employee;

ALTER TABLE tbl_empdesighist_tab 
DROP FOREIGN KEY  fk_tbl_empdesighist_tab_tbl_trg_employee;

ALTER TABLE tbl_empdsc_tab 
DROP FOREIGN KEY  fk_tbl_empdsc_tab_tbl_trg_employee;

ALTER TABLE tbl_emplogin_tab 
DROP FOREIGN KEY  fk_tbl_emplogin_tab_tbl_trg_employee;

ALTER TABLE tbl_enq_ack_det 
DROP FOREIGN KEY  FK_tbl_enq_ack_det_tbl_trg_employee;

ALTER TABLE tbl_enq_workflow_det 
DROP FOREIGN KEY  FK_tbl_enq_workflow_det_tbl_trg_employee;

ALTER TABLE tbl_enqscr_legal_det 
DROP FOREIGN KEY  FK_tbl_enqscr_legal_det_tbl_trg_employee;

ALTER TABLE tbl_enqscr_overall_legal 
DROP FOREIGN KEY  FK_tbl_enqscr_overall_legal_tbl_trg_employee;

ALTER TABLE tbl_idm_cersai_registration 
DROP FOREIGN KEY  fk_tbl_idm_cersai_registration_tbl_trg_employee;

ALTER TABLE tbl_idm_deed_det 
DROP FOREIGN KEY  fk_tbl_idm_deed_det_tbl_trg_employee;

ALTER TABLE tbl_idm_dsb_charge 
DROP FOREIGN KEY  fk_tbl_idm_dsb_charge_tbl_trg_employee;

ALTER TABLE tbl_idm_dsb_cleg 
DROP FOREIGN KEY  fk_tbl_idm_dsb_cleg_confirm_tbl_idm_dsb_spl_cleg;

ALTER TABLE tbl_idm_dsb_spl_cleg 
DROP FOREIGN KEY  fk_tbl_idm_dsb_spl_cleg_tbl_trg_employee;

ALTER TABLE tbl_idm_guar_deed_det 
DROP FOREIGN KEY  fk_tbl_idm_guar_deed_det_tbl_idm_dsb_spl_cleg;

ALTER TABLE tbl_idm_hypoth_det 
DROP FOREIGN KEY  fk_tbl_idm_hypoth_det_tbl_idm_dsb_spl_cleg;

ALTER TABLE tbl_idm_legal_chklist_confirm 
DROP FOREIGN KEY  fk_tbl_idm_legal_chklist_confirm_tbl_idm_dsb_spl_cleg;

ALTER TABLE tbl_idm_legal_workflow 
DROP FOREIGN KEY  fk_tbl_idm_legal_workflow_tbl_idm_dsb_spl_cleg,
DROP FOREIGN KEY  fk_tbl_idm_legal_workflow_tbl_idm_dsb_spl_cleg_1;
END;


ALTER TABLE tbl_dop_cdtab 
DROP FOREIGN KEY  fk_tbl_dop_cdtab_tbl_trg_emp_grade;

ALTER TABLE tbl_dophist_det 
DROP FOREIGN KEY  fk_tbl_dophist_det_tbl_trg_emp_grade;

ALTER TABLE tbl_empchair_det 
DROP FOREIGN KEY  fk_tbl_empchair_det_tbl_trg_emp_grade;

ALTER TABLE tbl_empchairhist_det 
DROP FOREIGN KEY  fk_tbl_empchairhist_det_tbl_trg_emp_grade;

ALTER TABLE tbl_empdesig_tab 
DROP FOREIGN KEY  fk_tbl_empdesig_tab_tbl_trg_emp_grade_1,
DROP FOREIGN KEY  fk_tbl_empdesig_tab_tbl_trg_emp_grade_2,
DROP FOREIGN KEY  fk_tbl_empdesig_tab_tbl_trg_emp_grade_3;


ALTER TABLE tbl_empdesighist_tab 
DROP FOREIGN KEY  fk_tbl_empdesighist_tab_tbl_trg_emp_grade;

Begin

--
-- Table structure for table `tbl_trg_employee`
--

DROP TABLE IF EXISTS `tbl_trg_employee`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_trg_employee` (
			  `tey_unit_code` varchar(10) DEFAULT NULL,
			  `tey_ticket_num` varchar(8) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
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
			  `created_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
			  `modified_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
			  `created_date` datetime DEFAULT NULL,
			  `modified_date` datetime DEFAULT NULL,
			  `unique_id` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
			  PRIMARY KEY (`tey_ticket_num`),
			  UNIQUE KEY `uk_emp_mobile_no` (`emp_mobile_no`)
			)ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

END;
Begin
--
-- Table structure for table `tbl_trg_emp_grade`
--
DROP TABLE IF EXISTS `tbl_trg_emp_grade`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_trg_emp_grade` (
			  `tges_code` varchar(5) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
			  `tges_desc` varchar(30) NOT NULL,
			  `tegs_order` decimal(3,1) NOT NULL,
			  `is_active` bit(1) DEFAULT NULL,
			  `is_deleted` bit(1) DEFAULT NULL,
			  `created_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
			  `modified_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
			  `created_date` datetime DEFAULT NULL,
			  `modified_date` datetime DEFAULT NULL,
			  `unique_id` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
			  PRIMARY KEY (`tges_code`)
			)ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

/*!40101 SET character_set_client = @saved_cs_client */;

END;

Begin
--
-- Table structure for table `tbl_unit_mast`
--
DROP TABLE IF EXISTS `tbl_unit_mast`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_unit_mast` (
  `ut_rowid` int NOT NULL AUTO_INCREMENT,
  `ut_cd` int NOT NULL,
  `ut_name` varchar(100) DEFAULT NULL,
  `ut_off_cd` tinyint NOT NULL,
  `ut_ut_pan` varchar(10) DEFAULT NULL,
  `ut_from_date` varchar(200) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) DEFAULT NULL,
  `modified_by` varchar(50) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  PRIMARY KEY (`ut_rowid`),
  UNIQUE KEY `uq_tbl_unit_mast` (`ut_cd`),
  UNIQUE KEY `ut_cd` (`ut_cd`),
  KEY `fk_tbl_unit_mast_offc_cdtab` (`ut_off_cd`),
  CONSTRAINT `fk_tbl_unit_mast_offc_cdtab` FOREIGN KEY (`ut_off_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

/*!40101 SET character_set_client = @saved_cs_client */;

END;
BEGIN
truncate table tbl_app_team_det;
truncate table tbl_empchair_det;
truncate table tbl_empchairhist_det;
truncate table tbl_empdesig_tab;
truncate table tbl_empdesighist_tab;
truncate table tbl_empdsc_tab;
truncate table tbl_emplogin_tab;
truncate table tbl_enq_ack_det;
truncate table tbl_enq_workflow_det;
truncate table tbl_enqscr_legal_det;
truncate table tbl_enqscr_overall_legal;
truncate table tbl_idm_cersai_registration;
truncate table tbl_app_guarnator;
truncate table tbl_app_proj_cost;
truncate table tbl_app_proj_mf;
truncate table tbl_app_promoter;
truncate table tbl_app_promoter_bank_details;
END;
Begin
ALTER TABLE tbl_app_team_det 
ADD CONSTRAINT FK_tbl_app_team_det_tbl_trg_employee
  FOREIGN KEY (emp_id)
  REFERENCES tbl_trg_employee (tey_ticket_num);
  
 ALTER TABLE tbl_empchair_det 
ADD CONSTRAINT fk_tbl_empchair_det_tbl_trg_employee
  FOREIGN KEY (emp_id)
  REFERENCES tbl_trg_employee (tey_ticket_num);
  
  
  ALTER TABLE tbl_empchairhist_det 
ADD CONSTRAINT fk_tbl_empchairhist_det_tbl_trg_employee
  FOREIGN KEY (emp_id)
  REFERENCES tbl_trg_employee (tey_ticket_num);
  
  
  ALTER TABLE tbl_empdesig_tab 
ADD CONSTRAINT fk_tbl_empdesig_tab_tbl_trg_employee
  FOREIGN KEY (emp_id)
  REFERENCES tbl_trg_employee (tey_ticket_num);
  
  
  ALTER TABLE tbl_empdesighist_tab 
ADD CONSTRAINT fk_tbl_empdesighist_tab_tbl_trg_employee
  FOREIGN KEY (emp_id)
  REFERENCES tbl_trg_employee (tey_ticket_num);
  
  
  ALTER TABLE tbl_empdsc_tab 
ADD CONSTRAINT fk_tbl_empdsc_tab_tbl_trg_employee
  FOREIGN KEY (emp_id)
  REFERENCES tbl_trg_employee (tey_ticket_num);
  
  
    ALTER TABLE tbl_emplogin_tab 
ADD CONSTRAINT fk_tbl_emplogin_tab_tbl_trg_employee
  FOREIGN KEY (emp_id)
  REFERENCES tbl_trg_employee (tey_ticket_num);
  
  
    ALTER TABLE tbl_enq_ack_det 
ADD CONSTRAINT FK_tbl_enq_ack_det_tbl_trg_employee
  FOREIGN KEY (emp_id)
  REFERENCES tbl_trg_employee (tey_ticket_num);
  
  
    ALTER TABLE tbl_enq_workflow_det 
ADD CONSTRAINT FK_tbl_enq_workflow_det_tbl_trg_employee
  FOREIGN KEY (emp_id)
  REFERENCES tbl_trg_employee (tey_ticket_num);
  
  
    ALTER TABLE tbl_enqscr_legal_det 
ADD CONSTRAINT FK_tbl_enqscr_legal_det_tbl_trg_employee
  FOREIGN KEY (emp_id)
  REFERENCES tbl_trg_employee (tey_ticket_num);
  
  
    ALTER TABLE tbl_enqscr_overall_legal 
ADD CONSTRAINT FK_tbl_enqscr_overall_legal_tbl_trg_employee
  FOREIGN KEY (emp_id)
  REFERENCES tbl_trg_employee (tey_ticket_num);
  
  
    ALTER TABLE tbl_idm_cersai_registration 
ADD CONSTRAINT fk_tbl_idm_cersai_registration_tbl_trg_employee
  FOREIGN KEY (approved_emp_id)
  REFERENCES tbl_trg_employee (tey_ticket_num);
  
  
    ALTER TABLE tbl_idm_deed_det 
ADD CONSTRAINT fk_tbl_idm_deed_det_tbl_trg_employee
  FOREIGN KEY (approved_emp_id)
  REFERENCES tbl_trg_employee (tey_ticket_num);
  
  
    ALTER TABLE tbl_idm_dsb_charge 
ADD CONSTRAINT fk_tbl_idm_dsb_charge_tbl_trg_employee
  FOREIGN KEY (approved_emp_id)
  REFERENCES tbl_trg_employee (tey_ticket_num);
  
  
      ALTER TABLE tbl_idm_dsb_cleg 
ADD CONSTRAINT fk_tbl_idm_dsb_cleg_confirm_tbl_idm_dsb_spl_cleg
  FOREIGN KEY (approved_emp_id)
  REFERENCES tbl_trg_employee (tey_ticket_num);
  
  
      ALTER TABLE tbl_idm_dsb_spl_cleg 
ADD CONSTRAINT fk_tbl_idm_dsb_spl_cleg_tbl_trg_employee
  FOREIGN KEY (approved_emp_id)
  REFERENCES tbl_trg_employee (tey_ticket_num);
  
  
      ALTER TABLE tbl_idm_guar_deed_det 
ADD CONSTRAINT fk_tbl_idm_guar_deed_det_tbl_idm_dsb_spl_cleg
  FOREIGN KEY (approved_emp_id)
  REFERENCES tbl_trg_employee (tey_ticket_num);
  
  
      ALTER TABLE tbl_idm_hypoth_det 
ADD CONSTRAINT fk_tbl_idm_hypoth_det_tbl_idm_dsb_spl_cleg
  FOREIGN KEY (approved_emp_id)
  REFERENCES tbl_trg_employee (tey_ticket_num);
  
  
      ALTER TABLE tbl_idm_legal_chklist_confirm 
ADD CONSTRAINT fk_tbl_idm_legal_chklist_confirm_tbl_idm_dsb_spl_cleg
  FOREIGN KEY (emp_id)
  REFERENCES tbl_trg_employee (tey_ticket_num);
  
  
      ALTER TABLE tbl_idm_legal_workflow 
ADD CONSTRAINT fk_tbl_idm_legal_workflow_tbl_idm_dsb_spl_cleg
  FOREIGN KEY (emp_id_from)
  REFERENCES tbl_trg_employee (tey_ticket_num);
  
  
        ALTER TABLE tbl_idm_legal_workflow 
ADD CONSTRAINT fk_tbl_idm_legal_workflow_tbl_idm_dsb_spl_cleg_1
  FOREIGN KEY (emp_id_to)
  REFERENCES tbl_trg_employee (tey_ticket_num);
  
  
  
  ALTER TABLE tbl_dop_cdtab 
ADD CONSTRAINT fk_tbl_dop_cdtab_tbl_trg_emp_grade
  FOREIGN KEY (tges_code)
  REFERENCES tbl_trg_emp_grade (tges_code);
  
  ALTER TABLE tbl_dophist_det 
ADD CONSTRAINT fk_tbl_dophist_det_tbl_trg_emp_grade
  FOREIGN KEY (tges_code)
  REFERENCES tbl_trg_emp_grade (tges_code);
  
  
  ALTER TABLE tbl_empchair_det 
ADD CONSTRAINT fk_tbl_empchair_det_tbl_trg_emp_grade
  FOREIGN KEY (tges_code)
  REFERENCES tbl_trg_emp_grade (tges_code);
  
  
  ALTER TABLE tbl_empchairhist_det 
ADD CONSTRAINT fk_tbl_empchairhist_det_tbl_trg_emp_grade
  FOREIGN KEY (tges_code)
  REFERENCES tbl_trg_emp_grade (tges_code);
  
  
  ALTER TABLE tbl_empdesig_tab 
ADD CONSTRAINT fk_tbl_empdesig_tab_tbl_trg_emp_grade_1
  FOREIGN KEY (subst_desig_code)
  REFERENCES tbl_trg_emp_grade (tges_code);
  
  ALTER TABLE tbl_empdesig_tab 
ADD CONSTRAINT fk_tbl_empdesig_tab_tbl_trg_emp_grade_2
  FOREIGN KEY (pp_design_code)
  REFERENCES tbl_trg_emp_grade (tges_code);
  
    ALTER TABLE tbl_empdesig_tab 
ADD CONSTRAINT fk_tbl_empdesig_tab_tbl_trg_emp_grade_3
  FOREIGN KEY (ic_desig_code)
  REFERENCES tbl_trg_emp_grade (tges_code);
COMMIT;
END;
END$$

DELIMITER ;
call KSFC_NewDump_change_DDL_SP()
