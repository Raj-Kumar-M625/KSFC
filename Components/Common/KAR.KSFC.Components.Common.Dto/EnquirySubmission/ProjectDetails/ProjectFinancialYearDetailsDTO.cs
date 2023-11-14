using KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.ProjectDetails
{
    public class ProjectFinancialYearDetailsDTO
    {

        [DisplayName("Financial Year Id")]
        public int? EnqPjfinId { get; set; }

        [DisplayName("Enquiry Id")]
        public int? EnqtempId { get; set; }

        [DisplayName("Financial Year Code")]
        [Required(ErrorMessage = "Financial year is required")]
        public int? FinyearCode { get; set; }

        [DisplayName("Financial Component Code")]
        [Required(ErrorMessage = "Financial component is required")]
        public int FincompCd { get; set; }


        [DisplayName("Financial Amount")]
        [Required(ErrorMessage = "Financial amount is required")]
        public decimal? EnqPjfinamt { get; set; }

        [DisplayName("Provisional Account")]
        public bool WhPjprov { get; set; }


        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }

        public string Operation { get; set; }
        public virtual FinancialYearMasterDTO FinyearCodeNavigation { get; set; }
        public virtual FinancialComponentMasterDTO FincompCdNavigation { get; set; }
    }
}
