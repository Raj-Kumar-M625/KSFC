ALTER TABLE [dbo].[SqliteAction]
ADD 
	[ClientCode] NVARCHAR(50) NOT NULL DEFAULT '',
	[IMEI] NVARCHAR(50) NOT NULL DEFAULT '',
	[ContactCount] INT NOT NULL DEFAULT 0,
	[AtBusiness] BIT NOT NULL DEFAULT 0,
	[InstrumentId] NVARCHAR(50) NOT NULL DEFAULT '',
	[ActivityAmount] DECIMAL(19,2) NOT NULL DEFAULT 0.0,
	[LocationTaskStatus] NVARCHAR(50) NULL, 
    [LocationException] NVARCHAR(256) NULL;
GO

CREATE TABLE [dbo].[SqliteActionContact]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [SqliteActionId] BIGINT NOT NULL REFERENCES [SqliteAction]([Id]), 
    [Name] NVARCHAR(50) NOT NULL, 
    [PhoneNumber] NVARCHAR(20) NOT NULL, 
    [IsPrimary] BIT NOT NULL
)
GO

ALTER TABLE [dbo].[SqliteEntity]
ALTER COLUMN [LocationException] NVARCHAR(256) NULL;
GO

ALTER TABLE [dbo].[SqliteActionDup]
ADD 
	[ClientCode] NVARCHAR(50) NOT NULL DEFAULT '',
	[ContactCount] INT NOT NULL DEFAULT 0,
	[AtBusiness] BIT NOT NULL DEFAULT 0,
	[ActivityAmount] DECIMAL(19,2) NOT NULL DEFAULT 0.0;
GO

ALTER TABLE [dbo].[Activity]
ADD [ClientCode] NVARCHAR(50) NOT NULL DEFAULT '',
	[AtBusiness] BIT NOT NULL DEFAULT 0,
	[ActivityAmount] DECIMAL(19,2) NOT NULL DEFAULT 0.0,
	[ContactCount] INT NOT NULL DEFAULT 0;
GO

CREATE TABLE [dbo].[ActivityContact]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [ActivityId] BIGINT NOT NULL REFERENCES [Activity]([Id]), 
    [Name] NVARCHAR(50) NOT NULL, 
    [PhoneNumber] NVARCHAR(20) NOT NULL, 
    [IsPrimary] BIT NOT NULL
);
GO

IF OBJECT_ID ( '[dbo].[AddActivityData]', 'P' ) IS NOT NULL   
    DROP PROCEDURE [dbo].[AddActivityData];  
GO 

CREATE PROCEDURE [dbo].[AddActivityData]
	@employeeDayId BIGINT,
	@activityDateTime DateTime2,
	@clientName NVARCHAR(50),
	@clientPhone NVARCHAR(20),
	@clientType NVARCHAR(50),
	@activityType NVARCHAR(50),
	@comments NVARCHAR(2048),
	@clientCode NVARCHAR(50),
	@activityAmount DECIMAL(19,2),
	@atBusiness BIT,
	@imageCount INT,
	@contactCount INT,
	@activityId BIGINT OUTPUT
AS
BEGIN
   -- Records tracking activity
   SET @activityId = 0
	-- check if a record already exist in Employee Day table
	IF EXISTS(SELECT 1 FROM dbo.EmployeeDay WHERE Id = @employeeDayId)
	BEGIN
		INSERT into dbo.Activity
		(EmployeeDayId, ClientName, ClientPhone, ClientType, ActivityType, Comments, ImageCount, [At], ClientCode, ActivityAmount, AtBusiness, ContactCount)
		VALUES
		(@employeeDayId, @clientName, @clientPhone, @clientType, @activityType, @comments, @imageCount, @activityDateTime, @clientCode, @activityAmount, @atBusiness, @contactCount)

		SET @activityId = SCOPE_IDENTITY()
	END
END
GO
