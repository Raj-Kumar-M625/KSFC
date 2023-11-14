--TO GET LOAN NUMBERS--by Gowtham
INSERT INTO `ksfc_csg`.`tbl_chair_cdtab` (`chair_id`, `chair_desc`, `offc_cd`, `is_active`, `is_deleted`) VALUES ('3', 'C3', '11', b'1', b'0');
UPDATE `ksfc_csg`.`tbl_empchair_det` SET `emp_id` = '00000001', `chair_code` = '3', `is_active` = 1, `is_deleted` = 0 WHERE (`empchair_id` = '1');

--CERSAI Module --- Added by Gagana

UPDATE `ksfc_csg`.`tbl_asset_refno_det` SET `asset_othdetails` = 'Picking Machine' WHERE (`asset_refno_mast_id` = '1');
INSERT INTO `ksfc_csg`.`tbl_idm_cersai_registration` (`loan_acc`, `security_cd`, `asset_cd`, `cersai_reg_no`, `cersai_reg_date`, `cersai_remarks`, `is_active`,`is_deleted`) VALUES ( '98765432101', '1', '1', 'CERZ00123148', '2021-12-15', 'Working123', b'1',b'0');


--Condition Module --- Added by Gagana

INSERT INTO `ksfc_csg`.`tbl_cond_type_cdtab` (`cond_type_dets`, `cond_type_dis_seq`, `is_active`, `is_deleted`) VALUES ('Normal', '10', b'1', b'0'),('Special Condition', '20', b'1', b'0');
INSERT INTO `ksfc_csg`.`tbl_cond_det_cdtab` (`cond_det_dets`, `cond_det_dis_seq`, `is_active`, `is_deleted`) VALUES ('Copy Address proof to be submitted', '10', b'1', b'0'),('Copy Citizen proof to be submitted', '20', b'1', b'0');
INSERT INTO `ksfc_csg`.`tbl_cond_stg_cdtab` (`cond_stg_dets`,`cond_stg_dis_seq`, `is_active`, `is_deleted`) VALUES ('First Investment clause', '10', b'1', b'0'),('Second Investment clause','20', b'1', b'0');
INSERT INTO `ksfc_csg`.`tbl_idm_cond_det` (`loan_acc`, `cond_type`, `cond_stg`, `cond_details`, `is_active`, `is_deleted`,`unique_id`) VALUES ('98765432101', '1', '1', 'Test One', b'1', b'0','abc'),('98765432101', '2', '2', 'Tested', b'1', b'0','iji');


INSERT INTO ksfc_csg.tbl_charge_type_cdtab (chrg_type_dets, chrg_type_dis_seq, is_active, is_deleted, created_by, created_date) VALUES ('Charge type 1', 10, 1, 0, 'Sandeep', '2022-08-10');
INSERT INTO ksfc_csg.tbl_charge_type_cdtab (chrg_type_dets, chrg_type_dis_seq, is_active, is_deleted, created_by, created_date) VALUES ('Charge type 2', 20, 1, 0, 'Sandeep', '2022-08-11');


--Hypothecation Module --- Added by Manoj

INSERT INTO `ksfc_csg`.`tbl_asset_refno_det` (`asset_cd`, `loan_acc`, `loan_sub`, `offc_cd`, `assetcat_cd`, `assettype_cd`, `asset_details`, `asset_value`, `asset_othdetails`, `is_active`, `is_deleted`) VALUES ('6', '98765432101', '1', '11', '1', '1', 'Loan Sanctioned', '450000', 'Kept Land for Loan', b'1', b'0');
INSERT INTO `ksfc_csg`.`tbl_idm_hypoth_det` (`loan_acc`, `loan_sub`, `asset_cd`, `hypoth_no`, `hypoth_desc`, `asset_value`, `execution_date`, `is_active`, `is_deleted`) VALUES ('98765432101', '1', '6', '7563', 'Agricultural land', '450000', '2022/08/09', b'1', b'0');
--Only AssetRef Table values To Create Hypothecation -- Added By Manoj 
INSERT INTO `ksfc_csg`.`tbl_asset_refno_det` (`asset_cd`, `loan_acc`, `loan_sub`, `offc_cd`, `assetcat_cd`, `assettype_cd`, `asset_details`, `asset_value`, `asset_othdetails`, `is_active`, `is_deleted`) VALUES ('7', '98765432101', '1', '11', '1', '1', 'Loan Sanctioned', '450000', 'Kept Land for Loan', b'1', b'0');
INSERT INTO `ksfc_csg`.`tbl_asset_refno_det` (`asset_cd`, `loan_acc`, `loan_sub`, `offc_cd`, `assetcat_cd`, `assettype_cd`, `asset_details`, `asset_value`, `asset_othdetails`, `is_active`, `is_deleted`) VALUES ('8', '98765432101', '1', '11', '1', '1', 'Farmer land', '650000', 'Sanctioned', b'1', b'0');
INSERT INTO `ksfc_csg`.`tbl_asset_refno_det` (`asset_cd`, `loan_acc`, `loan_sub`, `offc_cd`, `assetcat_cd`, `assettype_cd`, `asset_details`, `asset_value`, `asset_othdetails`, `is_active`, `is_deleted`) VALUES ('9', '98765432101', '1', '11', '1', '1', 'Property For Loan', '950000', 'Kept Land for Loan', b'1', b'0');
INSERT INTO `ksfc_csg`.`tbl_idm_cond_det` (`loan_acc`, `cond_type`, `cond_stg`, `cond_details`, `is_active`, `is_deleted`) VALUES ('98765432101', '1', '1', 'Test One', b'1', b'0'),('98765432101', '2', '2', 'Tested', b'1', b'0');

