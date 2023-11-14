using Domain.Address;
using Domain.Contacts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Vendor
{
    public class VendorPerson
    {
        [Key]
        public int Id { get; set; }

       
        public int VendorId { get; set; }
        public string CreatedBy { get; set; } 

        public string? ModifiedBy { get; set; }
        public DateTime CreatedOn {get; set; }
        public DateTime ModifiedOn { get; set; }
        
        public virtual Vendors Vendor { get; set; }
        public virtual Contact Contacts { get; set; }
        public virtual Addresses Addresses { get; set; }
    }
}
