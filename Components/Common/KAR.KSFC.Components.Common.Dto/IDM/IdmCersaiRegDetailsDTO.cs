using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.IDM
{
    /// <summary>
    ///  Author: Gagana K; Module:Cersai; Date:27/07/2022
    /// </summary>
    public class IdmCersaiRegDetailsDTO
    {
        public int? Action { get; set; }
        public int IdmDsbChargeId { get; set; }
        public long LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public int? SecurityCd { get; set; }
        public long? AssetCd { get; set; }
        public string? CersaiRegNo { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? CersaiRegDate { get; set; }
        public string? CersaiRemarks { get; set; }
        public string? UploadDocument { get; set; }
        public string? ApprovedEmpId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public string ModifiedBy { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? CreatedDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? ModifiedDate { get; set; }
        public decimal AssetVal { get; set; }
        public string AssetDet { get; set; }
        public virtual AssetRefnoDetailsDTO TblAssetRefnoDet { get; set; }
        public string AssetDescription { get; set; }
        public decimal? TotalValue { get; set; }
        public string UniqueId { get; set; }
    }
}
