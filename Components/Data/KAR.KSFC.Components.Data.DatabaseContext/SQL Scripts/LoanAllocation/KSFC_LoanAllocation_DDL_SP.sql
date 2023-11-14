USE `ksfc_oct`;
DROP procedure IF EXISTS `KSFC_LoanAllocation_DDL_SP`;

DELIMITER $$
USE `ksfc_oct`$$
CREATE PROCEDURE `KSFC_LoanAllocation_DDL_SP` ()
BEGIN
DECLARE DBName varchar(50) DEFAULT "ksfc_oct";
-- ---------------------- tbl_allc_cdtab --------------------
Begin
IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
					WHERE table_schema = DBName
					AND table_name LIKE 'tbl_allc_cdtab')
	-- If exists, retreive columns information from that table
		Then
		select 'tbl_allc_cdtab table already exist ' as ' ';
		else
CREATE TABLE `tbl_allc_cdtab` (
			  allc_id int NOT NULL AUTO_INCREMENT,  
			  allc_cd int DEFAULT NULL,
			  allc_dets varchar(100) DEFAULT NULL,
			  allc_flg tinyint DEFAULT NULL,
			  is_active bit(1) DEFAULT NULL,
			  is_deleted bit(1) DEFAULT NULL,  
			  created_by varchar(50) DEFAULT NULL,
			  modified_by varchar(50) DEFAULT NULL,
			  created_date datetime DEFAULT NULL,
			  modified_date datetime DEFAULT NULL,
              unique_id varchar(200) DEFAULT NULL,
			  PRIMARY KEY (allc_id)
			);
select 'tbl_allc_cdtab Table Created' as ' ';
END IF;
END;
-- -----------------------Loan Allocation Module Added by Gagana ----------------------------------
						-- tbl_idm_dhcg_allc --
Begin
IF  EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_dhcg_allc')

		-- If not exists, creat a new table
Then
Begin

			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dhcg_allc' AND column_name='unique_id')
						THEN
						select 'Column unique_id already Exists in tbl_idm_dhcg_allc table' as ' ';
						ELSE
                       ALTER TABLE `tbl_idm_dhcg_allc` 
								ADD COLUMN `unique_id` VARCHAR(50) NULL AFTER `modified_date`;
							  select 'Column unique_id in tbl_idm_dhcg_allc Modified' as ' ';
				end if;

				IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dhcg_allc' AND column_name='dcalc_amt_revised')
						THEN
                       ALTER TABLE tbl_idm_dhcg_allc
					  MODIFY COLUMN  dcalc_amt decimal(10,2) DEFAULT NULL;
							  select 'Column dcalc_amt_revised in tbl_idm_dhcg_allc table Modified' as ' ';
						ELSE
						select 'Column dcalc_amt_revised in tbl_idm_dhcg_allc Created' as ' ';
				end if;
				IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_dhcg_allc' AND column_name='dcalc_amt')
						THEN
                       ALTER TABLE `tbl_idm_dhcg_allc` 
					  MODIFY COLUMN  dcalc_amt_revised  decimal(10,2) DEFAULT NULL;
							  select 'Column dcalc_amt in tbl_idm_dhcg_allc table Modified' as ' ';
						ELSE
						select 'Column dcalc_amt in tbl_idm_dhcg_allc Created' as ' ';
				end if;
				
            end;
 select 'tbl_idm_dhcg_allc Table Already Exist' as ' ';
 elseif  EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_dhcg_allc')
Then
Begin
 select 'tbl_idm_dhcg_allc Table Already Exist' as ' ';
 end;
else
CREATE TABLE `tbl_idm_dhcg_allc` (
  `dcalc_id` bigint NOT NULL AUTO_INCREMENT,
  `loan_acc` bigint DEFAULT NULL,
  `loan_sub` int DEFAULT NULL,
  `offc_cd` tinyint DEFAULT NULL,
  `dcalc_cd` int DEFAULT NULL,
  `dcalc_amt` decimal(10,2) DEFAULT NULL,
  `dcalc_rqdt` date DEFAULT NULL,
  `dcalc_apdt` date DEFAULT NULL,
  `dcalc_apau` int DEFAULT NULL,
  `dcalc_comdt` date DEFAULT NULL,
  `dcalc_amt_revised` decimal(10,2) DEFAULT NULL,
  `dcalc_details` varchar(100) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) DEFAULT NULL,
  `modified_by` varchar(50) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
   `unique_id` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`dcalc_id`),
  KEY `FK_tbl_idm_dhcg_allc_offc_cdtab` (`offc_cd`),
  CONSTRAINT `FK_tbl_idm_dhcg_allc_offc_cdtab` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`)
);
select 'tbl_idm_dhcg_allc Table Created' as ' ';
END IF;
END;
END$$

DELIMITER ;
call KSFC_LoanAllocation_DDL_SP()
