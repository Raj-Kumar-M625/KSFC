using Presentation.Models.Vendor;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.GenerateBankFiles
{
    /// <summary>
    /// Author:Swetha M Date:04/11/2022
    /// Purpose:Generate Bnak File Mapping Table
    /// </summary>
    /// <returns></returns>
    public class MappingPaymentGenerateBankFileModel
    {
        [Key]
        public int Id { get; set; }

        public int GenerateBankFileId { get; set; }
        public int VendorId { get; set; }
        [ForeignKey("Payments")]
        public int PaymentId { get; set; }

        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifedBy { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }

        public virtual VendorPaymentViewModel Payments { get; set; }
        public virtual GenerateBankFileModel GenerateBankFile { get; set; }

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
