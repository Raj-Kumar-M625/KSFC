USE `ksfc_oct`;
DROP procedure IF EXISTS `CSAA_Script`;

DELIMITER $$
USE `ksfc_oct`$$
CREATE PROCEDURE `CSAA_Script` ()

BEGIN
DECLARE DBName varchar(50) DEFAULT "ksfc_oct";
-- ----------------------- Land Acquisition Module ----------------------------------


						-- tbl_idm_ir_land --
Begin
IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_ir_land')
                     
			-- If exists, retreive columns information from that table
		THEN
		select 'tbl_idm_ir_land table Already Exist' as ' ';
			Begin
			
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_ir_land' AND column_name='irl_landcost')
						THEN
                       ALTER TABLE tbl_idm_ir_land 
					  MODIFY COLUMN irl_landcost decimal(10,2) DEFAULT NULL;
							  select 'Column irl_landcost in tbl_idm_ir_land table Modified' as ' ';
						ELSE
						select 'Column irl_landcost in tbl_idm_ir_land Created' as ' ';
				end if;

				IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_ir_land' AND column_name='irl_devcost')
						THEN
                       ALTER TABLE tbl_idm_ir_land 
					  MODIFY COLUMN irl_devcost decimal(10,2) DEFAULT NULL;
							  select 'Column irl_devcost in tbl_idm_ir_land table Modified' as ' ';
						ELSE
						select 'Columnirl_devcost in tbl_idm_ir_land Created' as ' ';
				end if;

				IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_ir_land' AND column_name='irl_secvalue')
						THEN
                       ALTER TABLE tbl_idm_ir_land 
					  MODIFY COLUMN irl_secvalue decimal(15,2) DEFAULT NULL;
							  select 'Column irl_secvalue in tbl_idm_ir_land table Modified' as ' ';
						ELSE
						select 'Column irl_secvalue in tbl_idm_ir_land Created' as ' ';
				end if;

            end;
		-- If not exists, creat a new table

		select 'tbl_idm_ir_land Table Exist' as ''; 
		ELSE
   CREATE TABLE `tbl_idm_ir_land` (
  `irl_id` bigint NOT NULL AUTO_INCREMENT,
  `loan_acc` bigint DEFAULT NULL,
  `loan_sub` int DEFAULT NULL,
  `offc_cd` tinyint DEFAULT NULL,
  `irl_ino` bigint DEFAULT NULL,
  `irl_idt` date DEFAULT NULL,
  `irl_rdt` date DEFAULT NULL,
  `irl_area` int DEFAULT NULL,
  `irl_areain` varchar(10) DEFAULT NULL,
  `irl_landcost` decimal(10,2) DEFAULT NULL,
  `irl_devcost` decimal(10,2) DEFAULT NULL,
  `irl_landty` int DEFAULT NULL,
  `irl_rem` varchar(150) DEFAULT NULL,
  `irl_secvalue` decimal(15,2) DEFAULT NULL,
  `irl_rel_stat` int DEFAULT NULL,
  `irl_land_finance` varchar(1) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) DEFAULT NULL,
  `modified_by` varchar(50) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`irl_id`),
  KEY `FK_tbl_idm_ir_land_tbl_app_loan_mast` (`loan_acc`),
  KEY `FK_tbl_idm_ir_land_offc_cdtab_offc_cdtab` (`offc_cd`),
