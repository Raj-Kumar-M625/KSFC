-- -----------------------------Deployement Script--------------------------------------------------------
USE `ksfc_oct`;
DROP procedure IF EXISTS `ChangeOfUnit_SP`;

DELIMITER $$
USE `ksfc_oct`$$
CREATE PROCEDURE `ChangeOfUnit_SP` ()
BEGIN
DECLARE DBName varchar(50) DEFAULT "ksfc_oct";
-- -----------------------Change Location Details Tab -------------------- Added by Gagana--------------
						-- tbl_idm_unit_address --
Begin
IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_unit_address')

		-- If not exists, creat a new table
THEN
select 'tbl_idm_unit_address Table Already Exist' as ' ';
begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_unit_address' AND column_name='dist_cd')
						THEN
						ALTER TABLE `tbl_idm_unit_address`
							Rename Column    `dist_cd` TO `ut_dist_cd` ;
								  select 'Column lut_dist_cd in tbl_idm_unit_address  Modified' as ' ';
						ELSE
							
						select 'Column cond_det_id in tbl_idm_cond_det  Created' as ' ';
				end if;
            end;
			begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_unit_address' AND column_name='tlq_cd')
						THEN
						ALTER TABLE `tbl_idm_unit_address`
							Rename Column    `tlq_cd` TO `ut_tlq_cd` ;
								  select 'Column tlq_cd in tbl_idm_unit_address  Modified' as ' ';
						ELSE
							
						select 'Column tlq_cd in tbl_idm_unit_address  already exist' as ' ';
				end if;
            end;
			begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_unit_address' AND column_name='hob_cd')
						THEN
						ALTER TABLE `tbl_idm_unit_address`
							Rename Column    `hob_cd` TO `ut_hob_cd` ;
								  select 'Column ut_hob_cd in tbl_idm_unit_address  Modified' as ' ';
						ELSE
							
						select 'Column ut_hob_cd in tbl_idm_unit_address  already exist' as ' ';
				end if;
            end;
			begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_unit_address' AND column_name='vil_cd')
						THEN
						ALTER TABLE `tbl_idm_unit_address`
							Rename Column    `vil_cd` TO `ut_vil_cd` ;
								  select 'Column vil_cd in tbl_idm_unit_address  Modified' as ' ';
						ELSE
							
						select 'Column vil_cd in tbl_idm_unit_address  already exist' as ' ';
				end if;
            end;
			Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_unit_address' AND column_name='loan_acc')
						THEN
                        ALTER TABLE tbl_idm_unit_address
							MODIFY COLUMN  loan_acc  bigint NOT NULL;
								  select 'Column loan_acc in tbl_idm_unit_address table Modified' as ' ';
						ELSE
							
						select 'Column loan_acc in tbl_idm_unit_address table Created' as ' ';
				end if;
            end;

ELSE
CREATE TABLE `tbl_idm_unit_address`(
  `idm_utaddress_rowid` int NOT NULL AUTO_INCREMENT,
   `loan_acc` bigint DEFAULT NULL,
   `loan_sub` int DEFAULT NULL,
   `offc_cd` tinyint DEFAULT NULL,
  `ut_cd` int NOT NULL,
  `addtype_cd` int NOT NULL,
  `ut_address` varchar(200) DEFAULT NULL,
  `ut_area` varchar(100) DEFAULT NULL,
  `ut_city` varchar(100) DEFAULT NULL,
  `ut_pincode` int DEFAULT NULL,
  `ut_telephone` int DEFAULT NULL,
  `ut_mobile` int DEFAULT NULL,
  `ut_alt_mobile` int DEFAULT NULL,
  `ut_email` varchar(50) DEFAULT NULL,
  `ut_alt_email` varchar(50) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) DEFAULT NULL,
  `modified_by` varchar(50) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `ut_dist_cd` tinyint DEFAULT NULL,
  `ut_tlq_cd` int DEFAULT NULL,
  `ut_hob_cd` int DEFAULT NULL,
  `ut_vil_cd` int DEFAULT NULL,
  PRIMARY KEY (`idm_utaddress_rowid`),
  KEY `FK_tbl_idm_unit_address_tbl_app_unit_details` (`ut_cd`),
  KEY `FK_tbl_idm_unit_address_tbl_address_cdtab` (`addtype_cd`),
  KEY `fk_tbl_idm_unit_address_dist_cdtab` (`ut_dist_cd`),
  KEY `fk_tbl_idm_unit_address_tlq_cdtab` (`ut_tlq_cd`),
  KEY `fk_tbl_idm_unit_address_hob_cdtab` (`ut_hob_cd`),
  KEY `fk_tbl_idm_unit_address_vil_cdtab` (`ut_vil_cd`),
   KEY `fk_tbl_idm_unit_address_offc_cdtab` (`offc_cd`),
  CONSTRAINT `fk_tbl_idm_unit_address_dist_cdtab` FOREIGN KEY (`ut_dist_cd`) REFERENCES `dist_cdtab` (`DIST_CD`),
  CONSTRAINT `fk_tbl_idm_unit_address_hob_cdtab` FOREIGN KEY (`ut_hob_cd`) REFERENCES `hob_cdtab` (`HOB_CD`),
  CONSTRAINT `fK_tbl_idm_unit_address_tbl_address_cdtab` FOREIGN KEY (`addtype_cd`) REFERENCES `tbl_address_cdtab` (`addtype_cd`),
  CONSTRAINT `fk_tbl_idm_unit_address_tlq_cdtab` FOREIGN KEY (`ut_tlq_cd`) REFERENCES `tlq_cdtab` (`TLQ_CD`),
  CONSTRAINT `fk_tbl_idm_unit_address_vil_cdtab` FOREIGN KEY (`ut_vil_cd`) REFERENCES `vil_cdtab` (`VIL_CD`),
