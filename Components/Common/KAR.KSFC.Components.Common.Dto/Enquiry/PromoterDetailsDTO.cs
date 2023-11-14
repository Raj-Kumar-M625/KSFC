using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace KAR.KSFC.Components.Common.Dto.Enquiry
{
    public class PromoterDetailsDTO
    {
        [Display(Name = "S. No.")]
        public int? Id { get; set; }
        [Display(Name = "Name of Promoter")]
        [Required(ErrorMessage = "Please Enter Promoter Name.")]
        [MaxLength(20, ErrorMessage = "Promoter Name cannot be longer than 20 characters.")]
        public string PromoName { get; set; }
        [Display(Name = "Position / Designation")]
        [MaxLength(50)]
        public string Designation { get; set; }
        [Display(Name = "Date of Birth")]
        [MaxLength(20)]
        public string Age { get; set; }
        public string DateOfBirth { get; set; }
        [Display(Name = "% of Shareholding")]
        [MaxLength(10, ErrorMessage = "Shareholding cannot be longer than 10 characters.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Invalid Share Percent.")]
        public string SharePercent { get; set; }
        [Display(Name = "Domicile Status")]
        public string DomicileStatus { get; set; }
        [Display(Name = "PAN of Promoter")]
        [MaxLength(10)]
        [RegularExpression("^([A-Za-z]){5}([0-9]){4}([A-Za-z]){1}$", ErrorMessage = "Invalid PAN Number")]
        [Required(ErrorMessage = "Please Enter PAN Number.")]
        public string PronmoPan { get; set; }
        [Display(Name = "Passport No.")]
        [MaxLength(20)]
        public string PassportNum { get; set; }
        [Display(Name = "Aadhaar")]
        [MaxLength(12)]
        [RegularExpression(@"^([0-9]{12})$", ErrorMessage = "Invalid Adhaar.")]
        public string AdhaarNum { get; set; }
        [Display(Name = "Experience (Years)")]
        [MaxLength(10)]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Invalid Experience.")]
        public string Experince { get; set; }
        [Display(Name = "Experience Details")]
        [MaxLength(100)]
        public string ExpDetails { get; set; }
        public BankDetailsDTO BankDetails { get; set; }
        [Display(Name = "Consent for taking CIBIL Score")]
        public bool ConsentForCIBILScore { get; set; }
        [Display(Name = "Upload Photo")]
        public IFormFile Photo { get; set; }
        //public List<LiabilityDetails> ListLiabilityDetails { get; set; }
        //public List<AssetDetails> ListAssetDetails { get; set; }
        //public List<NetWorth> ListNetworth { get; set; }
    }
}
