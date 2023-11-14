using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public class IdmPromAssetDet
    {
        public int IdmPromassetId { get; set; }
        public long? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public int UtCd { get; set; }
        public long PromoterCode { get; set; }
        public int AssetCatCD { get; set; }
        public int AssetTypeCD { get; set; }
        public int? LandType { get; set; }
        public string? IdmAssetSiteno { get; set; }
        public string? IdmAssetaddr { get; set; }
        public string? IdmAssetDim { get; set; }
        public string? IdmAssetArea { get; set; }
        public string? IdmAssetDesc { get; set; }
        public decimal? IdmAssetValue { get; set; }
        public string UniqueId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblAssettypeCdtab TblAssettypeCdtabs { get; set; }

        public virtual TblAssetcatCdtab AssetcatCdNavigation { get; set; }

        public virtual TblPromCdtab TblPromCdtab { get; set; }

    }
}
