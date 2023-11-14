using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    /// <summary>
    ///  Author: Gagana K; Module:Loan Alloaction; Date:28/09/2022
    /// </summary>
    public partial class TblIdmDhcgAllc
    {
        public long DcalcId { get; set; }

        public long? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public int? DcalcCd { get; set; }
        public decimal? DcalcAmt { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? DcalcRqdt { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? DcalcApdt { get; set; }
        public int? DcalcApau { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? DcalcComdt { get; set; }
        public decimal? DcalcAmtRevised { get; set; }
        public string? DcalcDetails { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? UniqueId { get; set; }
    }
}
