using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SqliteDomainBatch
    {
        [Display(Name = "Batch Id")]
        public long Id { get; set; }
        public long EmployeeId { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        public string EmployeeCode { get; set; }

        [Display(Name = "Total Items")]
        public long BatchInputCount { get; set; }

        [Display(Name = "Saved")]
        public long BatchSavedCount { get; set; }

        [Display(Name = "Ignored")]
        public long DuplicateItemCount { get; set; }

        public long ExpenseLineInputCount { get; set; }
        public long ExpenseLineSavedCount { get; set; }
        public long ExpenseLineRejectCount { get; set; }
        public long ExpenseId { get; set; }

        public decimal TotalExpenseAmount { get; set; }
	    public DateTime? ExpenseDate { get; set; }

        [Display(Name = "Input Order Count")]
        public long NumberOfOrders { get; set; }

        [Display(Name = "Input Order Items")]
        public long NumberOfOrderLines { get; set; }

        [Display(Name = "Total Order Amount")]
        public decimal TotalOrderAmount { get; set; }

        [Display(Name = "Saved Order Count")]
        public long NumberOfOrdersSaved { get; set; }

        [Display(Name = "Saved Order Items")]
        public long NumberOfOrderLinesSaved { get; set; }


        [Display(Name = "Input Return Count")]
        public long NumberOfReturns { get; set; }

        [Display(Name = "Input Return Items")]
        public long NumberOfReturnLines { get; set; }

        [Display(Name = "Total Return Amount")]
        public decimal TotalReturnAmount { get; set; }

        [Display(Name = "Saved Return Count")]
        public long NumberOfReturnsSaved { get; set; }

        [Display(Name = "Saved Return Items")]
        public long NumberOfReturnLinesSaved { get; set; }

        public long NumberOfPayments { get; set; }
        public decimal TotalPaymentAmount { get; set; }
        public long NumberOfPaymentsSaved { get; set; }

        [Display(Name = "Input Entity Count")]
        public long NumberOfEntities { get; set; }

        [Display(Name = "Saved Entity Count")]
        public long NumberOfEntitiesSaved { get; set; }

        public long NumberOfLeaves { get; set; }

        public long NumberOfLeavesSaved { get; set; }

        public long NumberOfCancelledLeaves { get; set; }
        public long NumberOfCancelledLeavesSaved { get; set; }

        public long NumberOfIssueReturns { get; set; }
        public long NumberOfIssueReturnsSaved { get; set; }

        public long NumberOfWorkFlow { get; set; }
	    public long NumberOfWorkFlowSaved { get; set; }

        [Display(Name = "Sent On (UTC)")]
        public System.DateTime At { get; set; }

        [Display(Name = "Sent On (IST)")]
        public System.DateTime AtIST { get; set; }

        [Display(Name = "Saved At (UTC)")]
        public System.DateTime Timestamp { get; set; }
        [Display(Name = "Saved At (IST)")]
        public System.DateTime TimestampIST { get; set; }

        [Display(Name = "Processed?")]
        public bool BatchProcessed { get; set; }

        [Display(Name = "Locked at (UTC)")]
        public DateTime? LockTimestamp { get; set; }
        [Display(Name = "Locked at (IST)")]
        public DateTime? LockTimestampIST { get; set; }

        public long DuplicateExpenseCount { get; set; }
        public long DuplicateOrderCount { get; set; }
        public long DuplicateReturnCount { get; set; }
        public long DuplicatePaymentCount { get; set; }
        public long DuplicateEntityCount { get; set; }

        public long DeviceLogCount { get; set; }

        public string BatchGuid { get; set; }
        public long NumberOfImages { get; set; }
        public long NumberOfImagesSaved { get; set; }

        public string DataFileName { get; set; }
        public long DataFileSize { get; set; }

        public bool IsDataFileExist => String.IsNullOrEmpty(DataFileName) == false && File.Exists(DataFileName);


        public long Agreements { get; set; }
        public long AgreementsSaved { get; set; }

        public long Surveys { get; set; }
        public long SurveysSaved { get; set; }

        public long AdvanceRequests { get; set; }
        public long AdvanceRequestsSaved { get; set; }
        public long TerminateRequests { get; set; }
        public long TerminateRequestsSaved { get; set; }

        public long BankDetails { get; set; }
        public long BankDetailsSaved { get; set; }

        // May 10 2020
        public long STRCount { get; set; }
        public long STRSavedCount { get; set; }

        // June 03 2021
        public long DayPlanTarget { get; set; }
        public long DayPlanTargetSaved { get; set; }

        // Author: Rajesh; Date: 13-07-2021; Purpose: Display Questionnaire Details
        public long QuestionnaireTarget { get; set; }
        public long QuestionnaireTargetSaved { get; set; }

        // Author: Kartik; Date: 09-10-2021; Purpose: FollowupDetails
        public long Task { get; set; }
        public long TaskSaved { get; set; }

        public long TaskAction { get; set; }
        public long TaskActionSaved { get; set; }
    }
}
