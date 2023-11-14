using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class EmployeeRecordModel
    {
        [Display(Name = "CRM Id")]
        public long EmployeeId { get; set; }
        public long TenantId { get; set; }
        public Nullable<long> ManagerId { get; set; }
        public string Name { get; set; }

        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        [Display(Name="Employee")]
        public bool IsActive { get; set; }

        [Display(Name = "IMEI")]
        public string IMEI { get; set; }

        [Display(Name = "Phone")]
        public bool OnPhone { get; set; }

        [Display(Name = "Web")]
        public bool OnWebPortal { get; set; }

        [Display(Name = "Active (SAP)")]
        public bool IsActiveInSap { get; set; }
        public string Phone { get; set; }

        [Display(Name = "Phone Log")]
        public bool SendLogFromPhone { get; set; }

        [Display(Name = "Exec App")]
        public bool ExecAppAccess { get; set; }

        [Display(Name = "Auto Upload From Phone")]
        public bool AutoUploadFromPhone { get; set; }

        [Display(Name = "Activity Page")]
        public string ActivityPageName { get; set; }

        [Display(Name = "Location From")]
        public string LocationFromType { get; set; }

        [Display(Name = "Enhanced Debug Enabled?")]
        public bool EnhancedDebugEnabled { get; set; }

        [Display(Name = "Test Features Enabled?")]
        public bool TestFeatureEnabled { get; set; }

        [Display(Name = "Voice Features Enabled?")]
        public bool VoiceFeatureEnabled { get; set; }

        [Display(Name = "Exec App Detail Level (1=Zone/2/3/4=HQ)")]
        public int ExecAppDetailLevel { get; set; }
    }
}