IF OBJECT_ID ( 'dbo.ClearEmployeeData', 'P' ) IS NOT NULL   
    DROP PROCEDURE dbo.ClearEmployeeData;  
GO  

CREATE PROCEDURE [dbo].[ClearEmployeeData]
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
		DELETE FROM dbo.SqliteEntityWorkFlow WHERE  ActivityId in (SELECT ID FROM @activity)

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
		print '[SqliteEntity]'
		DELETE FROM dbo.[SqliteEntity] WHERE EmployeeId = @employeeId

		-- Delete SqliteLeave data
		print '[SqliteLeave]'
		DELETE FROM dbo.[SqliteLeave] WHERE EmployeeId = @employeeId

		-- Delete SqliteCancelledLeave data
		print '[SqliteCancelledLeave]'
		DELETE FROM dbo.[SqliteCancelledLeave] WHERE EmployeeId = @employeeId
		
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

		-- DELETE IMAGES
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
