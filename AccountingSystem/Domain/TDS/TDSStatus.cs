using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Master;
using Domain.Bill;

namespace Domain.TDS
{
    public class TdsStatus
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Vendor")]
        public int VendorId { get; set; }

        [ForeignKey("Bill")]
        public int BillID { get; set; }

        [ForeignKey(nameof(StatusMaster))]
        public int StatusCMID { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifedBy { get; set; }

        [ForeignKey("TDSPaymentChallan")]
        public int? TDSPaymentChallanID { get; set; }
        public virtual CommonMaster StatusMaster { get; set; }
      
        public virtual Bills Bill { get; set; }

    }
}
