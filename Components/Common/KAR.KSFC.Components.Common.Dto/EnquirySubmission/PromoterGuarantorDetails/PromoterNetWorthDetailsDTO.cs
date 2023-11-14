using System;
using System.ComponentModel;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails
{
    public class PromoterNetWorthDetailsDTO
    {
        [DisplayName("Promoter NetWorthId")]
        public int? EnqPromnwId { get; set; }
  
         [DisplayName("TempId")]
         public int EnqtempId { get; set; }
 
         [DisplayName("Promoter Code")]
 
        public long PromoterCode { get; set; }
 
 
         [DisplayName("Immovable Assets")]
        public decimal? EnqImmov { get; set; }
 
 
         [DisplayName("Movable Assets")]
        public decimal? EnqMov { get; set; }
 
 
         [DisplayName("Asset Value")]
        public decimal? EnqPromotertotalAssetValue { get; set; }
 
 
         [DisplayName("Liability")]
        public decimal? EnqLiab { get; set; }
 
 
         [DisplayName("Net Worth")]
        public decimal? EnqNw { get; set; }
 
 
         [DisplayName("Unique Id")]
         public string UniqueId { get; set; }
        public virtual PromoterDetailsDTO PromoterDetailsDTO { get; set; }
        public virtual PromoterAssetsNetWorthDTO PromoterAssetsNetWorthDTO { get; set; }
        public virtual PromoterLiabilityDetailsDTO PromLiabilityDetailsDTO { get; set; }
    }
}
