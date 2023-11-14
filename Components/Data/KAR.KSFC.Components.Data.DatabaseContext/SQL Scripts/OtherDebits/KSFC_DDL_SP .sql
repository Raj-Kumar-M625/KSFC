USE `ksfc_oct`;
DROP procedure IF EXISTS `KSFC_OtherDebit_DDL_SP`;

DELIMITER $$
USE `ksfc_oct`$$
CREATE PROCEDURE `KSFC_OtherDebit_DDL_SP` ()
BEGIN
DECLARE DBName varchar(50) DEFAULT "ksfc_oct";
-- ---------------------- idm_othdebits_mast --------------------
Begin
IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
					WHERE table_schema = DBName
					AND table_name LIKE 'idm_othdebits_mast')
	-- If exists, retreive columns information from that table
		Then
		select 'idm_othdebits_mast table already exist ' as ' ';
		 Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='idm_othdebits_mast' AND column_name='dsb_othdebit_dis_seq')
						THEN
						select 'Column dsb_othdebit_dis_seq already Exists in idm_othdebits_mast table' as ' ';
						ELSE
                       ALTER TABLE `idm_othdebits_mast` 
								ADD COLUMN `dsb_othdebit_dis_seq` int DEFAULT NULL AFTER `dsb_othdebit_desc`;
							  select 'Column dsb_othdebit_dis_seq in idm_othdebits_mast table Created' as ' ';
					end if;
		 END;
		 Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='idm_othdebits_mast' AND column_name='is_active')
						THEN
						select 'Column is_active already Exists in idm_othdebits_mast table' as ' ';
						ELSE
                       ALTER TABLE `idm_othdebits_mast` 
								ADD COLUMN `is_active` bit(1) DEFAULT NULL AFTER `dsb_othdebit_desc`;
							  select 'Column is_active in idm_othdebits_mast table Created' as ' ';
					end if;
		 END;
		 Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='idm_othdebits_mast' AND column_name='is_deleted')
						THEN
						select 'Column is_deleted already Exists in idm_othdebits_mast table' as ' ';
						ELSE
                       ALTER TABLE `idm_othdebits_mast` 
								ADD COLUMN `is_deleted` bit(1) DEFAULT NULL AFTER `is_active`;
							  select 'Column is_deleted in idm_othdebits_mast table Created' as ' ';
					end if;
		 END;
		 Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='idm_othdebits_mast' AND column_name='created_by')
						THEN
						select 'Column created_by already Exists in idm_othdebits_mast table' as ' ';
						ELSE
                       ALTER TABLE `idm_othdebits_mast` 
								ADD COLUMN `created_by` varchar(50) DEFAULT NULL AFTER `is_deleted`;
							  select 'Column created_by in idm_othdebits_mast table Created' as ' ';
					end if;
		 END;
		 Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='idm_othdebits_mast' AND column_name='modified_by')
						THEN
						select 'Column modified_by already Exists in idm_othdebits_mast table' as ' ';
						ELSE
                       ALTER TABLE `idm_othdebits_mast` 
								ADD COLUMN `modified_by` varchar(50) DEFAULT NULL AFTER `created_by`;
							  select 'Column modified_by in idm_othdebits_mast table Created' as ' ';
					end if;
		 END;
		 Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='idm_othdebits_mast' AND column_name='created_date')
						THEN
						select 'Column created_date already Exists in idm_othdebits_mast table' as ' ';
						ELSE
                       ALTER TABLE `idm_othdebits_mast` 
								ADD COLUMN `created_date` datetime DEFAULT NULL AFTER `modified_by`;
							  select 'Column created_date in idm_othdebits_mast table Created' as ' ';
					end if;
		 END;
		 Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='idm_othdebits_mast' AND column_name='modified_date')
						THEN
						select 'Column modified_date already Exists in idm_othdebits_mast table' as ' ';
						ELSE
                       ALTER TABLE `idm_othdebits_mast` 
								ADD COLUMN `modified_date` datetime DEFAULT NULL AFTER `created_date`;
							  select 'Column modified_date in idm_othdebits_mast table Created' as ' ';
					end if;
		 END;

		else
