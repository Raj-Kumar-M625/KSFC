using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters
{
    public class IndusSizeMasterDTO
    {

        [DisplayName("Size Code")]
        [Required(ErrorMessage = "The Size Code is required")]
        public int SizeCd { get; set; }

        [DisplayName("Size Details")]
        [Required(ErrorMessage = "The Size Details is required")]
        public string SizeDets { get; set; }

        [DisplayName("Size Flag")]
        public int? SizeFlag { get; set; }

        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }
    }
}
