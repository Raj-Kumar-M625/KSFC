using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails
{
    public class PromoterMasterDTO
    {
        [DisplayName("Promoter Code")]
        public long PromoterCode { get; set; }
        [DisplayName("Promoter PAN")]
        [Required(ErrorMessage = "PAN is required")]
        public string PromoterPan { get; set; }

        [DisplayName("Promoter Name")]
        [Required(ErrorMessage ="Name is required")]
        public string PromoterName { get; set; }

        [DisplayName("Promoter DOB")]
        [Required(ErrorMessage = "DOB is required")]
        public DateTime? PromoterDob { get; set; }

        public string Age { get; set; }

        [DisplayName("Promoter Gender")]
        [Required(ErrorMessage = "Gender is required")]
        public string PromoterGender { get; set; }

        [DisplayName("Promoter Passport")]
        [Required(ErrorMessage = "Passport is required")]
        public string PromoterPassport { get; set; }

        [DisplayName("Promoter Photo")]
        public string PromoterPhoto { get; set; }

        [DisplayName("Promoter ValidationDate")]
       
        public DateTime? PanValidationDate { get; set; }


        [DisplayName("Email of Promoter")]
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
         @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
         @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Invalid email address")]
        public string PromoterEmailid { get; set; }

        [DisplayName("Mobile Number of Promoter")]
        [Required(ErrorMessage = "Mobile number is required")]
        public Int64? PromoterMobno { get; set; }

        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }



    }
}
