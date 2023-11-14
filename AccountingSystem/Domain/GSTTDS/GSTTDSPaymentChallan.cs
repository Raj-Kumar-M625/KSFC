using Domain.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.GSTTDS
{
    public class GsttdsPaymentChallan
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(Bank))]
        public int? BankMasterId { get; set; }

        public int NoOfVendors { get; set; }
        public int NoOfTrans { get; set; }

        public string UTRNo { get; set; }

        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public DateTime? PaidDate { get; set; }

        public virtual GsttdsStatus GSTTDSStatus { get; set; }
        public virtual BankMaster Bank { get; set; }

        public string GSTR7ACertificate { get; set; }
        public string AcknowledgementRefNo { get; set; }

        public int AssementYearCMID { get; set; }

        [DefaultValue("true")]
        public decimal Interest { get; set; }

        [DefaultValue("true")]
        public decimal Penalty { get; set; }

        [DefaultValue("true")]
        public decimal TotalGSTTDSPayment { get; set; }

        public decimal PaidAmount { get; set; }

        [NotMapped]
        public List<int> BillsId { get; set; }

       
      
    }
}
