CREATE PROCEDURE [dbo].[CalculateDWSOnSiloCheck]
    @strWeightId BIGINT,
	@strTagId BIGINT,
	@updatedBy NVARCHAR(50)
AS
BEGIN
    DECLARE @siloDeductPercent DECIMAL(19,2) = 0
	SELECT @siloDeductPercent = DeductionPercent
	FROM dbo.STRWeight
	WHERE Id = @strWeightId

	;With dwsCTE(Id, GoodsWeight, SiloDeductWt, RatePerKg)
	AS
	(
	  SELECT d.Id, 
	  (FilledBagsWeightKg - EmptyBagsWeightKg),
	  ((FilledBagsWeightKg - EmptyBagsWeightKg) * @siloDeductPercent) / 100.0,
	  ea.RatePerKg
	  FROM dbo.DWS d
	  INNER JOIN dbo.EntityAgreement ea on ea.Id = d.AgreementId
	  WHERE d.STRTagId = @strTagId
	)
	UPDATE dbo.DWS
	SET
	SiloDeductPercent = @siloDeductPercent,
	GoodsWeight = c.GoodsWeight,
	SiloDeductWt = c.SiloDeductWt,
	SiloDeductWtOverride = c.SiloDeductWt,
	NetPayableWt = c.GoodsWeight - c.SiloDeductWt,
	RatePerKg = c.RatePerKg,
	GoodsPrice = (c.GoodsWeight - c.SiloDeductWt) * c.RatePerKg,
	DeductAmount = 0,
	NetPayable = (c.GoodsWeight - c.SiloDeductWt) * c.RatePerKg,
	UpdatedBy = @updatedBy,
	DateUpdated = SysUTCDateTime()
	FROM dbo.DWS d
	INNER JOIN dwsCTE c on d.Id = c.Id


	UPDATE dbo.STRTag
	SET CyclicCount = CyclicCount + 1,
	STRWeightId = @strWeightId
	WHERE Id = @strTagId
END
GO
