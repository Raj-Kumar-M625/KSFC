-- June 10 2020

ALTER TABLE [dbo].[FeatureControl]
ADD
	[StockReceiveOption] BIT NOT NULL DEFAULT 0,
	[StockReceiveConfirmOption] BIT NOT NULL DEFAULT 0,
	[StockRequestOption] BIT NOT NULL DEFAULT 0,
	[StockRequestFulfillOption] BIT NOT NULL DEFAULT 0,
	[ExtraAdminOption] BIT NOT NULL DEFAULT 0
GO

/* Drop constraint first */
/*******************/
ALTER TABLE dbo.FeatureControl
DROP CONSTRAINT DF__FeatureCo__STRSi__48E5AC6E

ALTER TABLE dbo.FeatureControl
DROP COLUMN	[STRSiloControl];
/*******************/

-- June 11 2020
CREATE TABLE [dbo].[VendorInput]
(
	[Vendor Id] NVARCHAR(10) NOT NULL,
	[Company name] NVARCHAR(50) NOT NULL,
	[Contact person] NVARCHAR(50) NOT NULL,
	[Address] NVARCHAR(100) NOT NULL,
	[Mobile] NVARCHAR(20) NULL,
)
GO

CREATE TABLE [dbo].[Vendor]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[VendorId] NVARCHAR(10) NOT NULL,
	[CompanyName] NVARCHAR(50) NOT NULL,
	[ContactPerson] NVARCHAR(50) NOT NULL,
	[Address] NVARCHAR(100) NOT NULL,
	[Mobile] NVARCHAR(20) NOT NULL,
)
GO

INSERT into dbo.codeTable
(codeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
('ExcelUpload', 'Vendor', 'VendorInput', 130, 1, 1)
go

CREATE PROCEDURE [dbo].[TransformVendorDataFeed]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
		-- Step 1: Truncate table if opted to do so

		IF @IsCompleteRefresh = 1
		BEGIN
			TRUNCATE TABLE dbo.Vendor
		END

		-- Update existing Data
		UPDATE dbo.Vendor
		SET 
			[CompanyName] = ti.[Company name],
			[ContactPerson] = ti.[Contact person],
			[Address] = ti.[Address],
			[Mobile] = ti.[Mobile]
		FROM dbo.Vendor t
		INNER JOIN dbo.VendorInput ti on t.VendorId = ti.[Vendor Id]

		-- Insert New data
		INSERT INTO dbo.Vendor
		(
			[VendorId],
			[CompanyName],
			[ContactPerson],
			[Address],
			[Mobile]
		)
		SELECT  
			[Vendor Id],
			[Company name],
			[Contact person],
			ti.[Address],
			ti.[Mobile]
		
		FROM dbo.VendorInput ti
		LEFT JOIN dbo.Vendor t ON ti.[Vendor Id] = t.VendorId
		WHERE t.VendorId IS NULL


		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformVendorDataFeed', 'Success'
END
GO

CREATE TABLE [dbo].[GRNNumber]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[Sequence] BIGINT NOT NULL,
	[GRNNumber] NVARCHAR(20) NOT NULL,
	[IsUsed] BIT NOT NULL DEFAULT 0,
	[UsedTimestamp] DATETIME2 NOT NULL DEFAULT SysUtcDateTime()
)
GO

CREATE TABLE [dbo].[GRNNumberInput]
(
	[Sequence] BIGINT NOT NULL,
	[GRNNumber] NVARCHAR(20) NOT NULL
)
GO

CREATE PROCEDURE [dbo].[TransformGRNNumberData]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
	
		DELETE FROM dbo.[GRNNumber]
		WHERE IsUsed = 0

		-- Step 2: Insert data
		INSERT INTO dbo.[GRNNumber]
		([Sequence], GRNNumber)
		SELECT  
		ani.[Sequence],
		ltrim(rtrim(ani.[GRNNumber]))
		FROM dbo.GRNNumberInput ani
		LEFT JOIN dbo.GRNNumber an on ani.GRNNumber = an.GRNNumber
		WHERE an.GRNNumber is null

		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformGRNNumberData', 'Success'
END
GO

