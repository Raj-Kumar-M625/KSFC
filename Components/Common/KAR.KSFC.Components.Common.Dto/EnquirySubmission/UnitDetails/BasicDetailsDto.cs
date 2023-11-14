using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.UnitDetails
{
    public class BasicDetailsDto
    {
        [DisplayName("Promoter PAN")]
        public string PromoterPan { get; set; }

        [DisplayName("Enquiry Basic Details Id")]
        public int? EnqBdetId { get; set; }

        [DisplayName("Enquiry Id")]
        public int? EnqtempId { get; set; }

        [DisplayName("Applicant Name")]
        [Required(ErrorMessage = "Name is required")]
        public string EnqApplName { get; set; }

        [DisplayName("Applicant Address")]
        [Required(ErrorMessage = "Address is required")]
        public string EnqAddress { get; set; }

        [DisplayName("Place of Applicant")]
        [Required(ErrorMessage = "Place is required")]
        public string EnqPlace { get; set; }

        [DisplayName("Pincode of Applicant")]
        [Required(ErrorMessage = "Pincode is required")]
        public int? EnqPincode { get; set; }

        [DisplayName("Email of Promoter")]
        [Required(ErrorMessage = "Email Address is required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
         @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
         @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Invalid email address")]
     
        public string EnqEmail { get; set; }

        [DisplayName("Additional Loan")]
        public int? AddlLoan { get; set; }

        [DisplayName("Name of Unit")]
        [Required(ErrorMessage = "Unit Name is required")]
        public string UnitName { get; set; }

        [DisplayName("Months for Repayment")]
        [Required(ErrorMessage = "Repayment Month is required")]
        public int? EnqRepayPeriod { get; set; }

        [DisplayName("Expected LoanAmount")]
        [Required(ErrorMessage = "Expected Loan Amount is required")]
        public int? EnqLoanamt { get; set; }

        [DisplayName("Const Id")]
        public int? ConstCd { get; set; }
        [DisplayName("Industry type")]
        [Required(ErrorMessage = "Industry type is required")]
        public int IndCd { get; set; }
        public string ConstType { get; set; }

        [DisplayName("Loan Purpose Id")]
        public int? PurpCd { get; set; }
        public string PurposeOfLoan { get; set; }

        [DisplayName("Industry Size")]
        public int SizeCd { get; set; }
        public string SizeOfFirm { get; set; }

        [DisplayName("Product Code")]
        public int ProdCd { get; set; }
        public string ProdCode { get; set; }

        [DisplayName("Village Code")]
        [Required(ErrorMessage = "The Village code is required")]
        public int VilCd { get; set; }

        public string VillageName { get; set; }


        [DisplayName("Permises Type Code")]

        public int PremCd { get; set; }
        public string PremCode { get; set; }

        [DisplayName("Branch Office")]
        public byte OffcCd { get; set; }
        public string OffcCode { get; set; }

        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }

        [DisplayName("Industry Type")]
        public string TypeOfIndustry { get; set; }
        [DisplayName("District")]
        [Required(ErrorMessage = "District is required")]
        public int DistrictCd{ get; set; }
        public int TalukaCd { get; set; }
        public int HobliCd { get; set; }


        [DisplayName("District")]
        public string District { get; set; }
        [DisplayName("Taluk")]
        public string Taluk { get; set; }
        [DisplayName("Hobli")]
        public string Hobli { get; set; }


        [DisplayName("Promotor Class")]
        [Required(ErrorMessage = "Promotor class is required")]
        public int? PromotorClass { get; set; } 
    }
}
