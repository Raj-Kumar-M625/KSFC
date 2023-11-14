using Presentation.Models.Master;
using Presentation.Models.Vendor;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.Bill
{
    public class BillStatusModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Vendor")]
        public int VendorId { get; set; }
        [ForeignKey("Bill")]
        public int BillID { get; set; }
        public string Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifedBy { get; set; }
        public CommonMasterModel StatusMaster { get; set; }
        public virtual BillModel Bill { get; set; }

        public virtual VendorViewModel Vendor { get; set; }
    }
}