CONSTRAINT `fk_tbl_idm_unit_address_offc_cdtab` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`),
  CONSTRAINT `fk_tbl_idm_unit_address_tbl_unit_mast` FOREIGN KEY (`ut_cd`) REFERENCES `tbl_unit_mast` (`ut_cd`),
CONSTRAINT `fk_tbl_idm_unit_address_tbl_app_loan_mast` FOREIGN KEY (`loan_acc`) REFERENCES `tbl_app_loan_mast` (`ln_no`)
);
select 'tbl_idm_unit_address Table Created' as ' ';
END IF;
END;

    -- --------------------Asset details-------------------

    -- ----------------------tbl_idm_proj_land----------------------
	BEgin
IF EXISTS(SELECT table_name 
						FROM INFORMATION_SCHEMA.TABLES
					   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_proj_land')

		-- If exists, retreive columns information from that table
		THEN		 	
           select 'tbl_idm_proj_land Table Already Exist' as '';                     
            else
            
             begin
CREATE TABLE `tbl_idm_proj_land` (
  `pjlnd_rowid` bigint NOT NULL AUTO_INCREMENT,
  `ut_cd` int DEFAULT NULL,
  `pjlnd_offc` tinyint DEFAULT NULL,
  `pjlnd_unit` int DEFAULT NULL,
  `pjlnd_sno` int DEFAULT NULL,
  `pjlnd_area` decimal(20,2) DEFAULT NULL,
  `pjlnd_areain` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `pjlnd_type` int DEFAULT NULL,
  `pjlnd_cost` decimal(20,2) DEFAULT NULL,
  `pjlnd_subarea` decimal(20,2) DEFAULT NULL,
  `pjlnd_areaunit` int DEFAULT NULL,
  `pjlnd_subareaunit` int DEFAULT NULL,
  `pjlnd_land` int DEFAULT NULL,
  `ut_slno` int DEFAULT NULL,
  `ln_acc` int DEFAULT NULL,
  `ln_sub` int DEFAULT NULL,
  `pjlnd_site_no` varchar(50)  DEFAULT NULL,
  `pjlnd_address` varchar(200)  DEFAULT NULL,
  `pjlnd_dim` varchar(50)  DEFAULT NULL,
  `pjlnd_land_details` varchar(500)  DEFAULT NULL,
  `pjlnd_location` varchar(500)  DEFAULT NULL,
  `pjlnd_landmark` varchar(200)  DEFAULT NULL,
  `pjlnd_conversation_det` varchar(500)  DEFAULT NULL,
  `finance_ksfc` bit(1) DEFAULT NULL,
  `existing_land` bit(1) DEFAULT NULL,
  `pjlnd_notes` mediumtext ,
  `pjlnd_upload` varchar(300) DEFAULT NULL,
  `pjlnd_itemno` bigint DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50)  DEFAULT NULL,
  `modified_by` varchar(50) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200)  DEFAULT NULL,
  PRIMARY KEY (`pjlnd_rowid`),
  KEY `fk_tbl_app_furn_offc_cdtab` (`pjlnd_offc`),
  KEY `fk_tbl_app_furn_tbl_unit_mast` (`pjlnd_unit`)
);
select 'tbl_idm_proj_land Table Created' as ''; 
            end;
		END IF;
END;


     --  ---------------tbl_idm_proj_bldg---------------------
	BEgin
IF EXISTS(SELECT table_name 
						FROM INFORMATION_SCHEMA.TABLES
					   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_proj_bldg')

		-- If exists, retreive columns information from that table
		THEN		 	
           select 'tbl_idm_proj_bldg Table Already Exist' as '';                     
            else
            
             begin
CREATE TABLE `tbl_idm_proj_bldg` (
  `pjbdg_rowid` bigint NOT NULL AUTO_INCREMENT,
  `ut_cd` int DEFAULT NULL,
  `ut_slno` int DEFAULT NULL,
  `ln_acc` int DEFAULT NULL,
  `ln_sub` int DEFAULT NULL,
  `pjbdg_offc` tinyint DEFAULT NULL,
  `pjbdg_unit` int DEFAULT NULL,
  `pjbdg_sno` int DEFAULT NULL,
  `pjbdg_itm_no` bigint DEFAULT NULL,
  `pjbdg_dets` varchar(600) DEFAULT NULL,
  `pjbdg_roof` varchar(40) DEFAULT NULL,
  `pjbdg_plnth_o` decimal(20,2) DEFAULT NULL,
  `pjbdg_plnth_r` decimal(20,2) DEFAULT NULL,
  `pjbdg_ucosto` decimal(20,2) DEFAULT NULL,
  `pjbdg_ucostr` decimal(20,2) DEFAULT NULL,
  `pjbdg_tcosto` decimal(20,2) DEFAULT NULL,
  `pjbdg_tcostr` decimal(20,2) DEFAULT NULL,
  `subvention_bdg` bit(1) DEFAULT NULL,
  `apbs_totcst` decimal(20,2) DEFAULT NULL,
  `existing_bdg` bit(1) DEFAULT NULL,
  `contingency` decimal(20,2) DEFAULT NULL,
  `pjbdg_note` mediumtext ,
  `pjbdg_subv_note` mediumtext,
  `pjbdg_upload` varchar(300)  DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) DEFAULT NULL,
  `modified_by` varchar(50) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200)  DEFAULT NULL,
  `pjbdg_cons_value` decimal(10,2) DEFAULT NULL,
  `pjbdg_diff_value` decimal(10,2) DEFAULT NULL,
  `pjbdg_depr_method` int DEFAULT NULL,
  `pjbdg_depr_value` decimal(10,2) DEFAULT NULL,
  `pjbdg_subv_cost` decimal(10,2) DEFAULT NULL,
  PRIMARY KEY (`pjbdg_rowid`),
  KEY `fk_tbl_app_proj_bldg_offc_cdtab` (`pjbdg_offc`),
  KEY `fk_tbl_app_proj_bldg_tbl_unit_mast` (`pjbdg_unit`),
  KEY `fk_tbl_app_proj_bldg_tbl_depr_method_mast_idx` (`pjbdg_depr_method`)
);
select 'tbl_idm_proj_bldg Table Created' as ''; 
            end;
		END IF;
END;

     -- -------------------tbl_idm_proj_plmc---------------------
	 BEgin
    IF EXISTS(SELECT table_name 
						FROM INFORMATION_SCHEMA.TABLES
					   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_proj_plmc')

		-- If exists, retreive columns information from that table
		THEN		 	
           select 'tbl_idm_proj_plmc Table Already Exist' as '';                     
            else
            
             begin
	 CREATE TABLE `tbl_idm_proj_plmc` (
  `pjplmc_rowid` bigint NOT NULL AUTO_INCREMENT,
  `ud_cd` int DEFAULT NULL,
  `ut_slno` int DEFAULT NULL,
  `ln_acc` int DEFAULT NULL,
  `ln_sub` int DEFAULT NULL,
  `pjplmc_offc` tinyint DEFAULT NULL,
  `pjplmc_unit` int DEFAULT NULL,
  `pjplmc_sno` int DEFAULT NULL,
  `pjplmc_dets` varchar(600) DEFAULT NULL,
  `pjplmc_sup` varchar(100) DEFAULT NULL,
  `pjplmc_supadr` varchar(500)  DEFAULT NULL,
  `pjplmc_inv_no` varchar(100)  DEFAULT NULL,
  `pjplmc_inv_dt` date DEFAULT NULL,
  `pjplmc_clet_stat` int DEFAULT NULL,
  `pjplmc_reg` int DEFAULT NULL,
  `pjplmc_qty` int DEFAULT NULL,
  `pjplmc_stat` int DEFAULT NULL,
  `pjplmc_delry` int DEFAULT NULL,
  `pjplmc_cst` decimal(20,2) DEFAULT NULL,
  `pjplmc_tax` decimal(20,2) DEFAULT NULL,
  `pjplmc_totcst` decimal(20,2) DEFAULT NULL,
  `pjplmc_totcstr` decimal(20,2) DEFAULT NULL,
  `pjplmc_itm_no` bigint DEFAULT NULL,
  `pjplmc_non_ssi` bit(1) DEFAULT NULL,
  `pjplmc_qtyr` int DEFAULT NULL,
  `pjplmc_contingency` decimal(20,2) DEFAULT NULL,
  `pjplmc_existing` bit(1) DEFAULT NULL,
  `subvention_plmc` bit(1) DEFAULT NULL,
  `pjplmc_subv_cost` decimal(20,2) DEFAULT NULL,
  `pjplmc_depr_method` int DEFAULT NULL,
  `pjplmc_depr_value` decimal(20,2) DEFAULT NULL,
  `pjplmc_direct_prod` bit(1) DEFAULT NULL,
  `pjplmc_subv_notes` mediumtext,
  `pjplmc_notes` mediumtext,
  `pjplmc_upload` varchar(300) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50)  DEFAULT NULL,
  `modified_by` varchar(50)  DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200)  DEFAULT NULL,
  `pjplmc_gst` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`pjplmc_rowid`),
  KEY `fk_tbl_app_proj_plmc_offc_cdtab` (`pjplmc_offc`),
  KEY `fk_tbl_app_proj_plmc_tbl_unit_mast` (`pjplmc_unit`)
); 
select 'tbl_idm_proj_plmc Table Created' as ''; 
            end;
		END IF;
END;

    -- ------------------- tbl_idm_proj_imp_mc ----------------
		 BEgin
    IF EXISTS(SELECT table_name 
						FROM INFORMATION_SCHEMA.TABLES
					   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_proj_imp_mc ')

		-- If exists, retreive columns information from that table
		THEN		 	
           select 'tbl_idm_proj_imp_mc  Table Already Exist' as '';                     
            else
            
             begin

			 CREATE TABLE `tbl_idm_proj_imp_mc` (
  `pjimc_rowid` bigint NOT NULL AUTO_INCREMENT,
  `ut_cd` int DEFAULT NULL,
  `ut_slno` int DEFAULT NULL,
  `ln_acc` int DEFAULT NULL,
  `ln_sub` int DEFAULT NULL,
  `pjimc_offc` tinyint DEFAULT NULL,
  `pjimc_unit` int DEFAULT NULL,
  `pjimc_sno` int DEFAULT NULL,
  `pjimc_itm_no` bigint DEFAULT NULL,
  `pjimc_entry` varchar(50)  DEFAULT NULL,
  `pjimc_entry_i` varchar(50) DEFAULT NULL,
  `pjimc_crncy` varchar(50)  DEFAULT NULL,
  `pjimc_exrd` decimal(20,2) DEFAULT NULL,
  `pjimc_cif` decimal(20,2) DEFAULT NULL,
  `pjimc_cduty` decimal(20,2) DEFAULT NULL,
  `pjimc_tot_cost` decimal(20,2) DEFAULT NULL,
  `pjimc_stat` int DEFAULT NULL,
  `pjimc_dets` varchar(600)  DEFAULT NULL,
  `pjimc_sup` varchar(100)  DEFAULT NULL,
  `pjimc_qty` int DEFAULT NULL,
  `pjimc_delry` int DEFAULT NULL,
  `pjimc_supadr` varchar(500) DEFAULT NULL,
  `pjimc_cpct` decimal(20,2) DEFAULT NULL,
  `pjimc_camt` decimal(20,2) DEFAULT NULL,
  `pjimc_non_ssi` bit(1) DEFAULT NULL,
  `pjimc_rcif` decimal(20,2) DEFAULT NULL,
  `pjimc_rcrncy` varchar(50)  DEFAULT NULL,
  `pjimc_rexch` decimal(20,2) DEFAULT NULL,
  `pjimc_rcduty` decimal(20,2) DEFAULT NULL,
  `pjimc_rtot_cost` decimal(20,2) DEFAULT NULL,
  `pjimc_rcpct` decimal(20,2) DEFAULT NULL,
  `pjimc_rcamt` decimal(20,2) DEFAULT NULL,
  `pjimc_basic_cost` decimal(20,2) DEFAULT NULL,
  `pjimc_sup_regd` bit(1) DEFAULT NULL,
  `pjimc_boeno` varchar(10)  DEFAULT NULL,
  `pjimc_existing` bit(1) DEFAULT NULL,
  `pjimc_subvention` bit(1) DEFAULT NULL,
  `pjimc_subv_cost` decimal(20,2) DEFAULT NULL,
  `pjimc_depr_method` int DEFAULT NULL,
  `pjplmc_depr_value` decimal(20,2) DEFAULT NULL,
  `pjimc_direct_prod` bit(1) DEFAULT NULL,
  `pjimc_contingency` decimal(20,2) DEFAULT NULL,
  `pjimc_subv_notes` mediumtext,
  `pjimc_notes` mediumtext,
  `pjimc_upload` varchar(300) DEFAULT NULL,
  `pjimc_gst` decimal(20,2) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50)  DEFAULT NULL,
  `modified_by` varchar(50)  DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200)  DEFAULT NULL,
  `pjimc_total_basic_cost` decimal(20,2) DEFAULT NULL,
  `pjimc_oth_exp` decimal(10,2) DEFAULT NULL,
  PRIMARY KEY (`pjimc_rowid`),
  KEY `fk_tbl_app_proj_imp_mc_offc_cdtab` (`pjimc_offc`),
  KEY `fk_tbl_app_proj_imp_mc_tbl_unit_mast` (`pjimc_unit`)
);
       select 'tbl_idm_proj_imp_mc Table Created' as ''; 
            end;
		END IF;
END;

   -- ------------------ tbl_idm_furn -----------------------

   	 BEgin
    IF EXISTS(SELECT table_name 
						FROM INFORMATION_SCHEMA.TABLES
					   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_furn ')

		-- If exists, retreive columns information from that table
		THEN		 	
           select 'tbl_idm_furn  Table Already Exist' as '';                     
            else
            
             begin
       CREATE TABLE `tbl_idm_furn` (
  `pjf_rowid` bigint NOT NULL AUTO_INCREMENT,
  `ut_cd` int DEFAULT NULL,
  `ut_slno` int DEFAULT NULL,
  `ln_acc` int DEFAULT NULL,
  `pjf_offc` tinyint DEFAULT NULL,
  `pjf_unit` int DEFAULT NULL,
  `pjf_sno` int DEFAULT NULL,
  `pjf_itm_no` bigint DEFAULT NULL,
  `pjf_dets` varchar(600)  DEFAULT NULL,
  `pjf_cst` decimal(20,2) DEFAULT NULL,
  `pjf_qty` int DEFAULT NULL,
  `pjf_tax` decimal(20,2) DEFAULT NULL,
  `pjf_totcst` decimal(20,2) DEFAULT NULL,
  `pjf_cpct` decimal(20,2) DEFAULT NULL,
  `pjf_rcpct` decimal(20,2) DEFAULT NULL,
  `pjf_contingency` decimal(20,2) DEFAULT NULL,
  `pjf_camt` decimal(20,2) DEFAULT NULL,
  `pjf_rcamt` decimal(20,2) DEFAULT NULL,
  `pjf_sup` varchar(100)  DEFAULT NULL,
  `pjf_supadr` varchar(500) DEFAULT NULL,
  `pjf_reg` bit(1) DEFAULT NULL,
  `pjf_inv_no` varchar(100) DEFAULT NULL,
  `pjf_inv_dt` date DEFAULT NULL,
  `pjf_clet_stat` int DEFAULT NULL,
  `pjf_stat` int DEFAULT NULL,
  `pjf_delry` int DEFAULT NULL,
  `pjf_totcstr` int DEFAULT NULL,
  `pjf_non_ssi` bit(1) DEFAULT NULL,
  `pjf_aqrd_stat` int DEFAULT NULL,
  `pjf_existing` bit(1) DEFAULT NULL,
  `pjf_notes` mediumtext,
  `pjf_upload` varchar(300)  DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50)  DEFAULT NULL,
  `modified_by` varchar(50) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200)  DEFAULT NULL,
  PRIMARY KEY (`pjf_rowid`),
  KEY `fk_tbl_app_furn_offc_cdtab` (`pjf_offc`),
  KEY `fk_tbl_app_furn_tbl_unit_mast` (`pjf_unit`)
); 
select 'tbl_idm_furn Table Created' as ''; 
            end;
		END IF;
END;

	-- ------------------- tbl_actype_cdtab-----------------------
    BEgin
IF EXISTS(SELECT table_name 
						FROM INFORMATION_SCHEMA.TABLES
					   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_actype_cdtab')

		-- If exists, retreive columns information from that table
		THEN		 	
           select 'tbl_actype_cdtab Table Already Exist' as '';                     
            else
            
             begin

CREATE TABLE `tbl_actype_cdtab` (
  `actype_cd` int NOT NULL AUTO_INCREMENT,
  `actype_dets` varchar(50) DEFAULT NULL,
  `actype_dis_seq` int DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) DEFAULT NULL,
  `modified_by` varchar(50) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  PRIMARY KEY (`actype_cd`)
);
select 'tbl_actype_cdtab Table Created' as ''; 
            end;
		END IF;
END;

						-- tbl_ind_cdtab --
Begin
IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_ind_cdtab')

		-- If not exists, creat a new table
Then
ALTER TABLE `tbl_ind_cdtab` 
CHANGE COLUMN `created_by` `created_by` VARCHAR(50) CHARACTER SET 'utf8mb4' NULL DEFAULT NULL ,
CHANGE COLUMN `modified_by` `modified_by` VARCHAR(50) CHARACTER SET 'utf8mb4' NULL DEFAULT NULL ,
CHANGE COLUMN `unique_id` `unique_id` VARCHAR(200) CHARACTER SET 'utf8mb4' NULL DEFAULT NULL ;
select 'tbl_ind_cdtab Table Modified' as ''; 
ELSE
CREATE TABLE `tbl_ind_cdtab` (
  `ind_cd` int NOT NULL AUTO_INCREMENT,
  `ind_dets` varchar(100) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `modified_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`ind_cd`)
) ;
select 'tbl_ind_cdtab Table Already Exist' as ''; 
END IF;
END;

						-- tbl_prod_cdtab --