CREATE TABLE `idm_othdebits_mast` (
			   dsb_othdebit_id int NOT NULL AUTO_INCREMENT,
			   dsb_othdebit_desc varchar(30) DEFAULT NULL,
			   dsb_othdebit_dis_seq int,
			  is_active bit(1) DEFAULT NULL,
			  is_deleted bit(1) DEFAULT NULL,  
			  created_by varchar(50) DEFAULT NULL,
			  modified_by varchar(50) DEFAULT NULL,
			  created_date datetime DEFAULT NULL,
              modified_date datetime DEFAULT NULL,
			  PRIMARY KEY (dsb_othdebit_id)
			);
select 'idm_othdebits_mast Table Created' as ' ';
END IF;
END;

						-- idm_othdebits_details --
Begin
IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
					WHERE table_schema = DBName
					AND table_name LIKE 'idm_othdebits_details')
	-- If exists, retreive columns information from that table
		Then
		select 'idm_othdebits_details table already exist ' as ' ';
		
		 Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='idm_othdebits_details' AND column_name='modified_date')
						THEN
						select 'Column modified_date already Exists in idm_othdebits_details table' as ' ';
						ELSE
                       ALTER TABLE `idm_othdebits_details` 
								ADD COLUMN `modified_date` datetime DEFAULT NULL AFTER `created_date`;
							  select 'Column modified_date in idm_othdebits_details table Created' as ' ';
					end if;
		 END;
		 Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='idm_othdebits_details' AND column_name='unique_id')
						THEN
						select 'Column unique_id already Exists in idm_othdebits_details table' as ' ';
						ELSE
                       ALTER TABLE `idm_othdebits_details` 
								ADD COLUMN `unique_id` varchar(200) DEFAULT NULL AFTER `modified_date`;
							  select 'Column unique_id in idm_othdebits_mast table Created' as ' ';
					end if;
		 END;
		 		  Begin
					IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='idm_othdebits_details' AND column_name='is_submitted')
						THEN
						select 'Column is_submitted already Exists in idm_othdebits_details table' as ' ';
						ELSE
                       ALTER TABLE `idm_othdebits_details` 
								ADD COLUMN `is_submitted` bit(1) DEFAULT NULL AFTER `unique_id`;
							  select 'Column is_submitted in idm_othdebits_details table Created' as ' ';
					end if;
		 END;
		else
CREATE TABLE `idm_othdebits_details` (
  `othdebit_det_id` bigint NOT NULL AUTO_INCREMENT,
  `loan_acc` bigint DEFAULT NULL,
  `loan_sub` int DEFAULT NULL,
  `offc_cd` tinyint DEFAULT NULL,
  `dsb_othdebit_id` int DEFAULT NULL,
  `othdebit_amt` decimal(15,2) DEFAULT NULL,
  `othdebit_gst` decimal(15,2) DEFAULT NULL,
  `othdebit_taxes` decimal(15,2) DEFAULT NULL,
  `othdebit_duedate` date DEFAULT NULL,
  `othdebit_total` decimal(15,2) DEFAULT NULL,
  `othdebit_remarks` varchar(100) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(50) DEFAULT NULL,
  `modified_by` varchar(50) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
    `modified_date` datetime DEFAULT NULL,
      `unique_id` varchar(200) DEFAULT NULL,
   is_submitted bit(1) DEFAULT NULL,
  PRIMARY KEY (`othdebit_det_id`),
  KEY `loan_acc_idx` (`loan_acc`),
  KEY `dsb_otherdebit_id_idx` (`dsb_othdebit_id`)
 );
select 'idm_othdebits_details Table Created' as ' ';
END IF;
END;
END$$

DELIMITER ;
call KSFC_OtherDebit_DDL_SP()
