-- March 18 2020 - PJMargo Contract Farming

ALTER TABLE [dbo].[SqliteActionBatch]
ADD	[BankDetails] BIGINT NOT NULL DEFAULT 0,
	[BankDetailsSaved] BIGINT NOT NULL DEFAULT 0
GO

CREATE TABLE [dbo].[SqliteBankDetail]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[BatchId] BIGINT NOT NULL REFERENCES dbo.SqliteActionBatch,
	[EmployeeId] BIGINT NOT NULL,

	[IsNewEntity] BIT NOT NULL,
	[EntityId] BIGINT NOT NULL,
	[EntityName] NVARCHAR(50) NOT NULL,

	[IsPrimaryAccount] BIT NOT NULL,
	[IsSelfAccount] BIT NOT NULL,
	[AccountHolderName] NVARCHAR(50) NOT NULL,
	[AccountHolderPAN] NVARCHAR(50) NOT NULL,
	[BankName] NVARCHAR(50) NOT NULL,
	[BankAccount] NVARCHAR(50) NOT NULL,
	[BankIFSC] NVARCHAR(50) NOT NULL,
	[BankBranch] NVARCHAR(50) NOT NULL,

	[BankDetailDate] DATETIME2 NOT NULL,
	[ImageCount] INT NOT NULL DEFAULT 0,

	[ActivityId] NVARCHAR(50) NOT NULL DEFAULT '',
	[PhoneDbId] NVARCHAR(50) NOT NULL, 
	[ParentReferenceId]  NVARCHAR(50) NOT NULL DEFAULT '',

	[IsProcessed] BIT NOT NULL DEFAULT 0,  
	[EntityBankDetailId] BIGINT NOT NULL DEFAULT 0,
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
)
GO

CREATE TABLE [dbo].[SqliteBankDetailImage]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[SqliteBankDetailId] BIGINT NOT NULL REFERENCES SqliteBankDetail(Id),
	[SequenceNumber] INT NOT NULL DEFAULT 0,
	[ImageFileName] VARCHAR(100) NOT NULL DEFAULT 'FileNotFound.jpg'
)
GO

ALTER PROCEDURE [dbo].[PurgeStagingData]
	@nukeFromDate DateTime,
	@nukeUptoDate DateTime
