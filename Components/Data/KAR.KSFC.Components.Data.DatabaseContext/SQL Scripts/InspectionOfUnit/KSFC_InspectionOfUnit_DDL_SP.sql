USE `ksfc_oct`;
DROP procedure IF EXISTS `KSFC_InspectionOfUnit_DDL_SP`;

DELIMITER $$
USE `ksfc_oct`$$
CREATE PROCEDURE `KSFC_InspectionOfUnit_DDL_SP` ()
BEGIN
DECLARE DBName varchar(50) DEFAULT "ksfc_oct";
-- -----------------------Land inspection   ----------------------------------
						-- tbl_landtype_mast --
Begin
IF EXISTS(SELECT table_name
					FROM INFORMATION_SCHEMA.TABLES
					WHERE table_schema = DBName
					AND table_name LIKE 'tbl_landtype_mast')

		-- If not exists, create a new table
		THEN
		select 'tbl_landtype_mast Table Exist' as '';
		ELSE
		CREATE TABLE `tbl_landtype_mast` (
  `landtype_id` int NOT NULL AUTO_INCREMENT,
  `landtype_cd` int DEFAULT NULL,
  `landtype_desc` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `modified_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  PRIMARY KEY (`landtype_id`)
) ;
select 'tbl_landtype_mast Table Created' as ''; 
END IF;
END;
						-- tbl_idm_dchg_land --
		Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_dchg_land')

		-- If exists, retreive columns information from that table
		THEN
		select 'tbl_idm_dchg_land Table Already Exist' as '';
		Begin 

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_land' AND column_name='dclnd_rowid')
						THEN
                        ALTER TABLE tbl_idm_dchg_land
							MODIFY COLUMN  dclnd_rowid  int NOT NULL AUTO_INCREMENT;
								  select 'Column dclnd_rowid in tbl_idm_dchg_land table Modified' as ' ';
						ELSE
							
						select 'Column dclnd_rowid in tbl_idm_dchg_land table Created' as ' ';
				end if;
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_land' AND column_name='loan_acc')
						THEN
                        ALTER TABLE tbl_idm_dchg_land
							MODIFY COLUMN  loan_acc  bigint NOT NULL;
								  select 'Column loan_acc in tbl_idm_dchg_land table Modified' as ' ';
						ELSE
							
						select 'Column loan_acc in tbl_idm_dchg_land table Created' as ' ';
				end if;

			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_land' AND column_name='dclnd_ino')
						THEN
                       ALTER TABLE tbl_idm_dchg_land 
					  MODIFY COLUMN  dclnd_ino bigint DEFAULT NULL;
							  select 'Column dclnd_ino in tbl_idm_dchg_land table Modified' as ' ';
						ELSE
						select 'Column dclnd_ino in tbl_idm_dchg_allc Created' as ' ';
				end if;

		IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_land' AND column_name='dclnd_area')
						THEN
                       ALTER TABLE tbl_idm_dchg_land 
					  MODIFY COLUMN  dclnd_amt decimal(15,2) DEFAULT NULL;
							  select 'Column dclnd_area in tbl_idm_dchg_land table Modified' as ' ';
						ELSE
						select 'Column dclnd_area in tbl_idm_dchg_allc Created' as ' ';
				end if;

		IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_land' AND column_name='dclnd_area')
						THEN
                       ALTER TABLE tbl_idm_dchg_land 
					  MODIFY COLUMN  dclnd_area int DEFAULT NULL;
							  select 'Column dclnd_area in tbl_idm_dchg_land table Modified' as ' ';
						ELSE
						select 'Column dclnd_area in tbl_idm_dchg_allc Created' as ' ';
				end if;
		IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_land' AND column_name='dclnd_land_finance')
						THEN
                       ALTER TABLE tbl_idm_dchg_land 
					  MODIFY COLUMN  dclnd_land_finance varchar(1) DEFAULT NULL;
							  select 'Column dclnd_land_finance in tbl_idm_dchg_land table Modified' as ' ';
						ELSE
						select 'Column dclnd_land_finance in tbl_idm_dchg_allc Created' as ' ';
				end if;
           IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_land' AND column_name='dclnd_refno')
						THEN
                       
								  select 'Column dclnd_refno in tbl_idm_dchg_land table Exist' as ' ';
						ELSE
							 ALTER TABLE tbl_idm_dchg_land
							ADD COLUMN  `dclnd_refno`  bigint DEFAULT NULL;
						select 'Column dclnd_refno in tbl_idm_dchg_land table Created' as ' ';
				end if;
            end;
		else
			CREATE TABLE `tbl_idm_dchg_land` (
			  `dclnd_rowid` int NOT NULL AUTO_INCREMENT,
			  `loan_acc` bigint NOT NULL,
			  `loan_sub` int DEFAULT NULL,
			  `offc_cd` tinyint DEFAULT NULL,
			  `dclnd_area` int DEFAULT NULL,
			  `dclnd_areain` varchar(10) DEFAULT NULL,
			  `dclnd_type` int DEFAULT NULL,
			  `dclnd_amt` decimal(15,2) DEFAULT NULL,
			  `dclnd_apau` int DEFAULT NULL,
			  `dclnd_apdt` datetime DEFAULT NULL,
			  `dclnd_devcst` decimal(10,0) DEFAULT NULL,
			  `dclnd_land_finance` varchar(1) DEFAULT NULL,
			  `dclnd_stat_chgdate` datetime DEFAULT NULL,
			  `dclnd_aqrd_indicator` int DEFAULT NULL,
			  `dclnd_sec_created` int DEFAULT NULL,
			  `dclnd_ino` bigint DEFAULT NULL,
			  `dclnd_refno`  bigint DEFAULT NULL,
			  `dclnd_dets` varchar(200) DEFAULT NULL,
			  `created_by` varchar(50) DEFAULT NULL,
			  `created_date` datetime DEFAULT NULL,
			  `modified_by` varchar(50) DEFAULT NULL,
			  `modified_date` datetime DEFAULT NULL,
			  `unique_id` varchar(150) DEFAULT NULL,
			  `is_active` bit(1) DEFAULT NULL,
			  `is_deleted` bit(1) DEFAULT NULL,
			  PRIMARY KEY (`dclnd_rowid`),
			  KEY `fk_tbl_idm_dchg_land_offc_cdtab_idx` (`offc_cd`),
			  KEY `fk_tbl_idm_dchg_land_tbl_app_loan_master_idx` (`loan_acc`),
			  CONSTRAINT `fk_tbl_idm_dchg_land_offc_cdtab` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`),
			  CONSTRAINT `fk_tbl_idm_dchg_land_tbl_app_loan_master` FOREIGN KEY (`loan_acc`) REFERENCES `tbl_app_loan_mast` (`ln_no`)
			);
            select 'tbl_idm_dchg_land Table Created' as '';
		END IF;
		END;

		-- -----------------------Building material at Site inspection   ----------------------------------
						-- tbl_idm_ir_bldgmat --
		Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_ir_bldgmat')

		-- If exists, retreive columns information from that table
		THEN
		 select 'tbl_idm_ir_bldgmat Table Already Exist' as '';
		Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_ir_bldgmat' AND column_name='loan_acc')
						THEN
                        ALTER TABLE tbl_idm_ir_bldgmat
							MODIFY COLUMN  loan_acc  bigint NOT NULL;
								  select 'Column loan_acc in tbl_idm_ir_bldgmat table Modified' as ' ';
						ELSE
							
						select 'Column loan_acc in tbl_idm_ir_bldgmat table Created' as ' ';
				end if;
            end;
			Begin
			IF Not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_ir_bldgmat' AND column_name='irbm_amt')
						THEN
                        ALTER TABLE tbl_idm_ir_bldgmat
						ADD COLUMN `irbm_amt` decimal(20,2) DEFAULT NULL AFTER `unique_id`;
								  select 'Column irbm_amt in tbl_idm_ir_bldgmat table Modified' as ' ';
						ELSE
							
						select 'Column irbm_amt in tbl_idm_ir_bldgmat table Created' as ' ';
				end if;
            end;
		else
			CREATE TABLE `tbl_idm_ir_bldgmat` (
			  `irbm_rowid` int NOT NULL AUTO_INCREMENT,
			  `loan_acc` bigint NOT NULL,
			  `loan_sub` int DEFAULT NULL,
			  `offc_cd` tinyint DEFAULT NULL,
			  `irbm_idt` datetime DEFAULT NULL,
			  `irbm_rdt` datetime DEFAULT NULL,
			  `irbm_item` int DEFAULT NULL,
			  `irbm_mat` varchar(100) DEFAULT NULL,
			  `irbm_qty` int DEFAULT NULL,
			  `irbm_rate` decimal(10,2) DEFAULT NULL,
			  `irbm_no` int DEFAULT NULL,
			  `irbm_qtyin` varchar(10) DEFAULT NULL,
			  `uom_id` int DEFAULT NULL,
			  `irbm_ino` bigint DEFAULT NULL,
			  `created_by` varchar(50) DEFAULT NULL,
			  `created_date` datetime DEFAULT NULL,
			  `modified_by` varchar(50) DEFAULT NULL,
			  `modified_date` datetime DEFAULT NULL,
			  `unique_id` varchar(150) DEFAULT NULL,
			  `irbm_amt` decimal(20,2) DEFAULT NULL,
			  `is_active` bit(1) DEFAULT NULL,
			  `is_deleted` bit(1) DEFAULT NULL,
			  PRIMARY KEY (`irbm_rowid`),
			  KEY `fk_tbl_idm_ir_bldgmat_offc_cdtab_idx` (`offc_cd`),
			  KEY `fk_tbl_idm_ir_bldgmat_tbl_app_loan_master_idx` (`loan_acc`),
			  CONSTRAINT `fk_tbl_idm_ir_bldgmat_offc_cdtab` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`),
			  CONSTRAINT `fk_tbl_idm_ir_bldgmat_tbl_app_loan_master` FOREIGN KEY (`loan_acc`) REFERENCES `tbl_app_loan_mast` (`ln_no`)
			);
             select 'tbl_idm_ir_bldgmat Table Created' as '';
		END IF;
		END;

-- -----------------------Building inspection   ----------------------------------
						-- tbl_idm_dchg_bldg--
		Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_dchg_bldg')

		-- If exists, retreive columns information from that table
		THEN
		 select 'tbl_idm_dchg_bldg Table Already Exist' as '';
		Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_bldg' AND column_name='loan_acc')
						THEN
                        ALTER TABLE tbl_idm_dchg_bldg
							MODIFY COLUMN  loan_acc  bigint NOT NULL;
								  select 'Column loan_acc in tbl_idm_dchg_bldg table Modified' as ' ';
						ELSE
							
						select 'Column loan_acc in tbl_idm_dchg_bldg table Created' as ' ';
				end if;
            end;
			Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_bldg' AND column_name='dcbdg_plnth')
						THEN
                        ALTER TABLE tbl_idm_dchg_bldg
							MODIFY COLUMN  `dcbdg_plnth` bigint DEFAULT NULL;
								  select 'Column dcbdg_plnth in tbl_idm_dchg_bldg table Modified' as ' ';
						ELSE
							
						select 'Column dcbdg_plnth in tbl_idm_dchg_bldg table Created' as ' ';
				end if;
            end;
			Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_bldg' AND column_name='dcbdg_itmno')
						THEN
                        ALTER TABLE tbl_idm_dchg_bldg
							MODIFY COLUMN   `dcbdg_itmno` bigint DEFAULT NULL;
								  select 'Column dcbdg_itmno in tbl_idm_dchg_bldg table Modified' as ' ';
						ELSE
							
						select 'Column dcbdg_itmno in tbl_idm_dchg_bldg table Created' as ' ';
				end if;
            end;
			Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_bldg' AND column_name='dcbdg_ucost')
						THEN
                        ALTER TABLE tbl_idm_dchg_bldg
							MODIFY COLUMN    `dcbdg_ucost` decimal(10,2) DEFAULT NULL;
								  select 'Column dcbdg_ucost in tbl_idm_dchg_bldg table Modified' as ' ';
						ELSE
							
						select 'Column dcbdg_ucost in tbl_idm_dchg_bldg table Created' as ' ';
				end if;
            end;
			Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_bldg' AND column_name='dcbdg_tcost')
						THEN
                        ALTER TABLE tbl_idm_dchg_bldg
							MODIFY COLUMN     `dcbdg_tcost` decimal(10,2) DEFAULT NULL;
								  select 'Column dcbdg_tcost in tbl_idm_dchg_bldg table Modified' as ' ';
						ELSE
							
						select 'Column dcbdg_tcost in tbl_idm_dchg_bldg table Created' as ' ';
				end if;
            end;
			Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_bldg' AND column_name='dcbdg_aplnth')
						THEN
                        ALTER TABLE tbl_idm_dchg_bldg
							MODIFY COLUMN `dcbdg_aplnth` int DEFAULT NULL;
								  select 'Column dcbdg_aplnth in tbl_idm_dchg_bldg table Modified' as ' ';
						ELSE
							
						select 'Column dcbdg_aplnth in tbl_idm_dchg_bldg table Created' as ' ';
				end if;
            end;
			Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_bldg' AND column_name='dcbdg_ino')
						THEN
                        ALTER TABLE tbl_idm_dchg_bldg
							MODIFY COLUMN `dcbdg_ino` bigint DEFAULT NULL;
								  select 'Column dcbdg_ino in tbl_idm_dchg_bldg table Modified' as ' ';
						ELSE
							
						select 'Column dcbdg_ino in tbl_idm_dchg_bldg table Created' as ' ';
				end if;
            end;
			Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_bldg' AND column_name='dcbdg_sec_creatd')
						THEN
                        ALTER TABLE tbl_idm_dchg_bldg
							MODIFY COLUMN `dcbdg_sec_creatd` int DEFAULT NULL;
								  select 'Column dcbdg_sec_creatd in tbl_idm_dchg_bldg table Modified' as ' ';
						ELSE
							
						select 'Column dcbdg_sec_creatd in tbl_idm_dchg_bldg table Created' as ' ';
				end if;
            end;
			
		else
			CREATE TABLE `tbl_idm_dchg_bldg` (
			  `dcbdg_rowid` bigint NOT NULL AUTO_INCREMENT,
			  `loan_acc` bigint NOT NULL,
			  `loan_sub` int DEFAULT NULL,
			  `offc_cd` tinyint DEFAULT NULL,
			  `dcbdg_itmno` bigint DEFAULT NULL,
			  `dcbdg_dets` varchar(600) DEFAULT NULL,
			  `dcbdg_roof` varchar(40) DEFAULT NULL,
			  `dcbdg_plnth` bigint DEFAULT NULL,
			  `dcbdg_ucost` decimal(10,2) DEFAULT NULL,
			  `dcbdg_tcost` decimal(10,2) DEFAULT NULL,
			  `dcbdg_rqdt` datetime DEFAULT NULL,
			  `dcbdg_apdt` datetime DEFAULT NULL,
			  `dcbdg_apau` int DEFAULT NULL,
			  `dcbdg_rqrd_stat` int DEFAULT NULL,
			  `dcbdg_comdt` datetime DEFAULT NULL,
			  `dcbdg_stat` int DEFAULT NULL,
			  `dcbdg_stat_chgdate` datetime DEFAULT NULL,
			  `dcbdg_sec_creatd` int DEFAULT NULL,
			  `dcbdg_aplnth` int DEFAULT NULL,
			  `dcbdg_atcost` int DEFAULT NULL,
			  `dcbdg_percent` int DEFAULT NULL,
			  `dcbdg_ino` bigint DEFAULT NULL,
			  `created_by` varchar(50) DEFAULT NULL,
			  `created_date` datetime DEFAULT NULL,
			  `modified_by` varchar(50) DEFAULT NULL,
			  `modified_date` datetime DEFAULT NULL,
			  `unique_id` varchar(150) DEFAULT NULL,
			  `is_active` bit(1) NOT NULL DEFAULT 1,
			  `is_deleted` bit(1) NOT NULL DEFAULT 0,
			  PRIMARY KEY (`dcbdg_rowid`),
			  KEY `fk_tbl_idm_dchg_bldg_tbl_app_loan_mast_idx` (`loan_acc`),
			  KEY `fk_tbl_idm_dchg_bldg_offc_cdtab_idx` (`offc_cd`),
			  CONSTRAINT `fk_tbl_idm_dchg_bldg_offc_cdtab` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`),
			  CONSTRAINT `fk_tbl_idm_dchg_bldg_tbl_app_loan_mast` FOREIGN KEY (`loan_acc`) REFERENCES `tbl_app_loan_mast` (`ln_no`)
			);
            select 'tbl_idm_dchg_bldg Table Created' as '';
		END IF;
		END;

		-- -----------------------Indigenous machinery inspection   ----------------------------------
						-- tbl_idm_dchg_plmc--
		Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_dchg_plmc')

		-- If exists, retreive columns information from that table
		THEN
		 select 'tbl_idm_dchg_plmc Table Already Exist' as '';
		Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_plmc' AND column_name='loan_acc')
						THEN
                        ALTER TABLE tbl_idm_dchg_plmc
							MODIFY COLUMN  loan_acc  bigint NOT NULL;
								  select 'Column loan_acc in tbl_idm_dchg_plmc table Modified' as ' ';
						ELSE
							
						select 'Column loan_acc in tbl_idm_dchg_plmc table Created' as ' ';
				end if;
            end;
			Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_plmc' AND column_name='dcplmc_sec_rel')
						THEN
                        ALTER TABLE tbl_idm_dchg_plmc
							MODIFY COLUMN  dcplmc_sec_rel  varchar(50) DEFAULT NULL;
								  select 'Column dcplmc_sec_rel in tbl_idm_dchg_plmc table Modified' as ' ';
						ELSE
							
						select 'Column dcplmc_sec_rel in tbl_idm_dchg_plmc table Created' as ' ';
				end if;
            end;
			Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_plmc' AND column_name='dcplmc_basic_cost')
						THEN
                       
								  select 'Column dcplmc_basic_cost in tbl_idm_dchg_plmc table Exist' as ' ';
						ELSE
							 ALTER TABLE tbl_idm_dchg_plmc
							ADD COLUMN  `dcplmc_basic_cost` decimal(10,2) DEFAULT NULL;
						select 'Column dcplmc_basic_cost in tbl_idm_dchg_plmc table Created' as ' ';
				end if;
            end;
		else
			CREATE TABLE `tbl_idm_dchg_plmc` (
			  `dcplmc_rowid` bigint NOT NULL AUTO_INCREMENT,
			  `loan_acc` bigint DEFAULT NULL,
			  `loan_sub` int DEFAULT NULL,
			  `offc_cd` tinyint DEFAULT NULL,
			  `dcplmc_rqdt` date DEFAULT NULL,
			  `dcplmc_apdt` date DEFAULT NULL,
			  `dcplmc_apau` int DEFAULT NULL,
			  `dcplmc_itmno` int DEFAULT NULL,
			  `dcplmc_dets` varchar(600) DEFAULT NULL,
			  `dcplmc_sup` varchar(80) DEFAULT NULL,
			  `dcplmc_inv_no` varchar(20) DEFAULT NULL,
			  `dcplmc_inv_dt` date DEFAULT NULL,
			  `dcplmc_reg` int DEFAULT NULL,
			  `dcplmc_qty` int DEFAULT NULL,
			  `dcplmc_stat` varchar(20) DEFAULT NULL,
			  `dcplmc_delry` int DEFAULT NULL,
			  `dcplmc_cst` decimal(10,2) DEFAULT NULL,
			  `dcplmc_tax` int DEFAULT NULL,
			  `dcplmc_tcost` decimal(10,2) DEFAULT NULL,
			  `dcplmc_comdt` date DEFAULT NULL,
			  `dcplmc_rqno` int DEFAULT NULL,
			  `dcplmc_supadr1` varchar(30) DEFAULT NULL,
			  `dcplmc_supadr2` varchar(30) DEFAULT NULL,
			  `dcplmc_supadr3` varchar(30) DEFAULT NULL,
			  `dcplmc_clet_stat` int DEFAULT NULL,
			  `dcplmc_aqrd_stat` int DEFAULT NULL,
			  `dcplmc_actual_cost` int DEFAULT NULL,
			  `dcplmc_bank_advice` varchar(3) DEFAULT NULL,
			  `dcplmc_bank_name` varchar(20) DEFAULT NULL,
			  `dcplmc_bank_advdate` date DEFAULT NULL,
			  `dcplmc_aqrd_indicator` int DEFAULT NULL,
			  `dcplmc_stat_chgdate` date DEFAULT NULL,
			  `dcplmc_clet_validity` varchar(1) DEFAULT NULL,
			  `dcplmc_sec` int DEFAULT NULL,
			  `dcplmc_ino` int DEFAULT NULL,
			  `dcplmc_sec_rel` varchar(50) DEFAULT NULL,
			  `dcplmc_sec_elig` bit(1) DEFAULT NULL,
			  `dcplmc_basic_cost` decimal(10,2) DEFAULT NULL,
			  `unique_id` varchar(200) DEFAULT NULL,
			  `created_by` varchar(50) DEFAULT NULL,
			  `modified_by` varchar(50) DEFAULT NULL,
			  `created_date` datetime DEFAULT NULL,
			  `modified_date` datetime DEFAULT NULL,
			  `is_active` bit(1) DEFAULT NULL,
			  `is_deleted` bit(1) DEFAULT NULL,
			  PRIMARY KEY (`dcplmc_rowid`),
			  UNIQUE KEY `unique_id` (`unique_id`),
			  KEY `loan_acc_idx` (`loan_acc`),
			  KEY `offc_cd_idx` (`offc_cd`)
			);
            select 'tbl_idm_dchg_plmc Table Created' as '';
		END IF;
		END;

		-- -----------tbl_dsb_stat_imp   -----------------------------

		Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_dsb_stat_imp')

		-- If exists, retreive columns information from that table
		THEN
		 select 'tbl_dsb_stat_imp Table Already Exist' as '';
		 else
		 CREATE TABLE `tbl_dsb_stat_imp` (
		 `dsb_id` int NOT NULL AUTO_INCREMENT,
		 `loan_acc` bigint  NOT NULL,
		 `loan_sub` int DEFAULT NULL,
		 `offc_cd`  tinyint DEFAULT NULL,
		 `dsb_offc` int(2) NOT NULL,
		 `dsb_unit` int(6) NOT NULL,
		 `dsb_sno` int(2) NOT NULL,
		 `dsb_ino` int(2) DEFAULT NULL,
		 `dsb_imp_stat` varchar(750) DEFAULT NULL,
		 `dsb_name_pl` int(1)  DEFAULT NULL,
		 `dsb_progimp_bldg` varchar(100)  DEFAULT NULL,
		 `dsb_progimp_mc`  varchar(100)  DEFAULT NULL,
		 `dsb_bldg_val` decimal(10,2)  DEFAULT NULL,
		 `dsb_mc_val` decimal(10,2)  DEFAULT NULL,
		 `dsb_phy_prg` decimal(8,2)  DEFAULT NULL,
		 `dsb_val_prg` decimal(8,2)  DEFAULT NULL,
		 `dsb_tmcst_ovr` varchar(300) DEFAULT NULL,
		 `dsb_rec` varchar(2000) DEFAULT NULL,
		 `dsb_compl_dt` date DEFAULT NULL,
		 `dsb_bal_bldg` varchar(750) DEFAULT NULL,
		 `created_by` varchar(50) DEFAULT NULL,
		 `modified_by` varchar(50) DEFAULT NULL,
	     `created_date` datetime DEFAULT NULL,
		 `modified_date` datetime DEFAULT NULL,
		 `is_active` bit(1) DEFAULT NULL,
		 `is_deleted` bit(1) DEFAULT NULL,
		 `unique_id` varchar(200) DEFAULT NULL,
		  PRIMARY KEY (`dsb_id`)
		  );
		  select 'tbl_dsb_stat_imp Table Created' as '';
		END IF;
		END;




						-- tbl_state_zone_cdtab--
		Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_state_zone_cdtab')

		-- If exists, retreive columns information from that table
		THEN
		 select 'tbl_state_zone_cdtab Table Already Exist' as '';
		
		else
				CREATE TABLE `tbl_state_zone_cdtab` (
  `state_zone_cd` int NOT NULL AUTO_INCREMENT,
  `state_zone_desc` varchar(200) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) DEFAULT NULL,
  `modified_by` varchar(50) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  PRIMARY KEY (`state_zone_cd`)
);
            select 'tbl_state_zone_cdtab Table Created' as '';
		END IF;
		END;
							-- tbl_procure_mast--
		Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_procure_mast')

		-- If exists, retreive columns information from that table
		THEN
		 select 'tbl_procure_mast Table Already Exist' as '';
		
		else
