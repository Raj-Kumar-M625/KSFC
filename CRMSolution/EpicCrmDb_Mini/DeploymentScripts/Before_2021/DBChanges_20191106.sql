
-- Nov 6 2019
/* 
 -- these two insert and delete statements are there in previous sql file
insert into dbo.CodeTable
(CodeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
( 'CropType', 'Babycorn', 'Babycorn', 20, 1, 1),
( 'CropType', 'Jalapeno pepper', 'Jalapeno pepper', 30, 1, 1),
( 'CropType', 'Banana pepper', 'Banana pepper', 40, 1, 1),
( 'CropType', 'Piri Piri chilli-Green', 'Piri Piri chilli-Green', 50, 1, 1),
( 'CropType', 'Piri Piri chilli-Red', 'Piri Piri chilli-Red', 60, 1, 1)
go

DELETE from dbo.codeTable
where codeType = 'AgreementStatus'
and codeValue = 'No Agreement'
and IsActive = 1
and TenantId = 1
GO
*/
ALTER TABLE [dbo].[FeatureControl]
ADD
	[EmployeeExpenseReport2] BIT NOT NULL DEFAULT 0,
	[UnSownReport] BIT NOT NULL DEFAULT 0,
	[IsReadOnlyUser] BIT NOT NULL DEFAULT 0
GO

CREATE TABLE [dbo].[PPAInput]
(
	[Branch Code] [nvarchar](20) NOT NULL, 
	[Branch Name] [nvarchar](50) NOT NULL, 
	[HQ Code] [nvarchar](10) NOT NULL, 
	[HQ Name] [nvarchar](50) NOT NULL,
	[PPA Code] [Nvarchar](10) NOT NULL, 
	[PPA Name] [Nvarchar](50) NOT NULL, 
	[Contact Number] [NVARCHAR](20) NOT NULL, 
	[Location] NVARCHAR(50) NOT NULL
)
GO

INSERT into dbo.codeTable
(codeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
('ExcelUpload', 'PPA', 'PPAInput', 100, 1, 1)
go

ALTER TABLE [dbo].[EmployeeExpenseModel]
ADD TrackingDistanceInMeters DECIMAL(19,2)
GO
