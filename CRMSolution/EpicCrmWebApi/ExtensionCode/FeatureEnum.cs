using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public enum FeatureEnum
    {
        None = 0,
        ActivityFeature = 1,
        OrderFeature = 2,
        PaymentFeature = 3,
        OrderReturnFeature = 4,
        ExpenseFeature = 5,

        ExpenseReportFeature = 6,
        AttendanceReportFeature = 7,
        
        KPIFeature = 8,

        SalesPersonFeature = 9,
        CustomerFeature = 10,
        ProductFeature = 11,
        CrmUserFeature = 12,
        AssignmentFeature = 13,
        UploadDataFeature = 14,
        OfficeHierarchyFeature=15,
        BankAccountFeature = 16,
        EntityFeature = 17,
        GstRateFeature = 18,
      
        WorkFlowReportFeature = 19,   //WorkFlow -> FieldActivityReport
        IssueReturnFeature = 20,

        EntityProgressReportFeature = 21,
        AbsenteeReportFeature=22,
        AppSignUpReportFeature=23,
        AppSignInReportFeature=24,
        ActivityReportFeature=25,
        ActivityByTypeReportFeature=26,
        MAPFeature=27,
        AttendanceSummaryReportFeature = 28,

        EmployeeExpenseReportFeature = 29,
        DistanceReportFeature = 30,

        LeaveFeature = 31,
        WorkflowFeature = 32,
        RedFarmerModule = 33,
        DistantActivityReportFeature = 34,
        AdvanceRequest = 35,
        EmployeeExpenseReport2Feature = 36,  // heranba
        UnSownReportFeature = 37, // Reitzel
        AttendanceRegisterReportFeature = 38,

        // May 12 2020
        STRFeature = 39,
        STRWeighControl = 40,
        //STRSiloControl = 41,

        DWSPaymentReport = 42,
        TransporterPaymentReport = 43,

        // May 26 2020
        DWSApproveWeightOption = 44,
        DWSApproveAmountOption = 45,
        DWSPaymentOption = 46,

        // June 10 2020
        StockReceive = 47,
        StockReceiveConfirm = 48,
        StockRequest = 49,
        StockRequestFulfill = 50,
        ExtraAdminOption = 51,

        StockLedger = 52,
        StockBalance = 53,
        StockRemove = 54,
        StockRemoveConfirm = 55,

        StockAdd = 56,
        StockAddConfirm = 57,

        // April 20 2021
        //BonusCalculationOption = 58,
        BonusCalculationPaymentOption = 58,
        
        // Author:PankajKumar; Purpose: Day Planning; Date: 26/04/2021
        DayPlanningReport = 59,

        //BonusPaymentReport = 60,
        SurveyFormReport = 61,
        // Author:Ajith; Purpose: Dealer Questionnaire; 
        QuestionnaireFeature = 62,
        //Author - SA; Date: 20/05/2021; Purpose: VendorPayment feature
        //AddOrApproveVendorPayment = 62,
        //VendorPaymentOption = 63, 
        ProjectOption = 63,
        FollowUpTaskOption = 64,

        FarmerSummaryReport = 65,
        //Author - Venkatesh; Date: 04/11/2022
        //DealerNotMet
        DealerNotMetReport=66,
        DealersSummaryReport = 67,
        GeoTagReport =68,
        AgreementsReport = 69,
        DuplicateFarmersReport = 70, //Added by Gagana
        FarmersBankAccountReport = 71 //Added by Gagana
    }
}