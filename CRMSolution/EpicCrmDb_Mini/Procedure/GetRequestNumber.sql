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
