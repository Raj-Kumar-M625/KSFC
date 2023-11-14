using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters
{
    public class PremisesMasterDTO
    {


        [DisplayName("Premises Code")]
        [Required(ErrorMessage = "The Permises Code is required")]
        public int PremCd { get; set; }


        [DisplayName("Premises Details")]
        [Required(ErrorMessage = "The Permises Details is required")]
        public string PremDets { get; set; }
 

        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }
    }
}