CONSTRAINT `FK_tbl_idm_ir_land_tbl_app_loan_mast` FOREIGN KEY (`loan_acc`) REFERENCES `tbl_app_loan_mast` (`ln_no`),
  CONSTRAINT `FK_tbl_idm_ir_land_offc_cdtab_offc_cdtab` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`)
);

select 'tbl_idm_ir_land Table Created' as ''; 
END IF;
END;
                 
-- ----------------------- Building Acquisition Module ------------------------------
						-- table name --
        						-- tbl_idm_id_bldg--
		Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_id_bldg')

		-- If exists, retreive columns information from that table
		THEN
		select 'tbl_idm_id_bldg table already exist ' as ' ';

		       Begin   
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_id_bldg' AND column_name='irb_unit_cost')
					THEN
				        select 'Column irb_unit_cost in tbl_idm_id_bldg Already Exist' as ' ';
                               
					ELSE
					ALTER TABLE `tbl_idm_id_bldg`
							Add `irb_unit_cost`  decimal(20,2) DEFAULT NULL;
						 select 'Column irb_unit_cost in tbl_idm_id_bldg Created' as ' ';
					end if;
			    END;

				Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_id_bldg' AND column_name='rf_ty')
					THEN
				        select 'Column rf_ty in tbl_idm_id_bldg Already Exist' as ' ';
                               
					ELSE
					ALTER TABLE `tbl_idm_id_bldg`
							Add `rf_ty`  varchar(50) DEFAULT NULL;
						 select 'Column rf_ty in tbl_idm_id_bldg Created' as ' ';
					end if;
			    END;

				Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_id_bldg' AND column_name='irb_cost')
					THEN
				        select 'Column irb_cost in tbl_idm_id_bldg Already Exist' as ' ';
                               
					ELSE
					ALTER TABLE `tbl_idm_id_bldg`
							Add `irb_cost` DECIMAL(20,0) DEFAULT NULL;
						 select 'Column irb_cost in tbl_idm_id_bldg Created' as ' ';
					end if;
			    END;
				Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_id_bldg' AND column_name='irb_ino')
					THEN
				        select 'Column irb_ino in tbl_idm_id_bldg Already Exist' as ' ';
                               
					ELSE
					ALTER TABLE `tbl_idm_id_bldg`
							Add `irb_ino`int DEFAULT NULL;
						 select 'Column irb_ino in tbl_idm_id_bldg Created' as ' ';
					end if;
			    END;

		else
	CREATE TABLE `tbl_idm_id_bldg` (
  `irb_id` bigint NOT NULL AUTO_INCREMENT,
  `loan_acc` bigint DEFAULT NULL,
  `loan_sub` int DEFAULT NULL,
  `offc_cd` tinyint DEFAULT NULL,
  `irb_idt` date DEFAULT NULL,
  `irb_rdt` date DEFAULT NULL,
  `irb_itm` int DEFAULT NULL,
  `irb_area` int DEFAULT NULL,
  `irb_value` decimal(20,2) DEFAULT NULL,
  `irb_ino` int DEFAULT NULL,
  `irb_no` int DEFAULT NULL,
  `irb_stat` int DEFAULT NULL,
  `irb_secvalue` int DEFAULT NULL,
  `irb_rel_stat` int DEFAULT NULL,
  `irb_aarea` int DEFAULT NULL,
  `irb_avalue` decimal(20,2) DEFAULT NULL,
  `irb_percent` int DEFAULT NULL,
  `irb_bldgconst_status` varchar(50) DEFAULT NULL,
  `irb_bldg_details` varchar(200) DEFAULT NULL,
  `irb_unit_cost` decimal(20,2) DEFAULT NULL,
  `rf_ty` varchar(50) DEFAULT NULL,
  `irb_cost` decimal(20,0) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) DEFAULT NULL,
  `modified_by` varchar(50) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `unique_id` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`irb_id`),
  KEY `FK_tbl_idm_id_bldg_offc_cdtab` (`offc_cd`),
  CONSTRAINT `FK_tbl_idm_id_bldg_offc_cdtab` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`));
			select 'tbl_idm_id_bldg table Created' as ' ';
		END IF;
		END;   
        
-- ----------------------- Machinery Acquisition Module -----------------------------
						-- tbl_idm_ir_plmc --
