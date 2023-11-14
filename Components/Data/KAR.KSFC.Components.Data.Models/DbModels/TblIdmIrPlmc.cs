using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public class TblIdmIrPlmc
    {
        public long IrPlmcId { get; set; }
        public long? LoanAcc { get; set;}
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public long? IrPlmcIno { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? IrPlmcIdt { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? IrPlmcRdt { get; set; }
        public long? IrPlmcItem { get; set; }
        public decimal? IrPlmcAmt { get; set; }
        public int? IrPlmcNo { get; set; }
        public int? IrPlmcAcqrdStatus { get; set; }
        public int? IrPlmcFlg { get; set; }
        public decimal? IrPlmcSecAmt { get; set; }
        public int? IrPlmcAcqrdIndicator { get; set; }
        public int? IrPlmcRelseStat { get; set; }
        public decimal? IrPlmcAamt { get; set; }
        public string IrPlmcItemDets { get; set; }
        public string IrPlmcSupplier { get; set; }
        public int? IrPlmcTotalRelease { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
    }
}