AS
BEGIN
	DECLARE @BatchIds Table ( BatchId BIGINT )
	DECLARE @ActionPurged BIGINT = 0
	DECLARE @ActionDupPurged BIGINT = 0
	DECLARE @ExpensePurged BIGINT = 0
	DECLARE @OrderPurged BIGINT = 0
	DECLARE @PaymentPurged BIGINT = 0
	DECLARE @ReturnPurged BIGINT = 0
	DECLARE @EntityPurged BIGINT = 0
	DECLARE @EntityBankDetailPurged BIGINT = 0

	INSERT INTO @BatchIds
	(BatchId)
	SELECT ID FROM dbo.SqliteActionbatch
	WHERE BatchProcessed = 1 and LockTimestamp is null and UnderConstruction = 0
	and at >= @nukeFromDate and at <= @nukeUptoDate

	-- DELETE Action Image
	DELETE FROM dbo.SqliteActionImage
	WHERE SqliteActionId In 
	 ( SELECT id FROM dbo.SqliteAction a
	   JOIN @BatchIds b on b.BatchId = a.BatchId
	 )

    -- DELETE SqliteActionContact
	DELETE FROM dbo.SqliteActionContact
	WHERE SqliteActionId In 
	 ( SELECT id FROM dbo.SqliteAction a
	   JOIN @BatchIds b on b.BatchId = a.BatchId
	 )

	-- DELETE SqliteActionLocation
	DELETE FROM dbo.SqliteActionLocation
	WHERE SqliteActionId In 
	 ( SELECT id FROM dbo.SqliteAction a
	   JOIN @BatchIds b on b.BatchId = a.BatchId
	 )

	-- DELETE Action
	DELETE FROM dbo.SqliteAction 
	WHERE BatchId in (SELECT BatchId FROM @BatchIds)
	SET @ActionPurged = @@ROWCOUNT


	-- DELETE Action Dup
	DELETE FROM dbo.SqliteActionDup
	WHERE BatchId in (SELECT BatchId FROM @BatchIds)
	SET @ActionDupPurged = @@ROWCOUNT

	-- DELETE Expense Image
	DELETE FROM dbo.SqliteExpenseImage
	WHERE SqliteExpenseId In 
	 ( SELECT id FROM dbo.SqliteExpense a
	   JOIN @BatchIds b on b.BatchId = a.BatchId
	 )

	-- DELETE Expense
	DELETE FROM dbo.SqliteExpense
	WHERE BatchId in (SELECT BatchId FROM @BatchIds)
	SET @ExpensePurged = @@ROWCOUNT

	-- DELETE Payment Image
	DELETE FROM dbo.SqlitePaymentImage
	WHERE SqlitePaymentId In 
	 ( SELECT id FROM dbo.SqlitePayment a
	   JOIN @BatchIds b on b.BatchId = a.BatchId
	 )

	-- DELETE Payment
	DELETE FROM dbo.SqlitePayment
	WHERE BatchId in (SELECT BatchId FROM @BatchIds)
	SET @PaymentPurged = @@ROWCOUNT


	-- DELETE OrderItem
	DELETE FROM dbo.SqliteOrderItem
	WHERE SqliteOrderId In 
	 ( SELECT id FROM dbo.SqliteOrder a
	   JOIN @BatchIds b on b.BatchId = a.BatchId
	 )

	-- DELETE Order
	DELETE FROM dbo.SqliteOrder
	WHERE BatchId in (SELECT BatchId FROM @BatchIds)
	SET @OrderPurged = @@ROWCOUNT


	-- Delete ReturnOrderItem
	DELETE FROM dbo.SqliteReturnOrderItem
	WHERE SqliteReturnOrderId In 
	 ( SELECT id FROM dbo.SqliteReturnOrder a
	   JOIN @BatchIds b on b.BatchId = a.BatchId
	 )

	-- DELETE Return
	DELETE FROM dbo.SqliteReturnOrder
	WHERE BatchId in (SELECT BatchId FROM @BatchIds)
	SET @ReturnPurged = @@ROWCOUNT

	-- DELETE Entity Image

	DELETE FROM dbo.SqliteEntityImage
	WHERE SqliteEntityId In 
	 ( SELECT id FROM dbo.SqliteEntity a
	   JOIN @BatchIds b on b.BatchId = a.BatchId
	 )

    -- DELETE SqliteEntityContact
	DELETE FROM dbo.SqliteEntityContact
	WHERE SqliteEntityId In 
	 ( SELECT id FROM dbo.SqliteEntity a
	   JOIN @BatchIds b on b.BatchId = a.BatchId
	 )

	-- DELETE SqliteEntityCrop
	DELETE FROM dbo.SqliteEntityCrop
	WHERE SqliteEntityId In 
	 ( SELECT id FROM dbo.SqliteEntity a
	   JOIN @BatchIds b on b.BatchId = a.BatchId
	 )

	-- DELETE SqliteEntity
	DELETE FROM dbo.SqliteEntity
	WHERE BatchId in (SELECT BatchId FROM @BatchIds)
	SET @EntityPurged = @@ROWCOUNT

	-- March 18 2020
	-- DELETE SqliteBankDetailImage
	DELETE FROM dbo.SqliteBankDetailImage
	WHERE SqliteBankDetailId In 
	 ( SELECT id FROM dbo.SqliteBankDetail a
	   JOIN @BatchIds b on b.BatchId = a.BatchId
	 )

	-- DELETE SqliteBankDetail
	DELETE FROM dbo.SqliteBankDetail
	WHERE BatchId in (SELECT BatchId FROM @BatchIds)
	SET @EntityBankDetailPurged = @@ROWCOUNT


	INSERT INTO dbo.PurgeLog
	(DateFrom, DateTo, ActionPurged, ActionDupPurged, ExpensePurged, OrderPurged, PaymentPurged, ReturnPurged, EntityPurged)
	VALUES
	(
		@nukeFromDate, 
		@nukeUptoDate, 
		@ActionPurged,
		@ActionDupPurged,
		@ExpensePurged,
		@OrderPurged,
		@PaymentPurged,
		@ReturnPurged,
		@EntityPurged
	)
END
GO

CREATE TABLE [dbo].[EntityBankDetail]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[EntityId] BIGINT NOT NULL REFERENCES [Entity]([Id]), 

	[IsPrimaryAccount] BIT NOT NULL,
	[IsSelfAccount] BIT NOT NULL,
	[AccountHolderName] NVARCHAR(50) NOT NULL,
	[AccountHolderPAN] NVARCHAR(50) NOT NULL,
	[BankName] NVARCHAR(50) NOT NULL,
	[BankAccount] NVARCHAR(50) NOT NULL,
	[BankIFSC] NVARCHAR(50) NOT NULL,
	[BankBranch] NVARCHAR(50) NOT NULL,
	[ImageCount] INT NOT NULL DEFAULT 0,

	[SqliteBankDetailId] BIGINT NOT NULL,
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[CreatedBy] NVARCHAR(50) NOT NULL,
	[UpdatedBy] NVARCHAR(50) NOT NULL,
	IsActive BIT NOT NULL DEFAULT 1
)
GO

