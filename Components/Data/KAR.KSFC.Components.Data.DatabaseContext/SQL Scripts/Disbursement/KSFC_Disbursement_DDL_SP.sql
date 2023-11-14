USE `ksfc_oct`;
DROP procedure IF EXISTS `KSFC_Disbursement_DDL_SP`;

DELIMITER $$
USE `ksfc_oct`$$
CREATE PROCEDURE `KSFC_Disbursement_DDL_SP` ()
BEGIN
	DECLARE DBName varchar(50) DEFAULT "ksfc_oct";
		Begin
			IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_prom_type_cdtab')

					 -- If not exists, creat a new table
			Then
				 select 'tbl_prom_type_cdtab Table Already Exist' as ' ';
			else
            CREATE TABLE tbl_prom_type_cdtab (
				  prom_type_cd tinyint NOT NULL AUTO_INCREMENT,  
				  prom_type_dets varchar(50) DEFAULT NULL,
				  prom_type_dis_seq int,
				  is_active bit(1) DEFAULT NULL,
				  is_deleted bit(1) DEFAULT NULL,  
				  created_by varchar(50) DEFAULT NULL,
				  modified_by varchar(50) DEFAULT NULL,
				  created_date datetime DEFAULT NULL,
				  modified_date datetime DEFAULT NULL,
				  PRIMARY KEY (prom_type_cd)
				);
				 select 'tbl_prom_type_cdtab Table Created' as ' ';
			END IF;
		END;
        -- tbl_idm_sidbi_approval -----------------------------------------
        Begin
			IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_sidbi_approval')

					 -- If not exists, creat a new table
			Then
             select 'tbl_idm_sidbi_approval Table Already Exist' as ' ';
			  Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_sidbi_approval' AND column_name='loan_acc')
					THEN
					ALTER TABLE `tbl_idm_sidbi_approval`
							Modify Column  `loan_acc` bigint DEFAULT NULL;
								select 'Column loan_acc in tbl_idm_sidbi_approval Modified' as ' ';
                               
					ELSE
							
						 select 'Column loan_acc in tbl_idm_sidbi_approval existed' as ' ';
					end if;
			    END;
				Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_sidbi_approval' AND column_name='prom_type_cd')
					THEN
								select 'Column prom_type_cd in tbl_idm_sidbi_approval Already Exist' as ' ';
					ELSE
							ALTER TABLE `tbl_idm_sidbi_approval`
							Add `prom_type_cd` tinyint NOT NULL;
						select 'Column prom_type_cd in tbl_idm_sidbi_approval Created' as ' ';
					end if;
			    END;
                
                Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND  TABLE_NAME='tbl_idm_sidbi_approval' AND column_name='is_active')
					THEN
								select 'Column is_active in tbl_idm_sidbi_approval Already Exist' as ' ';
					ELSE
							ALTER TABLE `tbl_idm_sidbi_approval`
							Add `is_active` bit(1) DEFAULT NULL;
						select 'Column is_active in tbl_idm_sidbi_approval Created' as ' ';
					end if;
			    END;
                
                Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_sidbi_approval' AND column_name='is_deleted')
					THEN
								select 'Column is_deleted in tbl_idm_sidbi_approval Already Exist' as ' ';
                               
					ELSE
							ALTER TABLE `tbl_idm_sidbi_approval`
							Add `is_deleted`  bit(1) DEFAULT NULL;
						 select 'Column is_deleted in tbl_idm_sidbi_approval Created' as ' ';
					end if;
			    END;
                
                Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_sidbi_approval' AND column_name='ln_sanc_amt')
					THEN
								select 'Column ln_sanc_amt in tbl_idm_sidbi_approval Already Exist' as ' ';
					ELSE
							ALTER TABLE `tbl_idm_sidbi_approval`
							Add `ln_sanc_amt`  decimal(15,2) DEFAULT NULL;
						 select 'Column ln_sanc_amt in tbl_idm_sidbi_approval Created' as ' ';
					end if;
			    END;
			
			else
				 CREATE TABLE `tbl_idm_sidbi_approval` (
                 `sidbi_appr_id` bigint NOT NULL,
                 `prom_type_cd` tinyint NOT NULL,
                 `loan_acc` bigint DEFAULT NULL,
                 `loan_sub` int DEFAULT NULL,
				 `ln_sanc_amt` decimal(15,2) DEFAULT NULL,
                 `offc_cd` tinyint DEFAULT NULL,
                 `wh_appr` bit(1) DEFAULT NULL,
                 `sidbi_upload` varchar(100) DEFAULT NULL,
                 `is_active` bit(1) DEFAULT NULL,
                 `is_deleted` bit(1) DEFAULT NULL,
				PRIMARY KEY (`sidbi_appr_id`),
                KEY `loan_acc` (`loan_acc`),
				KEY `offc_cd` (`offc_cd`),
                KEY `prom_type_cd` (`prom_type_cd`),
                CONSTRAINT `tbl_idm_sidbi_approval_ibfk_1` FOREIGN KEY (`loan_acc`) REFERENCES `tbl_app_loan_mast` (`ln_no`),
                CONSTRAINT `tbl_idm_sidbi_approval_ibfk_2` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`),
                CONSTRAINT `tbl_idm_sidbi_approval_ibfk_3` FOREIGN KEY (`prom_type_cd`) REFERENCES `tbl_prom_type_cdtab` (`prom_type_cd`)
				);
                 select 'tbl_idm_sidbi_approval Table Created' as ' ';
			END IF;
		END;
        -- -----------------------First investment Clause Module ----------------------------------
-- ---------------------- tbl_idm_dchg_fic --------------------
		BEGIN
			IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
					WHERE table_schema = DBName
					AND table_name LIKE 'tbl_idm_dchg_fic')

		-- If exists, retreive columns information from that table
		THEN
		 	 select 'tbl_idm_dchg_fic Table Already Exist' as ' ';
			 Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_fic' AND column_name='dcfic_unit')
					THEN
								 ALTER TABLE `tbl_idm_dchg_fic` RENAME COLUMN `dcfic_unit` TO `dcfic_loan_no`;
					end if;
			END;
			 Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_fic' AND column_name='dcfic_loan_no')
					THEN
					ALTER TABLE `tbl_idm_dchg_fic`
							Modify Column  `dcfic_loan_no` bigint DEFAULT NULL;
								select 'Column dcfic_loan_no in tbl_idm_dchg_fic Modified' as ' ';
                               
					ELSE
							
						 select 'Column dcfic_loan_no in tbl_idm_dchg_fic existed' as ' ';
					end if;
			    END;
           Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_fic' AND column_name='is_active')
					THEN
								  select 'Column is_active tbl_idm_dchg_fic  Already Exist' as ' ';
					ELSE
							ALTER TABLE `tbl_idm_dchg_fic` 
							Add `is_active` bit(1) DEFAULT NULL;
						select 'Column is_active tbl_idm_dchg_fic  Created' as ' ';
					end if;
			END;
            
            Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_fic' AND column_name='is_deleted')
					THEN
								select 'Column is_deleted tbl_idm_dchg_fic  Already Exist' as ' ';
					ELSE
							ALTER TABLE `tbl_idm_dchg_fic` 
							Add `is_deleted`  bit(1) DEFAULT NULL;
						select 'Column is_deleted tbl_idm_dchg_fic  Created' as ' ';
					end if;
			END;
            
            
            
		else
            
             begin 
             CREATE TABLE `tbl_idm_dchg_fic` (
			 `dcfic_id` bigint NOT NULL AUTO_INCREMENT,
			 `dcfic_offc` tinyint DEFAULT NULL,
			 `dcfic_loan_no` bigint DEFAULT NULL,
			 `dcfic_sno` int DEFAULT NULL,
			 `dcfic_rqdt` date DEFAULT NULL,
			 `dcfic_apdt` date DEFAULT NULL,
			 `dcfic_apau` date DEFAULT NULL,
			 `dcfic_amt` decimal(10,2) DEFAULT NULL,
			 `dcfic_comdt` date DEFAULT NULL,
			 `dcfic_amt_original` decimal(10,2) DEFAULT NULL,
			 `is_active` bit(1) DEFAULT NULL,
	    	 `is_deleted` bit(1) DEFAULT NULL,
			 `created_by` varchar(50) DEFAULT NULL,
			 `modified_by` varchar(50) DEFAULT NULL,
			 `created_date` datetime DEFAULT NULL,
			 `modified_date` datetime DEFAULT NULL,
             PRIMARY KEY (`dcfic_id`),
             KEY `FK_tbl_idm_dchg_fic_offc_cdtab` (`dcfic_offc`),
             KEY `tbl_idm_dchg_fic_ibfk_2` (`dcfic_loan_no`),
             CONSTRAINT `FK_tbl_idm_dchg_fic_offc_cdtab` FOREIGN KEY (`dcfic_offc`) REFERENCES `offc_cdtab` (`OFFC_CD`),
             CONSTRAINT `tbl_idm_dchg_fic_ibfk_2` FOREIGN KEY (`dcfic_loan_no`) REFERENCES `tbl_app_loan_mast` (`ln_no`)
             );
             select  'tbl_idm_dchg_fic  Created' as ' ';
            end;
		END IF;
	END;
		
	-- ----------------------------- Additional Conditional Module --------------------------------------
-- ----------------------------- tbl_idm_addlcond_det -----------------------------------------------

	BEGIN
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_addlcond_det')

		 -- If not exists, creat a new table
		THEN
        select 'tbl_idm_addlcond_det already Exist' as '';
		Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_addlcond_det' AND column_name='loan_acc')
					THEN                    
					ALTER TABLE `tbl_idm_addlcond_det`
							Modify Column  `loan_acc` bigint DEFAULT NULL;
								select 'Column loan_acc in tbl_idm_addlcond_det Modified' as ' ';
					
					ELSE
							
						 select 'Column loan_acc in tbl_idm_addlcond_det existed' as ' ';
					end if;
			    END;

		Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_addlcond_det' AND column_name='wh_rel_sought')
					THEN
					
								select 'Column wh_rel_sought in tbl_idm_addlcond_det exists' as ' ';
                               
					ELSE
						ALTER TABLE `tbl_idm_addlcond_det`
							ADD Column  `wh_rel_sought` bit(1) DEFAULT NULL AFTER wh_relaxation;
						 select 'Column wh_rel_sought in tbl_idm_addlcond_det created' as ' ';
					end if;
		END;
		Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_addlcond_det' AND column_name='addcond_compl')
					THEN
					
								select 'Column addcond_compl in tbl_idm_addlcond_det exists' as ' ';
                               
					ELSE
						ALTER TABLE `tbl_idm_addlcond_det`
							ADD Column  `addcond_compl` VARCHAR(5) DEFAULT NULL AFTER wh_rel_sought;
						 select 'Column addcond_compl in tbl_idm_addlcond_det created' as ' ';
					end if;
			    END;
        else
		CREATE TABLE `tbl_idm_addlcond_det` (
		`addlcond_det_id` bigint NOT NULL AUTO_INCREMENT,
		`loan_acc` bigint DEFAULT NULL,
		`loan_sub` int DEFAULT NULL,
		`offc_cd` tinyint DEFAULT NULL,
		`addl_cond_code` tinyint DEFAULT NULL,
		`addl_cond_stg` tinyint DEFAULT NULL,
		`addl_cond_details` varchar(200) DEFAULT NULL,
		`wh_relaxation` bit(1) DEFAULT NULL,
		`wh_rel_sought` bit(1) DEFAULT NULL,
		`addcond_compl` VARCHAR(5) DEFAULT NULL,
		`is_active` bit(1) DEFAULT NULL,
		`is_deleted` bit(1) DEFAULT NULL,
		`created_by` varchar(50) DEFAULT NULL,
		`modified_by` varchar(50) DEFAULT NULL,
		`created_date` datetime DEFAULT NULL,
		`modified_date` datetime DEFAULT NULL,
		`unique_id` varchar(200) DEFAULT NULL,
		PRIMARY KEY (`addlcond_det_id`),
		KEY `fk_tbl_idm_addlcond_det_offc_cdtab` (`offc_cd`),
		CONSTRAINT `fk_tbl_idm_addlcond_det_offc_cdtab` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`)
		);
             select  'tbl_idm_addlcond_det  Created' as ' ';
		END IF;		
	END;
    
     -- ----------------------------- Disbursement Condition Module --------------------------------------
