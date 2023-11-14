CREATE PROCEDURE [dbo].[ReAssignSTRNumber]
	@strId BIGINT,
	@fromStrTagId BIGINT,
	@toStrTagId BIGINT,
	@updatedBy NVARCHAR(50)
AS
BEGIN
	UPDATE dbo.DWS
	SET STRTagId = @toStrTagId,
	UpdatedBy = @updatedBy,
	DateUpdated = SysUTCDateTime()
	WHERE STRTagId = @fromSTRTagId
	AND STRId = @strId

	UPDATE dbo.[STR]
	SET STRTagId = @toStrTagId,
	UpdatedBy = @updatedBy,
	DateUpdated = SysUTCDateTime()
	WHERE STRTagId = @fromSTRTagId
	AND Id = @strId
END
GO
