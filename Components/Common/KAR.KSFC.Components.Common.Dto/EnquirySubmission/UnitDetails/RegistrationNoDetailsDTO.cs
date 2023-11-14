using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.UnitDetails
{
    public class RegistrationNoDetailsDTO
    {

        [DisplayName("Registration Id")]
        public int? EnqRegnoId { get; set; }
        [DisplayName("Enquiry Id")]
        public int EnqtempId { get; set; }

        [DisplayName("Registration Type")]
        [Required(ErrorMessage = "The Registration type is required")]
        public int RegrefCd { get; set; }

        [DisplayName("Registration Number")]
        [Required(ErrorMessage = "The Registration number is required")]
        public string EnqRegno { get; set; }


        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }


        public string RegTypeText { get; set; }
    }
}
