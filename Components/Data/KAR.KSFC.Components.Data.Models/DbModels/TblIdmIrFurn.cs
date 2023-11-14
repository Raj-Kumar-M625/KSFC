using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public class TblIdmIrFurn
    {
        /*
            This class is used to define the fields used to handle the values of the Furniture
            Acquisition details of the IDM module.
            
            Note:
            1. The table name must match the Table in the database but defined here in camel case.
            2. The field datatype must match the database table column datatypes.
         */

        public long IrfId { get; set; }
        public long? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public DateTime? IrfIDT { get; set; }
        public DateTime? IrfRDT { get; set; }
        public long? IrfItem { get; set; }
        public decimal? IrfAmt { get; set; }
        public int? IrfNo { get; set; }
        public long? IrfIno { get; set; }
        public int? IrfAqrdStat { get; set; }
        public int? IrfSecAmt { get; set; }
        public int? IrfRelStat { get; set; }
        public decimal? IrfAamt { get; set; }
        public int? IrfTotalRelease { get; set; }
        public string? IrfItemDets { get; set; }
        public string? IrfSupplier { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        // Define the keys
        //public virtual TblAppLoanMast LoanAccount { get; set; }
        //public virtual OffcCdtab OffCd { get; set; }
    }
}
