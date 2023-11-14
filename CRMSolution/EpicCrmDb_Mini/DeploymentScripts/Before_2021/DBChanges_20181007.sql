CREATE TABLE [dbo].[SqliteDeviceLog]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [BatchId] BIGINT NOT NULL  REFERENCES dbo.SqliteActionBatch, 
    [EmployeeId] BIGINT NOT NULL, 
	[At] DATETIME2 NOT NULL,
	[LogText] NVARCHAR(255) NOT NULL,
    [DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(), 
)
GO

ALTER TABLE [dbo].[SqliteActionBatch]
ADD [DeviceLogCount] BIGINT NOT NULL DEFAULT 0
GO

IF OBJECT_ID ( '[dbo].[ClearEmployeeData]', 'P' ) IS NOT NULL   
    DROP PROCEDURE [dbo].[ClearEmployeeData];  
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

		DELETE FROM dbo.ActivityImage
		WHERE ActivityId IN (SELECT id FROM @activity)

		DELETE from dbo.Activity
		WHERE id in (SELECT Id from @activity)

		-----------------

		DELETE from dbo.DistanceCalcErrorLog
		WHERE id in
		(SELECT l.id from dbo.distanceCalcErrorLog l
		INNER JOIN dbo.Tracking t on l.TrackingId = t.Id
		INNER JOIN dbo.EmployeeDay ed on ed.Id = t.employeeDayId
		and ed.TenantEmployeeId = @employeeId)

		DELETE from dbo.Tracking
		WHERE id in
		(SELECT t.id from dbo.Tracking t
		INNER JOIN dbo.EmployeeDay ed on ed.Id = t.employeeDayId
		and ed.TenantEmployeeId = @employeeId)

		DELETE from dbo.employeeDay WHERE TenantEmployeeId = @employeeId

		DELETE from dbo.Imei WHERE TenantEmployeeId = @employeeId


		--DELETE FROM dbo.SqliteAction WHERE EmployeeId = @employeeId
		--DELETE FROM dbo.SqliteActionBatch WHERE EmployeeId = @employeeId

		-- delete expense Data as well
		DELETE FROM dbo.SqliteExpenseImage WHERE SqliteExpenseId in (SELECT Id FROM dbo.SqliteExpense WHERE EmployeeId = @employeeId)
		DELETE FROM dbo.SqliteExpense WHERE EmployeeId = @employeeId

		-- Delete SqliteOrder data
		DELETE FROM dbo.SqliteOrderItem WHERE SqliteOrderId IN (SELECT Id FROM dbo.[SqliteOrder] WHERE EmployeeId = @employeeId)
		DELETE FROM dbo.SqliteOrderImage WHERE SqliteOrderId IN (SELECT Id FROM dbo.[SqliteOrder] WHERE EmployeeId = @employeeId)
		DELETE FROM dbo.[SqliteOrder] WHERE EmployeeId = @employeeId

		-- delete SqlLiteAction Data as well
		DELETE FROM dbo.SqliteActionImage where SqliteActionId in (SELECT id FROM dbo.SqliteAction WHERE EmployeeId = @employeeId)
		DELETE FROM dbo.SqliteAction WHERE EmployeeId = @employeeId
		DELETE FROM dbo.SqliteActionDup WHERE EmployeeId = @employeeId

		-- delete SqlitePayment data as well
		DELETE FROM dbo.SqlitePaymentImage WHERE SqlitePaymentId in (SELECT Id FROM dbo.SqlitePayment WHERE EmployeeId = @employeeId)
		DELETE FROM dbo.SqlitePayment WHERE EmployeeId = @employeeId

		-- Delete SqliteReturnOrder data
		DELETE FROM dbo.SqliteReturnOrderItem WHERE SqliteReturnOrderId IN (SELECT Id FROM dbo.[SqliteReturnOrder] WHERE EmployeeId = @employeeId)
		DELETE FROM dbo.[SqliteReturnOrder] WHERE EmployeeId = @employeeId

		-- Delete SqliteEntity data
		DECLARE @SqliteEntity TABLE (Id BIGINT)
		INSERT INTO @SqliteEntity SELECT Id FROM dbo.[SqliteEntity] WHERE EmployeeId = @employeeId
		DELETE FROM dbo.[SqliteEntityContact] WHERE [SqliteEntityId] IN (SELECT Id FROM @SqliteEntity)
		DELETE FROM dbo.[SqliteEntityCrop] WHERE [SqliteEntityId] IN (SELECT Id FROM @SqliteEntity)
		DELETE FROM dbo.[SqliteEntity] WHERE EmployeeId = @employeeId

		-- Delete SqliteLeave data
		DELETE FROM dbo.[SqliteLeave] WHERE EmployeeId = @employeeId

		-- Delete SqliteCancelledLeave data
		DELETE FROM dbo.[SqliteCancelledLeave] WHERE EmployeeId = @employeeId
		
		-- Delete Device Log
		DELETE FROM dbo.[SqliteDeviceLog] WHERE EmployeeId = @employeeId

		DELETE FROM dbo.SqliteActionBatch WHERE EmployeeId = @employeeId

		-- store image ids first for processed expense data
		INSERT INTO @image (id)
		SELECT ImageId 
		FROM dbo.ExpenseItemImage WHERE ExpenseItemId IN 
		(SELECT ei.id FROM dbo.ExpenseItem ei
		INNER JOIN dbo.Expense e on ei.ExpenseId = e.Id
		AND e.EmployeeId = @employeeId)

		DELETE FROM dbo.ExpenseItemImage WHERE ExpenseItemId IN 
		(SELECT ei.id FROM dbo.ExpenseItem ei INNER JOIN dbo.Expense e on ei.ExpenseId = e.Id AND e.EmployeeId = @employeeId)

		DELETE FROM dbo.ExpenseItem WHERE ExpenseId in (SELECT Id FROM dbo.Expense WHERE EmployeeId = @employeeId)
		DELETE FROM dbo.ExpenseApproval WHERE ExpenseId in (SELECT Id FROM dbo.Expense WHERE EmployeeId = @employeeId)
		DELETE FROM dbo.Expense WHERE EmployeeId = @employeeId

		-- Delete order Data
		DELETE FROM dbo.OrderItem WHERE OrderId IN (SELECT Id FROM dbo.[Order] WHERE EmployeeId = @employeeId)

		INSERT INTO @image (id)
		SELECT oim.ImageId
		FROM dbo.OrderImage oim
		INNER JOIN dbo.[Order] o on o.Id = oim.OrderId
		AND o.EmployeeId = @employeeId

		DELETE FROM dbo.OrderImage WHERE OrderId IN (SELECT Id FROM dbo.[Order] WHERE EmployeeId = @employeeId)

		DELETE FROM dbo.[Order] WHERE EmployeeId = @employeeId



		-- Delete Return Order Data
		DELETE FROM dbo.ReturnOrderItem WHERE ReturnOrderId IN (SELECT Id FROM dbo.[ReturnOrder] WHERE EmployeeId = @employeeId)
		DELETE FROM dbo.[ReturnOrder] WHERE EmployeeId = @employeeId

		-- Delete Entity data
		DECLARE @Entity TABLE (Id BIGINT)
		INSERT INTO @Entity SELECT Id FROM dbo.[Entity] WHERE EmployeeId = @employeeId
		DELETE FROM dbo.[EntityContact] WHERE [EntityId] IN (SELECT Id FROM @Entity)
		DELETE FROM dbo.[EntityCrop] WHERE [EntityId] IN (SELECT Id FROM @Entity)
		DELETE FROM dbo.[Entity] WHERE EmployeeId = @employeeId


		--
		-- Delete Payment Data
		--
		INSERT INTO @image (id)
		SELECT pim.ImageId
		FROM dbo.PaymentImage pim
		INNER JOIN dbo.Payment p on p.Id = pim.PaymentId
		AND p.EmployeeId = @employeeId

		DELETE FROM dbo.PaymentImage WHERE PaymentId IN (SELECT Id FROM dbo.[Payment] WHERE EmployeeId = @employeeId)
		DELETE FROM dbo.[Payment] WHERE EmployeeId = @employeeId

		-- DELETE IMAGES
		DELETE FROM dbo.[Image]
		WHERE Id in (SELECT Id FROM @image)

		DELETE from dbo.TenantEmployee WHERE id = @employeeId

		COMMIT
	END TRY

	BEGIN CATCH
		
		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:ClearEmployeeData', ERROR_MESSAGE()

		ROLLBACK TRANSACTION
		throw;

	END CATCH
END	
GO
