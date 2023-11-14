using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters
{
    public class EgStatusMasterDTO
    {

        [DisplayName("Status Code")]
        [Required(ErrorMessage = "The Status Code is required")]
        public int EsmCode { get; set; }

        [DisplayName("Status Description")]
        [Required(ErrorMessage = "The Status Description is required")]
        public string EsmDescription { get; set; }

        [DisplayName("Status Flag")]
        [Required(ErrorMessage = "The Status Flag is required")]
        public int EsmFlag { get; set; }

        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }
    }
}
