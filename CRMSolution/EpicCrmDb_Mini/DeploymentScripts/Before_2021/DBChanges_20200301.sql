-- March 01 2020 - PJMargo Contract Farming

insert into dbo.CodeTable
(CodeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
( 'WaterSource', 'Rain', 'Rain', 10, 1, 1),
( 'WaterSource', 'Canal', 'Canal', 20, 1, 1),
( 'WaterSource', 'Borewell', 'Borewell', 30, 1, 1)
go

insert into dbo.CodeTable
(CodeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
( 'SoilType', 'Red Soil', 'Red Soil', 10, 1, 1),
( 'SoilType', 'Black Soil', 'Black Soil', 20, 1, 1),
( 'SoilType', 'Loamy Soil', 'Loamy Soil', 30, 1, 1)
go

insert into dbo.CodeTable
(CodeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
( 'MajorCrop', 'Apple', 'Apple', 10, 1, 1),
( 'MajorCrop', 'Wheat', 'Wheat', 20, 1, 1)
go

insert into dbo.CodeTable
(CodeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
( 'LastCrop', 'Maize', 'Maize', 10, 1, 1),
( 'LastCrop', 'Wheat', 'Wheat', 20, 1, 1)
go

insert into dbo.CodeTable
(CodeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
( 'ExcelUpload', 'EntityNumber', 'EntityNumberInput', 100, 1, 1)
go


ALTER TABLE dbo.Entity
ADD	
   [EntityNumber] NVARCHAR(50) NULL,
    [FatherHusbandName] NVARCHAR(50) NULL,
	[HQName] NVARCHAR(50) NULL,
	[TerritoryCode] NVARCHAR(10) NULL,
	[TerritoryName] NVARCHAR(50) NULL,
	[MajorCrop] NVARCHAR(50) NULL,
	[LastCrop] NVARCHAR(50) NULL,
	[WaterSource] NVARCHAR(50) NULL,
	[SoilType] NVARCHAR(50) NULL,
	[SowingType] NVARCHAR(50) NULL,
	[SowingDate] DATETIME2 NULL
GO

CREATE TABLE [dbo].[EntityNumberInput]
(
	[Sequence] BIGINT NOT NULL,
	[EntityNumber] NVARCHAR(50) NOT NULL
)
go

CREATE TABLE [dbo].[EntityNumber]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[Sequence] BIGINT NOT NULL,
	[EntityNumber] NVARCHAR(50) NOT NULL,
	[IsUsed] BIT NOT NULL DEFAULT 0,
	[UsedTimestamp] DATETIME2 NOT NULL DEFAULT SysUtcDateTime()
)
go

CREATE PROCEDURE [dbo].[TransformEntityNumberData]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
	
		DELETE FROM dbo.[EntityNumber]
		WHERE IsUsed = 0

		-- Step 2: Insert data
		INSERT INTO dbo.[EntityNumber]
		([Sequence], EntityNumber)
		SELECT  
		ani.[Sequence],
		ltrim(rtrim(ani.[EntityNumber]))
		FROM dbo.EntityNumberInput ani
		LEFT JOIN dbo.EntityNumber an on ani.EntityNumber = an.EntityNumber
		WHERE an.EntityNumber is null

		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformEntityNumberData', 'Success'
END
GO

ALTER TABLE dbo.SqliteEntity
ADD	
	[FatherHusbandName] NVARCHAR(50) NOT NULL DEFAULT '',
	[TerritoryCode] NVARCHAR(10) NOT NULL DEFAULT '',
	[TerritoryName] NVARCHAR(50) NOT NULL DEFAULT '',
	[HQCode] NVARCHAR(10) NOT NULL DEFAULT '',
	[HQName] NVARCHAR(50) NOT NULL DEFAULT '',
	[MajorCrop] NVARCHAR(50) NOT NULL DEFAULT '',
	[LastCrop] NVARCHAR(50) NOT NULL DEFAULT '',
	[WaterSource] NVARCHAR(50) NOT NULL DEFAULT '',
	[SoilType] NVARCHAR(50) NOT NULL DEFAULT '',
	[SowingType] NVARCHAR(50) NOT NULL DEFAULT '',
	[SowingDate] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
GO

ALTER PROCEDURE [dbo].[ProcessSqliteEntityData]
	@batchId BIGINT
AS
BEGIN

	-- if batch is already processed - return	
	IF NOT EXISTS(SELECT 1 FROM dbo.SqliteActionBatch 
			WHERE Id = @batchId AND NumberOfEntitiesSaved > 0
			AND BatchProcessed = 0)
	BEGIN
		RETURN;
	END

	-- First create required records in Day Table
	INSERT INTO dbo.[Day]
	([DATE])
	SELECT DISTINCT CAST(e.[TimeStamp] AS [Date])
	FROM dbo.SqliteEntity e
	LEFT JOIN dbo.[Day] d on CAST(e.[TimeStamp] AS [Date]) = d.[DATE]
	WHERE e.BatchId = @batchId
	AND d.[DATE] IS NULL

	-- Identify duplicate Entity records that have already come in some other batch
	--UPDATE se
	--SET EntityId = e.EntityId,
	--IsProcessed = 1,
	--DateUpdated = SYSUTCDATETIME()
	--FROM dbo.SqliteEntity se
	--INNER JOIN dbo.[SqliteEntity] e on se.[TimeStamp] = e.[TimeStamp]
	--AND e.EmployeeId = se.EmployeeId
	--AND e.PhoneDbId = se.PhoneDbId
	--AND se.BatchId = @batchId

	DECLARE @dupRows BIGINT = @@RowCount
	IF @dupRows > 0
	BEGIN
		UPDATE dbo.SqliteActionBatch
		SET DuplicateEntityCount = @dupRows,
		Timestamp = SYSUTCDATETIME()
		WHERE id = @batchId		
	END

	-- select current max entity Id
	DECLARE @lastMaxEntityId BIGINT
	SELECT @lastMaxEntityId = ISNULL(MAX(Id),0) FROM dbo.Entity

	-- We need to assign EntityNumber to each new Entity
	-- March 04 2020
	-- store new Rows id in in-memory table
	DECLARE @sqliteEnt TABLE (ID BIGINT Identity, RowId BIGINT)
	INSERT INTO @sqliteEnt
	(RowId)
	SELECT Id FROM dbo.SqliteEntity
	WHERE BatchId = @batchId
	AND isProcessed = 0
	ORDER BY Id

	-- Count the number of Entities
	DECLARE @entCount BIGINT
	SELECT @entCount = count(*)	FROM @sqliteEnt

	-- Select entity numbers
	DECLARE @entNum TABLE (Id BIGINT Identity, EntityNumber NVARCHAR(50))

	-- take as many entity numbers from EntityNumber table
	-- (may have to enhance to check that we get enough / required entity numbers)
	UPDATE dbo.EntityNumber
	SET ISUsed = 1,
	UsedTimeStamp = SYSUTCDATETIME()
	OUTPUT deleted.EntityNumber INTO @entNum
	FROM dbo.EntityNumber an
	INNER JOIN 
	(
		SELECT TOP(@entCount) Id
		FROM dbo.EntityNumber WITH (READPAST)
		WHERE ISUsed = 0
		ORDER BY [Sequence]
	) an2 on an.Id = an2.Id

	-- Create Entity Records, with running Entity Number filled
	INSERT INTO dbo.[Entity]
	([EmployeeId], [DayId], [HQCode], [AtBusiness], 
	[EntityType], [EntityName], [EntityDate], 
	[Address], [City], [State], [Pincode], [LandSize], 
	[Latitude], [Longitude],
	[UniqueIdType], [UniqueId], [TaxId],

	[FatherHusbandName], [TerritoryCode], [TerritoryName], [HQName], 
	[MajorCrop], [LastCrop], [WaterSource], [SoilType], [SowingType], [SowingDate],

	[SqliteEntityId], [ContactCount], [CropCount], [ImageCount], [EntityNumber])

	SELECT sqe.[EmployeeId], d.[Id] AS [DayId], 
	     case when ltrim(rtrim(ISNULL(sqe.HQCode, ''))) = '' THEN sp.[HQCode] ELSE ltrim(rtrim(sqe.HQCode)) END,
	sqe.[AtBusiness], 
	sqe.[EntityType], sqe.[EntityName], sqe.[TimeStamp] AS [EntityDate], 
	sqe.[Address], sqe.[City], sqe.[State], sqe.[Pincode], sqe.[LandSize], 
	sqe.[DerivedLatitude], sqe.[DerivedLongitude], 
	sqe.[UniqueIdType], sqe.[UniqueId], sqe.[TaxId],

	sqe.[FatherHusbandName], sqe.TerritoryCode, sqe.TerritoryName, sqe.HQName,
	sqe.MajorCrop, sqe.LastCrop, sqe.WaterSource, sqe.SoilType, sqe.SowingType, sqe.SowingDate,

	sqe.[Id], sqe.[ContactCount], sqe.[CropCount], sqe.[ImageCount], ent.EntityNumber

	FROM dbo.SqliteEntity sqe
	INNER JOIN dbo.[Day] d ON d.[DATE] = CAST(sqe.[TimeStamp] AS [Date])
	INNER JOIN dbo.TenantEmployee te on te.Id = sqe.EmployeeId
	INNER JOIN dbo.SalesPerson sp on te.EmployeeCode = sp.StaffCode

	INNER JOIN @sqliteEnt snt ON sqe.Id = snt.RowId
	INNER JOIN @entNum ent ON ent.Id = snt.ID

	WHERE sqe.BatchId = @batchId AND sqe.IsProcessed = 0
	ORDER BY sqe.Id

	-- Now Images
	UPDATE dbo.[Image] SET SourceId = 0 WHERE SourceId > 0

	INSERT INTO dbo.[Image]
	(SourceId, [ImageFileName])  
	SELECT sei.Id, sei.[ImageFileName]
	FROM dbo.SqliteEntityImage sei
	INNER JOIN dbo.SqliteEntity se on sei.SqliteEntityId = se.Id
	AND se.BatchId = @batchId
	AND se.IsProcessed = 0
		
	-- Now create entries in EntityImage
	INSERT INTO dbo.EntityImage
	(EntityId, ImageId, SequenceNumber)
	SELECT e.Id, i.[Id], sei.SequenceNumber
	FROM dbo.SqliteEntityImage sei
	INNER JOIN dbo.[Image] i on sei.Id = i.SourceId
	INNER JOIN dbo.SqliteEntity sle on sei.SqliteEntityId = sle.Id
	AND sle.BatchId = @batchId
	INNER JOIN dbo.[Entity] e on sle.Id = e.SqliteEntityId

	-- Prepare for SMS
	DECLARE @NewProfile TABLE
	( EntityId BIGINT,
	  EntityName NVARCHAR(50),
	  PhoneNumber NVARCHAR(20)
	)

	-- now we need to update the id in SqliteEntity table
	UPDATE dbo.SqliteEntity
	SET EntityId = e.Id,
	IsProcessed = 1,
	DateUpdated = SYSUTCDATETIME()
	OUTPUT inserted.EntityId, '', '' INTO @NewProfile
	FROM dbo.SqliteEntity se
	INNER JOIN dbo.[Entity] e on se.Id = e.SqliteEntityId
	AND se.BatchId = @batchId
	AND e.Id > @lastMaxEntityId

	--Create Entity Contacts
	INSERT INTO dbo.[EntityContact]
	([EntityId], [Name], [PhoneNumber], [IsPrimary])
	SELECT se.[EntityId], sqecn.[Name], sqecn.[PhoneNumber], sqecn.[IsPrimary]
	FROM dbo.SqliteEntityContact sqecn
	INNER JOIN dbo.SqliteEntity se on se.Id = sqecn.SqliteEntityId
	AND se.BatchId = @batchId
	AND se.EntityId > @lastMaxEntityId

	--Create Entity Crops
	INSERT INTO dbo.[EntityCrop]
	([EntityId], [CropName])
	SELECT se.[EntityId], sqecr.[Name] AS [CropName]
	FROM dbo.SqliteEntityCrop sqecr
	INNER JOIN dbo.SqliteEntity se on se.Id = sqecr.SqliteEntityId
	AND se.BatchId = @batchId
	AND se.EntityId > @lastMaxEntityId

	-- retrieve tenant Id for batch
	DECLARE @tenantId BIGINT
	SELECT @tenantId = TenantId
	FROM dbo.SqliteActionBatch b
	INNER JOIN dbo.TenantEmployee te on b.EmployeeId = te.Id
	WHERE b.Id = @batchId

	-- PUT Name and phone number in new records where sms is to be sent
	UPDATE @NewProfile
	SET EntityName = ec.Name,
	PhoneNumber = ec.PhoneNumber
	FROM @NewProfile np
	INNER JOIN dbo.EntityContact ec on np.EntityId = ec.EntityId
	and ec.IsPrimary = 1

	-- Insert into Table for SMS
	INSERT INTO dbo.TenantSmsData
	(TenantId, TemplateName, DataType, MessageData)
	SELECT @tenantId, 'EntityProfile', 'XML', 
	(SELECT * FROM @NewProfile i WHERE i.EntityId = o.EntityId FOR XML PATH('Row'))
	FROM @NewProfile o
END
GO


insert into dbo.CodeTable
(CodeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
( 'XamlEntityAddPage', 'Farmer', 'AddFarmerPage', 10, 1, 1),
( 'XamlEntityAddPage', 'Retailer', 'AddGenericEntityPage', 20, 1, 1),
( 'XamlEntityAddPage', 'Dealer', 'AddGenericEntityPage', 30, 1, 1),
( 'XamlEntityAddPage', 'Other', 'AddGenericEntityPage', 30, 1, 1)
go

insert into dbo.CodeTable
(CodeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
( 'SowingType', '', 'Seeds',  10, 1, 1),
( 'SowingType', '', 'Seedlings', 20, 1, 1)
go


ALTER PROCEDURE [dbo].[ClearEmployeeData]
	@employeeId BIGINT
AS
BEGIN
	BEGIN TRY

		BEGIN TRANSACTION

		-- Delete Activity Data
		DECLARE @activity Table ( Id BIGINT )
		DECLARE @image TABLE (Id BIGINT)

		-- first find out the activity ids that need to be deleted
		INSERT INTO @activity (Id)
		SELECT a.id from dbo.Activity a
		INNER JOIN dbo.EmployeeDay ed on ed.Id = a.employeeDayId
		and ed.TenantEmployeeId = @employeeId

		-- delete activity images
		-- store image Ids first - delete images at the end, as we need to delete images for payment as well;
		INSERT INTO @image (Id)
		SELECT ImageId FROM dbo.ActivityImage ai 
					 INNER JOIN @activity a on ai.ActivityId = a.Id

		print 'ActivityImage'
		DELETE FROM dbo.ActivityImage
		WHERE ActivityId IN (SELECT id FROM @activity)

		print 'ActivityContact'
		DELETE FROM dbo.ActivityContact WHERE ActivityId in (SELECT ID FROM @activity)


		print 'SqliteEntityWorkFlow'
		-- this table is no longer used;
		DELETE FROM dbo.SqliteEntityWorkFlow WHERE  ActivityId in (SELECT ID FROM @activity)

		print 'SqliteEntityWorkFlowV2'
		DELETE FROM dbo.SqliteEntityWorkFlowV2 WHERE EmployeeId = @employeeId


		print 'Activity'
		DELETE from dbo.Activity WHERE id in (SELECT Id from @activity)

		-----------------
		print 'dbo.DistanceCalcErrorLog'
		DELETE from dbo.DistanceCalcErrorLog
		WHERE id in
		(SELECT l.id from dbo.distanceCalcErrorLog l
		INNER JOIN dbo.Tracking t on l.TrackingId = t.Id
		INNER JOIN dbo.EmployeeDay ed on ed.Id = t.employeeDayId
		and ed.TenantEmployeeId = @employeeId)

		print 'dbo.Tracking'
		DELETE from dbo.Tracking
		WHERE id in
		(SELECT t.id from dbo.Tracking t
		INNER JOIN dbo.EmployeeDay ed on ed.Id = t.employeeDayId
		and ed.TenantEmployeeId = @employeeId)

		print 'employeeDay'
		DELETE from dbo.employeeDay WHERE TenantEmployeeId = @employeeId

		print 'imei'
		DELETE from dbo.Imei WHERE TenantEmployeeId = @employeeId

		--DELETE FROM dbo.SqliteAction WHERE EmployeeId = @employeeId
		--DELETE FROM dbo.SqliteActionBatch WHERE EmployeeId = @employeeId

		-- delete expense Data as well
		print 'SqliteExpenseImage'
		DELETE FROM dbo.SqliteExpenseImage WHERE SqliteExpenseId in (SELECT Id FROM dbo.SqliteExpense WHERE EmployeeId = @employeeId)
		print 'SqliteExpense'
		DELETE FROM dbo.SqliteExpense WHERE EmployeeId = @employeeId

		-- Delete SqliteOrder data
		print 'SqliteOrderItem'
		DELETE FROM dbo.SqliteOrderItem WHERE SqliteOrderId IN (SELECT Id FROM dbo.[SqliteOrder] WHERE EmployeeId = @employeeId)
		print 'SqliteOrderImage'
		DELETE FROM dbo.SqliteOrderImage WHERE SqliteOrderId IN (SELECT Id FROM dbo.[SqliteOrder] WHERE EmployeeId = @employeeId)
		print '[SqliteOrder]'
		DELETE FROM dbo.[SqliteOrder] WHERE EmployeeId = @employeeId

		-- delete SqlLiteAction Data as well
		print 'SqliteActionImage'
		DELETE FROM dbo.SqliteActionImage where SqliteActionId in (SELECT id FROM dbo.SqliteAction WHERE EmployeeId = @employeeId)
		print 'SqliteActionContact'
		DELETE FROM dbo.SqliteActionContact WHERE SqliteActionId in (SELECT id FROM dbo.SqliteAction WHERE EmployeeId = @employeeId)
		print 'SqliteActionLocation'
		DELETE FROM dbo.SqliteActionLocation WHERE SqliteActionId in (SELECT id FROM dbo.SqliteAction WHERE EmployeeId = @employeeId)
		print 'SqliteAction'
		DELETE FROM dbo.SqliteAction WHERE EmployeeId = @employeeId
		print 'SqliteActionDup'
		DELETE FROM dbo.SqliteActionDup WHERE EmployeeId = @employeeId

		-- delete SqlitePayment data as well
		print 'SqlitePaymentImage'
		DELETE FROM dbo.SqlitePaymentImage WHERE SqlitePaymentId in (SELECT Id FROM dbo.SqlitePayment WHERE EmployeeId = @employeeId)
		print 'SqlitePayment'
		DELETE FROM dbo.SqlitePayment WHERE EmployeeId = @employeeId

		-- Delete SqliteReturnOrder data
		print 'SqliteReturnOrderItem'
		DELETE FROM dbo.SqliteReturnOrderItem WHERE SqliteReturnOrderId IN (SELECT Id FROM dbo.[SqliteReturnOrder] WHERE EmployeeId = @employeeId)
		print '[SqliteReturnOrder]'
		DELETE FROM dbo.[SqliteReturnOrder] WHERE EmployeeId = @employeeId

		-- Delete SqliteEntity data
		DECLARE @SqliteEntity TABLE (Id BIGINT)
		INSERT INTO @SqliteEntity SELECT Id FROM dbo.[SqliteEntity] WHERE EmployeeId = @employeeId
		print '[SqliteEntityContact]'
		DELETE FROM dbo.[SqliteEntityContact] WHERE [SqliteEntityId] IN (SELECT Id FROM @SqliteEntity)
		print '[SqliteEntityCrop]'
		DELETE FROM dbo.[SqliteEntityCrop] WHERE [SqliteEntityId] IN (SELECT Id FROM @SqliteEntity)
		print '[SqliteEntityImage]'
		DELETE FROM dbo.[SqliteEntityImage] WHERE [SqliteEntityId] IN (SELECT Id FROM @SqliteEntity)
		print '[SqliteEntityLocation]'
		DELETE FROM dbo.[SqliteEntityLocation] WHERE [SqliteEntityId] IN (SELECT Id FROM @SqliteEntity)
		print '[SqliteEntity]'
		DELETE FROM dbo.[SqliteEntity] WHERE EmployeeId = @employeeId

		-- Delete SqliteLeave data
		print '[SqliteLeave]'
		DELETE FROM dbo.[SqliteLeave] WHERE EmployeeId = @employeeId

		-- Delete SqliteCancelledLeave data
		print '[SqliteCancelledLeave]'
		DELETE FROM dbo.[SqliteCancelledLeave] WHERE EmployeeId = @employeeId
		
		-- 
		print '[SqliteIssueReturn]'
		DELETE FROM dbo.[SqliteIssueReturn] WHERE EmployeeId = @employeeId

		-- Dec 8 2019 
		print '[SqliteAgreement]'
		DELETE FROM dbo.[SqliteAgreement] WHERE EmployeeId = @employeeId

		print '[SqliteAdvanceRequest]'
		DELETE FROM dbo.[SqliteAdvanceRequest] WHERE EmployeeId = @employeeId

		print '[SqliteTerminateAgreement]'
		DELETE FROM dbo.[SqliteTerminateAgreement] WHERE EmployeeId = @employeeId

		-- End Dec 8 2019

		-- Delete Device Log
		print '[SqliteDeviceLog]'
		DELETE FROM dbo.[SqliteDeviceLog] WHERE EmployeeId = @employeeId
		print 'SqliteActionBatch'
		DELETE FROM dbo.SqliteActionBatch WHERE EmployeeId = @employeeId

		-- store image ids first for processed expense data
		INSERT INTO @image (id)
		SELECT ImageId 
		FROM dbo.ExpenseItemImage WHERE ExpenseItemId IN 
		(SELECT ei.id FROM dbo.ExpenseItem ei
		INNER JOIN dbo.Expense e on ei.ExpenseId = e.Id
		AND e.EmployeeId = @employeeId)

		print 'ExpenseItemImage'
		DELETE FROM dbo.ExpenseItemImage WHERE ExpenseItemId IN 
		(SELECT ei.id FROM dbo.ExpenseItem ei INNER JOIN dbo.Expense e on ei.ExpenseId = e.Id AND e.EmployeeId = @employeeId)

		print 'ExpenseItem'
		DELETE FROM dbo.ExpenseItem WHERE ExpenseId in (SELECT Id FROM dbo.Expense WHERE EmployeeId = @employeeId)
		print 'ExpenseApproval'
		DELETE FROM dbo.ExpenseApproval WHERE ExpenseId in (SELECT Id FROM dbo.Expense WHERE EmployeeId = @employeeId)
		print 'Expense'
		DELETE FROM dbo.Expense WHERE EmployeeId = @employeeId

		-- Delete order Data
		print 'OrderItem'
		DELETE FROM dbo.OrderItem WHERE OrderId IN (SELECT Id FROM dbo.[Order] WHERE EmployeeId = @employeeId)

		INSERT INTO @image (id)
		SELECT oim.ImageId
		FROM dbo.OrderImage oim
		INNER JOIN dbo.[Order] o on o.Id = oim.OrderId
		AND o.EmployeeId = @employeeId

		print 'OrderImage'
		DELETE FROM dbo.OrderImage WHERE OrderId IN (SELECT Id FROM dbo.[Order] WHERE EmployeeId = @employeeId)

		print '[Order]'
		DELETE FROM dbo.[Order] WHERE EmployeeId = @employeeId


		-- Delete Return Order Data
		print 'ReturnOrderItem'
		DELETE FROM dbo.ReturnOrderItem WHERE ReturnOrderId IN (SELECT Id FROM dbo.[ReturnOrder] WHERE EmployeeId = @employeeId)
		print '[ReturnOrder]'
		DELETE FROM dbo.[ReturnOrder] WHERE EmployeeId = @employeeId

		-- DELETE Workflow data
		print 'EntityWorkFlowDetail'
		DELETE FROM dbo.EntityWorkFlowDetail
		WHERE ID IN (SELECT d.ID FROM dbo.EntityWorkFlowDetail d
		             INNER JOIN dbo.EntityWorkFlow p on d.EntityWorkFlowId = p.Id
					 AND p.EmployeeId = @employeeId)

		print 'EntityWorkFlow'
		DELETE FROM dbo.EntityWorkFlow WHERE EmployeeId = @employeeId

		print 'Issue/Return'
		DELETE FROM dbo.[IssueReturn] WHERE EmployeeId = @employeeId

		-- Dec 8 2019
		print 'AdvanceRequest'
		DELETE FROM dbo.[AdvanceRequest] WHERE EmployeeId = @employeeId

		print 'TerminateAgreementRequest'
		DELETE FROM dbo.[TerminateAgreementRequest] WHERE EmployeeId = @employeeId

		-- End Dec 8 2019

		-- Delete Entity data
		DECLARE @Entity TABLE (Id BIGINT)
		INSERT INTO @Entity SELECT Id FROM dbo.[Entity] WHERE EmployeeId = @employeeId

		INSERT INTO @image (id)
		SELECT oim.ImageId
		FROM dbo.EntityImage oim
		INNER JOIN dbo.[Entity] o on o.Id = oim.EntityId
		AND o.EmployeeId = @employeeId

		print '[EntityContact]'
		DELETE FROM dbo.[EntityContact] WHERE [EntityId] IN (SELECT Id FROM @Entity)
		print '[EntityCrop]'
		DELETE FROM dbo.[EntityCrop] WHERE [EntityId] IN (SELECT Id FROM @Entity)
		print 'EntityImage'
		DELETE FROM dbo.EntityImage WHERE EntityId IN (SELECT Id FROM @Entity)
		print 'EntityAgreement'
		-- clear foreign key reference first
		UPDATE dbo.[IssueReturn] SET EntityAgreementId = NULL
		WHERE EntityAgreementId IN 
		(
		   SELECT ID FROM dbo.EntityAgreement WHERE EntityId IN (SELECT Id FROM @Entity)
		)

		-- Dec 8 2019
		-- delete data from TerminateAgreementRequest for same agreement that is being deleted
		DELETE FROM dbo.TerminateAgreementRequest
		WHERE EntityAgreementId in
		(
			SELECT ID FROM dbo.EntityAgreement WHERE EntityId IN (SELECT Id FROM @Entity)
		)

		-- delete data from AdvanceRequest for same agreement that is being deleted
		DELETE FROM dbo.AdvanceRequest
		WHERE EntityAgreementId in
		(
			SELECT ID FROM dbo.EntityAgreement WHERE EntityId IN (SELECT Id FROM @Entity)
		)


		DELETE FROM dbo.EntityAgreement WHERE EntityId IN (SELECT Id FROM @Entity)

		-- User 1 has created an entity
		-- User 2 has created a workflow based on this entity
		-- Question: Should we delete the workflow created by user 2 on this entity
		-- Answer: Basic use of this script is to delete the data that is created by test users
		--         during testing on live site.  So the answer is yes.


		print 'EntityWorkFlowDetail - again'
		DELETE FROM dbo.EntityWorkFlowDetail
		WHERE ID IN (SELECT d.ID FROM dbo.EntityWorkFlowDetail d
		             INNER JOIN dbo.EntityWorkFlow p on d.EntityWorkFlowId = p.Id
					 AND p.EntityId in (SELECT Id FROM @Entity))

		print 'EntityWorkFlow - again'
		DELETE FROM dbo.EntityWorkFlow WHERE EntityId in (SELECT Id FROM @Entity)

		print 'IssueReturn - again'
		DELETE FROM dbo.[IssueReturn] WHERE EntityId in (SELECT Id FROM @Entity)

		print '[Entity]'
		DELETE FROM dbo.[Entity] WHERE ID in (SELECT Id from @Entity)

		--
		-- Delete Payment Data
		--
		INSERT INTO @image (id)
		SELECT pim.ImageId
		FROM dbo.PaymentImage pim
		INNER JOIN dbo.Payment p on p.Id = pim.PaymentId
		AND p.EmployeeId = @employeeId

		print 'PaymentImage'
		DELETE FROM dbo.PaymentImage WHERE PaymentId IN (SELECT Id FROM dbo.[Payment] WHERE EmployeeId = @employeeId)
		print '[Payment]'
		DELETE FROM dbo.[Payment] WHERE EmployeeId = @employeeId

		print 'EntityImage - again'
		DELETE FROM dbo.EntityImage WHERE ImageId IN (SELECT Id FROM @image)

		-- DELETE IMAGES
		print 'Image'
		DELETE FROM dbo.[Image]
		WHERE Id in (SELECT Id FROM @image)

		-- CLEAR DEVICE LOG
		print 'SqliteDeviceLog'
		DELETE FROM dbo.SqliteDeviceLog WHERE EmployeeId = @employeeId

		print 'TenantEmployee'
		DELETE from dbo.TenantEmployee WHERE id = @employeeId
		COMMIT
	END TRY

	BEGIN CATCH
		PRINT 'Inside Catch...'
		PRINT ERROR_MESSAGE()
		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:ClearEmployeeData', ERROR_MESSAGE()

		ROLLBACK TRANSACTION
		throw;

	END CATCH
END	
GO