-- Guarantor Deed Module --- Added by Akhila


INSERT INTO `ksfc_csg`.`tbl_idm_deed_det` (`loan_acc`, `loan_sub`, `offc_cd`, `security_cd`, `deed_no`, `deed_desc`, `security_value`, `is_active`, `is_deleted`) VALUES ( '98765432101', '1', '1', '1', '3211', 'Ref.No KSFC/H.O/ED -1 /Legal.Registered Mortage Deed', '6000', b'1', b'0');
INSERT INTO ksfc_csg.tbl_ifsc_master ( ifsc_cd, bank_name, branch_name, bank_address, bank_state, bank_district, bank_taluka, is_active, is_deleted, created_by) VALUES ('BARB0125', 'BOB', 'WCR', 'Basaweshawaranagar', 'KAR', 'fdsf', 'sds', 1, 0, 'Megha');
INSERT INTO ksfc_csg.tbl_idm_dsb_charge (loan_acc, loan_sub, offc_cd, security_cd, charge_type_cd, security_value, noc_issue_by, noc_isssue_to, noc_date, auth_letter_by, auth_letter_date, board_resolution_date, moe_insured_date, request_ltr_no, request_ltr_date, bank_ifsc_cd, bank_request_ltr_no, charge_purpose, charge_details, charge_conditions, is_active, is_deleted) VALUES ('98765432101', 1, 11, 1, 1, 100000, 'Bank', 'Customer', '2022-04-16', 'Bank', '2022-04-16', '2022-05-16', '2022-05-17', '552', '2022-03-16', 'BARB0124', 5512, 'BANK', 'bank', 'Guarantee', 1, 0);
INSERT INTO `ksfc_csg`.`tbl_eg_num_mast` (`eg_num_rowid`, `eg_no`, `enq_ref_no`, `off_cd`, `ut_cd`, `is_active`, `is_deleted`) VALUES ('2', '2', '2','11', '2', '2','2','0');
INSERT INTO `ksfc_csg`.`tbl_app_guar_asset_det` (`promoter_code`, `eg_no`, `offc_cd`, `ut_cd`, `assetcat_cd`, `assettype_cd`, `land_type`, `app_asset_siteno`, `app_asset_addr`, `app_asset_dim`, `app_asset_area`, `app_asset_desc`, `app_asset_value`, `is_active`, `is_deleted`) VALUES ('90', '1', '1', '1', '1', '1', 'Farmer Land', '#3456', 'Near National Highway Mysore Road', '2000*1500', '3500', 'Agricultural land For Loan', '500000', b'1', b'0');
INSERT INTO `ksfc_csg`.`tbl_idm_guar_deed_det` (`loan_acc`, `loan_sub`, `offc_cd`, `promoter_code`, `app_guarasset_id`, `value_asset`, `value_liab`, `value_networth`, `deed_no`, `deed_desc`, `execution_date`, `approved_emp_id`, `is_active`, `is_deleted`) VALUES ('98765432101', '1', '1', '90', '1', '350000', '400000', '450000', '8056', 'Agricultural land', '2022-08-04', '0000001', b'1', b'0');
INSERT INTO `ksfc_csg`.`tbl_pclas_cdtab` (`pclas_cd` ,`pclas_dets`, `id`, `is_active`, `is_deleted`) VALUES ('2','2', '2', b'1', b'0');
INSERT INTO `ksfc_csg`.`tbl_psubclas_cdtab` (`psubclas_desc`, `pclas_cd`, `is_active`, `is_deleted`) VALUES ('Description', '1', b'1', b'0');
INSERT INTO `ksfc_csg`.`tbl_app_guarnator` (`promoter_code`, `eg_no`, `offc_cd`, `ut_cd`, `ut_name`, `guar_name`, `guar_gender`, `guar_dob`, `guar_age`, `name_father_spouse`, `pclas_cd`, `psubclas_cd`, `dom_cd`, `guar_mobile_no`, `is_active`, `is_deleted`) VALUES ('90', '1', '1', '1', 'Rahul', 'Rahul', 'Male', '1995-08-22', '24', 'Ramanna', '1', '1', '1', '8553824656', b'1', b'0');
INSERT INTO `ksfc_csg`.`tbl_app_guar_liab_det` (`promoter_code`, `eg_no`, `offc_cd`, `ut_cd`, `app_liab_desc`, `app_liab_value`, `is_active`, `is_deleted`) VALUES ('90', '1', '1', '1', 'Agricultural Land', '250000', b'1', b'0');
INSERT INTO `ksfc_csg`.`tbl_app_guar_nw_det` (`promoter_code`, `eg_no`, `offc_cd`, `ut_cd`, `app_immov`, `app_mov`, `app_liab`, `app_nw`, `is_active`, `is_deleted`) VALUES ('90', '1', '1', '1', '2500', '5000', '50000', '45000', b'1', b'0');

