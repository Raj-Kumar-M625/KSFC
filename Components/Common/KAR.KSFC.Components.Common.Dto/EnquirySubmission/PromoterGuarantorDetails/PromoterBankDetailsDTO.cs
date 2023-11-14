using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails
{
    public class PromoterBankDetailsDTO
    {
        [DisplayName("Promoter Bank Id")]
        public int EnqPrombankId { get; set; }

        [DisplayName("Enquiry Id")]
        public int? EnqtempId { get; set; }

        [DisplayName("Promoter Code")]
        public long PromoterCode { get; set; }

        [DisplayName("Accouunt Type")]
        public string PromAcctype { get; set; }

        [DisplayName("Accouunt Number")]
        [Required(ErrorMessage = "Account number is required")]
        public string PromBankaccno { get; set; }

        [DisplayName("IFSC Code")]
        [Required(ErrorMessage = "IFSC code is required")]
        public string PromIfsc { get; set; }

        [DisplayName("Accouunt Name")]
        [Required(ErrorMessage = "Name as in Bank Account is required")]
        public string PromAccName { get; set; }


        [DisplayName("Bank Name")]
        [Required(ErrorMessage = "Bank name is required")]
        public string PromBankname { get; set; }


        [DisplayName("Bank Branch")]
        public string PromBankbr { get; set; }


        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }
    }
}
