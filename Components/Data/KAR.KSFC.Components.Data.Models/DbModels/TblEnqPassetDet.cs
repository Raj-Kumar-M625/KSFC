using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblEnqPassetDet
    {
        public int EnqPromassetId { get; set; }
        public int? EnqtempId { get; set; }
        public long PromoterCode { get; set; }
        public int AssetcatCd { get; set; }
        public int AssettypeCd { get; set; }
        public string EnqAssetDesc { get; set; }
        public decimal? EnqAssetValue { get; set; }
        public string EnqAssetSiteno { get; set; }
        public string EnqAssetAddr { get; set; }
        public string EnqAssetDim { get; set; }
        public decimal? EnqAssetArea { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual TblAssetcatCdtab AssetcatCdNavigation { get; set; }
        public virtual TblAssettypeCdtab AssettypeCdNavigation { get; set; }
        public virtual TblEnqTemptab Enqtemp { get; set; }
        public virtual TblPromCdtab PromoterCodeNavigation { get; set; }
    }
}
