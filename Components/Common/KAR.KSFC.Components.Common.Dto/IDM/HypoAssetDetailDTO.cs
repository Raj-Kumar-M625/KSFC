using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM
{
    public class HypoAssetDetailDTO
    {

        public int IdmHypothDetId { get; set; }
        public int? LoanSub { get; set; }
        public long? AssetCd { get; set; }
        public long? AssetTypeCd { get; set; }
        public string HypothNo { get; set; }
        public string HypothDesc { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2}")]
        public decimal? AssetValue { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2}")]
        public decimal? AssetValuehypo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? ExecutionDate { get; set; }
        public long? LoanAcc { get; set; }
        public string AssetOthDetails { get; set; }
        public string AssetDetails { get; set; }
        public string AssetTypeDets { get; set; }
        public int? Action { get; set; }
        public byte? OffcCd { get; set; }
        public string? HypothUpload { get; set; }
        public string? ApprovedEmpId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public string ModifiedBy { get; set; }
        public string AssetName { get; set; }
        public string AssetDet { get; set; }
        public decimal HypothValue { get; set; }
        //public int HypothMapId { get; set; }
        //public int UtCd { get; set; }
        //public long AssetItemNo { get; set; }
        //public int AssetQty { get; set; }
        //public int UtSlno { get; set; }


        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? CreatedDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
    }
}
