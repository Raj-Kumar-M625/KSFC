alter table dbo.[Order]
ADD [IsCustomerSMSSent] BIT NOT NULL DEFAULT 0,
    [IsAgentSMSSent] BIT NOT NULL DEFAULT 0,
	[LockTimestamp] DATETIME2 NULL
Go

ALTER TABLE dbo.[Tenant]
ADD	[IsSendingSMS] BIT NOT NULL DEFAULT 0,
	[SMSProcessingAt] DATETIME2
go


UPDATE dbo.[Order] SET IsCustomerSMSSent = 1, IsAgentSMSSent = 1
go

ALTER TABLE [dbo].[TenantSmsType] ALTER COLUMN [MessageText] NVARCHAR (1024) NOT NULL;
go

insert into dbo.TenantSmsType
(tenantId, TypeName, MessageText, SprocName)
values
(1, 'OrderSmsToCustomer', 'Thank you for placing order No.{OrderNumber} / {OrderDate}, we will ship your order soon.<NL>Order (Rs.): {OrderTotal}* (*Prices are subject to change)', ''),
(1, 'OrderSmsToAgent', 'The order No. {OrderNumber} is received on {OrderDate} from {CustomerName} ({CustomerCode}).<NL>Order (Rs.): {OrderTotal}', ''),
(1, 'EODSmsToAgent', '', '')

go

-- wrap it in if exist
DROP INDEX [IX_Tracking_DistanceCalculated] on dbo.Tracking
go

CREATE INDEX [IX_Tracking_DistanceCalculated]
	ON [dbo].[Tracking]
	([DistanceCalculated])
	INCLUDE ([LockTimestamp])

go

CREATE TABLE [dbo].[SMSProcessLog]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[TenantId] BIGINT NOT NULL,
	[LockAcquiredStatus] BIT NOT NULL,
	[HasCompleted] BIT NOT NULL DEFAULT 0,
	[HasFailed] BIT NOT NULL DEFAULT 0,
	[At] DATETIME2 NOT NULL DEFAULT SysutcDateTime(),
	[Timestamp] DATETIME2 NOT NULL DEFAULT SysutcDateTime()
)
go

CREATE PROCEDURE [dbo].[GetOrderForCustomerSMS]
	@recordCount int,
	@tenantId BIGINT,
	@employeeId BIGINT
AS
BEGIN
    DECLARE @currentDateTime DATETIME2 = SYSUTCDATETIME();

	;WITH batchCTE(orderId)
	AS
	(
		SELECT TOP(@recordCount) o.Id  -- order Id
		FROM dbo.[Order] o WITH (NOLOCK)
		INNER JOIN dbo.TenantEmployee t ON t.Id = o.EmployeeId
		WHERE t.TenantId = @tenantId
		AND (@employeeId = -1 OR t.Id = @employeeId)
		AND o.LockTimestamp IS NULL
		AND o.IsCustomerSMSSent = 0
		ORDER BY o.Id
	)
	-- Lock the selected batch records
	UPDATE dbo.[Order]
	SET LockTimestamp = @currentDateTime
	OUTPUT inserted.Id OrderId
	FROM dbo.[Order] b
	INNER JOIN batchCTE cte ON b.Id = cte.orderId
END
go

CREATE PROCEDURE [dbo].[GetOrderForAgentSMS]
	@recordCount int,
	@tenantId BIGINT,
	@employeeId BIGINT
AS
BEGIN
    DECLARE @currentDateTime DATETIME2 = SYSUTCDATETIME();

	;WITH batchCTE(orderId)
	AS
	(
		SELECT TOP(@recordCount) o.Id  -- order Id
		FROM dbo.[Order] o WITH (NOLOCK)
		INNER JOIN dbo.TenantEmployee t ON t.Id = o.EmployeeId
		WHERE t.TenantId = @tenantId
		AND (@employeeId = -1 OR t.Id = @employeeId)
		AND o.LockTimestamp IS NULL
		AND o.IsAgentSMSSent = 0

		ORDER BY o.Id
	)
	-- Lock the selected batch records
	UPDATE dbo.[Order]
	SET LockTimestamp = @currentDateTime
	OUTPUT inserted.Id OrderId
	FROM dbo.[Order] b
	INNER JOIN batchCTE cte ON b.Id = cte.orderId
END
go


ALTER TABLE dbo.TenantSmsType
Add [SmsProcessClass] NVARCHAR(100) NOT NULL DEFAULT ''
go

UPDATE dbo.TenantSmsType
SET SmsProcessClass = 'StartEndDaySms' WHERE TypeName = 'Start Day' OR TypeName = 'End Day'

UPDATE dbo.TenantSmsType
SET SmsProcessClass = 'OrderCustomerSms' WHERE TypeName = 'OrderSmsToCustomer'

UPDATE dbo.TenantSmsType
SET SmsProcessClass = 'OrderAgentSms' WHERE TypeName = 'OrderSmsToAgent'

UPDATE dbo.TenantSmsType
SET SmsProcessClass = 'EndDayAgentSms' WHERE TypeName = 'EODSmsToAgent'

UPDATE dbo.TenantSmsType
SET MessageText =
'{SmsDate} Summary<NL>Time: {WorkingHrs}<NL>Activity(No): {ActivityCount}<NL>Order Rs.{OrderTotal}<NL>Payment Rs.{PaymentTotal}<NL>Expenses Rs.{ExpenseTotal}<NL>Trv {TravelKm} km'
WHERE TypeName = 'EODSmsToAgent'

go

declare @tenantSmsTypeId BIGINT
SELECT @tenantSmsTypeId = id FROM dbo.TenantSmsType where TypeName = 'EODSmsToAgent'
INSERT INTO dbo.tenantSmsSchedule
(tenantId, TenantSmsTypeId, WeekDayName, SmsAt)
values
(1, @tenantSmsTypeId, 'Monday', '21:00:00'),
(1, @tenantSmsTypeId, 'Tuesday', '21:00:00'),
(1, @tenantSmsTypeId, 'Wednesday', '21:00:00'),
(1, @tenantSmsTypeId, 'Thursday', '21:00:00'),
(1, @tenantSmsTypeId, 'Friday', '21:00:00'),
(1, @tenantSmsTypeId, 'Saturday', '21:00:00'),
(1, @tenantSmsTypeId, 'Sunday', '21:00:00')
