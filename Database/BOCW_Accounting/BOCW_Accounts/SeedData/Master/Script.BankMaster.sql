/*
Post-Deployment Script Template
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.
 Use SQLCMD syntax to include a file in the post-deployment script.
 Example:      :r .\myfile.sql
 Use SQLCMD syntax to reference a variable in the post-deployment script.
 Example:      :setvar TableName MyTable
               SELECT * FROM [$(TableName)]
--------------------------------------------------------------------------------------
*/


-- BankMaster
if not exists(select 1 from BankMaster)
begin

INSERT INTO BankMaster ("BankName", "BranchName", "IfscCode", "StartDate", "EndDate", "Status", "CreatedOn", "ModifiedOn") VALUES ('SBI', 'Rajaginagar', 'IFSC00123', '2022-01-01 00:00:00.0000000', '2023-01-01 00:00:00.0000000', True, '2022-01-01 00:00:00.0000000', '2022-01-01 00:00:00.0000000');
INSERT INTO BankMaster ("BankName", "BranchName", "IfscCode", "StartDate", "EndDate", "Status", "CreatedOn", "ModifiedOn") VALUES ('Bank Of Baroda', 'Majestic', 'IFSC001234', '2022-01-01 00:00:00.0000000', '2023-01-01 00:00:00.0000000', True, '2022-01-01 00:00:00.0000000', '2022-01-01 00:00:00.0000000');
INSERT INTO BankMaster ("BankName", "BranchName", "IfscCode", "StartDate", "EndDate", "Status", "CreatedOn", "ModifiedOn") VALUES ('SBI', 'Domlur', 'IFSC00DOM4', '2021-01-01 00:00:00.0000000', '2023-01-01 00:00:00.0000000', True, '2022-01-01 00:00:00.0000000', '2022-01-01 00:00:00.0000000');

end