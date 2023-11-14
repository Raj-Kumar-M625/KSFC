using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class AppSignUpReportDataModel
    {
        [Display(Name = "Employee Name")]
        public string Name { get; set; }

        [Display(Name = "Signup Date")]
        public DateTime SignupDate { get; set; }

        [Display(Name = "Employee Code")]
        public string StaffCode { get; set; }

        public string ExpenseHQCode { get; set; }

        public string Phone { get; set; }

        [Display(Name = "Employee")]
        public bool IsActive { get; set; }

        [Display(Name = "App Version")]
        public string AppVersion { get; set; }

        [Display(Name = "Phone Model")]
        public string PhoneModel { get; set; }

        [Display(Name = "Phone OS")]
        public string PhoneOS { get; set; }

        [Display(Name = "Last Access Date")]
        public DateTime LastAccessDate { get; set; }
    }
}