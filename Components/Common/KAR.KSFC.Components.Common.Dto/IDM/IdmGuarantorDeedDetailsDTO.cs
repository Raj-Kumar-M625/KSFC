using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.IDM
{

    /// <summary>
    ///  Author: Manoj BJ; Module: Guarantor DEED; Date:01/08/2022
    /// </summary>
    public class IdmGuarantorDeedDetailsDTO
    {
        public int IdmGuarDeedId { get; set; }
        public long? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public long? PromoterCode { get; set; }
        public string? GuarName { get; set; }
        public string? GuarMobileNo { get; set; }
        public int? AppGuarassetId { get; set; }
        public decimal? ValueAsset { get; set; }
        public decimal? ValueLiab { get; set; }
        public decimal? ValueNetworth { get; set; }
        public string DeedNo { get; set; }
        public string DeedDesc { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
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
        public int? Action { get; set; }
        public virtual GuarantorAssetDetailDTO TblAppGuarAssetDet { get; set; }

    }
}
