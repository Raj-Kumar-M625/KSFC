CREATE TABLE [dbo].[GeoLocation]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[EmployeeId] BIGINT NOT NULL REFERENCES TenantEmployee(Id),
	[ClientCode] NVARCHAR(50) NOT NULL DEFAULT '',
    [Latitude] DECIMAL(19, 9) NOT NULL, 
    [Longitude] DECIMAL(19, 9) NOT NULL, 
	[At] [DateTime2] NOT NULL, 
	[IsActive] BIT NOT NULL DEFAULT 1,
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
)
GO

CREATE PROCEDURE [dbo].[AddGeoLocationData]
	@employeeId BIGINT,
	@clientCode NVARCHAR(50),
	@trackingDateTime DateTime2,
	@latitude Decimal(19,9),
	@longitude Decimal(19,9),
	@geoLocationId BIGINT OUTPUT
AS
BEGIN
    SET @geoLocationId = 0

	INSERT INTO dbo.GeoLocation
	(EmployeeId, ClientCode, Latitude, Longitude, [At])
	VALUES
	(@employeeId, @clientCode, @latitude, @longitude, @trackingDateTime)

	SET @geoLocationId = SCOPE_IDENTITY()

	--make all older records for current clientCode, inactive
	
	UPDATE dbo.GeoLocation
	SET IsActive = 0
	WHERE ClientCode = @clientCode AND Id != @geoLocationId

END
GO

CREATE TABLE [dbo].[SOParentSOInput]
(
	[ParentSoCode] [nvarchar](10) NOT NULL, 
	[ParentSoName] [nvarchar](50) NOT NULL, 
	[ParentEmpCode] [nvarchar](10) NOT NULL,
	[SoCode] [nvarchar](10) NOT NULL, 
	[SoName] [nvarchar](50) NOT NULL, 
	[SoEmpCode] [nvarchar](10) NOT NULL
)
GO

-- SP to Process and Assign Employee based on Direct and Indirect reporting
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
GO


--New Upload for Tstanes
Insert into dbo.CodeTable
(tenantId, CodeType, CodeName, CodeValue, IsActive, DisplaySequence)
values
(1, 'ExcelUpload', 'SOParentSOMapping', 'SOParentSOInput', 1, 200)
GO

-- Insert Geotagging Activity Type
Insert into dbo.CodeTable
(tenantId, CodeType, CodeName, CodeValue, IsActive, DisplaySequence)
values
(1, 'ActivityType', '', 'Geo Tagging', 1, 110)
GO

