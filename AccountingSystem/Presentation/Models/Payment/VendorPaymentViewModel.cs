using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Domain.Payment;
using Presentation.Models.Bill;
using Presentation.Models.Payment;

namespace Presentation.Models.Vendor
{
    public class VendorPaymentViewModel
    {
        [Key]
        public int? Id { get; set; }

        //[ForeignKey("Vendor")]
        public int? VendorID { get; set; }
        public string PaymentBillReference { get; set; }
        public decimal PayableAmount { get; set; }
        public decimal PaymentAmount { get; set; }

        public decimal BalanceAmount { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string PaymentReferenceNo { get; set; }

        public DateTime PaymentDate { get; set; }

        public DateTime ModifiedOn { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]

        public decimal? PaidAmount { get; set; }


        //public string PaymentStatus { get; set; }
        [Required]
       public string Remarks { get; set; }
       public string Type { get; set; }
       public string Description { get; set; }
       public decimal AdvanceAmountUsed { get; set; }
       public decimal BalanceAdvanceAmount { get; set; }


        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ApprovedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual VendorViewModel Vendor { get; set; }
        public virtual PaymentStatus PaymentStatus  { get; set;}
        public List<MappingAdvancePaymentModel> MappingAdvancePayment { get; set; }
        #region To generate bank file based on payment details
        public int NumnberOfVenders { get; set; }
        public int NumberOfTransaction { get; set; }
        #endregion
    }
}