CREATE TABLE [dbo].[EntityBankDetailImage](
	[Id] [bigint] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[EntityBankDetailId] [bigint] NOT NULL REFERENCES [dbo].[EntityBankDetail](Id),
	[SequenceNumber] INT NOT NULL DEFAULT 0,
	[ImageFileName] VARCHAR(100) NOT NULL DEFAULT 'FileNotFound.jpg'
)
GO

CREATE PROCEDURE [dbo].[ProcessSqliteBankDetailsData]
	@batchId BIGINT
AS
BEGIN

	-- if batch is already processed - return	
	IF NOT EXISTS(SELECT 1 FROM dbo.SqliteActionBatch 
			WHERE Id = @batchId AND BankDetailsSaved > 0
			AND BatchProcessed = 0)
	BEGIN
		RETURN;
	END

	-- First create required records in Day Table
	INSERT INTO dbo.[Day]
	([DATE])
	SELECT DISTINCT CAST(e.[BankDetailDate] AS [Date])
	FROM dbo.SqliteBankDetail e
	LEFT JOIN dbo.[Day] d on CAST(e.[BankDetailDate] AS [Date]) = d.[DATE]
	WHERE e.BatchId = @batchId
	AND d.[DATE] IS NULL

	-- Fill Entity Id in SqliteBankDetail that belong to new entity created on phone.
	Update dbo.SqliteBankDetail
	SET EntityId = en.EntityId,
	EntityName = en.EntityName,
	DateUpdated = SYSUTCDATETIME()
	FROM dbo.SqliteBankDetail ag
	INNER JOIN dbo.SqliteEntity en on ag.ParentReferenceId = en.PhoneDbId
	AND ag.BatchId = @batchId
	AND ag.IsNewEntity = 1
	AND ag.IsProcessed = 0


	DECLARE @insertTable TABLE (EntityBankDetailId BIGINT, SqliteBankDetailId BIGINT)

	-- Insert rows in dbo.EntityAgreement
	INSERT into dbo.EntityBankDetail
	(EntityId, IsPrimaryAccount, IsSelfAccount, AccountHolderName, AccountHolderPAN,
	BankName, BankAccount, BankIFSC, BankBranch, ImageCount,
	CreatedBy, UpdatedBy, SqliteBankDetailId)
	OUTPUT inserted.Id, inserted.SqliteBankDetailId INTO @insertTable
	SELECT ag.EntityId, ag.IsPrimaryAccount, ag.IsSelfAccount, ag.AccountHolderName, ag.AccountHolderPAN,
	ag.BankName, ag.BankAccount, ag.BankIFSC, ag.BankBranch, ag.ImageCount,
	'ProcessSqliteBankDetailsData', 'ProcessSqliteBankDetailsData', ag.Id
	FROM dbo.SqliteBankDetail ag
	WHERE BatchId = @batchId
	AND IsProcessed = 0

	-- Now update EntityBankDetailId back in SqliteBankDetail
	Update dbo.SqliteBankDetail
	SET EntityBankDetailId = m3.EntityBankDetailId,
	IsProcessed = 1,
	DateUpdated = SYSUTCDATETIME()
	FROM dbo.SqliteBankDetail sagg
	INNER JOIN @insertTable m3 on m3.SqliteBankDetailId = sagg.Id

	-- Now Images
		
	-- Now create entries in EntityBankDetailImage
	INSERT INTO dbo.EntityBankDetailImage
	(EntityBankDetailId, SequenceNumber, ImageFileName)
	SELECT sbd.EntityBankDetailId, sbdi.SequenceNumber, sbdi.ImageFileName
	FROM dbo.SqliteBankDetailImage sbdi
	INNER JOIN dbo.SqliteBankDetail sbd ON sbdi.SqliteBankDetailId = sbd.Id
	AND sbd.BatchId = @batchId
END
GO

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

		-- March 18 2020
		print '[SqliteBankDetailImage]'
		DELETE FROM dbo.SqliteBankDetailImage WHERE SqliteBankDetailId in
		( SELECT id FROM dbo.SqliteBankDetail WHERE EmployeeId = @employeeId )

		print '[SqliteBankDetail]'
		DELETE FROM dbo.[SqliteBankDetail] WHERE EmployeeId = @employeeId

		-- End March 18 2020


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

		-- March 19 2020
		print 'EntityBankDetailImage'
		DELETE FROM dbo.EntityBankDetailImage
		WHERE EntityBankDetailId IN
		(SELECT ebd.ID FROM dbo.EntityBankDetail ebd
		INNER JOIN @Entity e2 on e2.Id = ebd.EntityId)

		print 'EntityBankDetail'
		DELETE FROM dbo.EntityBankDetail
		WHERE EntityId IN (SELECT Id from @Entity)
		-- END of changes on March 19 2020


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

