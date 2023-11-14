CREATE TABLE [dbo].[Audit](
	[Id] [bigint] IDENTITY(1,1) NOT NULL Primary Key,
	[TableName] [nvarchar](100) NOT NULL,
	[PrimaryKey] [nvarchar](50) NOT NULL,
	[FieldName] [nvarchar](100) NOT NULL,
	[OldValue] [nvarchar](512) NULL,
	[NewValue] [nvarchar](512) NULL,
	[Timestamp] [datetime2](7) NOT NULL DEFAULT (sysutcdatetime()),
	[User] [nvarchar](50) NULL
)