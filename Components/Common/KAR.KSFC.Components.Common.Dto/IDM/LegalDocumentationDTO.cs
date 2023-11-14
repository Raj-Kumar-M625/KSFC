using KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM
{
    public class LegalDocumentationDTO
    {
        public AllDDLListDTO DDLDTO { get; set; }
        public List<IdmSecurityDetailsDTO> SecurityDetails { get; set; }
        public List<HypoAssetDetailDTO> AssetDetails { get; set; }
        public List<IdmSecurityChargeDTO> SecurityChargeDetails { get; set; }
        public List<IdmCersaiRegDetailsDTO> CersaiRegDetails { get; set; }
        public List<IdmGuarantorDeedDetailsDTO> IdmGuarantorDeedDetailsDTO { get; set; }
        public List<LDConditionDetailsDTO> ConditionDetails { get; set; }
        public List<LoanAccountNumberDTO> LoanAccountDetails { get; set; }
        public LDCheckListDTO LDCheckList { get; set; }

    }
}
