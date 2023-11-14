using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Domain.Master;
using System.Collections.Generic;

namespace Presentation.Models.GSTTDS
{
    public class GsttdsPaymentChallanViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int NoOfVendors { get; set; }
        [Required]
        public int NoOfTrans { get; set; }
        [Required]
        public int BankMasterID { get; set; }

        public string UTRNo { get; set; }       
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public DateTime? PaidDate { get; set; }
        public int AssementYearCMID { get; set; }
        public bool IsBulkGSTTDS { get; set; }
        public bool IsBulkSTTDSModified { get; set; }

        public decimal? Interest { get; set; }
        public string GSTR7ACertificate { get; set; }
        public string AcknowledgementRefNo { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.#}")]
        public decimal PaidAmount { get; set; }
        public decimal? Penalty { get; set; }
        public decimal? TotalGSTTDSPayment { get; set; }
        public virtual GsttdsStatusModel GSTTDSStatus { get; set; } = new GsttdsStatusModel();
        public virtual BankMaster Bank { get; set; }

        [NotMapped]
        public decimal? GSTTDSAmount { get; set; }

        [NotMapped]
        public List<int> BillsId { get; set; }
    }
}
