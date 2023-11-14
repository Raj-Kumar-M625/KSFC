using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class FeatureControlModel
    {
        public long Id { get; set; }
        [Display(Name = "User")]
        public string UserName { get; set; }

        [Display(Name = "Activity")]
        public bool ActivityFeature { get; set; }

        [Display(Name = "Order")]
        public bool OrderFeature { get; set; }

        [Display(Name = "Payment")]
        public bool PaymentFeature { get; set; }

        [Display(Name = "Return")]
        public bool OrderReturnFeature { get; set; }

        [Display(Name = "Expense")]
        public bool ExpenseFeature { get; set; }

        [Display(Name = "Issue/Return")]
        public bool IssueReturnFeature { get; set; }

        //report fields
        [Display(Name = "Expense Rep")]
        public bool ExpenseReportFeature { get; set; }

        [Display(Name = "FieldActivity Rep")]
        public bool FieldActivityReportFeature { get; set; }

        [Display(Name = "ProfileProgress Rep")]
        public bool EntityProgressReportFeature { get; set; }

        [Display(Name = "Attendance Rep")]
        public bool AttendanceReportFeature { get; set; }

        [Display(Name = "Attendance Summary Rep")]
        public bool AttendanceSummaryReportFeature { get; set; }

        [Display(Name = "Attendance Register Rep")]
        public bool AttendanceRegister { get; set; }

        [Display(Name = "Absentee Rep")]
        public bool AbsenteeReportFeature { get; set; }

        [Display(Name = "AppSignUp Rep")]
        public bool AppSignUpReportFeature { get; set; }

        [Display(Name = "AppSignIn Rep")]
        public bool AppSignInReportFeature { get; set; }

        [Display(Name = "Activity Rep")]
        public bool ActivityReportFeature { get; set; }

        [Display(Name = "ActivityByType Rep")]
        public bool ActivityByTypeReportFeature { get; set; }

        [Display(Name = "KPI")]
        public bool KPIFeature { get; set; }

        [Display(Name = "MAP")]
        public bool MAPFeature { get; set; }
        //report fields end

        [Display(Name = "Sales Person")]
        public bool SalesPersonFeature { get; set; }

        [Display(Name = "Customer")]
        public bool CustomerFeature { get; set; }

        [Display(Name = "Product")]
        public bool ProductFeature { get; set; }

        [Display(Name = "CrmUser")]
        public bool CrmUserFeature { get; set; }

        [Display(Name = "Association")]
        public bool AssignmentFeature { get; set; }

        [Display(Name = "Upload")]
        public bool UploadDataFeature { get; set; }

        [Display(Name = "Org Chart")]
        public bool OfficeHierarchyFeature { get; set; }

        [Display(Name = "Super Admin Page")]
        public bool SuperAdminPage { get; set; }

        [Display(Name = "Date Created (IST)")]
        public System.DateTime DateCreated { get; set; }

        [Display(Name = "Date Updated (IST)")]
        public System.DateTime DateUpdated { get; set; }

        [Display(Name = "Bank Account")]
        public bool BankAccountFeature { get; set; }

        [Display(Name = "Profile")]
        public bool EntityFeature { get; set; }

        [Display(Name = "Gst Rate")]
        public bool GstRateFeature { get; set; }

        [Display(Name = "Activity Work Flow")]
        public bool WorkFlowReportFeature { get; set; }

        [Display(Name = "Expense Report By Employee")]
        public bool EmployeeExpenseReport { get; set; }

        [Display(Name = "Expense Report By Employee2 (Heranba)")]
        public bool EmployeeExpenseReport2 { get; set; }

        [Display(Name = "Distance Calc Report")]
        public bool DistanceReport { get; set; }

        [Display(Name = "UnSown Report (Reitzel)")]
        public bool UnSownReport { get; set; }

        [Display(Name = "Security Context User")]
        public string SecurityContextUser { get; set; }

        public bool IsDefaultSecurityContext { get; set; }

        [Display(Name = "Advance Request")]
        public bool AdvanceRequestModule { get; set; }

        [Display(Name = "Wrong Location Report")]
        public bool WrongLocationReport { get; set; }

        [Display(Name = "Red Farmer")]
        public bool RedFarmerModule { get; set; }

        [Display(Name = "Read Only User?")]
        public bool IsReadOnlyUser { get; set; }

        // May 12 2020
        [Display(Name = "STR")]
        public bool STRFeature { get; set; }

        [Display(Name = "DWS Payment Report")]
        public bool DWSPaymentReport { get; set; }

        [Display(Name = "Transporter Payment Report")]
        public bool TransporterPaymentReport { get; set; }

        [Display(Name = "STR Weigh Bridge")]
        public bool STRWeighControl { get; set; }

        [Display(Name = "STR Silo")]
        // not used
        public bool STRSiloControl { get; set; }

        [Display(Name = "Approve DWS Wt.")]
        public bool DWSApproveWeightOption { get; set; }

        [Display(Name = "Approve DWS Amount")]
        public bool DWSApproveAmountOption { get; set; }

        [Display(Name = "DWS Payment")]
        public bool DWSPaymentOption { get; set; }

        // June 10 2020
        [Display(Name = "Receive Stock (Entry)")]
        public bool StockReceiveOption { get; set; }

        [Display(Name = "Receive Stock (Confirm)")]
        public bool StockReceiveConfirmOption { get; set; }

        [Display(Name = "Request Stock")]
        public bool StockRequestOption { get; set; }

        [Display(Name = "Fulfill Stock Request")]
        public bool StockRequestFulfillOption { get; set; }

        [Display(Name = "Extra Admin Rights")]
        public bool ExtraAdminOption { get; set; }

        [Display(Name = "Stock Ledger")]
        public bool StockLedgerOption { get; set; }

        [Display(Name = "Stock Balance")]
        public bool StockBalanceOption { get; set; }

        [Display(Name = "Stock Remove")]
        public bool StockRemoveOption { get; set; }

        [Display(Name = "Confirm Stock Remove")]
        public bool StockRemoveConfirmOption { get; set; }

        [Display(Name = "Stock Add")]
        public bool StockAddOption { get; set; }

        [Display(Name = "Confirm Stock Add")]
        public bool StockAddConfirmOption { get; set; }

        // April 20 2021
        //[Display(Name = "Bonus Calculation ")]
        //public bool BonusCalculationOption { get; set; }

        [Display(Name = "Bonus Calculation Payment")]
        public bool BonusCalculationPaymentOption { get; set; }

        //[Display(Name = "Bonus Payment Report")]
        //public bool BonusPaymentReport { get; set; }

        [Display(Name = "Survey Form Report")]
        public bool SurveyFormReport { get; set; }

        [Display(Name = "Day Plan")]
        public bool DayPlanReport { get; set; }

        // Author:Ajith; Purpose: Dealer Questionnaire ;  Date: 11/06/2021
        [Display(Name = "Questionnaire")]
        public bool QuestionnaireFeature { get; set; }

        // Author:Rajesh V; Purpose: Farmer Summary Report ;  Date: 08/10/2021

        [Display(Name = "Farmer Summary Report")]
        public bool FarmerSummaryReport { get; set; }
        //Author:Venkatesh, Purpose: Dealers Not Met Report on: 2022/23/09
        [Display(Name = "Dealers Not Met Report")]
        public bool DealersNotMetReport { get; set; }

        // Author:Kartik; Purpose: FollowUp Task;  Date: 14/09/2021
        [Display(Name = "Projects")]
        public bool ProjectOption { get; set; }

        [Display(Name = "Follow-Up / Task")]
        public bool FollowUpTaskOption { get; set; }
        // Author:Swetha; Purpose: Leave Module;  Date: 18/04/2022
        [Display(Name = "Leave Module")]
        public bool LeaveModuleOption { get; set; }

        // Author:Swetha; Purpose:Dealers Summary and Geo Tag Report;  Date: 02/01/2023

        [Display(Name = "Dealers Summary Report")]
        public bool DealersSummaryReport { get; set; }

        [Display(Name = "Geo Tagging Report")]
        public bool GeoTaggingReport { get; set; }

        [Display(Name = "Agreement  Report")]
        public bool AgreementsReport { get; set; }

        [Display(Name = "Duplicate Farmers Report")]
        public bool DuplicateFarmersReport { get; set; }

        [Display(Name = "Farmers Bank Account Report")]
        public bool FarmersBankAccountReport { get; set; }

    }
}