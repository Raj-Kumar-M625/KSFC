-- Feb 14 2021

insert into dbo.CodeTable
(CodeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
( 'NumberPrefix','STR', 'T21/', 10, 1, 1),
( 'NumberPrefix','DWS', 'W21/', 20, 1, 1),
( 'NumberPrefix','IssueReturn', 'IR21/', 30, 1, 1)
go

ALTER TABLE [dbo].[DWS]
ALTER COLUMN [DWSNumber] NVARCHAR(50) NOT NULL
GO

ALTER TABLE [dbo].[SqliteSTRDWS]
ALTER COLUMN [DWSNumber] NVARCHAR(50) NOT NULL
GO

ALTER TABLE [dbo].[DWSAudit]
ALTER COLUMN [DWSNumber] NVARCHAR(50) NOT NULL
GO

