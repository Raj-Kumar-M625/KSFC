using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    /// <summary>
    ///  Author: Gagana K; Module: Hypothecation; Date:21/07/2022
    /// </summary>
    public partial class TblAssetRefnoDet
    {

        public TblAssetRefnoDet()
        {
            TblIdmHypothDet = new HashSet<TblIdmHypothDet>();
        }
        public long AssetRefnoMastId { get; set; }
        public long AssetCd { get; set; }
        public long LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public int? AssetcatCd { get; set; }
        public int? AssetypeCd { get; set; }
        public string? AssetDetails { get; set; }
        public decimal? AssetValue { get; set; }
        public string? AssetOthDetails { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
        public bool? WhHyp { get; set; }
        public bool? WhCersai { get; set; }
        public virtual ICollection<TblIdmHypothDet> TblIdmHypothDet { get; set; }
        public virtual ICollection<TblIdmCersaiRegistration> TblIdmCersaiRegistration { get; set; }
        public virtual TblAssettypeCdtab TblAssettypeCdtab { get; set; }
        public virtual TblAssetcatCdtab TblAssetcatCdtab { get; set; }

    }
}
