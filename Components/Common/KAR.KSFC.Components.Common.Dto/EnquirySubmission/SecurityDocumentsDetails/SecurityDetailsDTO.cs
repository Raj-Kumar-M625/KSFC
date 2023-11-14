using KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.SecurityDocumentsDetails
{
    public class SecurityDetailsDTO
    {

        [DisplayName("Enquiry Security Id")]
        public int? EnqSecId { get; set; }

        [DisplayName("Enquiry Enquiry Id")]
        public int? EnqtempId { get; set; }

        [DisplayName("Security Id")]
        [Required(ErrorMessage = "Security type is required")]
        public int? SecCode { get; set; }

        [DisplayName("Security Code")]
        [Required(ErrorMessage = "The Security detail is required")]
        public short SecCd { get; set; }

        [DisplayName("Security Description")]
        [Required(ErrorMessage = "Security description is required")]
        public string EnqSecDesc { get; set; }


        [DisplayName("Security Value")]
        [Required(ErrorMessage = "Security value is required")]
        public decimal? EnqSecValue { get; set; }


        [DisplayName("Security Name")]
        [Required(ErrorMessage = "Security name is required")]
        public string EnqSecName { get; set; }


        [DisplayName("Promoter Relation Code")]
        [Required(ErrorMessage = "Relation code is required")]
        public int PromrelCd { get; set; }


        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }

        public string Operation { get; set; }
        public virtual SecurityDetailsMasterDTO SecCdNavigation { get; set; }

        public virtual ProjSecurityMasterDTO SecCodeNavigation { get; set; }

    }
}
