CREATE TABLE [dbo].[FeatureControl]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity,
	[UserName] NVARCHAR(50) NOT NULL,
	[ActivityFeature] BIT NOT NULL DEFAULT 0,
	[OrderFeature] BIT NOT NULL DEFAULT 0,
	[PaymentFeature] BIT NOT NULL DEFAULT 0,
	[OrderReturnFeature] BIT NOT NULL DEFAULT 0,
	[ExpenseFeature] BIT NOT NULL DEFAULT 0,
	[IssueReturnFeature] BIT NOT NULL DEFAULT 0,
	[STRFeature] BIT NOT NULL DEFAULT 0,

	[ExpenseReportFeature] BIT NOT NULL DEFAULT 0,
	[FieldActivityReportFeature] BIT NOT NULL DEFAULT 0,
	[EntityProgressReportFeature] BIT NOT NULL DEFAULT 0,
	[AttendanceReportFeature] BIT NOT NULL DEFAULT 0,
	[AttendanceSummaryReportFeature] BIT NOT NULL DEFAULT 0,
	[AbsenteeReportFeature] BIT NOT NULL DEFAULT 0,
	[AppSignUpReportFeature] BIT  NOT NULL DEFAULT 0,
	[AppSignInReportFeature] BIT  NOT NULL DEFAULT 0,
	[ActivityReportFeature] BIT  NOT NULL DEFAULT 0,
	[ActivityByTypeReportFeature] BIT  NOT NULL DEFAULT 0,
	[KPIFeature] BIT NOT NULL DEFAULT 0,
	[MAPFeature] BIT  NOT NULL DEFAULT 0,
	[WrongLocationReport] BIT NOT NULL DEFAULT 0,
	[DWSPaymentReport] BIT NOT NULL DEFAULT 0,
	[TransporterPaymentReport] BIT NOT NULL DEFAULT 0,

	[SalesPersonFeature] BIT NOT NULL DEFAULT 0,
	[CustomerFeature] BIT NOT NULL DEFAULT 0,
	[ProductFeature] BIT NOT NULL DEFAULT 0,
	[CrmUserFeature] BIT NOT NULL DEFAULT 0,
	[AssignmentFeature] BIT NOT NULL DEFAULT 0,
	[UploadDataFeature] BIT NOT NULL DEFAULT 0,
	[OfficeHierarchyFeature] BIT NOT NULL DEFAULT 0,
	[BankAccountFeature] BIT NOT NULL DEFAULT 0,
	[EntityFeature] BIT NOT NULL DEFAULT 0,
	[GstRateFeature]  BIT NOT NULL DEFAULT 0,
	[WorkFlowReportFeature] BIT NOT NULL DEFAULT 0,
	[RedFarmerModule] BIT NOT NULL DEFAULT 0,

	[AdvanceRequestModule] BIT NOT NULL DEFAULT 0,


	[EmployeeExpenseReport] BIT NOT NULL DEFAULT 0,
	[DistanceReport] BIT NOT NULL DEFAULT 0,

	[EmployeeExpenseReport2] BIT NOT NULL DEFAULT 0,
	[UnSownReport] BIT NOT NULL DEFAULT 0,
	[IsReadOnlyUser] BIT NOT NULL DEFAULT 0,

	[STRWeighControl] BIT NOT NULL DEFAULT 0,
	[STRSiloControl] BIT NOT NULL DEFAULT 0,  -- not used


	[DWSApproveWeightOption] BIT NOT NULL DEFAULT 0,
	[DWSApproveAmountOption] BIT NOT NULL DEFAULT 0,
	[DWSPaymentOption] BIT NOT NULL DEFAULT 0,

	-- June 10 2020
	[StockReceiveOption] BIT NOT NULL DEFAULT 0,
	[StockReceiveConfirmOption] BIT NOT NULL DEFAULT 0,
	[StockRequestOption] BIT NOT NULL DEFAULT 0,
	[StockRequestFulfillOption] BIT NOT NULL DEFAULT 0,
	[ExtraAdminOption] BIT NOT NULL DEFAULT 0,

	[StockLedgerOption] BIT NOT NULL DEFAULT 0,
	[StockBalanceOption] BIT NOT NULL DEFAULT 0,
	[StockRemoveOption] BIT NOT NULL DEFAULT 0,
	[StockRemoveConfirmOption] BIT NOT NULL DEFAULT 0,

	[StockAddOption] BIT NOT NULL DEFAULT 0,
	[StockAddConfirmOption] BIT NOT NULL DEFAULT 0,

	[BonusCalculationPaymentOption] BIT NOT NULL DEFAULT 0,
	[SurveyFormReport] BIT NOT NULL DEFAULT 0,
	[DayPlanReport] BIT NOT NULL DEFAULT 0,
	[QuestionnaireFeature] BIT NOT NULL DEFAULT 0,
	[ProjectOption] BIT NOT NULL DEFAULT 0,
	[FollowUpTaskOption] BIT NOT NULL DEFAULT 0,
	[FarmerSummaryReport] BIT NOT NULL DEFAULT 0,
	[DealersNotMetReport] BIT NOT NULL DEFAULT 0,
	[DealersSummaryReport] BIT NOT NULL DEFAULT 0,
	[GeoTaggingReport] BIT NOT NULL DEFAULT 0,

	[AgreementsReport] BIT NOT NULL DEFAULT 0,
	[DuplicateFarmersReport] BIT NOT NULL DEFAULT 0,
	[FarmersBankAccountReport] BIT NOT NULL DEFAULT 0,
	-- super Admin area
	[SuperAdminPage] BIT NOT NULL DEFAULT 0,

	-- to show the data only as per security context of staff code given here.
	[SecurityContextUser] NVARCHAR(20) NOT NULL DEFAULT '',
	
	[DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	[DateUpdated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
)
