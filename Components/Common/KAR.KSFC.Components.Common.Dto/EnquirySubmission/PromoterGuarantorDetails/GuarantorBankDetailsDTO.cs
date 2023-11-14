using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails
{
    public class GuarantorBankDetailsDTO
    {
        [DisplayName("Bank Id")]
        
        public int EnqGuarbankId { get; set; }
 
         [DisplayName("Enquiry Id")]
        public int? EnqtempId { get; set; }
 
 
         [DisplayName("Promoter Code")]
         public long PromoterCode { get; set; }
 
 
         [DisplayName("Account Type")]
      
        public string GuarAcctype { get; set; }
 
         [DisplayName("Account Number")]
        [Required(ErrorMessage = "The Account Number is required")]
        public string GuarBankaccno { get; set; }
 
 
         [DisplayName("IFSC Code")]
        [Required(ErrorMessage = "The IFSC is required")]
        public string GuarIfsc { get; set; }
 
         [DisplayName("Account Name")]
        [Required(ErrorMessage = "The Account Name is required")]
        public string GuarAccName { get; set; }
 
 
         [DisplayName("Bank Name")]
        [Required(ErrorMessage = "The Bank Name is required")]
        public string GuarBankname { get; set; }
 
 
         [DisplayName("Bank Branch")]
        
        public string GuarBankbr { get; set; }
 
         [DisplayName("Unique Id")]
         
        public string UniqueId { get; set; }
    }
}
