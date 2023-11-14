using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM
{
    public class GuarantorAssetDetailDTO
    {
        public int AppGuarassetId { get; set; }
        public long? PromoterCode { get; set; }
        public long? EgNo { get; set; }
        public int? OffcCd { get; set; }
        public int? UtCd { get; set; }
        public int? AssetcatCd { get; set; }
        public int? AssettypeCd { get; set; }
        public string LandType { get; set; }
        public string AppAssetSiteNo { get; set; }
        public string AppAssetAddr { get; set; }
        public string AppAssetDim { get; set; }
        public decimal? AppAssetArea { get; set; }
        public string? AppAssetDesc { get; set; }
        public decimal? AppAssetvalue { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public virtual IdmGuarantorDeedDetailsDTO TblIdmGuarDeedDet { get; set; }
    }
}
