CREATE TABLE [dbo].[SqliteActionBatch]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[BatchGuid] NVARCHAR(50) NOT NULL DEFAULT '',  -- this is coming from phone
	[EmployeeId] BIGINT NOT NULL,
	[BatchInputCount] BIGINT NOT NULL,
	[BatchSavedCount] BIGINT NOT NULL,
	[DuplicateItemCount] BIGINT NOT NULL DEFAULT 0,

	[ExpenseLineInputCount] BIGINT NOT NULL DEFAULT 0,
	[ExpenseLineSavedCount] BIGINT NOT NULL DEFAULT 0,
	[ExpenseLineRejectCount] BIGINT NOT NULL DEFAULT 0,
	[TotalExpenseAmount] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[ExpenseDate] DATETIME2,
	[ExpenseId] BIGINT NOT NULL DEFAULT 0,
	[DuplicateExpenseCount] BIGINT NOT NULL DEFAULT 0,

	[NumberOfOrders] BIGINT NOT NULL DEFAULT 0,
	[NumberOfOrderLines] BIGINT NOT NULL DEFAULT 0,
	[TotalOrderAmount] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[NumberOfOrdersSaved] BIGINT NOT NULL DEFAULT 0,
	[NumberOfOrderLinesSaved] BIGINT NOT NULL DEFAULT 0,
	[DuplicateOrderCount] BIGINT NOT NULL DEFAULT 0,

	[NumberOfReturns] BIGINT NOT NULL DEFAULT 0,
	[NumberOfReturnLines] BIGINT NOT NULL DEFAULT 0,
	[TotalReturnAmount] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[NumberOfReturnsSaved] BIGINT NOT NULL DEFAULT 0,
	[NumberOfReturnLinesSaved] BIGINT NOT NULL DEFAULT 0,
	[DuplicateReturnCount] BIGINT NOT NULL DEFAULT 0,

	[NumberOfPayments] BIGINT NOT NULL DEFAULT 0,
	[TotalPaymentAmount] DECIMAL(19,2) NOT NULL DEFAULT 0,
	[NumberOfPaymentsSaved] BIGINT NOT NULL DEFAULT 0,
	[DuplicatePaymentCount] BIGINT NOT NULL DEFAULT 0,

	[NumberOfEntities] BIGINT NOT NULL DEFAULT 0,
	[NumberOfEntitiesSaved] BIGINT NOT NULL DEFAULT 0,
	[DuplicateEntityCount] BIGINT NOT NULL DEFAULT 0,

	[NumberOfLeaves] BIGINT NOT NULL DEFAULT 0,
	[NumberOfLeavesSaved] BIGINT NOT NULL DEFAULT 0,

	[NumberOfCancelledLeaves] BIGINT NOT NULL DEFAULT 0,
	[NumberOfCancelledLeavesSaved] BIGINT NOT NULL DEFAULT 0,

	[DeviceLogCount] BIGINT NOT NULL DEFAULT 0,

	[NumberOfIssueReturns] BIGINT NOT NULL DEFAULT 0,
	[NumberOfIssueReturnsSaved] BIGINT NOT NULL DEFAULT 0,

	[NumberOfWorkFlow] BIGINT NOT NULL DEFAULT 0,
	[NumberOfWorkFlowSaved] BIGINT NOT NULL DEFAULT 0,

	[NumberOfImages] BIGINT NOT NULL DEFAULT 0,
	[NumberOfImagesSaved] BIGINT NOT NULL DEFAULT 0,

	[Agreements] BIGINT NOT NULL DEFAULT 0,
	[AgreementsSaved] BIGINT NOT NULL DEFAULT 0,

	[AdvanceRequests] BIGINT NOT NULL DEFAULT 0,
	[AdvanceRequestsSaved] BIGINT NOT NULL DEFAULT 0,

	[TerminateRequests] BIGINT NOT NULL DEFAULT 0,
	[TerminateRequestsSaved] BIGINT NOT NULL DEFAULT 0,

	[BankDetails] BIGINT NOT NULL DEFAULT 0,
	[BankDetailsSaved] BIGINT NOT NULL DEFAULT 0,

	[STRCount] BIGINT NOT NULL DEFAULT 0,
	[STRSavedCount] BIGINT NOT NULL DEFAULT 0,

	[Surveys] BIGINT NOT NULL DEFAULT 0,
	[SurveysSaved] BIGINT NOT NULL DEFAULT 0,

	[DayPlanTarget] BIGINT NOT NULL DEFAULT 0,
	[DayPlanTargetSaved] BIGINT NOT NULL DEFAULT 0,

	[QuestionnaireTarget] [bigint] NOT NULL DEFAULT 0,
	[QuestionnaireTargetSaved] [bigint] NOT NULL DEFAULT 0,

	[Task] BIGINT NOT NULL DEFAULT 0,
	[TaskSaved] BIGINT NOT NULL DEFAULT 0,
	[TaskAction] BIGINT NOT NULL DEFAULT 0,
	[TaskActionSaved] BIGINT NOT NULL DEFAULT 0,

	[At] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[Timestamp] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

	[BatchProcessed] BIT NOT NULL DEFAULT 0,
	[UnderConstruction] BIT NOT NULL DEFAULT 0,
	[LockTimestamp] DATETIME2,

	[DataFileName] NVARCHAR(128) NOT NULL DEFAULT '',
	[DataFileSize] BIGINT NOT NULL DEFAULT 0
)
