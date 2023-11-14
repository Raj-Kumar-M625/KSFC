using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.Enquiry
{
    public class PromoterAllDetailsDTO
    {
        public List<PromoterDetailsDTO> ListPromoters { get; set; }
        public AssetLiabilityDetailsDTO PromotersAssetLiabilityDetails { get; set; }
    }
}
