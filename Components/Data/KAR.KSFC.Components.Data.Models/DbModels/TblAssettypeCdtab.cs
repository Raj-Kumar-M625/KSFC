using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblAssettypeCdtab
    {
        public TblAssettypeCdtab()
        {
            TblEnqGassetDets = new HashSet<TblEnqGassetDet>();
            TblEnqPassetDets = new HashSet<TblEnqPassetDet>();
            //TblIdmHypothDet = new HashSet<TblIdmHypothDet>();
        }

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
        public virtual TblAssetcatCdtab AssetcatCdNavigation { get; set; }
        public virtual ICollection<TblEnqGassetDet> TblEnqGassetDets { get; set; }
        public virtual ICollection<TblEnqPassetDet> TblEnqPassetDets { get; set; }
        //public virtual TblIdmHypothDet TblIdmHypothDet { get; set; }
        public virtual TblAssetRefnoDet TblAssetRefnoDet { get; set; }

        public virtual IdmPromAssetDet IdmPromAssetDet { get; set; }
    }
}
