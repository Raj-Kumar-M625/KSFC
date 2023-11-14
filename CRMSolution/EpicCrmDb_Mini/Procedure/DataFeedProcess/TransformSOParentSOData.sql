CREATE PROCEDURE [dbo].[TransformSOParentSOData]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS

BEGIN
	DECLARE @LevelN Table(EmpCode nvarchar(10));
	DECLARE @LevelNMinus1 TABLE(EmpCode nvarchar(10));
	DECLARE @EmpCodeHQ TABLE(EmpCode nvarchar(10),HQCode nvarchar(10));
	DECLARE @levelNCount INT;
	DECLARE @level INT = 0;

	BEGIN 
		-- 1. Get people whom no one is reporting in Temp Table @LevelN
		INSERT INTO @LevelN(EmpCode) (SELECT Distinct(SoEmpCode) FROM SOParentSOInput WHERE SoEmpCode not in (Select DISTINCT(ParentEmpCode) from SOParentSOInput));

		-- Set count
		SET @levelNCount = (SELECT Count(*) FROM @LevelN);  -- 155
	
		-- 2. Get and Insert all direct HQ Assignment to temp table for all employees

		INSERT INTO @EmpCodeHQ (EmpCode, HQCode) (SELECT DISTINCT [Staff Code], [HQ Code] FROM StaffHQInput);

		-- Delete Existing HQ Assignments
		 DELETE FROM SalesPersonAssociation WHERE CodeType = 'HeadQuarter'

	-- LOOP
		WHILE @levelNCount > 0
		BEGIN 


			-- 3. Get Manager for level N Employees got from step 1
				INSERT INTO @LevelNMinus1(EmpCode) 
				SELECT Distinct ParentEmpCode FROM SOParentSOInput
				WHERE SoEmpCode in (select EmpCode from @LevelN)

			-- 5. Get Direct or Indirect HQs for these Managers
			
				INSERT INTO @EmpCodeHQ (EmpCode, HQCode) 
				(SELECT DISTINCT se.ParentEmpCode, s.HQCode
				FROM SOParentSOInput se
				INNER JOIN @EmpCodeHQ s
				ON s.EmpCode = se.SoEmpCode
				WHERE se.ParentEmpCode IN (SELECT EmpCode from @LevelNMinus1))


			-- 6. Delete Temp Table Data

				-- Now delete lowest level employees from ta
				DELETE FROM @LevelN

				-- Now put manager as lowest level
				INSERT INTO @LevelN(EmpCode) (SELECT EmpCode from @LevelNMinus1)

				-- Now delete Manager table data
				DELETE FROM @LevelNMinus1
			
				SET @levelNCount = (SELECT Count(*) FROM @LevelN);
				SET @level = @level + 1;
				--Select @level As [Level];
				--Select @levelNCount;			

		END

		INSERT INTO dbo.[SalesPersonAssociation]
		(
			[StaffCode], [CodeType], [CodeValue], [IsDeleted], [DateCreated], [DateUpdated], [CreatedBy], [UpdatedBy]
		)
		SELECT Distinct
			EmpCode,
			'HeadQuarter',
			HQCode,
			0,
			SYSUTCDATETIME(),
			SYSUTCDATETIME(),
			'TransformSOReportingData',
			'TransformSOReportingData'
		FROM @EmpCodeHQ 

		DELETE FROM @EmpCodeHQ;
	END
END