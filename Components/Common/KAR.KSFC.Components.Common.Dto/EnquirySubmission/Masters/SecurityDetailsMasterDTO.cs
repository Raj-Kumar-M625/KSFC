using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters
{
    public class SecurityDetailsMasterDTO
    {
        [DisplayName("Security Code")]
       // [Required(ErrorMessage = "The Security Code is required")]
        public short SecCd { get; set; }

        [DisplayName("Security Details")]
       // [Required(ErrorMessage = "The Security Details is required")]
        public string SecDets { get; set; }

        [DisplayName("Security Flag")]
        public byte? SecTy { get; set; }

        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }
    }
}
