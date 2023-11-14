
START TRANSACTION;

-- ------------------------------------------------------------- Land Acquisition --------------------------------------------------------------------
INSERT INTO `tbl_landtype_mast` (`landtype_cd`, `landtype_desc`, `is_active`, `is_deleted`) VALUES ('2', 'KIADB', b'1', b'0');
INSERT INTO `tbl_landtype_mast` (`landtype_cd`, `landtype_desc`, `is_active`, `is_deleted`) VALUES ('3', 'KSSIDC Shed', b'1', b'0');
INSERT INTO `tbl_landtype_mast` (`landtype_cd`, `landtype_desc`, `is_active`, `is_deleted`) VALUES ('4', 'Shed', b'1', b'0');
INSERT INTO `tbl_landtype_mast` (`landtype_cd`, `landtype_desc`, `is_active`, `is_deleted`) VALUES ('5', 'Rental Shed', b'1', b'0');
INSERT INTO `tbl_landtype_mast` (`landtype_cd`, `landtype_desc`, `is_active`, `is_deleted`) VALUES ('6', 'Leased Land', b'1', b'0');
INSERT INTO `tbl_landtype_mast` (`landtype_cd`, `landtype_desc`, `is_active`, `is_deleted`) VALUES ('7', 'Own Private Land', b'1', b'0');
INSERT INTO `tbl_landtype_mast` (`landtype_cd`, `landtype_desc`, `is_active`, `is_deleted`) VALUES ('8', 'KSIIDC', b'1', b'0');

INSERT INTO `ksfc_oct`.`tbl_procure_mast` (`procure_id`, `procure_desc`) VALUES ('1', 'Procurement Initiated');
INSERT INTO `ksfc_oct`.`tbl_procure_mast` (`procure_id`, `procure_desc`) VALUES ('2', 'Procurement in Transit');
INSERT INTO `ksfc_oct`.`tbl_procure_mast` (`procure_id`, `procure_desc`) VALUES ('3', 'Procured');

INSERT INTO `ksfc_oct`.`tbl_machinery_status` (`mac_status`, `mac_dets`) VALUES ('1', 'NO CHANGE FROM APPRAISAL');
INSERT INTO `ksfc_oct`.`tbl_machinery_status` (`mac_status`, `mac_dets`) VALUES ('2', 'CANCELLED');
INSERT INTO `ksfc_oct`.`tbl_machinery_status` (`mac_status`, `mac_dets`) VALUES ('3', 'ADDED');
INSERT INTO `ksfc_oct`.`tbl_machinery_status` (`mac_status`, `mac_dets`) VALUES ('4', 'CHANGED');
INSERT INTO `ksfc_oct`.`tbl_machinery_status` (`mac_status`, `mac_dets`) VALUES ('5', 'REQUIRED AND BIFURCATED');
INSERT INTO `ksfc_oct`.`tbl_machinery_status` (`mac_status`, `mac_dets`) VALUES ('6', 'CHANGED AND BIFURCATED');
INSERT INTO `ksfc_oct`.`tbl_machinery_status` (`mac_status`, `mac_dets`) VALUES ('7', 'ADDED AND BIFURCATED');
INSERT INTO `ksfc_oct`.`tbl_machinery_status` (`mac_status`, `mac_dets`) VALUES ('8', 'BIFURCATED AND CANCELLED');
INSERT INTO `ksfc_oct`.`tbl_machinery_status` (`mac_status`, `mac_dets`) VALUES ('9', 'BIFURCATED AND CHANGED');
INSERT INTO `ksfc_oct`.`tbl_machinery_status` (`mac_status`, `mac_dets`) VALUES ('10', 'ADDED AND CANCELLED');
INSERT INTO `ksfc_oct`.`tbl_machinery_status` (`mac_status`, `mac_dets`) VALUES ('11', 'ADDED AND CHANGED');
INSERT INTO `ksfc_oct`.`tbl_machinery_status` (`mac_status`, `mac_dets`) VALUES ('12', 'CHANGED AND CANCELLED');
INSERT INTO `ksfc_oct`.`tbl_machinery_status` (`mac_status`, `mac_dets`) VALUES ('13', 'ADDITIONAL NOT FINANCED');

-- INSERT INTO `tbl_pjcostgroup_cdtab` VALUES (1,'test',_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL);
-- INSERT INTO `tbl_pjcsgroup_cdtab` VALUES (1,'tes',1,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL);
-- INSERT INTO `tbl_pjcost_cdtab` VALUES (1,'Equipment Cost',1,1.00,1,1,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL),(2,'Furniture Cost',1,1.00,1,1,_binary '',_binary '\0',NULL,NULL,NULL,NULL,NULL);
-- INSERT INTO `tbl_state_zone_cdtab` VALUES (1,'Karnataka',_binary '',_binary '\0','1',NULL,'2022-05-25 11:28:26',NULL),(2,'Kalyana Karnataka',_binary '',_binary '\0','1',NULL,'2022-05-25 11:28:26',NULL),(3,'Malnad',_binary '',_binary '\0','1',NULL,'2022-05-25 11:28:26',NULL);

COMMIT