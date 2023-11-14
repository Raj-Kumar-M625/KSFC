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
		-- this table is no longer used;
		DELETE FROM dbo.SqliteEntityWorkFlow WHERE  ActivityId in (SELECT ID FROM @activity)

		-- April 11 2020
		print 'SqliteEntityWorkFlowItemUsed'
		DELETE FROM [dbo].[SqliteEntityWorkFlowItemUsed]
		WHERE SqliteEntityWorkFlowId IN
			(SELECT Id FROM dbo.SqliteEntityWorkFlowV2 WHERE EmployeeId = @employeeId)

		-- April 11 2020
		print 'SqliteEntityWorkFlowFollowUp'
		DELETE FROM [dbo].SqliteEntityWorkFlowFollowUp
		WHERE SqliteEntityWorkFlowId IN
			(SELECT Id FROM dbo.SqliteEntityWorkFlowV2 WHERE EmployeeId = @employeeId)

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

		-- May 10 2020
		print '[SqliteSTRImage]'
		DELETE FROM dbo.SqliteSTRImage WHERE [SqliteSTRId] in
		( SELECT id FROM dbo.SqliteSTR WHERE EmployeeId = @employeeId )

		print '[SqliteSTRDWS]'
		DELETE FROM dbo.SqliteSTRDWS WHERE [SqliteSTRId] in
		( SELECT id FROM dbo.SqliteSTR WHERE EmployeeId = @employeeId )

		print '[SqliteSTR]'
		DELETE FROM dbo.[SqliteSTR] WHERE EmployeeId = @employeeId

		print '[SqliteAnswerDetail]'
		DELETE FROM dbo.[SqliteAnswerDetail] WHERE [AnswerId] in
		(SELECT Id FROM dbo.SqliteAnswer WHERE [CrossRefId] in
		(SELECT Id FROM dbo.SqliteQuestionnaire WHERE EmployeeId = @employeeId ))

	    print '[SqliteAnswer]'
		DELETE FROM dbo.[SqliteAnswer] WHERE [CrossRefId] in
		(SELECT Id FROM dbo.SqliteQuestionnaire WHERE EmployeeId = @employeeId )

	    print '[SqliteQuestionnaire ]'
		DELETE FROM dbo.SqliteQuestionnaire WHERE EmployeeId = @employeeId

		print '[SqliteSurvey]'
		DELETE FROM dbo.SqliteSurvey WHERE EmployeeId = @employeeId

		print'[SqliteDayPlanTarget]'
		DELETE FROM dbo.SqliteDayPlanTarget WHERE EmployeeId=@employeeId

		print'[SqliteTask]'
		DELETE FROM dbo.SqliteTask WHERE EmployeeId=@employeeId

		print'[SqliteTaskAction]'
		DELETE FROM dbo.SqliteTaskAction WHERE EmployeeId=@employeeId
		-- End May 10 2020

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

		-- May 12 2020 - DELETE STR/DWS Data
		DECLARE @insertedSTR TABLE
		(
		  Id BIGINT,   -- STRId
		  STRTagId BIGINT
		)
		INSERT INTO @insertedSTR
		(Id, STRTagId)
		SELECT Id, STRTagId
		FROM dbo.[STR]
		WHERE EmployeeId = @employeeId

		-- STR may have data for different employee Id as well - tagged to same STRTagId
		-- so select those as well
		INSERT INTO @insertedSTR
		(Id, STRTagId)
		SELECT Id, STRTagId
		FROM dbo.[STR]
		WHERE STRTagId in (SELECT distinct STRTagId FROM @insertedSTR)

		DELETE FROM dbo.STRImage
		WHERE STRId IN (SELECT Id FROM @insertedSTR)

		DELETE FROM dbo.DWS
		WHERE STRId IN (SELECT Id FROM @insertedSTR)

		DELETE FROM dbo.[STR]
		WHERE Id IN (SELECT Id FROM @insertedSTR)

		DELETE FROM dbo.[STRTag]
		WHERE Id IN (SELECT STRTagId FROM @insertedSTR)

		-- End May 12 2020

		-- DELETE Workflow data
		print 'EntityWorkFlowDetailItemUsed'
		DELETE FROM dbo.EntityWorkFlowDetailItemUsed
		WHERE EntityWorkFlowDetailId IN (SELECT d.ID FROM dbo.EntityWorkFlowDetail d
		             INNER JOIN dbo.EntityWorkFlow p on d.EntityWorkFlowId = p.Id
					 AND p.EmployeeId = @employeeId)

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

		print 'EntitySurvey'
		DELETE FROM dbo.EntitySurvey
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

		print 'Task Action'
        DELETE FROM dbo.TaskAction WHERE EmployeeId=@employeeId

		print 'Task Assignment'
        DELETE FROM dbo.TaskAssignment WHERE EmployeeId = @employeeId

		print 'DayPlanTarget'
		DELETE FROM dbo.DayPlanTarget WHERE EmployeeId=@employeeId

		print 'AnswerDetail'
		DELETE FROM dbo.AnswerDetail WHERE AnswerId in
		(SELECT ID FROM Answer WHERE CrossRefId IN
		(SELECT ID FROM CustomerQuestionnaire WHERE EmployeeId=@employeeId))

		print 'Answer'
		DELETE FROM dbo.Answer WHERE CrossRefId IN
		(SELECT ID FROM CustomerQuestionnaire WHERE EmployeeId=@employeeId)

		print 'CustomerQuestionnaire'
		DELETE FROM dbo.[CustomerQuestionnaire] WHERE EmployeeId=@employeeId

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
			COMMIT
	END CATCH

END