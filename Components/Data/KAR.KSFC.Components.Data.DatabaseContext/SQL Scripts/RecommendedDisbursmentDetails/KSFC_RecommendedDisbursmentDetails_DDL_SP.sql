USE `ksfc_oct`;
DROP procedure IF EXISTS `KSFC_RecommendedDisbursmentDetails_DDL_SP`;

DELIMITER $$
USE `ksfc_oct`$$
CREATE PROCEDURE `KSFC_RecommendedDisbursmentDetails_DDL_SP` ()
BEGIN
DECLARE DBName varchar(50) DEFAULT "ksfc_oct";
-- -----------------------Recommended Disbursment Module Added by Gowtham ----------------------------------
-- ---------------------- tbl_idm_dsb_dets --------------------
Begin
IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
					WHERE table_schema = DBName
					AND table_name LIKE 'tbl_idm_dsb_dets')
	-- If exists, retreive columns information from that table
		Then
		select 'tbl_idm_dsb_dets table already exist ' as ' ';
		 Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dsb_dets' AND column_name='sec_considered_f_release')
						THEN
						select 'Column sec_considered_f_release already Exists in tbl_idm_dsb_dets table' as ' ';
						ELSE
                       ALTER TABLE `tbl_idm_dsb_dets` 
								ADD COLUMN `sec_considered_f_release` DECIMAL(10,2) DEFAULT NULL AFTER `dsb_est_amt`;
							  select 'Column sec_considered_f_release in tbl_idm_dsb_dets table Created' as ' ';
					end if;
			    END;
				Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dsb_dets' AND column_name='sec_inspection')
						THEN
						select 'Column sec_inspection already Exists in tbl_idm_dsb_dets table' as ' ';
						ELSE
                       ALTER TABLE `tbl_idm_dsb_dets` 
								ADD COLUMN `sec_inspection` DECIMAL(10,2) DEFAULT NULL AFTER `sec_considered_f_release`;
							  select 'Column sec_inspection in tbl_idm_dsb_dets table Created' as ' ';
					end if;
			    END;

				
                Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dsb_dets' AND column_name='Margin_retained')
						THEN
						select 'Column Margin_retained already Exists in tbl_idm_dsb_dets table' as ' ';
						ELSE
                       ALTER TABLE `tbl_idm_dsb_dets` 
								ADD COLUMN `Margin_retained` int DEFAULT NULL AFTER `sec_inspection`;
							  select 'Column Margin_retained in tbl_idm_dsb_dets table Created' as ' ';
					end if;
			    END;
                Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dsb_dets' AND column_name='aloc_amt')
						THEN
						select 'Column aloc_amt already Exists in tbl_idm_dsb_dets table' as ' ';
						ELSE
                       ALTER TABLE `tbl_idm_dsb_dets` 
								ADD COLUMN `aloc_amt` DECIMAL(10,2) DEFAULT NULL AFTER `Margin_retained`;
							  select 'Column aloc_amt in tbl_idm_dsb_dets table Created' as ' ';
					end if;
			    END;
		 Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dsb_dets' AND column_name='unique_id')
						THEN
						select 'Column unique_id already Exists in tbl_idm_dsb_dets table' as ' ';
						ELSE
                       ALTER TABLE `tbl_idm_dsb_dets` 
								ADD COLUMN `unique_id` varchar(50) DEFAULT NULL AFTER `modified_date`;
							  select 'Column unique_id in tbl_idm_dsb_dets table Created' as ' ';
					end if;
			    END;
	   Begin
				IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dsb_dets' AND column_name='prop_amt')
						THEN
						select 'Column prop_amt already Exists in tbl_idm_dsb_dets table' as ' ';
						ELSE
                       ALTER TABLE `tbl_idm_dsb_dets` 
								ADD COLUMN `prop_amt` DECIMAL(10,2) DEFAULT NULL AFTER `dsb_acd`;
							  select 'Column prop_amt in tbl_idm_dsb_dets table Created' as ' ';
					end if;
			    END;
		else
 CREATE TABLE `tbl_idm_dsb_dets` (
				`dsb_dets_id` bigint NOT NULL AUTO_INCREMENT,
				`loan_acc` bigint DEFAULT NULL,
				`loan_sub` int DEFAULT NULL,
				`offc_cd` tinyint DEFAULT NULL,
				`dsb_no` int DEFAULT NULL,
				`dsb_dt` date DEFAULT NULL,
				`dsb_amt` int DEFAULT NULL,
				`dsb_acd` int DEFAULT NULL,
				`prop_amt` decimal(10,2) DEFAULT NULL,
				`dsb_est_amt` int DEFAULT NULL,
				`sec_considered_f_release` decimal(10,2) DEFAULT NULL,
				`sec_inspection` decimal(10,2) DEFAULT NULL,
				`Margin_retained` int DEFAULT NULL,
				`aloc_amt` decimal(10,2) DEFAULT NULL,
				`is_active` bit(1) DEFAULT NULL,
				`is_deleted` bit(1) DEFAULT NULL,
				`created_by` varchar(50) DEFAULT NULL,
				`modified_by` varchar(50) DEFAULT NULL,
				`created_date` datetime DEFAULT NULL,
				`modified_date` datetime DEFAULT NULL,
				`unique_id` varchar(50) DEFAULT NULL,
				PRIMARY KEY (`dsb_dets_id`),
				KEY `FK_tbl_idm_dsb_dets_offc_cdtab` (`offc_cd`),
				CONSTRAINT `FK_tbl_idm_dsb_dets_offc_cdtab` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`)
				);
                 select 'tbl_idm_dsb_dets Table Created' as ' ';
			END IF;
	END;

						-- tbl_idm_disb_prop --
Begin
IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
					WHERE table_schema = DBName
					AND table_name LIKE 'tbl_idm_disb_prop')
	-- If exists, retreive columns information from that table
		Then
		select 'tbl_idm_disb_prop table already exist ' as ' ';
		
		Begin
               
				IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_disb_prop' AND column_name='prop_id')
						THEN
                       ALTER TABLE `tbl_idm_disb_prop` 
					  MODIFY COLUMN  prop_number  BIGINT DEFAULT NULL;
							  select 'Column prop_number in tbl_idm_disb_prop table Modified' as ' ';
						ELSE
						select 'Column prop_number in tbl_idm_disb_prop Created' as ' ';
				end if;
				
            end;
			Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_disb_prop' AND column_name='prop_amt')
						THEN
						select 'Column prop_amt already Exists in tbl_idm_disb_prop table' as ' ';
						ELSE
                       ALTER TABLE `tbl_idm_disb_prop` 
								ADD COLUMN `prop_amt` DECIMAL(20,2) DEFAULT NULL AFTER `unique_id`;
							  select 'Column prop_amt in tbl_idm_disb_prop table Created' as ' ';
					end if;
			    END;
		
		else
CREATE TABLE `tbl_idm_disb_prop` (
  `prop_id` bigint NOT NULL AUTO_INCREMENT,
  `loan_acc` bigint DEFAULT NULL,
  `loan_sub` int DEFAULT NULL,
  `offc_cd` tinyint DEFAULT NULL,
  `prop_number` bigint DEFAULT NULL,
  `prop_date` date DEFAULT NULL,
  `prop_dept` int DEFAULT NULL,
  `prop_loan_type` int DEFAULT NULL,
  `prop_sanc_amt` int DEFAULT NULL,
  `prop_disb_amt` int DEFAULT NULL,
  `prop_rec_amt` int DEFAULT NULL,
  `prop_status_flg` int DEFAULT NULL,
  `prop_fdsb_flg` varchar(1) DEFAULT NULL,
  `prop_relty_flg` varchar(1) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) DEFAULT NULL,
  `modified_by` varchar(50) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) DEFAULT NULL,
  `prop_amt` decimal(20,2) DEFAULT NULL,
  PRIMARY KEY (`prop_id`),
  KEY `FK_tbl_idm_disb_prop_offc_cdtab` (`offc_cd`),
  CONSTRAINT `FK_tbl_idm_disb_prop_offc_cdtab` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`)
);
select 'tbl_idm_disb_prop Table Created' as ' ';
END IF;
END;

						-- tbl_idm_rele_detls --
