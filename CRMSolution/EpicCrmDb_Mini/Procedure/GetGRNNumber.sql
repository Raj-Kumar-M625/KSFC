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
