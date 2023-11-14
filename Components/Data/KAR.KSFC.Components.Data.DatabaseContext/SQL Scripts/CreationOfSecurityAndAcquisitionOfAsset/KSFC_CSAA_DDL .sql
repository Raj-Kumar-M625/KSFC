
 -- ------------------------------------ Add Constraints Section --------------------------------------------


START TRANSACTION;

ALTER TABLE  tbl_idm_ir_land 
			ADD CONSTRAINT FK_tbl_idm_ir_land_tbl_landtype_mast
			  FOREIGN KEY (irl_landty)
			   REFERENCES tbl_landtype_mast(landtype_id);

COMMIT;