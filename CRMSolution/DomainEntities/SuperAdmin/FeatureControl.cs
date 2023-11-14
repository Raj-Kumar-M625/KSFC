using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class FeatureControl
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public bool ActivityFeature { get; set; }
        public bool OrderFeature { get; set; }
        public bool PaymentFeature { get; set; }
        public bool OrderReturnFeature { get; set; }
        public bool ExpenseFeature { get; set; }
        public bool IssueReturnFeature { get; set; }

        public bool ExpenseReportFeature { get; set; }
        public bool AttendanceReportFeature { get; set; }
        public bool AttendanceSummaryReportFeature { get; set; }
        public bool AttendanceRegister { get; set; }

        public bool KPIFeature { get; set; }
        public bool FieldActivityReportFeature { get; set; }
        public bool EntityProgressReportFeature { get; set; }
        public bool AbsenteeReportFeature { get; set; }
        public bool AppSignUpReportFeature { get; set; }
        public bool AppSignInReportFeature { get; set; }
        public bool ActivityReportFeature { get; set; }
        public bool ActivityByTypeReportFeature { get; set; }
        public bool MAPFeature { get; set; }
        public bool SalesPersonFeature { get; set; }
        public bool CustomerFeature { get; set; }
        public bool ProductFeature { get; set; }
        public bool CrmUserFeature { get; set; }
        public bool AssignmentFeature { get; set; }
        public bool UploadDataFeature { get; set; }
        public bool OfficeHierarchyFeature { get; set; }
        public bool SuperAdminPage { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateUpdated { get; set; }
        public bool BankAccountFeature { get; set; }
        public bool EntityFeature { get; set; }
        public bool GstRateFeature { get; set; }
        public bool WorkFlowReportFeature { get; set; } // not used

        public bool EmployeeExpenseReport { get; set; }
        public bool DistanceReport { get; set; }

        public bool WrongLocationReport { get; set; }
        public bool DistantActivityReport => WrongLocationReport;

        public bool RedFarmerModule { get; set; }

        public bool AdvanceRequestModule { get; set; }

        public bool EmployeeExpenseReport2 { get; set; }
        public bool UnSownReport { get; set; }
        public bool IsReadOnlyUser { get; set; }

        public string SecurityContextUser { get; set; }

        // May 12 2020
        public bool STRFeature { get; set; }
        public bool DWSPaymentReport { get; set; }
        public bool TransporterPaymentReport { get; set; }
        //public bool AddOrApproveTransporterPayment { get; set; }
        //public bool TransporterPaymentOption { get; set; }

        public bool STRWeighControl { get; set; }
        //public bool STRSiloControl { get; set; }

        // May 26 2020
        public bool DWSApproveWeightOption { get; set; }
        public bool DWSApproveAmountOption { get; set; }
        public bool DWSPaymentOption { get; set; }

        // June 10 2020
        public bool StockReceiveOption { get; set; }
        public bool StockReceiveConfirmOption { get; set; }
        public bool StockRequestOption { get; set; }
        public bool StockRequestFulfillOption { get; set; }
        public bool ExtraAdminOption { get; set; }

        // June 19 2020
        public bool StockLedgerOption { get; set; }
        public bool StockBalanceOption { get; set; }
        public bool StockRemoveOption { get; set; }
        public bool StockRemoveConfirmOption { get; set; }

        // July 01 2020
        public bool StockAddOption { get; set; }
        public bool StockAddConfirmOption { get; set; }

        // April 20 2021
        //public bool BonusCalculationOption { get; set; }
        public bool BonusCalculationPaymentOption { get; set; }
        //public bool BonusPaymentReport { get; set; }
        public bool SurveyFormReport { get; set; }

        // Author:PK; Purpose: Day Planning; Date: 26/04/2021
        public bool DayPlanReport { get; set; }

        public bool QuestionnaireFeature { get; set; }

        // Author:Rajesh V; Purpose: Farmer Summary Report; Date: 08/10/2021
        public bool FarmerSummaryReport { get; set; }

        // Author:Kartik; Purpose: Followup Tasks; Date: 14/09/2021
        public bool ProjectOption { get; set; }
        public bool FollowUpTaskOption { get; set; }
        public bool LeaveFeature { get; set; }
        public bool DealersNotMetReport { get; set; }
        public bool GeoTaggingReport { get; set; }
        public bool AgreementsReport { get; set; }
        public bool DealersSummaryReport { get; set; }
        public bool DuplicateFarmersReport { get; set; }
        public bool FarmersBankAccountReport { get; set; }
        public bool IsDefaultSecurityContext =>
              (String.IsNullOrEmpty(SecurityContextUser) || 
                "Default".Equals(SecurityContextUser, StringComparison.OrdinalIgnoreCase));
    }
}