CREATE TABLE `tbl_procure_mast` (
  `procure_id` int NOT NULL,
  `procure_desc` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`procure_id`)
);
            select 'tbl_procure_mast Table Created' as '';
		END IF;
		END;

						-- tbl_machinery_status--
		Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_machinery_status')

		-- If exists, retreive columns information from that table
		THEN
		 select 'tbl_machinery_status Table Already Exist' as '';
		
		else
CREATE TABLE `tbl_machinery_status` (
  `mac_status` int NOT NULL,
  `mac_dets` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`mac_status`)
);
            select 'tbl_machinery_status Table Created' as '';
		END IF;
		END;

	

	-- -----------------------Inspection Detail  ----------------------------------
						-- idm_dsp_insp--
		Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_dsp_insp')

		-- If exists, retreive columns information from that table
		THEN
		 select 'tbl_idm_dsp_insp Table Already Exist' as '';
		Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dsp_insp' AND column_name='loan_acc')
						THEN
                        ALTER TABLE tbl_idm_dsp_insp
							MODIFY COLUMN  loan_acc  bigint NOT NULL;
								  select 'Column loan_acc in tbl_idm_dsp_insp table Modified' as ' ';
						ELSE
							
						select 'Column loan_acc in tbl_idm_dsp_insp table Created' as ' ';
				end if;
            end;
		Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dsp_insp' AND column_name='din_team')
						THEN
                        ALTER TABLE tbl_idm_dsp_insp
							MODIFY COLUMN  din_team  varchar(8) DEFAULT NULL;
								  select 'Column din_team in tbl_idm_dsp_insp table Modified' as ' ';
						ELSE
							
						select 'Column din_team in tbl_idm_dsp_insp table Created' as ' ';
				end if;
            end;
		else
			CREATE TABLE `tbl_idm_dsp_insp` (
			  `din_rowid` bigint NOT NULL AUTO_INCREMENT,
			  `loan_acc` bigint DEFAULT NULL,
			  `loan_sub` int DEFAULT NULL,
			  `offc_cd` tinyint DEFAULT NULL,
			  `din_no` int NOT NULL,
			  `din_dt` date DEFAULT NULL,
			  `din_team` varchar(8) DEFAULT NULL,
			  `din_dept` int NOT NULL,
			  `din_rdt` date DEFAULT NULL,
			  `din_seccd` int DEFAULT NULL,
			  `din_secamt` decimal(10,2) NOT NULL,
			  `din_secrt` int DEFAULT NULL,
			  `din_land` int DEFAULT NULL,
			  `din_land_area` int DEFAULT NULL,
			  `din_land_dev` int DEFAULT NULL,
			  `is_active` bit(1) DEFAULT NULL,
			  `is_deleted` bit(1) DEFAULT NULL,
			  `created_by` varchar(50) DEFAULT NULL,
			  `modified_by` varchar(50) DEFAULT NULL,
			  `created_date` datetime DEFAULT NULL,
			  `modified_date` datetime DEFAULT NULL,
			  PRIMARY KEY (`din_rowid`),
			  KEY `offc_cd` (`offc_cd`),
			  KEY `tbl_idm_unit_details_idfk_7_idx` (`loan_acc`),
			  CONSTRAINT `tbl_idm_unit_details_ibfk_1` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`),
			  CONSTRAINT `tbl_idm_unit_details_idfk_2` FOREIGN KEY (`loan_acc`) REFERENCES `tbl_app_loan_mast` (`ln_no`)
			);
            select 'idm_dsp_insp Table Created' as '';
		END IF;
		END;

-- -----------------------Furniture/Equipment Inspection Detail  ----------------------------------
						-- tbl_idm_dchg_furn--
		Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_dchg_furn')

		-- If exists, retreive columns information from that table
		THEN
		 select 'tbl_idm_dchg_furn Table Already Exist' as '';
		Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_furn' AND column_name='loan_acc')
						THEN
                        ALTER TABLE tbl_idm_dchg_furn
							MODIFY COLUMN  loan_acc  bigint NOT NULL;
								  select 'Column loan_acc in tbl_idm_dchg_furn table Modified' as ' ';
						ELSE
							
						select 'Column loan_acc in tbl_idm_dchg_furn table Created' as ' ';
				end if;
            end;
		Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_furn' AND column_name='stat')
						THEN
                        ALTER TABLE tbl_idm_dchg_furn
							MODIFY COLUMN  stat  int DEFAULT NULL;
								  select 'Column stat in tbl_idm_dchg_furn table Modified' as ' ';
						ELSE
							
						select 'Column stat in tbl_idm_dchg_furn table Created' as ' ';
				end if;
            end;
		Begin
			IF Not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_furn' AND column_name='dfurn_sec_rel')
						THEN
                        ALTER TABLE tbl_idm_dchg_furn
						ADD COLUMN `dfurn_sec_rel` decimal(20,2) DEFAULT NULL AFTER `unique_id`;
								  select 'Column stat in tbl_idm_dchg_furn table Modified' as ' ';
						ELSE
							
						select 'Column dfurn_sec_rel in tbl_idm_dchg_furn table Created' as ' ';
				end if;
            end;
		else
		CREATE TABLE `tbl_idm_dchg_furn` (
			  `dfurn_rowid` bigint NOT NULL AUTO_INCREMENT,
			  `loan_acc` bigint DEFAULT NULL,
			  `loan_sub` int DEFAULT NULL,
			  `offc_cd` tinyint DEFAULT NULL,
			  `dfurn_dets` varchar(200) DEFAULT NULL,
			  `dfurn_sup` varchar(100) DEFAULT NULL,
			  `dfurn_supadr1` varchar(100) DEFAULT NULL,
			  `dfurn_supadr2` varchar(100) DEFAULT NULL,
			  `dfurn_supadr3` varchar(100) DEFAULT NULL,
			  `dfurn_inv_no` int DEFAULT NULL,
			  `dfurn_inv_dt` datetime DEFAULT NULL,
			  `dfurn_clet_stat` int DEFAULT NULL,
			  `dfurn_reg` int DEFAULT NULL,
			  `dfurn_qty` int DEFAULT NULL,
			  `stat` int DEFAULT NULL,
			  `dfurn_delry` int DEFAULT NULL,
			  `dfurn_cst` int DEFAULT NULL,
			  `dfurn_tax` decimal(18,2) DEFAULT NULL,
			  `dfurn_totcst` decimal(18,2) DEFAULT NULL,
			  `dfurn_itm_no` int DEFAULT NULL,
			  `dfurn_aqrd_stat` int DEFAULT NULL,
			  `dfurn_rqdt` datetime DEFAULT NULL,
			  `dfurn_apdt` int DEFAULT NULL,
			  `dfurn_apau` int DEFAULT NULL,
			  `dfurn_non_ssi` int DEFAULT NULL,
			  `dfurn_actual_cost` decimal(18,2) DEFAULT NULL,
			  `dfurn_aqrd_indicator` int DEFAULT NULL,
			  `dfurn_stat_chgdate` datetime DEFAULT NULL,
			  `dfurn_bank_advice` varchar(100) DEFAULT NULL,
			  `dfurn_bank_advdate` varchar(100) DEFAULT NULL,
			  `dfurn_bank_name` varchar(100) DEFAULT NULL,
			  `dfurn_stat` int DEFAULT NULL,
			  `dfurn_sec` int DEFAULT NULL,
			  `dfurn_ino` bigint DEFAULT NULL,
			  `is_active` bit(1) DEFAULT NULL,
			  `is_deleted` bit(1) DEFAULT NULL,
			  `created_by` varchar(50) DEFAULT NULL,
			  `modified_by` varchar(50) DEFAULT NULL,
			  `created_date` datetime DEFAULT NULL,
			  `modified_date` datetime DEFAULT NULL,
			  `unique_id` varchar(150) DEFAULT NULL,
			   `dfurn_sec_rel` decimal(20,2) DEFAULT NULL,
			  PRIMARY KEY (`dfurn_rowid`),
			  KEY `fk_tbl_idm_dchg_furn_offc_cdtab` (`offc_cd`),
			  KEY `fk_tbl_idm_dchg_furn_tbl_app_loan_mast` (`loan_acc`),
			  CONSTRAINT `fk_tbl_idm_dchg_furn_offc_cdtab` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`),
			  CONSTRAINT `fk_tbl_idm_dchg_furn_tbl_app_loan_mast` FOREIGN KEY (`loan_acc`) REFERENCES `tbl_app_loan_mast` (`ln_no`)
			) ;
            select 'tbl_idm_dchg_furn Table Created' as '';
		END IF;
		END;

		
