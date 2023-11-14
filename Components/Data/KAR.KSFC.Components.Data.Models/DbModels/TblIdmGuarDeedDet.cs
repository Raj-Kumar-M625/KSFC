using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public class TblIdmGuarDeedDet
    {
        public int IdmGuarDeedId { get; set; }
        public long? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public long? PromoterCode { get; set; }
        public int? AppGuarassetId { get; set; }
        public decimal? ValueAsset { get; set; }
        public decimal? ValueLiab { get; set; }
        public decimal? ValueNetWorth { get; set; }
        public string DeedNo { get; set; }
        public string DeedDesc { get; set; }
        public DateTime? ExcecutionDate { get; set; }
        public string DeedUpload { get; set; }
        public string ApprovedEmpId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
        public virtual TblAppGuarAssetDet TblAppGuarAssetDet { get; set; }
      //  public virtual TblPromCdtab TblPromCdtab { get; set; }
    }
}
