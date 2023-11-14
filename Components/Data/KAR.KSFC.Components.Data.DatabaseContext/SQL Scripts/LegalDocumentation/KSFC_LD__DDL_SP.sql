USE `ksfc_oct`;
DROP procedure IF EXISTS `LD_Script`;

DELIMITER $$
USE `ksfc_oct`$$
CREATE PROCEDURE `LD_Script` ()

BEGIN
DECLARE DBName varchar(50) DEFAULT "ksfc_oct";
-- -----------------------GuarantotDeed Module ----------------------------------
						-- tbl_charge_type_cdtab--
		Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_charge_type_cdtab')

		-- If exists, retreive columns information from that table
		THEN
		
		select 'tbl_charge_type_cdtab table already exist ' as ' ';
		else
			CREATE TABLE tbl_charge_type_cdtab (
			  chrg_type_cd int NOT NULL AUTO_INCREMENT,  
			  chrg_type_dets varchar(150) DEFAULT NULL,
			  chrg_type_dis_seq int,
			  is_active bit(1) DEFAULT NULL,
			  is_deleted bit(1) DEFAULT NULL,  
			  created_by varchar(50) DEFAULT NULL,
			  modified_by varchar(50) DEFAULT NULL,
			  created_date datetime DEFAULT NULL,
			  modified_date datetime DEFAULT NULL,
			  PRIMARY KEY (chrg_type_cd)
			);
			select 'tbl_charge_type_cdtab table Created' as ' ';
		END IF;
		END;
        
         -- -------------- tbl_idm_guar_deed_det-------------------------------
        
         Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_guar_deed_det')

		-- If exists, retreive columns information from that table
		THEN
		 
		 select 'tbl_idm_guar_deed_det table already exist' as ' ';
				IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_guar_deed_det' AND column_name='app_guarasset_id')
						THEN
								 select 'Column app_guarasset_id in tbl_idm_guar_deed_det table already exist' as ' ';
						ELSE
							ALTER TABLE `tbl_idm_guar_deed_det` 
							ADD COLUMN `app_guarasset_id` INT NULL after promoter_code;
						 select 'Column app_guarasset_id in tbl_idm_guar_deed_det table Created' as ' ';
				end if;
			
		END IF;
		END;
        
        -- ----------------tbl_app_guarnator-----------------------------------
        Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_app_guarnator')

		-- If exists, retreive columns information from that table
		THEN
		 select 'tbl_app_guarnator table already exist' as ' ';
		
		-- ALTER TABLE tbl_app_guarnator MODIFY column guar_mobile_no varchar(10);
			
		END IF;
		END;
        
        -- -------------------Condition Module---------------------------------
        -- ---------------- tbl_cond_stg_cdtab --------------------------------
        Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_cond_stg_cdtab')

		-- If exists, retreive columns information from that table
		THEN
		select 'tbl_cond_stg_cdtab table already exist' as ' ';
		
		else
			CREATE TABLE `tbl_cond_stg_cdtab` (
			  cond_stg_cd tinyint NOT NULL AUTO_INCREMENT,  
			  cond_stg_dets varchar(150) DEFAULT NULL,
			  cond_stg_dis_seq int,
			  is_active bit(1) DEFAULT NULL,
			  is_deleted bit(1) DEFAULT NULL,  
			  created_by varchar(50) DEFAULT NULL,
			  modified_by varchar(50) DEFAULT NULL,
			  created_date datetime DEFAULT NULL,
			  modified_date datetime DEFAULT NULL,
			  PRIMARY KEY (cond_stg_cd)
			);
			select 'tbl_cond_stg_cdtab table Created' as ' ';
		END IF;
		END;
        -- -------------------- tbl_cond_type_cdtab -----------------------------
        
        Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_cond_type_cdtab')

		-- If exists, retreive columns information from that table
		THEN
		 select 'tbl_cond_type_cdtab table already exist' as ' ';
		
		else
			CREATE TABLE `tbl_cond_type_cdtab` (
			  cond_type_cd tinyint NOT NULL AUTO_INCREMENT,  
			  cond_type_dets varchar(150) DEFAULT NULL,
			  cond_type_dis_seq int,
			  is_active bit(1) DEFAULT NULL,
			  is_deleted bit(1) DEFAULT NULL,  
			  created_by varchar(50) DEFAULT NULL,
			  modified_by varchar(50) DEFAULT NULL,
			  created_date datetime DEFAULT NULL,
			  modified_date datetime DEFAULT NULL,
			  PRIMARY KEY (cond_type_cd)
			);
			 select 'tbl_cond_type_cdtab table Created' as ' ';
		END IF;
		END;
        
        -- ------------------ tbl_cond_det_cdtab -----------------------------
        
         Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_cond_det_cdtab')

		-- If exists, retreive columns information from that table
		THEN
		 select 'tbl_cond_det_cdtab table Already Exist' as ' ';
		
		else
			CREATE TABLE `tbl_cond_det_cdtab` (
			  cond_det_cd int NOT NULL AUTO_INCREMENT,  
			  cond_det_dets varchar(150) DEFAULT NULL,
			  cond_det_dis_seq int,
			  is_active bit(1) DEFAULT NULL,
			  is_deleted bit(1) DEFAULT NULL,  
			  created_by varchar(50) DEFAULT NULL,
			  modified_by varchar(50) DEFAULT NULL,
			  created_date datetime DEFAULT NULL,
			  modified_date datetime DEFAULT NULL,
			  PRIMARY KEY (cond_det_cd)
			);
			 select 'tbl_cond_det_cdtab table Created' as ' ';
		END IF;
		END;
        
        -- -------------------- tbl_idm_cond_det ---------------------------
        
         Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_cond_det')

		-- If exists, retreive columns information from that table
		THEN
		  select 'tbl_idm_cond_det table Already Exist' as ' ';
				Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_cond_det' AND column_name='unique_id')
						THEN
								  select 'Column Unique ID in tbl_idm_cond_det  already exist' as ' ';
						ELSE
							ALTER TABLE `tbl_idm_cond_det`
					Add `unique_id` varchar(150) DEFAULT NULL;
						select 'Column Unique ID in tbl_idm_cond_det  Created' as ' ';
				end if;
            end;
			Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_cond_det' AND column_name='compliance')
						THEN
						select 'Column compliance already Exists in tbl_idm_cond_det table' as ' ';
						ELSE
                       ALTER TABLE `tbl_idm_cond_det` 
								ADD COLUMN `compliance` varchar(50) DEFAULT NULl AFTER `cond_details`;
							  select 'Column compliance in tbl_idm_cond_det table Created' as ' ';
					end if;
			    END;
           Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_cond_det' AND column_name='wh_relaxation')
						THEN
						select 'Column wh_relaxation already Exists in tbl_idm_cond_det table' as ' ';
						ELSE
                       ALTER TABLE `tbl_idm_cond_det` 
								ADD COLUMN `wh_relaxation`  bit(1) DEFAULT NULL AFTER `cond_remarks`;
							  select 'Column wh_relaxation in tbl_idm_cond_det table Created' as ' ';
					end if;
			    END;
           Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_cond_det' AND column_name='wh_rel_allowed')
						THEN
						select 'Column wh_rel_allowed already Exists in tbl_idm_cond_det table' as ' ';
						ELSE
                       ALTER TABLE `tbl_idm_cond_det` 
								ADD COLUMN `wh_rel_allowed`  bit(1) DEFAULT NULL AFTER `wh_relaxation`;
							  select 'Column wh_rel_allowed in tbl_idm_cond_det table Created' as ' ';
					end if;
			    END;
            begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_cond_det' AND column_name='loan_acc')
						THEN
						ALTER TABLE tbl_idm_cond_det MODIFY column loan_acc bigint; 
								  select 'Column loan_acc ID in tbl_idm_cond_det  Modified' as ' ';
						ELSE
							
						select 'Column cond_det_id in tbl_idm_cond_det  Created' as ' ';
				end if;
            end;
            else
             begin
				CREATE TABLE `tbl_idm_cond_det` (
				  `cond_det_id` bigint NOT NULL AUTO_INCREMENT,
				  `loan_acc` bigint DEFAULT NULL,
				  `loan_sub` int DEFAULT NULL,
				  `offc_cd` tinyint DEFAULT NULL,
				  `cond_type` tinyint DEFAULT NULL,
				  `cond_cd` smallint DEFAULT NULL,
				  `cond_stg` tinyint DEFAULT NULL,
				  `cond_details` varchar(200) DEFAULT NULL,
				   `compliance` varchar(50) DEFAULT NULL,
				  `cond_remarks` varchar(200) DEFAULT NULL,
				  `wh_relaxation` bit(1) DEFAULT NULL,
				  `wh_rel_allowed` bit(1) DEFAULT NULL,
				  `cond_upload` varchar(100) DEFAULT NULL,
				  is_active bit(1) DEFAULT NULL,
				  is_deleted bit(1) DEFAULT NULL,  
				  created_by varchar(50) DEFAULT NULL,
				  modified_by varchar(50) DEFAULT NULL,
				  created_date datetime DEFAULT NULL,
				  modified_date datetime DEFAULT NULL,
				  unique_id varchar(150) DEFAULT NULL,
				  PRIMARY KEY (`cond_det_id`),
				  KEY `loan_acc` (`loan_acc`),
				  KEY `offc_cd` (`offc_cd`),
				  KEY `cond_type` (`cond_type`),
				  KEY `cond_stg` (`cond_stg`),
				  CONSTRAINT `tbl_idm_cond_det_ibfk_1` FOREIGN KEY (`loan_acc`) REFERENCES `tbl_app_loan_mast` (`ln_no`),
				  CONSTRAINT `tbl_idm_cond_det_ibfk_2` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`),
				  CONSTRAINT `tbl_idm_cond_det_ibfk_3` FOREIGN KEY (`cond_type`) REFERENCES `tbl_cond_type_cdtab` (`cond_type_cd`),
				  CONSTRAINT `tbl_idm_cond_det_ibfk_4` FOREIGN KEY (`cond_stg`) REFERENCES `tbl_cond_stg_cdtab` (`cond_stg_cd`)
				);
				select 'tbl_idm_cond_det table Created' as ' ';
            end;
		END IF;
		END;
        
	-- -----------------------------Security Charge --------------------------------------------------

	-- -------------------------------tbl_unit_mast---------------------------
Begin
IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_unit_mast')

		-- If not exists, creat a new table
Then
select 'tbl_unit_mast Table Exist' as ''; 
ELSE
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
);
select 'tbl_unit_mast Table Created' as ''; 
END IF;
END;
	-- ------------------ tbl_pjsec_cdtab -----------------------------
        
         Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_pjsec_cdtab')

		-- If exists, retreive columns information from that table
		THEN
		 select 'tbl_pjsec_cdtab table Already Exist' as ' ';
		
		else
			CREATE TABLE `tbl_pjsec_cdtab` (
			  `sec_code` mediumint NOT NULL AUTO_INCREMENT,
			  `sec_dets` varchar(50) DEFAULT NULL,
			  `sec_flg` tinyint DEFAULT NULL,
			  `is_active` bit(1) DEFAULT NULL,
			  `is_deleted` bit(1) DEFAULT NULL,
			  `created_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
			  `modified_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
			  `created_date` datetime DEFAULT NULL,
			  `modified_date` datetime DEFAULT NULL,
			  `unique_id` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
			  PRIMARY KEY (`sec_code`)
			);
			 select 'tbl_pjsec_cdtab table Created' as ' ';
		END IF;
		END;
	
	-- ------------------ tbl_sec_cdtab -----------------------------
        
         Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_sec_cdtab')

		-- If exists, retreive columns information from that table
		THEN
		 select 'tbl_sec_cdtab table Already Exist' as ' ';
		
		else
				CREATE TABLE `tbl_sec_cdtab` (
				  `sec_cd` smallint NOT NULL AUTO_INCREMENT,
				  `sec_dets` varchar(150) DEFAULT NULL,
				  `sec_ty` tinyint DEFAULT NULL,
				  `is_active` bit(1) DEFAULT NULL,
				  `is_deleted` bit(1) DEFAULT NULL,
				  `created_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
				  `modified_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
				  `created_date` datetime DEFAULT NULL,
				  `modified_date` datetime DEFAULT NULL,
				  `unique_id` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
				  PRIMARY KEY (`sec_cd`)
				);
			 select 'tbl_sec_cdtab table Created' as ' ';
		END IF;
		END;

	 -- ------------------ tbl_security_refno_mast -----------------------------
        
         Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_security_refno_mast')

		-- If exists, retreive columns information from that table
		THEN
		 select 'tbl_security_refno_mast table Already Exist' as ' ';
		
		else
			CREATE TABLE `tbl_security_refno_mast` (
			  `sec_refno_mast_id` int NOT NULL AUTO_INCREMENT,
			  `security_cd` int NOT NULL,
			  `loan_acc` bigint NOT NULL,
			  `loan_sub` int DEFAULT NULL,
			  `offc_cd` tinyint DEFAULT NULL,
			  `sec_cd` smallint DEFAULT NULL,
			  `pjsec_cd` mediumint DEFAULT NULL,
			  `security_details` varchar(200) DEFAULT NULL,
			  `security_value` decimal(15,2) DEFAULT NULL,
			  `sec_name_holder` varchar(200) DEFAULT NULL,
			  `is_active` bit(1) DEFAULT NULL,
			  `is_deleted` bit(1) DEFAULT NULL,
			  `created_by` varchar(50) DEFAULT NULL,
			  `modified_by` varchar(50) DEFAULT NULL,
			  `created_date` datetime DEFAULT NULL,
			  `modified_date` datetime DEFAULT NULL,
			  `unique_id` varchar(200) DEFAULT NULL,
			  `wh_charge` bit(1) DEFAULT NULL,
			  PRIMARY KEY (`sec_refno_mast_id`),
			  UNIQUE KEY `security_cd` (`security_cd`),
			  KEY `fk_tbl_security_refno_mast_tbl_app_loan_mast` (`loan_acc`),
			  KEY `fk_tbl_security_refno_mast_offc_cdtab` (`offc_cd`),
			  KEY `fk_tbl_security_refno_mast_tbl_sec_cdtab` (`sec_cd`),
			  KEY `fk_tbl_security_refno_mast_tbl_pjsec_cdtab` (`pjsec_cd`),
			  CONSTRAINT `fk_tbl_security_refno_mast_offc_cdtab` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`),
			  CONSTRAINT `fk_tbl_security_refno_mast_tbl_app_loan_mast` FOREIGN KEY (`loan_acc`) REFERENCES `tbl_app_loan_mast` (`ln_no`),
			  CONSTRAINT `fk_tbl_security_refno_mast_tbl_pjsec_cdtab` FOREIGN KEY (`pjsec_cd`) REFERENCES `tbl_pjsec_cdtab` (`sec_code`),
			  CONSTRAINT `fk_tbl_security_refno_mast_tbl_sec_cdtab` FOREIGN KEY (`sec_cd`) REFERENCES `tbl_sec_cdtab` (`sec_cd`)
			);
			 select 'tbl_security_refno_mast table Created' as ' ';
		END IF;
		END;
		 -- ------------------ tlq_cdtab -----------------------------
        
         Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tlq_cdtab')

		-- If exists, retreive columns information from that table
		THEN
		 select 'tlq_cdtab table Already Exist' as ' ';
		
		else
			CREATE TABLE `tlq_cdtab` (
			  `TLQ_CD` int NOT NULL,
			  `TLQ_NAM` varchar(25) DEFAULT NULL COMMENT 'Taluka Name',
			  `DIST_CD` tinyint DEFAULT NULL COMMENT 'District Code Ref: dist_cdtab (dist_cd)',
			  `TLQ_INDZONE` tinyint DEFAULT NULL,
			  `TLQ_NAME_KANNADA` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
			  `TLQ_LGDCODE` int DEFAULT NULL,
			  `TLQ_BHOOMICODE` int DEFAULT NULL,
			  `is_active` bit(1) DEFAULT NULL,
			  `is_deleted` bit(1) DEFAULT NULL,
			  `created_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
			  `modified_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
			  `created_date` datetime DEFAULT NULL,
			  `modified_date` datetime DEFAULT NULL,
			  `unique_id` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
			  PRIMARY KEY (`TLQ_CD`),
			  KEY `fk_TLQ_CDTAB_DIST_CDTAB` (`DIST_CD`),
			  CONSTRAINT `fk_TLQ_CDTAB_DIST_CDTAB` FOREIGN KEY (`DIST_CD`) REFERENCES `dist_cdtab` (`DIST_CD`)
			);
			 select 'tlq_cdtab table Created' as ' ';
		END IF;
		END;

	 -- ------------------ tbl_subregistrar_mast -----------------------------
        
         Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_subregistrar_mast')

		-- If exists, retreive columns information from that table
		THEN
		 select 'tbl_subregistrar_mast table Already Exist' as ' ';
		
		else
			CREATE TABLE `tbl_subregistrar_mast` (
			  `sr_office_id` bigint NOT NULL AUTO_INCREMENT,
			  `subregistrar_cd` int NOT NULL,
			  `sr_code` varchar(100) DEFAULT NULL,
			  `sr_office_name` varchar(200) DEFAULT NULL,
			  `dist_cd` tinyint DEFAULT NULL,
			  `tlq_cd` int DEFAULT NULL,
			  `sr_oth_details` varchar(500) DEFAULT NULL,
			  `is_active` bit(1) DEFAULT NULL,
			  `is_deleted` bit(1) DEFAULT NULL,
			  `created_by` varchar(50) DEFAULT NULL,
			  `modified_by` varchar(50) DEFAULT NULL,
			  `created_date` datetime DEFAULT NULL,
			  `modified_date` datetime DEFAULT NULL,
			  `unique_id` varchar(200) DEFAULT NULL,
			  PRIMARY KEY (`sr_office_id`),
			  UNIQUE KEY `subregistrar_cd` (`subregistrar_cd`),
			  KEY `fk_tbl_subregistrar_mast_dist_cdtab` (`dist_cd`),
			  KEY `fk_tbl_subregistrar_mast_tlq_cdtab` (`tlq_cd`),
			  CONSTRAINT `fk_tbl_subregistrar_mast_dist_cdtab` FOREIGN KEY (`dist_cd`) REFERENCES `dist_cdtab` (`DIST_CD`),
			  CONSTRAINT `fk_tbl_subregistrar_mast_tlq_cdtab` FOREIGN KEY (`tlq_cd`) REFERENCES `tlq_cdtab` (`TLQ_CD`)
			);
			 select 'tbl_subregistrar_mast table Created' as ' ';
		END IF;
		END;
	


		 -- ------------------ tbl_pclas_cdtab -----------------------------
        
         Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_pclas_cdtab')

		-- If exists, retreive columns information from that table
		THEN
		 select 'tbl_pclas_cdtab table Already Exist' as ' ';
		
		else
			CREATE TABLE `tbl_pclas_cdtab` (
			  `pclas_cd` int NOT NULL,
			  `pclas_dets` varchar(30) DEFAULT NULL,
			  `id` int NOT NULL AUTO_INCREMENT,
			  `is_active` bit(1) DEFAULT NULL,
			  `is_deleted` bit(1) DEFAULT NULL,
			  `unique_id` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
			  `created_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
			  `modified_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
			  PRIMARY KEY (`id`),
			  UNIQUE KEY `uq_tbl_pclas_cdtab` (`pclas_cd`)
			);
			 select 'tbl_pclas_cdtab table Created' as ' ';
		END IF;
		END;
	
	
		 -- ------------------ tbl_psubclas_cdtab -----------------------------
        
         Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_psubclas_cdtab')

		-- If exists, retreive columns information from that table
		THEN
		 select 'tbl_psubclas_cdtab table Already Exist' as ' ';
		
		else
				CREATE TABLE `tbl_psubclas_cdtab` (
				  `psubclas_cd` int NOT NULL AUTO_INCREMENT,
				  `psubclas_desc` varchar(200) DEFAULT NULL,
				  `pclas_cd` int NOT NULL,
				  `is_active` bit(1) DEFAULT NULL,
				  `is_deleted` bit(1) DEFAULT NULL,
				  `created_by` varchar(50) DEFAULT NULL,
				  `modified_by` varchar(50) DEFAULT NULL,
				  `created_date` datetime DEFAULT NULL,
				  `modified_date` datetime DEFAULT NULL,
				  PRIMARY KEY (`psubclas_cd`),
				  KEY `fk_tbl_psubclas_cdtab_tbl_pclas_cdtab` (`pclas_cd`),
				  CONSTRAINT `fk_tbl_psubclas_cdtab_tbl_pclas_cdtab` FOREIGN KEY (`pclas_cd`) REFERENCES `tbl_pclas_cdtab` (`id`)
				);
			 select 'tbl_psubclas_cdtab table Created' as ' ';
		END IF;
		END;

		-- -------------------tbl_idm_deed_det--------------------------------
		Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_deed_det')

         -- If not exists ,create a new table
		THEN
		 select 'tbl_idm_deed_det table Already Exist' as ' ';
		    Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_deed_det' AND column_name='ut_cd')
					THEN
								select 'Column ut_cd in tbl_idm_deed_det  Already Exist' as ' ';
					ELSE
							ALTER TABLE `tbl_idm_deed_det` 
							Add `ut_cd`  int DEFAULT NULL;
						select 'Column ut_cd in  tbl_idm_deed_det  Created' as ' ';
					end if;
			END;
             Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_deed_det' AND column_name='pjsec_nam')
					THEN
								select 'Column pjsec_nam in  tbl_idm_deed_det  Already Exist' as ' ';
					ELSE
							ALTER TABLE `tbl_idm_deed_det` 
							Add `pjsec_nam`  varchar(100)  DEFAULT NULL;
						select 'Column pjsec_nam  in tbl_idm_deed_det  Created' as ' ';
					end if;
			END;
             Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_deed_det' AND column_name='pjsec_dets')
					THEN
								select 'Column pjsec_dets in tbl_idm_deed_det  Already Exist' as ' ';
					ELSE
							ALTER TABLE `tbl_idm_deed_det` 
							Add `pjsec_dets`  varchar(100)  DEFAULT NULL;
						select 'Column pjsec_dets in  tbl_idm_deed_det  Created' as ' ';
					end if;
			END;
			Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_deed_det' AND column_name='pjsec_rel')
					THEN
								select 'Column pjsec_rel in  tbl_idm_deed_det  Already Exist' as ' ';
					ELSE
							ALTER TABLE `tbl_idm_deed_det` 
							Add `pjsec_rel`  int DEFAULT NULL;
						select 'Column pjsec_rel  in tbl_idm_deed_det  Created' as ' ';
					end if;
			END;
			Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_deed_det' AND column_name='pjsec_dets_cd')
					THEN
								select 'Column pjsec_dets_cd in  tbl_idm_deed_det  Already Exist' as ' ';
					ELSE
							ALTER TABLE `tbl_idm_deed_det` 
							Add `pjsec_dets_cd`  int DEFAULT NULL;
						select 'Column pjsec_dets_cd in tbl_idm_deed_det  Created' as ' ';
					end if;
			END;
			Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_deed_det' AND column_name='ut_slno')
					THEN
								select 'Column ut_slno in tbl_idm_deed_det  Already Exist' as ' ';
					ELSE
							ALTER TABLE `tbl_idm_deed_det` 
							Add `ut_slno`  int DEFAULT NULL;
						select 'Column ut_slno in  tbl_idm_deed_det  Created' as ' ';
					end if;
			END;



		else
				CREATE TABLE `tbl_idm_deed_det` (

				  `idm_deed_detid` int NOT NULL AUTO_INCREMENT,
                  `loan_acc` bigint DEFAULT NULL,
                  `loan_sub` int DEFAULT NULL,
                  `offc_cd` tinyint DEFAULT NULL,
                  `security_cd` int DEFAULT NULL,
                  `deed_no` varchar(20)  DEFAULT NULL,
                  `deed_desc` varchar(200)  DEFAULT NULL,
                  `security_value` decimal(10,2) DEFAULT NULL,
                 `subregistrar_cd` int DEFAULT NULL,
                 `execution_date` date DEFAULT NULL,
                 `deed_upload` varchar(300)  DEFAULT NULL,
                 `approved_emp_id` varchar(8)  DEFAULT NULL,
                 `is_active` bit(1) DEFAULT NULL,
                 `is_deleted` bit(1) DEFAULT NULL,
                 `created_by` varchar(50)  DEFAULT NULL,
                 `modified_by` varchar(50)  DEFAULT NULL,
                 `created_date` datetime DEFAULT NULL,
                 `modified_date` datetime DEFAULT NULL,
		         `ut_cd` int DEFAULT NULL,
		         `pjsec_nam`  varchar(100)  DEFAULT NULL,
		         `pjsec_dets`  varchar(100)  DEFAULT NULL,
		         `pjsec_dets_cd` int DEFAULT NULL,
		         `pjsec_rel` int DEFAULT NULL,
			     `ut_slno` int DEFAULT NULL,
		  
           PRIMARY KEY (`idm_deed_detid`),
            KEY `fk_tbl_idm_deed_det_offc_cdtab` (`offc_cd`),
             KEY `fk_tbl_idm_deed_det_tbl_app_loan_mast` (`loan_acc`),
            KEY `fk_tbl_idm_deed_det_tbl_security_refno_mast` (`security_cd`),
            KEY `fk_tbl_idm_deed_det_tbl_subregistrar_mast` (`subregistrar_cd`),
            KEY `fk_tbl_idm_deed_det_tbl_trg_employee` (`approved_emp_id`),
            CONSTRAINT `fk_tbl_idm_deed_det_offc_cdtab` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`),
            CONSTRAINT `fk_tbl_idm_deed_det_tbl_app_loan_mast` FOREIGN KEY (`loan_acc`) REFERENCES `tbl_app_loan_mast` (`ln_no`),
            CONSTRAINT `fk_tbl_idm_deed_det_tbl_security_refno_mast` FOREIGN KEY (`security_cd`) REFERENCES `tbl_security_refno_mast` (`security_cd`),
            CONSTRAINT `fk_tbl_idm_deed_det_tbl_subregistrar_mast` FOREIGN KEY (`subregistrar_cd`) REFERENCES `tbl_subregistrar_mast` (`subregistrar_cd`),
            CONSTRAINT `fk_tbl_idm_deed_det_tbl_trg_employee` FOREIGN KEY (`approved_emp_id`) REFERENCES `tbl_trg_employee` (`tey_ticket_num`)
   );
			 select 'tbl_idm_deed_det table Created' as ' ';
		END IF;
		END;

	-- ------------------------------tbl_tbl_idm_hypoth_det-----------
	Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_hypoth_det')

         -- If not exists ,create a new table
		THEN
		 select 'tbl_idm_hypoth_det table Already Exist' as ' ';
		    Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_hypoth_det' AND column_name='ut_cd')
					THEN
								select 'Column ut_cd in tbl_idm_hypoth_det  Already Exist' as ' ';
					ELSE
							ALTER TABLE `tbl_idm_hypoth_det` 
							Add `ut_cd`  int DEFAULT NULL;
						select 'Column ut_cd in  tbl_idm_hypoth_det  Created' as ' ';
					end if;
			END;
			Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_hypoth_det' AND column_name='asset_item_no')
					THEN
								select 'Column asset_item_no in tbl_idm_hypoth_det  Already Exist' as ' ';
					ELSE
							ALTER TABLE `tbl_idm_hypoth_det` 
							Add `asset_item_no` bigint DEFAULT NULL;
						select 'Column asset_item_no in  tbl_idm_hypoth_det  Created' as ' ';
					end if;
			END;
			Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_hypoth_det' AND column_name='asset_qty')
					THEN
								select 'Column asset_qty in tbl_idm_hypoth_det  Already Exist' as ' ';
					ELSE
							ALTER TABLE `tbl_idm_hypoth_det` 
							Add `asset_qty`  int DEFAULT NULL;
						select 'Column asset_qty in  tbl_idm_hypoth_det  Created' as ' ';
					end if;
			END;
			Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_hypoth_det' AND column_name='asset_name')
					THEN
								select 'Column asset_name in tbl_idm_hypoth_det  Already Exist' as ' ';
					ELSE
							ALTER TABLE `tbl_idm_hypoth_det` 
							Add `asset_name` varchar(600) DEFAULT NULL;
						select 'Column asset_name in  tbl_idm_hypoth_det  Created' as ' ';
					end if;
			END;
			Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_hypoth_det' AND column_name='asset_details')
					THEN
								select 'Column asset_details in tbl_idm_hypoth_det  Already Exist' as ' ';
					ELSE
							ALTER TABLE `tbl_idm_hypoth_det` 
							Add `asset_details`  varchar(600) DEFAULT NULL;
						select 'Column asset_details in  tbl_idm_hypoth_det  Created' as ' ';
					end if;
			END;
			Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_hypoth_det' AND column_name='ut_slno')
					THEN
								select 'Column ut_slno in tbl_idm_hypoth_det  Already Exist' as ' ';
					ELSE
							ALTER TABLE `tbl_idm_hypoth_det` 
							Add `ut_slno`  int DEFAULT NULL;
						select 'Column ut_slno in  tbl_idm_hypoth_det  Created' as ' ';
					end if;
			END;
			else
			CREATE TABLE `tbl_idm_hypoth_det` (
  `idm_hypoth_detid` int NOT NULL AUTO_INCREMENT,
  `loan_acc` bigint DEFAULT NULL,
  `loan_sub` int DEFAULT NULL,
  `offc_cd` tinyint DEFAULT NULL,
  `asset_cd` bigint DEFAULT NULL,
  `hypoth_no` varchar(20) DEFAULT NULL,
  `hypoth_desc` varchar(200) DEFAULT NULL,
  `asset_value` decimal(10,2) DEFAULT NULL,
  `execution_date` date DEFAULT NULL,
  `hypoth_upload` varchar(300) DEFAULT NULL,
  `approved_emp_id` varchar(8) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50)  DEFAULT NULL,
  `modified_by` varchar(50)  DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `ut_cd` int DEFAULT NULL,
  `asset_item_no` bigint DEFAULT NULL,
  `asset_qty` int DEFAULT NULL,
  `asset_name` varchar(600)DEFAULT NULL,
  `asset_details`varchar(600)DEFAULT NULL,
  `ut_slno` int DEFAULT NULL,
  PRIMARY KEY (`idm_hypoth_detid`),
  KEY `fk_tbl_idm_hypoth_det_offc_cdtab` (`offc_cd`),
  KEY `fk_tbl_idm_hypoth_det_tbl_app_loan_mast` (`loan_acc`),
  KEY `fk_tbl_idm_hypoth_det_tbl_asset_refno_det` (`asset_cd`),
  KEY `fk_tbl_idm_hypoth_det_tbl_idm_dsb_spl_cleg` (`approved_emp_id`),
  CONSTRAINT `fk_tbl_idm_hypoth_det_offc_cdtab` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`),
  CONSTRAINT `fk_tbl_idm_hypoth_det_tbl_app_loan_mast` FOREIGN KEY (`loan_acc`) REFERENCES `tbl_app_loan_mast` (`ln_no`),
  CONSTRAINT `fk_tbl_idm_hypoth_det_tbl_asset_refno_det` FOREIGN KEY (`asset_cd`) REFERENCES `tbl_asset_refno_det` (`asset_cd`),
  CONSTRAINT `fk_tbl_idm_hypoth_det_tbl_idm_dsb_spl_cleg` FOREIGN KEY (`approved_emp_id`) REFERENCES `tbl_trg_employee` (`tey_ticket_num`)
);

			 select 'tbl_idm_hypoth_det table Created' as ' ';
		END IF;
		END;


			-- ------------------------------tbl_idm_cersai_registration-----------
	Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_cersai_registration')

         -- If not exists ,create a new table
		THEN
		 select 'tbl_idm_cersai_registration table Already Exist' as ' ';

		  Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_cersai_registration' AND column_name='asset_details')
					THEN
								select 'Column asset_details in tbl_idm_cersai_registration  Already Exist' as ' ';
					ELSE
							ALTER TABLE `tbl_idm_cersai_registration` 
							Add `asset_details`  varchar(200) DEFAULT NULL;
						select 'Column asset_details in  tbl_idm_cersai_registration  Created' as ' ';
					end if;
			END;
			  Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_cersai_registration' AND column_name='asset_value')
					THEN
								select 'Column asset_value in tbl_idm_cersai_registration  Already Exist' as ' ';
					ELSE
							ALTER TABLE `tbl_idm_cersai_registration` 
							Add `asset_value` decimal(15,2)  DEFAULT NULL;
						select 'Column asset_value in  tbl_idm_cersai_registration  Created' as ' ';
					end if;
			END;
			 Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_cersai_registration' AND column_name='ut_cd')
					THEN
								select 'Column ut_cd in tbl_idm_cersai_registration  Already Exist' as ' ';
					ELSE
							ALTER TABLE `tbl_idm_cersai_registration` 
							Add `ut_cd` int DEFAULT NULL;
						select 'Column ut_cd in  tbl_idm_cersai_registration  Created' as ' ';
					end if;
			END;
			 Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_cersai_registration' AND column_name='assetcat_cd')
					THEN
								select 'Column assetcat_cd in tbl_idm_cersai_registration  Already Exist' as ' ';
					ELSE
							ALTER TABLE `tbl_idm_cersai_registration` 
							Add `assetcat_cd` int DEFAULT NULL;
						select 'Column assetcat_cd in  tbl_idm_cersai_registration  Created' as ' ';
					end if;
			END;
			Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_cersai_registration' AND column_name='assettype_cd')
					THEN
								select 'Column assettype_cd in tbl_idm_cersai_registration  Already Exist' as ' ';
					ELSE
							ALTER TABLE `tbl_idm_cersai_registration` 
							Add `assettype_cd` int DEFAULT NULL;
						select 'Column assettype_cd in  tbl_idm_cersai_registration  Created' as ' ';
					end if;
			END;
			Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_cersai_registration' AND column_name='asset_othdetails')
					THEN
								select 'Column asset_othdetails in tbl_idm_cersai_registration  Already Exist' as ' ';
					ELSE
							ALTER TABLE `tbl_idm_cersai_registration` 
							Add `asset_othdetails` varchar(500) DEFAULT NULL;
						select 'Column asset_othdetails in  tbl_idm_cersai_registration  Created' as ' ';
					end if;
			END;
			Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_cersai_registration' AND column_name='ut_slno')
					THEN
								select 'Column ut_slno in tbl_idm_cersai_registration  Already Exist' as ' ';
					ELSE
							ALTER TABLE `tbl_idm_cersai_registration` 
							Add `ut_slno` int DEFAULT NULL;
						select 'Column ut_slno in  tbl_idm_cersai_registration  Created' as ' ';
					end if;
			END;