Begin
IF  EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_rele_detls')

		-- If not exists, creat a new table
Then
Begin
              IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_rele_detls' AND column_name='rele_at_par_charges')
						THEN
						select 'Column rele_at_par_charges already Exists in tbl_idm_rele_detls table' as ' ';
						ELSE
                       ALTER TABLE `tbl_idm_rele_detls` 
								ADD COLUMN `rele_at_par_charges` DECIMAL(10,2) NULL AFTER `rele_at_par_amt`;
							  select 'Column rele_at_par_charges in tbl_idm_rele_detls table Created' as ' ';
				end if;

				IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_rele_detls' AND column_name='rele_id')
						THEN
                       ALTER TABLE tbl_idm_rele_detls
					  MODIFY COLUMN  rele_due_amt decimal(10,2) DEFAULT NULL;
							  select 'Column rele_due_amt in tbl_idm_rele_detls table Modified' as ' ';
						ELSE
						select 'Column rele_due_amt in tbl_idm_rele_detls Created' as ' ';
				end if;

				IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_rele_detls' AND column_name='rele_due_amt')
						THEN
                       ALTER TABLE tbl_idm_rele_detls
					  MODIFY COLUMN  rele_bnk_chg decimal(10,2) DEFAULT NULL;
							  select 'Column rele_bnk_chg in tbl_idm_rele_detls table Modified' as ' ';
						ELSE
						select 'Column rele_bnk_chg in tbl_idm_rele_detls Created' as ' ';
				end if;

				IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_rele_detls' AND column_name='rele_bnk_chg')
						THEN
                       ALTER TABLE tbl_idm_rele_detls
					  MODIFY COLUMN  rele_up_frt_amt decimal(10,2) DEFAULT NULL;
							  select 'Column rele_up_frt_amt in tbl_idm_rele_detls table Modified' as ' ';
						ELSE
						select 'Column rele_up_frt_amt in tbl_idm_rele_detls Created' as ' ';
				end if;

				IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_rele_detls' AND column_name='rele_up_frt_amt')
						THEN
                       ALTER TABLE tbl_idm_rele_detls
					  MODIFY COLUMN  rele_doc_chg decimal(10,2) DEFAULT NULL;
							  select 'Column rele_doc_chg in tbl_idm_rele_detls table Modified' as ' ';
						ELSE
						select 'Column rele_doc_chg in tbl_idm_rele_detls Created' as ' ';
				end if;

				IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_rele_detls' AND column_name='rele_doc_chg')
						THEN
                       ALTER TABLE tbl_idm_rele_detls
					  MODIFY COLUMN  rele_com_chg decimal(10,2) DEFAULT NULL;
							  select 'Column rele_com_chg in tbl_idm_rele_detls table Modified' as ' ';
						ELSE
						select 'Column rele_com_chg in tbl_idm_rele_detls Created' as ' ';
				end if;

				IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_rele_detls' AND column_name='rele_com_chg')
						THEN
                       ALTER TABLE tbl_idm_rele_detls
					  MODIFY COLUMN  rele_adj_amt decimal(10,2) DEFAULT NULL;
							  select 'Column rele_adj_amt in tbl_idm_rele_detls table Modified' as ' ';
						ELSE
						select 'Column rele_com_chg in tbl_idm_rele_detls Created' as ' ';
				end if;

					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_rele_detls' AND column_name='rele_adj_amt')
						THEN
                       ALTER TABLE tbl_idm_rele_detls
					  MODIFY COLUMN  rele_adj_rec_seq decimal(10,2) DEFAULT NULL;
							  select 'Column rele_adj_rec_seq in tbl_idm_rele_detls table Modified' as ' ';
						ELSE
						select 'Column rele_adj_rec_seq in tbl_idm_rele_detls Created' as ' ';
				end if;

				IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_rele_detls' AND column_name='rele_adj_rec_seq')
						THEN
                       ALTER TABLE tbl_idm_rele_detls
					  MODIFY COLUMN  rele_amt decimal(10,2) DEFAULT NULL;
							  select 'Column rele_amt in tbl_idm_rele_detls table Modified' as ' ';
						ELSE
						select 'Column rele_amt in tbl_idm_rele_detls Created' as ' ';
				end if;

				IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_rele_detls' AND column_name='addl_amt1')
						THEN
						select 'Column addl_amt1 already Exists in tbl_idm_rele_detls table' as ' ';
						ELSE
                       ALTER TABLE tbl_idm_rele_detls
					  ADD COLUMN  addl_amt1 int DEFAULT NULL;
							  select 'Column addl_amt1 in tbl_idm_rele_detls table Created' as ' ';
						
						
				end if;
				
				IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_rele_detls' AND column_name='addl_amt2')
						THEN
						select 'Column addl_amt2 already Exists in tbl_idm_rele_detls table' as ' ';
						ELSE
                       ALTER TABLE tbl_idm_rele_detls
					  ADD COLUMN  addl_amt2 int DEFAULT NULL;
							  select 'Column addl_amt2 in tbl_idm_rele_detls table Created' as ' ';
						
						
				end if;

				IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_rele_detls' AND column_name='addl_amt3')
						THEN
						select 'Column addl_amt3 already Exists in tbl_idm_rele_detls table' as ' ';
						ELSE
                       ALTER TABLE tbl_idm_rele_detls
					  ADD COLUMN  addl_amt3 int DEFAULT NULL;
							  select 'Column addl_amt3 in tbl_idm_rele_detls table Created' as ' ';
						
				end if;
				IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_rele_detls' AND column_name='addl_amt4')
						THEN
						select 'Column addl_amt4 already Exists in tbl_idm_rele_detls table' as ' ';
						ELSE
                       ALTER TABLE tbl_idm_rele_detls
					  ADD COLUMN  addl_amt4 int DEFAULT NULL;
							  select 'Column addl_amt4 in tbl_idm_rele_detls table Created' as ' ';
						
				end if;
				IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_rele_detls' AND column_name='addl_amt5')
						THEN
						select 'Column addl_amt5 already Exists in tbl_idm_rele_detls table' as ' ';
						ELSE
                       ALTER TABLE tbl_idm_rele_detls
					  ADD COLUMN  addl_amt5 int DEFAULT NULL;
							  select 'Column addl_amt5 in tbl_idm_rele_detls table Created' as ' ';
						
				end if;


            end;
