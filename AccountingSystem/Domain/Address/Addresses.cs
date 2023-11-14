using Domain.Vendor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Address
{
    public class Addresses
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("VendorPerson")]
        public int VendorPersonID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PinCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public bool Status { get; set; }
        public virtual VendorPerson VendorPerson { get; set; }
    }
}
