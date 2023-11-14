using Domain.Master;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.GSTTDS
{
    public class GsttdsStatusModel
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("Vendor")]
        public int VendorId { get; set; }
        [ForeignKey("Bill")]
        public int BillID { get; set; }
        [ForeignKey(nameof(StatusMaster))]
        public int Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifedBy { get; set; }
        public virtual CommonMaster StatusMaster { get; set; }
    }
}
