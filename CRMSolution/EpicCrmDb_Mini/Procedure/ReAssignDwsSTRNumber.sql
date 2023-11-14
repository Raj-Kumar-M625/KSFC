CREATE PROCEDURE [dbo].[ReAssignDwsSTRNumber]
	@dwsId BIGINT,
	@fromStrTagId BIGINT,
	@toStrTagId BIGINT,
	@updatedBy NVARCHAR(50)
AS
BEGIN
    -- find out original employeeId
	DECLARE @empId BIGINT
	DECLARE @fromStrId BIGINT
	SELECT @empId = s.EmployeeId,
	@fromSTrId = d.StrId
	FROM dbo.DWS d
	INNER JOIN dbo.[STR] s on d.STRId = s.Id
	AND d.Id = @dwsId

	-- Now find out STRId for the empId that is with @toStrTagId
	DECLARE @toStrId BIGINT
	SELECT @toStrId = Id
	FROM dbo.[STR] s
	WHERE s.StrTagId = @toSTrTagId
	AND s.EmployeeId = @empId

	-- we have to put the DWS in the toStrTag
	-- where Str record belongs to the original user.

	IF @toStrId > 0
	BEGIN
		UPDATE dbo.DWS
		SET STRTagId = @toStrTagId,
		STRId = @toStrId,
		UpdatedBy = @updatedBy,
		DateUpdated = SysUTCDateTime()
		WHERE STRTagId = @fromSTRTagId
		AND Id = @dwsId

		IF @fromSTRTagId <> @toStrTagId
		BEGIN
			UPDATE dbo.STRTag
			SET CyclicCount = CyclicCount + 1
			WHERE ID IN (@toSTRTagId, @fromSTRTagId)
		END

		EXEC dbo.RecalculateSTRTotals @fromStrId, @updatedBy
		EXEC dbo.RecalculateSTRTotals @toStrId, @updatedBy
	END
END
GO