else
CREATE TABLE `tbl_idm_rele_detls` (
  `rele_id` bigint NOT NULL AUTO_INCREMENT,
  `loan_acc` bigint DEFAULT NULL,
  `loan_sub` int DEFAULT NULL,
  `offc_cd` tinyint DEFAULT NULL,
  `prop_number` bigint DEFAULT NULL,
  `rele_due_amt` decimal(10,2) DEFAULT NULL,
  `rele_at_par_amt` int DEFAULT NULL,
  `rele_at_par_charges` decimal(10,2) DEFAULT NULL,
  `rele_bnk_chg` decimal(10,2) DEFAULT NULL,
  `rele_up_frt_amt` decimal(10,2) DEFAULT NULL,
  `rele_doc_chg` decimal(10,2) DEFAULT NULL,
  `rele_com_chg` decimal(10,2) DEFAULT NULL,
  `rele_fd_amt` int DEFAULT NULL,
  `rele_fd_glcd` int DEFAULT NULL,
  `rele_oth_amt` int DEFAULT NULL,
  `rele_oth_glcd` int DEFAULT NULL,
  `rele_adj_amt` decimal(10,2) DEFAULT NULL,
  `rele_adj_rec_seq` decimal(10,2) DEFAULT NULL,
  `rele_add_up_frt_amt` int DEFAULT NULL,
  `rele_addlafd_amt` int DEFAULT NULL,
  `rele_sertax_amt` int DEFAULT NULL,
  `rele_cersai` int DEFAULT NULL,
  `rele_swachcess` int DEFAULT NULL,
  `rele_krishikalyancess` int DEFAULT NULL,
  `rele_coll_guarantee_fee` int DEFAULT NULL,
  `rele_number` int DEFAULT NULL,
  `rele_date` date DEFAULT NULL,
  `rele_amt` decimal(10,2) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) DEFAULT NULL,
  `modified_by` varchar(50) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) DEFAULT NULL,
  `addl_amt1` int DEFAULT NULL,
  `addl_amt2` int DEFAULT NULL,
  `addl_amt3` int DEFAULT NULL,
  `addl_amt4` int DEFAULT NULL,
  `addl_amt5` int DEFAULT NULL,
  PRIMARY KEY (`rele_id`),
  KEY `FK_tbl_idm_rele_detls_offc_cdtab` (`offc_cd`),
  KEY `FK_tbl_idm_rele_detls_tbl_idm_disb_prop` (`prop_number`),
  CONSTRAINT `FK_tbl_idm_rele_detls_offc_cdtab` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`),
  CONSTRAINT `FK_tbl_idm_rele_detls_tbl_idm_disb_prop` FOREIGN KEY (`prop_number`) REFERENCES `tbl_idm_disb_prop` (`prop_id`)
);
select 'tbl_idm_rele_detls Table Created' as ' ';
END IF;
END;
							-- tbl_idm_benf_detls --
			Begin
			IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_benf_detls')

					 -- If not exists, creat a new table
			Then
             select 'tbl_idm_benf_detls Table Already Exist' as ' ';
			  Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_benf_detls' AND column_name='benf_amt')
					THEN
					ALTER TABLE `tbl_idm_benf_detls`
							Modify Column  `benf_amt` decimal(10,2) DEFAULT NULL;
								select 'Column benf_amt in tbl_idm_benf_detls Modified' as ' ';
                               
					ELSE
							
						 select 'Column benf_amt in tbl_idm_benf_detls existed' as ' ';
					end if;
			    END;
				Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_benf_detls' AND column_name='benf_inst_flag')
					THEN
					ALTER TABLE `tbl_idm_benf_detls`
							Modify Column  `benf_inst_flag` bit(1) DEFAULT NULL;
								select 'Column benf_inst_flag in tbl_idm_benf_detls Modified' as ' ';
                               
					ELSE
							
						 select 'Column benf_inst_flag in tbl_idm_benf_detls existed' as ' ';
					end if;
			    END;
                Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_benf_detls' AND column_name='benf_reladj_amt')
					THEN
					ALTER TABLE `tbl_idm_benf_detls`
							Modify Column  `benf_reladj_amt` decimal(10,2) DEFAULT NULL;
								select 'Column benf_reladj_amt in tbl_idm_benf_detls Modified' as ' ';
                               
					ELSE
							
						 select 'Column benf_reladj_amt in tbl_idm_benf_detls existed' as ' ';
					end if;
			    END;
			
			else
				 CREATE TABLE `tbl_idm_benf_detls` (
				`benf_id` bigint NOT NULL AUTO_INCREMENT,
				`loan_acc` bigint DEFAULT NULL,
				`loan_sub` int DEFAULT NULL,
				`offc_cd` tinyint DEFAULT NULL,
				`benf_number` int DEFAULT NULL,
				`benf_date` date DEFAULT NULL,
				`benf_dept` int DEFAULT NULL,
				`benf_type` varchar(2) DEFAULT NULL,
				`benf_name` varchar(50) DEFAULT NULL,
				`benf_code` int DEFAULT NULL,
				`benf_amt` decimal(10,2) DEFAULT NULL,
				`benf_inst_type` int DEFAULT NULL,
				`benf_inst_flag` bit(1) DEFAULT NULL,
				`benf_rec_seq` int DEFAULT NULL,
				`dd_atpar_loc` varchar(30) DEFAULT NULL,
				`benf_reladj_amt` decimal(10,2) DEFAULT NULL,
				`benf_rtgs_acno` varchar(18) DEFAULT NULL,
				`benf_rtgs_ifsc` varchar(16) DEFAULT NULL,
				`benf_rtgs_bank` varchar(30) DEFAULT NULL,
				`benf_rtgs_bkbranch` varchar(30) DEFAULT NULL,
				`benf_rtgs_bkcity` varchar(100) DEFAULT NULL,
				`benf_rtgs_cheqno` int DEFAULT NULL,
				`benf_rtgs_cheqdt` date DEFAULT NULL,
				`is_active` bit(1) DEFAULT NULL,
				`is_deleted` bit(1) DEFAULT NULL,
				`created_by` varchar(50) DEFAULT NULL,
				`modified_by` varchar(50) DEFAULT NULL,
				`created_date` datetime DEFAULT NULL,
				`modified_date` datetime DEFAULT NULL,
				`unique_id` varchar(200) DEFAULT NULL,
				PRIMARY KEY (`benf_id`),
				KEY `FK_tbl_idm_benf_detls_offc_cdtab` (`offc_cd`),
				CONSTRAINT `FK_tbl_idm_benf_detls_offc_cdtab` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`)
				);
                 select 'tbl_idm_benf_detls Table Created' as ' ';
			END IF;
	END;
	                           -- tbl_dept_master --
	Begin
			IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_dept_master')

					 -- If not exists, creat a new table
			Then
				 select 'tbl_dept_master Table Already Exist' as ' ';
			else
            CREATE TABLE `tbl_dept_master` (
           `dept_code` int NOT NULL,
           `dept_name` varchar(50) DEFAULT NULL,
            PRIMARY KEY (`dept_code`)
            );
				 select 'tbl_dept_master Table Created' as ' ';
			END IF;
		END;
		
		-- tbl_dsb_charge_map --
	Begin
			IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_dsb_charge_map')

					 -- If not exists, creat a new table
			Then
				 select 'tbl_dsb_charge_map Table Already Exist' as ' ';
			else
           CREATE TABLE `tbl_dsb_charge_map` (
           `dsb_othdebit_id` int DEFAULT NULL,
           `data_field_name` varchar(100) DEFAULT NULL
            );
				 select 'tbl_dsb_charge_map Table Created' as ' ';
			END IF;
		END;
END$$

DELIMITER ;
call KSFC_RecommendedDisbursmentDetails_DDL_SP()
