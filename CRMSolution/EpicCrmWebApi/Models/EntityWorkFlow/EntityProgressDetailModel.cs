using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class EntityProgressDetailModel
    {
        [Display(Name ="Profile Id")]
        public long EntityId { get; set; }
        
        [Display(Name ="Profile Name")]
        public string EntityName { get; set; }

        [Display(Name ="Employee Code")]
        public string EmployeeCode { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Display(Name = "Crop")]
        public string TypeName { get; set; }

        [Display(Name = "Season Name")]
        public string SeasonName { get; set; }

        [Display(Name ="Last Activity")]
        public string LastPhase { get; set; }

        [Display(Name ="Last Activity Completion Date")]
        public DateTime? LastPhaseDate { get; set; }

        [Display(Name ="Current Activity")]
        public string CurrentPhase { get; set; }

        [Display(Name = "Current Activity Start Date")]
        public DateTime? CurrentPlannedFromDate { get; set; }

        [Display(Name = "Current Activity End Date")]
        public DateTime? CurrentPlannedEndDate { get; set; }

        public bool IsComplete { get; set; }

        [Display(Name = "Area")]
        public string AreaName { get; set; }

        [Display(Name = "Agreement Number")]
        public string AgreementNumber { get; set; }

        [Display(Name = "Agreement Status")]
        public string AgreementStatus { get; set; }
    }
}