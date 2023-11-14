CREATE PROCEDURE [dbo].[TransformDivisionSegmentData]
	@tenantId BIGINT,
	@IsCompleteRefresh BIT
AS
BEGIN

	  DELETE FROM dbo.CodeTable where CodeType = 'Division' and TenantId = @tenantId
	  INSERT INTO dbo.CodeTable
	  (CodeType, CodeName, CodeValue, DisplaySequence, TenantId)
	  SELECT Distinct 'Division', [Division Name], [Division Code], 10, @tenantId
	  FROM dbo.DivisionSegment

	  DELETE FROM dbo.CodeTable where CodeType = 'Segment' and TenantId = @tenantId
	  INSERT INTO dbo.CodeTable
	  (CodeType, CodeName, CodeValue, DisplaySequence, TenantId)
	  SELECT Distinct 'Segment', [Segment Name], [Segment Code], 10, @tenantId
	  FROM dbo.DivisionSegment

	  DELETE FROM dbo.CodeTable where CodeType = 'DivisionPrefix' and TenantId = @tenantId
	  INSERT INTO dbo.CodeTable
	  (CodeType, CodeName, CodeValue, DisplaySequence, TenantId)
	  SELECT Distinct 'DivisionPrefix', [Division Name], [Division Prefix], 10, @tenantId
	  FROM dbo.DivisionSegment


	  DELETE FROM dbo.[Division] WHERE tenantId = @tenantId
		INSERT INTO dbo.[Division]
		(TenantId, DivisionCode, SegmentCode)
		SELECT  
		@tenantId,
		rtrim(ltrim([Division Code])),
		rtrim(ltrim([Segment Code]))
		FROM dbo.DivisionSegment

		INSERT INTO dbo.ErrorLog
		(Process, LogText)
		SELECT 'SP:TransformDivisionSegmentData', 'Success'
END
