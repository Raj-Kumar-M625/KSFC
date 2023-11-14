-- build action is set to None for this sproc
-- as it accesses Information_Schema.Columns, which does not exist here.
--
CREATE PROCEDURE [dbo].[TableSchema]
  @tableName NVARCHAR(100)
AS
BEGIN
		-- Stored procedure to get table schema for Upload routine
	SELECT 
	ISNULL(ORDINAL_POSITION,0) Position, 
	ISNULL(Column_name,'') ColumnName, 
	CASE WHEN Is_Nullable = 'YES' THEN 1 ELSE 0 END IsNullable,
	--IS_NULLABLE IsNullable, 
	ISNULL(Data_Type,'') DataType, 
	IsNull(CHARACTER_MAXIMUM_LENGTH, '') CharMaxLen, 
	IsNull(column_Default, '') ColumnDefault
	FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_Name = @tableName
	ORDER by ORDINAL_POSITION
END
GO
