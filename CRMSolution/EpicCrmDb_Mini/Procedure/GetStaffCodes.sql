CREATE PROCEDURE [dbo].[GetStaffCodes]
 @tenantId bigint,
 @todayDate Date,
 @sprocName VARCHAR(50)
AS
BEGIN
    -- Gets the list of staff codes by running sproc dynamically - used in SMS
	DECLARE @ParmDefinition NVARCHAR(500) = N'@tenantIdIn BIGINT, @todayDateIn Date'

	declare @sqlStatement NVARCHAR(2048)
	set @sqlStatement = 'exec [dbo].' + @sprocName + ' @tenantIdIn, @todayDateIn';
	print @sqlStatement

	EXECUTE sp_executesql
		@sqlStatement,
		@ParmDefinition,
		@tenantIdIn = @tenantId,
		@todayDateIn = @todayDate
END
