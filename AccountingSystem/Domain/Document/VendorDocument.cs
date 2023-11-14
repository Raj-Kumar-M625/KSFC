using Domain.Vendor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Document
{
    public class VendorDocument
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Vendor")]
        public int VendorId { get; set; }

        [ForeignKey("Documents")]
        public int DocumentId { get; set; }
        public bool Status { get; set; }
        public virtual Vendors Vendor { get; set; }
        public virtual Documents Document { get; set; }
    }
}
