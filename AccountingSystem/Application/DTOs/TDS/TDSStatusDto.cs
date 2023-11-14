using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Master;

namespace Application.DTOs.TDS
{
    public class TdsStatusDto
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("Vendor")]
        public int VendorId { get; set; }
        [ForeignKey("Bill")]
        public int BillID { get; set; }

        [ForeignKey("TDSPaymentChallan")]
        public int TDSPaymentChallanID { get; set; }
        
        [ForeignKey(nameof(StatusMaster))]
        public int Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifedBy { get; set; }
        public virtual CommonMaster StatusMaster { get; set; }


    }
}
