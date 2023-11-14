using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable enable
namespace Application.DTOs.Vendor
{
    public class VendorDetailsDto
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string GSTRegistration { get; set; }
        public bool GST_TDS_Applicable { get; set; }
        public string GSTIN_Number { get; set; }
        public string PAN_Number { get; set; }
        public string TAN_Number { get; set; }
        public string OwnerOrDirectorName { get; set; }
        public string Notes { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
       
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public List<IFormFile> files { get; set; }
        public List<string>? DocumentName { get; set; }
        public string[] OpeningBalance { get; set; }
        public DateTime ModifiedOn { get; set; }
        public virtual VendorDefaultsDto VendorDefaults { get; set; }
        public virtual VendorBankAccountDto VendorBankAccounts { get; set; }
        public virtual VendorPersonDto VendorPerson { get; set; }               
        public virtual VendorBalanceDto VendorBalance { get; set; }


    }
}
#nullable disable
