CREATE PROCEDURE [dbo].[SetDWSStatus]
	@strTagId BIGINT,
	@toDWSStatus NVARCHAR(50),
	@updatedBy NVARCHAR(50)
AS
BEGIN
	UPDATE dbo.DWS
	SET 
	[Status] = @toDWSStatus,
	UpdatedBy = @updatedBy,
	DateUpdated = SysUTCDateTime()
	WHERE STRTagId = @strTagId

	UPDATE dbo.STRTag
	SET CyclicCount = CyclicCount + 1
	WHERE Id = @strTagId
END
GO
