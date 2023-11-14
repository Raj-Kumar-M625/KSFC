using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblDsbStatImp
    {
        public int DsbId { get; set; }
        public long? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }

        public int DsbOffc  { get; set; }
        public int DsbUnit { get; set;}
        public int DsbSno { get; set;}
        public long DsbIno  { get; set; }
        public string DsbImpStat { get; set; }
        public int DsbNamePl { get; set; }
        public string  DsbProgimpBldg { get; set; }
        public string DsbProgimpMc { get; set; }
        public decimal DsbBldgVal { get; set; }
        public decimal DsbMcVal { get; set; }
        public decimal DsbPhyPrg { get; set; }
        public decimal DsbValPrg { get; set; }  
        public string DsbTmcstOvr { get; set; }
        public string DsbRec  { get; set; }
        public DateTime DsbComplDt { get; set; }
        public string DsbBalBldg { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public  bool? IsActive { get; set; }
        public bool? IsDeleted  { get; set; }
        public string UniqueId { get; set; }
    }
}
