
 -- ------------------------------------ Add Constraints Section --------------------------------------------

START TRANSACTION;

ALTER TABLE idm_prom_asset_det 
			ADD CONSTRAINT FK_tbl_idm_prom_asset_det_idm_prom_asset_det
			  FOREIGN KEY (land_type)
			   REFERENCES tbl_landtype_mast (landtype_id);

ALTER TABLE idm_promoter_bank_details
		ADD CONSTRAINT idm_promoter_bank_details_ibfk_3
		  FOREIGN KEY (promoter_code)
		   REFERENCES tbl_prom_cdtab(promoter_code);

		   ALTER TABLE tbl_idm_unit_address
		ADD CONSTRAINT tbl_idm_unit_address_tbl_pincode_master
		  FOREIGN KEY (ut_pincode)
		   REFERENCES tbl_pincode_master(pincode_rowid);

COMMIT;