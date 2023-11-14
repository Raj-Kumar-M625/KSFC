using Application.DTOs.Vendor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Contacts
{
    public class ContactDto
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("VendorPerson")]
        public int VendorPersonID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
        public virtual VendorPersonDto VendorPerson { get; set; }
    }
}
