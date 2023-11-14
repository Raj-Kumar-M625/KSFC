	USE `ksfc_oct`;
DROP procedure IF EXISTS `KSFC_oct_NewDump_Collate_DDL_SP`;

DELIMITER $$
USE `ksfc_oct`$$
CREATE PROCEDURE `KSFC_oct_NewDump_Collate_DDL_SP` ()
BEGIN
-- -----------------------Removing Foreign Key Constraints ----------------------------------
							
Begin
DECLARE DBName varchar(50) DEFAULT "ksfc_oct";

   --   update tbl_prod_cdtab set created_by = null;

	  --  ALTER TABLE ksfc_oct.tbl_prod_cdtab MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	  -- ALTER TABLE ksfc_oct.tbl_prod_cdtab MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	  -- ALTER TABLE ksfc_oct.tbl_prod_cdtab MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;

	   ALTER TABLE ksfc_oct.tbl_pjmf_cdtab MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_pjmf_cdtab MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_pjmf_cdtab MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;

	   ALTER TABLE ksfc_oct.tbl_pjmfcat_cdtab MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_pjmfcat_cdtab MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_pjmfcat_cdtab MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;

      ALTER TABLE ksfc_oct.tbl_sec_cdtab MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_sec_cdtab MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_sec_cdtab MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;

      ALTER TABLE ksfc_oct.tbl_pjsec_cdtab MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_pjsec_cdtab MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_pjsec_cdtab MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;

      ALTER TABLE ksfc_oct.tbl_pdesig_cdtab MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_pdesig_cdtab MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_pdesig_cdtab MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;

      ALTER TABLE ksfc_oct.tbl_size_cdtab MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_size_cdtab MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_size_cdtab MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;

      ALTER TABLE ksfc_oct.vil_cdtab MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.vil_cdtab MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.vil_cdtab MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;

      ALTER TABLE ksfc_oct.tlq_cdtab MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tlq_cdtab MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tlq_cdtab MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;

      ALTER TABLE ksfc_oct.tbl_pjcostgroup_cdtab MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_pjcostgroup_cdtab MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_pjcostgroup_cdtab MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;

      ALTER TABLE ksfc_oct.tbl_pjcsgroup_cdtab MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_pjcsgroup_cdtab MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_pjcsgroup_cdtab MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
  


