CREATE PROCEDURE [dbo].[UpdateWorkflowStatus]
	@entityWorkflowId BIGINT,
	@currentUser NVARCHAR(50)
AS
BEGIN
	/* Called from web when status changes take place
		created by making a copy of [ProcessSqliteEntityWorkFlowDataV3]
     */

	DECLARE @entityWorkFlow TABLE 
	(
		[Id] BIGINT,
		[TagName] NVARCHAR(50) NOT NULL
	)

	DECLARE @updateTime DATETIME2 = SYSUTCDATETIME()
	----------------------------------------
	-- Step 9
	-- Find out current phase that need to be updated in parent table
	-- (This update ignores rows created as follow Up rows)
	-- Follow up activity rows does not restrict the workflow from moving to next phase/stage
	----------------------------------------
	;WITH updateRecCTE(Id, TagName, rownumber)
	AS
	(
		SELECT wfd.[EntityWorkFlowId], wfd.TagName,
		ROW_NUMBER() OVER (PARTITION BY wfd.[EntityWorkFlowId] Order By wfd.[Sequence])
		FROM dbo.EntityWorkFlowDetail wfd 
		WHERE @entityWorkflowId = wfd.EntityWorkFlowId
		AND wfd.IsComplete = 0
		AND wfd.IsFollowUpRow = 0
		AND wfd.IsActive = 1 -- ignore rows marked as inactive
	)
	INSERT INTO @entityWorkFlow
	(Id, TagName)
	SELECT Id, TagName 
	FROM updateRecCTE
	WHERE rownumber = 1
	
	----------------------------------------
	-- Step 10
	-- Now update the status values in Parent table
	-- (this only updates the rows when atleast there is one pending stage)
	----------------------------------------
	UPDATE dbo.EntityWorkFlow
	SET TagName = memwf.TagName,
	[Timestamp] = @updateTime
	FROM dbo.EntityWorkFlow wf
	INNER JOIN @entityWorkFlow memWf ON wf.Id = memWf.Id
	AND wf.TagName <> memwf.TagName


	----------------------------------------
	-- Step 11
	-- Now mark the rows as complete where phase is marked as complete in Detail table
	-- (this will happen only when all phases are complete, otherwise previous sql
	--  will have already set the phase in parent to next available phase)
	--
	-- [ This step is also repeated/duplicated in [UpdateWorkflowStatus]
	-- Aug 25 2020 ]
	----------------------------------------

	DECLARE @isComplete BIT
	SELECT @isComplete = CASE WHEN totalRows = completedRows THEN 1 ELSE 0 END
	FROM
	(
		SELECT COUNT(*) totalRows, SUM(CASE WHEN IsComplete = 1 THEN 1 ELSE 0 END) completedRows
		FROM dbo.EntityWorkFlowDetail wfd 
		WHERE wfd.EntityWorkFlowId = @entityWorkflowId
		AND wfd.IsActive = 1
	) iq

	UPDATE dbo.EntityWorkFlow
	SET IsComplete = @isComplete,
	[Timestamp] = @updateTime
	WHERE Id = @entityWorkflowId
	AND IsComplete <> @isComplete

END
GO
