CREATE PROCEDURE [dbo].[ReCalculateSTRTotals]
	@strId BIGINT,
	@updatedBy NVARCHAR(50)
AS
BEGIN
	with dwsCTE(recCount, bagCount, FilledBagsWeightKg, EmptyBagsWeightKg)
	AS
	(
		SELECT 
		count(*),
		IsNull(sum(bagCount),0),
		IsNull(Sum(FilledBagsWeightKg),0),
		IsNull(Sum(EmptyBagsWeightKg),0)
		FROM dbo.DWS
		WHERE STRId = @strId
	)
	UPDATE dbo.[STR]
	SET DWSCount = cte.recCount,
	BagCount = cte.bagCount,
	GrossWeight = cte.FilledBagsWeightKg,
	NetWeight = cte.FilledBagsWeightKg - cte.EmptyBagsWeightKg,
	UpdatedBy = @updatedBy,
	DateUpdated = SysUTCDateTime()
	FROM dbo.[STR] s
	INNER JOIN dwsCTE cte ON s.Id = @strId
END
GO