-- ----------------------------- tbl_idm_cond_det -----------------------------------------------

	BEGIN
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_cond_det')

		 -- If not exists, creat a new table
		THEN
        select 'tbl_idm_cond_det already Exist' as '';
		Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_cond_det' AND column_name='wh_rel_sought')
					THEN
					
								select 'Column wh_rel_sought in tbl_idm_cond_det exists' as ' ';
                               
					ELSE
						ALTER TABLE `tbl_idm_cond_det`
							ADD Column  `wh_rel_sought` bit(1) DEFAULT NULL AFTER wh_relaxation;
						 select 'Column wh_rel_sought in tbl_idm_cond_det created' as ' ';
					end if;
			    END;
		Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_cond_det' AND column_name='cond_compl')
					THEN
					
								select 'Column cond_compl in tbl_idm_cond_det exists' as ' ';
                               
					ELSE
						ALTER TABLE `tbl_idm_cond_det`
							ADD Column  `cond_compl` VARCHAR(5) DEFAULT NULL AFTER cond_details;
						 select 'Column cond_compl in tbl_idm_cond_det created' as ' ';
					end if;
			    END;
					    
		Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_cond_det' AND column_name='cond_stg')
					THEN
					
								select 'Column cond_stg in tbl_idm_cond_det exists' as ' ';
                               
					ELSE
						ALTER TABLE `tbl_idm_cond_det`
							Modify Column  `cond_stg` bigint DEFAULT NULL;
						 select 'Column cond_stg in tbl_idm_cond_det created' as ' ';
					end if;
			    END;
        else
		CREATE TABLE `tbl_idm_cond_det` (
		`cond_det_id` bigint NOT NULL AUTO_INCREMENT COMMENT 'Row ID',
		`loan_acc` bigint DEFAULT NULL,
		`loan_sub` int DEFAULT NULL COMMENT 'Loan Sub',
		`offc_cd` tinyint DEFAULT NULL COMMENT 'Branch Code',
		`cond_type` tinyint DEFAULT NULL COMMENT 'Condition Type',
		`cond_cd` smallint DEFAULT NULL COMMENT 'Condition code',
		`cond_stg` bigint DEFAULT NULL COMMENT 'Condition Stage',
		`cond_details` varchar(200) DEFAULT NULL COMMENT 'Condition Details',
		`cond_compl` VARCHAR(5) DEFAULT NULL,
		`cond_remarks` varchar(200) DEFAULT NULL COMMENT 'Condition Remarks',
		`wh_relaxation` bit(1) DEFAULT NULL COMMENT 'Whether Relaxation Sought',
		`wh_rel_sought` bit(1) DEFAULT NULL,
		`cond_upload` varchar(100) DEFAULT NULL COMMENT 'Document Upload path',
		`is_active` bit(1) DEFAULT NULL,
		`is_deleted` bit(1) DEFAULT NULL,
		`created_by` varchar(50) DEFAULT NULL,
		`modified_by` varchar(50) DEFAULT NULL,
		`created_date` datetime DEFAULT NULL,
		`modified_date` datetime DEFAULT NULL,
		`unique_id` varchar(150) DEFAULT NULL,
		PRIMARY KEY (`cond_det_id`),
		KEY `fk_idm_cond_det_offc_cdtab` (`offc_cd`),
		CONSTRAINT `fk_idm_cond_det_offc_cdtab` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`)
		);
             select  'tbl_idm_cond_det  Created' as ' ';
		END IF;		
	END;
    
     -- -----------------------Form 8 and 13 Module ----------------------------------
	-- ---------------------- tbl_idm_dsb_fm813 --------------------
		BEGIN
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_dsb_fm813')

		 -- If not exists, creat a new table
		THEN
		select 'tbl_idm_dsb_fm813 Table Already Exist' as'';
		Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dsb_fm813' AND column_name='df813_unit')
					THEN
					ALTER TABLE `tbl_idm_dsb_fm813`
							Modify Column  `df813_unit` bigint DEFAULT NULL;
								select 'Column df813_unit in tbl_idm_dsb_fm813 Modified' as ' ';
                               
					ELSE
							
						 select 'Column df813_unit in tbl_idm_dsb_fm813 existed' as ' ';
					end if;
			    END;
				Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dsb_fm813' AND column_name='df813_loan_acc')
					THEN
					
								select 'Column df813_loan_acc in tbl_idm_dsb_fm813 Exist' as ' ';
                               
					ELSE
							ALTER TABLE `tbl_idm_dsb_fm813`
							Add `df813_loan_acc` bigint DEFAULT NULL;
						 select 'Column df813_loan_acc in tbl_idm_dsb_fm813 Created' as ' ';
					end if;
			    END;
		Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dsb_fm813' AND column_name='df813_rqdt')
					THEN
					
								select 'Column df813_rqdt in tbl_idm_dsb_fm813 Exist' as ' ';
                               
					ELSE
							ALTER TABLE `tbl_idm_dsb_fm813`
							Add COLUMN `df813_rqdt` date DEFAULT NULL AFTER `df813_dt`;
						 select 'Column df813_rqdt in tbl_idm_dsb_fm813 Created' as ' ';
					end if;
			    END;
		else
		CREATE TABLE `tbl_idm_dsb_fm813` (
			  `df813_id` bigint NOT NULL AUTO_INCREMENT,
			  `df813_offc` tinyint DEFAULT NULL,
			  `df813_unit` bigint DEFAULT NULL,
			  `df813_sno` int DEFAULT NULL,
			  `df813_dt` date DEFAULT NULL,
			  `df813_rqdt` date DEFAULT NULL,
			  `df813_ref` varchar(100) DEFAULT NULL,
			  `df813_cc` varchar(100) DEFAULT NULL,
			  `df813_t1` int DEFAULT NULL,
			  `df813_a1` int DEFAULT NULL,
			  `df813_upload` varchar(100) DEFAULT NULL,
			  `df813_loan_acc` bigint DEFAULT NULL,
			  `is_active` bit(1) DEFAULT NULL,
			  `is_deleted` bit(1) DEFAULT NULL,
			  `created_by` varchar(50) DEFAULT NULL,
			  `modified_by` varchar(50) DEFAULT NULL,
			  `created_date` datetime DEFAULT NULL,
			  `modified_date` datetime DEFAULT NULL,
			  `unique_id` varchar(150) DEFAULT NULL,
			  PRIMARY KEY (`df813_id`),
			  KEY `df813_offc` (`df813_offc`),
			  KEY `df813_unit` (`df813_unit`),
			  CONSTRAINT `tbl_idm_dsb_fm813_ibfk_1` FOREIGN KEY (`df813_offc`) REFERENCES `offc_cdtab` (`OFFC_CD`),
			  CONSTRAINT `tbl_idm_dsb_fm813_ibfk_2` FOREIGN KEY (`df813_unit`) REFERENCES `tbl_app_loan_mast` (`ln_mast_id`)
			);
            select  'tbl_idm_dsb_fm813  Created' as ' ';
		END IF;		
	END;

	-- ---------------------- tbl_idm_dsb_fm813 --------------------
		BEGIN
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_fm8_fm13_cdtab')

		 -- If not exists, creat a new table
		THEN
		select  'tbl_fm8_fm13_cdtab  Table Already Exist' as ' ';

		ELSE
		CREATE TABLE `tbl_fm8_fm13_cdtab` (
			`form_type_cd` int NOT NULL AUTO_INCREMENT,
		  `form_type` varchar(150) DEFAULT NULL,
		  `is_active` bit(1) DEFAULT NULL,
		  `is_deleted` bit(1) DEFAULT NULL,
		  `created_by` varchar(50) DEFAULT NULL,
		  `modified_by` varchar(50) DEFAULT NULL,
		  `created_date` datetime DEFAULT NULL,
		  `modified_date` datetime DEFAULT NULL,
		  PRIMARY KEY (`form_type_cd`)
		);	
            select  'tbl_fm8_fm13_cdtab  Created' as ' ';
		END IF;		
	END;
-- ---------------------- tbl_idm_hypoth_map --------------------
		BEGIN
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_hypoth_map')

		 -- If not exists, creat a new table
		THEN
		select  'tbl_idm_hypoth_map  Table Already Exist' as ' ';
		Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_hypoth_map' AND column_name='hypoth_deed_no')
						THEN
								  select 'Column hypoth_deed_no in tbl_idm_hypoth_map  already exist' as ' ';
						ELSE
							ALTER TABLE `tbl_idm_hypoth_map`
					ADD COLUMN `hypoth_deed_no` VARCHAR(100) NULL AFTER `is_deleted`;
						select 'Column hypoth_deed_no in tbl_idm_hypoth_map  Created' as ' ';
				end if;
            end;
			Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_hypoth_map' AND column_name='hypoth_value')
						THEN
								  select 'Column hypoth_value in tbl_idm_hypoth_map  already exist' as ' ';
						ELSE
							ALTER TABLE `tbl_idm_hypoth_map`
					ADD COLUMN `hypoth_value` DECIMAL(20,2) NULL AFTER `hypoth_deed_no`;
						select 'Column hypoth_value in tbl_idm_hypoth_map  Created' as ' ';
				end if;
            end;
			Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_hypoth_map' AND column_name='hypoth_deed_no')
						THEN
								  select 'Column hypoth_deed_no in tbl_idm_hypoth_map  already exist' as ' ';
						ELSE
							ALTER TABLE `tbl_idm_hypoth_map`
					ADD COLUMN `hypoth_deed_no` varchar(100) DEFAULT NULL AFTER `hypoth_no`;
						select 'Column hypoth_deed_no in tbl_idm_hypoth_map  Created' as ' ';
				end if;
            end;
		ELSE
		CREATE TABLE `tbl_idm_hypoth_map` (
  `hypoth_map_id` int NOT NULL,
  `hypoth_code` int DEFAULT NULL,
  `hypoth_no` varchar(20) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `hypoth_deed_no` varchar(100) DEFAULT NULL,
  `hypoth_value` decimal(20,2) DEFAULT NULL,
  PRIMARY KEY (`hypoth_map_id`),
  KEY `hypoth_no_idx` (`hypoth_code`),
  CONSTRAINT `hypoth_no` FOREIGN KEY (`hypoth_code`) REFERENCES `tbl_idm_hypoth_det` (`idm_hypoth_detid`)
		);	
            select  'tbl_idm_hypoth_map  Created' as ' ';
		END IF;		
	END;
END$$

DELIMITER ;
call KSFC_Disbursement_DDL_SP()