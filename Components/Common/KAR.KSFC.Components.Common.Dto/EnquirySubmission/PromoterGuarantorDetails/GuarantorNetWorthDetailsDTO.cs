using System;
using System.ComponentModel;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails
{
    public class GuarantorNetWorthDetailsDTO
    {

        [DisplayName("Networth Id")]
        public int EnqGuarnwId { get; set; }
 
        [DisplayName("Enquiry Id")]
        public int EnqtempId { get; set; }

        [DisplayName("Promoter Code")]
        public long PromoterCode { get; set; }


        [DisplayName("Immovable Assets")]
        public decimal? GuarImmov { get; set; }

        [DisplayName("Movable Assets")]
        public decimal? GuarMov { get; set; }

        [DisplayName("Liability")]
        public decimal? GuarLiab { get; set; }

        [DisplayName("Total NetWorth")]
        public decimal? GuarNw { get; set; }


        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }
        public virtual GuarantorDetailsDTO GuarantorDetailsDTO { get; set; }
        public virtual GuarantorAssetsNetWorthDTO GuarantorAssetsNetWorthDTO { get; set; }
        public virtual GuarantorLiabilityDetailsDTO GuarantorLiabilityDetailsDTO { get; set; }
    }
}
