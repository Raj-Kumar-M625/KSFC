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

-- GSTRegistration Types
if not exists(select 1 from CommonMaster where CodeType = 'GSTRegistrationType')
begin

INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('GSTRegistrationType', 'Registred', 'Registred', 10, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('GSTRegistrationType', 'UnRegistred', 'UnRegistred', 20, 1);

end
-- VendorCategory Types

if not exists(select 1 from CommonMaster where CodeType = 'VendorCategoryType')
begin

INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('VendorCategoryType', 'CatOne', 'CatOne', 10, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('VendorCategoryType', 'CatTwo', 'CatTwo', 20, 1);

end

-- PaymentTerms Types
if not exists(select 1 from CommonMaster where CodeType = 'PaymentTermsType')
begin

INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('PaymentTermsType', 'Net 0 Days', '0', 10, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('PaymentTermsType', 'Net 15 Days', '15', 20, 1);

end

-- PaymentMethod Types
if not exists(select 1 from CommonMaster where CodeType = 'PaymentMethodType')
begin

INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('PaymentMethodType', 'RTGS', 'RTGS', 10, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('PaymentMethodType', 'NEFT', 'NEFT', 20, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('PaymentMethodType', 'IMPS', 'IMPS', 30, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('PaymentMethodType', 'CHEQUE', 'CHEQUE', 40, 1);

end

-- TDSSection Types
if not exists(select 1 from CommonMaster where CodeType = 'TDSSectionType')
begin

INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('TDSSectionType', '194C Individual / Partnership', '194C Individual / Partnership', 10, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('TDSSectionType', '194C Company', '194C Company', 20, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('TDSSectionType', '194H', '194H', 30, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('TDSSectionType', '194J', '194J', 40, 1);

end

-- VendorDocument Types
if not exists(select 1 from CommonMaster where CodeType = 'VendorDocumentType')
begin

INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('VendorDocumentType', 'PAN', 'PAN', 10, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('VendorDocumentType', 'Shops Estiblishment Certificate', 'Shops Estiblishment Certificate', 20, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('VendorDocumentType', 'Others', 'Others', 30, 1);

end

-- City Types
if not exists(select 1 from CommonMaster where CodeType = 'CityType')
begin

INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('CityType', 'Delhi', 'Delhi', 10, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('CityType', 'Mumbai', 'Mumbai', 20, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('CityType', 'Bangalore', 'Bangalore', 30, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('CityType', 'Kolkata', 'Kolkata', 40, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('CityType', 'Chennai', 'Chennai', 50, 1);

end

-- State Types
if not exists(select 1 from CommonMaster where CodeType = 'StateType')
begin

INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('StateType', 'Andra Pradesh', 'Andra Pradesh', 10, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('StateType', 'Arunachal Pradesh', 'Arunachal Pradesh', 20, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('StateType', 'Assam', 'Assam', 30, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('StateType', 'Bihar', 'Bihar', 40, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('StateType', 'Chhattisgarh', 'Chhattisgarh', 50, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('StateType', 'Goa', 'Goa', 60, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('StateType', 'Gujarat', 'Gujarat', 70, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('StateType', 'Haryana', 'Haryana', 80, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('StateType', 'Himachal Pradesh', 'Himachal Pradesh', 90, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('StateType', 'Jammu and Kashmir', 'Jammu and Kashmir', 100, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('StateType', 'Jharkhand', 'Jharkhand', 110, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('StateType', 'Karnataka', 'Karnataka', 120, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('StateType', 'Kerala', 'Kerala', 130, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('StateType', 'Madya Pradesh', 'Madya Pradesh', 140, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('StateType', 'Maharashtra', 'Maharashtra', 150, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('StateType', 'Manipur', 'Manipur', 160, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('StateType', 'Meghalaya', 'Meghalaya', 170, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('StateType', 'Mizoram', 'Mizoram', 180, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('StateType', 'Nagaland', 'Nagaland', 190, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('StateType', 'Orissa', 'Orissa', 200, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('StateType', 'Punjab', 'Punjab', 210, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('StateType', 'Rajasthan', 'Rajasthan', 220, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('StateType', 'Sikkim', 'Sikkim', 230, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('StateType', 'Tamil Nadu', 'Tamil Nadu', 240, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('StateType', 'Telagana', 'Telagana', 250, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('StateType', 'Tripura', 'Tripura', 260, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('StateType', 'Uttaranchal', 'Uttaranchal', 270, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('StateType', 'Uttar Pradesh', 'Uttar Pradesh', 280, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('StateType', 'West Bengal', 'West Bengal', 290, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('StateType', 'Andaman and Nicobar Islands', 'Andaman and Nicobar Islands', 300, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('StateType', 'Chandigarh', 'Chandigarh', 310, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('StateType', 'Dadar and Nagar Haveli', 'Dadar and Nagar Haveli', 320, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('StateType', 'Daman and Diu', 'Daman and Diu', 330, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('StateType', 'Delhi', 'Delhi', 340, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('StateType', 'Lakshadeep', 'Lakshadeep', 350, 1);
INSERT INTO CommonMaster ("CodeType", "CodeName", "CodeValue", "DisplaySequence", "IsActive") VALUES ('StateType', 'Pondicherry', 'Pondicherry', 360, 1);

end

