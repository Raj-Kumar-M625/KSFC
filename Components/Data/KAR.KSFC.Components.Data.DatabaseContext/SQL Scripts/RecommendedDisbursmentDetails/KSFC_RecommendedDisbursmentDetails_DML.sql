START TRANSACTION;

INSERT INTO `tbl_idm_dsb_dets` (`loan_acc`, `loan_sub`, `offc_cd`, `dsb_no`, `dsb_amt`, `dsb_acd`, `dsb_est_amt`, `sec_considered_f_release`, `sec_inspection`, `Margin_retained`, `aloc_amt`, `is_active`, `is_deleted`, `created_date`, `unique_id`) VALUES ('98765432101', '1', '11', '1', '11111111', '1', '1', '11.00', '1.00', '1', '11.00', b'1', b'0', '2022-10-31 09:38:13', '76be8fc9-e4b1-4701-951c-de40e000653e');



INSERT INTO tbl_dept_master (dept_code,dept_name) VALUES (1,'ENTERPRENURAL GUIDENCE');
INSERT INTO tbl_dept_master (dept_code,dept_name) VALUES (2,'A D M - 1');
INSERT INTO tbl_dept_master (dept_code,dept_name) VALUES (3,'A D M - 2');
INSERT INTO tbl_dept_master (dept_code,dept_name) VALUES (4,'A D M - 3');
INSERT INTO tbl_dept_master (dept_code,dept_name) VALUES (5,'FINANCE & ACCOUNTS');
INSERT INTO tbl_dept_master (dept_code,dept_name) VALUES (6,'TREASURY');
INSERT INTO tbl_dept_master (dept_code,dept_name) VALUES (7,'REFINANCE');
INSERT INTO tbl_dept_master (dept_code,dept_name) VALUES (8,'S U M C');


INSERT INTO `ksfc_oct`.`tbl_dsb_charge_map` (`dsb_othdebit_id`, `data_field_name`) VALUES ('1', 'rele_bnk_chg');
INSERT INTO `ksfc_oct`.`tbl_dsb_charge_map` (`dsb_othdebit_id`, `data_field_name`) VALUES ('2', 'rele_doc_chg');
INSERT INTO `ksfc_oct`.`tbl_dsb_charge_map` (`dsb_othdebit_id`, `data_field_name`) VALUES ('3', 'addl_amt1');
INSERT INTO `ksfc_oct`.`tbl_dsb_charge_map` (`dsb_othdebit_id`, `data_field_name`) VALUES ('4', 'addl_amt2');
INSERT INTO `ksfc_oct`.`tbl_dsb_charge_map` (`dsb_othdebit_id`, `data_field_name`) VALUES ('5', 'addl_amt3');



INSERT INTO `idm_othdebits_mast` (`dsb_othdebit_id`, `dsb_othdebit_desc`, `is_active`, `is_deleted`) VALUES ('6', 'additional_amount1', True, False);

INSERT INTO `idm_othdebits_mast` (`dsb_othdebit_id`, `dsb_othdebit_desc`, `is_active`, `is_deleted`) VALUES ('7', 'additional_amount2', True, False);
INSERT INTO `idm_othdebits_mast` (`dsb_othdebit_id`, `dsb_othdebit_desc`, `is_active`, `is_deleted`) VALUES ('8', 'additional_amount3', True, False);
INSERT INTO `idm_othdebits_mast` (`dsb_othdebit_id`, `dsb_othdebit_desc`, `is_active`, `is_deleted`) VALUES ('9', 'additional_amount4', True, False);
INSERT INTO `idm_othdebits_mast` (`dsb_othdebit_id`, `dsb_othdebit_desc`, `is_active`, `is_deleted`) VALUES ('10', 'additional_amount5', True, False);
INSERT INTO `idm_othdebits_mast` (`dsb_othdebit_id`, `dsb_othdebit_desc`, `is_active`, `is_deleted`) VALUES ('11', 'Due Amount', True, False);
INSERT INTO `idm_othdebits_mast` (`dsb_othdebit_id`, `dsb_othdebit_desc`, `is_active`, `is_deleted`) VALUES ('12', 'At Par Amount', True, False);
INSERT INTO `idm_othdebits_mast` (`dsb_othdebit_id`, `dsb_othdebit_desc`, `is_active`, `is_deleted`) VALUES ('13', 'UpFront Amount', True, False);
INSERT INTO `idm_othdebits_mast` (`dsb_othdebit_id`, `dsb_othdebit_desc`, `is_active`, `is_deleted`) VALUES ('14', 'Commitment Amount', True, False);
INSERT INTO `idm_othdebits_mast` (`dsb_othdebit_id`, `dsb_othdebit_desc`, `is_active`, `is_deleted`) VALUES ('15', 'FD Amount', True, False);
INSERT INTO `idm_othdebits_mast` (`dsb_othdebit_id`, `dsb_othdebit_desc`, `is_active`, `is_deleted`) VALUES ('16', 'Other Amount', True, False);
INSERT INTO `idm_othdebits_mast` (`dsb_othdebit_id`, `dsb_othdebit_desc`, `is_active`, `is_deleted`) VALUES ('17', 'Adjustment Amount', True, False);
INSERT INTO `idm_othdebits_mast` (`dsb_othdebit_id`, `dsb_othdebit_desc`, `is_active`, `is_deleted`) VALUES ('18', 'Additional UpFront Amount', True, False);
INSERT INTO `idm_othdebits_mast` (`dsb_othdebit_id`, `dsb_othdebit_desc`, `is_active`, `is_deleted`) VALUES ('19', 'Additional Land FD Amount', True, False);
INSERT INTO `idm_othdebits_mast` (`dsb_othdebit_id`, `dsb_othdebit_desc`, `is_active`, `is_deleted`) VALUES ('20', 'Service Tax Amount', True, False);
INSERT INTO `idm_othdebits_mast` (`dsb_othdebit_id`, `dsb_othdebit_desc`, `is_active`, `is_deleted`) VALUES ('21', 'Cersai Amount', True, False);
INSERT INTO `idm_othdebits_mast` (`dsb_othdebit_id`, `dsb_othdebit_desc`, `is_active`, `is_deleted`) VALUES ('22', 'Krishi Kalyana CESS', True, False);
INSERT INTO `idm_othdebits_mast` (`dsb_othdebit_id`, `dsb_othdebit_desc`, `is_active`, `is_deleted`) VALUES ('23', 'Coll Gurantee FEE', True, False);
INSERT INTO `idm_othdebits_mast` (`dsb_othdebit_id`, `dsb_othdebit_desc`, `is_active`, `is_deleted`) VALUES ('24', 'Release Amount', True, False);
COMMIT;
