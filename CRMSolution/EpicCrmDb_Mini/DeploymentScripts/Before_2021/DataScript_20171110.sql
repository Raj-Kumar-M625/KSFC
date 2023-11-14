delete from dbo.TenantWorkDay

INSERT INTO dbo.TenantWorkDay
(TenantId, WeekDayName, IsWorkingDay)
values
(1, 'Monday', 1),
(1, 'Tuesday', 1),
(1, 'Wednesday', 1),
(1, 'Thursday', 1),
(1, 'Friday', 1),
(1, 'Saturday', 1),
(1, 'Sunday', 0)


delete from dbo.TenantSMSSchedule

DECLARE @dayName VARCHAR(10)
set @dayName = 'Monday'
INSERT INTO dbo.TenantSMSSchedule
( TenantId, WeekDayName, SMSAt)
values
(1, @dayName, '09:00'),
(1, @dayName, '11:00'),
(1, @dayName, '13:00'),
(1, @dayName, '15:00')

set @dayName = 'Tuesday'
INSERT INTO dbo.TenantSMSSchedule
( TenantId, WeekDayName, SMSAt)
values
(1, @dayName, '09:00'),
(1, @dayName, '11:00'),
(1, @dayName, '13:00'),
(1, @dayName, '15:00')

set @dayName = 'Wednesday'
INSERT INTO dbo.TenantSMSSchedule
( TenantId, WeekDayName, SMSAt)
values
(1, @dayName, '09:00'),
(1, @dayName, '11:00'),
(1, @dayName, '13:00'),
(1, @dayName, '15:00')

set @dayName = 'Thursday'
INSERT INTO dbo.TenantSMSSchedule
( TenantId, WeekDayName, SMSAt)
values
(1, @dayName, '09:00'),
(1, @dayName, '11:00'),
(1, @dayName, '13:00'),
(1, @dayName, '15:00')

set @dayName = 'Friday'
INSERT INTO dbo.TenantSMSSchedule
( TenantId, WeekDayName, SMSAt)
values
(1, @dayName, '09:00'),
(1, @dayName, '11:00'),
(1, @dayName, '13:00'),
(1, @dayName, '15:00')

set @dayName = 'Saturday'
INSERT INTO dbo.TenantSMSSchedule
( TenantId, WeekDayName, SMSAt)
values
(1, @dayName, '09:00'),
(1, @dayName, '11:00'),
(1, @dayName, '13:00'),
(1, @dayName, '15:00')
