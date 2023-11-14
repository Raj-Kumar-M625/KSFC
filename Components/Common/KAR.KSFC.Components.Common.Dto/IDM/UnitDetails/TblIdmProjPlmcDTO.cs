using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM.UnitDetails
{
    public class TblIdmProjPlmcDTO
    {
        public long PjPlmcRowId { get; set; }
        public int UtCd { get; set; }
        public int UtSlno { get; set; }
        public long LoanAcc { get; set; }
        public int LoanSub { get; set; }
        public byte PjPlmcOffc { get; set; }
        public int PjPlmcUnit { get; set; }
        public int PjPlmcSno { get; set; }
        public string PjPlmcDets { get; set; }
        public string PjPlmcSup { get; set; }
        public string PjPlmcSupadr { get; set; }
        public string PjPlmcInvNo { get; set; }
        public DateTime PjPlmcInvDt { get; set; }
        public int PjplmcCletStat { get; set; }
        public int PjplmcReg { get; set; }
        public int PjplmcQty { get; set; }
        public int PjplmcStat { get; set; }
        public int PjplmcDelry { get; set; }
        public decimal PjplmcCst { get; set; }
        public decimal PjplmcTax { get; set; }
        public decimal PjplmcTotCst { get; set; }
        public decimal PjplmcTotCstr { get; set; }
        public long PjplmcItemNo { get; set; }
        public bool PjplmcNonSsi { get; set; }
        public int PjplmcQtyr { get; set; }
        public decimal PjplmcContingency { get; set; }
        public bool PjplmcExisting { get; set; }
        public bool SubventionPlmc { get; set; }
        public decimal PjplmcSubvCost { get; set; }
        public int PjplmcDeprmethod { get; set; }
        public decimal PjplmcDeprvalue { get; set; }
        public bool PjplmcDirectProd { get; set; }
        public string PjplmcSubvNotes { get; set; }
        public string PjplmcNotes { get; set; }
        public string PjplmcUpload { get; set; }
        public string PjPlmcGst { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool UniqueId { get; set; }

    }
}
