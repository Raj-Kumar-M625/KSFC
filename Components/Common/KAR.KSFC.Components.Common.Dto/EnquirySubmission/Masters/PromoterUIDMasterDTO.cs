using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters
{
    public class PromoterUIDMasterDTO
    {
        [DisplayName("Promoter Code")]
        [Required(ErrorMessage = "The Promoter Code is required")]
        public int PromoterCode { get; set; }

        [DisplayName("Promote Uid Code")]
        [Required(ErrorMessage = "The Promoter UID is required")]
        public int? PromUid { get; set; }

        

        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }

    }
}
