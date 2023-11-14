
--Condition Module --- Added by Gagana

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
)

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
)

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
)


CREATE TABLE `tbl_idm_cond_det` (
  `cond_det_id` int NOT NULL AUTO_INCREMENT,
  `loan_acc` bigint DEFAULT NULL,
  `loan_sub` int DEFAULT NULL,
  `offc_cd` tinyint DEFAULT NULL,
  `cond_type` tinyint DEFAULT NULL,
  `cond_cd` smallint DEFAULT NULL,
  `cond_stg` tinyint DEFAULT NULL,
  `cond_details` varchar(200) DEFAULT NULL,
  `cond_remarks` varchar(200) DEFAULT NULL,
  `wh_relaxation` bit(1) DEFAULT NULL,
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
) 

 ALTER TABLE `tbl_idm_cond_det`
 Add `unique_id` varchar(150) DEFAULT NULL

ALTER TABLE `ksfc_csg`.`tbl_idm_cond_det` 
ADD CONSTRAINT `tbl_idm_cond_det_ibfk_3`
  FOREIGN KEY (`cond_type`)
  REFERENCES `ksfc_csg`.`tbl_cond_type_cdtab` (`cond_type_cd`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION;

ALTER TABLE `ksfc_csg`.`tbl_idm_cond_det` 
ADD CONSTRAINT `tbl_idm_cond_det_ibfk_4`
  FOREIGN KEY (`cond_stg`)
  REFERENCES `ksfc_csg`.`tbl_cond_stg_cdtab` (`cond_stg_cd`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION;

-- Guarantor
CREATE TABLE tbl_idm_guar_deed_upload (
  idm_guardeed_upload_id bigint NOT NULL,
  idm_guar_deed_id bigint NOT NULL,
  guar_deed_upload varchar(300) DEFAULT NULL,
  PRIMARY KEY (idm_guardeed_upload_id,idm_guar_deed_id)
);

CREATE TABLE ksfc_csg.tbl_charge_type_cdtab (
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
)

    ALTER TABLE ksfc_csg.tbl_idm_dsb_charge 
DROP FOREIGN KEY  fk_tbl_idm_dsb_charge_tbl_charge_type_cdtab;
ALTER TABLE ksfc_csg.tbl_idm_dsb_charge 
ADD CONSTRAINT fk_tbl_idm_dsb_charge_tbl_charge_type_cdtab
  FOREIGN KEY (charge_type_cd)
  REFERENCES ksfc_csg.tbl_charge_type_cdtab (chrg_type_cd);


  ALTER TABLE ksfc_csg.tbl_idm_dsb_charge
ADD bank_ifsc_id_cd  int DEFAULT NULL;

ALTER TABLE ksfc_csg.tbl_idm_dsb_charge 
ADD CONSTRAINT fk_tbl_idm_dsb_charge_tbl_ifsc_master_id
  FOREIGN KEY (bank_ifsc_id_cd)
  REFERENCES ksfc_csg.tbl_ifsc_master (ifsc_rowid);

-- Guarantor
ALTER TABLE `ksfc_csg`.`tbl_idm_guar_deed_det` 
ADD COLUMN `app_guarasset_id` INT NULL AFTER `promoter_code`;
-- Guarantor
ALTER TABLE `ksfc_csg`.`tbl_idm_guar_deed_det` 
ADD INDEX `fk_tbl_idm_guar_deed_det_tbl_app_guar_asset_det_idx` (`app_guarasset_id` ASC) VISIBLE;
-- Guarantor
ALTER TABLE `ksfc_csg`.`tbl_idm_guar_deed_det` 
ADD CONSTRAINT `fk_tbl_idm_guar_deed_det_tbl_app_guar_asset_det`
  FOREIGN KEY (`app_guarasset_id`)
  REFERENCES `ksfc_csg`.`tbl_app_guar_asset_det` (`app_guarasset_id`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION;


  -- Documents 
  CREATE TABLE `tbl_Ld_document` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `SubModuleId` int NOT NULL,
  `SubModuleType` varchar(50) DEFAULT NULL,
  `FileName` varchar(150) DEFAULT NULL,
  `FilePath` varchar(200) DEFAULT NULL,
  `FileType` varchar(45) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(45) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `modified_by` varchar(45) DEFAULT NULL,
  `unique_id` varchar(150) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `id_UNIQUE` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci
  -- Updated BY Akhila -16-08-2022
  ALTER TABLE tbl_app_guarnator MODIFY column guar_mobile_no varchar(10);


  -- Disbursement Module Form 8 and 13 tab --

  CREATE TABLE `tbl_idm_dsb_fm813` (
  `df813_id` bigint NOT NULL,
  `df813_offc` tinyint DEFAULT NULL,
  `df813_unit` bigint DEFAULT NULL,
  `df813_sno` int DEFAULT NULL,
  `df813_dt` date DEFAULT NULL,
  `df813_ref` varchar(100) DEFAULT NULL,
  `df813_cc` varchar(100) DEFAULT NULL,
  `df813_t1` int DEFAULT NULL,
  `df813_a1` int DEFAULT NULL,
  `df813_upload` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`df813_id`),
  KEY `df813_offc` (`df813_offc`),
  KEY `df813_unit` (`df813_unit`),
  CONSTRAINT `tbl_idm_dsb_fm813_ibfk_1` FOREIGN KEY (`df813_offc`) REFERENCES `offc_cdtab` (`OFFC_CD`),
  CONSTRAINT `tbl_idm_dsb_fm813_ibfk_2` FOREIGN KEY (`df813_unit`) REFERENCES `tbl_app_loan_mast` (`ln_mast_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci

-- Added Missing Column --


ALTER TABLE `ksfc_csg`.`tbl_idm_dsb_fm813` 
ADD COLUMN `df813_loan_acc` BIGINT NULL DEFAULT NULL AFTER `df813_upload`;
ADD COLUMN `is_active` BIT NULL DEFAULT NULL AFTER `df813_loan_acc`,
ADD COLUMN `is_deleted` BIT NULL DEFAULT NULL AFTER `is_active`,
ADD COLUMN `created_by` VARCHAR(50) NULL DEFAULT NULL AFTER `is_deleted`,
ADD COLUMN `modified_by` VARCHAR(50) NULL DEFAULT NULL AFTER `created_by`,
ADD COLUMN `created_date` DATETIME NULL DEFAULT NULL AFTER `modified_by`,
ADD COLUMN `modified_date` DATETIME NULL DEFAULT NULL AFTER `created_date`;


  
--Audit Clearance Module --- Added by Gagana

CREATE TABLE `tbl_idm_audit_det` (
  `idm_audit_id` int NOT NULL AUTO_INCREMENT,
  `loan_acc` bigint NOT NULL,
  `loan_sub` int DEFAULT NULL,
  `offc_cd` tinyint DEFAULT NULL,
  `audit_observation` varchar(150) DEFAULT NULL,
  `audit_compliance` varchar(150) DEFAULT NULL,
  `audit_upload` varchar(100) DEFAULT NULL,
  `audit_emp_id` int DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `unique_id` varchar(150) DEFAULT NULL,
  `created_by` varchar(50) DEFAULT NULL,
  `modified_by` varchar(50) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  PRIMARY KEY (`idm_audit_id`),
  KEY `loan_acc` (`loan_acc`),
  KEY `offc_cd` (`offc_cd`),
  CONSTRAINT `tbl_idm_audit_det_ibfk_1` FOREIGN KEY (`loan_acc`) REFERENCES `tbl_app_loan_mast` (`ln_no`),
  CONSTRAINT `tbl_idm_audit_det_ibfk_2` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci

  -- Audit Clearance Module Added by Gagana

  CREATE TABLE `tbl_idm_audit_det` (
  `idm_audit_id` int NOT NULL AUTO_INCREMENT,
  `loan_acc` bigint NOT NULL,
  `loan_sub` int DEFAULT NULL,
  `offc_cd` tinyint DEFAULT NULL,
  `audit_observation` varchar(150) DEFAULT NULL,
  `audit_compliance` varchar(150) DEFAULT NULL,
  `audit_upload` varchar(100) DEFAULT NULL,
  `audit_emp_id` int DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `unique_id` varchar(150) DEFAULT NULL,
  `created_by` varchar(50) DEFAULT NULL,
  `modified_by` varchar(50) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  PRIMARY KEY (`idm_audit_id`),
  KEY `loan_acc` (`loan_acc`),
  KEY `offc_cd` (`offc_cd`),
  CONSTRAINT `tbl_idm_audit_det_ibfk_1` FOREIGN KEY (`loan_acc`) REFERENCES `tbl_app_loan_mast` (`ln_no`),
  CONSTRAINT `tbl_idm_audit_det_ibfk_2` FOREIGN KEY (`offc_cd`) REFERENCES `offc_cdtab` (`OFFC_CD`)
)

CREATE TABLE `tbl_idm_Audit_upload` (
  `idm_AuditUpload_id` bigint NOT NULL,
  `idm_audit_id` int DEFAULT NULL,
  `audit_uploads` varchar(300) DEFAULT NULL,
  PRIMARY KEY (`idm_AuditUpload_id`),
  KEY `idm_Audit_code` (`idm_audit_id`),
  CONSTRAINT `tbl_idm_Audit_upload_ibfk_1` FOREIGN KEY (`idm_audit_id`) REFERENCES `tbl_idm_audit_det` (`idm_audit_id`)
)

-- Verification of Disbursement Condition Module
ALTER TABLE `tbl_idm_cond_det`
 Add `unique_id` varchar(150) DEFAULT NULL
-- Sidbi Approval

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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci



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
)

  -- Sibi Approval Module Added by Sandeep M
alter table tbl_idm_sidbi_approval  
add  `modified_by` varchar(50) DEFAULT NULL,
add `modified_date` datetime DEFAULT NULL