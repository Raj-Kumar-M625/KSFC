using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicCrmWebApi
{
   public  class DashboardLeaveModel 
    {
       
        [Display (Name ="Id")]
        public long Id { get; set; }

        [Display(Name = "Leave Types")]
        public string LeaveType { get; set; }

        public int DaysCountExcludingHolidays{ get; set; }

        [Display(Name = "Leave Duration")]
        public int LeaveDuration { get; set; }

        [Display(Name ="Start Date")]
        public string StartDate { get; set; }

        [Display(Name = "End Date")]
        public string EndDate { get; set; }

        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Display (Name = "Leave Reason")]
        public string LeaveReason { get; set; }

        [Display(Name ="Comment")]
        public string Comment { get; set; }

        [Display (Name ="Leave Status")]
        public string LeaveStatus { get; set; }
        [Display(Name = "Approve Notes")]
        public string ApproveNotes { get; set; }
        public string  Notes { get; set; }

        [Display(Name = "Approved On")]
        public DateTime DateUpdated{ get; set; }
       
        [Display(Name = "Approved By")]
        public string UpdatedBy { get; set; }
        public bool IsEditAllowed { get; set; }
         
    }
}
