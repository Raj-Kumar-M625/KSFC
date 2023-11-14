using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails
{
    public class PromoterAllDetailsDTO
    {
        public List<PromoterDetailsDTO> ListPromoters { get; set; }
        public PromAssetLiabilityDetailsDTO PromotersAssetLiabilityDetails { get; set; }

        public List<PromoterNetWorthDetailsDTO> PromoterNetWorthList{ get; set; }
    }
}
