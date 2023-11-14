using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.Enquiry
{
    public class AssetLiabilityDetailsDTO
    {
        public string TotalAssets { get; set; }
        public string TotalLiabilities { get; set; }
        public string TotalNetworth { get; set; }
        public List<LiabilityDetailsDTO> ListLiabilityDetails { get; set; }
        public List<AssetDetailsDTO> ListAssetDetails { get; set; }
    }
}