Begin
IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_prod_cdtab')

		-- If not exists, creat a new table
Then

ALTER TABLE `tbl_prod_cdtab` 
CHANGE COLUMN `unique_id` `unique_id` VARCHAR(200) CHARACTER SET 'utf8mb4' NULL DEFAULT NULL ,
CHANGE COLUMN `created_by` `created_by` VARCHAR(50) CHARACTER SET 'utf8mb4' NULL DEFAULT NULL ,
CHANGE COLUMN `modified_by` `modified_by` VARCHAR(50) CHARACTER SET 'utf8mb4' NULL DEFAULT NULL ;
select 'tbl_prod_cdtab Table Modified' as ''; 
ELSE
CREATE TABLE `tbl_prod_cdtab` (
  `prod_ind` int NOT NULL,
  `prod_cd` int NOT NULL,
  `prod_dets` varchar(80) NOT NULL,
  `dept` varchar(6) DEFAULT NULL,
  `prod_ncd` int DEFAULT NULL,
  `prod_ndt` varchar(50) DEFAULT NULL,
  `prof_flg` int DEFAULT NULL,
  `id` int NOT NULL AUTO_INCREMENT,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `unique_id` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `created_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `modified_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_tbl_prod_cdtab_tbl_ind_cdtab` (`prod_ind`),
  CONSTRAINT `fk_tbl_prod_cdtab_tbl_ind_cdtab` FOREIGN KEY (`prod_ind`) REFERENCES `tbl_ind_cdtab` (`ind_cd`)
);
select 'tbl_prod_cdtab Table Already Exist' as ''; 
END IF;
END;

						-- tbl_pqual_cdtab --
Begin
IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_pqual_cdtab')

		-- If not exists, creat a new table
Then


select 'tbl_pqual_cdtab Table Modified' as ''; 
ELSE

CREATE TABLE `tbl_pqual_cdtab` (
  `pqual_cd` int NOT NULL AUTO_INCREMENT,
  `pqual_desc` varchar(200) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) DEFAULT NULL,
  `modified_by` varchar(50) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  PRIMARY KEY (`pqual_cd`)
);
select 'tbl_pqual_cdtab Table Already Exist' as ''; 
END IF;
END;





						-- tbl_domi_cdtab --
Begin
IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_domi_cdtab')

		-- If not exists, creat a new table
Then

ALTER TABLE `tbl_domi_cdtab` 
CHANGE COLUMN `created_by` `created_by` VARCHAR(50) CHARACTER SET 'utf8mb4' NULL DEFAULT NULL ;
select 'tbl_domi_cdtab Table Modified' as ''; 
ELSE
CREATE TABLE `tbl_domi_cdtab` (
  `dom_cd` int NOT NULL AUTO_INCREMENT,
  `dom_dets` varchar(30) NOT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `modified_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`dom_cd`)
);
select 'tbl_domi_cdtab Table Already Exist' as ''; 
END IF;
END;

