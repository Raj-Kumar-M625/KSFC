
 -- ------------------------------------ Add Constraints Section --------------------------------------------

START TRANSACTION;

ALTER TABLE `tbl_idm_guar_deed_det` 
			ADD INDEX `fk_tbl_idm_guar_deed_det_tbl_app_guar_asset_det_idx` (`app_guarasset_id` ASC) VISIBLE;
     
			ALTER TABLE `tbl_idm_guar_deed_det` 
			ADD CONSTRAINT `fk_tbl_idm_guar_deed_det_tbl_app_guar_asset_det`
			  FOREIGN KEY (`app_guarasset_id`)
			  REFERENCES `tbl_app_guar_asset_det` (`app_guarasset_id`)
			  ON DELETE NO ACTION
			  ON UPDATE NO ACTION;
     
     
			ALTER TABLE tbl_idm_dsb_charge 
			ADD CONSTRAINT fk_tbl_idm_dsb_charge_tbl_ifsc_master_id
			  FOREIGN KEY (bank_ifsc_id_cd)
			  REFERENCES tbl_ifsc_master (ifsc_rowid);

COMMIT;