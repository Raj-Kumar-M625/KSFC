using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Domain.Master;
using System.Collections.Generic;
using Presentation.Models.TDS;

namespace Presentation.Models.Payment
{
    public class TdsPaymentChallanViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int NoOfVendors { get; set; }
        [Required]
        public int NoOfTrans { get; set; }
        [Required]
        public int BankMasterID { get; set; }
        [Required]
        public string TDSSection { get; set; }

        public string ChallanNo { get; set; }

        public string UTRNo { get; set; }
        public bool IsBulkTDS { get; set; }
        public bool IsBulkTDSModified { get; set; }
        public string BSRCode { get; set; }
        public DateTime CreatedBy { get; set; }
        public DateTime ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public DateTime? PaymentDate { get; set; }

        public DateTime? TenderDate { get; set; }
        public DateTime? TDSChallanDate { get; set; }

       // public int TDSStatus { get; set; }
        public int AssementYear { get; set; }
        public int Quarter { get; set; }

        public virtual TdsStatusModel TDSStatus { get; set; } = new TdsStatusModel();

        [Required]
        public decimal? Interest { get; set; }

        public decimal? Penalty { get; set; }

        [Required]
        public decimal? TotalTDSPayment { get; set; }

        [NotMapped]
        public decimal? TDSAmount { get; set; }

        public virtual BankMaster Bank { get; set; }

        public virtual CommonMaster QuarterMaster { get; set; }

        [NotMapped]
        public List<int> BillsId { get; set; }


    }
}
