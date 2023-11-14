using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.Enquiry
{
    public class GuarantorAllDetailsDTO
    {
        public List<GuarantorDetailsDTO> ListGuarantor { get; set; }
        public AssetLiabilityDetailsDTO GuarantorAssetLiabilityDetails { get; set; }
    }
}