Begin
IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tlq_cdtab')

		-- If not exists, creat a new table
Then


ALTER TABLE `tlq_cdtab` 
CHANGE COLUMN `TLQ_NAME_KANNADA` `TLQ_NAME_KANNADA` VARCHAR(100) CHARACTER SET 'utf8mb4' COLLATE 'utf8mb4_0900_ai_ci' NULL DEFAULT NULL ;
select 'tlq_cdtab Table Modified' as ''; 
ELSE
select 'tlq_cdtab Table Already Exist' as ''; 
END IF;
END;


-- -------------------------------tbl_pdesig_cdtab---------------------------
Begin
IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_pdesig_cdtab')

		-- If not exists, creat a new table
Then
select 'tbl_pdesig_cdtab Table Exist' as ''; 
ELSE
CREATE TABLE `tbl_pdesig_cdtab` (
  `pdesig_cd` int NOT NULL AUTO_INCREMENT,
  `pdesig_dets` varchar(100) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `modified_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`pdesig_cd`)
);
select 'tbl_pdesig_cdtab Table Created' as ''; 
END IF;
END;
-- -------------------------------tbl_size_cdtab---------------------------
Begin
IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_size_cdtab')

		-- If not exists, creat a new table
Then
select 'tbl_size_cdtab Table Exist' as ''; 
ELSE
CREATE TABLE `tbl_size_cdtab` (
  `size_cd` int NOT NULL AUTO_INCREMENT,
  `size_dets` varchar(50) NOT NULL,
  `size_flag` int DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `modified_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`size_cd`)
);
select 'tbl_size_cdtab Table Created' as ''; 
END IF;
END;

-- -----------------------Change Loaction Details Tab -------------------- Added by Gagana--------------
						-- tlq_cdtab --
Begin
IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tlq_cdtab')

		-- If exists, Update the table
Then
select 'tlq_cdtab Table Already Exist' as '';
				Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tlq_cdtab' AND column_name='TLQ_NAME_KANNADA')
					THEN
								ALTER TABLE `tlq_cdtab` 
								CHANGE COLUMN `TLQ_NAME_KANNADA` `TLQ_NAME_KANNADA` VARCHAR(100) CHARACTER SET 'utf8mb4' COLLATE 'utf8mb4_0900_ai_ci' NULL DEFAULT NULL ;
					      	select 'Column TLQ_NAME_KANNADA in tlq_cdtab Modified' as ' ';
						ELSE
						select 'Column TLQ_NAME_KANNADA in tlq_cdtab NotExist' as ' ';
					end if;
			    END;
END IF;
END;

-- -----------------------Change Protomer Address Details Tab -------------------- Added by Gagana--------------
						-- idm_prom_address --
Begin
IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_prom_address')

		-- If not exists, creat a new table
Then
select 'tbl_idm_prom_address Table Already Exist' as ' ';
Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_prom_address' AND column_name='loan_acc')
						THEN
                        ALTER TABLE tbl_idm_prom_address
							MODIFY COLUMN  loan_acc  bigint NOT NULL;
								  select 'Column loan_acc in tbl_idm_prom_address table Modified' as ' ';
						ELSE
							
						select 'Column loan_acc in tbl_idm_prom_address table Created' as ' ';
				end if;
            end;
