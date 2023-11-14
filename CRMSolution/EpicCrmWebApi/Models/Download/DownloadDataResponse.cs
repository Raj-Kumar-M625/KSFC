using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class DownloadMiniDataResponse : MinimumResponse
    {
        public IEnumerable<CodeTable> ExpenseTypes { get; set; }
        public IEnumerable<TransportType> TransportTypes { get; set; }
        public IEnumerable<CodeTableEx> CustomerTypes { get; set; }
        public IEnumerable<CodeTable> ActivityTypes { get; set; }
        public IEnumerable<CodeTable> PaymentTypes { get; set; }
        public IEnumerable<CodeTable> LeaveTypes { get; set; }
        //public IEnumerable<CodeTable> LeaveReasons { get; set; }
        public IEnumerable<CodeTable> LeaveDurations { get; set; } //Added by Swetha -Mar 16
        public IEnumerable<CodeTable> CropTypes { get; set; }
        public IEnumerable<CodeTable> StateCodes { get; set; }
        public IEnumerable<CodeTable> TransactionTypes { get; set; } //issue, return
        public IEnumerable<CodeTable> ItemTypes { get; set; } // fertilizers, seeds...
        public IEnumerable<CodeTable> FuelTypes { get; set; }
        public IEnumerable<CodeTable> Divisions { get; set; }
        public IEnumerable<CodeTable> Segments { get; set; }
        public IEnumerable<CodeTable> ActiveCrops { get; set; }
        public IEnumerable<CodeTable> TerminateAgreementReasons { get; set; }
        public IEnumerable<CodeTableEx> Locales { get; set; }

        public IEnumerable<CodeTable> WaterSources { get; set; }
        public IEnumerable<CodeTable> SoilTypes { get; set; }
        public IEnumerable<CodeTable> MajorCrops { get; set; }
        public IEnumerable<CodeTable> LastCrops { get; set; }
        public IEnumerable<CodeTableEx> XamlEntityAddPages { get; set; }
        public IEnumerable<CodeTable> SowingTypes { get; set; }
        public IEnumerable<CodeTable> IssueReturnActivityTypes { get; set; }
        public IEnumerable<CodeTableEx> WorkFlowFollowUpPhases { get; set; }

        public IEnumerable<CodeTable> Fertilizers { get; set; }
        public IEnumerable<CodeTable> Sprays { get; set; }
        public IEnumerable<CodeTable> Acres { get; set; }
        public IEnumerable<CodeTable> CustomerBanks { get; set; }
        public IEnumerable<CodeTableEx> NumberPrefixes { get; set; }
        public IEnumerable<CodeTableEx> QuestionnaireType { get; set; }
        public IEnumerable<CodeTableEx> ProjectStatus { get; set; }
        public IEnumerable<CodeTableEx> ProjectCategory { get; set; }
        public IEnumerable<CodeTableEx> TaskStatus { get; set; }
        public IEnumerable<CodeTableEx> Notification { get; set; }

    }

    public class DownloadDataResponse : DownloadMiniDataResponse
    {
        public IEnumerable<DownloadCustomerModel> Customers { get; set; }
        public IEnumerable<DownloadProduct> Products { get; set; }
        public DownloadConsolidatedCustomerModel ConsolidatedCustomerInfo { get; set; }
        public IEnumerable<DownloadBankAccountModel> BankAccounts { get; set; }
        public IEnumerable<DownloadLeaveModel> Leaves { get; set; }
        public IEnumerable<DownloadServerEntity> ServerBusinessEntities { get; set; }

        public IEnumerable<DownloadWorkFlowSchedule> WorkFlowSchedule { get; set; }
        public IEnumerable<DownloadWorkFlowFollowUp> WorkFlowFollowUp { get; set; }
        public IEnumerable<DownloadEntityWorkFlow> EntitiesWorkFlow { get; set; }
        public IEnumerable<DownloadItemMaster> ItemMaster { get; set; }
        public IEnumerable<DownloadStaffDailyData> StaffDailyData { get; set; }
        public IEnumerable<string> StaffDivisionCodes { get; set; }
        
        // Oct 2 2019 - added for TStanes enhancements
        public IEnumerable<CustomerDivisionBalance> CustomerDivisionBalances { get; set; }

        public string MessageBarText { get; set; }

        public string AgreementStateForWorkFlow { get; set; } = "Approved";

        public IEnumerable<PPA> PPAData { get; set; }
        public IEnumerable<OfficeHierarchy> OfficeHierarchy { get; set; }

        // May 4 2020 - STR/DWS
        public IEnumerable<DownloadTransporter> TransporterData { get; set; }

        // May 21 2020 - control # of agreements per crop
        public IEnumerable<DownloadWorkflowSeason> WorkflowSeasons { get; set; }

        public IEnumerable<DownloadStockBalance> StockBalances { get; set; }

        public IEnumerable<EmployeeAchieved> EmployeeAchievedData { get; set; }
        public IEnumerable<EmployeeMonthlyTarget> EmployeeMonthlyTargetData { get; set; }
        public IEnumerable<EmployeeYearlyTarget> EmployeeYearlyTargetData { get; set; }
        public IEnumerable<DownloadQuestionnaire> QuestionnaireData { get; set; }
        public IEnumerable<DownloadServerProjects> ProjectData { get; set; }
        public IEnumerable<DownloadServerTasks> FollowUpTaskData { get; set; }
        public ICollection<LeaveTypes> LeaveTypeData { get; set; } //Added by Swetha -Mar 15
        public IEnumerable<DownloadHolidayList> HolidayListData { get; set; }//Added by Swetha -Mar 16
        public IEnumerable<DownloadAvailableLeaves> AvailableLeaveData { get; set; } //Added by Swetha -April 28

    }
}