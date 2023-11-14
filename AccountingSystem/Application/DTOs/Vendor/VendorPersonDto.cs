using Application.DTOs.Address;
using Application.DTOs.Contacts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Vendor
{
    public class VendorPersonDto
    {
        [Key]
        public int Id { get; set; }

        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public int VendorId { get; set; }

        public virtual VendorDetailsDto Vendor { get; set; }
        public virtual ContactDto Contacts { get; set; }
        public virtual AddressesDto Addresses { get; set; }
    }
}