Else
CREATE TABLE `tbl_idm_prom_address` (
  `idm_promadr_id` int NOT NULL AUTO_INCREMENT,
  `loan_acc` bigint DEFAULT NULL,
  `loan_sub` int DEFAULT NULL,
  `offc_cd` tinyint DEFAULT NULL,
  `ut_cd` int DEFAULT NULL,
  `promoter_code` bigint NOT NULL,
  `prom_address` varchar(500) DEFAULT NULL,
  `prom_state_cd` int DEFAULT NULL,
  `prom_district_cd` int DEFAULT NULL,
  `prom_pincode` int DEFAULT NULL,
  `adr_permanent` bit(1) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) DEFAULT NULL,
  `modified_by` varchar(50) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  PRIMARY KEY (`idm_promadr_id`),
  KEY `fK_tbl_idm_prom_address_tbl_unit_mast` (`ut_cd`),
  KEY `fk_tbl_idm_prom_address_offc_cdtab` (`offc_cd`),
  KEY `fk_tbl_idm_prom_address_tbl_app_loan_mast` (`loan_acc`),
  KEY `fk_tbl_idm_prom_address_tbl_pincode_state_cdtab` (`prom_state_cd`),
  KEY `fk_tbl_idm_prom_address_tbl_pincode_district_cdtab` (`prom_district_cd`),
  KEY `fk_tbl_idm_prom_address_tbl_prom_cdtab` (`promoter_code`), 
  CONSTRAINT `fk_tbl_idm_prom_address_offc_cdtab` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`),
  CONSTRAINT `fk_tbl_idm_prom_address_tbl_app_loan_mast` FOREIGN KEY (`loan_acc`) REFERENCES `tbl_app_loan_mast` (`ln_no`),
  CONSTRAINT `fK_tbl_idm_prom_address_tbl_unit_mast` FOREIGN KEY (`ut_cd`) REFERENCES `tbl_unit_mast` (`ut_cd`),
  CONSTRAINT `fk_tbl_idm_prom_address_tbl_pincode_state_cdtab`  FOREIGN KEY (`prom_state_cd`) REFERENCES `tbl_pincode_state_cdtab` (`pincode_state_cd`),
  CONSTRAINT `fk_tbl_idm_prom_address_tbl_pincode_district_cdtab`  FOREIGN KEY (`prom_district_cd`) REFERENCES `tbl_pincode_district_cdtab` (`pincode_district_cd`),
  CONSTRAINT `fk_tbl_idm_prom_address_tbl_prom_cdtab`  FOREIGN KEY (`promoter_code`) REFERENCES `tbl_prom_cdtab` (`promoter_code`)
) ;
select 'tbl_idm_prom_address Table Created' as ' ';
END IF;
END;
-- -----------------------Change Protomer Address Details Tab -------------------- Added by Gagana--------------
						-- tbl_prom_cdtab --
Begin
IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_prom_cdtab')

		-- If exists, Update the table
Then
select 'tbl_prom_cdtab Table Already Exist' as '';
				Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_prom_cdtab' AND column_name='created_by')
					THEN
								ALTER TABLE `tbl_prom_cdtab` 
								CHANGE COLUMN `created_by` `created_by` VARCHAR(50) CHARACTER SET 'utf8mb4' COLLATE 'utf8mb4_0900_ai_ci' NULL DEFAULT NULL ;
								select 'Column created_by in tbl_prom_cdtab Modified' as ' ';
					ELSE
							

						select 'Column created_by in tbl_prom_cdtab Modified' as ' ';
					end if;
			    END;

				Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_prom_cdtab' AND column_name='modified_by')
					THEN
								ALTER TABLE `tbl_prom_cdtab` 
							CHANGE COLUMN `modified_by` `modified_by` VARCHAR(50) CHARACTER SET 'utf8mb4' COLLATE 'utf8mb4_0900_ai_ci' NULL DEFAULT NULL ;
							select 'Column modified_by in tbl_prom_cdtab Modified' as ' ';
					ELSE
							


						select 'Column modified_by in tbl_prom_cdtab Modified' as ' ';
					end if;
			    END;

				Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_prom_cdtab' AND column_name='unique_id')
					THEN
							ALTER TABLE `tbl_prom_cdtab` 
							CHANGE COLUMN `unique_id` `unique_id` VARCHAR(200) CHARACTER SET 'utf8mb4' COLLATE 'utf8mb4_0900_ai_ci' NULL DEFAULT NULL ;
								select 'Column unique_id in tbl_prom_cdtab Modified' as ' ';
					ELSE
							

							select 'Column unique_id in tbl_prom_cdtab NotExist' as ' ';
						
					end if;
			    END;




END IF;
END;



-- -----------------------Change Product Details Tab -------------------- Added by Gowtham--------------
						-- tbl_idm_unit_products --
Begin
IF  EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_unit_products')

		-- If not exists, creat a new table
Then
select'tbl_idm_unit_products Already Exist' as ' ';
 Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_unit_products' AND column_name='ut_slno')
					THEN
								select 'Column ut_slno in tbl_idm_unit_products  Already Exist' as ' ';
					ELSE
							ALTER TABLE `tbl_idm_unit_products` 
							Add `ut_slno`  int DEFAULT NULL AFTER `modified_date`;
						select 'Column ut_slno in tbl_idm_unit_products  Created' as ' ';
					end if;
			END;
 Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_unit_products' AND column_name='prod_id')
					THEN
								select 'Column prod_id in tbl_idm_unit_products  Already Exist' as ' ';
					ELSE
							ALTER TABLE `tbl_idm_unit_products` 
							Add `prod_id`  int DEFAULT NULL AFTER `prod_cd`;
						select 'Column prod_id in tbl_idm_unit_products  Created' as ' ';
					end if;
			END;
 Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_unit_products' AND column_name='ind_id')
					THEN
								select 'Column ind_id in tbl_idm_unit_products  Already Exist' as ' ';
					ELSE
							ALTER TABLE `tbl_idm_unit_products` 
							Add `ind_id`  int DEFAULT NULL AFTER `prod_id`;
						select 'Column ind_id in tbl_idm_unit_products  Created' as ' ';
					end if;
			END;
Else
CREATE TABLE `tbl_idm_unit_products` (
  `idm_utproduct_rowid` bigint NOT NULL AUTO_INCREMENT,
  `loan_acc` bigint DEFAULT NULL,
  `loan_sub` int DEFAULT NULL,
  `offc_cd` tinyint DEFAULT NULL,
  `prod_cd` int DEFAULT  NULL,
  `prod_id` int DEFAULT  NULL,
  `ind_id` int DEFAULT  NULL,
  `unique_id` varchar(150) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `created_by` varchar(50) DEFAULT NULL,
  `modified_by` varchar(50) DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `ut_slno` int DEFAULT NULL,
  PRIMARY KEY (`idm_utproduct_rowid`),
  KEY `offc_cd` (`offc_cd`),
  KEY `loan_acc` (`loan_acc`),
  KEY `prod_id` (`prod_id`),
  CONSTRAINT `fk_tbl_idm_unit_products_tbl_ind_cdtab` FOREIGN KEY (`prod_id`) REFERENCES `tbl_ind_cdtab` (`ind_cd`),
  CONSTRAINT `fk_tbl_idm_unit_products_tbl_prod_cdtab` FOREIGN KEY (`prod_id`) REFERENCES `tbl_prod_cdtab` (`id`),
  CONSTRAINT `tbl_idm_unit_products_ibfk_2` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`),
  CONSTRAINT `tbl_idm_unit_products_idfk_7` FOREIGN KEY (`loan_acc`) REFERENCES `tbl_app_loan_mast` (`ln_no`)
);
select'tbl_idm_unit_products Created' as ' ';
END IF;
END;

-- -----------------------Change Asset Information Tab -------------------- Added by Gowtham--------------
						-- idm_prom_asset_det --
Begin
IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'idm_prom_asset_det')

		-- If not exists, creat a new table
Then
		select 'idm_prom_asset_det table already exist ' as ' ';
		 Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='idm_prom_asset_det' AND column_name='land_type')
						THEN
                       ALTER TABLE idm_prom_asset_det 
					  MODIFY COLUMN land_type int DEFAULT NULL;
							  select 'Column land_type in idm_prom_asset_det table Modified' as ' ';
						ELSE
						select 'Column land_type in idm_prom_asset_det Created' as ' ';
					end if;
		 END;
