using KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails
{
    public class GuarantorDetailsDTO
    {

        [DisplayName("Guaranter Id")]
        public int? EnqGuarId { get; set; }

        [DisplayName("Enquiry Id")]
        public int? EnqtempId { get; set; }

        [DisplayName("Promoter Code")]
        
        public long PromoterCode { get; set; }

        [DisplayName("Domicile")]
        [Required(ErrorMessage = "Domicile is required")]
        public int DomCd { get; set; }

        [DisplayName("Guaranter Name")]
        public string GuarName { get; set; }


        [DisplayName("PAN Number")]
        public string Pan { get; set; }

        [DisplayName("Pincode ")]
        public string pinCode { get; set; }

        [DisplayName("Cibil Score")]
        public string EnqGuarcibil { get; set; }


        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }
        public DomicileMasterDTO DomicileMasterDTO { get; set; }
        public GuarantorBankDetailsDTO GuarantorBankDetails { get; set; }
        public PromoterMasterDTO PromoterMaster { get; set; }
    }
}
