using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters
{
    public class IndusTypeMasterDTO
    {
        [DisplayName("Industry Code")]
        [Required(ErrorMessage = "The Industry Code is required")]
        public int IndCd { get; set; }

        [DisplayName("Industry Details")]
        [Required(ErrorMessage = "The Industry Details is required")]
        public string IndDets { get; set; }
         
        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }
    }
}
