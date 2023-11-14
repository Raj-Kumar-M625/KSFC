using KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.AssociateSisterConcernDetails
{
    public class SisterConcernDetailsDTO
    {

        [DisplayName("Sister Concerns Id")]
        public int? EnqSisId { get; set; }

        [DisplayName("Enquiry Id")]
        public int? EnqtempId { get; set; }

        [DisplayName("Sister Concerns Name")]
        [Required(ErrorMessage = "Name is required")]
        public string EnqSisName { get; set; }

        [DisplayName("Sister Concerns IFSC")]
        [Required(ErrorMessage = "IFSC is required")]
        public string EnqSisIfsc { get; set; }

        [DisplayName("Banking Facility Code")]
        [Required(ErrorMessage = "Facility is required")]
        public int? BfacilityCode { get; set; }

        [DisplayName("Outstanding Amount")]
        [Required(ErrorMessage = "Required")]
        public decimal? EnqOutamt { get; set; }
        [DisplayName("Default Amount")]
        [Required(ErrorMessage = "Required")]
        public decimal? EnqDeftamt { get; set; }

        [DisplayName("One Time Settlement Amount")]
        [Required(ErrorMessage = "Required")]
        public decimal? EnqOts { get; set; }

        [DisplayName("Relief and settlement Amount")]
        [Required(ErrorMessage = "Required")]
        public string EnqRelief { get; set; }
        [DisplayName("Bank Name")]
        public string bankName { get; set; }

        [DisplayName("Cibil Score")]
        public string EnqSiscibil { get; set; }

        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }
        public virtual BankFacilityMasterDTO BfacilityCodeNavigation { get; set; }

    }
}
