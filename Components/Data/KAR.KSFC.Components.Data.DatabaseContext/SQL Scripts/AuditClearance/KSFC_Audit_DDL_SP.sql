USE `ksfc_oct`;
DROP procedure IF EXISTS `KSFC_Audit_DDL_SP`;
 
DELIMITER $$
USE `ksfc_oct`$$
CREATE PROCEDURE `KSFC_Audit_DDL_SP` ()
BEGIN
-- -----------------------Audit Clearance Module ----------------------------------
						-- tbl_idm_audit_det --
Begin
DECLARE DBName varchar(50) DEFAULT "ksfc_oct";
IF  EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_idm_audit_det')

		-- If not exists, creat a new table
Then
Begin
			IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
						WHERE table_schema = DBName AND TABLE_NAME='tbl_idm_audit_det' AND column_name='loan_acc')
						THEN
                        ALTER TABLE tbl_idm_audit_det
							MODIFY COLUMN  loan_acc  bigint NOT NULL;
							  select 'Column loan_acc in tbl_idm_audit_det table Modified' as ' ';
						ELSE
							
						select 'Column loan_acc in tbl_idm_audit_det table Created' as ' ';
				end if;
            end;
 select 'tbl_idm_audit_det Table Already Exist' as ' ';
else
CREATE TABLE `tbl_idm_audit_det` (
			  `idm_audit_id` int NOT NULL AUTO_INCREMENT,
			  `loan_acc` bigint NOT NULL,
			  `loan_sub` int DEFAULT NULL,
			  `offc_cd` tinyint DEFAULT NULL,
			  `audit_observation` varchar(150) DEFAULT NULL,
			  `audit_compliance` varchar(150) DEFAULT NULL,
			  `audit_upload` varchar(100) DEFAULT NULL,
			  `audit_emp_id` int DEFAULT NULL,
               `unique_id` varchar(100) DEFAULT NULL,
			  `is_active` bit(1) DEFAULT NULL,
			  `is_deleted` bit(1) DEFAULT NULL,
			  `created_by` varchar(50) DEFAULT NULL,
			  `modified_by` varchar(50) DEFAULT NULL,
			  `created_date` datetime DEFAULT NULL,
			  `modified_date` datetime DEFAULT NULL,
			  PRIMARY KEY (`idm_audit_id`),
			  KEY `loan_acc` (`loan_acc`),
			  KEY `offc_cd` (`offc_cd`),
			  CONSTRAINT `tbl_idm_audit_det_ibfk_1` FOREIGN KEY (`loan_acc`) REFERENCES `tbl_app_loan_mast` (`ln_no`),
			  CONSTRAINT `tbl_idm_audit_det_ibfk_2` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`)
			);
			select 'tbl_idm_audit_det Table Created' as ' ';
END IF;
END;
END$$

DELIMITER ;
call KSFC_Audit_DDL_SP()
