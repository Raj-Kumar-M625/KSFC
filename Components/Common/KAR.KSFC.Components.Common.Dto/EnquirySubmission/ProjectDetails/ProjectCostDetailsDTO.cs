using KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.ProjectDetails
{
    public class ProjectCostDetailsDTO
    {

        public int EnqPjcostId { get; set; }
        public int? EnqtempId { get; set; }

        [DisplayName("Project Cost Id")]
        [Required(ErrorMessage = "Project Cost Component is required")]
        public int PjcostCd { get; set; }

        [DisplayName("Project Cost Amount")]
        [Required(ErrorMessage = "Project Cost Amount is required")]
        public decimal? EnqPjcostAmt { get; set; }
        public string EnqPjcostRem { get; set; }
        public string UniqueId { get; set; }
        public virtual ProjectCostMasterDTO PjcostCdNavigation { get; set; }
        public string Operation { get; set; }

    }
}
