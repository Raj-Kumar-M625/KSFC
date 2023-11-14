using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SearchCriteria : BaseSearchCriteria
    {
        public string ReportType { get; set; }

        public bool ApplyDataStatusFilter { get; set; }
        public bool DataStatus { get; set; }  // used for Order, Payment, Return

        // used only for Expenses
        public bool ZMApprovedExpense { get; set; }  // Zone Manager
        public bool AMApprovedExpense { get; set; }  // Area Manager
        public bool TMApprovedExpense { get; set; }  // Territory Manager


        public bool ApplyAmountFilter { get; set; }
        public decimal AmountFrom { get; set; }
        public decimal AmountTo { get; set; }

        public bool ApplyDateFilter { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public bool ApplyPlannedDateFilter { get; set; }
        public DateTime PlannedDateFrom { get; set; }
        public DateTime PlannedDateTo { get; set; }

        public bool ApplyHarvestDateFilter { get; set; }
        public DateTime HarvestDate { get; set; }


        //For Activity Report
        public bool ApplyActivityTypeFilter { get; set; }
        public string ActivityType { get; set; }

        public bool ApplyClientTypeFilter { get; set; }
        public string ClientType { get; set; }

        public bool ApplyEmployeeCodeFilter { get; set; }
        public string EmployeeCode { get; set; }

        public bool ApplyEmployeeNameFilter { get; set; }
        public string EmployeeName { get; set; }

        //For Employee Status
        public bool ApplyEmployeeStatusFilter { get; set; }
        public bool EmployeeStatus { get; set; }

        // For AppSignIn
        public bool ApplyDepartmentFilter { get; set; }
        public string Department { get; set; }

        public bool ApplyDesignationFilter { get; set; }
        public string Designation { get; set; }

        public bool ApplyClientNameFilter { get; set; }
        public string ClientName { get; set; }

        public bool ApplyOrderIdFilter { get; set; }
        public long Id { get; set; }

        public bool ApplyWorkFlowFilter { get; set; }
        public string WorkFlow { get; set; }

        public bool ApplyWorkFlowStatusFilter { get; set; }
        public WorkFlowStatus WorkFlowStatus { get; set; }

        public bool ApplyEntityNameFilter { get; set; }
        public string EntityName { get; set; }

        public bool ApplyDistanceFilter { get; set; }
        public decimal Distance { get; set; }

        public bool ApplyIdFilter { get; set; } = false;
        public long FilterId { get; set; } = 0;

        // used from AdminController - while approving DWS Amount
        // to show all Issue/Returns on current Agreement
        public bool ApplyAgreementIdFilter { get; set; }
        public long AgreementId { get; set; }

        public bool ApplyAgreementNumberFilter { get; set; }
        public string AgreementNumber { get; set; }

        public bool ApplyEntityIdFilter { get; set; }
        public long EntityId { get; set; }
            
        public bool ApplySlipNumberFilter { get; set; }
        public string SlipNumber { get; set; }

        public bool ApplyRowStatusFilter { get; set; }
        public string RowStatus { get; set; }

        public bool ApplyCropFilter { get; set; }
        public string Crop { get; set; }

        public bool ApplyAgreementStatusFilter { get; set; }
        public string AgreementStatus { get; set; }

        // Author: Pankaj Kumar; Purpose: Search criteria for Target Status; Date: 30-04-2021
        public bool ApplyTargetStatusFilter { get; set; }
        public string TargetStatus { get; set; }

        // Author: Pankaj Kumar; Purpose: Search criteria for Day Plan Type; Date: 30-04-2021
        public bool ApplyDayPlanTypeFilter { get; set; }
        public string DayPlanType { get; set; }
        public bool ApplyQuestionnaireFilter { get; set; }
        public long QuestionPaperId { get; set; }

        // Author: Rajesh V; Purpose: Search criteria for Farmer Summary Report; Date: 07-10-2021
        public bool ApplyUniqueIdFilter { get; set; }
        public string UniqueId { get; set; }
        public bool ApplySeasonNameFilter { get; set; }
        public string SeasonName { get; set; }
        //Author: Venkatesh
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public bool GeoTagStatus { get; set; }
        public bool ApplyGeoTagStatusFilter { get; set; }

        public bool ApplyCustomerNameFilter { get; set; }
        public bool ApplyCustomerCodeFilter { get; set; }
        public bool ApplyBankDetailStatusFilter { get; set; }
        public string BankDetailStatus { get; set; } 
        public bool ApplyBusinessRoleFilter { get; set; }
        public string BusinessRole { get; set; }
        public bool ProfileStatus { get; set; }
        public bool ApplyProfileStatusFilter { get; set; }
    }
}
