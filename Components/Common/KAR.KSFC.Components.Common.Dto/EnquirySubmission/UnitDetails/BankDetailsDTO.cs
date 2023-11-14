using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.UnitDetails
{
    public class BankDetailsDTO
    {

        public int? EnqBankId { get; set; }

        [DisplayName("Enquiry Id")]
        public int? EnqtempId { get; set; }

        [DisplayName("Account Type")]
        public string EnqAcctype { get; set; }

        [DisplayName("Account Number")]
        [Required(ErrorMessage = "The Account Number is required")]
        public string EnqBankaccno { get; set; }

        [DisplayName("IFSC Code")]
        [Required(ErrorMessage = "The IFSC code is required")]
        public string EnqIfsc { get; set; }

        [DisplayName("Account HolderName")]
        [Required(ErrorMessage = "The Account Holder Name is required")]
        public string EnqAccName { get; set; }

        [DisplayName("Bank Name")]
        [Required(ErrorMessage = "The Bank Name is required")]
        public string EnqBankname { get; set; }

        [DisplayName("Branch Name")]
        [Required(ErrorMessage = "The Branch Name is required")]
        public string EnqBankbr { get; set; }

        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }

        [DisplayName("Bank Pincode")]
        public string BankPinCode { get; set; }

    }
}
