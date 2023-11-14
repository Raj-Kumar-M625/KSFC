
 -- ------------------------------------ Add Constraints Section --------------------------------------------

START TRANSACTION;

ALTER TABLE idm_othdebits_details 
			ADD CONSTRAINT FK_idm_othdebits_details_idm_othdebits_mast
			  FOREIGN KEY (dsb_othdebit_id)
			   REFERENCES idm_othdebits_mast (dsb_othdebit_id);


COMMIT;