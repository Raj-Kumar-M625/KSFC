using Presentation.Models.Vendor;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.Address
{
    public class AddressesModel
    {
        [Key]
        public int? Id { get; set; }

        [ForeignKey("VendorPerson")]
        public int? VendorPersonID { get; set; }

        [Display(Name = "Address Name")]
        public string Name { get; set; }

        [Display(Name = "Address Type")]
        public string Type { get; set; }

        [MaxLength(250)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        [Display(Name = "Country")]
        public string Country { get; set; }


        [Display(Name = "Pincode")]
        [MaxLength(6)]
        [Required(ErrorMessage = "Pincode is Required")]
        [RegularExpression(@"^\d{6}(-\d{5})?$", ErrorMessage = "Invalid Pincode")]
        public string PinCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Status { get; set; }
        public virtual VendorPersonModel VendorPerson { get; set; }
    }
}
