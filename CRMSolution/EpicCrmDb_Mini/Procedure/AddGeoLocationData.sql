CREATE PROCEDURE [dbo].[AddGeoLocationData]
	@employeeId BIGINT,
	@clientCode NVARCHAR(50),
	@trackingDateTime DateTime2,
	@latitude Decimal(19,9),
	@longitude Decimal(19,9),
	@geoLocationId BIGINT OUTPUT
AS
BEGIN
    SET @geoLocationId = 0

	INSERT INTO dbo.GeoLocation
	(EmployeeId, ClientCode, Latitude, Longitude, [At])
	VALUES
	(@employeeId, @clientCode, @latitude, @longitude, @trackingDateTime)

	SET @geoLocationId = SCOPE_IDENTITY()

	--make all older records for current clientCode, inactive
	
	UPDATE dbo.GeoLocation
	SET IsActive = 0
	WHERE ClientCode = @clientCode AND Id != @geoLocationId

END