Begin
IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_ir_plmc')
                     
			-- If exists, retreive columns information from that table
		THEN
		select 'tbl_idm_ir_plmc table Already Exist' as ' ';
			Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_ir_plmc' AND column_name='irplmc_aqrd_stat')
						THEN
								 ALTER TABLE tbl_idm_ir_plmc
							MODIFY irplmc_aqrd_stat  int DEFAULT NULL;
						ELSE							
						 select 'column irplmc_aqrd_stat tbl_idm_ir_plmc table Created' as ' ';
				end if;
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_ir_plmc' AND column_name='irplmc_ino')
						THEN
						 select 'Column irplmc_ino in tbl_idm_ir_plmc Already Exist' as ' ';
								
						ELSE		
						 ALTER TABLE tbl_idm_ir_plmc
							Add irplmc_ino  Bigint DEFAULT NULL;
						 select 'column irplmc_ino tbl_idm_ir_plmc table Created' as ' ';
				end if;
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_ir_plmc' AND column_name='irplmc_secamt')
						THEN
								 ALTER TABLE tbl_idm_ir_plmc
							MODIFY irplmc_secamt  decimal(15,2) DEFAULT NULL;
						ELSE							
						 select 'column irplmc_secamt tbl_idm_ir_plmc table Created' as ' ';
				end if;
            end;
		-- If not exists, creat a new table

		select 'tbl_idm_ir_plmc Table Exist' as ''; 
		ELSE
		CREATE TABLE `tbl_idm_ir_plmc` (
			`irplmc_id` bigint NOT NULL AUTO_INCREMENT,
			`loan_acc` bigint DEFAULT NULL,
			`loan_sub` int DEFAULT NULL,
			`offc_cd` tinyint DEFAULT NULL,
			`irplmc_idt` date DEFAULT NULL,
			`irplmc_rdt` date DEFAULT NULL,
			`irplmc_item` int DEFAULT NULL,
			`irplmc_amt` int DEFAULT NULL,
			`irplmc_no` int DEFAULT NULL,
			`irplmc_ino` bigint DEFAULT NULL,
			`irplmc_aqrd_stat` bit(1) DEFAULT NULL,
			`irplmc_flg` int DEFAULT NULL,
			`irplmc_secamt` decimal(15,2) DEFAULT NULL,
			`irplmc_aqrd_indicator` int DEFAULT NULL,
			`irplmc_rel_stat` int DEFAULT NULL,
			`irplmc_aamt` int DEFAULT NULL,
			`irplmc_itemdets` varchar(200) DEFAULT NULL,
			`irplmc_supplier` varchar(100) DEFAULT NULL,
			`irplmc_total_release` int DEFAULT NULL,
			`is_active` bit(1) DEFAULT NULL,
			`is_deleted` bit(1) DEFAULT NULL,
			`created_by` varchar(50) DEFAULT NULL,
			`modified_by` varchar(50) DEFAULT NULL,
			`created_date` datetime DEFAULT NULL,
			`modified_date` datetime DEFAULT NULL,
			`unique_id` varchar(200) DEFAULT NULL,
			PRIMARY KEY (`irplmc_id`),
			KEY `FK_tbl_idm_ir_plmcs_tbl_app_loan_mast` (`loan_acc`),
			KEY `FK_tbl_idm_ir_plmc_offc_cdtab` (`offc_cd`),
			CONSTRAINT `FK_tbl_idm_ir_plmc_offc_cdtab` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`),
			CONSTRAINT `FK_tbl_idm_ir_plmc_tbl_app_loan_mast` FOREIGN KEY (`loan_acc`) REFERENCES `tbl_app_loan_mast` (`ln_no`)
		);

select 'tbl_idm_ir_plmc Table Created' as ''; 
END IF;
END;

-- ----------------------- Furniture Acquisition Module -----------------------------
						-- table name --

Begin
			IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_ir_furn')

					 -- If not exists, creat a new table
				Then
					select 'tbl_idm_ir_furn Table Already Exist' as ' ';
						 Begin
							IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
								WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_ir_furn' AND column_name='irf_aqrd_stat')
							THEN
							ALTER TABLE `tbl_idm_ir_furn`
									Modify Column  `irf_aqrd_stat` int DEFAULT NULL;
										select 'Column irf_aqrd_stat in tbl_idm_ir_furn Modified' as ' ';
                               
							ELSE
							
								 select 'Column irf_aqrd_stat in tbl_idm_ir_furn existed' as ' ';
							end if;

							
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_ir_furn' AND column_name='irf_ino')
					THEN
				        select 'Column irb_ino in tbl_idm_ir_furn Already Exist' as ' ';
                               
					ELSE
					ALTER TABLE `tbl_idm_ir_furn`
							Add `irf_ino`bigint DEFAULT NULL;
						 select 'Column irb_ino in tbl_idm_ir_furn Created' as ' ';
					end if;
			   
						END;

			else
				 CREATE TABLE `tbl_idm_ir_furn` (
				  `irf_id` bigint NOT NULL AUTO_INCREMENT,
				  `loan_acc` bigint DEFAULT NULL,
				  `loan_sub` int DEFAULT NULL,
				  `offc_cd` tinyint DEFAULT NULL,
				  `irf_idt` date DEFAULT NULL,
				  `irf_rdt` date DEFAULT NULL,
				  `irf_item` int DEFAULT NULL,
				  `irf_amt` int DEFAULT NULL,
				  `irf_ino` bigint DEFAULT NULL,
				  `irf_no` int DEFAULT NULL,
				  `irf_aqrd_stat` int DEFAULT NULL,
				  `irf_secamt` int DEFAULT NULL,
				  `irf_rel_stat` int DEFAULT NULL,
				  `irf_aamt` int DEFAULT NULL,
				  `irf_total_release` int DEFAULT NULL,
				  `irf_itemdets` varchar(200) DEFAULT NULL,
				  `irf_supplier` varchar(100) DEFAULT NULL,
				  `is_active` bit(1) DEFAULT NULL,
				  `is_deleted` bit(1) DEFAULT NULL,
				  `created_by` varchar(50) DEFAULT NULL,
				  `modified_by` varchar(50) DEFAULT NULL,
				  `created_date` datetime DEFAULT NULL,
				  `modified_date` datetime DEFAULT NULL,
				  `unique_id` varchar(200) DEFAULT NULL,
				  PRIMARY KEY (`irf_id`),
				  KEY `FK_tbl_idm_ir_furn_tbl_app_loan_mast` (`loan_acc`),
				  KEY `FK_tbl_idm_ir_furn_offc_cdtab` (`offc_cd`),
				  CONSTRAINT `FK_tbl_idm_ir_furn_offc_cdtab` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`),
				  CONSTRAINT `FK_tbl_idm_ir_furn_tbl_app_loan_mast` FOREIGN KEY (`loan_acc`) REFERENCES `tbl_app_loan_mast` (`ln_no`)
				) ;
                 select 'tbl_idm_ir_furn Table Created' as ' ';
			END IF;
		END;

END$$

DELIMITER ;

Call CSAA_Script()