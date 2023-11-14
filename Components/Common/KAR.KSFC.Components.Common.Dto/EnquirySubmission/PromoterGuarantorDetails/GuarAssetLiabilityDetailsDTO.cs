using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails
{
    public class GuarAssetLiabilityDetailsDTO
    {
        [DisplayName("Total Assets")]
        [Required(ErrorMessage = "The Total Assets is required")]
        public string TotalAssets { get; set; }
 
         [DisplayName("Total Liabilities")]
        [Required(ErrorMessage = "The Total Liabilities is required")]
        public string TotalLiabilities { get; set; }
 
         [DisplayName("Total Networth")]
        [Required(ErrorMessage = "The Total Networth is required")]
        public string TotalNetworth { get; set; }

        public List<GuarantorLiabilityDetailsDTO> ListLiabilityDetails { get; set; }
        public List<GuarantorAssetsNetWorthDTO> ListAssetDetails { get; set; }
    }
}
