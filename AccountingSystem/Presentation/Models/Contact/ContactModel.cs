using Presentation.Models.Vendor;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.Contacts
{
    public class ContactModel
    {
        [Key]
        public int? Id { get; set; }

        [ForeignKey("VendorPerson")]
        public int? VendorPersonID { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Contact Person Name")]
        public string Name { get; set; }

        [Required]
        [MaxLength(10)]
        [Display(Name = "Phone")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string Phone { get; set; }

        [MaxLength(100)]
        [Display(Name = "Email")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
        ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
        public bool Status { get; set; }
        public virtual VendorPersonModel VendorPerson { get; set; }
    }
}