ELSE
CREATE TABLE `idm_prom_asset_det` (
  `idm_promasset_id` int NOT NULL AUTO_INCREMENT,
  `loan_acc` bigint DEFAULT NULL,
  `loan_sub` int DEFAULT NULL,
  `offc_cd` tinyint DEFAULT NULL,
  `ut_cd` int DEFAULT NULL,
  `promoter_code` bigint NOT NULL,
  `assetcat_cd` int NOT NULL,
  `assettype_cd` int NOT NULL,
  `land_type` int DEFAULT NULL,
  `idm_asset_siteno` varchar(50) DEFAULT NULL,
  `idm_asset_addr` varchar(200) DEFAULT NULL,
  `idm_asset_dim` varchar(100) DEFAULT NULL,
  `idm_asset_area` decimal(10,0) DEFAULT NULL,
  `idm_asset_desc` varchar(500) DEFAULT NULL,
  `idm_asset_value` decimal(10,0) DEFAULT NULL,
  `unique_id` varchar(150) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) DEFAULT NULL,
  `modified_by` varchar(50) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  PRIMARY KEY (`idm_promasset_id`),
  KEY `fk_idm_prom_asset_det_offc_cdtab` (`offc_cd`),
  KEY `fk_idm_prom_asset_det_tbl_assetcat_cdtab` (`assetcat_cd`),
  KEY `idm_prom_asset_det_tbl_assettype_cdtab` (`assettype_cd`),
  KEY `idm_prom_asset_det_tbl_prom_cdtab` (`promoter_code`),
  CONSTRAINT `idm_prom_asset_det_offc_cdtab` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`),
  CONSTRAINT `idm_prom_asset_det_tbl_assetcat_cdtab` FOREIGN KEY (`assetcat_cd`) REFERENCES `tbl_assetcat_cdtab` (`assetcat_cd`),
  CONSTRAINT `idm_prom_asset_det_tbl_assettype_cdtab` FOREIGN KEY (`assettype_cd`) REFERENCES `tbl_assettype_cdtab` (`assettype_cd`),
  CONSTRAINT `idm_prom_asset_det_tbl_prom_cdtab` FOREIGN KEY (`promoter_code`) REFERENCES `tbl_prom_cdtab` (`promoter_code`)
);
select 'idm_prom_asset_det Table Created' as '';
END IF;
END;



-- -----------------------Change Bank Details--------------
						-- tbl_idm_unit_bank --
Begin
IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_unit_bank')

		-- If not exists, creat a new table
Then
select 'tbl_idm_unit_bank Already Exist' as '';
Begin
Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_unit_bank' AND column_name='ut_cd')
						THEN
                        ALTER TABLE tbl_idm_unit_bank
							MODIFY COLUMN `ut_cd` int DEFAULT NULL;
								  select 'Column ut_cd in tbl_idm_unit_bank table Modified' as ' ';
						ELSE
							
						select 'Column ut_cd in tbl_idm_unit_bank table Created' as ' ';
				end if;
            end;
			Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_unit_bank' AND column_name='loan_acc')
						THEN
                        ALTER TABLE tbl_idm_unit_bank
							MODIFY COLUMN  loan_acc  bigint NOT NULL;
								  select 'Column loan_acc in tbl_idm_unit_bank table Modified' as ' ';
						ELSE
							
						select 'Column loan_acc in tbl_idm_unit_bank table Created' as ' ';
				end if;
            end;

			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_unit_bank' AND column_name='ut_bank_pincode')
						THEN
								  select 'Column ut_pincode in tbl_idm_unit_bank  already exist' as ' ';
						ELSE
							ALTER TABLE `tbl_idm_unit_bank`
					Add `ut_bank_pincode` int DEFAULT NULL;
						select 'Column ut_bank_pincode in tbl_idm_unit_bank  Created' as ' ';
				end if;
            end;
			Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_unit_bank' AND column_name='bank_ifsc_id')
						THEN
								  select 'Column ut_pincode in tbl_idm_unit_bank  already exist' as ' ';
						ELSE
							ALTER TABLE `tbl_idm_unit_bank`
					Add `bank_ifsc_id` int DEFAULT NULL;
						select 'Column bank_ifsc_id in tbl_idm_unit_bank  Created' as ' ';
				end if;
            end;
			Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_unit_bank' AND column_name='unique_id')
						THEN
								  select 'Column unique_id in tbl_idm_unit_bank  already exist' as ' ';
						ELSE
							ALTER TABLE `tbl_idm_unit_bank`
					Add `unique_id` varchar(200) DEFAULT NULL;
						select 'Column unique_id in tbl_idm_unit_bank  Created' as ' ';
				end if;
            end;
			 
Else
CREATE TABLE `tbl_idm_unit_bank` (
  `idm_utbank_rowid` int NOT NULL AUTO_INCREMENT,
  `loan_acc` bigint DEFAULT NULL,
  `loan_sub` int DEFAULT NULL,
  `offc_cd` tinyint DEFAULT NULL,
  `ut_cd` int DEFAULT NULL,
  `ut_ifsc` varchar(10) DEFAULT NULL,
  `ut_bank_pincode` int DEFAULT NULL,
  `bank_ifsc_id` int DEFAULT NULL,
  `ut_bank` varchar(100) DEFAULT NULL,
  `ut_bank_branch` varchar(100) DEFAULT NULL,
  `ut_bank_address` varchar(500) DEFAULT NULL,
  `ut_bank_area` varchar(100) DEFAULT NULL,
  `ut_bank_city` varchar(100) DEFAULT NULL,
  `ut_bank_phone` varchar(10) DEFAULT NULL,
  `ut_bank_primary` bit(1) DEFAULT NULL,
  `ut_bank_state` varchar(100) DEFAULT NULL,
  `ut_bank_district` varchar(100) DEFAULT NULL,
  `ut_bank_taluka` varchar(100) DEFAULT NULL,
  `ut_bank_accno` varchar(30) DEFAULT NULL,
  `ut_bank_holdername` varchar(100) DEFAULT NULL,
  `ut_acc_type` int DEFAULT NULL,
  `unique_id` varchar(200) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) DEFAULT NULL,
  `modified_by` varchar(50) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  PRIMARY KEY (`idm_utbank_rowid`),
  KEY `fk_tbl_idm_unit_bank_offc_cdtab` (`offc_cd`),
  KEY `fk_tbl_idm_unit_bank_tbl_app_loan_mast` (`loan_acc`),
  KEY `fk_tbl_idm_unit_bank_tbl_unit_mast` (`ut_cd`),
  KEY `fk_tbl_idm_unit_bank_tbl_ifsc_master` (`ut_ifsc`),
  CONSTRAINT `fk_tbl_idm_unit_bank_offc_cdtab` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`),
  CONSTRAINT `fk_tbl_idm_unit_bank_tbl_app_loan_mast` FOREIGN KEY (`loan_acc`) REFERENCES `tbl_app_loan_mast` (`ln_no`),
  CONSTRAINT `fk_tbl_idm_unit_bank_tbl_ifsc_master` FOREIGN KEY (`ut_ifsc`) REFERENCES `tbl_ifsc_master` (`ifsc_cd`),
  CONSTRAINT `fk_tbl_idm_unit_bank_tbl_unit_mast` FOREIGN KEY (`ut_cd`) REFERENCES `tbl_unit_mast` (`ut_cd`)
);
select 'tbl_idm_unit_bank Created' as '';
END IF;
END;


-- -----------------------Change in Liability Information -------------------- 
						-- tbl_idm_prom_liab_det --
Begin
IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_prom_liab_det')

		-- If not exists, creat a new table
Then
select 'tbl_idm_prom_liab_det Already Exist' as '';
Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_prom_liab_det' AND column_name='idm_promliab_id' and COLUMN_KEY = 'PRI')
						THEN
                        select 'Primary Kay has been added' as ' ';
						ELSE
						 ALTER TABLE `tbl_idm_prom_liab_det` 
						CHANGE COLUMN `idm_promliab_id` `idm_promliab_id` INT NOT NULL AUTO_INCREMENT ,
						ADD PRIMARY KEY (`idm_promliab_id`);
			           select 'Column idm_promliab_id in tbl_idm_prom_liab_det table Modified' as ' ';
				end if;
            end;
Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_prom_liab_det' AND column_name='loan_acc')
						THEN
                        ALTER TABLE tbl_idm_prom_liab_det
							MODIFY COLUMN  loan_acc  bigint NOT NULL;
								  select 'Column loan_acc in tbl_idm_prom_liab_det table Modified' as ' ';
						ELSE
							
						select 'Column loan_acc in tbl_idm_prom_liab_det table Created' as ' ';
				end if;
            end;
ELSE
CREATE TABLE `tbl_idm_prom_liab_det` (
  `idm_promliab_id` int NOT NULL AUTO_INCREMENT,
  `loan_acc` bigint DEFAULT NULL,
  `loan_sub` int DEFAULT NULL,
  `offc_cd` tinyint DEFAULT NULL,
  `ut_cd` int DEFAULT NULL,
  `promoter_code` bigint DEFAULT NULL,
  `idm_liab_desc` varchar(100) DEFAULT NULL,
  `idm_liab_value` decimal(10,0) DEFAULT NULL,    
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) DEFAULT NULL,
  `modified_by` varchar(50) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(150) DEFAULT NULL,
  PRIMARY KEY (`idm_promliab_id`),
  KEY `fk_tbl_idm_prom_liab_det_offc_cdtab` (`offc_cd`),
  KEY `fk_tbl_idm_prom_liab_det_tbl_app_loan_mast` (`loan_acc`), 
 KEY `fk_tbl_idm_prom_liab_det_tbl_unit_mast` (`ut_cd`), 
  KEY `fk_tbl_idm_prom_liab_det_tbl_prom_cdtab` (`promoter_code`), 
