using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM
{
    /// <summary>
    ///  Author: Gagana K; Module: Hypothecation; Date:21/07/2022
    /// </summary>
    public class IdmHypotheDetailsDTO
    {
        public int IdmHypothDetId { get; set; }
        public long? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public long? AssetCd { get; set; }
        public string? HypothNo { get; set; }
        public string? HypothDesc { get; set; }
        public decimal? AssetValue { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? ExecutionDate { get; set; }
        public string? HypothUpload { get; set; }
        public string? ApprovedEmpId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string AssetName { get; set; }
        public string AssetDet { get; set; }
        //public int UtCd { get; set; }
        //public long AssetItemNo { get; set; }
        //public int AssetQty { get; set; }
        //public int UtSlno { get; set; }
        public string UniqueId { get; set; }
        public int? Action { get; set; }
        public decimal HypothValue { get; set; }
        //public int HypothMapId { get; set; }

        public virtual AssetRefnoDetailsDTO TblAssetRefnoDet { get; set; }
        //public virtual AssetTypeDetailsDTO TblAssettypeCdtab { get; set; }
    }
}
