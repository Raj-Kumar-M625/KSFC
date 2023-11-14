START TRANSACTION;

-- Sidbi Approval -- Added by Dev on 19/09/2022
INSERT INTO  tbl_prom_type_cdtab (prom_type_dets,prom_type_dis_seq,is_active) 
VALUES ('Professional Promoter',1,1);
INSERT INTO  tbl_prom_type_cdtab (prom_type_dets,prom_type_dis_seq,is_active) 
VALUES ('Financial Promoter',2,1);
INSERT INTO tbl_idm_sidbi_approval(prom_type_cd,loan_acc,loan_sub,ln_sanc_amt,offc_cd,wh_appr,is_active,is_deleted) 
VALUES (1,98765432101,1,100000,11,1,1,0);

-- FIC Module -- Added by Akhila
INSERT INTO `tbl_idm_dchg_fic` (`dcfic_offc`, `dcfic_loan_no`, `dcfic_rqdt`, `dcfic_amt`, `dcfic_comdt`, `dcfic_amt_original`, `is_active`, `is_deleted`) VALUES ( '11', '98765432101', '2022-06-23', '30.00', '2022-08-03', '20', 1, 0);

--- Additional Conditional --- Added by Sandeep M
INSERT INTO `tbl_idm_addlcond_det` (`loan_acc`, `loan_sub`, `offc_cd`, `addl_cond_code`, `addl_cond_stg`, `addl_cond_details`, `wh_relaxation`, `is_active`, `is_deleted`, `created_by`, `created_date`) VALUES ('98765432101', 1, 11, 1, 1, 'Demo', 1, 1, 0, 'Sandeep', '2022-08-24 10:19:45');

--- Disbursement Condition Module --- Added by Dev
INSERT INTO tbl_idm_cond_det (loan_acc,loan_sub,offc_cd,cond_type,is_active,is_deleted) VALUES (98765432101,1,11,1,1,0);

--- Form 8 and Form 13----------------
INSERT INTO `tbl_idm_dsb_fm813` ( `df813_offc`, `df813_unit`, `df813_sno`, `df813_dt`, `df813_ref`, `df813_cc`, `df813_t1`, `df813_a1`, `df813_loan_acc`, `is_active`, `is_deleted`, `modified_date`, `unique_id`) VALUES ( '11', '1', '1', '2022-09-02', 'Form 8 and 13', 'test', '1', '1', '98765432101', b'0', b'1', '2022-09-03 17:09:50', '7d2207f0-0863-4a26-8122-365a38ff5ae5');
INSERT INTO `tbl_idm_dsb_fm813` (`df813_offc`, `df813_unit`, `df813_sno`, `df813_dt`, `df813_ref`, `df813_cc`, `df813_t1`, `df813_a1`, `df813_loan_acc`, `is_active`, `is_deleted`) VALUES ('11', '1', '1', '2022-09-02', 'Form 8 and 13', 'test', '1', '1', '98765432101', b'1', b'0');
INSERT INTO `tbl_fm8_fm13_cdtab` (`form_type_cd`, `form_type`, `is_active`, `is_deleted`) VALUES (1, 'Form 8', b'1', b'0'),(2, 'Form 13', b'1', b'0');

COMMIT;