CONSTRAINT `fk_tbl_idm_prom_liab_det_offc_cdtab` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`),
CONSTRAINT `fk_tbl_idm_prom_liab_det_tbl_app_loan_mast` FOREIGN KEY (`loan_acc`) REFERENCES `tbl_app_loan_mast` (`ln_no`),
CONSTRAINT `fk_tbl_idm_prom_liab_det_tbl_unit_mast` FOREIGN KEY (`ut_cd`) REFERENCES `tbl_unit_mast` (`ut_cd`),
CONSTRAINT `fk_tbl_idm_prom_liab_det_tbl_prom_cdtab` FOREIGN KEY (`promoter_code`) REFERENCES `tbl_prom_cdtab` (`promoter_code`)
);
select 'tbl_idm_prom_liab_det Created' as '';
END IF;
END;

-- -----------------------Change in Promoter Net Worth -------------------- 
						-- ---tbl_idm_prom_nw_det---------
Begin
IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_prom_nw_det')

		-- If not exists, creat a new table
Then
select 'tbl_idm_prom_nw_det Already Exist' as '';
Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_prom_nw_det' AND column_name='loan_acc')
						THEN
                        ALTER TABLE tbl_idm_prom_nw_det
							MODIFY COLUMN  loan_acc  bigint NOT NULL;
								  select 'Column loan_acc in tbl_idm_prom_nw_det table Modified' as ' ';
						ELSE
							
						select 'Column loan_acc in tbl_idm_prom_nw_det table Created' as ' ';
				end if;
            end;
ELSE
CREATE TABLE `tbl_idm_prom_nw_det` (
  `idm_prom_nw_id` int NOT NULL AUTO_INCREMENT,
  `loan_acc` bigint DEFAULT NULL,
  `loan_sub` int DEFAULT NULL,
  `offc_cd` tinyint DEFAULT NULL,
  `ut_cd` int DEFAULT NULL,
  `promoter_code` bigint DEFAULT NULL,
  `idm_immov` decimal(10,2) DEFAULT NULL,
  `idm_mov` decimal(10,2) DEFAULT NULL,
  `idm_liab` decimal(10,2) DEFAULT NULL,  
  `idm_nw` decimal(10,2) DEFAULT NULL,  
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) DEFAULT NULL,
  `modified_by` varchar(50) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(150) DEFAULT NULL,
  PRIMARY KEY (`idm_prom_nw_id`),
  KEY `fk_tbl_idm_prom_nw_det_offc_cdtab` (`offc_cd`),
  KEY `fk_tbl_idm_prom_nw_det_tbl_app_loan_mast` (`loan_acc`), 
 KEY `fk_tbl_idm_prom_nw_det_tbl_unit_mast` (`ut_cd`), 
  KEY `fk_tbl_idm_prom_nw_det_tbl_prom_cdtab` (`promoter_code`), 
CONSTRAINT `fk_tbl_idm_prom_nw_det_offc_cdtab` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`),
CONSTRAINT `fk_tbl_idm_prom_nw_det_tbl_app_loan_mast` FOREIGN KEY (`loan_acc`) REFERENCES `tbl_app_loan_mast` (`ln_no`),
CONSTRAINT `fk_tbl_idm_prom_nw_det_tbl_unit_mast` FOREIGN KEY (`ut_cd`) REFERENCES `tbl_unit_mast` (`ut_cd`),
CONSTRAINT `fk_tbl_idm_prom_nw_det_tbl_prom_cdtab` FOREIGN KEY (`promoter_code`) REFERENCES `tbl_prom_cdtab` (`promoter_code`)
) ;
select 'tbl_idm_prom_nw_det Created' as '';
END IF;
END;


	-- ------------------- Idm_Promoter_Bank_Details-----------------------
    BEgin
IF EXISTS(SELECT table_name 
						FROM INFORMATION_SCHEMA.TABLES
					   WHERE table_schema = DBName
					 AND table_name LIKE 'idm_promoter_bank_details')

		-- If exists, retreive columns information from that table
		THEN		 	
           select 'idm_promoter_bank_details Table Already Exist' as '';                     
            else
            
             begin
				CREATE TABLE `idm_promoter_bank_details` (
  `idm_prom_bank_id` int NOT NULL AUTO_INCREMENT,
  `loan_acc` bigint DEFAULT NULL,
  `loan_sub` int DEFAULT NULL,
  `offc_cd` tinyint DEFAULT NULL,
  `ut_cd` int DEFAULT NULL,
  `promoter_code` bigint NOT NULL,
  `prm_ac_type` int DEFAULT NULL,
  `prm_bank_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `prm_bank_branch` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `prm_bank_loc` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `prm_ac_no` bigint DEFAULT NULL,
  `prm_ifsc_id` int NOT NULL,
  `prm_bank_address` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `prm_bank_state` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `prm_bank_district` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `prm_bank_taluka` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `prm_bank_pincode` int DEFAULT NULL,
  `prm_bank_ac_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `prm_primary_bank` tinyint(1) DEFAULT NULL,
  `prm_cibil_score` int DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) DEFAULT NULL,
  `modified_by` varchar(50) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  PRIMARY KEY (`idm_prom_bank_id`),
  KEY `loan_acc` (`loan_acc`),
  KEY `offc_cd` (`offc_cd`), 
  KEY `prm_ifsc_id` (`prm_ifsc_id`),
  CONSTRAINT `idm_promoter_bank_details_ibfk_1` FOREIGN KEY (`loan_acc`) REFERENCES `tbl_app_loan_mast` (`ln_no`),
  CONSTRAINT `idm_promoter_bank_details_ibfk_2` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`),
  CONSTRAINT `idm_promoter_bank_details_ibfk_5` FOREIGN KEY (`prm_ifsc_id`) REFERENCES `tbl_ifsc_master` (`ifsc_rowid`)
);
select 'idm_promoter_bank_details Table Created' as ''; 
            end;
		END IF;
END;


-- ----------------------Idm_Promoter--------------------------

Begin
IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'idm_promoter')

		-- If exists, retreive columns information from that table
		THEN		 	
            select 'idm_promoter Table Already Exist' as '';                     
            else
            
             begin
				CREATE TABLE `idm_promoter` (
  `idm_prom_id` int NOT NULL AUTO_INCREMENT,
  `loan_acc` bigint DEFAULT NULL,
  `loan_sub` int DEFAULT NULL,
  `offc_cd` tinyint DEFAULT NULL,
  `ut_cd` int DEFAULT NULL,
  `promoter_code` bigint NOT NULL,
  `prom_name` varchar(35) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `pdesig_cd` int NOT NULL,
  `prom_gender` tinytext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `prom_dob` date DEFAULT NULL,
  `prom_age` int DEFAULT NULL,
  `name_father_spouse` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `pclas_cd` int DEFAULT NULL,
  `psubclas_cd` int DEFAULT NULL,
  `prom_exp_yrs` int DEFAULT NULL,
  `prom_exp_det` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `pqual_cd` int DEFAULT NULL,
  `prom_addlqual` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `dom_cd` int DEFAULT NULL,
  `prom_passport` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `prom_pan` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `prom_jn_dt` date DEFAULT NULL,
  `prom_ex_dt` date DEFAULT NULL,
  `prom_mobile_no` bigint DEFAULT NULL,
  `prom_email` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `prom_tel_no` bigint DEFAULT NULL,
  `prom_chief` bit(1) DEFAULT NULL,
  `prom_major` bit(1) DEFAULT NULL,
  `prom_phy_handicap` bit(1) DEFAULT NULL,
  `prom_photo` varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) DEFAULT NULL,
  `modified_by` varchar(50) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  PRIMARY KEY (`idm_prom_id`),
  KEY `loan_acc` (`loan_acc`),
  KEY `offc_cd` (`offc_cd`),
  KEY `ut_cd` (`ut_cd`), 
  KEY `pdesig_cd` (`pdesig_cd`),
  CONSTRAINT `idm_promoter_ibfk_1` FOREIGN KEY (`loan_acc`) REFERENCES `tbl_app_loan_mast` (`ln_no`),
  CONSTRAINT `idm_promoter_ibfk_2` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`),
  CONSTRAINT `idm_promoter_ibfk_3` FOREIGN KEY (`ut_cd`) REFERENCES `tbl_unit_mast` (`ut_cd`),
  CONSTRAINT `idm_promoter_ibfk_5` FOREIGN KEY (`pdesig_cd`) REFERENCES `tbl_pdesig_cdtab` (`pdesig_cd`)
) ;
select 'idm_promoter Table Created' as '';  
            end;
		END IF;
END;

