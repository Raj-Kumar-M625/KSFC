﻿using Domain.GenarteBankfile;
using Domain.Payment;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.GenerateBankfile
{
    public class MappingPaymentGenerateBankFile
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("GenerateBankFile")]
        public int GenerateBankFileId { get; set; }
        public int VendorId { get; set; }
        [ForeignKey("Payments")]
        public int PaymentId { get; set; }

        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifedBy { get; set; }
        public virtual Payments Payments { get; set; }
        public virtual GenerateBankFile GenerateBankFile { get; set; }

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
