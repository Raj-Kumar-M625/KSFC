-- Process to nuke staging data
CREATE TABLE [dbo].[PurgeLog]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[DateFrom] DATETIME NOT NULL,
	[DateTo] DATETIME NOT NULL,
	[ActionPurged] BIGINT NOT NULL DEFAULT 0,
	[ActionDupPurged] BIGINT NOT NULL DEFAULT 0,
	[ExpensePurged] BIGINT NOT NULL DEFAULT 0,
	[OrderPurged] BIGINT NOT NULL DEFAULT 0,
	[PaymentPurged] BIGINT NOT NULL DEFAULT 0,
	[ReturnPurged] BIGINT NOT NULL DEFAULT 0,
	[DateCreated] DATETIME2 NOT NULL DEFAULT SysUtcDateTime()
)
go

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


	INSERT INTO dbo.PurgeLog
	(DateFrom, DateTo, ActionPurged, ActionDupPurged, ExpensePurged, OrderPurged, PaymentPurged, ReturnPurged)
	VALUES
	(
		@nukeFromDate, 
		@nukeUptoDate, 
		@ActionPurged,
		@ActionDupPurged,
		@ExpensePurged,
		@OrderPurged,
		@PaymentPurged,
		@ReturnPurged 
	)
END
go

declare @nukeFromDate DateTime = '2017-09-01 00:00:00'
declare @nukeUptoDate DateTime = '2017-09-30 23:59:59'
exec [dbo].[PurgeStagingData] @nukeFromDate, @nukeUptoDate
go
declare @nukeFromDate DateTime = '2017-10-01 00:00:00'
declare @nukeUptoDate DateTime = '2017-10-31 23:59:59'
exec [dbo].[PurgeStagingData] @nukeFromDate, @nukeUptoDate
go
declare @nukeFromDate DateTime = '2017-11-01 00:00:00'
declare @nukeUptoDate DateTime = '2017-11-30 23:59:59'
exec [dbo].[PurgeStagingData] @nukeFromDate, @nukeUptoDate
go
declare @nukeFromDate DateTime = '2017-12-01 00:00:00'
declare @nukeUptoDate DateTime = '2017-12-31 23:59:59'
exec [dbo].[PurgeStagingData] @nukeFromDate, @nukeUptoDate
go
declare @nukeFromDate DateTime = '2018-01-01 00:00:00'
declare @nukeUptoDate DateTime = '2018-01-31 23:59:59'
exec [dbo].[PurgeStagingData] @nukeFromDate, @nukeUptoDate
go
declare @nukeFromDate DateTime = '2018-02-01 00:00:00'
declare @nukeUptoDate DateTime = '2018-02-28 23:59:59'
exec [dbo].[PurgeStagingData] @nukeFromDate, @nukeUptoDate
go
declare @nukeFromDate DateTime = '2018-03-01 00:00:00'
declare @nukeUptoDate DateTime = '2018-03-31 23:59:59'
exec [dbo].[PurgeStagingData] @nukeFromDate, @nukeUptoDate
go
declare @nukeFromDate DateTime = '2018-04-01 00:00:00'
declare @nukeUptoDate DateTime = '2018-04-30 23:59:59'
exec [dbo].[PurgeStagingData] @nukeFromDate, @nukeUptoDate
