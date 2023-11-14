
 -- ------------------------------------ Add Constraints Section --------------------------------------------

START TRANSACTION;

ALTER TABLE tbl_idm_dhcg_allc 
			ADD CONSTRAINT FK_tbl_idm_dhcg_tbl_allc_cdtab
			  FOREIGN KEY (dcalc_cd)
			   REFERENCES tbl_allc_cdtab (allc_id);

ALTER TABLE tbl_idm_dhcg_allc 
			ADD CONSTRAINT FK_tbl_idm_dhcg_allc_tbl_app_loan_mast
			  FOREIGN KEY (loan_acc)
			   REFERENCES tbl_app_loan_mast (ln_no);

COMMIT;