using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class CustomerModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(20, ErrorMessage = "Customer Code be upto 20 characters only.")]
        public string Code { get; set; }

        [Display(Name = "Customer Name")]
        [Required]
        [MaxLength(50, ErrorMessage = "Customer Name can be upto 50 characters only.")]
        public string Name { get; set; }

        [Display(Name = "Contact #")]
        [Required]
        [MaxLength(50, ErrorMessage = "Contact number can be upto 50 characters only.")]
        [RegularExpression(@"^[0-9]{1,10}$", ErrorMessage = "Please enter valid Contact Number")]
        public string PhoneNumber { get; set; }

        [MaxLength(20, ErrorMessage = "Type can be upto 20 characters only.")]
        public string Type { get; set; } // Dealer/P.Distributor/Distributor 

        [Display(Name = "Credit Limit")]
        [RegularExpression(@"((-)?(\d+)(\.\d{1,2})?)$", ErrorMessage = "Please enter valid Credit Limit")]
        public decimal CreditLimit { get; set; }
 
        [RegularExpression(@"((-)?(\d+)(\.\d{1,2})?)$", ErrorMessage = "Please enter valid Out Standing Limit")]
        public decimal Outstanding { get; set; }

        [Display(Name = "Long overdue")]
        [RegularExpression(@"((-)?(\d+)(\.\d{1,2})?)$", ErrorMessage = "Please enter valid overdue Limit")]
        public decimal LongOutstanding { get; set; }

        [RegularExpression(@"((-)?(\d+)(\.\d{1,2})?)$", ErrorMessage = "Please enter valid Target")]
        public decimal Target { get; set; }

        [RegularExpression(@"((-)?(\d+)(\.\d{1,2})?)$", ErrorMessage = "Please enter valid Sales Limit")]
        public decimal Sales { get; set; }

        [RegularExpression(@"((-)?(\d+)(\.\d{1,2})?)$", ErrorMessage = "Please enter valid Payment Limit")]
        public decimal Payment { get; set; }

        [MaxLength(10, ErrorMessage = "HQCode can be upto 10 characters only.")]
        public string HQCode { get; set; }

        //[MaxLength(50, ErrorMessage = "District can be upto 50 characters only.")]
        //public string District { get; set; }

        //[MaxLength(50, ErrorMessage = "State can be upto 50 characters only.")]
        //public string State { get; set; }

        //[MaxLength(50, ErrorMessage = "Branch can be upto 50 characters only.")]
        //public string Branch { get; set; }

        //[RegularExpression(@"^[0-9]{6}$", ErrorMessage = "PinCode must be numeric")]
        //public string Pincode { get; set; }

        [Display(Name = "Address")]
        [MaxLength(50, ErrorMessage = "Address can be upto 50 characters only.")]
        public string Address1 { get; set; }

        [Display(Name = "Street")]
        [MaxLength(50, ErrorMessage = "Street can be upto 50 characters only.")]
        public string Address2 { get; set; }

        //[Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [MaxLength(50, ErrorMessage = "Email can be upto 50 charactesr only.")]
        public string Email { get; set; }

        public bool Active { get; set; }
    }
}