Begin
IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'idm_unit_details')

		-- If exists, retreive columns information from that table
		THEN		 	
            select 'idm_unit_details Table Already Exist' as '';                     
            else
            
             begin
				CREATE TABLE `idm_unit_details` (
				  `idm_ut_rowid` bigint NOT NULL AUTO_INCREMENT,
				  `loan_acc` bigint DEFAULT NULL,
				  `loan_sub` int DEFAULT NULL,
				  `offc_cd` tinyint DEFAULT NULL,
				  `ut_cd` int NOT NULL,
				  `unitdetails_name` int NOT NULL,
				  `cnst_cd` tinyint DEFAULT NULL,
				  `incorporation_dt` date DEFAULT NULL,
				  `kzn_cd` tinyint DEFAULT NULL,
				  `ut_zone` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
				  `size_cd` int DEFAULT NULL,
				  `unit_pan` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
				  `unit_gstin` varchar(15) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
				  `ind_cd` int DEFAULT NULL,
				  PRIMARY KEY (`idm_ut_rowid`),
				  KEY `offc_cd` (`offc_cd`),
				  KEY `cnst_cd` (`cnst_cd`),
				  KEY `kzn_cd` (`kzn_cd`),
				  KEY `size_cd` (`size_cd`),
				  KEY `ind_cd` (`ind_cd`),
				  KEY `idm_unit_details_idfk_7_idx` (`loan_acc`),
				  KEY `idm_unit_details_idfk_11` (`unitdetails_name`),
				  CONSTRAINT `idm_unit_details_ibfk_2` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`),
				  CONSTRAINT `idm_unit_details_ibfk_3` FOREIGN KEY (`cnst_cd`) REFERENCES `cnst_cdtab` (`CNST_CD`),
				  CONSTRAINT `idm_unit_details_ibfk_4` FOREIGN KEY (`kzn_cd`) REFERENCES `kzn_cdtab` (`KZN_CD`),
				  CONSTRAINT `idm_unit_details_ibfk_5` FOREIGN KEY (`size_cd`) REFERENCES `tbl_size_cdtab` (`size_cd`),
				  CONSTRAINT `idm_unit_details_ibfk_6` FOREIGN KEY (`ind_cd`) REFERENCES `tbl_ind_cdtab` (`ind_cd`),
				  CONSTRAINT `idm_unit_details_idfk_11` FOREIGN KEY (`unitdetails_name`) REFERENCES `tbl_unit_mast` (`ut_rowid`),
				  CONSTRAINT `idm_unit_details_idfk_7` FOREIGN KEY (`loan_acc`) REFERENCES `tbl_app_loan_mast` (`ln_no`)
				);
select 'idm_unit_details Table Created' as '';  
            end;
		END IF;
END;



Begin
IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'vil_cdtab')

		-- If exists, retreive columns information from that table
		THEN		 	
            select 'vil_cdtab Table Already Exist' as '';                     
            else
            
             begin
				CREATE TABLE `vil_cdtab` (
				  `VIL_CD` int NOT NULL AUTO_INCREMENT,
				  `VIL_NAM` varchar(25) DEFAULT NULL COMMENT 'Village Name',
				  `HOB_CD` int DEFAULT NULL COMMENT 'Hobli Code Ref: hob_cdtab (hob_cd)',
				  `VIL_NAME_KANNADA` varchar(50) DEFAULT NULL COMMENT 'Village Name in Kannada',
				  `VIL_LGDCODE` decimal(20,0) DEFAULT NULL COMMENT 'LGD code from KSRSAC',
				  `VIL_BHOOMICODE` decimal(20,0) DEFAULT NULL COMMENT 'LGD code from Bhoomi Project',
				  `CONSTMLA_CD` smallint DEFAULT NULL COMMENT 'MLA Constituency Code Ref: constmla_cdtab (constmla_cd)',
				  `CONSTMP_CD` tinyint DEFAULT NULL COMMENT 'MP Constiutency Code Ref: constmp_cdtab (constmp_cd)',
				  `is_active` bit(1) DEFAULT NULL,
				  `is_deleted` bit(1) DEFAULT NULL,
				  `created_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
				  `modified_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
				  `created_date` datetime DEFAULT NULL,
				  `modified_date` datetime DEFAULT NULL,
				  `unique_id` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
				  PRIMARY KEY (`VIL_CD`),
				  KEY `fk_VIL_CDTAB_CONSTMLA_CDTAB` (`CONSTMLA_CD`),
				  KEY `fk_VIL_CDTAB_CONSTMP_CDTAB` (`CONSTMP_CD`),
				  CONSTRAINT `fk_VIL_CDTAB_CONSTMLA_CDTAB` FOREIGN KEY (`CONSTMLA_CD`) REFERENCES `constmla_cdtab` (`CONSTMLA_CD`),
				  CONSTRAINT `fk_VIL_CDTAB_CONSTMP_CDTAB` FOREIGN KEY (`CONSTMP_CD`) REFERENCES `constmp_cdtab` (`CONSTMP_CD`)
				) ENGINE=InnoDB AUTO_INCREMENT=42 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='Village Details';
select 'vil_cdtab Table Created' as '';  
            end;
		END IF;
END;



Begin
IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_pincode_state_cdtab')

		-- If exists, retreive columns information from that table
		THEN		 	
            select 'tbl_pincode_state_cdtab Table Already Exist' as '';                     
            else
            
             begin
				CREATE TABLE `tbl_pincode_state_cdtab` (
					  `pincode_state_cd` int NOT NULL AUTO_INCREMENT,
					  `pincode_state_desc` varchar(200) DEFAULT NULL,
					  `is_active` bit(1) DEFAULT NULL,
					  `is_deleted` bit(1) DEFAULT NULL,
					  `created_by` varchar(50) DEFAULT NULL,
					  `modified_by` varchar(50) DEFAULT NULL,
					  `created_date` datetime DEFAULT NULL,
					  `modified_date` datetime DEFAULT NULL,
					  PRIMARY KEY (`pincode_state_cd`)
					);
select 'tbl_pincode_state_cdtab Table Created' as '';  
            end;
		END IF;
END;


Begin
IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_pincode_district_cdtab')

		-- If exists, retreive columns information from that table
		THEN		 	
            select 'tbl_pincode_district_cdtab Table Already Exist' as '';                     
            else
            
             begin
				CREATE TABLE `tbl_pincode_district_cdtab` (
					  `pincode_district_cd` int NOT NULL AUTO_INCREMENT,
					  `pincode_district_desc` varchar(200) DEFAULT NULL,
					  `pincode_state_cd` int NOT NULL,
					  `is_active` bit(1) DEFAULT NULL,
					  `is_deleted` bit(1) DEFAULT NULL,
					  `created_by` varchar(50) DEFAULT NULL,
					  `modified_by` varchar(50) DEFAULT NULL,
					  `created_date` datetime DEFAULT NULL,
					  `modified_date` datetime DEFAULT NULL,
					  `DIST_CD` tinyint DEFAULT NULL,
					  PRIMARY KEY (`pincode_district_cd`),
					  KEY `fk_tbl_pincode_district_cdtab_tbl_pincode_state_cdtab` (`pincode_state_cd`),
					  CONSTRAINT `fk_tbl_pincode_district_cdtab_tbl_pincode_state_cdtab` FOREIGN KEY (`pincode_state_cd`) REFERENCES `tbl_pincode_state_cdtab` (`pincode_state_cd`)
					) ;
select 'tbl_pincode_district_cdtab Table Created' as '';  
            end;
		END IF;
END;


Begin
IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_pincode_master')

		-- If exists, retreive columns information from that table
		THEN		 	
            select 'tbl_pincode_master Table Already Exist' as '';                     
            else
            
             begin
				CREATE TABLE `tbl_pincode_master` (
				  `pincode_rowid` int NOT NULL AUTO_INCREMENT,
				  `pincode_cd` int NOT NULL,
				  `pincode_state` varchar(200) DEFAULT NULL,
				  `pincode_district` varchar(200) DEFAULT NULL,
				  `pincode_district_cd` int DEFAULT NULL,
				  `is_active` bit(1) DEFAULT NULL,
				  `is_deleted` bit(1) DEFAULT NULL,
				  `created_by` varchar(50) DEFAULT NULL,
				  `modified_by` varchar(50) DEFAULT NULL,
				  `created_date` datetime DEFAULT NULL,
				  `modified_date` datetime DEFAULT NULL,
				  PRIMARY KEY (`pincode_rowid`),
				  UNIQUE KEY `pincode_cd` (`pincode_cd`)
				)  ;
select 'tbl_pincode_master Table Created' as '';  
            end;
		END IF;
END;




END$$

DELIMITER ;

call ChangeOfUnit_SP();
