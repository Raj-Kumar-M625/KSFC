using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.ProjectDetails
{
    public class ProjectWorkingCapitalDeatailsDTO
    {


        [DisplayName("Working Id")]
        public int EnqWcId { get; set; }

        [DisplayName("Enquiry Id")]
        public int? EnqtempId { get; set; }

        [DisplayName("IFSC")]
        [Required(ErrorMessage = "Bank IFSC is required")]
        public string EnqWcIfsc { get; set; }

        [DisplayName("Bank")]
        [Required(ErrorMessage = "Bank Name is required")]
        public string EnqWcBank { get; set; }

        [DisplayName("Branch")]
        [Required(ErrorMessage = "Branch Name is required")]
        public string EnqWcBranch { get; set; }

        [DisplayName("Amount")]
        [Required(ErrorMessage = "Amount is required")]
        public decimal? EnqWcAmt { get; set; }

        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }

        public string Operation { get; set; }

    }
}
