using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblIdmProjBldg
    {
        public long PjBdgRowId { get; set; }
        public int UtCd { get; set; }
        public int UtSlno { get; set; }
        public long LoanAcc { get; set; }
        public int LoanSub { get; set; }
        public byte PjBdgOffc { get; set; }
        public int PjBdgUnit { get; set; }
        public int PjBdgSno { get; set; }
        public long PjBdgItemNo  { get; set; }
        public string PjBdgdets { get; set; }
        public string PjBdgRoof { get; set; }
        public decimal PjBdgPlnthO { get; set; }
        public decimal PjBdgPlnthR { get; set; }
        public decimal PjBdgUcostO { get; set; }
        public decimal PjBdgUcostR { get; set; }
        public decimal PjBdgTcostO { get; set; }
        public decimal PjBdgTcostR { get; set; }
        public bool SubVentionBdg  { get; set; }
        public decimal ApbsTotCst { get; set; }
        public bool ExistingBdg { get; set; }
        public decimal Contingency { get; set; }
        public string PjBdgNote { get; set; }
        public string PjBdgSubvNote { get; set; }
        public string PjBdgUpload { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string UniqueId { get; set; }
        public decimal PjBdgConsValue { get; set; }
        public decimal PjBdgDiffValue { get; set; }
        public int PjBdgDeprMethod { get; set; }
        public decimal PjBdgDeprValue { get; set; }
        public decimal PjBdgSubvCost { get; set; }


    }
}
