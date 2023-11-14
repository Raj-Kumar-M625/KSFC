START TRANSACTION;

INSERT INTO `code_table` (`module_name`, `code_type`, `code_name`, `code_value`, `display_sequence`, `is_active`) VALUES ('LoanAccounting', 'ModeOfPayment', 'Online', 'Online', '10', b'1');
INSERT INTO `code_table` (`module_name`, `code_type`, `code_name`, `code_value`, `display_sequence`, `is_active`) VALUES ('LoanAccounting', 'ModeOfPayment', 'Offline', 'Offline', '20', b'1');
INSERT INTO `code_table` (`module_name`, `code_type`, `code_name`, `code_value`, `display_sequence`, `is_active`) VALUES ('LoanAccounting','TransactionType', 'LAFD(Applicant fee deposit)', 'LAFD(Applicant fee deposit)', '30', b'1');
INSERT INTO `code_table` (`module_name`, `code_type`, `code_name`, `code_value`, `display_sequence`, `is_active`) VALUES ('LoanAccounting', 'TransactionType', 'Registration Charges', 'Registration Charges', '40', b'1');
INSERT INTO `code_table` (`module_name`, `code_type`, `code_name`, `code_value`, `display_sequence`, `is_active`) VALUES ('LoanAccounting', 'TransactionType', 'Penalty Charges', 'Penalty Charges', '50', b'1');
INSERT INTO `code_table` (`module_name`, `code_type`, `code_name`, `code_value`, `display_sequence`, `is_active`) VALUES ('LoanAccounting', 'TransactionType', 'Recovery of excess payment from suppliers', 'Recovery of excess payment from suppliers', '60', b'1');
INSERT INTO `code_table` (`module_name`, `code_type`, `code_name`, `code_value`, `display_sequence`, `is_active`) VALUES ('LoanAccounting', 'TransactionType', 'Cersai Charges', 'Cersai Charges', '70', b'1');
INSERT INTO `code_table` (`module_name`, `code_type`, `code_name`, `code_value`, `display_sequence`, `is_active`) VALUES ('LoanAccounting', 'TransactionType', 'Insurance Charges', 'Insurance Charges', '80', b'1');
INSERT INTO `code_table` (`module_name`, `code_type`, `code_name`, `code_value`, `display_sequence`, `is_active`) VALUES ('LoanAccounting', 'PaymentStatus', 'Pending', 'Pending', '90', b'1');
INSERT INTO `code_table` (`module_name`, `code_type`, `code_name`, `code_value`, `display_sequence`, `is_active`) VALUES ('LoanAccounting', 'PaymentStatus', 'Not Paid', 'Not Paid', '100', b'1');
INSERT INTO `code_table` (`module_name`, `code_type`, `code_name`, `code_value`, `display_sequence`, `is_active`) VALUES ('LoanAccounting', 'PaymentStatus', 'Payment Initiated', 'Payment Initiated', '110', b'1');
INSERT INTO `code_table` (`module_name`, `code_type`, `code_name`, `code_value`, `display_sequence`, `is_active`) VALUES ('LoanAccounting', 'PaymentStatus', 'Fully paid', 'Fully paid', '120', b'1');
INSERT INTO `code_table` (`module_name`, `code_type`, `code_name`, `code_value`, `display_sequence`, `is_active`) VALUES ('LoanAccounting', 'PaymentStatus', 'Partially paid', 'Partially paid', '130', b'1');
INSERT INTO `code_table` (`module_name`, `code_type`, `code_name`, `code_value`, `display_sequence`, `is_active`) VALUES ('LoanAccounting', 'PaymentStatus', 'Rejected', 'Rejected', '140', b'1');
INSERT INTO `code_table` (`module_name`, `code_type`, `code_name`, `code_value`, `display_sequence`, `is_active`) VALUES ('LoanAccounting', 'PaymentStatus', 'Payment Failed', 'Payment Failed', '150', b'1');

INSERT INTO `promsession_tab` (`PROM_PAN`, `LOGIN_DATE_TIME`, `IPADRESS`, `ACCESSTOKEN`, `REFRESHTOKEN`, `ACCESSTOKENREVOKE`, `REFRESHTOKENREVOKE`, `SESSION_STATUS`, `is_active`) VALUES ('AAAAAAAAAA', '2022-09-26 12:31:15', '::1', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwYW4iOiJBQUFBQUFBQUFBIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQ3VzdG9tZXIiLCJJcEFkZHJlc3MiOiI6OjEiLCJleHAiOjE2NjQxNzEyMTMsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjQ0Mzc5IiwiYXVkIjoiVXNlciJ9.y_hYDRsunsC4WC2SkKLZGQo7OKJsFDDg9psV-O5ynEg', 'CzwbrspczJPFz5tti1zViNd2ot96S4wuR7mvItefftE=', '0', '0', '1', b'1');
INSERT INTO `regduser_tab` (`USER_PAN`, `USER_MOBILENO`, `USER_REGN_DATE`, `is_active`, `is_deleted`) VALUES ('AAAAAAAAAA', '9876542541', '2022-09-26 12:30:26', b'1', b'0');

COMMIT;
