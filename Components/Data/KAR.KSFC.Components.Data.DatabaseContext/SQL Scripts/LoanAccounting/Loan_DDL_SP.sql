USE `ksfc_oct`;
DROP procedure IF EXISTS `Loan_Related_Receipts`;

DELIMITER $$
USE `ksfc_oct`$$
CREATE PROCEDURE `Loan_Related_Receipts` ()

BEGIN
DECLARE DBName varchar(50) DEFAULT "ksfc_oct";
-- -----------------------Loan_Related_Receipts ----------------------------
        
         						-- code_table--
		Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'code_table')

		-- If exists, retreive columns information from that table
		THEN
		
		select 'code_table table already exist ' as ' ';
		else
			CREATE TABLE code_table (
			id int NOT NULL AUTO_INCREMENT,
			module_name varchar(50) NOT NULL,
			code_type varchar(50) NOT NULL,
			code_name varchar(50) NOT NULL,
			code_value varchar(50) NOT NULL,
			display_sequence varchar(50) NOT NULL,
			is_active bit(1) NOT NULL,
			PRIMARY KEY (`id`)
			);
			select 'code_table table Created' as ' ';
		END IF;
		END;
        
           						-- tbl_la_payment--
		Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_la_payment')

		-- If exists, retreive columns information from that table
		THEN
		
		select 'tbl_la_payment table already exist ' as ' ';
		else
			CREATE TABLE tbl_la_payment (
			id int AUTO_INCREMENT PRIMARY KEY NOT NULL,
			loan_no bigint NOT NULL,
			promoter_id int NOT NULL,
			payment_ref_no varchar(50) DEFAULT NULL,
			actual_amt decimal(15,2) DEFAULT NULL,
			date_of_initiation datetime DEFAULT NULL,
			promoter_name varchar(50) DEFAULT NULL,
			cheque_no varchar(20) DEFAULT NULL,
			cheque_date datetime DEFAULT NULL,
			ifsc_code varchar(20) DEFAULT NULL,
			branch_code varchar(20) DEFAULT NULL,
			date_of_cheque_realization datetime DEFAULT NULL,
			utr_no varchar(20) DEFAULT NULL,
			paid_date datetime DEFAULT NULL,
			payment_mode int DEFAULT NULL,
			payment_status varchar(30) DEFAULT NULL,
			is_active bit(1) DEFAULT NULL,
			is_deleted bit(1) DEFAULT NULL,
			created_by varchar(50) DEFAULT NULL,
			created_date datetime DEFAULT NULL,
			modified_by varchar(50) DEFAULT NULL,
			modified_date datetime DEFAULT NULL
			);
			select 'tbl_la_payment table Created' as ' ';
		END IF;
		END;
        
                						-- tbl_la_receipt--
		Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_la_receipt')

		-- If exists, retreive columns information from that table
		THEN
		
		select 'tbl_la_receipt table already exist ' as ' ';
		else
			CREATE TABLE tbl_la_receipt (
			id int AUTO_INCREMENT PRIMARY KEY NOT NULL,
			trans_type_id int NOT NULL,
			loan_no bigint NOT NULL,
			receipt_ref_no varchar(50) DEFAULT NULL,
			receipt_status varchar(50) DEFAULT NULL,
			date_of_generation datetime DEFAULT NULL,
			amount_due decimal(15,2) DEFAULT NULL,
			due_date_payment datetime DEFAULT NULL,
			`remarks` varchar(200) DEFAULT NULL,
			is_active bit(1) DEFAULT NULL,
			is_deleted bit(1) DEFAULT NULL,
			created_by varchar(50) DEFAULT NULL,
			created_date datetime DEFAULT NULL,
			modified_by varchar(50) DEFAULT NULL,
			modified_date datetime DEFAULT NULL,
			FOREIGN KEY (trans_type_id) REFERENCES code_table (id)
			);
			select 'tbl_la_receipt table Created' as ' ';
		END IF;
		END;
        
                        						-- tbl_la_receipt_payment--
		Begin
		IF EXISTS(SELECT table_name 
					FROM INFORMATION_SCHEMA.TABLES
				   WHERE table_schema = DBName
					 AND table_name LIKE 'tbl_la_receipt_payment')

		-- If exists, retreive columns information from that table
		THEN
		
		select 'tbl_la_receipt_payment table already exist ' as ' ';
		else
			CREATE TABLE tbl_la_receipt_payment (
			id int AUTO_INCREMENT PRIMARY KEY NOT NULL,
			payment_id int DEFAULT NULL,
			receipt_id int NOT NULL,
			receipt_payment_status varchar(50) DEFAULT NULL,
			date_of_initiation varchar(10) DEFAULT NULL,
			payment_amt decimal(15,2) DEFAULT NULL,
			total_amt decimal(15,2) DEFAULT NULL,
			actual_amt decimal(15,2) DEFAULT NULL,
			balance_amt decimal(15,2) DEFAULT NULL,
			is_active bit(1) DEFAULT NULL,
			is_deleted bit(1) DEFAULT NULL,
			created_by varchar(50) DEFAULT NULL,
			created_date datetime DEFAULT NULL,
			modified_by varchar(50) DEFAULT NULL,
			modified_date datetime DEFAULT NULL,
			unique_id varchar(50) DEFAULT NULL,
			FOREIGN KEY (payment_id) REFERENCES tbl_la_payment (id),
			FOREIGN KEY (receipt_id) REFERENCES tbl_la_receipt (id)
			);
			select 'tbl_la_receipt_payment table Created' as ' ';
		END IF;
		END;
        END$$

DELIMITER ;

Call Loan_Related_Receipts()
