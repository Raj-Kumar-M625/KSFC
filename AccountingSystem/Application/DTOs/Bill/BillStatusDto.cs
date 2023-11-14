using Application.DTOs.Master;
using Application.DTOs.Vendor;
using Domain.Bill;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Bill
{
    public class BillStatusDto
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
        public CommonMasterDto StatusMaster { get; set; }
        public virtual BillsDto Bill { get; set; }
        public virtual VendorDetailsDto Vendor { get; set; }
    }
}
