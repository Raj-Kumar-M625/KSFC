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