-- Guarantor Deed Module ---Updated By Added by Akhila -Date - 16-08-2022

INSERT INTO `ksfc_csg`.`tbl_app_guar_asset_det` (`app_guarasset_id`, `promoter_code`, `eg_no`, `offc_cd`, `ut_cd`, `assetcat_cd`, `assettype_cd`, `land_type`, `app_asset_siteno`, `app_asset_addr`, `app_asset_dim`, `app_asset_area`, `app_asset_desc`, `app_asset_value`, `is_active`, `is_deleted`) VALUES ('2', '90', '1', '11', '1', '2', '2', 'Agriculture Land', '#332', 'Near National Highway Kanakapura Road', '600000', '30000', 'Agricultural land For Loan', '800000', b'1', b'0');
INSERT INTO `ksfc_csg`.`tbl_app_guar_liab_det` (`app_guarliab_id`, `promoter_code`, `eg_no`, `offc_cd`, `ut_cd`, `app_liab_desc`, `app_liab_value`, `is_active`, `is_deleted`) VALUES ('2', '90', '1', '11', '1', 'Agricultural Land', '500000.00', b'1', b'0');
INSERT INTO `ksfc_csg`.`tbl_app_guarnator` (`app_guar_id`, `promoter_code`, `eg_no`, `offc_cd`, `ut_cd`, `ut_name`, `guar_name`, `guar_gender`, `guar_dob`, `guar_age`, `name_father_spouse`, `pclas_cd`, `psubclas_cd`, `dom_cd`, `guar_mobile_no`, `guar_email`, `is_active`, `is_deleted`) VALUES ('2', '90', '1', '11', '1', '1', 'Shalini', 'Female', '1992-06-25', '24', 'Raghavendra', '1', '1', '1', '9876543256', 'shalini123@gmail.com', b'1', b'0');
INSERT INTO `ksfc_csg`.`tbl_idm_guar_deed_det` (`idm_guar_deed_id`, `loan_acc`, `loan_sub`, `offc_cd`, `promoter_code`, `app_guarasset_id`, `value_asset`, `value_liab`, `value_networth`, `deed_no`, `deed_desc`, `execution_date`, `approved_emp_id`, `is_active`, `is_deleted`) VALUES ('15', '98765432101', '1', '11', '74', '2', '60000', '69000', '500000', '1002', 'Agricultural Land', '2022-09-16', '0000001', b'1', b'0');


-- Sidbi Approval -- Added by Dev on 19/09/2022

INSERT INTO tbl_idm_sidbi_approval(sidbi_appr_id,prom_type_cd,loan_acc,loan_sub,ln_sanc_amt,offc_cd,wh_appr,is_active) 
VALUES (1,1,98765432101,1,100000,11,1,1);
INSERT INTO tbl_idm_sidbi_approval(sidbi_appr_id,prom_type_cd,loan_acc,loan_sub,ln_sanc_amt,offc_cd,wh_appr,is_active) 
VALUES (2,2,98765432101,2,200000,11,1,1);
INSERT INTO  tbl_prom_type_cdtab (prom_type_dets,prom_type_dis_seq,is_active) 
VALUES ('Professional Promoter',1,1);
INSERT INTO  tbl_prom_type_cdtab (prom_type_dets,prom_type_dis_seq,is_active) 
VALUES ('Financial Promoter',2,1);

--Audit Clearance Module --- Added by Gagana
INSERT INTO `ksfc_csg`.`tbl_idm_audit_det` (`loan_acc`,`offc_cd`, `audit_observation`, `audit_compliance`, `is_active`, `is_deleted`, `unique_id`) VALUES ('98765432101',11, 'Accurate', 'Recommendations Details', b'1', b'0','abc'),('98765432101',11, 'Inaccurate', 'Conditions Details', b'1', b'0','iji');