else
     CREATE TABLE `tbl_idm_cersai_registration` (
  `idm_dsb_charge_id` int NOT NULL AUTO_INCREMENT,
  `loan_acc` bigint DEFAULT NULL,
  `loan_sub` int DEFAULT NULL,
  `offc_cd` tinyint DEFAULT NULL,
  `security_cd` int DEFAULT NULL,
  `asset_cd` bigint DEFAULT NULL,
  `cersai_reg_no` varchar(100)DEFAULT NULL,
  `cersai_reg_date` date DEFAULT NULL,
  `cersai_remarks` varchar(200)  DEFAULT NULL,
  `upload_document` varchar(300)  DEFAULT NULL,
  `approved_emp_id` varchar(8)  DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50)  DEFAULT NULL,
  `modified_by` varchar(50)  DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `asset_details` varchar(200)  DEFAULT NULL,
  `asset_value` decimal(15,2) DEFAULT NULL,
  `ut_cd` int DEFAULT NULL,
  `assetcat_cd` int DEFAULT NULL,
  `assettype_cd` int DEFAULT NULL,
  `asset_othdetails` varchar(500)  DEFAULT NULL,
  `ut_slno` int DEFAULT NULL,
  PRIMARY KEY (`idm_dsb_charge_id`),
  KEY `fk_tbl_idm_cersai_registration_offc_cdtab` (`offc_cd`),
  KEY `fk_tbl_idm_cersai_registration_tbl_app_loan_mast` (`loan_acc`),
  KEY `fk_tbl_idm_cersai_registration_tbl_security_refno_mast` (`security_cd`),
  KEY `fk_tbl_idm_cersai_registration_tbl_asset_refno_det` (`asset_cd`),
  KEY `fk_tbl_idm_cersai_registration_tbl_trg_employee` (`approved_emp_id`),
  CONSTRAINT `fk_tbl_idm_cersai_registration_offc_cdtab` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`),
  CONSTRAINT `fk_tbl_idm_cersai_registration_tbl_app_loan_mast` FOREIGN KEY (`loan_acc`) REFERENCES `tbl_app_loan_mast` (`ln_no`),
  CONSTRAINT `fk_tbl_idm_cersai_registration_tbl_asset_refno_det` FOREIGN KEY (`asset_cd`) REFERENCES `tbl_asset_refno_det` (`asset_cd`),
  CONSTRAINT `fk_tbl_idm_cersai_registration_tbl_security_refno_mast` FOREIGN KEY (`security_cd`) REFERENCES `tbl_security_refno_mast` (`security_cd`),
  CONSTRAINT `fk_tbl_idm_cersai_registration_tbl_trg_employee` FOREIGN KEY (`approved_emp_id`) REFERENCES `tbl_trg_employee` (`tey_ticket_num`)
);
 select 'tbl_idm_cersai_registration table Created' as ' ';
		END IF;
		END;

		-- ------------------ tbl_trg_employee -----------------------------
        
         Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_trg_employee')

		-- If exists, retreive columns information from that table
		THEN
		 select 'tbl_trg_employee table Already Exist' as ' ';
		
		else
				
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
			  `created_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
			  `modified_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
			  `created_date` datetime DEFAULT NULL,
			  `modified_date` datetime DEFAULT NULL,
			  `unique_id` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
			  PRIMARY KEY (`tey_ticket_num`),
			  UNIQUE KEY `uk_emp_mobile_no` (`emp_mobile_no`)
			);
			 select 'tbl_trg_employee table Created' as ' ';
		END IF;
		END;
     -- -----------------------------tbl_security_refno_mast-----------------------------------------
     
     Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_security_refno_mast')

		-- If exists, retreive columns information from that table
		THEN
		select 'tbl_security_refno_mast table Already Exist' as ' ';
			Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_security_refno_mast' AND column_name='wh_charge')
						THEN
								 select 'column wh_charge tbl_security_refno_mast table Already Exist' as ' ';
						ELSE
							ALTER TABLE tbl_security_refno_mast
							ADD wh_charge  bit(1) DEFAULT NULL;
						 select 'column wh_charge tbl_security_refno_mast table Created' as ' ';
				end if;
            end;
		END IF;
		END;
       -- --------------------------tbl_idm_dsb_charge-------------------------
       
       Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_dsb_charge')

		-- If exists, retreive columns information from that table
		THEN
		  select 'tbl_idm_dsb_charge table Already Exist' as ' ';
		Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dsb_charge' AND column_name='ut_cd')
					THEN
								select 'Column ut_cd in tbl_idm_dsb_charge  Already Exist' as ' ';
					ELSE
							ALTER TABLE `tbl_idm_dsb_charge` 
							Add `ut_cd`  int DEFAULT NULL;
						select 'Column ut_cd in  tbl_idm_dsb_charge  Created' as ' ';
					end if;
			END;
			Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dsb_charge' AND column_name='security_nam')
					THEN
								select 'Column security_nam in tbl_idm_dsb_charge  Already Exist' as ' ';
					ELSE
							ALTER TABLE `tbl_idm_dsb_charge` 
							Add `security_nam`  varchar(100) DEFAULT NULL;
						select 'Column security_nam in  tbl_idm_dsb_charge  Created' as ' ';
					end if;
			END;
			Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dsb_charge' AND column_name='security_dets')
					THEN
								select 'Column security_dets in tbl_idm_dsb_charge  Already Exist' as ' ';
					ELSE
							ALTER TABLE `tbl_idm_dsb_charge` 
							Add `security_dets`  varchar(100) DEFAULT NULL;
						select 'Column security_dets  in  tbl_idm_dsb_charge  Created' as ' ';
					end if;
			END;
			Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dsb_charge' AND column_name='security_dets_cd')
					THEN
								select 'Column security_dets_cd in tbl_idm_dsb_charge  Already Exist' as ' ';
					ELSE
							ALTER TABLE `tbl_idm_dsb_charge` 
							Add `security_dets_cd`  int DEFAULT NULL;
						select 'Column security_dets_cd in  tbl_idm_dsb_charge  Created' as ' ';
					end if;
			END;
			Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dsb_charge' AND column_name='security_rel')
					THEN
								select 'Column security_rel in tbl_idm_dsb_charge  Already Exist' as ' ';
					ELSE
							ALTER TABLE `tbl_idm_dsb_charge` 
							Add `security_rel`  int DEFAULT NULL;
						select 'Column security_rel in  tbl_idm_dsb_charge  Created' as ' ';
					end if;
			END;
			Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dsb_charge' AND column_name='ut_slno')
					THEN
								select 'Column ut_slno in tbl_idm_dsb_charge  Already Exist' as ' ';
					ELSE
							ALTER TABLE `tbl_idm_dsb_charge` 
							Add `ut_slno`  int DEFAULT NULL;
						select 'Column ut_slno in  tbl_idm_dsb_charge  Created' as ' ';
					end if;
			END;
			else
		
  CREATE TABLE `tbl_idm_dsb_charge` (
  `idm_dsb_charge_id` int NOT NULL AUTO_INCREMENT,
  `loan_acc` bigint DEFAULT NULL,
  `loan_sub` int DEFAULT NULL,
  `offc_cd` tinyint DEFAULT NULL,
  `security_cd` int DEFAULT NULL,
  `charge_type_cd` int DEFAULT NULL,
  `security_value` decimal(10,0) DEFAULT NULL,
  `noc_issue_by` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `noc_isssue_to` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `noc_date` date DEFAULT NULL,
  `auth_letter_by` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `auth_letter_date` date DEFAULT NULL,
  `board_resolution_date` date DEFAULT NULL,
  `moe_insured_date` date DEFAULT NULL,
  `request_ltr_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `request_ltr_date` date DEFAULT NULL,
  `bank_ifsc_cd` varchar(11) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `bank_request_ltr_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `bank_request_ltr_date` date DEFAULT NULL,
  `charge_purpose` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `charge_details` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `charge_conditions` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `upload_document` varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `approved_emp_id` varchar(8) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `modified_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `bank_ifsc_id_cd` int DEFAULT NULL,
  `ut_cd` int DEFAULT NULL,
  `security_nam` varchar(100) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `security_dets_cd` int DEFAULT NULL,
  `security_rel` int DEFAULT NULL,
  `ut_slno` int DEFAULT NULL,
  `security_dets` varchar(100) COLLATE utf8mb4_general_ci DEFAULT NULL,
  PRIMARY KEY (`idm_dsb_charge_id`),
  KEY `fk_tbl_idm_dsb_charge_offc_cdtab` (`offc_cd`),
  KEY `fk_tbl_idm_dsb_charge_tbl_app_loan_mast` (`loan_acc`),
  KEY `fk_tbl_idm_dsb_charge_tbl_security_refno_mast` (`security_cd`),
  KEY `fk_tbl_idm_dsb_charge_tbl_trg_employee` (`approved_emp_id`),
  KEY `fk_tbl_idm_dsb_charge_tbl_ifsc_master` (`bank_ifsc_cd`),
  KEY `fk_tbl_idm_dsb_charge_tbl_ifsc_master_id` (`bank_ifsc_id_cd`),
  CONSTRAINT `fk_tbl_idm_dsb_charge_offc_cdtab` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`),
  CONSTRAINT `fk_tbl_idm_dsb_charge_tbl_app_loan_mast` FOREIGN KEY (`loan_acc`) REFERENCES `tbl_app_loan_mast` (`ln_no`),
  CONSTRAINT `fk_tbl_idm_dsb_charge_tbl_ifsc_master` FOREIGN KEY (`bank_ifsc_cd`) REFERENCES `tbl_ifsc_master` (`ifsc_cd`),
  CONSTRAINT `fk_tbl_idm_dsb_charge_tbl_ifsc_master_id` FOREIGN KEY (`bank_ifsc_id_cd`) REFERENCES `tbl_ifsc_master` (`ifsc_rowid`),
  CONSTRAINT `fk_tbl_idm_dsb_charge_tbl_security_refno_mast` FOREIGN KEY (`security_cd`) REFERENCES `tbl_security_refno_mast` (`security_cd`),
  CONSTRAINT `fk_tbl_idm_dsb_charge_tbl_trg_employee` FOREIGN KEY (`approved_emp_id`) REFERENCES `tbl_trg_employee` (`tey_ticket_num`)
);
 select 'tbl_idm_dsb_charge table Created' as ' ';
		END IF;
		END;
	   						-- tbl_prom_cdtab --
Begin
IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_prom_cdtab')

		-- If not exists, creat a new table
Then

ALTER TABLE `tbl_prom_cdtab` 
CHANGE COLUMN `unique_id` `unique_id` VARCHAR(200) CHARACTER SET 'utf8mb4' NULL DEFAULT NULL ,
CHANGE COLUMN `created_by` `created_by` VARCHAR(50) CHARACTER SET 'utf8mb4' NULL DEFAULT NULL ,
CHANGE COLUMN `modified_by` `modified_by` VARCHAR(50) CHARACTER SET 'utf8mb4' NULL DEFAULT NULL ;
select 'tbl_prom_cdtab Table Modified' as ''; 
ELSE
CREATE TABLE `tbl_prom_cdtab` (
  `promoter_code` bigint NOT NULL AUTO_INCREMENT,
  `promoter_pan` varchar(11) DEFAULT NULL,
  `promoter_name` varchar(100) NOT NULL,
  `promoter_dob` date DEFAULT NULL,
  `promoter_gender` tinytext NOT NULL,
  `promoter_passport` varchar(10) NOT NULL,
  `promoter_photo` varchar(300) DEFAULT NULL,
  `pan_validation_date` date DEFAULT NULL,
  `promoter_emailid` varchar(50) DEFAULT NULL,
  `promoter_mobno` bigint DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `modified_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`promoter_code`)
);
select 'tbl_prom_cdtab Table Already Exist' as ''; 
END IF;
END;
	    -- -------------------- tbl_loan_type_cdtab ---------------------------
        
         Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_loan_type_cdtab')

		-- If exists, retreive columns information from that table
		THEN
		  select 'tbl_loan_type_cdtab table Already Exist' as ' ';
            else
             begin
					   CREATE TABLE `tbl_loan_type_cdtab` (
					  `loan_type` int NOT NULL AUTO_INCREMENT,
					  `loan_type_desc` varchar(200) DEFAULT NULL,
					  `is_active` bit(1) DEFAULT NULL,
					  `is_deleted` bit(1) DEFAULT NULL,
					  `created_by` varchar(50) DEFAULT NULL,
					  `modified_by` varchar(50) DEFAULT NULL,
					  `created_date` datetime DEFAULT NULL,
					  `modified_date` datetime DEFAULT NULL,
					  PRIMARY KEY (`loan_type`)
					);
				select 'tbl_loan_type_cdtab table Created' as ' ';
            end;
		END IF;
		END;

		 -- -------------------- tbl_trg_emp_grade ---------------------------
        
         Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_trg_emp_grade')

		-- If exists, retreive columns information from that table
		THEN
		  select 'tbl_trg_emp_grade table Already Exist' as ' ';
            else
             begin
			 CREATE TABLE `tbl_trg_emp_grade` (
			  `tges_code` varchar(5) NOT NULL,
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
			);
				select 'tbl_trg_emp_grade table Created' as ' ';
            end;
		END IF;
		END;

		 -- -------------------- tbl_asset_refno_det ---------------------------
        
         Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_asset_refno_det')

		-- If exists, retreive columns information from that table
		THEN
		  select 'tbl_asset_refno_det table Already Exist' as ' ';
		  	Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_asset_refno_det' AND column_name='wh_hyp')
						THEN
								  select 'Column wh_hyp in tbl_asset_refno_det  already exist' as ' ';
						ELSE
							ALTER TABLE `tbl_asset_refno_det`
					Add `wh_hyp` bit(1) DEFAULT NULL;
						select 'Column wh_hyp in tbl_asset_refno_det  Created' as ' ';
				end if;
            end;
			Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_asset_refno_det' AND column_name='wh_cersai')
						THEN
								  select 'Column wh_cersai in tbl_asset_refno_det  already exist' as ' ';
						ELSE
							ALTER TABLE `tbl_asset_refno_det`
					Add `wh_cersai` bit(1) DEFAULT NULL;
						select 'Column wh_cersai in tbl_asset_refno_det  Created' as ' ';
				end if;
            end;
            else
             begin
			 CREATE TABLE `tbl_asset_refno_det` (
			  `asset_refno_mast_id` bigint NOT NULL AUTO_INCREMENT,
			  `asset_cd` bigint NOT NULL,
			  `loan_acc` bigint NOT NULL,
			  `loan_sub` int DEFAULT NULL,
			  `offc_cd` tinyint DEFAULT NULL,
			  `assetcat_cd` int DEFAULT NULL,
			  `assettype_cd` int DEFAULT NULL,
			  `asset_details` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
			  `asset_value` decimal(15,2) DEFAULT NULL,
			  `asset_othdetails` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
			  `is_active` bit(1) DEFAULT NULL,
			  `is_deleted` bit(1) DEFAULT NULL,
			  `created_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
			  `modified_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
			  `created_date` datetime DEFAULT NULL,
			  `modified_date` datetime DEFAULT NULL,
			  `unique_id` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
			  `wh_hyp` bit(1) DEFAULT NULL,
			  `wh_cersai` bit(1) DEFAULT NULL,
			  PRIMARY KEY (`asset_refno_mast_id`),
			  UNIQUE KEY `asset_cd` (`asset_cd`),
			  KEY `fk_tbl_asset_refno_det_tbl_app_loan_mast` (`loan_acc`),
			  KEY `fk_tbl_asset_refno_det_offc_cdtab` (`offc_cd`),
			  KEY `fk_tbl_asset_refno_det_tbl_assetcat_cdtab` (`assetcat_cd`),
			  KEY `fk_tbl_asset_refno_det_tbl_assettype_cdtab` (`assettype_cd`),
			  CONSTRAINT `fk_tbl_asset_refno_det_offc_cdtab` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`),
			  CONSTRAINT `fk_tbl_asset_refno_det_tbl_app_loan_mast` FOREIGN KEY (`loan_acc`) REFERENCES `tbl_app_loan_mast` (`ln_no`),
			  CONSTRAINT `fk_tbl_asset_refno_det_tbl_assetcat_cdtab` FOREIGN KEY (`assetcat_cd`) REFERENCES `tbl_assetcat_cdtab` (`assetcat_cd`),
			  CONSTRAINT `fk_tbl_asset_refno_det_tbl_assettype_cdtab` FOREIGN KEY (`assettype_cd`) REFERENCES `tbl_assettype_cdtab` (`assettype_cd`)
);
				select 'tbl_asset_refno_det table Created' as ' ';
            end;
		END IF;
		END;
	
 
END$$

DELIMITER ;

Call LD_Script()