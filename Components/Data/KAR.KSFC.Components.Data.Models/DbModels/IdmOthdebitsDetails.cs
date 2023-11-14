using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    /// <summary>
    // Author: Gagana K; Module:EntryOfOtherDebits; Date: 27/10/2022
    /// </summary>
    public partial class IdmOthdebitsDetails
    {
        public long OthdebitDetId { get; set; }
        public long? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public int? DsbOthdebitId { get; set; }
        public decimal? OthdebitAmt { get; set; }
        public decimal? OthdebitGst { get; set; }
        public decimal? OthdebitTaxes { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? OthdebitDuedate { get; set; }
        public decimal? OthdebitTotal { get; set; }
        public string? OthdebitRemarks { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? UniqueId { get; set; }
        public bool? IsSubmitted { get; set; }
    }
}
