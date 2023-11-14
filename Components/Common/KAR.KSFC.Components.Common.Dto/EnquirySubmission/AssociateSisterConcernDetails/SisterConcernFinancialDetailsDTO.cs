using KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.AssociateSisterConcernDetails
{
    public class SisterConcernFinancialDetailsDTO
    {

        [DisplayName("Financial Id")]
        public int? EnqSisfinId { get; set; }


        [DisplayName("SisterConcerns Id")]
        [Required(ErrorMessage = "Sister Concerns is required")]
        public int? EnqSisId { get; set; }

        [DisplayName("Financial Year Code")]
        [Required(ErrorMessage = "Financial year is required")]
        public int? FinyearCode { get; set; }

        [DisplayName("Financial Component Code")]
        [Required(ErrorMessage = "Financial component is required")]
        public int? FincompCd { get; set; }


        [DisplayName("Financial Component Amount")]
        [Required(ErrorMessage = "Amount is required")]
        public decimal? EnqFinamt { get; set; }


        [DisplayName("Whether Provisional Amount")]
        public bool WhProv { get; set; }

        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }
        public int? EnqtempId { get; set; }
        public virtual SisterConcernDetailsDTO EnqSis { get; set; }
        public virtual FinancialComponentMasterDTO FincompCdNavigation { get; set; }

        public virtual FinancialYearMasterDTO FinyearCodeNavigation { get; set; }
    }
}
