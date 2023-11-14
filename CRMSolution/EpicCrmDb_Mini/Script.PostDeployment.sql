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

DELETE FROM dbo.EmployeeDay;
DELETE FROM dbo.TenantEmployee;
DELETE FROM dbo.Tenant;
DELETE FROM dbo.[Day]
GO

INSERT INTO dbo.Tenant
(Name, [Description])
values
('Poonalab', 'Poonalab')
go

DECLARE @tenantId BIGINT
SELECT @tenantId = Id from dbo.Tenant where Name='Poonalab'

INSERT INTO dbo.TenantEmployee
(TenantId, Name)
VALUES
(@tenantId, 'Guru'),
(@tenantId, 'NagaArjun')
go

--DECLARE @appStartDay SmallDATETIME = SysUtcDateTime()
--DECLARE @i int = 1

--WHILE @i < (365*3)
--BEGIN
--   INSERT INTO dbo.[Day]
--   ([Date])
--   Values
--   (DATEADD(Day, @i, @appStartDay))

--   SET @i = @i + 1
--END
GO
*/