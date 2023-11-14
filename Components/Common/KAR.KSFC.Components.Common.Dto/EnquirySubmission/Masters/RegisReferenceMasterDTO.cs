using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters
{
    public class RegisReferenceMasterDTO
    {

        [DisplayName("RegisterReference Code")]
        [Required(ErrorMessage = "The Promoter Code is required")]
        public int RegrefCd { get; set; }

        [DisplayName("RegisterReference Details")]
        [Required(ErrorMessage = "The Promoter Code is required")]
        public string RegrefDets { get; set; }

        

        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }
    }
}
