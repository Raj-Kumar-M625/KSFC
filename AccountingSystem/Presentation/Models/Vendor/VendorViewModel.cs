using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Models.Vendor
{
    public class VendorViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Vendor Name")]
        public string Name { get; set; }

        [Display(Name = "GST Registration")]
        [Required]
        public string GSTRegistration { get; set; }

        [Display(Name = "GST TDS Applicable ?")]
        public bool GST_TDS_Applicable { get; set; }

        [MaxLength(15)]
        [Display(Name = "GSTIN [00AAAAA0000A0Z0]")]

        [RegularExpression("^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9]{1}([A-Za-z]){1}[a-zA-Z0-9]{1}$",ErrorMessage = "Invalid GST Number")]
        public string GSTIN_Number { get; set; }

        [MaxLength(10)]
        [Display(Name = "PAN No [AAAAA0000A]")]
        [Required]
        [RegularExpression("^([A-Za-z]){5}([0-9]){4}([A-Za-z]){1}$",ErrorMessage = "Invalid PAN Number")]
        public string PAN_Number { get; set; }
        [MaxLength(10)]
        [Display(Name = "TAN No [AAAA00000A]")]
        [Required]
        [RegularExpression("^([A-Za-z]){4}([0-9]){5}([A-Za-z]){1}$",ErrorMessage = "Invalid TAN Number")]
        public string TAN_Number { get; set; }


        [MaxLength(100)]
        [Required]
        [Display(Name = "Owner/Director Name")]
        public string OwnerOrDirectorName { get; set; }

        [MaxLength(500)]
        [Display(Name = "Notes")]
        public string Notes { get; set; }

        [MaxLength(500)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Is Vendor Status Active ?")]
        public bool Status { get; set; }

       
     
        public List<IFormFile> files { get; set; }
        public List<string>? DocumentName { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
      
        public string FileType { get; set; }

        public virtual VendorDefaultsModel VendorDefaults { get; set; } /*= new VendorDefaultsModel();*/
        public virtual VendorBankAccountModel VendorBankAccounts { get; set; } /*= new VendorBankAccountModel();*/
        public virtual VendorPersonModel VendorPerson { get; set; } /*= new VendorPersonModel();*/
        //public virtual DocumentsModel VendorDocuments { get; set; } = new DocumentsModel();
        public virtual VendorBalanceModel VendorBalance { get; set; } /*= new VendorBalanceModel();*/
    }
}
