using Presentation.Models.Address;
using Presentation.Models.Contacts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.Vendor
{
    public class VendorPersonModel
    {
        [Key]
        public int? Id { get; set; }

        [ForeignKey("Vendor")]
        public int? VendorId { get; set; }
        public string CreatedBy { get; set; } 
        public string ModifiedBy { get; set; }
        public virtual VendorViewModel Vendor { get; set; }
        public virtual ContactModel Contacts { get; set; } = new ContactModel();
        public virtual AddressesModel Addresses { get; set; } = new AddressesModel();
    }
}
