using Domain.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Domain.Payment;
using Domain.GSTTDS;

namespace Application.DTOs.GSTTDS
{
    public class GsttdsPaymentChallanDto
    {
        [Key]
        public int Id { get; set; }
        public int BankMasterID { get; set; }
        public int NoOfVendors { get; set; }
        public int NoOfTrans { get; set; }
        public string UTRNo { get; set; }
        public bool IsBulkGSTTDS { get; set; }
        public bool IsBulkSTTDSModified { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public DateTime? PaidDate { get; set; }

        [ForeignKey(nameof(StatusMaster))]
        public virtual GsttdsStatusDto GSTTDSStatus { get; set; }

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

        public virtual CommonMaster StatusMaster { get; set; }
        public virtual BankMaster Bank { get; set; }

        [NotMapped]
        public List<int> BillsId { get; set; }
    }
}
