using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.Enquiry
{
    public class BasicDetailsDTO
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please Enter Name.")]
        [MaxLength(10)]
        public string Name { get; set; }
       
        [Display(Name = "Address")]
        [Required(ErrorMessage = "Please Enter Address.")]
        [MaxLength(80)]
        public string Address { get; set; }
       
        [MaxLength(20)]
        public string Place { get; set; }
        
        
        [Display(Name = "Pin Code")]
        [RegularExpression(@"^([0-9]{6})$", ErrorMessage = "Invalid Pin Code.")]
        [Required(ErrorMessage = "Please Enter Pin Code.")]
        [MaxLength(6)]
        public string PinCode { get; set; }
        
        
        [EmailAddress]
        [MaxLength(40)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct Email Id.")]
        public string Email { get; set; }
       
        
        [Display(Name = "Branch")]
        [Required(ErrorMessage = "Please select Branch.")]
        public int Branch { get; set; }
       
        
        [Required(ErrorMessage = "Name of Unit is required.")]
        [MaxLength(40)]
        public string NameOfUnit { get; set; }
        
        public string PurposeOfLoan { get; set; }
        
        
        [Display(Name = "Period Of Repayment")]
        [MaxLength(10)]
        public string PeriodOfRepayment { get; set; }
        public string ConstitutionType { get; set; }
        [MaxLength(50)]
        public string SizeOfFirm { get; set; }
        [MaxLength(40, ErrorMessage = "Name of Firm cannot be longer than 40 characters.")]
        public string NameOfFirem { get; set; }
        [MaxLength(50)]
        public string NameOfProduct { get; set; }
        [MaxLength(50)]
        [Display(Name = "Type Of Industry")]
        public string TypeOfIndustry { get; set; }
        [Display(Name = "Expected Loan Amount")]
        [MaxLength(20)]
        public string ExpectedLoanAmount { get; set; }
        public string District { get; set; }
        public string Taluk { get; set; }
        public string Hobli { get; set; }
        public string Village { get; set; }
        public string NatureOfPremises { get; set; }
    }
}