-- March 21 2020
ALTER TABLE [dbo].[SqliteBankDetail]
DROP COLUMN [IsPrimaryAccount]
GO

ALTER TABLE [dbo].[EntityBankDetail]
DROP COLUMN [IsPrimaryAccount]
GO

ALTER TABLE [dbo].[EntityBankDetail]
ADD [Status] NVARCHAR(50) NOT NULL DEFAULT 'Pending',
	[IsApproved] BIT NOT NULL DEFAULT 0,
	[Comments] NVARCHAR(100) NOT NULL DEFAULT ''
GO

ALTER PROCEDURE [dbo].[ProcessSqliteBankDetailsData]
	@batchId BIGINT
AS
BEGIN

	-- if batch is already processed - return	
	IF NOT EXISTS(SELECT 1 FROM dbo.SqliteActionBatch 
			WHERE Id = @batchId AND BankDetailsSaved > 0
			AND BatchProcessed = 0)
	BEGIN
		RETURN;
	END

	-- First create required records in Day Table
	INSERT INTO dbo.[Day]
	([DATE])
	SELECT DISTINCT CAST(e.[BankDetailDate] AS [Date])
	FROM dbo.SqliteBankDetail e
	LEFT JOIN dbo.[Day] d on CAST(e.[BankDetailDate] AS [Date]) = d.[DATE]
	WHERE e.BatchId = @batchId
	AND d.[DATE] IS NULL

	-- Fill Entity Id in SqliteBankDetail that belong to new entity created on phone.
	Update dbo.SqliteBankDetail
	SET EntityId = en.EntityId,
	EntityName = en.EntityName,
	DateUpdated = SYSUTCDATETIME()
	FROM dbo.SqliteBankDetail ag
	INNER JOIN dbo.SqliteEntity en on ag.ParentReferenceId = en.PhoneDbId
	AND ag.BatchId = @batchId
	AND ag.IsNewEntity = 1
	AND ag.IsProcessed = 0


	DECLARE @insertTable TABLE (EntityBankDetailId BIGINT, SqliteBankDetailId BIGINT)

	-- Insert rows in dbo.EntityAgreement
	INSERT into dbo.EntityBankDetail
	(EntityId, IsSelfAccount, AccountHolderName, AccountHolderPAN,
	BankName, BankAccount, BankIFSC, BankBranch, ImageCount,
	CreatedBy, UpdatedBy, SqliteBankDetailId, [Status], IsApproved)
	OUTPUT inserted.Id, inserted.SqliteBankDetailId INTO @insertTable
	SELECT ag.EntityId, ag.IsSelfAccount, ag.AccountHolderName, ag.AccountHolderPAN,
	ag.BankName, ag.BankAccount, ag.BankIFSC, ag.BankBranch, ag.ImageCount,
	'ProcessSqliteBankDetailsData', 'ProcessSqliteBankDetailsData', ag.Id, 'Pending', 0
	FROM dbo.SqliteBankDetail ag
	WHERE BatchId = @batchId
	AND IsProcessed = 0

	-- Now update EntityBankDetailId back in SqliteBankDetail
	Update dbo.SqliteBankDetail
	SET EntityBankDetailId = m3.EntityBankDetailId,
	IsProcessed = 1,
	DateUpdated = SYSUTCDATETIME()
	FROM dbo.SqliteBankDetail sagg
	INNER JOIN @insertTable m3 on m3.SqliteBankDetailId = sagg.Id

	-- Now Images
		
	-- Now create entries in EntityBankDetailImage
	INSERT INTO dbo.EntityBankDetailImage
	(EntityBankDetailId, SequenceNumber, ImageFileName)
	SELECT sbd.EntityBankDetailId, sbdi.SequenceNumber, sbdi.ImageFileName
	FROM dbo.SqliteBankDetailImage sbdi
	INNER JOIN dbo.SqliteBankDetail sbd ON sbdi.SqliteBankDetailId = sbd.Id
	AND sbd.BatchId = @batchId
END
GO

INSERT into dbo.codeTable
(codeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
('BankDetailStatus', '', 'Pending', 5, 1, 1),
('BankDetailStatus', '', 'Approved', 5, 1, 1),
('BankDetailStatus', '', 'Not approved', 25, 1, 1)
go
