using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails
{
    public class GuarantorAllDetailsDTO
    {
        public List<GuarantorDetailsDTO> ListGuarantor { get; set; }
        public GuarAssetLiabilityDetailsDTO GuarantorAssetLiabilityDetails { get; set; }
        public List<GuarantorNetWorthDetailsDTO> GuarantorNetWorthList { get; set; }
        
    }
}
