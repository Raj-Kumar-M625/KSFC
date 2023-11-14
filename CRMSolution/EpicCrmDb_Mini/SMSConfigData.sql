-- Mar 18 2018

insert into dbo.TenantSmsType
(TenantId, [TypeName], MessageText, SprocName, IsActive)
values
(1, 'Start Day', 'If you are on duty, please Start Day on mobile app or else will be considered Absent.', 'SMS_GetStartDayStaffCode', 1),
(1, 'End Day', 'If you are not travelling on duty, please "End Day" on mobile app.', 'SMS_GetEndDayStaffCode', 1)
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

(1, @smsTypeForEndDay, 'Monday', '19:00', 1),
(1, @smsTypeForEndDay, 'Tuesday', '19:00', 1),
(1, @smsTypeForEndDay, 'Wednesday', '19:00', 1),
(1, @smsTypeForEndDay, 'Thursday', '19:00', 1),
(1, @smsTypeForEndDay, 'Friday', '19:00', 1),
(1, @smsTypeForEndDay, 'Saturday', '19:00', 1)

