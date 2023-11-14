using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM.UnitDetails
{
    public class IdmPromAssetDetDTO
    {
        public int IdmPromassetId { get; set; }
        public long? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public int UtCd { get; set; }
        public long PromoterCode { get; set; }
        public string PromoterName { get; set; }
        public int AssetCatCD { get; set; }
        public string AssetcatDets { get; set; }
        public int AssetTypeCD { get; set; }
        public string AssettypeDets { get; set; }
        public int? LandType { get; set; }
        public int? Action { get; set; }
        public string? IdmAssetSiteno { get; set; }
        public string? IdmAssetaddr { get; set; }
        public string? IdmAssetDim { get; set; }
        public string? IdmAssetArea { get; set; }
        public string? IdmAssetDesc { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2}")]
        public decimal? IdmAssetValue { get; set; }
        public string UniqueId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public virtual TblAssetCatCDTabDTO AssetcatCdNavigation { get; set; }
        public virtual AssetTypeDetailsDTO TblAssettypeCdtabs { get; set; } 
       public virtual TblPromcdtabDTO TblPromCdtab { get; set; }


    }
}
