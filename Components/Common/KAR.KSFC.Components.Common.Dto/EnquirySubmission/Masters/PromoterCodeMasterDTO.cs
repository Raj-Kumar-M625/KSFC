using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters
{
    public class PromoterCodeMasterDTO
    {

        [DisplayName("Promoter Code")]
        [Required(ErrorMessage = "The Promoter Code is required")]
        public long PromoterCode { get; set; }

        [DisplayName("Promoter Pan")]
        [Required(ErrorMessage = "The Promoter Pan is required")]
        [RegularExpression(@"^([A-Z]{5}\d{4}[A-Z]{ 1})$")]
        public string PromoterPan { get; set; }

        [DisplayName("Promoter NAme")]
        [Required(ErrorMessage = "The Promoter Name is required")]
        public string PromoterName { get; set; }

        [DisplayName("Promoter DOB")]
        [Required(ErrorMessage = "The Promoter DOB is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? PromoterDob { get; set; }

        [DisplayName("Promoter Gender")]
        [Required(ErrorMessage = "The Promoter Gender is required")]
        public string PromoterGender { get; set; }

        [DisplayName("Promoter Passport")]
        [Required(ErrorMessage = "The Promoter Passport is required")]
        [RegularExpression(@"^(?!^0+$)[a-zA-Z0-9]{3,20}$")]
        public string PromoterPassport { get; set; }

        [DisplayName("Promoter Photo")]
        public string PromoterPhoto { get; set; }


        [DisplayName("PanValidation Date")]
        [Required(ErrorMessage = "The Pan Validation Date is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? PanValidationDate { get; set; }

        [DisplayName("Email of Promoter")]
        [Required(ErrorMessage = "The Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string PromoterEmailid { get; set; }

        [DisplayName("Mobile of Promoter")]
        [Required(ErrorMessage = "The Mobile Number is required")]
        public int? PromoterMobno { get; set; }


        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }

    }
}