-- -----------------------Import Machinery Inspection Detail  ----------------------------------
						-- tbl_idm_dchg_imp_mc--
		Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_dchg_imp_mc')

		-- If exists, retreive columns information from that table
		THEN
		 select 'tbl_idm_dchg_imp_mc Table Already Exist' as '';
		Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_imp_mc' AND column_name='loan_acc')
						THEN
                        ALTER TABLE tbl_idm_dchg_imp_mc
							MODIFY COLUMN  loan_acc  bigint NOT NULL;
								  select 'Column loan_acc in tbl_idm_dchg_imp_mc table Modified' as ' ';
						ELSE
							
						select 'Column loan_acc in tbl_idm_dchg_imp_mc table Created' as ' ';
				end if;

				IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_imp_mc' AND column_name='dimc_itm_no')
						THEN
                       ALTER TABLE tbl_idm_dchg_imp_mc 
					  MODIFY COLUMN dimc_itm_no bigint DEFAULT NULL;
							  select 'Column dimc_itm_no in tbl_idm_dchg_imp_mc table Modified' as ' ';
						ELSE
						select 'Column dimc_itm_no in tbl_idm_dchg_imp_mc Created' as ' ';
				end if;
				IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_imp_mc' AND column_name='dimc_exrd')
						THEN
                       ALTER TABLE tbl_idm_dchg_imp_mc 
					  MODIFY COLUMN dimc_exrd bigint DEFAULT NULL;
							  select 'Column dimc_exrd in tbl_idm_dchg_imp_mc table Modified' as ' ';
						ELSE
						select 'Column dimc_exrd in tbl_idm_dchg_imp_mc Created' as ' ';
				end if;
				IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_imp_mc' AND column_name='dimc_cduty')
						THEN
                       ALTER TABLE tbl_idm_dchg_imp_mc 
					  MODIFY COLUMN dimc_cduty bigint DEFAULT NULL;
							  select 'Column dimc_cduty in tbl_idm_dchg_imp_mc table Modified' as ' ';
						ELSE
						select 'Column dimc_cduty in tbl_idm_dchg_imp_mc Created' as ' ';
				end if;
				IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_imp_mc' AND column_name='dimc_cif')
						THEN
                       ALTER TABLE tbl_idm_dchg_imp_mc 
					  MODIFY COLUMN dimc_cif bigint DEFAULT NULL;
							  select 'Column dimc_cif in tbl_idm_dchg_imp_mc table Modified' as ' ';
						ELSE
						select 'Column dimc_cif in tbl_idm_dchg_imp_mc Created' as ' ';
				end if;
				IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_imp_mc' AND column_name='dimc_qty')
						THEN
                       ALTER TABLE tbl_idm_dchg_imp_mc 
					  MODIFY COLUMN dimc_qty bigint DEFAULT NULL;
							  select 'Column dimc_qty in tbl_idm_dchg_imp_mc table Modified' as ' ';
						ELSE
						select 'Column dimc_qty in tbl_idm_dchg_imp_mc Created' as ' ';
				end if;
				IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_imp_mc' AND column_name='dimc_delry')
						THEN
                       ALTER TABLE tbl_idm_dchg_imp_mc 
					  MODIFY COLUMN dimc_delry bigint DEFAULT NULL;
							  select 'Column dimc_delry in tbl_idm_dchg_imp_mc table Modified' as ' ';
						ELSE
						select 'Column dimc_delry in tbl_idm_dchg_imp_mc Created' as ' ';
				end if;
				IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_imp_mc' AND column_name='dimc_tot_cost')
						THEN
                       ALTER TABLE tbl_idm_dchg_imp_mc
					  MODIFY COLUMN  dimc_tot_cost decimal(10,2) DEFAULT NULL;
							  select 'Column dclnd_area in tbl_idm_dchg_imp_mc table Modified' as ' ';
						ELSE
						select 'Column dclnd_area in tbl_idm_dchg_imp_mc Created' as ' ';
				end if;
				IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_imp_mc' AND column_name='dimc_qty')
						THEN
                       ALTER TABLE tbl_idm_dchg_imp_mc
					  MODIFY COLUMN  dimc_qty bigint DEFAULT NULL;
							  select 'Column dimc_qty in tbl_idm_dchg_imp_mc table Modified' as ' ';
						ELSE
						select 'Column dimc_qty in tbl_idm_dchg_imp_mc Created' as ' ';
				end if;
				IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_imp_mc' AND column_name='dimc_cpct')
						THEN
                       ALTER TABLE tbl_idm_dchg_imp_mc
					  MODIFY COLUMN  dimc_cpct bigint DEFAULT NULL;
							  select 'Column dimc_cpct in tbl_idm_dchg_imp_mc table Modified' as ' ';
						ELSE
						select 'Column dimc_cpct in tbl_idm_dchg_imp_mc Created' as ' ';
				end if;
				IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_imp_mc' AND column_name='dimc_camt')
						THEN
                       ALTER TABLE tbl_idm_dchg_imp_mc
					  MODIFY COLUMN  dimc_camt decimal(10,2) DEFAULT NULL;
							  select 'Column dimc_camt in tbl_idm_dchg_imp_mc table Modified' as ' ';
						ELSE
						select 'Column dimc_camt in tbl_idm_dchg_imp_mc Created' as ' ';
				end if;
				IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_imp_mc' AND column_name='dimc_apau')
						THEN
                       ALTER TABLE tbl_idm_dchg_imp_mc
					  MODIFY COLUMN  dimc_apau  bigint DEFAULT NULL;
							  select 'Column dimc_apau in tbl_idm_dchg_imp_mc table Modified' as ' ';
						ELSE
						select 'Column dimc_apau in tbl_idm_dchg_imp_mc Created' as ' ';
				end if;
				IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_imp_mc' AND column_name='dimc_ino')
						THEN
                       ALTER TABLE tbl_idm_dchg_imp_mc
					  MODIFY COLUMN  dimc_ino  bigint DEFAULT NULL;
							  select 'Column dimc_ino in tbl_idm_dchg_imp_mc table Modified' as ' ';
						ELSE
						select 'Column dimc_ino in tbl_idm_dchg_imp_mc Created' as ' ';
				end if;
				IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_imp_mc' AND column_name='dimc_sec_rel')
						THEN
                       ALTER TABLE tbl_idm_dchg_imp_mc
					  MODIFY COLUMN  dimc_sec_rel  bigint DEFAULT NULL;
							  select 'Column dimc_apau in tbl_idm_dchg_imp_mc table Modified' as ' ';
						ELSE
						select 'Column dimc_apau in tbl_idm_dchg_imp_mc Created' as ' ';
				end if;
				IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_imp_mc' AND column_name='dimc_sec_elig')
						THEN
                       ALTER TABLE tbl_idm_dchg_imp_mc
					  MODIFY COLUMN  dimc_sec_elig  bigint DEFAULT NULL;
							  select 'Column dimc_apau in tbl_idm_dchg_imp_mc table Modified' as ' ';
						ELSE
						select 'Column dimc_apau in tbl_idm_dchg_imp_mc Created' as ' ';
				end if;
				IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_imp_mc' AND column_name='dimc_stat_desc')
						THEN
                       ALTER TABLE tbl_idm_dchg_imp_mc
					  Add COLUMN  dimc_stat_desc  Varchar(100) DEFAULT NULL;
							  select 'Column dimc_stat_desc in tbl_idm_dchg_imp_mc table Modified' as ' ';
						ELSE
						select 'Column dimc_stat_desc in tbl_idm_dchg_imp_mc Created' as ' ';
				end if;
            end;
			Begin
			IF Not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_imp_mc' AND column_name='dimc_gst')
						THEN
                        ALTER TABLE tbl_idm_dchg_imp_mc
						ADD COLUMN `dimc_gst` decimal(20,2) DEFAULT NULL AFTER `dimc_stat_desc`;
								  select 'Column dimc_gst in tbl_idm_dchg_imp_mc table Modified' as ' ';
						ELSE
							
						select 'Column dimc_gst in tbl_idm_dchg_imp_mc table Created' as ' ';
				end if;
            end;
			Begin
			IF Not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_imp_mc' AND column_name='dimc_othexp')
						THEN
                        ALTER TABLE tbl_idm_dchg_imp_mc
						ADD COLUMN `dimc_othexp` decimal(20,2) DEFAULT NULL AFTER `dimc_gst`;
								  select 'Column dimc_othexp in tbl_idm_dchg_imp_mc table Modified' as ' ';
						ELSE
							
						select 'Column dimc_othexp in tbl_idm_dchg_imp_mc table Created' as ' ';
				end if;
            end;
			Begin
			IF Not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_imp_mc' AND column_name='dimc_totcost_mem')
						THEN
                        ALTER TABLE tbl_idm_dchg_imp_mc
						ADD COLUMN `dimc_totcost_mem` decimal(20,2) DEFAULT NULL AFTER `dimc_othexp`;
								  select 'Column dimc_totcost_mem in tbl_idm_dchg_imp_mc table Modified' as ' ';
						ELSE
							
						select 'Column dimc_totcost_mem in tbl_idm_dchg_imp_mc table Created' as ' ';
				end if;
            end;
			Begin
			IF Not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_imp_mc' AND column_name='dimc_cur_basicamt')
						THEN
                        ALTER TABLE tbl_idm_dchg_imp_mc
						ADD COLUMN `dimc_cur_basicamt` decimal(20,2) DEFAULT NULL AFTER `dimc_crncy`;
								  select 'Column dimc_cur_basicamt in tbl_idm_dchg_imp_mc table Modified' as ' ';
						ELSE
							
						select 'Column dimc_cur_basicamt in tbl_idm_dchg_imp_mc table Created' as ' ';
				end if;
            end;
			Begin
			IF Not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_imp_mc' AND column_name='dimc_basicamt')
						THEN
                        ALTER TABLE tbl_idm_dchg_imp_mc
						ADD COLUMN `dimc_basicamt` decimal(20,2) DEFAULT NULL AFTER `dimc_cur_basicamt`;
								  select 'Column dimc_basicamt in tbl_idm_dchg_imp_mc table Modified' as ' ';
						ELSE
							
						select 'Column dimc_basicamt in tbl_idm_dchg_imp_mc table Created' as ' ';
				end if;
            end;

		else
		CREATE TABLE `tbl_idm_dchg_imp_mc` (
			  `dimc_rowid` bigint NOT NULL AUTO_INCREMENT,
			  `loan_acc` bigint NOT NULL,
			  `offc_cd` tinyint DEFAULT NULL,
			  `loan_sub` int DEFAULT NULL,
			  `dimc_itm_no` bigint DEFAULT NULL,
			  `dimc_entry` varchar(20) DEFAULT NULL,
			  `dimc_entry_i` varchar(20) DEFAULT NULL,
			  `dimc_crncy` varchar(10) DEFAULT NULL,
			  `dimc_cur_basicamt` decimal(20,2) DEFAULT NULL,
			  `dimc_basicamt` decimal(20,2) DEFAULT NULL,
			  `dimc_exrd` bigint DEFAULT NULL,
			  `dimc_cduty` bigint DEFAULT NULL,
			  `dimc_tot_cost` decimal(10,2) DEFAULT NULL,
			  `dimc_stat` int DEFAULT NULL,
			  `dimc_dets` varchar(200) DEFAULT NULL,
			  `dimc_sup` varchar(30) DEFAULT NULL,
			  `dimc_qty` bigint DEFAULT NULL,
			  `dimc_delry` bigint DEFAULT NULL,
			  `dimc_supadr1` varchar(30) DEFAULT NULL,
			  `dimc_supadr2` varchar(30) DEFAULT NULL,
			  `dimc_supadr3` varchar(30) DEFAULT NULL,
			  `dimc_cpct` bigint DEFAULT NULL,
			  `dimc_camt` decimal(10,2) DEFAULT NULL,
			  `dimc_aqrd_stat` int DEFAULT NULL,
			  `dimc_apau` bigint DEFAULT NULL,
			  `dimc_apdate` datetime DEFAULT NULL,
			  `dimc_clet_stat` varchar(1) DEFAULT NULL,
			  `dimc_actual_cost` decimal(10,2) DEFAULT NULL,
			  `dimc_aqrd_indicator` bigint DEFAULT NULL,
			  `dimc_bank_advice` varchar(20) DEFAULT NULL,
			  `dimc_cif` bigint DEFAULT NULL,
			  `dimc_bank_advdate` datetime DEFAULT NULL,
			  `dimc_mac_documents` varchar(30) DEFAULT NULL,
			  `dimc_stat_chgdate` datetime DEFAULT NULL,
			  `dimc_sec` bigint DEFAULT NULL,
			  `dimc_ino` bigint DEFAULT NULL,
			  `dimc_sec_rel` bigint DEFAULT NULL,
			  `dimc_sec_elig` bigint DEFAULT NULL,
			  `created_by` varchar(50) DEFAULT NULL,
			  `created_date` datetime DEFAULT NULL,
			  `modified_by` varchar(50) DEFAULT NULL,
			  `modified_date` datetime DEFAULT NULL,
			  `unique_id` varchar(150) DEFAULT NULL,
			  `is_active` bit(1) NOT NULL DEFAULT b'1',
			  `is_deleted` bit(1) NOT NULL DEFAULT b'0',
			  `dimc_stat_desc` varchar(100) DEFAULT NULL,
			  `dimc_gst` decimal(20,2) DEFAULT NULL,
			  `dimc_othexp` decimal(20,2) DEFAULT NULL,
			  `dimc_totcost_mem` decimal(20,2) DEFAULT NULL,
			  PRIMARY KEY (`dimc_rowid`),
			  KEY `fk_tbl_idm_dchg_imp_mc_tbl_app_loan_mast_idx` (`loan_acc`),
			  KEY `fk_tbl_idm_dchg_imp_mc_offc_cdtab_idx` (`offc_cd`),
			  CONSTRAINT `fk_tbl_idm_dchg_imp_mc_offc_cdtab` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`),
			  CONSTRAINT `fk_tbl_idm_dchg_imp_mc_tbl_app_loan_mast` FOREIGN KEY (`loan_acc`) REFERENCES `tbl_app_loan_mast` (`ln_no`)
			)  ;
            select 'tbl_idm_dchg_imp_mc Table Created' as '';
		END IF;
		END;

		-- ----------------------Working Capital Inspection Detail  ----------------------------------
									-- tbl_idm_dchg_wc--
		Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_dchg_wc')

		-- If exists, retreive columns information from that table
		THEN
		 select 'tbl_idm_dchg_wc Table Already Exist' as '';
		Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_wc' AND column_name='loan_acc')
						THEN
                        ALTER TABLE tbl_idm_dchg_wc
							MODIFY COLUMN  loan_acc  bigint NOT NULL;
								  select 'Column loan_acc in tbl_idm_dchg_wc table Modified' as ' ';
						ELSE
							
						select 'Column loan_acc in tbl_idm_dchg_wc table Created' as ' ';
				end if;
            end;


			Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND  TABLE_NAME='tbl_idm_dchg_wc' AND column_name='dcwc_ino')
					THEN
								select 'Column dcwc_ino in tbl_idm_dchg_wc Already Exist' as ' ';
					ELSE
							ALTER TABLE `tbl_idm_dchg_wc`
							Add `dcwc_ino` int DEFAULT NULL;
						select 'Column dcwc_ino in tbl_idm_dchg_wc Created' as ' ';
					end if;
			    end;

else
		CREATE TABLE `tbl_idm_dchg_wc` (
				`dcwc_rowid` bigint NOT NULL AUTO_INCREMENT,
				`loan_acc` bigint NOT NULL,
				`loan_sub` int DEFAULT NULL,
				`offc_cd` tinyint DEFAULT NULL,
				`dcwc_obank` varchar(30) DEFAULT NULL,
				`dcwc_nbank` varchar(30) DEFAULT NULL,
				`dcwc_onoc` varchar(30) DEFAULT NULL,
				`dcwc_onocdt` datetime DEFAULT NULL,
				`dcwc_sandt` datetime DEFAULT NULL,
				`dcwc_amount` decimal(10,2) DEFAULT NULL,
				`dcwc_rem` varchar(40) DEFAULT NULL,
				`dcwc_memdt` datetime DEFAULT NULL,
				 `dcwc_ino` int DEFAULT NULL,
				`dcwc_nbnkadr1` varchar(30) DEFAULT NULL,
				`dcwc_nbnkadr2` varchar(30) DEFAULT NULL,
				`dcwc_nbnkadr3` varchar(30) DEFAULT NULL,
				`created_by` varchar(50) DEFAULT NULL,
				`created_date` datetime DEFAULT NULL,
				`modified_by` varchar(50) DEFAULT NULL,
				`modified_date` datetime DEFAULT NULL,
				`unique_id` varchar(150) DEFAULT NULL,
				`is_active` bit(1) NOT NULL DEFAULT b'1',
				`is_deleted` bit(1) NOT NULL DEFAULT b'0',
				`dcwc_ino` int DEFAULT NULL,
				PRIMARY KEY (`dcwc_rowid`),
				KEY `fk_tbl_idm_dchg_wc_tbl_app_loan_mast_idx` (`loan_acc`),
				KEY `fk_tbl_idm_dchg_wc_offc_cdtab_idx` (`offc_cd`),
				CONSTRAINT `fk_tbl_idm_dchg_wc_offc_cdtab` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`),
				CONSTRAINT `fk_tbl_idm_dchg_wc_tbl_app_loan_mast` FOREIGN KEY (`loan_acc`) REFERENCES `tbl_app_loan_mast` (`ln_no`)
			) ;
            select 'tbl_idm_dchg_wc Table Created' as '';
		END IF;
		END;

		-- ----------------------Means Of Finance  Detail  ----------------------------------
									-- `tbl_idm_dchg_mf--
		Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_dchg_mf')

		-- If exists, retreive columns information from that table
		THEN
		 select '`tbl_idm_dchg_mf Table Already Exist' as '';
		Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_mf' AND column_name='loan_acc')
						THEN
                        ALTER TABLE tbl_idm_dchg_mf
							MODIFY COLUMN  loan_acc  bigint NOT NULL;
								  select 'Column loan_acc in tbl_idm_dchg_mf table Modified' as ' ';
						ELSE
							
						select 'Column loan_acc in tbl_idm_dchg_mf table Created' as ' ';
				end if;
            end;
		else
		CREATE TABLE `tbl_idm_dchg_mf` (
		`dcmf_rowid` Bigint NOT NULL AUTO_INCREMENT,
		`loan_acc` bigint DEFAULT NULL,
		`loan_sub` int DEFAULT NULL,
		`offc_cd` tinyint DEFAULT NULL,
		`dcmf_rqdt` DateTime DEFAULT NULL,
		`dcmf_apdt` DateTime DEFAULT NULL,
		`dcmf_apau` int DEFAULT NULL,
		`dcmf_cd` int DEFAULT NULL,
		`dcmf_amt` decimal(10,2) DEFAULT NULL,
		`dcmf_mf_type` int DEFAULT NULL,
		`created_by` varchar(50) DEFAULT NULL,
		`created_date` datetime DEFAULT NULL,
		`modified_by` varchar(50) DEFAULT NULL,
		`modified_date` datetime DEFAULT NULL,
		`unique_id` varchar(150) DEFAULT NULL,
		`is_active` bit(1) NOT NULL DEFAULT 1,
		`is_deleted` bit(1) NOT NULL DEFAULT 0,
		PRIMARY KEY (`dcmf_rowid`),
		KEY `fk_tbl_idm_dchg_mf_tbl_app_loan_mast_idx` (`loan_acc`),
		KEY `fk_tbl_idm_dchg_mf_offc_cdtab_idx` (`offc_cd`),
		CONSTRAINT `fk_tbl_idm_dchg_mf_offc_cdtab` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`),
		CONSTRAINT `fk_tbl_idm_dchg_mf_tbl_app_loan_mast` FOREIGN KEY (`loan_acc`) REFERENCES `tbl_app_loan_mast` (`ln_no`)
		);
            select '`tbl_idm_dchg_mf Table Created' as '';
		END IF;
		END;


		
		-- ----------------------Letter of Creadit Detail  ----------------------------------
									-- tbl_idm_dsb_lt_crdt--
		Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_dsb_lt_crdt')

		-- If exists, retreive columns information from that table
		THEN
		 select 'tbl_idm_dsb_lt_crdt Table Already Exist' as '';
		Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dsb_lt_crdt' AND column_name='loan_acc')
						THEN
                        ALTER TABLE tbl_idm_dsb_lt_crdt
							MODIFY COLUMN  loan_acc  bigint NOT NULL;
								  select 'Column loan_acc in tbl_idm_dsb_lt_crdt table Modified' as ' ';
						ELSE
							
						select 'Column loan_acc in tbl_idm_dsb_lt_crdt table Created' as ' ';
				end if;
            end;
			Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dsb_lt_crdt' AND column_name='dlcrdt_itm_dts')
						THEN
                        
								  select 'Column loan_acc in tbl_idm_dsb_lt_crdt table Modified' as ' ';
						ELSE
							ALTER TABLE tbl_idm_dsb_lt_crdt
							ADD COLUMN  `dlcrdt_itm_dts` varchar(100) DEFAULT NULL;
						select 'Column loan_acc in tbl_idm_dsb_lt_crdt table Created' as ' ';
				end if;
            end;
			Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dsb_lt_crdt' AND column_name='dlcrdt_bank_ifsc')
						THEN
                        ALTER TABLE tbl_idm_dsb_lt_crdt
							MODIFY COLUMN  dlcrdt_bank_ifsc  varchar(50) DEFAULT NULL;
								  select 'Column dlcrdt_bank_ifsc in tbl_idm_dsb_lt_crdt table Modified' as ' ';
						ELSE
							
						select 'Column dlcrdt_bank_ifsc in tbl_idm_dsb_lt_crdt table Created' as ' ';
				end if;
            end;
		Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dsb_lt_crdt' AND column_name='dlcrdt_bank')
						THEN
                        ALTER TABLE tbl_idm_dsb_lt_crdt
							MODIFY COLUMN  dlcrdt_bank  varchar(100) DEFAULT NULL;
								  select 'Column dlcrdt_bank in tbl_idm_dsb_lt_crdt table Modified' as ' ';
						ELSE
							
						select 'Column dlcrdt_bank in tbl_idm_dsb_lt_crdt table Created' as ' ';
				end if;
            end;
		Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dsb_lt_crdt' AND column_name='dlcrdt_bnkadr1')
						THEN
                        ALTER TABLE tbl_idm_dsb_lt_crdt
							MODIFY COLUMN  dlcrdt_bnkadr1  varchar(500) DEFAULT NULL;
								  select 'Column dlcrdt_bnkadr1 in tbl_idm_dsb_lt_crdt table Modified' as ' ';
						ELSE
							
						select 'Column dlcrdt_bnkadr1 in tbl_idm_dsb_lt_crdt table Created' as ' ';
				end if;
            end;
		else
		CREATE TABLE `tbl_idm_dsb_lt_crdt` (
			  `dcloc_rowid` bigint NOT NULL AUTO_INCREMENT,
			  `loan_acc` bigint NOT NULL,
			  `loan_sub` int DEFAULT NULL,
			  `offc_cd` tinyint DEFAULT NULL,
			  `dlcrdt_itm_no` int DEFAULT NULL,
			  `dlcrdt_itm_dts` varchar(100) DEFAULT NULL,
			  `dlcrdt_crlt_no` varchar(10) DEFAULT NULL,
			  `dlcrdt_dt` datetime DEFAULT NULL,
			  `dlcrdt_amt` decimal(10,2) DEFAULT NULL,
			  `dlcrdt_cif` decimal(10,2) DEFAULT NULL,
			  `dlcrdt_bank` varchar(100) DEFAULT NULL,
			  `dlcrdt_bnkadr1` varchar(500) DEFAULT NULL,
			  `dlcrdt_bnkadr2` varchar(30) DEFAULT NULL,
			  `dlcrdt_bnkadr3` varchar(30) DEFAULT NULL,
			  `dlcrdt_rqdt` datetime DEFAULT NULL,
			  `dlcrdt_open_dt` datetime DEFAULT NULL,
			  `dlcrdt_vdty` datetime DEFAULT NULL,
			  `dlcrdt_hondt` datetime DEFAULT NULL,
			  `dlcrdt_au` int DEFAULT NULL,
			  `dlcrdt_clet_stat` int DEFAULT NULL,
			  `dlcrdt_mrg_mny` decimal(10,2) DEFAULT NULL,
			  `dlcrdt_sup` varchar(50) DEFAULT NULL,
			  `dlcrdt_sup_addr` varchar(200) DEFAULT NULL,
			  `dlcrdt_aqrd_stat` int DEFAULT NULL,
			  `dlcrdt_total_amt` decimal(10,2) DEFAULT NULL,
			  `dlcrdt_bank_ifsc` varchar(50) DEFAULT NULL,
			  `created_by` varchar(50) DEFAULT NULL,
			  `created_date` datetime DEFAULT NULL,
			  `modified_by` varchar(50) DEFAULT NULL,
			  `modified_date` datetime DEFAULT NULL,
			  `unique_id` varchar(150) DEFAULT NULL,
			  `is_active` bit(1) DEFAULT NULL,
			  `is_deleted` bit(1) DEFAULT NULL,
			  PRIMARY KEY (`dcloc_rowid`),
			  KEY `fk_tbl_idm_dsb_lt_crdt_offc_cdtab_idx` (`offc_cd`),
			  KEY `fk_tbl_idm_dsb_lt_crdt_tbl_app_loan_master_idx` (`loan_acc`),
			  CONSTRAINT `fk_tbl_idm_dsb_lt_crdt_offc_cdtab` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`),
			  CONSTRAINT `fk_tbl_idm_dsb_lt_crdt_tbl_app_loan_master` FOREIGN KEY (`loan_acc`) REFERENCES `tbl_app_loan_mast` (`ln_no`)
			);
            select 'tbl_idm_dsb_lt_crdt Table Created' as '';
		END IF;
		END;

									-- tbl_idm_dchg_pcost--
		Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_dchg_pcost')

		-- If exists, retreive columns information from that table
		THEN
		 select 'tbl_idm_dchg_pcost Table Already Exist' as '';
		Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_pcost' AND column_name='loan_acc')
						THEN
                        ALTER TABLE tbl_idm_dchg_pcost
							MODIFY COLUMN  loan_acc  bigint NOT NULL;
								  select 'Column loan_acc in tbl_idm_dchg_pcost table Modified' as ' ';
						ELSE
							
						select 'Column loan_acc in tbl_idm_dchg_pcost table Created' as ' ';
				end if;
				IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dchg_pcost' AND column_name='dcpcst_amt')
						THEN
                        ALTER TABLE tbl_idm_dchg_pcost
							MODIFY COLUMN  dcpcst_amt  decimal(10,2) DEFAULT NULL;
								  select 'Column dcpcst_amt in tbl_idm_dchg_pcost table Modified' as ' ';
						ELSE
							
						select 'Column dcpcst_amt in tbl_idm_dchg_pcost table Created' as ' ';
				end if;
            end;
		else
		CREATE TABLE `tbl_idm_dchg_pcost` (
		  `dcpcst_rowid` bigint NOT NULL AUTO_INCREMENT,
		  `loan_acc` bigint DEFAULT NULL,
		  `loan_sub` int DEFAULT NULL,
		  `offc_cd` tinyint DEFAULT NULL,
		  `dcpcst_rqdt` date DEFAULT NULL,
		  `dcpcst_apdt` date DEFAULT NULL,
		  `dcpcst_apau` int DEFAULT NULL,
		  `dcpcst_cd` int DEFAULT NULL,
		  `dcpcst_amt` decimal(10,2) DEFAULT NULL,
		  `dcpcst_comdt` date DEFAULT NULL,
		  `created_by` varchar(50) DEFAULT NULL,
		  `modified_by` varchar(50) DEFAULT NULL,
		  `created_date` datetime DEFAULT NULL,
		  `modified_date` datetime DEFAULT NULL,
		  `unique_id` varchar(200) DEFAULT NULL,
		  `is_active` bit(1) DEFAULT NULL,
		  `is_deleted` bit(1) DEFAULT NULL,
		  PRIMARY KEY (`dcpcst_rowid`),
		  KEY `loan_acc_idx` (`loan_acc`),
		  KEY `offc_cd_idx` (`offc_cd`)
		);
            select 'tbl_idm_dchg_pcost Table Created' as '';
		END IF;
		END;

									-- tbl_pjcostgroup_cdtab--
		

		
		-- 
		Begin
IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_pjcostgroup_cdtab')

		-- If not exists, creat a new table
Then


select 'tbl_pjcostgroup_cdtab Table Existed' as ''; 
ELSE
		CREATE TABLE `tbl_pjcostgroup_cdtab` (
  `pjcostgroup_cd` int NOT NULL AUTO_INCREMENT,
  `pjcostgroup_dets` varchar(100) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `modified_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  PRIMARY KEY (`pjcostgroup_cd`)
);
            select 'tbl_pjcostgroup_cdtab Table Created' as '';
END IF;
END;


									-- tbl_pjcostgroup_cdtab--
		

		
		-- 
		Begin
IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_pjcsgroup_cdtab')

		-- If not exists, creat a new table
Then


select 'tbl_pjcsgroup_cdtab Table Existed' as ''; 
ELSE
	CREATE TABLE `tbl_pjcsgroup_cdtab` (
  `pjcsgroup_cd` int NOT NULL AUTO_INCREMENT,
  `pjcsgroup_dets` varchar(100) DEFAULT NULL,
  `pjcostgroup_cd` int NOT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `modified_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  PRIMARY KEY (`pjcsgroup_cd`),
  KEY `fk_tbl_pjcsgroup_cdtab_tbl_pjcostgroup_cdtab` (`pjcostgroup_cd`),
  CONSTRAINT `fk_tbl_pjcsgroup_cdtab_tbl_pjcostgroup_cdtab` FOREIGN KEY (`pjcostgroup_cd`) REFERENCES `tbl_pjcostgroup_cdtab` (`pjcostgroup_cd`)
);
            select 'tbl_pjcsgroup_cdtab Table Created' as '';
END IF;
END;
									-- tbl_pjcost_cdtab--
		

		
		-- 
		Begin
IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_pjcost_cdtab')

		-- If not exists, creat a new table
Then

ALTER TABLE `tbl_pjcost_cdtab` 
CHANGE COLUMN `created_by` `created_by` VARCHAR(50) CHARACTER SET 'utf8mb4' NULL DEFAULT NULL ,
CHANGE COLUMN `modified_by` `modified_by` VARCHAR(50) CHARACTER SET 'utf8mb4' NULL DEFAULT NULL ,
CHANGE COLUMN `unique_id` `unique_id` VARCHAR(200) CHARACTER SET 'utf8mb4' NULL DEFAULT NULL ;
select 'tbl_pjcost_cdtab Table Modified' as ''; 
ELSE
		CREATE TABLE `tbl_pjcost_cdtab` (
			  `pjcost_cd` int NOT NULL AUTO_INCREMENT,
			  `pjcost_dets` varchar(50) NOT NULL,
			  `pjcost_flg` int DEFAULT NULL,
			  `seq_no` decimal(3,2) DEFAULT NULL,
			  `pjcostgroup_cd` int DEFAULT NULL,
			  `pjcsgroup_cd` int DEFAULT NULL,
			  `is_active` bit(1) DEFAULT NULL,
			  `is_deleted` bit(1) DEFAULT NULL,
			  `created_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
			  `modified_by` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
			  `created_date` datetime DEFAULT NULL,
			  `modified_date` datetime DEFAULT NULL,
			  `unique_id` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
			  PRIMARY KEY (`pjcost_cd`),
			  KEY `fk_tbl_pjcost_cdtab_tbl_pjcostgroup_cdtab` (`pjcostgroup_cd`),
			  KEY `fk_tbl_pjcost_cdtab_tbl_pjcsgroup_cdtab` (`pjcsgroup_cd`),
			  CONSTRAINT `fk_tbl_pjcost_cdtab_tbl_pjcostgroup_cdtab` FOREIGN KEY (`pjcostgroup_cd`) REFERENCES `tbl_pjcostgroup_cdtab` (`pjcostgroup_cd`),
			  CONSTRAINT `fk_tbl_pjcost_cdtab_tbl_pjcsgroup_cdtab` FOREIGN KEY (`pjcsgroup_cd`) REFERENCES `tbl_pjcsgroup_cdtab` (`pjcsgroup_cd`)
			);
            select 'tbl_pjcost_cdtab Table Created' as '';
END IF;
END;

		
END$$

DELIMITER ;

call KSFC_InspectionOfUnit_DDL_SP()




