using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.IDM
{
    public class GuarantorListDTO
    {
        public int IdmGuarDeedId { get; set; }
        public string AppAssetDesc { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2}")]
        public decimal? AppAssetvalue { get; set; }
        public string DeedDesc { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? ExcecutionDate { get; set; }
        public string DeedNo { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2}")]
        public decimal? AppLiabValue { get; set; }
        public string GuarName { get; set; }
        public string GuarMobileNo { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2}")]
        public decimal? AppNw { get; set; }
        public long? LoanAcc { get; set; }
        public long? PromoterCode { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2}")]
        public decimal? ValueAsset { get; set; }
        public int? Action { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? OffcCd { get; set; }
        public int? LoanSub { get; set; }
    }
}