-- new dump above --

	ALTER TABLE ksfc_oct.cnst_cdtab MODIFY CNST_DETS varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.cnst_cdtab MODIFY CNST_PANCHAR varchar(1) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.cnst_cdtab MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.cnst_cdtab MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.cnst_cdtab MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.constmla_cdtab MODIFY CONSTMLA_KANNADA varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.constmla_cdtab MODIFY CONSTMLA_NAME varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.constmla_cdtab MODIFY CONSTMLA_STATE_CD varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.constmla_cdtab MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.constmla_cdtab MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.constmla_cdtab MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.constmp_cdtab MODIFY CONSTMP_KANNADA varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.constmp_cdtab MODIFY CONSTMP_NAME varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.constmp_cdtab MODIFY CONSTMP_STATE_CD varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.constmp_cdtab MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.constmp_cdtab MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.constmp_cdtab MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.dist_cdtab MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.dist_cdtab MODIFY DIST_FB_FLG varchar(1) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.dist_cdtab MODIFY DIST_NAM varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.dist_cdtab MODIFY DIST_NAME_KANNADA varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.dist_cdtab MODIFY DIST_ZONE varchar(2) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.dist_cdtab MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.dist_cdtab MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.dsc_detailstab MODIFY created_by varchar(2) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.dsc_detailstab MODIFY DSC_NAME varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.dsc_detailstab MODIFY DSC_PUBLIC_KEY varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.dsc_detailstab MODIFY DSC_SERNO varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.dsc_detailstab MODIFY EMP_ID varchar(8) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.dsc_detailstab MODIFY EMP_PASSWORD varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.dsc_detailstab MODIFY modified_by varchar(2) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.dsc_detailstab MODIFY unique_id varchar(2) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.empsession_tab MODIFY ACCESSTOKEN varchar(2000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.empsession_tab MODIFY created_by varchar(2) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.empsession_tab MODIFY EMP_ID varchar(8) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.empsession_tab MODIFY IPADRESS varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.empsession_tab MODIFY modified_by varchar(2) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.empsession_tab MODIFY REFRESHTOKEN varchar(2000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.empsession_tab MODIFY unique_id varchar(2) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.hob_cdtab MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	
	ALTER TABLE ksfc_oct.hob_cdtab MODIFY HOB_NAME_KANNADA varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	ALTER TABLE ksfc_oct.hob_cdtab MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	ALTER TABLE ksfc_oct.hob_cdtab MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.idm_audit_det MODIFY audit_compliance varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.idm_audit_det MODIFY audit_observation varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.idm_audit_det MODIFY audit_upload varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.idm_audit_det MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	
	
	ALTER TABLE ksfc_oct.idm_audit_det MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	ALTER TABLE ksfc_oct.ip_cdtab MODIFY created_by varchar(145) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.ip_cdtab MODIFY IP varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	ALTER TABLE ksfc_oct.ip_cdtab MODIFY modified_by varchar(145) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.ip_cdtab MODIFY unique_id varchar(45) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.ksfcuser_designation_details MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.ksfcuser_designation_details MODIFY EMPLOYEEID varchar(8) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	ALTER TABLE ksfc_oct.ksfcuser_designation_details MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	
	ALTER TABLE ksfc_oct.ksfcuser_designation_details MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.kzn_cdtab MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	ALTER TABLE ksfc_oct.kzn_cdtab MODIFY KZN_ADR1 varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.kzn_cdtab MODIFY KZN_ADR2 varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.kzn_cdtab MODIFY KZN_ADR3 varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.kzn_cdtab MODIFY KZN_FAX varchar(15) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.kzn_cdtab MODIFY KZN_NAM varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	ALTER TABLE ksfc_oct.kzn_cdtab MODIFY KZN_TLX varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.kzn_cdtab MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.kzn_cdtab MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.nuserhistory_tab MODIFY created_by varchar(2) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	ALTER TABLE ksfc_oct.nuserhistory_tab MODIFY modified_by varchar(2) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.nuserhistory_tab MODIFY Process char(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.nuserhistory_tab MODIFY unique_id varchar(2) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	ALTER TABLE ksfc_oct.nuserhistory_tab MODIFY VER_DETAILS varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.nuserhistory_tab MODIFY VER_STATUS char(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.nuserhistory_tab MODIFY VER_TYPE char(1) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.offc_cdtab MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	ALTER TABLE ksfc_oct.offc_cdtab MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.offc_cdtab MODIFY OFFC_ADR1 varchar(35) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.offc_cdtab MODIFY OFFC_ADR2 varchar(35) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.offc_cdtab MODIFY OFFC_ADR3 varchar(35) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.offc_cdtab MODIFY OFFC_BM_MAIL_ID varchar(25) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.offc_cdtab MODIFY OFFC_BSR_CD varchar(7) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	ALTER TABLE ksfc_oct.offc_cdtab MODIFY OFFC_IFS_CD varchar(11) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.offc_cdtab MODIFY OFFC_INOPBNKAC_NO varchar(13) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.offc_cdtab MODIFY OFFC_MAIL_ID varchar(15) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.offc_cdtab MODIFY OFFC_NAM varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.offc_cdtab MODIFY OFFC_NAM_KANNADA varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.offc_cdtab MODIFY OFFC_ST_NO varchar(15) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.offc_cdtab MODIFY OFFC_STD_CD varchar(5) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.offc_cdtab MODIFY OFFC_TAX_NO varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	
	ALTER TABLE ksfc_oct.offc_cdtab MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.otp_tab MODIFY created_by varchar(2) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	ALTER TABLE ksfc_oct.otp_tab MODIFY modified_by varchar(2) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.otp_tab MODIFY OTP varchar(6) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	ALTER TABLE ksfc_oct.otp_tab MODIFY PROCESS char(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.otp_tab MODIFY unique_id varchar(2) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.otp_tab MODIFY USER_ID varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.prom_unit_maptab MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	ALTER TABLE ksfc_oct.prom_unit_maptab MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	ALTER TABLE ksfc_oct.prom_unit_maptab MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.promoter_maptab MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	ALTER TABLE ksfc_oct.promoter_maptab MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	
	ALTER TABLE ksfc_oct.promoter_maptab MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.promsession_tab MODIFY ACCESSTOKEN varchar(2000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	ALTER TABLE ksfc_oct.promsession_tab MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.promsession_tab MODIFY IPADRESS varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	ALTER TABLE ksfc_oct.promsession_tab MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.promsession_tab MODIFY PROM_PAN varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.promsession_tab MODIFY REFRESHTOKEN varchar(2000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	ALTER TABLE ksfc_oct.promsession_tab MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.regduser_tab MODIFY created_by varchar(2) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	ALTER TABLE ksfc_oct.regduser_tab MODIFY modified_by varchar(2) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.regduser_tab MODIFY unique_id varchar(2) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.regduser_tab MODIFY USER_PAN varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	ALTER TABLE ksfc_oct.tbl_activity_mast MODIFY activity_desc varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_activity_mast MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	ALTER TABLE ksfc_oct.tbl_activity_mast MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	ALTER TABLE ksfc_oct.tbl_address_cdtab MODIFY addtype_dets varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_address_cdtab MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	ALTER TABLE ksfc_oct.tbl_address_cdtab MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_address_cdtab MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_app_awork_cap MODIFY awc_adr1 varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_awork_cap MODIFY awc_adr2 varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_awork_cap MODIFY awc_bank varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_awork_cap MODIFY awc_bank_district varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_awork_cap MODIFY awc_bank_taluka varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	ALTER TABLE ksfc_oct.tbl_app_awork_cap MODIFY awc_fax varchar(15) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_app_awork_cap MODIFY awc_ifsc varchar(11) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_awork_cap MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_awork_cap MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_app_furn MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_furn MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_furn MODIFY pjf_dets varchar(600) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	ALTER TABLE ksfc_oct.tbl_app_furn MODIFY pjf_inv_no varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	ALTER TABLE ksfc_oct.tbl_app_furn MODIFY pjf_notes   text(16777215) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_furn MODIFY pjf_sup varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_furn MODIFY pjf_supadr varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_furn MODIFY pjf_upload varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_furn MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_guar_asset_det MODIFY app_asset_addr varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_app_guar_asset_det MODIFY app_asset_desc varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_guar_asset_det MODIFY app_asset_dim varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_guar_asset_det MODIFY app_asset_siteno varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_guar_asset_det MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_guar_asset_det MODIFY land_type varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_guar_asset_det MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_guar_bank_details MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_guar_bank_details MODIFY guar_bank_ac_name varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_guar_bank_details MODIFY guar_bank_address varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_guar_bank_details MODIFY guar_bank_branch varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_guar_bank_details MODIFY guar_bank_district varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_guar_bank_details MODIFY guar_bank_name varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_app_guar_bank_details MODIFY guar_bank_state varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_guar_bank_details MODIFY guar_bank_taluka varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_app_guar_bank_details MODIFY guar_ifsc_id varchar(11) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_guar_bank_details MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_guar_liab_det MODIFY app_liab_desc varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_app_guar_liab_det MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_guar_liab_det MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_guar_nw_det MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_guar_nw_det MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_guarnator MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_guarnator MODIFY guar_email varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_guarnator MODIFY guar_gender text(65535) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_app_guarnator MODIFY guar_name varchar(35) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_guarnator MODIFY guar_pan varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_guarnator MODIFY guar_passport varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	ALTER TABLE ksfc_oct.tbl_app_guarnator MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_app_guarnator MODIFY name_father_spouse varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	ALTER TABLE ksfc_oct.tbl_app_guarnator MODIFY prom_photo varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_guarnator MODIFY ut_name varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_app_kycrisk_status MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_kycrisk_status MODIFY kycrisk_status varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_kycrisk_status MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_loan_mast MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_app_loan_mast MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_app_loan_mast MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	

	ALTER TABLE ksfc_oct.tbl_app_note_market MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_app_note_market MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_app_note_market MODIFY mr_dets   text(16777215) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_note_market MODIFY mr_upload varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_pjmf_overall MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_pjmf_overall MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_bldg MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_bldg MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_app_proj_bldg MODIFY pjbdg_dets varchar(600) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_app_proj_bldg MODIFY pjbdg_note   text(16777215) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_bldg MODIFY pjbdg_roof varchar(40) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_bldg MODIFY pjbdg_subv_note   text(16777215) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_bldg MODIFY pjbdg_upload varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_app_proj_bldg MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_proj_cost MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_cost MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_cost MODIFY pjc_notes   text(16777215) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_cost MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_proj_fgoods MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_fgoods MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_fgoods MODIFY pjfg_nam varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_imp_mc MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_imp_mc MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_imp_mc MODIFY pjimc_boeno varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_imp_mc MODIFY pjimc_crncy varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_imp_mc MODIFY pjimc_dets varchar(600) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_app_proj_imp_mc MODIFY pjimc_entry varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_proj_imp_mc MODIFY pjimc_entry_i varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_imp_mc MODIFY pjimc_notes   text(16777215) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_imp_mc MODIFY pjimc_rcrncy varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_imp_mc MODIFY pjimc_subv_notes   text(16777215) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_app_proj_imp_mc MODIFY pjimc_sup varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_app_proj_imp_mc MODIFY pjimc_supadr varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_imp_mc MODIFY pjimc_upload varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_app_proj_imp_mc MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_proj_implement MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_implement MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_implement MODIFY pjimple_notes   text(16777215) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_implement MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_proj_land MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_land MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_app_proj_land MODIFY pjlnd_address varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_app_proj_land MODIFY pjlnd_areain varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_proj_land MODIFY pjlnd_conversion_det varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_app_proj_land MODIFY pjlnd_dim varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_app_proj_land MODIFY pjlnd_land_details varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_proj_land MODIFY pjlnd_landmark varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_proj_land MODIFY pjlnd_location varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_proj_land MODIFY pjlnd_notes   text(16777215) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_land MODIFY pjlnd_site_no varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_land MODIFY pjlnd_upload varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_proj_land MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_proj_mf MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_mf MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_mf MODIFY pjmf_notes   text(16777215) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_mf MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_proj_plmc MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_plmc MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_plmc MODIFY pjplmc_dets varchar(600) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_plmc MODIFY pjplmc_inv_no varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_plmc MODIFY pjplmc_notes   text(16777215) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_plmc MODIFY pjplmc_subv_notes   text(16777215) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_proj_plmc MODIFY pjplmc_sup varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_proj_plmc MODIFY pjplmc_supadr varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_plmc MODIFY pjplmc_upload varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_app_proj_plmc MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_proj_power MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_power MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_power MODIFY pjpwr_dg_unts varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_app_proj_power MODIFY pjpwr_normal_unts varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_proj_power MODIFY pjpwr_normal_unts_ext varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_app_proj_power MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_proj_rawmat MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_rawmat MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_rawmat MODIFY pjraw_grade varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_app_proj_rawmat MODIFY pjraw_name varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_rawmat MODIFY pjraw_suploc varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_proj_rawmat MODIFY pjraw_supnam varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_proj_rawmat MODIFY pjraw_ty varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_sale MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_sale MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_sale MODIFY sales_item1 varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_proj_sale MODIFY sales_item2 varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_sale MODIFY sales_prd varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_sale MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_app_proj_secu MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_secu MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_secu MODIFY pjsec_dets varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_secu MODIFY pjsec_nam varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_secu MODIFY pjsec_upload varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_proj_wc_corp MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_proj_wc_corp MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	ALTER TABLE ksfc_oct.tbl_app_projexpense_det MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_projexpense_det MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_app_projexpense_det MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	ALTER TABLE ksfc_oct.tbl_app_prom_address MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_prom_address MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	ALTER TABLE ksfc_oct.tbl_app_prom_address MODIFY prom_address varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_prom_asset_det MODIFY app_asset_addr varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_app_prom_asset_det MODIFY app_asset_desc varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_prom_asset_det MODIFY app_asset_dim varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_prom_asset_det MODIFY app_asset_siteno varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_prom_asset_det MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_prom_asset_det MODIFY land_type varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_prom_asset_det MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_prom_liab_det MODIFY app_liab_desc varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	ALTER TABLE ksfc_oct.tbl_app_prom_liab_det MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_prom_liab_det MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_prom_nw_det MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_prom_nw_det MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_promoter MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_promoter MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_app_promoter MODIFY name_father_spouse varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_promoter MODIFY prom_addlqual varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_promoter MODIFY prom_email varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_app_promoter MODIFY prom_exp_det varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_app_promoter MODIFY prom_gender text(65535) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_promoter MODIFY prom_name varchar(35) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_promoter MODIFY prom_pan varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_promoter MODIFY prom_passport varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_promoter MODIFY prom_photo varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_promoter MODIFY prom_phy_handicap varchar(2) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_promoter MODIFY ut_name varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_app_promoter_bank_details MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_promoter_bank_details MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	
	
	
	ALTER TABLE ksfc_oct.tbl_app_promoter_bank_details MODIFY prm_bank_ac_name varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_promoter_bank_details MODIFY prm_bank_address varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_promoter_bank_details MODIFY prm_bank_branch varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_promoter_bank_details MODIFY prm_bank_district varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_promoter_bank_details MODIFY prm_bank_loc varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_promoter_bank_details MODIFY prm_bank_name varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_app_promoter_bank_details MODIFY prm_bank_state varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_app_promoter_bank_details MODIFY prm_bank_taluka varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_empchair_det MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_empchair_det MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_empchair_det MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_empchairhist_det MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_empchairhist_det MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_empchairhist_det MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_empdesig_tab MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_empdesig_tab MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_empdesig_tab MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_empdesighist_tab MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_empdesighist_tab MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_empdesighist_tab MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_empdsc_tab MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_empdsc_tab MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_empdsc_tab MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_emplogin_tab MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_emplogin_tab MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_emplogin_tab MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_enq_address_det MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_address_det MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_address_det MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_enq_bank_det MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_bank_det MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_bank_det MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_enq_basic_det MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_basic_det MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_basic_det MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_enq_doc_det MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_doc_det MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_doc_det MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_enq_gasset_det MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_gasset_det MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_gasset_det MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci; 
	
	ALTER TABLE ksfc_oct.tbl_enq_gbank_det MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_gbank_det MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_gbank_det MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_enq_gliab_det MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_gliab_det MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_gliab_det MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_enq_gnw_det MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_gnw_det MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_gnw_det MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_enq_guar_det MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_guar_det MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_guar_det MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_enq_mftotal_det MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_mftotal_det MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_mftotal_det MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_enq_passet_det MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_passet_det MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_passet_det MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_enq_pbank_det MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_pbank_det MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_pbank_det MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_enq_pjcost_det MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_pjcost_det MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_pjcost_det MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_enq_pjfin_det MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_pjfin_det MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_pjfin_det MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_enq_pjmf_det MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_pjmf_det MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_pjmf_det MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_enq_pliab_det MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_pliab_det MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_pliab_det MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_enq_pnw_det MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_pnw_det MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_pnw_det MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_enq_prom_det MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_prom_det MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_prom_det MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_enq_regno_det MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_regno_det MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_regno_det MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_enq_sec_det MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_sec_det MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_sec_det MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci; 
	
	ALTER TABLE ksfc_oct.tbl_enq_sfin_det MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_sfin_det MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_sfin_det MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_enq_sis_det MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_sis_det MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_sis_det MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_enq_temptab MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_temptab MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_temptab MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_enq_trcost_det MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_trcost_det MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_trcost_det MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_enq_wc_det MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_wc_det MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_enq_wc_det MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_fincomp_cdtab MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_fincomp_cdtab MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_fincomp_cdtab MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	
	ALTER TABLE ksfc_oct.tbl_finyear_cdtab MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_finyear_cdtab MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_finyear_cdtab MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
    
    ALTER TABLE ksfc_oct.tbl_trg_employee MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_trg_employee MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_trg_employee MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
    
    ALTER TABLE ksfc_oct.tbl_chair_cdtab MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_chair_cdtab MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_chair_cdtab MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
    
    ALTER TABLE ksfc_oct.tbl_assettype_cdtab MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_assettype_cdtab MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_assettype_cdtab MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
    
    
     ALTER TABLE ksfc_oct.tbl_domi_cdtab MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_domi_cdtab MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_domi_cdtab MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
    
    
     ALTER TABLE ksfc_oct.tbl_assetcat_cdtab MODIFY created_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_assetcat_cdtab MODIFY modified_by varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
	ALTER TABLE ksfc_oct.tbl_assetcat_cdtab MODIFY unique_id varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
    COMMIT;
END;
END$$

DELIMITER ;
call KSFC_oct_NewDump_Collate_DDL_SP()