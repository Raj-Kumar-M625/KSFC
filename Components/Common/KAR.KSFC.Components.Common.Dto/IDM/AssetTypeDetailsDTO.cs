using KAR.KSFC.Components.Common.Dto.IDM.UnitDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM
{
    /// <summary>
    ///  Author: Gagana K; Module: Hypothecation-AssetType Lists; Date:21/07/2022
    /// </summary>
    public class AssetTypeDetailsDTO
    {
        public int AssettypeCd { get; set; }
        public string AssettypeDets { get; set; }
        public int AssetcatCd { get; set; }
        public decimal? SeqNo { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
        public virtual AssetRefnoDetailsDTO TblAssetRefnoDet { get; set; }
        public virtual IdmHypotheDetailsDTO TblIdmHypothDet { get; set; }

        public virtual TblAssetCatCDTabDTO TblAssetCatCDTab { get; set; }
        public virtual IdmPromAssetDetDTO IdmPromAssetDet { get; set; }

    }
}
