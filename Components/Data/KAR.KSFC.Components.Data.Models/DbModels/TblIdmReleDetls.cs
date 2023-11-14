using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblIdmReleDetls
    {

        public long ReleId { get; set; }
        public long? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public long? PropNumber { get; set; }
        public decimal? ReleDueAmount { get; set; }
        public int? ReleAtParAmount { get; set; }
       
         public decimal? ReleAtParCharges { get; set; }
        public decimal? ReleBnkChg { get; set; }
        public decimal? ReleUpFrtAmount { get; set; }
        public decimal? ReleDocChg { get; set; }
        public decimal? ReleComChg { get; set; }
        public int? ReleFdAmount { get; set; }
        public int? ReleFdGlcd { get; set; }
        public int? ReleOthAmount { get; set; }
        public int? ReleOthGlcd { get; set; }
        public decimal? ReleAdjAmount { get; set; }
        public decimal? ReleAdjRecSeq { get; set; }
        public int? ReleAddUpFrtAmount { get; set; }
        public int? ReleAddlafdAmount { get; set; }
        public int? ReleSertaxAmount{ get; set; }
        public int? ReleCersai { get; set; }
        public int? ReleSwachcess { get; set; }
        public int? Relekrishikalyancess { get; set; }
        public int? ReleCollGuaranteeFee { get; set; }
        public int? ReleNumber { get; set; }
        public DateTime? ReleDate { get; set; }
        public decimal? ReleAmount { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
        public int? AddlAmt1 { get; set; }
        public int? AddlAmt2 { get; set; }
        public int? AddlAmt3 { get; set; }
        public int? AddlAmt4 { get; set; }
        public int? AddlAmt5 { get; set; }


        public virtual TblIdmDisbProp TblIdmDisbProp { get; set; }

    }
}
