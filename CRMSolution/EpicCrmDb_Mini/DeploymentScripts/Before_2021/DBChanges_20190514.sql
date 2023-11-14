-- 14.5.2019

INSERT INTO dbo.TenantWorkDay
(TenantId, WeekDayName, IsWorkingDay)
values
(1, 'Monday', 1),
(1, 'Tuesday', 1),
(1, 'Wednesday', 1),
(1, 'Thursday', 1),
(1, 'Friday', 1),
(1, 'Saturday', 1),
(1, 'Sunday', 1)
go

insert into dbo.TenantSmsType
(TenantId, [TypeName], MessageText, SprocName, IsActive, SmsProcessClass)
values
(1, 'Start Day', 'If you are on duty, please Start Day on mobile app or else will be considered Absent.', 'SMS_GetStartDayStaffCode', 1, 'StartEndDaySms'),
(1, 'End Day', 'If you are not travelling on duty, please "End Day" on mobile app.', 'SMS_GetEndDayStaffCode', 1, 'StartEndDaySms')
;

DECLARE @smsTypeForStartDay BIGINT
SELECT @smsTypeForStartDay = id FROM dbo.TenantSMSType where [TypeName] = 'Start Day';

DECLARE @smsTypeForEndDay BIGINT
SELECT @smsTypeForEndDay = id FROM dbo.TenantSMSType where [TypeName] = 'End Day';

INSERT INTO dbo.TenantSmsSchedule
(TenantId, TenantSmsTypeId, WeekDayName, SmsAt, IsActive)
values
(1, @smsTypeForStartDay, 'Monday', '10:00', 1),
(1, @smsTypeForStartDay, 'Tuesday', '10:00', 1),
(1, @smsTypeForStartDay, 'Wednesday', '10:00', 1),
(1, @smsTypeForStartDay, 'Thursday', '10:00', 1),
(1, @smsTypeForStartDay, 'Friday', '10:00', 1),
(1, @smsTypeForStartDay, 'Saturday', '10:00', 1),
(1, @smsTypeForStartDay, 'Sunday', '10:00', 1),

(1, @smsTypeForEndDay, 'Monday', '20:30', 1),
(1, @smsTypeForEndDay, 'Tuesday', '20:30', 1),
(1, @smsTypeForEndDay, 'Wednesday', '20:30', 1),
(1, @smsTypeForEndDay, 'Thursday', '20:30', 1),
(1, @smsTypeForEndDay, 'Friday', '20:30', 1),
(1, @smsTypeForEndDay, 'Saturday', '20:30', 1),
(1, @smsTypeForEndDay, 'Sunday', '20:30', 1),

(1, @smsTypeForEndDay, 'Monday', '22:00', 1),
(1, @smsTypeForEndDay, 'Tuesday', '22:00', 1),
(1, @smsTypeForEndDay, 'Wednesday', '22:00', 1),
(1, @smsTypeForEndDay, 'Thursday', '22:00', 1),
(1, @smsTypeForEndDay, 'Friday', '22:00', 1),
(1, @smsTypeForEndDay, 'Saturday', '22:00', 1),
(1, @smsTypeForEndDay, 'Sunday', '22:00', 1)
GO

ALTER PROCEDURE [dbo].[SMS_GetStartDayStaffCode]
	@tenantId BIGINT,
	@runDate DATE
AS
BEGIN
	-- first create a row in [Day] table, if not already there
	IF NOT EXISTS( SELECT 1 FROM dbo.[Day] WHERE [Date] = @runDate )
	BEGIN
		INSERT INTO dbo.[Day] ([DATE])
		SELECT @runDate
	END

	-- get the list of staff codes who did not start their day
	SELECT te.EmployeeCode
	FROM dbo.TenantEmployee te
	INNER JOIN dbo.SalesPerson sp ON te.EmployeeCode = sp.StaffCode
	      AND te.IsActive = 1 
	      AND sp.IsActive = 1
		  AND te.TenantId = @tenantId
	INNER JOIN dbo.[Day] d ON d.[Date] = @runDate
	LEFT JOIN dbo.EmployeeDay ed ON te.Id = ed.TenantEmployeeId
	      AND ed.DayId = d.Id
		  AND ed.AppVersion <> '***'

	-- if I continued through mid night, and created activity after midnight
	-- I get an entry in employee day with app version = '***'
	-- this should not be treated as if I have started my day

	WHERE ed.TenantEmployeeId is null
END
GO

ALTER PROCEDURE [dbo].[SMS_GetEndDayStaffCode]
	@tenantId BIGINT,
	@runDate DATE
AS
BEGIN
	-- first create a row in [Day] table, if not already there
	IF NOT EXISTS( SELECT 1 FROM dbo.[Day] WHERE [Date] = @runDate )
	BEGIN
		INSERT INTO dbo.[Day] ([DATE])
		SELECT @runDate
	END

	-- get the list of staff codes who started the day but did not end it
	SELECT te.EmployeeCode
	FROM dbo.TenantEmployee te
	INNER JOIN dbo.SalesPerson sp ON te.EmployeeCode = sp.StaffCode
	      AND te.IsActive = 1 
	      AND sp.IsActive = 1
		  AND te.TenantId = @tenantId
	INNER JOIN dbo.[Day] d ON d.[Date] = @runDate
	INNER JOIN dbo.EmployeeDay ed ON te.Id = ed.TenantEmployeeId
	      AND ed.DayId = d.Id
		  AND ed.EndTime IS NULL
END
GO
