using BusinessLayer;
using CRMUtilities;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace EpicCrmWebApi
{
    [Authorize(Roles = "Manager")]
    public class DashboardController : BaseDashboardController
    {
        private static string OrderType = DomainEntities.Constant.OrderType;
        private static string ReturnType = DomainEntities.Constant.ReturnType;
        private static string PaymentType = DomainEntities.Constant.PaymentType;
        private static string ExpenseType = DomainEntities.Constant.ExpenseType;
        private static string IssueReturnType = DomainEntities.Constant.IssueReturnType;
        private static string ExpenseReport = DomainEntities.Constant.ExpenseReport;

        private static string EmployeeExpenseReport = DomainEntities.Constant.EmployeeExpenseReport;  // pjmargo
        //private static string EmployeeExpenseReport2 = DomainEntities.Constant.EmployeeExpenseReport2;  // heranba

        private static string DistanceReport = DomainEntities.Constant.DistanceReport;

        private static string AttendanceReport = DomainEntities.Constant.AttendanceReport;
        private static string AttendanceSummaryReport = DomainEntities.Constant.AttendanceSummaryReport;
        private static string AttendanceRegister = Constant.AttendanceRegister;
        private static string AbsenteeReport = DomainEntities.Constant.AbsenteeReport;
        private static string ActivityType = DomainEntities.Constant.ActivityType;
        private static string ActivityReport = DomainEntities.Constant.ActivityReport;
        private static string AppSignUpReport = DomainEntities.Constant.AppSignUpReport;
        private static string AppSignInReport = DomainEntities.Constant.AppSignInReport;
        private static string ActivityByTypeReport = DomainEntities.Constant.ActivityByTypeReport;
        private static string EntityWorkFlowReport = DomainEntities.Constant.EntityWorkFlowReport;
        private static string EntityProgressReport = DomainEntities.Constant.EntityProgressReport;
        private static string QuestionnaireType = DomainEntities.Constant.QuestionnaireType;
        private static string FarmerSummaryReport = DomainEntities.Constant.FarmerSummaryReport;

        private static string DealersSummaryReport = DomainEntities.Constant.DealersSummaryReport;

        //// Author:Kartik; Purpose: Geolife FollowUpTask; Date: 23/09/2021
        //private static string FollowUpTaskType = DomainEntities.Constant.FollowUpTaskType;
        // also called Wrong Location Report
        private static string DistantActivityReport = DomainEntities.Constant.DistantActivityReport;

        //private static string RedFarmerModule = DomainEntities.Constant.RedFarmerModule;
        //private static string AdvanceRequestReport = DomainEntities.Constant.AdvanceRequest;

        // Author:PankajKumar; Purpose: Geolife Day Planning; Date: 28/04/2021
        private static string DayPlanReport = DomainEntities.Constant.DayPlannningReport;

        private const string HtmlResultType = "Html";
        private const string PdfResultType = "Pdf";

        //Author: Venkatesh; Purpose: GeoLife Dealers Not Met
        //US:01

        private static string DealersNotMetReport = DomainEntities.Constant.DealersNotMetReport;

        private static string GeoTagReport = DomainEntities.Constant.GeoTagReport;

        private static string Agreements = DomainEntities.Constant.Agreements;
        private static string DuplicateFarmersReport = DomainEntities.Constant.DuplicateFarmersReport;
        private static string FarmersBankAccountReport = DomainEntities.Constant.FarmersBankAccountReport;
        // GET: Main Dashboard Page
        public ActionResult Index()
        {
            // Get rights
            PutFeatureSetInViewBag();

            return View();
        }

        private ActionResult GetExpenseItems(long expenseId)
        {
            Expense expenseRecord = Business.GetExpense(expenseId);
            IEnumerable<ExpenseItem> expenseItems = Business.GetExpenseItems(expenseId);
            ICollection<ExpenseApproval> expenseApprovals = Business.GetExpenseApprovals(expenseId);

            ModelExpense model = new ModelExpense()
            {
                Id = expenseRecord.Id,
                EmployeeId = expenseRecord.EmployeeId,
                EmployeeCode = expenseRecord.EmployeeCode,
                EmployeeName = expenseRecord.EmployeeName,
                DayId = expenseRecord.DayId,
                ExpenseDate = expenseRecord.ExpenseDate,
                TotalAmount = expenseRecord.TotalAmount,
                IsZoneApproved = expenseRecord.IsZoneApproved,
                IsAreaApproved = expenseRecord.IsAreaApproved,
                IsTerritoryApproved = expenseRecord.IsTerritoryApproved
            };

            model.Items = expenseItems.Select(x => new ModelExpenseItem()
            {
                Id = x.Id,
                ExpenseId = x.ExpenseId,
                SequenceNumber = x.SequenceNumber,
                Amount = x.Amount,
                DeductedAmount = x.DeductedAmount,
                RevisedAmount = x.RevisedAmount,
                ExpenseType = x.ExpenseType,
                OdometerStart = x.OdometerStart,
                OdometerEnd = x.OdometerEnd,
                TransportType = x.TransportType,
                ImageCount = x.ImageCount,
                FuelType = x.FuelType,
                FuelQuantityInLiters = x.FuelQuantityInLiters,
                Comment = x.Comment
            }).ToList();

            model.Approvals = expenseApprovals.Select(x => new ModelExpenseApproval()
            {
                Id = x.Id,
                ExpenseId = x.ExpenseId,
                ApproveLevel = x.ApproveLevel,
                ApproveDate = x.ApproveDate,
                ApproveRef = x.ApproveRef,
                ApproveNotes = x.ApproveNotes,
                ApproveAmount = x.ApproveAmount,
                ApprovedBy = x.ApprovedBy
            })
            .OrderBy(x => x.ApproveDate)
            .ToList();

            EmployeeDayData employeeDayData = Business.GetEmployeeDayData(expenseRecord.EmployeeId, expenseRecord.DayId);

            model.EmployeeDayId = employeeDayData.EmployeeDayId;
            model.ActivityCount = Business.GetActivityCount(employeeDayData.EmployeeDayId);

            return PartialView("_ShowExpenseItems", model);
        }

        /// <summary>
        /// Show Search renders/builds reports for Absentee Report, AppSignUp Report, AppSignIn Report,
        /// Activity Report, ActivityByType Report, KPI, MAP, Employee Expense Report,
        /// Distance Report, Wrong Location Report, Day Planning Report
        /// Modified By:PankajKumar; Purpose: Added Day Planning Report; Date: 28/04/2021
        /// </summary>
        /// <param name="reportType"></param>
        /// <returns></returns>
        [CheckRightsActionFilter]
        public ActionResult ShowSearch(string reportType, string EmployeeCode, string GeoLocationStatus, string EmployeeStatus)  // don't change this parameter name
        {
            IEnumerable<OfficeHierarchy> officeHierarchy = GetOfficeHierarchy();
            ModelFilter modelFilter = new ModelFilter()
            {
                ActivityType = Business.GetActivityTypes(),
                ClientType = Business.GetCodeTable("CustomerType"),
                Departments = Business.GetCodeTable("Department"),
                Designations = Business.GetCodeTable("Designation"),
                DayPlanTypes = Business.GetCodeTable("DayPlanType"),
                TargetStatuses = Business.GetCodeTable("TargetStatus"),
                WorkFlowPhases = Business.GetWorkFlowSchedule()
                .GroupBy(x => x.Phase)
                .Select(x => new DomainEntities.CodeTableEx()
                {
                    CodeName = x.Key,
                    Code = x.Key,
                    DisplaySequence = x.First().Sequence  // view is going to sort it by name anyway
                }).ToList(),

                AgreementStatus = Business.GetCodeTable("AgreementStatus"),
                ActiveCrops = Business.GetActiveCrops(),
                EmployeeCode = String.IsNullOrEmpty(EmployeeCode) ? "" : EmployeeCode,
                GeolocationType = String.IsNullOrEmpty(GeoLocationStatus) ? "" : GeoLocationStatus,
                EmployeeStatus = String.IsNullOrEmpty(EmployeeStatus) ? "" : (EmployeeStatus == "True" ? "1" : "0"),
                BankDetailStatus = Business.GetCodeTable("BankDetailStatus"),
            };


            // default to order
            if (String.IsNullOrEmpty(reportType))
            {
                reportType = ActivityType;
            }

            // for expense tab - we want to show status options as - All, Not approved, Approved by TM, etc.
            if (reportType == ExpenseType)
            {
                modelFilter.StatusOptions = new List<CodeTableEx>()
                {
                    new CodeTableEx() {  CodeName =  Constant.TerritoryManagerApproved, Code = Constant.TerritoryManagerApproved, DisplaySequence = 20 },
                    new CodeTableEx() {  CodeName =  Constant.AreaManagerApproved, Code = Constant.AreaManagerApproved, DisplaySequence = 30 },
                    new CodeTableEx() {  CodeName =  Constant.ZoneManagerApproved, Code = Constant.ZoneManagerApproved, DisplaySequence = 40 },
                    new CodeTableEx() {  CodeName =  Constant.ExpenseNotApproved, Code = Constant.ExpenseNotApproved, DisplaySequence = 50 },
                };
            }
            else
            {
                modelFilter.StatusOptions = new List<CodeTableEx>()
                {
                    new CodeTableEx() {  CodeName =  Constant.Approved, Code = Constant.Approved, DisplaySequence = 20 },
                    new CodeTableEx() {  CodeName =  Constant.NotApproved, Code = Constant.NotApproved, DisplaySequence = 30 }
                };
            }

            // Control whether to show amount filters or not
            ViewBag.ShowAmountFilters = !(reportType == ActivityType || reportType == ExpenseReport || reportType == ActivityReport ||
                reportType == AppSignUpReport || reportType == AppSignInReport || reportType == DistantActivityReport ||
                reportType == AttendanceReport || reportType == AbsenteeReport ||
                reportType == AttendanceSummaryReport || reportType == AttendanceRegister ||
                reportType == IssueReturnType ||
                reportType == EmployeeExpenseReport ||
                reportType == DistanceReport ||
                reportType == ActivityByTypeReport ||
                reportType == EntityWorkFlowReport || reportType == EntityProgressReport ||
                reportType == DayPlanReport || reportType == QuestionnaireType || reportType == FarmerSummaryReport 
                || reportType == DealersNotMetReport || reportType == DealersSummaryReport || reportType == GeoTagReport 
                || reportType == Agreements || reportType == DuplicateFarmersReport || reportType == FarmersBankAccountReport);


            ViewBag.ShowClientTypeFilter = (reportType == ActivityReport || reportType == QuestionnaireType);
            ViewBag.ShowClientNameFilter = (reportType == ActivityReport || reportType == QuestionnaireType 
                || reportType == FarmerSummaryReport || reportType == GeoTagReport || reportType == Agreements || reportType == DuplicateFarmersReport || reportType== FarmersBankAccountReport);

            //Control to show fields related to Activity Report (Activity Type)
            //ViewBag.ShowActivityReportFilters = (reportType == ActivityReport || reportType == DistantActivityReport);

            //ViewBag.ShowClientTypeFilter = (reportType == ActivityReport);
            //ViewBag.ShowClientNameFilter = (reportType == ActivityReport);

            ViewBag.ShowActivityTypeFilter = (reportType == ActivityReport || reportType == DistantActivityReport);
            ViewBag.ShowAgreementNumberFilter = (reportType == EntityWorkFlowReport || reportType == IssueReturnType || reportType == EntityProgressReport 
            || reportType == FarmerSummaryReport || reportType == Agreements);
            ViewBag.ShowAgreementStatusFilter = (reportType == EntityWorkFlowReport || reportType == EntityProgressReport || reportType == Agreements);
            ViewBag.ShowCropFilter = (reportType == EntityWorkFlowReport || reportType == EntityProgressReport || reportType == FarmerSummaryReport);

            ViewBag.ShowSlipNumberFilter = (reportType == IssueReturnType);
            ViewBag.ShowRowStatusFilter = (reportType == IssueReturnType);

            //Control to show Status(Active/InActive)
            ViewBag.ShowEmployeeStatusFilter =
                (reportType == AbsenteeReport || reportType == AppSignInReport || reportType == ActivityType ||
                reportType == ExpenseType || reportType == ReturnType || reportType == PaymentType ||
                reportType == OrderType || reportType == ExpenseReport || reportType == AttendanceReport ||
                reportType == AttendanceSummaryReport || reportType == AttendanceRegister ||
                reportType == EmployeeExpenseReport || reportType == DistantActivityReport ||
                reportType == AppSignUpReport || reportType == ActivityReport || reportType == ActivityByTypeReport ||
                reportType == IssueReturnType ||
                reportType == DistanceReport ||
                reportType == EntityWorkFlowReport ||
                reportType == DayPlanReport || reportType == QuestionnaireType || reportType == QuestionnaireType 
                || reportType == DealersSummaryReport || reportType == GeoTagReport );


            //Control to show Department field related to AppSignInReport
            if (reportType == AppSignInReport)
            {
                ViewBag.ShowDepartmentFilter = true;
                ViewBag.ShowDesignationFilter = true;
            }
            else
            {
                ViewBag.ShowDepartmentFilter = false;
                ViewBag.ShowDesignationFilter = false;
            }

            if (reportType == OrderType)
            {
                ViewBag.ShowOrderIdFilter = true;
            }
            else
            {
                ViewBag.ShowOrderIdFilter = false;
            }

            if (reportType == EntityWorkFlowReport)
            {
                ViewBag.ShowEntityWorkFLowFilter = true;
                ViewBag.ShowEntityWorkFlowStatusFilter = true;
            }
            else
            {
                ViewBag.ShowEntityWorkFLowFilter = false;
                ViewBag.ShowEntityWorkFlowStatusFilter = false;
            }

            //Added By:PankajKumar; Purpose: Added Day Planning Report; Date: 30/04/2021
            if (reportType == DayPlanReport)
            {
                ViewBag.ShowPlanTypeFilter = true;
                ViewBag.ShowTargetStatusFilter = true;
            }
            else
            {
                ViewBag.ShowPlanTypeFilter = false;
                ViewBag.ShowTargetStatusFilter = false;
            }

            ViewBag.CustomerFilter = false;
            if (reportType == DealersNotMetReport)
            {
                ViewBag.CustomerFilter = true;
                ViewBag.EmployeeFilter = true;
            }
            else
            {
                ViewBag.CustomersFilter = false;
                ViewBag.EmployeeFilter = false;
            }

            ViewBag.ShowEntityNameFilter = (reportType == EntityProgressReport || reportType == EntityWorkFlowReport || reportType == IssueReturnType );

            ViewBag.ShowDistanceFilters = (reportType == DistantActivityReport);

            //Modified By:PankajKumar; Purpose: Added Day Planning Report; Date: 28/04/2021
            // Modified By:Venkatesh; Purpose: Added DealersNotMetReport
            ViewBag.ShowDateFilters = (reportType == AbsenteeReport || reportType == AppSignInReport ||
                reportType == ActivityType ||
                reportType == ExpenseType || reportType == ReturnType || reportType == PaymentType ||
                reportType == OrderType || reportType == ExpenseReport || reportType == AttendanceReport ||
                reportType == AttendanceSummaryReport || reportType == AttendanceRegister ||
                reportType == EmployeeExpenseReport || reportType == DistantActivityReport ||
                reportType == DistanceReport ||
                reportType == IssueReturnType || reportType == EntityWorkFlowReport ||
                reportType == AppSignUpReport || reportType == ActivityReport || reportType == ActivityByTypeReport ||
                reportType == DayPlanReport || reportType == QuestionnaireType || reportType == DealersNotMetReport);

            ViewBag.ShowPlannedDateFilters = (reportType == EntityWorkFlowReport);
            ViewBag.ShowHarvestDateFilter = (reportType == EntityWorkFlowReport);

            // Control the label for search button
            // Modified By:PankajKumar; Purpose: Added Day Planning Report; Date: 28/04/2021
            // Modified By:Venkatesh; Purpose: Added DealersNotMetReport
            if (reportType == ExpenseReport || reportType == AttendanceReport ||
                reportType == AttendanceSummaryReport || reportType == AttendanceRegister ||
                reportType == EmployeeExpenseReport ||
                reportType == DistanceReport || reportType == DistantActivityReport ||
                reportType == AppSignUpReport || reportType == AppSignInReport ||
                reportType == AbsenteeReport || reportType == ActivityReport || reportType == ActivityByTypeReport ||
                reportType == EntityWorkFlowReport || reportType == EntityProgressReport ||
                reportType == DayPlanReport || reportType == DealersNotMetReport || reportType == GeoTagReport || reportType == DealersSummaryReport 
                || reportType == Agreements || reportType == DuplicateFarmersReport)
            {
                ViewBag.SearchButtonText = "Generate";
            }
            else
            {
                ViewBag.SearchButtonText = "Search";
            }

            ViewBag.ReportType = reportType;
            ViewBag.OfficeHierarchy = officeHierarchy;

            ViewBag.SMSOnDemandFeatureEnabled = Utils.SiteConfigData.SMSOnDemandFeatureEnabled;

            ViewBag.MaxSMSTextSize = Utils.MaxSMSTextSize();

            // provide a option to create pdf report
            ViewBag.ShowPdfReportOption = (reportType == EmployeeExpenseReport);
            ViewBag.EmployeeExpenseRdlcReportName = Utils.SiteConfigData.EmployeeExpenseRdlcReportName;

            if (reportType == IssueReturnType)
            {
                ViewBag.StatusValues = Business.GetCodeTable("BankDetailStatus");
            }
            else
            {
                ViewBag.StatusValues = Business.GetCodeTable("None");
            }
            //Author:Ajith, Purpose:Dealer Questionnaire,Dated:12/06/2021
            ViewBag.QuestionPaper = Business.GetQuestionpaper();
            ViewBag.ShowQuestionnaireTypeFilter = (reportType == QuestionnaireType);

            //Added By:Rajesh V; Purpose: Farmer Summary Report; Date: 07/10/2021
            if (reportType == FarmerSummaryReport )
            {
                ViewBag.ShowUniqueIdFilter = true;
                ViewBag.ShowSeasonFilter = (reportType == FarmerSummaryReport);
                ViewBag.ShowSeasonData = Business.GetSeasonNames();
            }
            else
            {
                ViewBag.ShowUniqueIdFilter = false;
                ViewBag.ShowSeasonFilter = false;
            }
            if (reportType == GeoTagReport)
            {
                ViewBag.ShowGeoTagStatusFilter = true;
            }
            else
            {
                ViewBag.ShowGeoTagStatusFilter = false;
            } 
            if (reportType == Agreements || reportType == FarmersBankAccountReport)
            {
                ViewBag.ShowBankDetailStatusFilter = true;
            }
            else
            {
                ViewBag.ShowBankDetailStatusFilter = false;
            } 
            if (reportType == GeoTagReport)
            {
                ViewBag.ShowBusinessRoleFilter = true;
            }
            else
            {
                ViewBag.ShowBusinessRoleFilter = false;
            }
            if (reportType == Agreements)
            {
                ViewBag.ShowAgreementRpeortFilters = true;
                ViewBag.ShowSeasonData = Business.GetSeasonNames();

            }
            else
            {
                ViewBag.ShowAgreementRpeortFilters = false;

            }

            if (reportType == DuplicateFarmersReport)
            {
                ViewBag.ShowUniqueIdFilter = true;
                ViewBag.ShowProfileStatusFilter = true;
            }
            else
            {
                ViewBag.ShowUniqueIdFilter = false;
                ViewBag.ShowProfileStatusFilter = false;
            }

            if (reportType == DuplicateFarmersReport || reportType == DealersNotMetReport || reportType == Agreements || reportType == FarmersBankAccountReport)
            {
                ViewBag.EmployeeFilter = true;
            }
            else
            {
                ViewBag.EmployeeFilter = false;
            }

                return View(modelFilter);
        }

        /// <summary>
        /// Ajax method used for detail download of orders
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpGet]
        public ActionResult GetDetailedOrderResultForDownload(SearchCriteria searchCriteria)
        {
            DomainEntities.SearchCriteria s = Helper.DashboardParseSearchCriteria(searchCriteria);

            ICollection<Order> orders = Business.GetOrders(s);

            IEnumerable<OrderItem> orderItems = Business.GetOrderItems(orders);

            var model = orders.Select(x => new OrderItemsModel()
            {
                Order = new ModelOrder()
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    EmployeeName = x.EmployeeName,
                    DayId = x.DayId,
                    CustomerCode = x.CustomerCode,
                    CustomerName = x.CustomerName,
                    OrderDate = x.OrderDate,
                    TotalAmount = x.TotalAmount,
                    ItemsCount = x.ItemCount,
                    IsApproved = x.IsApproved,
                    RevisedTotalAmount = x.RevisedTotalAmount,
                    ApprovedAmt = x.ApprovedAmt,
                    EmployeeCode = x.EmployeeCode
                },
                Items = orderItems.Where(y => y.OrderId == x.Id).OrderBy(y => y.SequenceNumber)
                        .Select(y => new ModelOrderItem()
                        {
                            Id = y.Id,
                            OrderId = y.OrderId,
                            SequenceNumber = y.SequenceNumber,
                            ProductCode = y.ProductCode,
                            ProductName = y.ProductName,
                            UnitPrice = y.UnitPrice,
                            Quantity = y.Quantity,
                            Amount = y.Amount,
                            UOM = y.UOM,
                            RevisedQuantity = y.RevisedQuantity,
                            RevisedAmount = y.RevisedAmount
                        }).ToList()
            }).ToList();

            return PartialView("_DownloadOrdersWithDetail", model);
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsActionFilter]
        public ActionResult GetSearchResult(SearchCriteria searchCriteria) // don't change this parameter name
        {
            TryValidateModel(searchCriteria);
            DomainEntities.SearchCriteria s = Helper.DashboardParseSearchCriteria(searchCriteria);

            ViewBag.ReportType = searchCriteria.ReportType;

            //s.IsSuperAdmin = this.IsSuperAdmin;
            //s.CurrentUserStaffCode = this.CurrentUserStaffCode;

            if (s.ReportType.Equals(OrderType, StringComparison.OrdinalIgnoreCase))
            {
                return this.GetOrders(s);
            }

            if (s.ReportType.Equals(PaymentType, StringComparison.OrdinalIgnoreCase))
            {
                return this.GetPayments(s);
            }

            if (s.ReportType.Equals(ReturnType, StringComparison.OrdinalIgnoreCase))
            {
                return this.GetReturns(s);
            }

            if (s.ReportType.Equals(ExpenseType, StringComparison.OrdinalIgnoreCase))
            {
                return this.GetExpenses(s);
            }

            if (s.ReportType.Equals(ActivityType, StringComparison.OrdinalIgnoreCase))
            {
                return this.GetActivities(s);
            }

            if (s.ReportType.Equals(DistanceReport, StringComparison.OrdinalIgnoreCase))
            {
                return this.GetDistanceReport(s);
            }

            if (s.ReportType.Equals(IssueReturnType, StringComparison.OrdinalIgnoreCase))
            {
                return this.GetIssueReturns(s);
            }

            if (s.ReportType.Equals(ExpenseReport, StringComparison.OrdinalIgnoreCase))
            {
                return this.GetExpenseReport(s);
            }

            if (s.ReportType.Equals(EmployeeExpenseReport, StringComparison.OrdinalIgnoreCase))
            {
                return this.GetEmployeeExpenseReport(s);
            }

            if (s.ReportType.Equals(AttendanceReport, StringComparison.OrdinalIgnoreCase) ||
                s.ReportType.Equals(AttendanceSummaryReport, StringComparison.OrdinalIgnoreCase))
            {
                return this.GetAttendanceReport(s);
            }

            if (s.ReportType.Equals(AttendanceRegister, StringComparison.OrdinalIgnoreCase))
            {
                return this.GetAttendanceRegisterReport(s);
            }

            if (s.ReportType.Equals(AbsenteeReport, StringComparison.OrdinalIgnoreCase))
            {
                return this.GetAbsenteeReport(s);
            }

            if (s.ReportType.Equals(ActivityReport, StringComparison.OrdinalIgnoreCase))
            {
                return this.GetActivityReport(s);
            }

            if (s.ReportType.Equals(AppSignUpReport, StringComparison.OrdinalIgnoreCase))
            {
                return this.GetAppSignUpReport(s);
            }

            if (s.ReportType.Equals(AppSignInReport, StringComparison.OrdinalIgnoreCase))
            {
                return this.GetAppSignInReport(s);
            }

            if (s.ReportType.Equals(ActivityByTypeReport, StringComparison.OrdinalIgnoreCase))
            {
                return this.GetActivityByTypeReport(s);
            }

            if (s.ReportType.Equals(EntityWorkFlowReport, StringComparison.OrdinalIgnoreCase))
            {
                return this.GetEntityWorkFlowReport(s);
            }

            if (s.ReportType.Equals(EntityProgressReport, StringComparison.OrdinalIgnoreCase))
            {
                return this.GetEntityProgressReport(s);
            }

            if (s.ReportType.Equals(DistantActivityReport, StringComparison.OrdinalIgnoreCase))
            {
                return this.GetDistantActivityReport(s);
            }

            //PK
            if (s.ReportType.Equals(DayPlanReport, StringComparison.OrdinalIgnoreCase))
            {
                return this.GetDayPlanReport(s);
            }
            //Author:Ajith, Purpose:CustomerQuestionnaire , dated:12/06/2021
            if (s.ReportType.Equals(QuestionnaireType, StringComparison.OrdinalIgnoreCase))
            {
                return this.GetCustomerQuestionnaire(s);
            }

            if (s.ReportType.Equals(FarmerSummaryReport, StringComparison.OrdinalIgnoreCase))
            {
                return this.GetFarmerSummaryReport(s);
            }

            // Modified By:Venkatesh; Purpose: Added DealersNotMetReport

            if (s.ReportType.Equals(DealersNotMetReport, StringComparison.OrdinalIgnoreCase))
            {
                return this.GetDealersNotMetReport(s);
            }

            if (s.ReportType.Equals(DealersSummaryReport, StringComparison.OrdinalIgnoreCase))
            {
                return this.GetDealersSummaryReport(s);
            }

            if (s.ReportType.Equals(GeoTagReport, StringComparison.OrdinalIgnoreCase))
            {
                return this.GetGeoTaggingReport(s);
            }
             if (s.ReportType.Equals(Agreements, StringComparison.OrdinalIgnoreCase))
            {
                return this.GetAgreementsReportData(s);
            }
            if (s.ReportType.Equals(DuplicateFarmersReport, StringComparison.OrdinalIgnoreCase))
            {
                return this.GetDuplicateFarmersReportData(s);
            }

            if (s.ReportType.Equals(FarmersBankAccountReport, StringComparison.OrdinalIgnoreCase))
            {
                return this.GetFarmersBankAccountReportData(s);
            }

            return Content("Option not implemented.");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="editable">Whether to present Revised Qty link or not</param>
        /// <returns></returns>
        private ActionResult GetOrderItems(long orderId, bool editable)
        {
            OrderItemsModel model = GetOrderItemsModel(orderId);
            ViewBag.Editable = editable;

            return PartialView("_ShowOrderItems", model);
        }

        private OrderItemsModel GetOrderItemsModel(long orderId)
        {
            IEnumerable<DomainEntities.OrderItem> orderItems = Business.GetOrderItems(orderId);

            Order order = Business.GetOrder(orderId);
            if (order == null)
            {
                order = new Order()
                {
                    Id = 0,
                    ApprovedAmt = 0,
                    ApproveRef = "",
                    CustomerName = "Customer",
                    ApprovedDate = DateTime.MinValue,
                    IsApproved = false,
                };
            }

            ModelOrder modelOrder = new ModelOrder()
            {
                Id = order.Id,
                EmployeeId = order.EmployeeId,
                EmployeeCode = order.EmployeeCode,
                EmployeeName = order.EmployeeName,
                DayId = order.DayId,
                CustomerCode = order.CustomerCode,
                CustomerName = order.CustomerName,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                TotalGST = order.TotalGST,
                NetAmount = order.NetAmount,
                ItemsCount = order.ItemCount,
                IsApproved = order.IsApproved,
                ApproveRef = order.ApproveRef,
                ApprovedAmt = order.ApprovedAmt,
                ApproveComments = order.ApproveComments,
                ApprovedDate = order.ApprovedDate,
                RevisedTotalAmount = order.RevisedTotalAmount,
                RevisedTotalGST = order.RevisedTotalGST,
                RevisedNetAmount = order.RevisedNetAmount,
                DiscountType = order.DiscountType,
                ImageCount = order.ImageCount
            };

            OrderItemsModel model = new OrderItemsModel()
            {
                Order = modelOrder,
                Items = orderItems.OrderBy(i => i.SequenceNumber)
                .Select(x => new ModelOrderItem()
                {
                    Id = x.Id,
                    OrderId = x.OrderId,
                    SequenceNumber = x.SequenceNumber,
                    ProductCode = x.ProductCode,
                    ProductName = x.ProductName,
                    UnitPrice = x.UnitPrice,
                    Quantity = x.Quantity,
                    Amount = x.Amount, // legacy apk v1.3 and lower
                    DiscountPercent = x.DiscountPercent,
                    DiscountedPrice = x.DiscountedPrice,
                    ItemPrice = x.ItemPrice,
                    GstPercent = x.GstPercent,
                    GstAmount = x.GstAmount,
                    NetPrice = x.NetPrice,

                    UOM = x.UOM,
                    RevisedQuantity = x.RevisedQuantity,
                    RevisedAmount = x.RevisedAmount,  // legacy apk v1.3 and lower

                    RevisedDiscountPercent = x.RevisedDiscountPercent,
                    RevisedDiscountedPrice = x.RevisedDiscountedPrice,
                    RevisedItemPrice = x.RevisedItemPrice,
                    RevisedGstPercent = x.RevisedGstPercent,
                    RevisedGstAmount = x.RevisedGstAmount,
                    RevisedNetPrice = x.RevisedNetPrice,

                }).ToList()
            };

            return model;
        }

        [HttpGet]
        public ActionResult Activity(long employeeDayId)
        {
            IEnumerable<ActivityMapData> activityData = Business.GetActivityMapData(employeeDayId);
            List<ActivityMapDataModel> dataModel = activityData.Select(ad => new ActivityMapDataModel()
            {
                ActivityId = ad.ActivityId,
                ClientName = ad.ClientName,
                ClientPhone = ad.ClientPhone,
                ClientType = ad.ClientType,
                ActivityType = ad.ActivityType,
                ActivityAmount = ad.ActivityAmount,
                AtLocation = ad.AtLocation,
                Comments = ad.Comments,
                At = ad.At,
                Latitude = ad.Latitude,
                Longitude = ad.Longitude,
                EmployeeDayId = ad.EmployeeDayId,
                ImageCount = ad.ImageCount,
                ContactCount = ad.ContactCount,
                ActivityTrackingType = ad.ActivityTrackingType
            }).ToList();

            EmployeeDayData empDayData = Business.GetEmployeeDayData(employeeDayId);
            ViewBag.ActivityDate = empDayData.Date;
            ViewBag.EmployeeName = empDayData.EmployeeName;

            return View(dataModel);
        }

        [CheckRightsAuthorize(Feature = FeatureEnum.UnSownReportFeature)]
        public ActionResult UnSownReport()
        {
            ModelFilter modelFilter = new ModelFilter()
            {
                ClientType = Business.GetCodeTable("CustomerType"),
            };

            ViewBag.AgreementStatus = Business.GetCodeTable("AgreementStatus");
            ViewBag.ActiveCrops = Business.GetActiveCrops();

            ViewBag.OfficeHierarchy = GetOfficeHierarchy();
            ViewBag.SMSOnDemandFeatureEnabled = Utils.SiteConfigData.SMSOnDemandFeatureEnabled;
            ViewBag.MaxSMSTextSize = Utils.MaxSMSTextSize();

            ViewBag.IsSuperAdmin = IsSuperAdmin;
            return View(modelFilter);
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.UnSownReportFeature)]
        public ActionResult GetSearchUnSownReport(EntitiesFilter searchCriteria)
        {
            DomainEntities.EntitiesFilter criteria = Helper.ParseEntitiesSearchCriteria(searchCriteria);
            criteria.CurrentISTTime = Helper.GetCurrentIstDateTime();

            IEnumerable<UnSownData> entities = Business.GetUnSownData(criteria);
            ICollection<EntityAgreement> agreementData = Business.GetEntityAgreementData(criteria, "Input Issue");

            ViewBag.AgreementData = agreementData;

            ICollection<UnSownModel> model =
                         entities.Select(x => new UnSownModel()
                         {
                             Id = x.Id,
                             EmployeeId = x.EmployeeId,
                             DayId = x.DayId,

                             EmployeeCode = x.EmployeeCode,
                             EmployeeName = x.EmployeeName,
                             HQCode = x.HQCode,

                             EntityType = x.EntityType,
                             EntityName = x.EntityName,
                             UniqueIdType = x.UniqueIdType,
                             UniqueId = x.UniqueId,

                             LandSize = x.LandSize,
                         }).ToList();

            return PartialView("_ShowUnSownReport", model);
        }

        [CheckRightsAuthorize(Feature = FeatureEnum.SurveyFormReport)]
        public ActionResult SurveyFormReport()
        {
            ModelFilter modelFilter = new ModelFilter()
            {
                ClientType = Business.GetCodeTable("CustomerType"),
            };

            ViewBag.ActiveCrops = Business.GetActiveCrops();

            ViewBag.OfficeHierarchy = GetOfficeHierarchy();
            ViewBag.SMSOnDemandFeatureEnabled = Utils.SiteConfigData.SMSOnDemandFeatureEnabled;
            ViewBag.MaxSMSTextSize = Utils.MaxSMSTextSize();

            ViewBag.IsSuperAdmin = IsSuperAdmin;
            return View(modelFilter);
        }

        [CheckRightsAuthorize(Feature = FeatureEnum.RedFarmerModule)]
        public ActionResult TerminateAgreementRequests()
        {
            ModelFilter modelFilter = new ModelFilter()
            {
                ClientType = Business.GetCodeTable("CustomerType"),
            };

            ViewBag.AgreementStatus = Business.GetCodeTable("AgreementStatus");
            ViewBag.ActiveCrops = Business.GetActiveCrops();
            ViewBag.RedFarmerReson = Business.GetCodeTable("TerminateAgreementReason");

            // RedFarmerRequestStatus => TerminateAgreementStatus
            ViewBag.RedFarmerRequestStatus = Business.GetCodeTable("RedFarmerRequestStatus");
            ViewBag.SMSOnDemandFeatureEnabled = Utils.SiteConfigData.SMSOnDemandFeatureEnabled;
            ViewBag.MaxSMSTextSize = Utils.MaxSMSTextSize();

            ViewBag.OfficeHierarchy = GetOfficeHierarchy();

            ViewBag.IsSuperAdmin = IsSuperAdmin;
            return View(modelFilter);
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.RedFarmerModule)]
        public ActionResult GetSearchTerminateAgreementRequests(TerminateAgreementRequestFilter searchCriteria)
        {
            // ParseRedFarmerSearchCriteria => ParseTerminateAgreement...
            DomainEntities.TerminateAgreementRequestFilter criteria = Helper.ParseRedFarmerSearchCriteria(searchCriteria);
            criteria.CurrentISTTime = Helper.GetCurrentIstDateTime();

            IEnumerable<TerminateAgreementRequest> terminateAgreements = Business.GetTerminateAgreementRequests(criteria);
            ICollection<TerminateAgreementRequestModel> model =
                         terminateAgreements.Select(x => new TerminateAgreementRequestModel()
                         {
                             Id = x.Id,
                             EmployeeId = x.EmployeeId,
                             HQCode = x.HQCode,
                             EmployeeCode = x.EmployeeCode,
                             EmployeeName = x.EmployeeName,
                             AgreementNumber = x.AgreementNumber,
                             AgreementStatus = x.AgreementStatus,
                             RedFarmerReqReason = x.RequestReason,
                             RedFarmerReqStatus = x.Status,
                             EntityName = x.EntityName,
                             Crop = x.Crop,
                             SeasonName = x.WorkFlowSeasonName,
                             ReviewedBy = x.ReviewedBy,
                             ReviewDate = Helper.ConvertUtcTimeToIst(x.ReviewDate),
                             ActivityId = x.ActivityId,
                             RequestDate = x.RequestDate
                         }).ToList();

            PutFeatureSetInViewBag();

            return PartialView("_ShowTerminateAgreementRequest", model);
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.RedFarmerModule)]
        public ActionResult ApproveTerminateAgreementRequest(long id)
        {
            PutRedFarmerRequestStatus();

            DomainEntities.TerminateAgreementRequest terminateAgreementData = Business.GetSingleTerminateAgreementRequest(id);

            if (terminateAgreementData == null)
            {
                return PartialView("_Error");
            }

            TerminateAgreementRequestModel model = new TerminateAgreementRequestModel
            {
                EntityAgreementId = terminateAgreementData.EntityAgreementId,
                EntityName = terminateAgreementData.EntityName,
                AgreementNumber = terminateAgreementData.AgreementNumber,
                Crop = terminateAgreementData.Crop,
                RedFarmerReqReason = terminateAgreementData.RequestReason,
                RedFarmerReqStatus = terminateAgreementData.Status,
                Notes = terminateAgreementData.Notes,
                ReviewDate = terminateAgreementData.ReviewDate,
                ActivityId = terminateAgreementData.ActivityId
            };

            return PartialView("_ApproveTerminateAgreementRequest", model);
        }

        [AjaxOnly]
        [HttpPost]
        [CheckRightsAuthorize(Feature = FeatureEnum.RedFarmerModule)]
        public ActionResult ApproveTerminateAgreementRequest(TerminateAgreementRequestModel model)
        {
            TerminateAgreementRequest terminateAgreement = new TerminateAgreementRequest
            {
                Id = model.Id,
                Status = model.RedFarmerReqStatus,
                EntityName = model.EntityName,
                AgreementNumber = model.AgreementNumber,
                Crop = model.Crop,
                EntityAgreementId = model.EntityAgreementId
            };

            try
            {
                string currentUser = this.CurrentUserStaffCode;
                Business.SaveTerminateAgreementRequestStatus(terminateAgreement, currentUser);
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(ApproveTerminateAgreementRequest), ex);
                ModelState.AddModelError("", ex.Message);
                PutRedFarmerRequestStatus();
                return PartialView("_ApproveTerminateAgreementRequest", model);
            }
        }

        private void PutRedFarmerRequestStatus()
        {
            var reqStatus = Business.GetCodeTable("RedFarmerRequestStatus");

            foreach (var item in reqStatus.ToList())
            {
                if (item.Code.Equals("pending", StringComparison.OrdinalIgnoreCase))
                {
                    reqStatus.Remove(item);
                }
            }

            ViewBag.RedFarmerRequestStatus = reqStatus;
        }

        [CheckRightsAuthorize(Feature = FeatureEnum.AdvanceRequest)]
        public ActionResult AdvanceRequests()
        {
            ViewBag.AgreementStatus = Business.GetCodeTable("AgreementStatus");
            ViewBag.RequestStatus = Business.GetCodeTable("AdvanceRequestStatus");
            ViewBag.ActiveCrops = Business.GetActiveCrops();

            ViewBag.OfficeHierarchy = GetOfficeHierarchy();
            ViewBag.SMSOnDemandFeatureEnabled = Utils.SiteConfigData.SMSOnDemandFeatureEnabled;
            ViewBag.MaxSMSTextSize = Utils.MaxSMSTextSize();

            ViewBag.IsSuperAdmin = IsSuperAdmin;
            return View();
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.AdvanceRequest)]
        public ActionResult GetSearchAdvanceRequests(AdvanceRequestFilter searchCriteria)
        {
            DomainEntities.AdvanceRequestFilter criteria = Helper.ParseAdvanceSearchCriteria(searchCriteria);
            criteria.CurrentISTTime = Helper.GetCurrentIstDateTime();

            IEnumerable<AdvanceRequest> advanceRequests = Business.GetAdvanceRequests(criteria);

            ICollection<AdvanceRequestModel> model =
                         advanceRequests.Select(x => Helper.CreateNewAdvanceRequestModel(x)).ToList();

            return PartialView("_ShowAdvanceRequests", model);
        }

        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.AdvanceRequest)]
        public ActionResult ReviewAdvanceRequest(long id)
        {
            AdvanceRequest ar = Business.GetSingleAdvanceRequest(id);

            try
            {
                AdvanceRequestModel model = new AdvanceRequestModel
                {
                    EntityName = ar.EntityName,
                    AgreementNumber = ar.AgreementNumber,
                    Crop = ar.Crop,
                    AmountRequested = ar.Amount,
                    RequestDate = ar.AdvanceRequestDate,
                    RequestNote = ar.RequestNotes,
                    AmountApproved = ar.AmountApproved,
                    ApproveNote = ar.ApproveNotes
                };

                return PartialView("_ReviewAdvanceRequest", model);
            }
            catch (Exception ex)
            {
                Business.LogError($"{nameof(ReviewAdvanceRequest)}", ex);
                return PartialView("_Error");
            }
        }

        [AjaxOnly]
        [HttpPost]
        [CheckRightsAuthorize(Feature = FeatureEnum.AdvanceRequest)]

        public ActionResult ReviewAdvanceRequest(AdvanceRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_ReviewAdvanceRequest", model);
            }

            AdvanceRequest ar = new AdvanceRequest
            {
                Id = model.Id,
                Amount = model.AmountRequested,
                EntityName = model.EntityName,
                AmountApproved = model.AmountApproved,
                ApproveNotes = model.ApproveNote ?? ""
            };

            try
            {
                Business.SaveAdvanceRequestReview(ar, this.CurrentUserStaffCode);
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError($"{nameof(ReviewAdvanceRequest)}", ex);
                ModelState.AddModelError("", ex.Message);
                return PartialView("_ReviewAdvanceRequest", model);
            }
        }

        [HttpGet]
        public ActionResult ActivityOne(long activityId)
        {
            IEnumerable<ActivityMapData> activityData = Business.GetActivityData(activityId);
            List<ActivityMapDataModel> dataModel = activityData.Select(ad => new ActivityMapDataModel()
            {
                ActivityId = ad.ActivityId,
                ClientName = ad.ClientName,
                ClientPhone = ad.ClientPhone,
                ClientType = ad.ClientType,
                ActivityType = ad.ActivityType,
                ActivityAmount = ad.ActivityAmount,
                AtLocation = ad.AtLocation,
                Comments = ad.Comments,
                At = ad.At,
                Latitude = ad.Latitude,
                Longitude = ad.Longitude,
                EmployeeDayId = ad.EmployeeDayId,
                ImageCount = ad.ImageCount,
                ContactCount = ad.ContactCount
            }).ToList();

            EmployeeDayData empDayData = Business.GetEmployeeDayData(activityData.FirstOrDefault()?.EmployeeDayId ?? 0);
            ViewBag.ActivityDate = empDayData?.Date ?? DateTime.MinValue;
            ViewBag.EmployeeName = empDayData?.EmployeeName ?? "";

            return View("Activity", dataModel);
        }

        //public ActionResult ActivityMany(IEnumerable<long> activityIds)
        //{
        //    IEnumerable<TaskAction> taskAction = Business.GetSavedFollowUpTaskAction()
        //    IEnumerable<ActivityMapData> activityData = Business.GetManyActivityData(activityIds);
        //    List<ActivityMapDataModel> dataModel = activityData.Select(ad => new ActivityMapDataModel()
        //    {
        //        ActivityId = ad.ActivityId,
        //        ClientName = ad.ClientName,
        //        ClientPhone = ad.ClientPhone,
        //        ClientType = ad.ClientType,
        //        ActivityType = ad.ActivityType,
        //        ActivityAmount = ad.ActivityAmount,
        //        AtLocation = ad.AtLocation,
        //        Comments = ad.Comments,
        //        At = ad.At,
        //        Latitude = ad.Latitude,
        //        Longitude = ad.Longitude,
        //        EmployeeDayId = ad.EmployeeDayId,
        //        ImageCount = ad.ImageCount,
        //        ContactCount = ad.ContactCount
        //    }).ToList();

        //    EmployeeDayData empDayData = Business.GetEmployeeDayData(activityData.FirstOrDefault()?.EmployeeDayId ?? 0);
        //    ViewBag.ActivityDate = empDayData?.Date ?? DateTime.MinValue;
        //    ViewBag.EmployeeName = empDayData?.EmployeeName ?? "";

        //    return View("Activity", dataModel);
        //}

        [AjaxOnly]
        [HttpGet]
        public ActionResult ShowActivityContacts(long activityId)
        {
            IEnumerable<ActivityContactData> activityContactData = Business.GetActivityContactData(activityId);

            var dataModel = activityContactData.Select(x => new ActivityContactDataModel()
            {
                Id = x.Id,
                ActivityId = x.ActivityId,
                Name = x.Name,
                PhoneNumber = x.PhoneNumber,
                IsPrimary = x.IsPrimary
            }).ToList();

            return PartialView("_ShowActivityContacts", dataModel);
        }

        [HttpGet]
        public ActionResult Tracking(long employeeDayId)
        {
            ICollection<TrackingRecord> trackingList = Business.TrackingData(employeeDayId);

            EmployeeDayData empDayData = Business.GetEmployeeDayData(employeeDayId);
            ViewBag.ActivityDate = empDayData.Date;
            ViewBag.EmployeeName = empDayData.EmployeeName;
            ViewBag.EmployeeDayId = employeeDayId;

            if (IsSetupSuperAdmin)
            {
                return View(trackingList);
            }
            else
            {
                return View("TrackingManager", trackingList);
            }
        }

        [HttpGet]
        public ActionResult TrackingLog(int trackingLogId)
        {
            ViewBag.TrackingLogId = trackingLogId;
            string logData = Business.TrackingLogData(trackingLogId);
            ViewBag.LogData = logData;
            return View();
        }

        [HttpGet]
        public EmptyResult ExpenseItemImage(long expenseItemId, int imageItem)
        {
            this.Response.ContentType = "image/jpeg";

            byte[] imageBytes = Business.ExpenseItemImage(expenseItemId, imageItem);
            this.Response.BinaryWrite(imageBytes);
            return new EmptyResult();
        }

        [HttpGet]
        public EmptyResult PaymentImage(long paymentId, int imageItem)
        {
            this.Response.ContentType = "image/jpeg";

            byte[] imageBytes = Business.PaymentImage(paymentId, imageItem);
            this.Response.BinaryWrite(imageBytes);
            return new EmptyResult();
        }

        [HttpGet]
        public EmptyResult ActivityImage(long activityId, int imageItem)
        {
            this.Response.ContentType = "image/jpeg";

            byte[] imageBytes = Business.ActivityImage(activityId, imageItem);
            this.Response.BinaryWrite(imageBytes);
            return new EmptyResult();
        }

        [HttpGet]
        public EmptyResult OrderImage(long orderId, int imageItem)
        {
            this.Response.ContentType = "image/jpeg";

            byte[] imageBytes = Business.OrderImage(orderId, imageItem);
            this.Response.BinaryWrite(imageBytes);
            return new EmptyResult();
        }

        private ActionResult GetPaymentItem(long paymentId)
        {
            Payment payment = Business.GetPayment(paymentId);

            PaymentModel pm = new PaymentModel()
            {
                Id = payment.Id,
                EmployeeId = payment.EmployeeId,
                EmployeeCode = payment.EmployeeCode,
                EmployeeName = payment.EmployeeName,
                DayId = payment.DayId,
                TotalAmount = payment.TotalAmount,
                Comment = payment.Comment,
                CustomerCode = payment.CustomerCode,
                CustomerName = payment.CustomerName,
                DateCreated = payment.DateCreated,
                DateUpdated = payment.DateUpdated,
                ImageCount = payment.ImageCount,
                PaymentDate = payment.PaymentDate,
                PaymentType = payment.PaymentType,
                SqlitePaymentId = payment.SqlitePaymentId,
                IsApproved = payment.IsApproved,
            };

            return PartialView("_ShowPaymentItem", pm);
        }

        private ActionResult GetReturnItems(long returnId)
        {
            IEnumerable<ReturnItem> returnItems = Business.GetReturnItems(returnId);
            Return returns = Business.GetReturn(returnId);

            ReturnItemsModel model = new ReturnItemsModel()
            {
                Return = returns,
                Items = returnItems.Select(x => new ModelReturnItem()
                {
                    Id = x.Id,
                    ReturnsId = x.ReturnsId,
                    SequenceNumber = x.SequenceNumber,
                    ProductCode = x.ProductCode,
                    ProductName = x.ProductName,
                    BillingPrice = x.BillingPrice,
                    Quantity = x.Quantity,
                    Amount = x.Amount,
                    Comments = x.Comments
                }).ToList()
            };

            return PartialView("_ShowReturnItems", model);
        }

        public ActionResult ShowApprovePage(long Id, string reportType)
        {
            ViewBag.Id = Id;
            ViewBag.reportType = reportType;

            return View();
        }

        public ActionResult GetOrderItemDetails(long Id)
        {
            return GetOrderItems(Id, false);
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult GetWorkFlowItemsUsed(long id)
        {
            IEnumerable<string> itemsList = Business.GetWorkFlowDetailItemsUsed(id);
            return PartialView("_ShowWorkFlowDetailItemsUsed", itemsList);
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult GetItemDetails(long Id, string reportType)
        {
            if (reportType.Equals(OrderType, StringComparison.OrdinalIgnoreCase))
            {
                return GetOrderItems(Id, true);
            }

            if (reportType.Equals(PaymentType, StringComparison.OrdinalIgnoreCase))
            {
                return GetPaymentItem(Id);
            }

            if (reportType.Equals(ReturnType, StringComparison.OrdinalIgnoreCase))
            {
                return GetReturnItems(Id);
            }

            if (reportType.Equals(ExpenseType, StringComparison.OrdinalIgnoreCase))
            {
                return GetExpenseItems(Id);
            }

            return View();
        }

        [AjaxOnly]
        [HttpGet]
        public JsonResult GetApprovedData(long Id, string reportType)
        {
            DomainEntities.ApprovalData approvalData = null;
            if (reportType.Equals(OrderType, StringComparison.OrdinalIgnoreCase))
            {
                approvalData = Business.GetOrder(Id);
            }
            if (reportType.Equals(PaymentType, StringComparison.OrdinalIgnoreCase))
            {
                approvalData = Business.GetPayment(Id);
            }

            if (reportType.Equals(ReturnType, StringComparison.OrdinalIgnoreCase))
            {
                approvalData = Business.GetReturn(Id);
            }

            if (reportType.Equals(ExpenseType, StringComparison.OrdinalIgnoreCase))
            {
                // An expense can have multiple approvals;
                SalesPersonLevel level = Business.GetHighestLevel(IsSuperAdmin, CurrentUserStaffCode);
                approvalData = Business.GetExpenseApprovalDataAtCurrentUserLevel(Id, level);
            }

            if (approvalData == null)
            {
                approvalData = new DomainEntities.ApprovalData();
            }

            ApprovedResponse approvedResponse = new ApprovedResponse()
            {
                ApproveAmount = approvalData.ApprovedAmt,
                ApproveDate = approvalData.ApprovedDate.ToString("ddd dd-MM-yyyy hh:mm:ss tt"),
                Comment = approvalData.ApproveComments,
                RefNo = approvalData.ApproveRef,
                IsApproved = approvalData.IsApproved,
                ApprovedBy = approvalData.ApprovedBy
            };

            return Json(approvedResponse, JsonRequestBehavior.AllowGet);
        }

        private bool IsValidSearchCriteria(string criteria)
        {
            if (String.IsNullOrEmpty(criteria) || criteria.Equals("All", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            return true;
        }

        private ActionResult GetActivities(DomainEntities.SearchCriteria searchCriteria)
        {
            FixDatesInSearchCriteria(searchCriteria);

            DashboardReportModel reportModel = new DashboardReportModel()
            {
                AppUsers = Business.GetActivityReportUsers(searchCriteria),
                ReportDays = Business.DaysTable(searchCriteria.DateFrom, searchCriteria.DateTo),
                DashboardDataSet = Business.DashboardData(searchCriteria.DateFrom, searchCriteria.DateTo),
                ReportStartDate = searchCriteria.DateFrom,
                ReportEndDate = searchCriteria.DateTo
            };

            return PartialView("_ShowActivity", reportModel);
        }

        private ActionResult GetDistanceReport(DomainEntities.SearchCriteria searchCriteria)
        {
            FixDatesInSearchCriteria(searchCriteria);

            DashboardReportModel reportModel = new DashboardReportModel()
            {
                AppUsers = Business.GetActivityReportUsers(searchCriteria),
                ReportDays = Business.DaysTable(searchCriteria.DateFrom, searchCriteria.DateTo),
                DashboardDataSet = Business.DashboardData(searchCriteria.DateFrom, searchCriteria.DateTo, true),
                ReportStartDate = searchCriteria.DateFrom,
                ReportEndDate = searchCriteria.DateTo,

                OfficeHierarchy = GetOfficeHierarchy().Select(x => new OfficeHierarchyModel()
                {
                    ZoneCode = x.ZoneCode,
                    ZoneName = x.ZoneName,
                    AreaCode = x.AreaCode,
                    AreaName = x.AreaName,
                    TerritoryCode = x.TerritoryCode,
                    TerritoryName = x.TerritoryName,
                    HQCode = x.HQCode,
                    HQName = x.HQName
                }).ToList(),
            };

            // keep only those users who are in AppUsers collection
            // DashboardDataSet is for all users; whereas AppUsers is the list of users we want to show
            // for current logged in user.Hence we want to count the user as active user only
            // if user is in AppUsers as well

            //reportModel.DashboardDataSet = reportModel.DashboardDataSet
            //                .Where(x => reportModel.AppUsers.Any(y => y.EmployeeId == x.TenantEmployeeId))
            //                .ToList();

            return PartialView("_ShowDistanceReport", reportModel);
        }

        private ActionResult GetActivityReport(DomainEntities.SearchCriteria searchCriteria)
        {
            var AppUsers = Business.GetActivityReportUsers(searchCriteria);
            ICollection<DashboardDataSet> DashboardDataSet = Business.DashboardData(searchCriteria.DateFrom, searchCriteria.DateTo);
            IEnumerable<ActivityMapData> ActivityData = Business.GetAllActivityMapData(searchCriteria);

            //
            // Start of change for Amruth - Sep 28 2020
            // Amruth needed Start/End Day activity also + (Total Distance and Total hours) for the day
            // along with each activity row;
            //
            // this is to get start and end day activities;
            ICollection<AttendanceData> attendanceData = Business.GetAttendanceReportData(searchCriteria);
            // this is to get total hours for a day
            ICollection<AttendanceReportData> AttendanceReportData = Business.GetAttendanceSummaryReportDataSet(searchCriteria, attendanceData);

            // Activity Data does not have start / end day activity data
            // Put it in there from AttendanceReportData

            // combined ActivityData with start day entries
            ActivityData = ActivityData.Concat(
                        attendanceData.Where(x => x.IsStartOfDay).Select(x => new ActivityMapData()
                        {
                            TenantEmployeeId = x.TenantEmployeeId,
                            EmployeeDayId = x.EmployeeDayId,
                            ActivityType = "Start Day",
                            At = x.At,
                            GoogleMapsDistanceInMeters = x.GoogleMapsDistanceInMeters,
                            LocationName = x.StartLocation,
                            IsStartOrEndDayActivity = true
                        }).ToList());

            // combined ActivityData with End day entries
            ActivityData = ActivityData.Concat(
                        attendanceData.Where(x => x.IsEndOfDay).Select(x => new ActivityMapData()
                        {
                            TenantEmployeeId = x.TenantEmployeeId,
                            EmployeeDayId = x.EmployeeDayId,
                            ActivityType = "End Day",
                            At = x.At,
                            GoogleMapsDistanceInMeters = x.GoogleMapsDistanceInMeters,
                            LocationName = x.EndLocation,
                            IsStartOrEndDayActivity = true
                        }).ToList());

            //
            // End of change for Amruth - Sep 28 2020
            //

            IEnumerable<ActivityReportModel> reportModel = (from r in AppUsers
                                                            join d in DashboardDataSet on r.EmployeeId equals d.TenantEmployeeId
                                                            join ad in ActivityData on d.EmployeeDayId equals ad.EmployeeDayId
                                                            join ard in AttendanceReportData on d.EmployeeDayId equals ard.EmployeeDayId
                                                            orderby d.Date
                                                            select new ActivityReportModel()
                                                            {
                                                                Id = d.TenantEmployeeId,
                                                                EmployeeCode = r.EmployeeCode,
                                                                Name = r.Name,
                                                                Date = d.Date,
                                                                EmployeeDayId = d.EmployeeDayId,
                                                                ActivityId = ad.ActivityId,
                                                                ClientName = ad.ClientName,
                                                                ClientPhone = ad.ClientPhone,
                                                                ClientType = ad.ClientType,
                                                                ActivityType = ad.ActivityType,
                                                                Comments = ad.Comments,
                                                                At = ad.At,
                                                                ActivityDistanceInMeters = ad.GoogleMapsDistanceInMeters,  // activity distance
                                                                LocationName = ad.LocationName,
                                                                TotalDistanceInMeters = d.TotalDistanceInMeters, // day's total distance
                                                                Hours = ard.Hours, // day's total hours
                                                                IsStartOrEndDayActivity = ad.IsStartOrEndDayActivity
                                                            }).ToList();

            return PartialView("_ShowActivityReport", reportModel);
        }

        private ActionResult GetExpenseReport(DomainEntities.SearchCriteria searchCriteria)
        {
            Helper.FixDatesInSearchCriteriaForAnyDateRange(searchCriteria);

            ExpenseReportModel reportModel = new ExpenseReportModel()
            {
                OfficeHierarchy = GetOfficeHierarchy().Select(x => new OfficeHierarchyModel()
                {
                    ZoneCode = x.ZoneCode,
                    ZoneName = x.ZoneName,
                    AreaCode = x.AreaCode,
                    AreaName = x.AreaName,
                    TerritoryCode = x.TerritoryCode,
                    TerritoryName = x.TerritoryName,
                    HQCode = x.HQCode,
                    HQName = x.HQName
                }).ToList(),

                ExpenseReportData = Business.GetExpenseReportData(searchCriteria)
                                    .Select(x => new ExpenseReportDataModel()
                                    {
                                        EmployeeId = x.EmployeeId,
                                        DayId = x.DayId,
                                        Name = x.Name,
                                        ExpenseDate = x.ExpenseDate,
                                        ExpenseHQCode = x.ExpenseHQCode,
                                        //RepairAmount = x.RepairAmount,
                                        //DailyAllowanceAmount = x.DailyAllowanceAmount,
                                        DALocalAmount = x.DALocalAmount + x.DailyAllowanceAmount,
                                        DAOutstationAmount = x.DAOutstationAmount,

                                        TelephoneAmount = x.TelephoneAmount,
                                        InternetAmount = x.InternetAmount,
                                        LodgeRent = x.LodgeRent,
                                        VehicleRepairAmount = x.VehicleRepairAmount,
                                        OwnStayAmount = x.OwnStayAmount,
                                        TollTaxAmount = x.TollTaxAmount,
                                        ParkingAmount = x.ParkingAmount,
                                        DriverSalary = x.DriverSalary,
                                        StationaryAmount = x.StationaryAmount,
                                        MiscellaneousAmount = x.MiscellaneousAmount,
                                        DailyConsolidation = new EmployeeDailyConsolidationModel()
                                        {
                                            DayId = x.DailyConsolidation.DayId,
                                            EmployeeId = x.DailyConsolidation.EmployeeId,
                                            StaffCode = x.DailyConsolidation.StaffCode,
                                            ActivityCount = x.DailyConsolidation.ActivityCount,
                                            EndPosition = x.DailyConsolidation.EndPosition,
                                            StartPosition = x.DailyConsolidation.StartPosition,
                                            TotalExpenseAmount = x.DailyConsolidation.TotalExpenseAmount,
                                            TotalOrderAmount = x.DailyConsolidation.TotalOrderAmount,
                                            TotalPaymentAmount = x.DailyConsolidation.TotalPaymentAmount,
                                            TotalReturnOrderAmount = x.DailyConsolidation.TotalReturnOrderAmount,
                                            TrackingDistanceInMeters = x.DailyConsolidation.TrackingDistanceInMeters,
                                            StartTime = x.DailyConsolidation.StartTime,
                                            EndTime = x.DailyConsolidation.EndTime
                                        },

                                        TravelPublic = x.TravelPublic.Select(a => new TravelPublicExpenseDataModel()
                                        {
                                            Amount = a.Amount,
                                            TransportType = a.TransportType
                                        }).ToList(),

                                        TravelPrivate = x.TravelPrivate.Select(a => new TravelPrivateExpenseDataModel()
                                        {
                                            TransportType = a.TransportType,
                                            Amount = a.Amount,
                                            OdometerEnd = a.OdometerEnd,
                                            OdometerStart = a.OdometerStart
                                        }).ToList(),

                                        TravelCompany = x.TravelCompany.Select(a => new TravelCompanyExpenseDataModel()
                                        {
                                            OdometerStart = a.OdometerStart,
                                            OdometerEnd = a.OdometerEnd
                                        }).ToList(),

                                        Fuel = x.Fuel.Select(a => new FuelExpenseDataModel()
                                        {
                                            Amount = a.Amount,
                                            FuelQuantityInLiters = a.FuelQuantityInLiters,
                                            FuelType = a.FuelType
                                        }).ToList()
                                    }).ToList()
            };

            return PartialView("_ShowExpenseReport", reportModel);
        }

        private ActionResult GetEmployeeExpenseReport(DomainEntities.SearchCriteria searchCriteria)
        {
            var reportModel = Helper.GetEmployeeExpenseReportModel(searchCriteria);
            string viewName = Utils.SiteConfigData.EmployeeExpenseRdlcReportName;
            //return PartialView("_ShowEmployeeExpenseReport", reportModel);
            // _ExpenseReport | _HeranbaExpenseReport
            return PartialView(viewName, reportModel);
        }

        private ActionResult GetAppSignUpReport(DomainEntities.SearchCriteria searchCriteria)
        {
            Helper.FixDatesInSearchCriteriaForAnyDateRange(searchCriteria);

            AppSignUpReportModel reportModel = new AppSignUpReportModel()
            {
                OfficeHierarchy = GetOfficeHierarchy().Select(x => new OfficeHierarchyModel()
                {
                    ZoneCode = x.ZoneCode,
                    ZoneName = x.ZoneName,
                    AreaCode = x.AreaCode,
                    AreaName = x.AreaName,
                    TerritoryCode = x.TerritoryCode,
                    TerritoryName = x.TerritoryName,
                    HQCode = x.HQCode,
                    HQName = x.HQName
                }).ToList(),

                AppSignUpReportData = Business.GetAppSignUpReportDataSet(searchCriteria)
                                    .Select(x => new AppSignUpReportDataModel()
                                    {
                                        SignupDate = x.SignupDate,
                                        Name = x.Name,
                                        StaffCode = x.StaffCode,
                                        ExpenseHQCode = x.ExpenseHQCode,
                                        Phone = x.Phone,
                                        IsActive = x.IsActive,
                                        LastAccessDate = x.LastAccessDate,
                                        AppVersion = x.AppVersion,
                                        PhoneModel = x.PhoneModel,
                                        PhoneOS = x.PhoneOS
                                    }).ToList()
            };

            return PartialView("_ShowAppSignUpReport", reportModel);
        }

        private ActionResult GetAppSignInReport(DomainEntities.SearchCriteria searchCriteria)
        {
            Helper.FixDatesInSearchCriteriaForAnyDateRange(searchCriteria);

            // DateReturned in SignupDate has time - so we want to include all records for upto dateto
            DateTime dateTo = searchCriteria.DateTo.AddDays(1);

            AppSignInReportModel reportModel = new AppSignInReportModel()
            {
                OfficeHierarchy = GetOfficeHierarchy().Select(x => new OfficeHierarchyModel()
                {
                    ZoneCode = x.ZoneCode,
                    ZoneName = x.ZoneName,
                    AreaCode = x.AreaCode,
                    AreaName = x.AreaName,
                    TerritoryCode = x.TerritoryCode,
                    TerritoryName = x.TerritoryName,
                    HQCode = x.HQCode,
                    HQName = x.HQName
                }).ToList(),

                AppSignInReportData = Business.GetAppSignInReportDataSet(searchCriteria)
                                    //.Where(x=> x.SignupDate <= searchCriteria.DateTo)
                                    .Select(x => new AppSignInReportDataModel()
                                    {
                                        Name = x.Name,
                                        StaffCode = x.StaffCode,
                                        ExpenseHQCode = x.ExpenseHQCode,
                                        DaysActive = x.DaysActive,
                                        Phone = x.Phone,
                                        IsActive = (x.IsActive && x.IsActiveInSap),
                                        SignupDate = x.SignupDate,
                                        HasSignedUpBeforeReportEndDate = x.SignupDate < dateTo
                                    }).ToList()
            };

            return PartialView("_ShowAppSignInReport", reportModel);
        }

        private ActionResult GetAbsenteeReport(DomainEntities.SearchCriteria searchCriteria)
        {
            Helper.FixDatesInSearchCriteriaForAnyDateRange(searchCriteria);

            if (searchCriteria.DateTo > DateTime.UtcNow.AddDays(1))
            {
                searchCriteria.DateTo = DateTime.UtcNow.AddDays(1);
            }

            AbsenteeReportModel reportModel = new AbsenteeReportModel()
            {
                OfficeHierarchy = GetOfficeHierarchy().Select(x => new OfficeHierarchyModel()
                {
                    ZoneCode = x.ZoneCode,
                    ZoneName = x.ZoneName,
                    AreaCode = x.AreaCode,
                    AreaName = x.AreaName,
                    TerritoryCode = x.TerritoryCode,
                    TerritoryName = x.TerritoryName,
                    HQCode = x.HQCode,
                    HQName = x.HQName
                }).ToList(),

                AbsenteeReportData = Business.GetAbsenteeReportDataSet(searchCriteria)
                                    .Select(x => new AbsenteeReportDataModel()
                                    {
                                        Date = x.Date,
                                        Name = x.Name,
                                        StaffCode = x.StaffCode,
                                        ExpenseHQCode = x.ExpenseHQCode,
                                        Phone = x.Phone,
                                        IsActive = (x.IsActive && x.IsActiveInSap),
                                        SignupDate = x.SignupDate
                                    }).ToList()
            };

            return PartialView("_ShowAbsenteeReport", reportModel);
        }

        private ActionResult GetAttendanceReport(DomainEntities.SearchCriteria searchCriteria)
        {
            Helper.FixDatesInSearchCriteriaForAnyDateRange(searchCriteria);

            AttendanceReportModel reportModel = new AttendanceReportModel()
            {
                OfficeHierarchy = GetOfficeHierarchy().Select(x => new OfficeHierarchyModel()
                {
                    ZoneCode = x.ZoneCode,
                    ZoneName = x.ZoneName,
                    AreaCode = x.AreaCode,
                    AreaName = x.AreaName,
                    TerritoryCode = x.TerritoryCode,
                    TerritoryName = x.TerritoryName,
                    HQCode = x.HQCode,
                    HQName = x.HQName
                }).ToList(),

                AttendanceReportData = Business.GetAttendanceReportDataSet(searchCriteria)
                                    .OrderBy(x => x.Date).ThenBy(x => x.Name).ThenBy(x => x.EndTime)
                                    .Select(x => new AttendanceReportDataModel()
                                    {
                                        Date = x.Date,
                                        DayId = x.DayId,
                                        EmployeeDayId = x.EmployeeDayId,
                                        StartTime = x.StartTime,
                                        EndTime = x.EndTime,
                                        Name = x.Name,
                                        StaffCode = x.StaffCode,
                                        TenantEmployeeId = x.TenantEmployeeId,
                                        RefStartTrackingId = x.RefStartTrackingId,
                                        RefEndTrackingId = x.RefEndTrackingId,
                                        Hours = x.Hours,
                                        ExpenseHQCode = x.ExpenseHQCode,
                                        StartLocation = x.StartLocation,
                                        EndLocation = x.EndLocation,
                                        ActivityCount = x.ActivityCount,
                                        DistanceTravelled = x.DistanceTravelled
                                    }).ToList()
            };

            string partialViewName = "_ShowAttendanceReport";
            if (searchCriteria.ReportType.Equals(AttendanceSummaryReport, StringComparison.OrdinalIgnoreCase))
            {
                partialViewName = "_ShowAttendanceSummaryReport";
            }

            return PartialView(partialViewName, reportModel);
        }

        private ActionResult GetAttendanceRegisterReport(DomainEntities.SearchCriteria searchCriteria)
        {
            AttendanceReportModel reportModel = new AttendanceReportModel()
            {
                AppUsers = Business.GetActivityReportUsers(searchCriteria),
                ReportDays = Business.DaysTable(searchCriteria.DateFrom, searchCriteria.DateTo),

                AttendanceReportData = Business.GetAttendanceReportDataSet(searchCriteria)
                                    .OrderBy(x => x.Date).ThenBy(x => x.Name).ThenBy(x => x.EndTime)
                                    .Select(x => new AttendanceReportDataModel()
                                    {
                                        Date = x.Date,
                                        DayId = x.DayId,
                                        EmployeeDayId = x.EmployeeDayId,
                                        Name = x.Name,
                                        StaffCode = x.StaffCode,
                                        TenantEmployeeId = x.TenantEmployeeId,
                                        RefStartTrackingId = x.RefStartTrackingId,
                                        RefEndTrackingId = x.RefEndTrackingId,
                                        Hours = x.Hours
                                    }).ToList()
            };

            return PartialView("_ShowAttendanceRegisterReport", reportModel);
        }

        //PK / Kartik Modified
        private ActionResult GetDayPlanReport(DomainEntities.SearchCriteria searchCriteria)
        {

            DayPlanReportModel reportModel = new DayPlanReportModel()
            {
                DayPlanReportData = Business.GetDayPlanReportDataSet(searchCriteria)
                                    .Select(x => new DayPlanReportDataModel()
                                    {
                                        DayId = x.DayId,
                                        EmployeeId = x.EmployeeId,
                                        EmployeeCode = x.EmployeeCode,
                                        Name = x.Name,
                                        Date = x.Date,
                                        TargetDealerAppt = x.TargetDealerAppt,
                                        TargetSalesAmount = x.TargetSalesAmount,
                                        TargetCollectionAmount = x.TargetCollectionAmount,
                                        TargetDemoActivity = x.TargetDemoActivity,
                                        TargetVigoreSales = x.TargetVigoreSales,
                                        ActualSalesAmount = x.ActualSalesAmount,
                                        ActualCollectionAmount = x.ActualCollectionAmount,
                                        ActualDealerAppt = x.ActualDealerAppt,
                                        ActualDemoActivity = x.ActualDemoActivity,
                                        ActualVigoreSales = x.ActualVigoreSales
                                    }).ToList()
            };
            return PartialView("_ShowDayPlanningReport", reportModel);
        }

        private void FixDatesInSearchCriteria(DomainEntities.SearchCriteria searchCriteria)
        {
            if (searchCriteria.ApplyDateFilter == false)
            {
                searchCriteria.ApplyDateFilter = true;
                searchCriteria.DateTo = DateTime.Now;
                searchCriteria.DateFrom = DateTime.Now.AddDays(-7);
            }
            else if (searchCriteria.DateTo.Subtract(searchCriteria.DateFrom).Days > 10)
            {
                searchCriteria.DateFrom = searchCriteria.DateTo.AddDays(-10);
            }
        }

        private ActionResult GetIssueReturns(DomainEntities.SearchCriteria searchCriteria)
        {
            ICollection<IssueReturn> issueReturns = Business.GetIssueReturns(searchCriteria);

            ICollection<IssueReturnModel> model = issueReturns.Select(x => CreateNewIssueReturnModel(x))
                                            .ToList();

            return PartialView("_ShowIssueReturn", model);
        }

        private ActionResult GetExpenses(DomainEntities.SearchCriteria searchCriteria)
        {
            ICollection<Expense> expenses = Business.GetExpenses(searchCriteria);

            ICollection<ModelExpense> model = expenses.Select(x => new ModelExpense()
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                EmployeeCode = x.EmployeeCode,
                EmployeeName = x.EmployeeName,
                DayId = x.DayId,
                ExpenseDate = x.ExpenseDate,
                TotalAmount = x.TotalAmount,
                IsZoneApproved = x.IsZoneApproved,
                IsAreaApproved = x.IsAreaApproved,
                IsTerritoryApproved = x.IsTerritoryApproved,
                Phone = x.Phone,
                Approvals = x.Approvals.Select(y => new ModelExpenseApproval()
                {
                    ApproveAmount = y.ApproveAmount,
                    ApproveLevel = y.ApproveLevel
                }).ToList()
            }).ToList();

            return PartialView("_ShowExpense", model);
        }

        private ActionResult GetReturns(DomainEntities.SearchCriteria searchCriteria)
        {
            ICollection<Return> returns = Business.GetReturns(searchCriteria);

            ICollection<ModelReturns> model = returns.Select(x => new ModelReturns()
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                EmployeeCode = x.EmployeeCode,
                EmployeeName = x.EmployeeName,
                DayId = x.DayId,
                CustomerCode = x.CustomerCode,
                CustomerName = x.CustomerName,
                ReturnsDate = x.ReturnDate,
                TotalAmount = x.TotalAmount,
                ItemsCount = x.ItemCount,
                Comments = x.Comments,
                ReferenceNumber = x.ReferenceNumber,
                IsApproved = x.IsApproved,
                Phone = x.Phone
            }).ToList();

            return PartialView("_ShowReturns", model);
        }

        private ActionResult GetPayments(DomainEntities.SearchCriteria searchCriteria)
        {
            ICollection<Payment> payments = Business.GetPayments(searchCriteria);

            ICollection<PaymentModel> model =
                         payments.Select(x => new PaymentModel()
                         {
                             Id = x.Id,
                             EmployeeId = x.EmployeeId,
                             EmployeeCode = x.EmployeeCode,
                             EmployeeName = x.EmployeeName,
                             DayId = x.DayId,
                             TotalAmount = x.TotalAmount,
                             Comment = x.Comment,
                             CustomerCode = x.CustomerCode,
                             CustomerName = x.CustomerName,
                             DateCreated = x.DateCreated,
                             DateUpdated = x.DateUpdated,
                             ImageCount = x.ImageCount,
                             PaymentDate = x.PaymentDate,
                             PaymentType = x.PaymentType,
                             SqlitePaymentId = x.SqlitePaymentId,
                             IsApproved = x.IsApproved,
                             Phone = x.Phone

                         }).ToList();

            return PartialView("_ShowPayments", model);
        }

        private ActionResult GetOrders(DomainEntities.SearchCriteria searchCriteria)
        {
            ICollection<Order> orders = Business.GetOrders(searchCriteria);
            ICollection<ModelOrder> model = orders.Select(x => new ModelOrder()
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                EmployeeCode = x.EmployeeCode,
                EmployeeName = x.EmployeeName,
                DayId = x.DayId,
                CustomerCode = x.CustomerCode,
                CustomerName = x.CustomerName,
                OrderDate = x.OrderDate,
                TotalAmount = x.TotalAmount,
                TotalGST = x.TotalGST,
                NetAmount = x.NetAmount,
                RevisedTotalAmount = x.RevisedTotalAmount,
                RevisedTotalGST = x.RevisedTotalGST,
                RevisedNetAmount = x.RevisedNetAmount,
                ItemsCount = x.ItemCount,
                IsApproved = x.IsApproved,
                ApprovedAmt = x.ApprovedAmt,
                Phone = x.Phone
            }).ToList();

            return PartialView("_ShowOrders", model);
        }

        private ActionResult GetDistantActivityReport(DomainEntities.SearchCriteria searchCriteria)
        {
            try
            {
                var tuple = Business.GetDistantActivityData(searchCriteria);

                IEnumerable<DistantActivityData> distantActivityData = tuple.Item1;
                ViewBag.Entities = tuple.Item2;

                IEnumerable<DistantActivityReportModel> reportModel = distantActivityData
                    .Where(x => !x.Delete)
                    .OrderBy(x => x.ActivityDate)
                       .Select(x => new DistantActivityReportModel()
                       {
                           ActivityId = x.ActivityId,
                           ActivityDate = x.ActivityDate,
                           ActivityType = x.ActivityType,
                           ActivityAtLocation = x.ActivityAtLocation,
                           ActivityLatitude = x.ActivityLatitude,
                           ActivityLongitude = x.ActivityLongitude,
                           TenantEmployeeId = x.TenantEmployeeId,
                           StaffCode = x.StaffCode,
                           EmployeeName = x.EmployeeName,
                           RadiusValue = x.RadiusValue,
                           EntityId = x.EntityId
                       }).ToList();

                return PartialView("_ShowDistantActivityReport", reportModel);
            }
            catch (Exception ex)
            {
                Business.LogError($"{nameof(GetDistantActivityReport)}", ex);
                return PartialView("_Error");
            }
        }

        private ActionResult GetEntityWorkFlowReport(DomainEntities.SearchCriteria searchCriteria)
        {
            ICollection<EntityWorkFlowDetail> entityWorkFlowDetails = Business.GetEntityWorkFlowDetails(searchCriteria);

            ICollection<EntityWorkFlowDetailModel> model =
                         entityWorkFlowDetails
                         .Where(x => x.IsVisibleOnSearch)
                         .Select(x => Helper.CreateNewWorkFlowDetailModel(x)).ToList();

            return PartialView("_ShowEntityWorkFlowDetail", model);
        }

        private ActionResult GetEntityProgressReport(DomainEntities.SearchCriteria searchCriteria)
        {
            IEnumerable<OfficeHierarchy> officeHierarchy = GetOfficeHierarchy();

            ICollection<EntityProgressDetail> entityProgressDetails = Business.GetEntityProgressDetails(searchCriteria);

            ICollection<EntityProgressDetailModel> model =
                         entityProgressDetails.Select(item => new EntityProgressDetailModel()
                         {
                             EntityId = item.EntityId,
                             EntityName = item.EntityName,
                             EmployeeCode = item.EmployeeCode,
                             EmployeeName = item.EmployeeName,
                             TypeName = item.TypeName,
                             SeasonName = item.SeasonName,
                             LastPhase = item.LastPhase,
                             LastPhaseDate = item.LastPhaseDate,
                             CurrentPhase = (!item.IsComplete) ? item.CurrentPhase : String.Empty,
                             CurrentPlannedFromDate = (!item.IsComplete) ? (DateTime?)item.CurrentPlannedFromDate : null,
                             CurrentPlannedEndDate = (!item.IsComplete) ? (DateTime?)item.CurrentPlannedEndDate : null,
                             IsComplete = item.IsComplete,
                             AreaName = officeHierarchy.Where(y => y.HQCode == item.HQCode).Select(y => y.AreaName).FirstOrDefault(),
                             AgreementNumber = item.AgreementNumber,
                             AgreementStatus = item.AgreementStatus
                         }).ToList();

            return PartialView("_ShowEntityProgressDetail", model);
        }

        [AjaxOnly]
        [HttpGet]
        public JsonResult ApproveAmount(ApprovalData approveData)
        {
            ApproveDataResponse response = new ApproveDataResponse();
            int result = 0;
            if (approveData == null)
            {
                result = 3;
            }
            else
            {
                DomainEntities.ApprovalData data = new DomainEntities.ApprovalData()
                {
                    Id = approveData.Id,
                    ApproveRef = approveData.RefNo,
                    ApprovedAmt = approveData.ApproveAmount,
                    ApproveComments = approveData.Comment,
                    ApprovedDate = approveData.ApproveDate,
                    ReportType = approveData.ReportType,
                    ApprovedBy = this.CurrentUserStaffCode
                };

                result = Business.SaveApproveAmount(data, IsSuperAdmin, CurrentUserStaffCode);
            }
            response.ApprovedBy = string.Empty; //get ApprovedBy value from respective database table

            switch (result)
            {
                case 0:
                    response.Status = 0;
                    response.Message = "Unable to save the changes, Item is already approved. Please refresh the page.";
                    break;
                case 1:
                    response.Status = 1;
                    response.Message = "Saved Successfully";
                    break;
                case 2:
                    response.Status = 2;
                    response.Message = "Item doesn't exist";
                    break;
                case 3:
                    response.Status = 3;
                    response.Message = "Enter Valid Details";
                    break;
                case 4:
                    response.Status = 4;
                    response.Message = $"Invalid Item Type {approveData.ReportType}";
                    break;
                case 5:
                    response.Status = 5;
                    response.Message = "Invalid Approval Level";
                    break;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Report()
        {
            return View();
        }

        [HttpGet]
        [AjaxOnly]
        public ActionResult EditOrder(long orderId)
        {
            Order order = Business.GetOrder(orderId);

            // if order is already approved - we can't allow edit
            // (order does not exist is taken as order approved)
            if ((order?.IsApproved ?? true) == true)
            {
                ViewBag.Message = "Order items can not be edited. Check order status and try again.";
                return PartialView("_EditOrderMessage");
            }

            IEnumerable<DomainEntities.OrderItem> orderItems = Business.GetOrderItems(orderId);
            IEnumerable<ModelOrderItem> modelOrderItems = orderItems.Select(x => new ModelOrderItem()
            {
                Id = x.Id,
                OrderId = x.OrderId,
                SequenceNumber = x.SequenceNumber,
                ProductCode = x.ProductCode,
                ProductName = x.ProductName,
                UnitPrice = x.UnitPrice,
                Quantity = x.Quantity,
                Amount = x.Amount, // legacy
                DiscountPercent = x.DiscountPercent,
                DiscountedPrice = x.DiscountedPrice,
                ItemPrice = x.ItemPrice,
                GstPercent = x.GstPercent,
                GstAmount = x.GstAmount,
                NetPrice = x.NetPrice,

                UOM = x.UOM,
                RevisedQuantity = x.RevisedQuantity,
                RevisedAmount = x.RevisedAmount,

                RevisedDiscountPercent = x.RevisedDiscountPercent,
                RevisedDiscountedPrice = x.RevisedDiscountedPrice,
                RevisedItemPrice = x.RevisedItemPrice,
                RevisedGstPercent = x.RevisedGstPercent,
                RevisedGstAmount = x.RevisedGstAmount,
                RevisedNetPrice = x.RevisedNetPrice
            }).ToList();

            return PartialView("_EditOrder", modelOrderItems);
        }

        [System.Web.Mvc.HttpPost]
        [AjaxOnly]
        public ActionResult EditOrder(long orderId, long[] itemId, int[] revisedQuantity)
        {
            // ensure order is not approved
            Order order = Business.GetOrder(orderId);

            // if order is already approved - we can't allow edit
            // (order does not exist is taken as order approved)
            if ((order?.IsApproved ?? true) == true)
            {
                ViewBag.Message = "Order items can not be edited. Check order status and try again.";
                return PartialView("_EditOrderMessage");
            }

            try
            {
                List<EditOrderItem> editedItems = new List<EditOrderItem>();
                int itemCount = itemId.Length;
                for (int i = 0; i < itemCount; i++)
                {
                    editedItems.Add(new EditOrderItem()
                    {
                        Id = itemId[i],
                        OrderId = orderId,
                        RevisedQuantity = revisedQuantity[i]
                    });
                }

                Business.SaveRevisedOrderItems(orderId, editedItems, CurrentUserStaffCode);

                ViewBag.Message = "Order items have been saved. Please refresh the page to see the saved changes.";
                return PartialView("_EditOrderMessage");
            }
            catch (ArgumentOutOfRangeException are)
            {
                Business.LogError(nameof(EditOrder), are.ToString(), ">");
                ViewBag.Message = are.Message;
                return PartialView("_EditOrderMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(EditOrder), ex.ToString(), ">");
                ViewBag.Message = $"An error occured while saving order items {ex.Message}. Please refresh the page and try again.";
                return PartialView("_EditOrderMessage");
            }
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult SendMessageOnDemand(IEnumerable<string> staffCodes, string smsText)
        {
            MessageOnDemandResponse response = new MessageOnDemandResponse();

            if ((staffCodes?.Count() ?? 0) == 0)
            {
                response.Message = "No staff codes.";
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            if (String.IsNullOrEmpty(smsText))
            {
                response.Message = "Message is not specified.";
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            smsText = smsText.Trim();
            if (smsText.Length > Utils.MaxSMSTextSize())
            {
                response.Message = $"Message is too large. Max length allowed is {Utils.MaxSMSTextSize()}";
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            if (Utils.SiteConfigData.SMSOnDemandFeatureEnabled == false)
            {
                response.Message = $"Messaging is disabled on server.";
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            long tenantId = Utils.SiteConfigData.TenantId;
            if (!Business.IsSMSEnabled(tenantId))
            {
                response.Message = $"SMS feature disabled for current tenant.";
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            try
            {
                // staff has to be active in Sales Person table +
                // staff either does not exist in TenantEmployee table (not signed up on phone)
                //       OR if exist in Tenant Employee, has to be active in there as well.
                var phoneNumbers = Business.GetStaffPhoneNumbers(staffCodes);

                if (phoneNumbers.Count == 0)
                {
                    response.Status = false;
                    response.Message = "Message is not sent as selected people are either not in employment or not using app.";
                    return Json(response, JsonRequestBehavior.AllowGet);
                }

                long smsLogId = 0;
                response.Status = Business.SendSMS(tenantId, phoneNumbers, smsText, Helper.GetCurrentIstDateTime(), "WebPortal", CurrentUserStaffCode,
                    Utils.SiteConfigData.SMSTemplate,
                    out smsLogId);
                response.Message = response.Status ? $"Message has been sent successfully to {phoneNumbers.Count} number(s). Inactive people / duplicate numbers were removed."
                    : "Attempt to send message failed. Contact Support.";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Log Exception to db
                Business.LogError(nameof(SendMessageOnDemand), ex);
                response.Message = ex.ToString();
                response.Status = false;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        private ActionResult GetActivityByTypeReport(DomainEntities.SearchCriteria searchCriteria)
        {
            Helper.FixDatesInSearchCriteriaForAnyDateRange(searchCriteria);

            IEnumerable<OfficeHierarchy> officeHierarchy = GetOfficeHierarchy();

            ActivityByTypeReportDataModel reportModel = new ActivityByTypeReportDataModel()
            {
                ActivityByTypeReport = Business.GetActivityByTypeReportDataSet(searchCriteria)
                                        .OrderBy(x => x.Date).ThenBy(x => x.Name)
                                        .Select(x => new ActivityByTypeReportModel()
                                        {
                                            Date = x.Date,
                                            Name = x.Name,
                                            StaffCode = x.StaffCode,
                                            ClientType = x.ClientType,
                                            ActivityType = x.ActivityType,
                                            ActivityCount = x.ActivityCount,
                                            AreaName = officeHierarchy.Where(y => y.HQCode == x.HQCode).Select(y => y.AreaName).FirstOrDefault(),
                                        }).ToList()
            };

            return PartialView("_ShowActivityByTypeReport", reportModel);
        }

        [ExcludeRole(XRole = "Admin")]
        public ActionResult ShowCustomers()
        {
            ViewBag.IsSuperAdmin = IsSuperAdmin;
            // user who is in admin role, should use Admin page to see customers
            return View();
        }

        [AjaxOnly]
        [HttpGet]
        [ExcludeRole(XRole = "Admin")]
        public ActionResult GetSearchCustomers(CustomersFilter searchCriteria)
        {
            // user who is in admin role, should use Admin page to see customers

            DomainEntities.CustomersFilter criteria = Helper.ParseCustomersSearchCriteria(searchCriteria);

            criteria.ApplyStaffCodeFilter = true;
            criteria.StaffCode = this.CurrentUserStaffCode;

            IEnumerable<DownloadCustomerExtend> customers = Business.GetCustomers(criteria);
            var model = customers.Select(x => new CustomerModel()
            {
                Code = x.Code,
                CreditLimit = x.CreditLimit,
                LongOutstanding = x.LongOutstanding,
                Name = x.Name,
                Outstanding = x.Outstanding,
                PhoneNumber = x.PhoneNumber,
                Type = x.Type,
                HQCode = x.HQCode,
                Target = x.Target,
                Sales = x.Sales,
                Payment = x.Payment,
                Active = x.Active
            }).ToList();

            ViewBag.IsSuperAdmin = IsSuperAdmin;
            return PartialView("_ShowCustomers", model);
        }

        [ExcludeRole(XRole = "Admin")]
        public ActionResult ShowProducts()
        {
            IEnumerable<OfficeHierarchy> officeHierarchy = GetOfficeHierarchy();
            ViewBag.OfficeHierarchy = officeHierarchy;
            ViewBag.Zones = Business.GetZones(IsSuperAdmin, officeHierarchy).OrderBy(s => s.CodeName);
            return View();
        }

        [AjaxOnly]
        [HttpGet]
        [ExcludeRole(XRole = "Admin")]
        public ActionResult GetSearchProducts(ProductsFilter searchCriteria)
        {
            DomainEntities.ProductsFilter criteria = Helper.ParseProductsSearchCriteria(searchCriteria);

            criteria.IsSuperAdmin = IsSuperAdmin;
            criteria.StaffCode = CurrentUserStaffCode;

            IEnumerable<DashboardProduct> productData = Business.GetProductData(criteria);
            return PartialView("_ShowProducts", productData);
        }

        [ExcludeRole(XRole = "Admin")]
        public ActionResult ShowStaff()
        {
            bool flagGrade = Helper.IsGradeFeatureEnabled();
            if (flagGrade)
            {
                ViewBag.Grade = Business.GetCodeTable("Grade");
            }
            ViewBag.GradesEnabled = flagGrade;
            ViewBag.IsSuperAdmin = IsSuperAdmin;
            ViewBag.Department = Business.GetCodeTable("Department");
            ViewBag.Designation = Business.GetCodeTable("Designation");
            IEnumerable<OfficeHierarchy> officeHierarchy = GetOfficeHierarchy();
            ViewBag.OfficeHierarchy = officeHierarchy;
            return View();
        }

        [AjaxOnly]
        [HttpGet]
        [ExcludeRole(XRole = "Admin")]
        public ActionResult GetSearchSalesPersons(StaffFilter searchCriteria)
        {
            DomainEntities.StaffFilter criteria = Helper.ParseStaffSearchCriteria(searchCriteria);

            if (!Helper.IsGradeFeatureEnabled())
            {
                criteria.ApplyGradeFilter = false;
            }
            if (!criteria.IsSuperAdmin)
            {
                criteria.ApplyAssociationFilter = false;
            }
            //criteria.IsSuperAdmin = IsSuperAdmin;
            //criteria.CurrentUserStaffCode = CurrentUserStaffCode;

            IEnumerable<SalesPersonModel> salesPersonData = Business.GetSalesPersonData(criteria);
            var model = salesPersonData.Select(spd => new SalesPersonViewModel()
            {
                Id = spd.Id,
                IsActive = spd.IsActive,
                Name = spd.Name,
                Phone = spd.Phone,
                StaffCode = spd.StaffCode,
                EmployeeId = spd.EmployeeId,
                OnWeb = spd.OnWeb,
                HQCode = spd.HQCode,
                HQName = spd.HQName,
                Grade = spd.Grade,
                Department = spd.Department,
                Designation = spd.Designation,
                Ownership = spd.Ownership,
                VehicleType = spd.VehicleType,
                FuelType = spd.FuelType,
                VehicleNumber = spd.VehicleNumber,
                BusinessRole = spd.BusinessRole,
                OverridePrivateVehicleRatePerKM = spd.OverridePrivateVehicleRatePerKM,
                TwoWheelerRatePerKM = spd.TwoWheelerRatePerKM,
                FourWheelerRatePerKM = spd.FourWheelerRatePerKM
            }).ToList();

            ViewBag.AllowSalesPersonMaintenanceOnWeb = false;
            ViewBag.GradesEnabled = Helper.IsGradeFeatureEnabled();
            ViewBag.IsSuperAdmin = IsSuperAdmin;
            IEnumerable<OfficeHierarchy> officeHierarchy = GetOfficeHierarchy();
            ViewBag.OfficeHierarchy = officeHierarchy;
            return PartialView("_ShowStaff", model);
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult EditIssueReturn(long issueReturnId)
        {
            DomainEntities.SearchCriteria sc = Helper.GetDefaultSearchCriteria();
            sc.ApplyIdFilter = true;
            sc.FilterId = issueReturnId;

            ICollection<IssueReturn> issueReturns = Business.GetIssueReturns(sc);

            if ((issueReturns?.Count ?? 0) != 1)
            {
                return PartialView("_Error");
            }

            IssueReturnModel model = issueReturns.Select(x => CreateNewIssueReturnModel(x)).First();
            model.TransactionDateAsText = model.TransactionDate.ToString("dd-MM-yyyy");

            if (!model.IsPending)
            {
                return PartialView("_Error");
            }

            FillEditIssueReturnViewBag(model.TypeName);

            return PartialView("_EditIssueReturn", model);
        }

        [AjaxOnly]
        [HttpPost]
        public ActionResult EditIssueReturn(IssueReturnModel model)
        {
            if (!ModelState.IsValid)
            {
                FillEditIssueReturnViewBag(model.TypeName);
                return PartialView("_EditIssueReturn", model);
            }

            IssueReturn origRec = model.OrigRec;

            origRec.AppliedTransactionType = model.AppliedTransactionType;
            origRec.TransactionDate = model.TransactionDate;
            origRec.AppliedQuantity = model.AppliedQuantity;
            origRec.Status = model.Status;
            origRec.Comments = Utils.TruncateString(model.Comments, 100);
            origRec.AppliedItemMasterId = model.SelectedItem.Id;

            var rateRec = model.SelectedItem.TypeNames.FirstOrDefault(x => x.TypeName.Equals(model.OrigRec.TypeName, StringComparison.OrdinalIgnoreCase));
            origRec.AppliedItemRate =
                    ((origRec.AppliedTransactionType.Contains("Issue")) ? rateRec?.Rate : rateRec?.ReturnRate) ?? 0.0M;

            origRec.CurrentUser = CurrentUserStaffCode;
            origRec.CyclicCount = model.CyclicCount;
            origRec.CurrentIstTime = Helper.GetCurrentIstDateTime();
            origRec.StockBalanceRecId = model.StockBalanceRec?.Id ?? 0;

            try
            {
                DBSaveStatus status = Business.SaveIssueReturnData(origRec);
                if (status == DBSaveStatus.CyclicCheckFail)
                {
                    ModelState.AddModelError("", "An error occured while saving changes. Please refresh the page and try again.");
                    FillEditIssueReturnViewBag(model.TypeName);
                    return PartialView("_EditIssueReturn", model);
                }

                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(EditIssueReturn), ex.ToString(), ">");
                ModelState.AddModelError("", ex.Message);
                FillEditIssueReturnViewBag(model.TypeName);
                return PartialView("_EditIssueReturn", model);
            }
        }

        private void FillEditIssueReturnViewBag(string typeName)
        {
            ViewBag.ActivityTypes = Business.GetCodeTable("IssueReturnActivityType");
            ViewBag.ItemTypes = Business.GetCodeTable("ItemType");
            ViewBag.Status = Business.GetCodeTable("BankDetailStatus");

            ICollection<DomainEntities.ItemMaster> itemsData = Business.GetAllItemMaster();
            ViewBag.ItemMaster = itemsData
                .Where(x => x.TypeNames.Any(y => y.TypeName.Equals(typeName, StringComparison.OrdinalIgnoreCase)))
                .OrderBy(x => x.ItemCode)
                .Select(x => new DownloadItemMaster()
                {
                    ItemId = x.Id,
                    ItemCode = x.ItemCode.Replace('\'', ' '),
                    ItemDesc = x.ItemDesc,
                    Category = x.Category,
                    Unit = x.Unit,
                    Classification = x.Classification,
                }).ToList();
        }

        private IssueReturnModel CreateNewIssueReturnModel(IssueReturn x)
        {
            return Helper.CreateNewIssueReturnModel(x);
        }

        //Author:Ajith, Purpose:For displaying CustomerQuestionnaire search table
        private ActionResult GetCustomerQuestionnaire(DomainEntities.SearchCriteria searchCriteria)
        {
            ICollection<DomainEntities.Questionnaire.CustomerQuestionnaire> CustomerQuestionnaires = Business.GetCustomerQuestionnaire(searchCriteria);
            ICollection<CustomerQuestionnaireModel> model = CustomerQuestionnaires.Select(x => new CustomerQuestionnaireModel()
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                EmployeeName = x.EmployeeName,
                CustomerCode = x.CustomerCode,
                DateCreated = x.DateCreated,
                EntityType = x.EntityType,
                EntityName = x.EntityName,
                EmployeeCode = x.EmployeeCode,
                ActivityId = x.ActivityId,
                QuestionPaperName = x.QuestionPaperName
            }).ToList();

            // DateCreated in CustomerQuestionnaire has time - so we want to include all records for upto dateto add day(1) in dblayer
            searchCriteria.DateTo = searchCriteria.DateTo.AddDays(-1);
            IEnumerable<DomainEntities.Questionnaire.CustomerQuestionnairedetails> CustomerQuestionnairedet = Business.GetCustomerQuestionnaireAlldetails(searchCriteria);
            IEnumerable<CustomerQuestionnaireModel> modeldet = CustomerQuestionnairedet.Select(x => new CustomerQuestionnaireModel()
            {
                Id = x.Id,
                DateCreated = x.DateCreated,
                EmployeeCode = x.EmployeeCode,
                EmployeeName = x.EmployeeName,
                CustomerCode = x.CustomerCode,
                EntityType = x.EntityType,
                EntityName = x.EntityName,
                QText = x.QText,
                AText = x.AText,
                TextComment = x.TextComment,
                QuestionPaperName = x.QuestionPaperName
            }).ToList();

            ViewBag.CustModel = modeldet.ToList();
            searchCriteria.DateTo = searchCriteria.DateTo.AddDays(-1);
            return PartialView("_ShowCustomerQuestionnaire", model);
        }

        //Author:Ajith, Purpose:For displaying dealer questionnaire details
        [AjaxOnly]
        [HttpGet]
        public ActionResult ShowCustomerQuestionnairedetails(long QuestionnaireID)
        {
            IEnumerable<DomainEntities.Questionnaire.CustomerQuestionnairedetails> detailsQA = Business.GetCustomerQuestionnaireQA(QuestionnaireID);
            IEnumerable<DomainEntities.Questionnaire.CustomerQuestionnairedetails> details = Business.GetCustomerQuestionnairedetails(QuestionnaireID);

            if (detailsQA == null || details == null)
            {
                ViewBag.Message = "Exception";
                return PartialView("_ShowCustomerQuestionnairedetails");
            }
            IEnumerable<CustomerQuestionnaireModel> model = details.Select(x => new CustomerQuestionnaireModel()
            {
                QuestionPaperName = x.QuestionPaperName,
                QText = x.QText,
                AText = x.AText,
                TextComment = x.TextComment,
                HasTextComment = x.HasTextComment,
                EmployeeCode = x.EmployeeCode,
                EmployeeName = x.EmployeeName,
                EntityName = x.EntityName,
                EntityType = x.EntityType,
                QuestionPaperQuestionId = x.QuestionPaperQuestionId,
                QuestionPaperAnswerId = x.QuestionPaperAnswerId,
                QuestionTypeName = x.QuestionTypeName,
                DateCreated = x.DateCreated,
                ActivityId = x.ActivityId,
                CustomerCode = x.CustomerCode

            }).ToList();

            ViewBag.AnswerDetail = detailsQA;
            return PartialView("_ShowCustomerQuestionnairedetails", model);

        }

        //Author : Kartik, Prupose:FollowupTask functionality
        [CheckRightsAuthorize(Feature = FeatureEnum.FollowUpTaskOption)]
        public ActionResult FollowUpTasks()
        {
            ViewBag.Projects = Business.GetProjectNames();
            IEnumerable<OfficeHierarchy> officeHierarchy = GetOfficeHierarchy();
            ViewBag.OfficeHierarchy = officeHierarchy;

            PutActivityTypeInViewBag();
            PutTaskStatusInViewBag();
            PutClientTypeViewBag();

            ViewBag.IsSuperAdmin = IsSuperAdmin;
            return View();
        }
        [CheckRightsAuthorize(Feature = FeatureEnum.LeaveFeature)]
        public ActionResult LeaveModule()
        {
            IEnumerable<OfficeHierarchy> officeHierarchy = GetOfficeHierarchy();
            ViewBag.OfficeHierarchy = officeHierarchy;

            PutLeaveTypesInViewBag();
            PutLeaveStatusInViewBag();
            ViewBag.IsSuperAdmin = IsSuperAdmin;
            return View();
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.FollowUpTaskOption)]
        public ActionResult GetFollowUpTasks(FollowUpTaskFilter searchCriteria)
        {
            DomainEntities.FollowUpTaskFilter criteria = Helper.ParseFollowUpTaskSearchCriteria(searchCriteria);
            ICollection<FollowUpTask> tasks = Business.GetFollowUpTasks(criteria);
            ICollection<FollowUpTaskViewModel> model = tasks.Select(x => new FollowUpTaskViewModel()
            {
                Id = x.Id,
                XRefProjectId = x.XRefProjectId,
                ProjectName = x.ProjectName,
                Description = x.Description,
                ActivityType = x.ActivityType,
                ClientType = x.ClientType,
                ClientName = x.ClientName,
                ClientCode = x.ClientCode,
                PlannedStartDate = x.PlannedStartDate,
                PlannedEndDate = x.PlannedEndDate,
                ActualStartDate = x.ActualStartDate,
                ActualEndDate = x.ActualEndDate,
                Comments = x.Comments,
                Status = x.Status,
                CyclicCount = x.CyclicCount,
                IsActive = x.IsActive,
                IsCreatedOnPhone = x.IsCreatedOnPhone,
                DateCreated = x.DateCreated,
                DateUpdated = x.DateUpdated,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                ActivityCount = x.ActivityCount
            }).ToList();

            ViewBag.IsSuperAdmin = IsSuperAdmin;
            return PartialView("_ShowFollowUpTasks", model);
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.FollowUpTaskOption)]
        public ActionResult EditFollowUpTask(long taskId)
        {
            DomainEntities.FollowUpTask rec = Business.GetSingleTask(taskId);

            //ViewBag.IsEditAllowed = IsSuperAdmin;

            FollowUpTaskViewModel model =
                         new FollowUpTaskViewModel()
                         {
                             Id = rec.Id,
                             XRefProjectId = rec.XRefProjectId,
                             Description = rec.Description,
                             ClientType = rec.ClientType,
                             ClientName = rec.ClientName,
                             ClientCode = rec.ClientCode,
                             ActivityType = rec.ActivityType,
                             PlannedStartDate = rec.PlannedStartDate,
                             PlannedEndDate = rec.PlannedEndDate,
                             ActualStartDate = rec.ActualStartDate,
                             ActualEndDate = rec.ActualEndDate,
                             Status = rec.Status,
                             Comments = rec.Comments,
                             CyclicCount = rec.CyclicCount,
                             IsActive = rec.IsActive
                         };

            model.PlannedStartDateAsText = model.PlannedStartDate.ToString("dd-MM-yyyy");
            model.PlannedEndDateAsText = model.PlannedEndDate.ToString("dd-MM-yyyy");

            ViewBag.Projects = Business.GetProjectNames();
            //IEnumerable<OfficeHierarchy> officeHierarchy = GetOfficeHierarchy();
            //ViewBag.OfficeHierarchy = officeHierarchy;

            PutActivityTypeInViewBag();
            PutTaskStatusInViewBag();
            //PutClientTypeViewBag();
            //ViewBag.ClientNameList = Business.GetCustomerAndEntityName();

            return PartialView("_EditFollowUpTask", model);
        }

        [AjaxOnly]
        [HttpPost]
        [CheckRightsAuthorize(Feature = FeatureEnum.FollowUpTaskOption)]
        public ActionResult EditFollowUpTask(FollowUpTaskViewModel model)
        {
            ViewBag.Projects = Business.GetProjectNames();
            IEnumerable<OfficeHierarchy> officeHierarchy = GetOfficeHierarchy();
            ViewBag.OfficeHierarchy = officeHierarchy;

            PutActivityTypeInViewBag();
            PutTaskStatusInViewBag();
            //PutClientTypeViewBag();

            //ViewBag.IsEditAllowed = true;

            if (!ModelState.IsValid)
            {
                return PartialView("_EditFollowUpTask", model);
            }

            DomainEntities.FollowUpTask rec = (model.Id > 0) ?
                                Business.GetSingleTask(model.Id) :
                                new DomainEntities.FollowUpTask();

            rec.Id = model.Id;
            rec.XRefProjectId = model.XRefProjectId;
            rec.Description = Utils.TruncateString(model.Description, 200);
            rec.ActivityType = model.ActivityType;
            rec.ClientType = model.ClientType ?? "";
            rec.ClientName = model.ClientName ?? "";
            rec.ClientCode = model.ClientCode ?? "";
            rec.PlannedStartDate = model.PlannedStartDate;
            rec.PlannedEndDate = model.PlannedEndDate;
            rec.ActualStartDate = model.ActualStartDate;
            rec.ActualEndDate = model.ActualEndDate;
            rec.Status = model.Status;
            rec.IsActive = model.IsActive;
            rec.Comments = Utils.TruncateString(model.Comments, 2048);

            rec.CurrentUser = CurrentUserStaffCode;

            rec.CyclicCount = model.CyclicCount;

            try
            {
                DBSaveStatus status = Business.SaveFollowUpTaskData(rec);
                if (status == DBSaveStatus.CyclicCheckFail)
                {
                    ModelState.AddModelError("", "An error occured while saving changes. Please refresh the page and try again.");
                    return PartialView("_EditFollowUpTask", model);
                }
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(EditFollowUpTask), ex.ToString(), ">");
                ModelState.AddModelError("", ex.Message);
                return PartialView("_EditFollowUpTask", model);
            }
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.FollowUpTaskOption)]
        public ActionResult AddFollowUpTask()
        {
            FollowUpTaskViewModel model =
                         new FollowUpTaskViewModel()
                         {
                             Id = 0,
                             Description = "",
                             ClientName = "",
                             ClientCode = "",
                             XRefProjectId = 0,
                             PlannedStartDate = Helper.GetCurrentIstDateTime(),
                             PlannedEndDate = Helper.GetCurrentIstDateTime(),
                             CyclicCount = 0
                         };

            ViewBag.IsEditAllowed = true;
            model.PlannedStartDateAsText = model.PlannedStartDate.ToString("dd-MM-yyyy");
            model.PlannedEndDateAsText = model.PlannedEndDate.ToString("dd-MM-yyyy");

            ViewBag.Projects = Business.GetProjectNames();
            //IEnumerable<OfficeHierarchy> officeHierarchy = GetOfficeHierarchy();
            //ViewBag.OfficeHierarchy = officeHierarchy;

            PutActivityTypeInViewBag();
            PutTaskStatusInViewBag();
            //PutClientTypeViewBag();

            return PartialView("_EditFollowUpTask", model);
        }

        private void PutActivityTypeInViewBag()
        {
            ViewBag.ActivityType = Business.GetCodeTable("ActivityType");
        }
        private void PutTaskStatusInViewBag()
        {
            ViewBag.TaskStatus = Business.GetCodeTable("TaskStatus");
        }
        private void PutLeaveTypesInViewBag()
        {
            ViewBag.LeaveType = Business.GetCodeTable("LeaveType");
        }
        private void PutLeaveStatusInViewBag()
        {
            ViewBag.BankDetailStatus = Business.GetCodeTable("BankDetailStatus");
        }
        private void PutClientTypeViewBag()
        {
            ViewBag.ClientType = Business.GetCodeTable("CustomerType");
        }

        // Here we are getting Visible HQ Codes
        private void PutVisibleHQListInViewBag()
        {
            var securityContext = Helper.GetSecurityContextUser(HttpContext.User);
            ViewBag.HeadQuarters = Business.GetHeadQuarters(securityContext.Item1, GetOfficeHierarchy());
        }

        private void PutEmployeeListViewBag()
        {
            var securityContext = Helper.GetSecurityContextUser(HttpContext.User);
            IEnumerable<string> visibleStaffCodes = Business.GetVisibleStaffCodes(securityContext.Item1, securityContext.Item2);
            IEnumerable<SalesPersonModel> salesPersonData = Business.GetSelectedSalesPersonData(visibleStaffCodes);

            ViewBag.SalesPersons = salesPersonData;
        }

        // Here we are getting Visible staff code for selected entity on task
        private void PutEmployeeListforSelectedEntityViewBag(Entity ent)
        {
            var securityContext = Helper.GetSecurityContextUser(HttpContext.User);
            ICollection<string> visibleStaffCodes = Business.GetVisibleStaffCodes(securityContext.Item1, securityContext.Item2);
            ICollection<string> visibleStaffsForEntity = Business.GetVisibleStaffCodesForEntity(securityContext.Item1, ent.HQCode, visibleStaffCodes);
            IEnumerable<SalesPersonModel> salesPersonData = Business.GetSelectedSalesPersonData(visibleStaffsForEntity);

            ViewBag.SalesPersons = salesPersonData;
        }

        // Link Entity to FollowUp Task

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.FollowUpTaskOption)]
        public ActionResult EditFollowUpTaskEntity(long taskId)
        {
            DomainEntities.FollowUpTask rec = Business.GetSingleTask(taskId);

            //var securityContext = Helper.GetSecurityContextUser(HttpContext.User);
            //IEnumerable<CodeTableEx> hqDetails = Business.GetHeadQuarters(securityContext.Item1, GetOfficeHierarchy());
            //IEnumerable<string> hqs = hqDetails.Select(x => x.Code).Distinct();
            //ViewBag.Entities = Business.GetEntitiesForTask(hqs);

            FollowUpTaskEntityTypeHQ model =
                         new FollowUpTaskEntityTypeHQ()
                         {
                             Id = rec.Id,
                         };

            PutClientTypeViewBag();
            PutVisibleHQListInViewBag();

            return PartialView("_FollowUpTaskEntityCriteria", model);
        }

        [HttpPost]
        [CheckRightsAuthorize(Feature = FeatureEnum.FollowUpTaskOption)]
        public ActionResult EditFollowUpTaskEntity(FollowUpTaskEntityTypeHQ model)
        {
            DomainEntities.FollowUpTask rec = Business.GetSingleTask(model.Id);
            PutClientTypeViewBag();
            PutVisibleHQListInViewBag();

            if (!ModelState.IsValid)
            {
                return PartialView("_FollowUpTaskEntityCriteria", model);
            }

            ViewBag.Entities = Business.GetEntitiesForTask(model.ClientType, model.HQCode);

            FollowUpTaskEntity ent =
                         new FollowUpTaskEntity()
                         {
                             Id = rec.Id,
                             ClientType = model.ClientType,
                             HQCode = model.HQCode,
                             ClientNameOld = rec.ClientName,
                             ClientCodeOld = rec.ClientCode,
                         };

            return PartialView("_EditFollowUpTaskEntity", ent);
        }

        [HttpPost]
        [CheckRightsAuthorize(Feature = FeatureEnum.FollowUpTaskOption)]
        public ActionResult SaveFollowUpTaskEntity(FollowUpTaskEntity model)
        {
            DomainEntities.FollowUpTask task = Business.GetSingleTask(model.Id);

            ICollection<Entity> entities = Business.GetEntitiesForTask(model.ClientType, model.HQCode);

            ViewBag.Entities = entities;

            if (!ModelState.IsValid)
            {
                return PartialView("_EditFollowUpTaskEntity", model);
            }

            DomainEntities.FollowUpTask rec = (model.Id > 0) ?
                     Business.GetSingleTask(model.Id) :
                     new DomainEntities.FollowUpTask();

            rec.Id = model.Id;
            rec.ClientType = model.ClientType;
            rec.ClientName = entities.Where(y => y.Id.ToString().Equals(model.ClientCode)).Select(x => x.EntityName).FirstOrDefault();
            rec.ClientCode = model.ClientCode ?? "";

            rec.CurrentUser = CurrentUserStaffCode;

            rec.CyclicCount = task.CyclicCount;

            try
            {
                DBSaveStatus status = Business.SaveFollowUpTaskData(rec);
                if (status == DBSaveStatus.CyclicCheckFail)
                {
                    ModelState.AddModelError("", "An error occured while saving changes. Please refresh the page and try again.");
                    return PartialView("_EditFollowUpTaskEntity", model);
                }
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(EditFollowUpTask), ex.ToString(), ">");
                ModelState.AddModelError("", ex.Message);
                return PartialView("_EditFollowUpTaskEntity", model);
            }
        }

        // Followup Task Assignment

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.FollowUpTaskOption)]
        public ActionResult GetTaskAssignments(long Id, string rowId, string parentRowId) // here it is Task Id
        {
            DomainEntities.FollowUpTask rec = Business.GetSingleTask(Id);

            ViewBag.IsEmptyClientCode = false;

            if (string.IsNullOrEmpty(rec.ClientCode))
            {
                ViewBag.IsEmptyClientCode = true;
            }

            ICollection<DomainEntities.TaskAssignments> items = Business.GetTaskAssignment(Id);

            ICollection<TaskAssignmentModel> model =
                         items.Select(x => new TaskAssignmentModel()
                         {
                             Id = x.Id,
                             XRefTaskId = x.XRefTaskId,
                             EmployeeId = x.EmployeeId,
                             EmployeeCode = x.EmployeeCode,
                             EmployeeName = x.EmployeeName,
                             StartDate = x.StartDate,
                             EndDate = x.EndDate,
                             IsAssigned = x.IsAssigned,
                             IsSelfAssigned = x.IsSelfAssigned,
                             Comments = x.Comments,
                             DateCreated = x.DateCreated,
                             DateUpdated = x.DateUpdated,
                             CreatedBy = x.CreatedBy,
                             UpdatedBy = x.UpdatedBy,
                             ActivityCount = x.ActivityCount
                         }).ToList();

            ViewBag.TaskId = Id;
            ViewBag.TaskDesc = rec?.Description ?? "";

            ViewBag.RowId = rowId;
            ViewBag.ParentRowId = parentRowId;
            return PartialView("_ListTaskAssignment", model);
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.FollowUpTaskOption)]
        public ActionResult EditTaskAssignment(long taskId, long assignmentId)
        {
            DomainEntities.FollowUpTask p = Business.GetSingleTask(taskId);
            DomainEntities.TaskAssignments rec = Business.GetSingleTaskAssignment(taskId, assignmentId);

            var ent = Business.GetEntityDetailsForTaskAssignment(p.ClientType, p.ClientCode);

            PutEmployeeListforSelectedEntityViewBag(ent);

            ViewBag.IsEditAllowed = IsSuperAdmin;

            TaskAssignmentModel model =
                         new TaskAssignmentModel()
                         {
                             Id = rec.Id,
                             XRefTaskId = rec.XRefTaskId,
                             Description = p.Description,
                             EmployeeId = rec.EmployeeId,
                             EmployeeCode = rec.EmployeeCode,
                             EmployeeName = rec.EmployeeName,
                             StartDate = rec.StartDate,
                             EndDate = rec.EndDate,
                             IsAssigned = rec.IsAssigned,
                             IsSelfAssigned = rec.IsSelfAssigned,
                             Comments = rec.Comments,
                             DateCreated = rec.DateCreated,
                             DateUpdated = rec.DateUpdated,
                             CreatedBy = rec.CreatedBy,
                             UpdatedBy = rec.UpdatedBy
                         };

            model.StartDateAsText = model.StartDate.ToString("dd-MM-yyyy");
            model.EndDateAsText = model.EndDate.ToString("dd-MM-yyyy");

            //PutEmployeeListViewBag();

            return PartialView("_EditTaskAssignment", model);
        }

        [AjaxOnly]
        [HttpPost]
        [CheckRightsAuthorize(Feature = FeatureEnum.FollowUpTaskOption)]
        public ActionResult EditTaskAssignment(TaskAssignmentModel model)
        {

            DomainEntities.FollowUpTask p = Business.GetSingleTask(model.XRefTaskId);

            var ent = Business.GetEntityDetailsForTaskAssignment(p.ClientType, p.ClientCode);

            PutEmployeeListforSelectedEntityViewBag(ent);

            if (!ModelState.IsValid)
            {
                return PartialView("_EditTaskAssignment", model);
            }

            EmployeeRecord empRec = Business.GetTenantEmployee(model.EmployeeCode);

            if (empRec == null)
            {
                return PartialView("_CustomError", $"User : {model.EmployeeCode} not registered on MyDay.  Please inform user to register on MyDay App and try again.");
            }

            IEnumerable<DomainEntities.TaskAssignments> taskAssignments = Business.GetSingleTaskEmployeeAssignment(model.XRefTaskId, empRec.EmployeeId);

            foreach (var Id in taskAssignments)
            {
                if (model.StartDate <= Id.EndDate && Id.StartDate <= model.EndDate && model.Id != Id.Id)
                {
                    return PartialView("_CustomError", "Assignments dates are overlapping.");
                }
            }

            DomainEntities.TaskAssignments rec = (model.Id > 0) ?
                                Business.GetSingleTaskAssignment(model.XRefTaskId, model.Id) :
                                new DomainEntities.TaskAssignments();

            rec.Id = model.Id;
            rec.XRefTaskId = model.XRefTaskId;
            rec.EmployeeId = empRec.EmployeeId;
            rec.StartDate = model.StartDate;
            rec.EndDate = model.EndDate;
            rec.IsAssigned = model.IsAssigned;
            rec.IsSelfAssigned = model.EmployeeCode == CurrentUserStaffCode ? true : false;
            rec.Comments = Utils.TruncateString(model.Comments, 2000);

            rec.CurrentUser = CurrentUserStaffCode;

            try
            {
                DBSaveStatus status = Business.SaveTaskAssignmentData(rec);
                if (status == DBSaveStatus.CyclicCheckFail)
                {
                    ModelState.AddModelError("", "An error occured while saving changes. Please refresh the page and try again.");
                    return PartialView("_EditTaskAssignment", model);
                }
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(EditTaskAssignment), ex.ToString(), ">");
                ModelState.AddModelError("", ex.Message);
                return PartialView("_EditTaskAssignment", model);
            }
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.FollowUpTaskOption)]
        public ActionResult AddTaskAssignment(long taskId)
        {
            DomainEntities.FollowUpTask p = Business.GetSingleTask(taskId);

            var ent = Business.GetEntityDetailsForTaskAssignment(p.ClientType, p.ClientCode);

            PutEmployeeListforSelectedEntityViewBag(ent);

            TaskAssignmentModel model =
                         new TaskAssignmentModel()
                         {
                             Id = 0,
                             XRefTaskId = taskId,
                             Description = p.Description,
                             EmployeeName = "",
                             StartDate = Helper.GetCurrentIstDateTime(),
                             EndDate = Helper.GetCurrentIstDateTime(),
                         };

            ViewBag.IsEditAllowed = true;
            model.StartDateAsText = model.StartDate.ToString("dd-MM-yyyy");
            model.EndDateAsText = model.EndDate.ToString("dd-MM-yyyy");

            return PartialView("_EditTaskAssignment", model);
        }

        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.FollowUpTaskOption)]
        public ActionResult GetTaskAction(long taskId) // here it is Task Id
        {
            ICollection<DomainEntities.FollowUpTaskAction> rec = Business.GetTaskActions(taskId);
            IEnumerable<long> activityIds = rec.Select(x => x.XRefActivityId);
            IEnumerable<DomainEntities.ActivityMapData> activityData = Business.GetManyActivityData(activityIds);

            List<ActivityMapDataModel> dataModel = activityData.Select(ad => new ActivityMapDataModel()
            {
                ActivityId = ad.ActivityId,
                ClientName = ad.ClientName,
                ClientPhone = ad.ClientPhone,
                ClientType = ad.ClientType,
                ActivityType = ad.ActivityType,
                ActivityAmount = ad.ActivityAmount,
                AtLocation = ad.AtLocation,
                Comments = ad.Comments,
                At = ad.At,
                Latitude = ad.Latitude,
                Longitude = ad.Longitude,
                EmployeeDayId = ad.EmployeeDayId,
                ImageCount = ad.ImageCount,
                ContactCount = ad.ContactCount
            }).ToList();

            EmployeeDayData empDayData = Business.GetEmployeeDayData(activityData.FirstOrDefault()?.EmployeeDayId ?? 0);
            ViewBag.ActivityDate = empDayData?.Date ?? DateTime.MinValue;
            ViewBag.EmployeeName = empDayData?.EmployeeName ?? "";

            return View("Activity", dataModel);

            //ViewBag.TaskId = Id;
            //ViewBag.TaskDesc = rec?.Description ?? "";

            //ViewBag.RowId = rowId;
            //ViewBag.ParentRowId = parentRowId;
            //return PartialView("_ListTaskAssignment", model);
        }

        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.FollowUpTaskOption)]
        public ActionResult GetTaskAssignmentAction(long taskId, long AssignmentId) // here it is Task Id and task assignment Id
        {
            ICollection<DomainEntities.FollowUpTaskAction> rec = Business.GetTaskAssignmentActions(taskId, AssignmentId);
            IEnumerable<long> activityIds = rec.Select(x => x.XRefActivityId);
            IEnumerable<DomainEntities.ActivityMapData> activityData = Business.GetManyActivityData(activityIds);

            List<ActivityMapDataModel> dataModel = activityData.Select(ad => new ActivityMapDataModel()
            {
                ActivityId = ad.ActivityId,
                ClientName = ad.ClientName,
                ClientPhone = ad.ClientPhone,
                ClientType = ad.ClientType,
                ActivityType = ad.ActivityType,
                ActivityAmount = ad.ActivityAmount,
                AtLocation = ad.AtLocation,
                Comments = ad.Comments,
                At = ad.At,
                Latitude = ad.Latitude,
                Longitude = ad.Longitude,
                EmployeeDayId = ad.EmployeeDayId,
                ImageCount = ad.ImageCount,
                ContactCount = ad.ContactCount
            }).ToList();

            EmployeeDayData empDayData = Business.GetEmployeeDayData(activityData.FirstOrDefault()?.EmployeeDayId ?? 0);
            ViewBag.ActivityDate = empDayData?.Date ?? DateTime.MinValue;
            ViewBag.EmployeeName = empDayData?.EmployeeName ?? "";

            return View("Activity", dataModel);

        }

        //Author:Rajesh V, Purpose:For displaying Farmer Summary Report; Date:07-10-2021
        private ActionResult GetFarmerSummaryReport(DomainEntities.SearchCriteria searchCriteria)
        {
            IEnumerable<DomainEntities.FarmerSummaryReport> farmerSummary = Business.GetFarmerSummary(searchCriteria);
            IEnumerable<FarmerSummaryReportModel> model = farmerSummary.Select(x => new FarmerSummaryReportModel()
            {
                AgreementId = x.AgreementId,
                AgreementNumber = x.AgreementNumber,
                EntityName = x.EntityName,
                UniqueId = x.UniqueId,
                Crop = x.Crop,
                SeasonName = x.SeasonName
            }).ToList();

            return PartialView("_ShowFarmerSummaryReport", model);
        }

        //Added for Dealers Not Met Report
        private ActionResult GetDealersNotMetReport(DomainEntities.SearchCriteria sc)
        {
            Helper.FixDatesInSearchCriteriaForAnyDateRange(sc);
            IEnumerable<DomainEntities.DealersNotMetReport> dnm = Business.GetDealerNotMet(sc);
            IEnumerable<DealersNotMetReportDataModel> entDNM = dnm.Select(d => new DealersNotMetReportDataModel()
            {
                CustomerCode = d.CustomerCode,
                CustomerName = d.CustomerName,
                EmployeeCode = d.EmployeeCode,
                EmployeeName = d.EmployeeName,
                HQCode = d.HQCode,
                HQName = d.HQName,
                LastActivity = d.LastActivity,
                IsActive = d.IsActive,
                ContactNumber = d.ContactNumber
            }

            ).ToList();
            return PartialView("_DealerNotMet", entDNM);

        }



        //Author:Kartik V, Purpose:Edit ExpenseItem for deducting amount; Date:08-03-2022

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.ExpenseFeature)]
        public ActionResult EditExpenseItem(long expenseItemId)
        {
            DomainEntities.ExpenseItem rec = Business.GetExpenseItem(expenseItemId);

            //ViewBag.IsEditAllowed = IsSuperAdmin;

            ExpenseItemViewModel model =
                         new ExpenseItemViewModel()
                         {
                             Id = rec.Id,
                             ExpenseId = rec.ExpenseId,
                             SequenceNumber = rec.SequenceNumber,
                             ExpenseType = rec.ExpenseType,
                             TransportType = rec.TransportType,
                             Amount = rec.Amount,
                             DeductedAmount = rec.DeductedAmount,
                             RevisedAmount = rec.RevisedAmount,
                             OdometerStart = rec.OdometerStart,
                             OdometerEnd = rec.OdometerEnd,
                             FuelType = rec.FuelType,
                             FuelQuantityInLiters = rec.FuelQuantityInLiters,
                             Comment = rec.Comment
                         };

            return PartialView("_EditExpenseItem", model);
        }

        [AjaxOnly]
        [HttpPost]
        [CheckRightsAuthorize(Feature = FeatureEnum.ExpenseFeature)]
        public ActionResult EditExpenseItem(ExpenseItemViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_EditExpenseItem", model);
            }

            DomainEntities.ExpenseItem rec = Business.GetExpenseItem(model.Id);

            rec.DeductedAmount = model.DeductedAmount;
            rec.RevisedAmount = model.RevisedAmount;

            try
            {
                DBSaveStatus status = Business.SaveExpenseItemData(rec);
                if (status == DBSaveStatus.CyclicCheckFail)
                {
                    ModelState.AddModelError("", "An error occurred while saving changes. Please refresh the page and try again.");
                    return PartialView("_EditExpenseItem", model);
                }
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(EditExpenseItem), ex.ToString(), ">");
                ModelState.AddModelError("", ex.Message);
                return PartialView("_EditExpenseItem", model);
            }
        }
        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.LeaveFeature)]
        public ActionResult GetDashboardLeaves(LeaveFilter searchCriteria)
        {
            DomainEntities.LeaveFilter criteria = Helper.ParseLeaveSearchCriteria(searchCriteria);
            ICollection<DashboardLeave> leave = Business.GetDashboardLeave(criteria);
            ICollection<DashboardLeaveModel> model = leave.Select(x => new DashboardLeaveModel()

            {
                Id = x.Id,
                EmployeeCode = x.EmployeeCode,
                EmployeeName = x.EmployeeName,
                LeaveType = x.LeaveType,
                StartDate = x.StartDate.ToString("dd-MM-yyyy"),
                EndDate = x.EndDate.ToString("dd-MM-yyyy"),
                DaysCountExcludingHolidays = x.DaysCountExcludingHolidays,
                LeaveReason = x.LeaveReason,
                Comment = x.Comment,
                LeaveStatus = x.LeaveStatus,
                ApproveNotes = x.ApproveNotes,
                UpdatedBy = x.UpdatedBy,
                DateUpdated = Helper.ConvertUtcTimeToIst(x.DateUpdated),
                IsEditAllowed = x.IsEditAllowed

            }).ToList();

            ViewBag.IsSuperAdmin = IsSuperAdmin;
            return PartialView("_ShowLeaveModule", model);
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.LeaveFeature)]
        public ActionResult EditLeave(long id)
        {
            PutLeaveStatusInViewBag();
            DomainEntities.DashboardLeave dl = Business.GetSingleLeave(id);

            DashboardLeaveModel model = new DashboardLeaveModel
            {
                Id = dl.Id,
                EmployeeCode = dl.EmployeeCode,
                EmployeeName = dl.EmployeeName,
                LeaveType = dl.LeaveType,
                StartDate = dl.StartDate.ToString("dd-MM-yyyy"),
                EndDate = dl.EndDate.ToString("dd-MM-yyyy"),
                DaysCountExcludingHolidays = dl.DaysCountExcludingHolidays,
                LeaveReason = dl.LeaveReason,
                Comment = dl.Comment,
                ApproveNotes = dl.ApproveNotes,
                UpdatedBy = dl.UpdatedBy,
                DateUpdated = dl.DateUpdated,

            };
            var availableLeaves = Business.GetDownloadAvailableLeave(model.EmployeeCode);
            var availableLeavesCount = availableLeaves.Where(x => x.LeaveType == model.LeaveType).Select(x => x.AvailableLeaves).FirstOrDefault();
            if (model.DaysCountExcludingHolidays > availableLeavesCount)
            {
                model.IsEditAllowed = false;
                return PartialView("_CustomError", "Unable to Edit !!! The  Leave Duration Count is Greater than the Available Leave Count.");

            }
            else
            {
                return PartialView("_EditLeave", model);
            }


        }
        [AjaxOnly]
        [HttpPost]
        [CheckRightsAuthorize(Feature = FeatureEnum.LeaveFeature)]
        public ActionResult EditLeave(DashboardLeaveModel model)
        {

            if (!ModelState.IsValid)
            {
                PutLeaveStatusInViewBag();
                return PartialView("_EditLeave", model);
            }

            DomainEntities.DashboardLeave dl = Business.GetSingleLeave(model.Id);

            dl.LeaveStatus = model.LeaveStatus;
            dl.ApproveNotes = model.ApproveNotes;
            dl.UpdatedBy = model.UpdatedBy;
            dl.DateUpdated = model.DateUpdated;

            try
            {
                string currentUser = this.CurrentUserStaffCode;
                DBSaveStatus status = Business.SaveLeaveData(dl, currentUser);
                if (status == DBSaveStatus.CyclicCheckFail)
                {
                    ModelState.AddModelError("", "An error occurred while saving changes. Please refresh the page and try again.");
                    return PartialView("_EditLeave", model);
                }
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(EditLeave), ex.ToString(), ">");
                ModelState.AddModelError("", ex.Message);
                PutLeaveStatusInViewBag();
                return PartialView("_EditLeave", model);
            }
        }

        //Author:Gagana K, Purpose:For displaying Geo Tagging Report; Date:29-12-2022

        private ActionResult GetGeoTaggingReport(DomainEntities.SearchCriteria sc)
        {

            IEnumerable<DomainEntities.GeoTaggingReport> dnm = Business.GetGeoTagging(sc);
            IEnumerable<GeoTagReportDataModel> GTR = dnm.Select(d => new GeoTagReportDataModel()
            {

                EmployeeCode = d.EmployeeCode,
                EmployeeName = d.EmployeeName,
                CustomerName = d.CustomerName,
                BranchName = d.BranchName,
                DivisionName = d.DivisionName,
                GeoTagStatus = d.GeoTagStatus,
                CustomerType = d.CustomerType,
                CustomerCode = d.CustomerCode,
                ZoneName= d.ZoneName,
            }).ToList();
            return PartialView("_GeoTaggingReport", GTR);
        }
        //Added for Dealers Summary Report
        private ActionResult GetDealersSummaryReport(DomainEntities.SearchCriteria sc)
        {
            IEnumerable<DomainEntities.DealersSummaryReportData> ds = Business.GetDealersSummaryReports(sc);
            IEnumerable<DealersSummaryReportDataModel> entDs = ds.Select(d => new DealersSummaryReportDataModel()
            {
                EmployeeCode = d.StaffCode,
                EmployeeName = d.EmployeeName,
                BranchName = d.BranchName,
                DivisionName = d.DivisionName,
                GeoTagCompleted = d.GeoTagCompleted,
                GeoTagsPending = d.GeoTagsPending,
                TotalDealersCount = d.TotalDealersCount,
                EmployeeStatus = d.EmployeeStatus,
            }).ToList();
            return PartialView("_DealersSummaryReport", entDs);

        }  
        //Added for Agreements Report Swetha M on 20230114
        private ActionResult GetAgreementsReportData(DomainEntities.SearchCriteria sc)
        {
            IEnumerable<DomainEntities.AgreementReportData> agreements = Business.GetAgreementsReportData(sc);
            IEnumerable<AgreementReportDataModel> agreementReport = agreements.Select(ea => new AgreementReportDataModel()
            {
                AgreementNumber = ea.AgreementNumber,
                Status = ea.Status,
                ZoneCode = ea.ZoneCode,
                ZoneName = ea.ZoneName,
                AreaCode = ea.AreaCode,
                AreaName = ea.AreaName,
                TerritoryCode = ea.TerritoryCode,
                TerritoryName = ea.TerritoryName,
                HQCode = ea.HQCode,
                HQName = ea.HQName,
                RatePerKg = ea.RatePerKg,
                LandSize = ea.LandSize,
                WorkflowSeasonName = ea.WorkflowSeasonName,
                EmployeeCode = ea.EmployeeCode,
                EmployeeName = ea.EmployeeName,
                AtBusiness = ea.AtBusiness==true ? "Yes" : "No",
                EntityName = ea.EntityName,
                EntityDate = ea.EntityDate,
                UniqueIdType = ea.UniqueIdType,
                UniqueId = ea.UniqueId,
                FatherHusbandName = ea.FatherHusbandName,
                IsActive = ea.IsActive== true ? "Active" : "InActive",
                BankDetailCount = ea.BankDetailCount,
                ContactCount = ea.ContactCount,
                ImageCount = ea.ImageCount,
                Latitude = ea.Latitude,
                Longitude = ea.Longitude,
                CropName = ea.CropName
            }).ToList();
            return PartialView("_AgreementsReport", agreementReport);

        }
        //Author:Swetha M, Purpose:Expense Bulk Approval; Date:30-01-2023
        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.ExpenseFeature)]
        public ActionResult BulkApproveExpenses(IEnumerable<long> expenseId)
        {
           
            //Get the Existing all the expenses           
            ICollection<BulkExpense> selectedExpenses = Business.GetExpensesById(expenseId);
            //Filter out the selected expenses           
            if (selectedExpenses.Count!=0)
            {
                //Get the count of Selected employees and Total amount
                BulkApproveExpensesModel bulkExpensesModel = new BulkApproveExpensesModel
                {
                    SelectedEmployees = selectedExpenses.Select(x => x.EmployeeId).Distinct().Count(),
                    TotalAmount = selectedExpenses.Sum(x => x.TotalAmount),
                    ExpenseId = selectedExpenses.Select(x=>x.Id).ToList()
                };

                return PartialView("_ExpenseBulkApprove", bulkExpensesModel);
            }
            else
            {
                return PartialView("_CustomError", "The Expenses are already Approved .");
            }
        }

        //Author:Swetha M, Purpose:Expense Bulk Approval; Date:30-01-2023

        [AjaxOnly]
        [HttpPost]
        [CheckRightsAuthorize(Feature = FeatureEnum.ExpenseFeature)]
        public ActionResult BulkApproveExpenses(BulkApproveExpensesModel expenseModel)
        {
           
            //Filter out the selected expenses
            ICollection<BulkExpense> selectedExpenses = Business.GetExpensesById(expenseModel.ExpenseId);
            int result = 0;
            if (selectedExpenses == null)
            {
                result = 0;
            }
            else
            {
                List<DomainEntities.ApprovalData> approvalData = new List<DomainEntities.ApprovalData>();
                foreach(var item in selectedExpenses)
                {
                    DomainEntities.ApprovalData data = new DomainEntities.ApprovalData()
                    {
                        Id = item.Id,
                        ApprovedAmt = item.TotalAmount,
                        ApproveComments =Constant.BulkApproved,
                        ApprovedDate = DateTime.UtcNow,
                        ApprovedBy = this.CurrentUserStaffCode
                    };
                    approvalData.Add(data);
                }
                result = Business.SaveBulkExpenseApprove(approvalData, IsSuperAdmin, CurrentUserStaffCode);

            }
            switch (result)
            {
                case 0:
                    ModelState.AddModelError("", "An error occurred while saving changes. Please refresh the page and try again.");
                    return PartialView("_ExpenseBulkApprove", expenseModel);
                case 1:
                    return PartialView("ConfirmMessage");
                case 2:
                    ModelState.AddModelError("", "Invalid Approval Level. Please refresh the page and try again.");
                    return PartialView("_ExpenseBulkApprove", expenseModel);
            }
            return View("_ExpenseBulkApprove", expenseModel);
        }
        //Added for DuplicateFarmersReport by GAgana on 20230130
        private ActionResult GetDuplicateFarmersReportData(DomainEntities.SearchCriteria sc)
        {
            IEnumerable<DomainEntities.DuplicateFarmersReport> duplicatefarmers = Business.GetDuplicateFarmersReportData(sc);
            IEnumerable<DuplicateFarmersReportDataModel> duplicatefarmerReport = duplicatefarmers.Select(ea => new DuplicateFarmersReportDataModel()
            {

                EmployeeCode = ea.EmployeeCode,
                EmployeeName = ea.EmployeeName,
                EntityName = ea.EntityName,
                UniqueId = ea.UniqueId,
                FatherHusbandName = ea.FatherHusbandName,
                IsActive = ea.IsActive,
                AgreementCount = ea.AgreementCount,
                HQName = ea.HQName,
                TerritoryName = ea.TerritoryName,
                EntityNumber = ea.EntityNumber
            }).ToList();
            return PartialView("_DuplicateFarmersReport", duplicatefarmerReport);

        }

        //Author:Gowtham S, Purpose:Add Issue/Reteruns; Date:15-02-2023
        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.IssueReturnFeature)]
        public ActionResult AddIssueReturns()
         {
           
            ViewBag.ActivityTypes = Business.GetCodeTable("IssueReturnActivityType");
           
            IssueReturnModel model = new IssueReturnModel();
            return PartialView("_AddIssueReturn", model);           
        }

        //Author:Gowtham S, Purpose:Add Issue/Reteruns; Date:15-02-2023
        [AjaxOnly]
        [HttpGet]
        public ActionResult GetSearchSalesPersonsDetails(StaffFilter searchCriteria)
        {
            DomainEntities.StaffFilter criteria = Helper.ParseStaffSearchCriteria(searchCriteria);
            criteria.ApplyAssociationFilter = false;
            criteria.ApplyStatusFilter = false;

            string hqCode = searchCriteria.HQ;
            IEnumerable<SalesPersonModel> salesPersonData = Business.GetSalesPersonData(criteria);

            var model = salesPersonData.Where(x => x.EmployeeId != 0 && x.IsActive != false).Select(spd => new SalesPersonViewModel()
            {
                Name = spd.Name + "(" + spd.StaffCode + ")",
                EmployeeId = spd.EmployeeId
            }).ToList();

          ICollection<Entity> associatedentityName = Business.GetAssociatedEntityName(hqCode);


            return Json( new { model, associatedentityName }, JsonRequestBehavior.AllowGet);
        }

        //Author:Gowtham S, Purpose:Add Issue/Reteruns; Date:15-02-2023
        [AjaxOnly]
        [HttpGet]
        public JsonResult GetEntityAgreements(long entityId)
        {
            ICollection<EntityAgreementForIR> associatedentityAgreement = Business.GetEntityAgreementsForIR(entityId);
            return Json(associatedentityAgreement, JsonRequestBehavior.AllowGet);
        }

        //Author:Gowtham S, Purpose:Add Issue/Reteruns; Date:15-02-2023
        [AjaxOnly]
        [HttpGet]
        public JsonResult GetItemType(long aggId)
        {

            ICollection<IssueReturn> associatedentityAgreement = Business.GetTypeName(aggId);

            Session["setvalues1"] = associatedentityAgreement;
            ICollection<EntityAgreement> getValues = (ICollection<EntityAgreement>)Session["setvalues"];

            var itemType = associatedentityAgreement.Select(x => x.TypeName).Distinct().ToList();
        
            return Json(itemType, JsonRequestBehavior.AllowGet);
        }

        //Author:Gowtham S, Purpose:Add Issue/Reteruns; Date:15-02-2023
        [AjaxOnly]
        [HttpGet]
        public JsonResult GetItemSelected(string wfsid)
        {

            ICollection<IssueReturn> getValues1 = (ICollection<IssueReturn>)Session["setvalues1"];

            var itemType = getValues1.Where(x => x.TypeName == wfsid).ToList();
            return Json(itemType, JsonRequestBehavior.AllowGet);
        }

        //Author:Gowtham S, Purpose:Add Issue/Reteruns; Date:15-02-2023
        [AjaxOnly]
        [HttpGet]
        public JsonResult GetItemList(string item)
        {

            ICollection<IssueReturn> getValues1 = (ICollection<IssueReturn>)Session["setvalues1"];

            var itemType = getValues1.Where(x => x.ItemType == item).ToList();

            return Json(itemType, JsonRequestBehavior.AllowGet);
        }

        //Author:Gowtham S, Purpose:Add Issue/Reteruns; Date:17-02-2023
        [AjaxOnly]
        [HttpPost]
        [CheckRightsAuthorize(Feature = FeatureEnum.IssueReturnFeature)]
        
        public ActionResult AddIssueReturns(List<IssueReturn> itemsData, IssueReturn masterData)
        {
            try
            {
              IssueReturn sp = new IssueReturn();
            
            string currentUser = this.CurrentUserStaffCode;
            foreach (var item in itemsData)
            {
                sp.AppliedItemMasterId = item.ItemMasterId;
                sp.ItemMasterId = item.ItemMasterId;
                sp.Quantity = item.Quantity;
                sp.AppliedQuantity = item.Quantity;
                sp.TransactionType = masterData.TransactionType;
                sp.Comments = item.Comments;
                sp.ItemRate = item.Rate;
                sp.AppliedItemRate = item.Rate;
                sp.TransactionDate = masterData.TransactionDate;
                sp.EntityAgreementId = masterData.EntityAgreementId;
                sp.EntityId = masterData.EntityId;
                sp.EmployeeId = masterData.EmployeeId;
                sp.AppliedTransactionType = masterData.TransactionType;
                sp.SlipNumber = masterData.SlipNumber;

                Business.SaveAddIssueReturnsDetails(sp, currentUser);

            }
          
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                return PartialView("_CustomError", ex.Message);
            }

        }

        //Added for FarmersBankAccountReport by GAgana on 20230130
        private ActionResult GetFarmersBankAccountReportData(DomainEntities.SearchCriteria sc)
        {
            IEnumerable<DomainEntities.FarmersBankAccountReport> farmersBankDetails = Business.GetFarmersBankAccountReportData(sc);
            IEnumerable<FarmersBankAccountReportDataModel> farmersBankDetailsreport = farmersBankDetails.Select(ea => new FarmersBankAccountReportDataModel()
            {
                EmployeeCode = ea.EmployeeCode,
                EmployeeName = ea.EmployeeName,
                EntityName = ea.EntityName,
                HQName = ea.HQName,
                TerritoryName = ea.TerritoryName,
                EntityNumber = ea.EntityNumber,
                IsSelfAccount = ea.IsSelfAccount ? "Yes" : "No",
                AccountHolderName = ea.AccountHolderName,
                AccountHolderPAN = ea.AccountHolderPAN,
                BankName = ea.BankName,
                BankAccount = ea.BankAccount,
                BankIFSC = ea.BankIFSC,
                BankBranch = ea.BankBranch,
                Status = ea.Status,
            }).ToList();
            return PartialView("_FarmersBankAccountReport", farmersBankDetailsreport);

        }


    }

}