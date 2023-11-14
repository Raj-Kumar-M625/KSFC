using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class AppSignInReportDataModel
    {
        [Display(Name = "Employee Name")]
        public string Name { get; set; }

        [Display(Name = "Employee Code")]
        public string StaffCode { get; set; }

        public string ExpenseHQCode { get; set; }

        [Display(Name="# Active Days")]
        public long DaysActive { get; set; }

        public string Phone { get; set; }

        [Display(Name="Employee")]
        public bool IsActive { get; set; }

        [Display(Name = "Signup Date")]
        public DateTime SignupDate { get; set; }

        public bool HasSignedUpBeforeReportEndDate { get; set; }
    }
}