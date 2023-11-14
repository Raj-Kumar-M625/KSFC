using KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM.UnitDetails
{
    public class IdmPromAddressDTO
    {
        public int IdmPromadrId { get; set; }
        public long? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public int? UtCd { get; set; }
        public long PromoterCode { get; set; }
        public string PromoterName { get; set; }
        public string PromAddress { get; set; }
        public int? PromStateCd { get; set; }
        public int? PromDistrictCd { get; set; }
        public int? PromPincode { get; set; }
        public bool AdrPermanent { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
        public int? Action { get; set; }
        public virtual PincodeDistrictCdtabDTO TblPincodeDistrictCdtab { get; set; }
        public virtual PincodeStateCdtabDTO TblPincodeStateCdtab { get; set; }

    }
}
