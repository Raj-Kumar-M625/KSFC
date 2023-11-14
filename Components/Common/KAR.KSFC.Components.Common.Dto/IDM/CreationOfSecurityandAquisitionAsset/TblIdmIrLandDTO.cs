using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM.CreationOfSecurityandAquisitionAsset
{
    public class TblIdmIrLandDTO
    {
        public long IrlId { get; set; }
        public long? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public int? IrlIno { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? IrlIdt { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? IrlRdt { get; set; }
        public int? IrlArea { get; set; }
        public string IrlAreaIn { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2}")]
        public decimal? IrlLandCost { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2}")]
        public decimal? IrlDevCost { get; set; }
        public int? IrlLandTy { get; set; }
        public string IrlRem { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2}")]
        public decimal? IrlSecValue { get; set; }
        public int? IrlRelStat { get; set; }
        public string IrlLandFinance { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
        public int? Action { get; set; }
        public string LandTyDet { get; set; }

        //public virtual OfficeDto OffcCdtab { get; set; }
    }
}
