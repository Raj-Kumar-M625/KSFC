using CRMUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class FeatureData
    {
        public long Id { get; set; }
        public string UserName { get; set; }

        private bool _activityFeature;
        public bool ActivityFeature
        {
            get
            {
                return _activityFeature;
            }
            set
            {
                _activityFeature = value && Utils.SiteConfigData.ActivityOption;
            }
        }

        private bool _orderFeature;
        public bool OrderFeature
        {
            get
            {
                return _orderFeature;
            }
            set
            {
                _orderFeature = value && Utils.SiteConfigData.CustomerOption && Utils.SiteConfigData.OrderButton;
            }
        }

        private bool _paymentFeature;
        public bool PaymentFeature
        {
            get
            {
                return _paymentFeature;
            }
            set
            {
                _paymentFeature = value && Utils.SiteConfigData.CustomerOption && Utils.SiteConfigData.PaymentButton;
            }
        }

        private bool _orderReturnFeature;
        public bool OrderReturnFeature
        {
            get
            {
                return _orderReturnFeature;
            }
            set
            {
                _orderReturnFeature = value && Utils.SiteConfigData.CustomerOption && Utils.SiteConfigData.ReturnButton;
            }
        }

        private bool _expenseFeature;
        public bool ExpenseFeature
        {
            get
            {
                return _expenseFeature;
            }
            set
            {
                _expenseFeature = value && Utils.SiteConfigData.ExpenseOption;
            }
        }

        private bool _issueReturnFeature;
        public bool IssueReturnFeature
        {
            get
            {
                return _issueReturnFeature;
            }
            set
            {
                _issueReturnFeature = value && Utils.SiteConfigData.IssueReturnModule;
            }
        }

        //report fields
        private bool _expenseReportFeature;
        public bool ExpenseReportFeature
        {
            get
            {
                return _expenseReportFeature;
            }
            set
            {
                _expenseReportFeature = value && Utils.SiteConfigData.ExpenseReport;
            }
        }

        private bool _fieldActivityReportFeature;
        public bool FieldActivityReportFeature
        {
            get
            {
                return _fieldActivityReportFeature;
            }
            set
            {
                _fieldActivityReportFeature = value && Utils.SiteConfigData.FieldActivityReport;
            }
        }

        private bool _entityProgressReportFeature;
        public bool EntityProgressReportFeature
        {
            get
            {
                return _entityProgressReportFeature;
            }
            set
            {
                _entityProgressReportFeature = value && Utils.SiteConfigData.ProfileProgressReport;
            }
        }

        private bool _attendanceReportFeature;
        public bool AttendanceReportFeature
        {
            get
            {
                return _attendanceReportFeature;
            }
            set
            {
                _attendanceReportFeature = value && Utils.SiteConfigData.AttendanceReport;
            }
        }

        private bool _attendanceSummaryReportFeature;
        public bool AttendanceSummaryReportFeature
        {
            get
            {
                return _attendanceSummaryReportFeature;
            }
            set
            {
                _attendanceSummaryReportFeature = value && Utils.SiteConfigData.AttendanceSummaryReport;
            }
        }

        private bool _attendanceRegister;
        public bool AttendanceRegisterReportFeature
        {
            get
            {
                return _attendanceRegister;
            }
            set
            {
                _attendanceRegister = value && Utils.SiteConfigData.AttendanceRegister;
            }
        }

        private bool _absenteeReportFeature;
        public bool AbsenteeReportFeature
        {
            get
            {
                return _absenteeReportFeature;
            }
            set
            {
                _absenteeReportFeature = value && Utils.SiteConfigData.AbsenteeReport;
            }
        }

        private bool _appSignUpReportFeature;
        public bool AppSignUpReportFeature
        {
            get
            {
                return _appSignUpReportFeature;
            }
            set
            {
                _appSignUpReportFeature = value && Utils.SiteConfigData.AppSignUpReport;
            }
        }

        private bool _appSignInReportFeature;
        public bool AppSignInReportFeature
        {
            get
            {
                return _appSignInReportFeature;
            }
            set
            {
                _appSignInReportFeature = value && Utils.SiteConfigData.AppSignInReport;
            }
        }
        
        private bool _activityReportFeature;
        public bool ActivityReportFeature
        {
            get
            {
                return _activityReportFeature;
            }
            set
            {
                _activityReportFeature = value && Utils.SiteConfigData.ActivityReport;
            }
        }

        private bool _activityByTypeReportFeature;
        public bool ActivityByTypeReportFeature
        {
            get
            {
                return _activityByTypeReportFeature;
            }
            set
            {
                _activityByTypeReportFeature = value && Utils.SiteConfigData.ActivityByTypeReport;
            }
        }

        private bool _kpiFeature;
        public bool KPIFeature
        {
            get
            {
                return _kpiFeature;
            }
            set
            {
                _kpiFeature = value && Utils.SiteConfigData.KPIReport;
            }
        }

        private bool _mapFeature;
        public bool MAPFeature
        {
            get
            {
                return _mapFeature;
            }
            set
            {
                _mapFeature = value && Utils.SiteConfigData.MAPReport;
            }
        }

        private bool _employeeExpenseReport;
        public bool EmployeeExpenseReport
        {
            get
            {
                return _employeeExpenseReport;
            }
            set
            {
                _employeeExpenseReport = value && Utils.SiteConfigData.EmployeeExpenseReport;
            }
        }

        private bool _employeeExpenseReport2;
        public bool EmployeeExpenseReport2
        {
            get
            {
                return _employeeExpenseReport2;
            }
            set
            {
                _employeeExpenseReport2 = value && Utils.SiteConfigData.EmployeeExpenseReport2;
            }
        }

        private bool _unSownReport;
        public bool UnSownReport
        {
            get
            {
                return _unSownReport;
            }
            set
            {
                _unSownReport = value && Utils.SiteConfigData.UnSownReport;
            }
        }

        // Author:Pankaj Kumar; Purpose: Day Planning; Date: 26/04/2021
        //private bool _dayPlanningReport;
        //public bool DayPlanningReport
        //{
        //    get
        //    {
        //        return _dayPlanningReport;
        //    }
        //    set
        //    {
        //        _dayPlanningReport = value && Utils.SiteConfigData.DayPlanningReport;
        //    }
        //}

        private bool _distanceReport;
        public bool DistanceReport
        {
            get
            {
                return _distanceReport;
            }
            set
            {
                _distanceReport = value && Utils.SiteConfigData.DistanceReport;
            }
        }

        private bool _distantActivityReport;
        public bool DistantActivityReport
        {
            get
            {
                return _distantActivityReport;
            }
            set
            {
                _distantActivityReport = value && Utils.SiteConfigData.DistantActivityReport;
            }
        }

        //report fields end

        public bool SalesPersonFeature { get; set; }

        private bool _customerFeature;
        public bool CustomerFeature
        {
            get
            {
                return _customerFeature;
            }
            set
            {
                _customerFeature = value && Utils.SiteConfigData.CustomerOption;
            }
        }

        private bool _productFeature;
        public bool ProductFeature
        {
            get
            {
                return _productFeature;
            }
            set
            {
                _productFeature = value && Utils.SiteConfigData.CustomerOption;
            }
        }

        public bool CrmUserFeature { get; set; }

        public bool AssignmentFeature { get; set; }

        public bool UploadDataFeature { get; set; }

        public bool OfficeHierarchyFeature { get; set; }

        private bool _redFarmerModule;
        public bool RedFarmerModule
        {
            get
            {
                return _redFarmerModule;
            }
            set
            {
                _redFarmerModule = value && Utils.SiteConfigData.RedFarmerModule;
            }
        }

        private bool _advanceRequestModule;
        public bool AdvanceRequestModule
        {
            get
            {
                return _advanceRequestModule;
            }
            set
            {
                _advanceRequestModule = value && Utils.SiteConfigData.AdvanceRequestModule;
            }
        }

        public bool SuperAdminPage { get; set; }

        public System.DateTime DateCreated { get; set; }

        public System.DateTime DateUpdated { get; set; }

        private bool _bankAccountFeature;
        public bool BankAccountFeature
        {
            get
            {
                return _bankAccountFeature;
            }
            set
            {
                _bankAccountFeature = value && Utils.SiteConfigData.BankOption;
            }
        }

        private bool _entityFeature;
        public bool EntityFeature
        {
            get
            {
                return _entityFeature;
            }
            set
            {
                _entityFeature = value && Utils.SiteConfigData.IsEntityButtonVisible;
            }
        }

        private bool _gstRateFeature;
        public bool GstRateFeature
        {
            get
            {
                return _gstRateFeature;
            }
            set
            {
                _gstRateFeature = value && Utils.SiteConfigData.CustomerOption;
            }
        }

        private bool _workflowFeature;
        public bool WorkFlowFeature
        {
            get
            {
                return _workflowFeature;
            }
            set
            {
                _workflowFeature = value && Utils.SiteConfigData.WorkflowActivityOption;
            }
        }

        public bool IsReadOnlyUser { get; set; }

        private bool _strFeature;
        public bool STRFeature
        {
            get
            {
                return _strFeature;
            }
            set
            {
                _strFeature = value && Utils.SiteConfigData.STROption;
            }
        }

        private bool _strWeighFeature;
        public bool STRWeighFeature
        {
            get
            {
                return _strWeighFeature;
            }
            set
            {
                _strWeighFeature = value && Utils.SiteConfigData.STROption;
            }
        }

        //private bool _strSiloFeature;
        //public bool STRSiloFeature
        //{
        //    get
        //    {
        //        return _strSiloFeature;
        //    }
        //    set
        //    {
        //        _strSiloFeature = value && Utils.SiteConfigData.STROption;
        //    }
        //}

        private bool _dwsDWSApproveWeightOption;
        public bool DWSApproveWeightOption
        {
            get
            {
                return _dwsDWSApproveWeightOption;
            }
            set
            {
                _dwsDWSApproveWeightOption = value && Utils.SiteConfigData.STROption;
            }
        }

        private bool _dwsApproveAmountOption;
        public bool DWSApproveAmountOption
        {
            get
            {
                return _dwsApproveAmountOption;
            }
            set
            {
                _dwsApproveAmountOption = value && Utils.SiteConfigData.STROption;
            }
        }

        private bool _dwsPaymentOption;
        public bool DWSPaymentOption
        {
            get
            {
                return _dwsPaymentOption;
            }
            set
            {
                _dwsPaymentOption = value && Utils.SiteConfigData.STROption;
            }
        }

        private bool _dwsPaymentReportFeature;
        public bool DWSPaymentReportFeature
        {
            get
            {
                return _dwsPaymentReportFeature;
            }
            set
            {
                _dwsPaymentReportFeature = value && Utils.SiteConfigData.STROption && Utils.SiteConfigData.DWSPaymentReport;
            }
        }

        private bool _transporterPaymentReportFeature;
        public bool TransporterPaymentReportFeature
        {
            get
            {
                return _transporterPaymentReportFeature;
            }
            set
            {
                _transporterPaymentReportFeature = value && Utils.SiteConfigData.STROption && Utils.SiteConfigData.TransporterPaymentReport;
            }
        }

        // - June 10 2020
        public bool StockReceiveOption { get; set; }
        public bool StockReceiveConfirmOption { get; set; }
        public bool StockRequestOption { get; set; }
        public bool StockRequestFulfillOption { get; set; }
        public bool ExtraAdminOption { get; set; }

        public bool StockLedgerOption { get; set; }
        public bool StockBalanceOption { get; set; }
        public bool StockRemoveOption { get; set; }
        public bool StockRemoveConfirmOption { get; set; }

        public bool StockAddOption { get; set; }
        public bool StockAddConfirmOption { get; set; }

        // April 20 2021
        //public bool BonusCalculationOption { get; set; }
        public bool BonusCalculationPaymentOption { get; set; }
        //public bool BonusPaymentReport { get; set; }
        public bool SurveyFormReport { get; set; }
        public bool DayPlanReport { get; set; }

        // Author - SA; Date:20/05/2021; Purpose: VendorPayment feature
        //public bool AddOrApproveTransporterPayment { get; set; }
        //public bool TransporterPaymentOption { get; set; }

        // Author:Ajith; Purpose: Dealer Questionnaire ; Date: 11/06/2021
        
        public bool QuestionnaireFeature { get; set; }

        //private bool _QuestionnaireFeature;
        //public bool QuestionnaireFeature
        //{
        //    get
        //    {
        //        return _QuestionnaireFeature;
        //    }
        //    set
        //    {
        //        _QuestionnaireFeature = value && Utils.SiteConfigData.QuestionnaireOption;
        //    }
        //}

        //Author:Rajesh V; Purpose: Farmer Summary Report; Date: 07/10/2021
        public bool FarmerSummaryReport { get; set; }

        // Author:Kartik; Purpose: FollowUp Task Feature ; Date: 14/09/2021
        public bool ProjectOption { get; set; }
        public bool FollowUpTaskOption { get; set; }
        private bool _leaveModule;
        public bool LeaveFeature
       {
           get
           {
               return _leaveModule;
           }
            set
            {
                _leaveModule = value && Utils.SiteConfigData.ApplyLeaveOption;            }
       }
        //Author: Venkatesh; Purpose: DealerNotMetReport;Date: 04/11/2022

        public bool DealersNotMetReport { get; set; }
        public bool DealersSummaryReport { get; set; }

        public bool GeoTagReport { get; set; }

        //Author: Gowtham; Purpose: Agreements Report ;Date: 15/01/2023
        public bool AgreementsReport { get; set; }

        //Author:Gagana; Purpose: Duplicate Farmers Report ;Date: 30/01/2023
        public bool DuplicateFarmersReport { get; set; }

        //Author:Gagana; Purpose: Duplicate Farmers Report ;Date: 30/01/2023
        public bool FarmersBankAccountReport { get; set; }
    }
}