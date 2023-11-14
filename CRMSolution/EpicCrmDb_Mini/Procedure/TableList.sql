-- build action is set to None for this sproc
-- as it accesses Information_Schema.Columns, which does not exist here.
--
CREATE PROCEDURE [dbo].[TableList]
AS
BEGIN
		-- Stored procedure to get table names
	SELECT TABLE_NAME
	FROM INFORMATION_SCHEMA.Tables
	WHERE table_Name not like '%Image%'
	AND TABLE_Name not like '%ASPNet%'
	AND TABLE_Name not like '%SysDiagrams%'
	ORDER BY TABLE_Name

END
GO
