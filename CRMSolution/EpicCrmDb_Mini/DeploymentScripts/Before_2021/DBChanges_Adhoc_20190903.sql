-- Sep 3 2019
-- fixing activity type in code table 
select * from dbo.codetable 
where codetype like 'activitytype'
and isactive = 1
order by displaysequence

delete from dbo.codetable where codetype like 'activitytype'
go
insert into dbo.codetable
(codetype, codevalue, DisplaySequence)
values
('ActivityType', 'Collection', 10),
('ActivityType', 'Dealer Appointment', 20),
('ActivityType', 'Demonstration', 30),
('ActivityType', 'Farmer Kit', 40),
('ActivityType', 'Field Visit', 50),
('ActivityType', 'M.D.A', 60),
('ActivityType', 'Market Research', 70),
('ActivityType', 'Meeting', 80),
('ActivityType', 'Leave Full Day', 90),
('ActivityType', 'Order Taken', 100),
('ActivityType', 'Sales', 110),
('ActivityType', 'Leave Half Day', 120),
('ActivityType', 'Stock Verification', 130),
('ActivityType', 'Other', 140)
go

-- Setting up SMS schedule
DECLARE @tenantId BIGINT
SELECT @tenantId = TenantId from dbo.Tenant

INSERT INTO dbo.TenantWorkDay
(TenantId, WeekDayName, IsWorkingDay)
values
(@tenantId, 'Monday', 1),
(@tenantId, 'Tuesday', 1),
(@tenantId, 'Wednesday', 1),
(@tenantId, 'Thursday', 1),
(@tenantId, 'Friday', 1),
(@tenantId, 'Saturday', 1),
(@tenantId, 'Sunday', 0)

insert into dbo.TenantSmsType
(TenantId, [TypeName], MessageText, SprocName, IsActive, SmsProcessClass)
values
(@tenantId, 'Start Day', 'If you are on duty, please Start Day on mobile app or else will be considered Absent.', 'SMS_GetStartDayStaffCode', 1, 'StartEndDaySms'),
(@tenantId, 'End Day', 'If you are not travelling on duty, please "End Day" on mobile app.', 'SMS_GetEndDayStaffCode', 0, 'StartEndDaySms')
;

DECLARE @smsTypeForStartDay BIGINT
SELECT @smsTypeForStartDay = id FROM dbo.TenantSMSType where [TypeName] = 'Start Day';

DECLARE @smsTypeForEndDay BIGINT
SELECT @smsTypeForEndDay = id FROM dbo.TenantSMSType where [TypeName] = 'End Day';

INSERT INTO dbo.TenantSmsSchedule
(TenantId, TenantSmsTypeId, WeekDayName, SmsAt, IsActive)
values
(@tenantId, @smsTypeForStartDay, 'Monday', '8:00', 1),
(@tenantId, @smsTypeForStartDay, 'Tuesday', '8:00', 1),
(@tenantId, @smsTypeForStartDay, 'Wednesday', '8:00', 1),
(@tenantId, @smsTypeForStartDay, 'Thursday', '8:00', 1),
(@tenantId, @smsTypeForStartDay, 'Friday', '8:00', 1),
(@tenantId, @smsTypeForStartDay, 'Saturday', '8:00', 1),

(@tenantId, @smsTypeForStartDay, 'Monday', '10:00', 1),
(@tenantId, @smsTypeForStartDay, 'Tuesday', '10:00', 1),
(@tenantId, @smsTypeForStartDay, 'Wednesday', '10:00', 1),
(@tenantId, @smsTypeForStartDay, 'Thursday', '10:00', 1),
(@tenantId, @smsTypeForStartDay, 'Friday', '10:00', 1),
(@tenantId, @smsTypeForStartDay, 'Saturday', '10:00', 1),

(@tenantId, @smsTypeForStartDay, 'Monday', '12:00', 1),
(@tenantId, @smsTypeForStartDay, 'Tuesday', '12:00', 1),
(@tenantId, @smsTypeForStartDay, 'Wednesday', '12:00', 1),
(@tenantId, @smsTypeForStartDay, 'Thursday', '12:00', 1),
(@tenantId, @smsTypeForStartDay, 'Friday', '12:00', 1),
(@tenantId, @smsTypeForStartDay, 'Saturday', '12:00', 1),

(@tenantId, @smsTypeForStartDay, 'Monday', '14:00', 1),
(@tenantId, @smsTypeForStartDay, 'Tuesday', '14:00', 1),
(@tenantId, @smsTypeForStartDay, 'Wednesday', '14:00', 1),
(@tenantId, @smsTypeForStartDay, 'Thursday', '14:00', 1),
(@tenantId, @smsTypeForStartDay, 'Friday', '14:00', 1),
(@tenantId, @smsTypeForStartDay, 'Saturday', '14:00', 1),

(@tenantId, @smsTypeForEndDay, 'Monday', '20:30', 1),
(@tenantId, @smsTypeForEndDay, 'Tuesday', '20:30', 1),
(@tenantId, @smsTypeForEndDay, 'Wednesday', '20:30', 1),
(@tenantId, @smsTypeForEndDay, 'Thursday', '20:30', 1),
(@tenantId, @smsTypeForEndDay, 'Friday', '20:30', 1),
(@tenantId, @smsTypeForEndDay, 'Saturday', '20:30', 1),
(@tenantId, @smsTypeForEndDay, 'Sunday', '20:30', 1),

(@tenantId, @smsTypeForEndDay, 'Monday', '22:00', 1),
(@tenantId, @smsTypeForEndDay, 'Tuesday', '22:00', 1),
(@tenantId, @smsTypeForEndDay, 'Wednesday', '22:00', 1),
(@tenantId, @smsTypeForEndDay, 'Thursday', '22:00', 1),
(@tenantId, @smsTypeForEndDay, 'Friday', '22:00', 1),
(@tenantId, @smsTypeForEndDay, 'Saturday', '22:00', 1),
(@tenantId, @smsTypeForEndDay, 'Sunday', '22:00', 1)
GO
