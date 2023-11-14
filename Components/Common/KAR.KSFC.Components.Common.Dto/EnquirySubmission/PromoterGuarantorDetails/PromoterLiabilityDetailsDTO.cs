using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails
{
    public class PromoterLiabilityDetailsDTO
    {
        [DisplayName("PromoterLiability Id")]
        public int? EnqPromliabId { get; set; }

        [DisplayName("Promoter Code")]
        
        public long? PromCode { get; set; }


        [DisplayName("Enquiry Id")]
        public int EnqtempId { get; set; }

        [DisplayName("Promoter Code")]
        [Required(ErrorMessage = "Promotor is required")]
        public long PromoterCode { get; set; }

        [DisplayName("PromoterLiability Description")]
        [Required(ErrorMessage = "Description is required")]
        public string EnqLiabDesc { get; set; }

        [DisplayName("PromoterLiability Value")]
        [Required(ErrorMessage = "Value is required")]
        public decimal? EnqLiabValue { get; set; }

        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }
        public int? EnqPromId { get; set; }
        public virtual PromoterMasterDTO PromoterMasterDTO { get; set; }
        public virtual PromoterNetWorthDetailsDTO PromoterNetWorthDetailsDTO { get; set; }

    }
}
