USE `ksfc_oct`;
DROP procedure IF EXISTS `Document_Upload_SP`;

DELIMITER $$
USE `ksfc_oct`$$
CREATE PROCEDURE `Document_Upload_SP` ()
BEGIN
DECLARE DBName varchar(50) DEFAULT "ksfc_oct";
-- -----------------------DocumentUpload ----------------------------------
						-- tbl_ld_document --

Begin
IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_Ld_document')

					 -- If not exists, creat a new table
 Then
 select'tbl_ld_document table Already Exist' as ' ';
 else
 CREATE TABLE tbl_Ld_document (
  Id int NOT NULL AUTO_INCREMENT,
  SubModuleId int NOT NULL,
  SubModuleType varchar(50) DEFAULT NULL,
  MainModule varchar(50) NOT NULL,
  FileName varchar(150) DEFAULT NULL,
  FilePath varchar(200) DEFAULT NULL,
  FileType varchar(45) DEFAULT NULL,
  is_active bit(1) DEFAULT NULL,
  is_deleted bit(1) DEFAULT NULL,
  created_by varchar(45) DEFAULT NULL,
  created_date datetime DEFAULT NULL,
  modified_date datetime DEFAULT NULL,
  modified_by varchar(45) DEFAULT NULL,
  unique_id varchar(150) DEFAULT NULL,
  PRIMARY KEY (Id),
  UNIQUE KEY id_UNIQUE (Id)
);
select'tbl_ld_document table Created' as ' ';
END IF;
END;

-- ---------------------- tbl_uc_document --------------------

Begin
IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_Uc_document')

					 -- If not exists, creat a new table
 Then
 select'tbl_uc_document table Already Exist' as ' ';
 else
 CREATE TABLE tbl_Uc_document (
  Id int NOT NULL AUTO_INCREMENT,
  SubModuleId int NOT NULL,
  SubModuleType varchar(50) DEFAULT NULL,
  MainModule varchar(50) NOT NULL,
  FileName varchar(150) DEFAULT NULL,
  FilePath varchar(200) DEFAULT NULL,
  FileType varchar(45) DEFAULT NULL,
  is_active bit(1) DEFAULT NULL,
  is_deleted bit(1) DEFAULT NULL,
  created_by varchar(45) DEFAULT NULL,
  created_date datetime DEFAULT NULL,
  modified_date datetime DEFAULT NULL,
  modified_by varchar(45) DEFAULT NULL,
  unique_id varchar(150) DEFAULT NULL,
  PRIMARY KEY (Id),
  UNIQUE KEY id_UNIQUE (Id)
);
select'tbl_uc_document table Created' as ' ';
END IF;
END;

-- ---------------------- tbl_dc_document --------------------

Begin
IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_dc_document')

					 -- If not exists, creat a new table
 Then
  select'tbl_dc_document table Already Exist' as ' ';
  ELSE
 CREATE TABLE tbl_dc_document (
  Id int NOT NULL AUTO_INCREMENT,
  SubModuleId int NOT NULL,
  SubModuleType varchar(50) DEFAULT NULL,
  MainModule varchar(50) NOT NULL,
  FileName varchar(150) DEFAULT NULL,
  FilePath varchar(200) DEFAULT NULL,
  FileType varchar(45) DEFAULT NULL,
  is_active bit(1) DEFAULT NULL,
  is_deleted bit(1) DEFAULT NULL,
  created_by varchar(45) DEFAULT NULL,
  created_date datetime DEFAULT NULL,
  modified_date datetime DEFAULT NULL,
  modified_by varchar(45) DEFAULT NULL,
  unique_id varchar(150) DEFAULT NULL,
  PRIMARY KEY (Id),
  UNIQUE KEY id_UNIQUE (Id)
);
  select'tbl_dc_document table Created' as ' ';
END IF;
END;
-- ---------------------- tbl_insp_document --------------------

Begin
IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_insp_document')

					 -- If not exists, creat a new table
 Then
 select'tbl_insp_document table Already Exist' as ' ';
 else
 CREATE TABLE `tbl_insp_document` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `SubModuleId` int NOT NULL,
  `SubModuleType` varchar(50) DEFAULT NULL,
  `MainModule` varchar(50) NOT NULL,
  `FileName` varchar(150) DEFAULT NULL,
  `FilePath` varchar(200) DEFAULT NULL,
  `FileType` varchar(45) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(45) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `modified_by` varchar(45) DEFAULT NULL,
  `unique_id` varchar(150) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `id_UNIQUE` (`Id`)
);
select'tbl_insp_document table Created' as ' ';
END IF;
END;


-- ---------------------- tbl_insp_document --------------------

Begin
IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_promoter_document')

					 -- If not exists, creat a new table
 Then
 select'tbl_promoter_document table Already Exist' as ' ';
 else
 CREATE TABLE `tbl_promoter_document` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `SubModuleId` int NOT NULL,
  `SubModuleType` varchar(50) DEFAULT NULL,
  `MainModule` varchar(50) NOT NULL,
  `FileName` varchar(150) DEFAULT NULL,
  `FilePath` varchar(200) DEFAULT NULL,
  `FileType` varchar(45) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(45) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `modified_by` varchar(45) DEFAULT NULL,
  `unique_id` varchar(150) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `id_UNIQUE` (`Id`)
);
select'tbl_promoter_document table Created' as ' ';
END IF;
END;

-- ---------------------- tbl_Cu_document --------------------

Begin
IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_Cu_document')

					 -- If not exists, creat a new table
 Then
 select'tbl_Cu_document table Already Exist' as ' ';
 else
 CREATE TABLE `tbl_Cu_document` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `SubModuleId` int NOT NULL,
  `SubModuleType` varchar(50) DEFAULT NULL,
  `MainModule` varchar(50) NOT NULL,
  `FileName` varchar(150) DEFAULT NULL,
  `FilePath` varchar(200) DEFAULT NULL,
  `FileType` varchar(45) DEFAULT NULL,
  `is_active` bit(1) DEFAULT NULL,
  `is_deleted` bit(1) DEFAULT NULL,
  `created_by` varchar(45) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `modified_date` datetime DEFAULT NULL,
  `modified_by` varchar(45) DEFAULT NULL,
  `unique_id` varchar(150) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `id_UNIQUE` (`Id`)
);
select'tbl_Cu_document table Created' as ' ';
END IF;
END;

END$$

DELIMITER ;

call Document_Upload_SP();