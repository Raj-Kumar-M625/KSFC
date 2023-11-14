using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Master;
using Domain.TDS;

namespace Domain.Payment
{
    public class TdsPaymentChallan
    {
        [Key]
        public int Id { get; set; }

        public int NoOfVendors { get; set; }
        public int NoOfTrans { get; set; }

        public string TDSSection { get; set; }

        [ForeignKey(nameof(Bank))]
        public int BankMasterID { get; set; }

        public string ChallanNo { get; set; }
        [NotMapped]
        public bool IsBulkTDS { get; set; }

        public string UTRNo { get; set; }

        public string BSRCode { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? TenderDate { get; set; }
        public DateTime? TDSChallanDate { get; set; }

      

        [ForeignKey(nameof(QuarterMaster))]
        public int Quarter { get; set; }

        public int AssementYear { get; set; }
        public decimal? Interest { get; set; }

        public decimal? Penalty { get; set; }

        public decimal TotalTDSPayment { get; set; }

        public decimal? TDSAmount { get; set; }

        public virtual BankMaster Bank { get; set; }

        public virtual CommonMaster QuarterMaster { get; set; }

        // public virtual CommonMaster StatusMaster { get; set; }


        public virtual TdsStatus TDSStatus { get; set; }

        [NotMapped]
        public List<int> BillsId { get; set; }
    }
}
