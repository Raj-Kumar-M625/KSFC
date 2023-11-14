using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails
{
    public class PromAssetLiabilityDetailsDTO
    {
        [DisplayName("Total Assets")]
        public string TotalAssets { get; set; }

        [DisplayName("Total Liabilities")]
        public string TotalLiabilities { get; set; }

        [DisplayName("Total Networth")]
        public string TotalNetworth { get; set; }
        public List<PromoterLiabilityDetailsDTO> ListLiabilityDetails { get; set; }
        public List<PromoterAssetsNetWorthDTO> ListAssetDetails { get; set; }
        public virtual PromoterDetailsDTO PromoterDetailsDTO { get; set; }

    }
}
