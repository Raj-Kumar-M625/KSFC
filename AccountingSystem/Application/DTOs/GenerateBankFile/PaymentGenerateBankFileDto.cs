using Application.DTOs.Payment;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.DTOs.GenerateBankFile
{
    public class PaymentGenerateBankFileDto
    {
        [Key]
        public int Id { get; set; }

        public int GenerateBankFileId { get; set; }
        public int VendorId { get; set; }
        public int PaymentId { get; set; }

        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifedBy { get; set; }
        public PaymentDto Payments { get; set; }
        public virtual GenerateBankFileDto GenerateBankFile { get; set; }

        [NotMapped]
        public string VendorAccountNumber { get; set; }
        [NotMapped]
        public string VendorBankName { get; set; }
        [NotMapped]
        public string VendorBranchName { get; set; }

        [NotMapped]
        public string VendorIfscCode { get; set; }

    }
}
