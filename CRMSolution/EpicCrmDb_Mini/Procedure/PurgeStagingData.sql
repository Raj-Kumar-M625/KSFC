CREATE PROCEDURE [dbo].[PurgeStagingData]
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

	-- DELETE OrderImage
	DELETE FROM dbo.SqliteOrderImage
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

	-- DELETE SqliteEntityLocation
	DELETE FROM dbo.SqliteEntityLocation
	WHERE SqliteEntityId In 
	 ( SELECT id FROM dbo.SqliteEntity a
	   JOIN @BatchIds b on b.BatchId = a.BatchId
	 )

	DELETE FROM dbo.SqliteAgreement
	WHERE BatchId in (SELECT BatchId FROM @BatchIds)

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

	--==============
	-- July 5 2020
	--==============
	DELETE FROM dbo.SqliteCancelledLeave
	WHERE BatchId in (SELECT BatchId FROM @BatchIds)

	DELETE FROM dbo.SqliteLeave
	WHERE BatchId in (SELECT BatchId FROM @BatchIds)

	-- 
	DELETE FROM dbo.SqliteAdvanceRequest
	WHERE BatchId in (SELECT BatchId FROM @BatchIds)

	DELETE FROM dbo.SqliteIssueReturn
	WHERE BatchId in (SELECT BatchId FROM @BatchIds)

	DELETE FROM dbo.SqliteTerminateAgreement
	WHERE BatchId in (SELECT BatchId FROM @BatchIds)

	DELETE FROM dbo.SqliteDeviceLog
	WHERE BatchId in (SELECT BatchId FROM @BatchIds)

	BEGIN
		DELETE FROM dbo.SqliteEntityWorkFlowFollowUp
		WHERE SqliteEntityWorkFlowId In 
		 ( SELECT id FROM dbo.SqliteEntityWorkFlowV2 a
		   JOIN @BatchIds b on b.BatchId = a.BatchId
		 )

		DELETE FROM dbo.SqliteEntityWorkFlowItemUsed
		WHERE SqliteEntityWorkFlowId In 
		 ( SELECT id FROM dbo.SqliteEntityWorkFlowV2 a
		   JOIN @BatchIds b on b.BatchId = a.BatchId
		 )

		DELETE FROM dbo.SqliteEntityWorkFlowV2
		WHERE BatchId in (SELECT BatchId FROM @BatchIds)
	END

	BEGIN
		DELETE FROM dbo.SqliteSTRDWS
		WHERE SqliteSTRId In 
			( SELECT id FROM dbo.SqliteSTR a
			JOIN @BatchIds b on b.BatchId = a.BatchId
			)

		DELETE FROM dbo.SqliteSTRImage
		WHERE SqliteSTRId In 
		 ( SELECT id FROM dbo.SqliteSTR a
		   JOIN @BatchIds b on b.BatchId = a.BatchId
		 )

		DELETE FROM dbo.SqliteSTR
		WHERE BatchId in (SELECT BatchId FROM @BatchIds)
	END


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
