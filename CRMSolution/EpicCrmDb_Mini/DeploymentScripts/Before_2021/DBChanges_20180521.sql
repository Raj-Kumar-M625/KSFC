Alter table dbo.SalesPerson
add [Department] NVARCHAR(50),
	[Designation] NVARCHAR(50),
	[OwnershipType] NVARCHAR(50),
	[VehicleType] NVARCHAR(50),
	[FuelType] NVARCHAR(50),
	[VehicleNumber] NVARCHAR(15)
go

delete from dbo.CodeTable where CodeType = 'Department'
delete from dbo.CodeTable where CodeType = 'Designation'
delete from dbo.CodeTable where CodeType = 'OwnershipType'
delete from dbo.CodeTable where CodeType = 'VehicleType'
delete from dbo.CodeTable where CodeType = 'FuelType'
go

insert into dbo.CodeTable
(CodeType, CodeValue, CodeName, DisplaySequence, IsActive)
values
--Department
('Department','Admin','Admin',10,1),
('Department','Development','Development',20,1),
('Department','Marketing','Marketing',30,1),
('Department','Other','Other',40,1),

--Designation
('Designation','ABM','ABM',10,1),
('Designation','ACT','ACT',20,1),
('Designation','ACT. ASST.','ACT. ASST.',30,1),
('Designation','ASM','ASM',40,1),
('Designation','BM','BM',50,1),
('Designation','CMM','CMM',60,1),
('Designation','DMM','DMM',70,1),
('Designation','DO','DO',80,1),
('Designation','DRIVER','DRIVER',90,1),
('Designation','DY.ASM','DY.ASM',100,1),
('Designation','FA','FA',110,1),
('Designation','JSR','JSR',120,1),
('Designation','OB','OB',130,1),
('Designation','RBM','RBM',140,1),
('Designation','RMM','RMM',150,1),
('Designation','SBM','SBM',160,1),
('Designation','SDO','SDO',170,1),
('Designation','SO','SO',180,1),
('Designation','SR','SR',190,1),
('Designation','SR.ACT','SR.ACT',200,1),
('Designation','SSO','SSO',210,1),
('Designation','SSR','SSR',220,1),
('Designation','ST','ST',230,1),
('Designation','OTHER','OTHER',240,1),

--OwnershipType
('OwnershipType','Company','Company',10,1),
('OwnershipType','Own','Own',20,1),
('OwnershipType','No Vehicle','No Vehicle',30,1),
('OwnershipType','Other','Other',40,1),

--VehicleType
('VehicleType','Two Wheeler','Two Wheeler',10,1),
('VehicleType','Four Wheeler','Four Wheeler',20,1),
('VehicleType','Other','Other',30,1),

--FuelType
('FuelType','CNG','CNG',10,1),
('FuelType','Diesel','Diesel',20,1),
('FuelType','Electric','Electric',30,1),
('FuelType','Petrol','Petrol',40,1),
('FuelType','Other','Other',50,1)
go
