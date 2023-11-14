
-- =============================================
-- Author:		<Swetha M>
-- Create date: <19-01-2023>
-- Description:	<SP to get the SO Staff Code Details>
-- =============================================

Create PROCEDURE [dbo].[GetSOStaffCodes]
AS
BEGIN
--Select SO StaffCodes --
SELECT DISTINCT(StaffCode) FROM SalesPerson s WHERE LEN(s.[StaffCode]) < 5 and IsActive = 1
AND S.[StaffCode] not IN (SELECT DISTINCT(ParentEmpCode) FROM SOParentSOInput)
END

GO
