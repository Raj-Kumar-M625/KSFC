using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblIdmDspInsp
    {
        public long DinRowID { get; set; }
        public long? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public int DinNo { get; set; }
        public DateTime? DinDt { get; set; }
        public string DinTeam { get; set; }   
        public int DinDept { get; set; }
        public DateTime? DinRdt { get; set; }
        public int? DinSeccd { get; set; }
        public Decimal DinSecAmt { get; set; }
        public int? DinSecrt { get; set; }
        public int? Dinland { get; set; }
        public  int? DinLandArea { get; set; }
        public int? DinLandDev { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
