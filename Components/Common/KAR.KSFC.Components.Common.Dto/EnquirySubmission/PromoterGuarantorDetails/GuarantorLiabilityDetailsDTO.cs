using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails
{
    public class GuarantorLiabilityDetailsDTO
    {
        [DisplayName("Liability Id")]
        public int? EnqGuarliabId { get; set; }
 
        [DisplayName("Enquiry Id")]
        public int EnqtempId { get; set; }


        [DisplayName("Promoter Code")]
        [Required(ErrorMessage = "Guarantor is required")]
        public long PromoterCode { get; set; }


        [DisplayName("Liability Description")]
        [Required(ErrorMessage = "Description is required")]
        public string GuarLiabDesc { get; set; }
        [DisplayName("Liability Value")]
        [Required(ErrorMessage = "Value is required")]
        public decimal? GuarLiabValue { get; set; }


        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }

        public int? EnqGuarId { get; set; }
        public virtual GuarantorDetailsDTO GuarantorDetailsDTO { get; set; }
        public virtual GuarantorNetWorthDetailsDTO GuarantorNetWorthDetailsDTO { get; set; }
    }
}