INSERT into dbo.codeTable
(codeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
('ExcelUpload', 'GRNNumber', 'GRNNumberInput', 100, 1, 1)
GO

CREATE PROCEDURE [dbo].[GetGRNNumber]
	@count INT
AS
BEGIN
	-- Select entity numbers
	DECLARE @grnNum TABLE (Id BIGINT Identity, GRNNumber NVARCHAR(20))

	-- take as many GRN Numbers as requested
	-- (may have to enhance to check that we get enough / required numbers)
	UPDATE dbo.GRNNumber
	SET ISUsed = 1,
	UsedTimeStamp = SYSUTCDATETIME()
	OUTPUT deleted.GRNNumber INTO @grnNum
	FROM dbo.GRNNumber an
	INNER JOIN 
	(
		SELECT TOP(@Count) Id
		FROM dbo.GRNNumber WITH (READPAST)
		WHERE ISUsed = 0
		ORDER BY [Sequence]
	) an2 on an.Id = an2.Id

	-- count the number of rows
	DECLARE @rowCount INT
	SELECT @rowCount = COUNT(*) FROM @grnNum

	IF @rowCount <> @count
	BEGIN
		THROW 51000, 'GetGRNNumber: Could not get enough/requested GRN Numbers', 1
	END

	SELECT GRNNumber
	FROM @grnNum
	ORDER BY Id
END
GO

-- June 12 2020
CREATE TABLE [dbo].[StockInputTag]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[GRNNumber] NVARCHAR(20) NOT NULL,
	[ReceiveDate] DATETIME2 NOT NULL,

	[VendorName] NVARCHAR(50) NOT NULL,
	[VendorBillNo] NVARCHAR(20) NOT NULL,
	[VendorBillDate] DATETIME2 NOT NULL,

	[TotalItemCount] INT NOT NULL,
	[TotalAmount] DECIMAL(19,2) NOT NULL,
	[Notes] NVARCHAR(100) NOT NULL,
	[Status] NVARCHAR(50) NOT NULL DEFAULT 'Received',

	[ZoneCode] NVARCHAR(50) NOT NULL,
	[AreaCode] NVARCHAR(50) NOT NULL,
	[TerritoryCode] NVARCHAR(50) NOT NULL,
	[HQCode] NVARCHAR(50) NOT NULL,
	[StaffCode] NVARCHAR(10) NOT NULL,

	[ReviewNotes] NVARCHAR(100),
	[ReviewedBy] NVARCHAR(50),
	[ReviewDate] DATETIME2 NOT NULL DEFAULT '2000-01-01',

	[CreatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[UpdatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

	[IsEditAllowed] BIT NOT NULL DEFAULT 1,
	[CyclicCount] BIGINT NOT NULL DEFAULT 1
)
GO

CREATE TABLE [dbo].[StockInput]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[StockInputTagId] BIGINT NOT NULL References dbo.[StockInputTag],

	[LineNumber] INT NOT NULL,
	[ItemMasterId] BIGINT NOT NULL REFERENCES dbo.ItemMaster,
	[Quantity] INT NOT NULL,
	[Rate] DECIMAL(19,2) NOT NULL,
	[Amount] DECIMAL(19,2) NOT NULL,

	[CyclicCount] BIGINT NOT NULL DEFAULT 1,

	[CreatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[UpdatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
)
GO

ALTER PROCEDURE [dbo].[TableList]
AS
BEGIN
		-- Stored procedure to get table names
	SELECT TABLE_NAME
	FROM INFORMATION_SCHEMA.Tables
	WHERE table_Name not like '%Image%'
	AND TABLE_Name not like '%ASPNet%'
	AND TABLE_Name not like '%SysDiagrams%'
	ORDER BY TABLE_Name

END
GO

-- June 15 2020

CREATE TABLE [dbo].[RequestNumber]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[Sequence] BIGINT NOT NULL,
	[RequestNumber] NVARCHAR(20) NOT NULL,
	[IsUsed] BIT NOT NULL DEFAULT 0,
	[UsedTimestamp] DATETIME2 NOT NULL DEFAULT SysUtcDateTime()
)
GO

CREATE TABLE [dbo].[RequestNumberInput]
(
	[Sequence] BIGINT NOT NULL,
	[RequestNumber] NVARCHAR(20) NOT NULL
)
GO

CREATE PROCEDURE [dbo].[TransformRequestNumberData]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN
	
		DELETE FROM dbo.[RequestNumber]
		WHERE IsUsed = 0

		-- Step 2: Insert data
		INSERT INTO dbo.[RequestNumber]
		([Sequence], RequestNumber)
		SELECT  
		ani.[Sequence],
		ltrim(rtrim(ani.[RequestNumber]))
		FROM dbo.RequestNumberInput ani
		LEFT JOIN dbo.RequestNumber an on ani.RequestNumber = an.RequestNumber
		WHERE an.RequestNumber is null

		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformRequestNumberData', 'Success'
END
GO

INSERT into dbo.codeTable
(codeType, CodeName, CodeValue, DisplaySequence, IsActive, TenantId)
values
('ExcelUpload', 'RequestNumber', 'RequestNumberInput', 100, 1, 1)
GO

CREATE PROCEDURE [dbo].[GetRequestNumber]
	@count INT
AS
BEGIN
	-- Select entity numbers
	DECLARE @reqNum TABLE (Id BIGINT Identity, RequestNumber NVARCHAR(20))

	-- take as many Request Numbers as requested
	-- (may have to enhance to check that we get enough / required numbers)
	UPDATE dbo.RequestNumber
	SET ISUsed = 1,
	UsedTimeStamp = SYSUTCDATETIME()
	OUTPUT deleted.RequestNumber INTO @reqNum
	FROM dbo.RequestNumber an
	INNER JOIN 
	(
		SELECT TOP(@Count) Id
		FROM dbo.RequestNumber WITH (READPAST)
		WHERE ISUsed = 0
		ORDER BY [Sequence]
	) an2 on an.Id = an2.Id

	-- count the number of rows
	DECLARE @rowCount INT
	SELECT @rowCount = COUNT(*) FROM @reqNum

	IF @rowCount <> @count
	BEGIN
		THROW 51000, 'GetRequestNumber: Could not get enough/requested Request Numbers', 1
	END

	SELECT RequestNumber
	FROM @reqNum
	ORDER BY Id
END
GO

CREATE TABLE [dbo].[StockRequestTag]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[RequestNumber] NVARCHAR(20) NOT NULL,
	[RequestDate] DATETIME2 NOT NULL,

	[Notes] NVARCHAR(100) NOT NULL,
	[Status] NVARCHAR(50) NOT NULL DEFAULT 'Received',

	[ZoneCode] NVARCHAR(50) NOT NULL,
	[AreaCode] NVARCHAR(50) NOT NULL,
	[TerritoryCode] NVARCHAR(50) NOT NULL,
	[HQCode] NVARCHAR(50) NOT NULL,
	[StaffCode] NVARCHAR(10) NOT NULL,

	[CreatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[UpdatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

	[CyclicCount] BIGINT NOT NULL DEFAULT 1
)
GO

CREATE TABLE [dbo].[StockRequest]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[StockRequestTagId] BIGINT NOT NULL References dbo.[StockRequestTag],

	[ItemMasterId] BIGINT NOT NULL REFERENCES dbo.ItemMaster,
	[Quantity] INT NOT NULL,

	[CyclicCount] BIGINT NOT NULL DEFAULT 1,

	[Status] NVARCHAR(50) NOT NULL DEFAULT 'PendingFulfillment',

	[QuantityIssued] INT NOT NULL DEFAULT 0,
	[IssueDate] DATETIME2,
	[StockLedgerId] BIGINT NOT NULL DEFAULT 0,

	[CreatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[UpdatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
)
GO

------------------
-- June 19 2020 --
------------------
CREATE TABLE [dbo].[StockLedger]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[TransactionDate] DATE NOT NULL,
	[ItemMasterId] BIGINT NOT NULL REFERENCES dbo.ItemMaster,
	[ReferenceNo] NVARCHAR(20) NOT NULL,
	[LineNumber] INT NOT NULL,
	[IssueQuantity] INT NOT NULL DEFAULT 0,
	[ReceiveQuantity] INT NOT NULL DEFAULT 0,

	[ZoneCode] NVARCHAR(50) NOT NULL,
	[AreaCode] NVARCHAR(50) NOT NULL,
	[TerritoryCode] NVARCHAR(50) NOT NULL,
	[HQCode] NVARCHAR(50) NOT NULL,
	[StaffCode] NVARCHAR(10) NOT NULL,

	[CyclicCount] BIGINT NOT NULL DEFAULT 1,
	[CreatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[UpdatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
)
GO

CREATE TABLE [dbo].[StockBalance]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[ItemMasterId] BIGINT NOT NULL REFERENCES dbo.ItemMaster,
	[StockQuantity] BIGINT NOT NULL DEFAULT 0,

	[ZoneCode] NVARCHAR(50) NOT NULL,
	[AreaCode] NVARCHAR(50) NOT NULL,
	[TerritoryCode] NVARCHAR(50) NOT NULL,
	[HQCode] NVARCHAR(50) NOT NULL,
	[StaffCode] NVARCHAR(10) NOT NULL,

	[CyclicCount] BIGINT NOT NULL DEFAULT 1,
	[CreatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[UpdatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
)
GO

CREATE PROCEDURE [dbo].[PostStockLedgerFromInput]
	@stockInputTagId BIGINT,
	@updatedBy NVARCHAR(50)
AS
BEGIN

	DECLARE @trn TABLE
	(
		[TransactionDate] DATE NOT NULL,
		[ItemMasterId] BIGINT NOT NULL,
		[ReferenceNo] NVARCHAR(20) NOT NULL,
		[LineNumber] INT NOT NULL,
		[Quantity] INT NOT NULL,
		[ZoneCode] NVARCHAR(50) NOT NULL,
		[AreaCode] NVARCHAR(50) NOT NULL,
		[TerritoryCode] NVARCHAR(50) NOT NULL,
		[HQCode] NVARCHAR(50) NOT NULL,
		[StaffCode] NVARCHAR(10) NOT NULL,
		[CyclicCount] BIGINT NOT NULL DEFAULT 1,
		[CreatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
		[UpdatedBy] NVARCHAR(50) NOT NULL DEFAULT ''
	)

	-- put transaction entries in an in-memory table.
	INSERT INTO @trn
	(TransactionDate, ItemMasterId, ReferenceNo, LineNumber, Quantity, 
	ZoneCode, AreaCode, TerritoryCode, HQCode, StaffCode, 
	CyclicCount, CreatedBy, UpdatedBy)
	SELECT
	tag.ReceiveDate, si.ItemMasterId, tag.GRNNumber, si.LineNumber, si.Quantity,
	tag.ZoneCode, tag.AreaCode, tag.TerritoryCode, tag.HQCode, '',
	1, @updatedBy, @updatedBy
	FROM dbo.StockInputTag tag
	INNER JOIN StockInput si on tag.Id = si.StockInputTagId
	AND tag.Id = @stockInputTagId
	AND si.Quantity > 0

	-- Create entries in StockLedger based on the stock received from vendors
	INSERT INTO dbo.StockLedger
	(TransactionDate, ItemMasterId, ReferenceNo, LineNumber, ReceiveQuantity, 
	ZoneCode, AreaCode, TerritoryCode, HQCode, StaffCode, 
	CyclicCount, CreatedBy, UpdatedBy)
	SELECT
	TransactionDate, ItemMasterId, ReferenceNo, LineNumber, Quantity, 
	ZoneCode, AreaCode, TerritoryCode, HQCode, StaffCode, 
	CyclicCount, CreatedBy, UpdatedBy
	FROM @trn


	-- Update Existing entries in StockBalance
	UPDATE dbo.StockBalance
	SET StockQuantity = b.StockQuantity + t.Quantity,
	CyclicCount = b.CyclicCount + 1,
	UpdatedBy = @updatedBy,
	DateUpdated = SysUtcDateTime()
	FROM dbo.StockBalance b
	INNER JOIN @trn t ON t.ItemMasterId = b.ItemMasterId
	AND t.ZoneCode = b.ZoneCode
	AND t.AreaCode = b.AreaCode
	AND t.TerritoryCode = b.TerritoryCode
	AND t.HQCode = b.HQCode
	AND t.StaffCode = b.StaffCode


	-- Create new entries in StockBalance
	INSERT INTO dbo.StockBalance
	(ItemMasterId, StockQuantity, 
	ZoneCode, AreaCode, TerritoryCode, HQCode, StaffCode, 
	CyclicCount, CreatedBy, UpdatedBy)
	SELECT t.ItemMasterId, t.Quantity,
	t.ZoneCode, t.AreaCode, t.TerritoryCode, t.HQCode, t.StaffCode, 
	1, @updatedBy, @updatedBy
	FROM @trn t
	LEFT JOIN dbo.StockBalance b ON t.ItemMasterId = b.ItemMasterId
	AND t.ZoneCode = b.ZoneCode
	AND t.AreaCode = b.AreaCode
	AND t.TerritoryCode = b.TerritoryCode
	AND t.HQCode = b.HQCode
	AND t.StaffCode = b.StaffCode
	WHERE b.Id IS NULL
	
END
GO

ALTER TABLE [dbo].[FeatureControl]
ADD
	[StockLedgerOption] BIT NOT NULL DEFAULT 0,
	[StockBalanceOption] BIT NOT NULL DEFAULT 0,
	[StockRemoveOption] BIT NOT NULL DEFAULT 0
GO

-- June 20 2020
--ALTER TABLE [dbo].[StockRequest]
--ADD	[Status] NVARCHAR(50) NOT NULL DEFAULT 'PendingFulfillment'

--GO

CREATE TABLE [dbo].[StockRequestFulfill]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[StockRequestId] BIGINT NOT NULL References dbo.[StockRequest],
	[StockRequestTagId] BIGINT NOT NULL References dbo.[StockRequestTag],
	[ItemMasterId] BIGINT NOT NULL REFERENCES dbo.ItemMaster,

	[StockInputId] BIGINT NOT NULL REFERENCES dbo.StockInput,

	[StockBalanceId] BIGINT NOT NULL REFERENCES dbo.StockBalance,
	[StockBalanceCyclicCount] BIGINT NOT NULL,
	[QuantityInHand] INT NOT NULL,
	[QuantityIssued] INT NOT NULL,

	[CyclicCount] BIGINT NOT NULL DEFAULT 1,
	[CreatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[UpdatedBy] NVARCHAR(50) NOT NULL DEFAULT '',
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
)
GO

/* only in tracking 
ALTER TABLE dbo.StockRequest
ADD
	[QuantityIssued] INT NOT NULL DEFAULT 0,
	[IssueDate] DATETIME2,
	[StockLedgerId] BIGINT NOT NULL DEFAULT 0
GO
*/


-- Create GRN and Request Numbers
DECLARE @n INT = 1
DECLARE @v NVARCHAR(10)
WHILE @n < 50000
BEGIN
   -- GRN Number
   IF NOT EXISTS(SELECT 1 FROM dbo.GRNNumber WHERE [Sequence] = @n)
   BEGIN
	   SET @v = CONCAT('0', @n)
	   SET @v = CONCAT('GRN', REPLICATE('0', 5-LEN(@v)), @v)

	   INSERT INTO dbo.GrnNumber
		([Sequence], GrnNumber)
		VALUES
		(@n, @v)
    END

	-- Request Number
   IF NOT EXISTS(SELECT 1 FROM dbo.RequestNumber WHERE [Sequence] = @n)
   BEGIN
	   SET @v = CONCAT('0', @n)
	   SET @v = CONCAT('QN', REPLICATE('0', 5-LEN(@v)), @v)

	   INSERT INTO dbo.RequestNumber
		([Sequence], RequestNumber)
		VALUES
		(@n, @v)
    END

	SET @n = @n + 1
END
GO

-- June 22 2020
ALTER TABLE [dbo].[StockRequestTag]
ADD	[RequestType] NVARCHAR(50) NOT NULL DEFAULT 'StockIssueRequest'
GO

-- June 28 2020
ALTER TABLE [dbo].[FeatureControl]
ADD
	[StockRemoveConfirmOption] BIT NOT NULL DEFAULT 0
GO

-- July 01 2020
ALTER TABLE [dbo].[FeatureControl]
ADD
	[StockAddOption] BIT NOT NULL DEFAULT 0,
	[StockAddConfirmOption] BIT NOT NULL DEFAULT 0
GO

-- July 02 2020
ALTER TABLE [dbo].[StockRequest]
ADD	[ReviewNotes] NVARCHAR(100) NOT NULL DEFAULT ''
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
GO
