using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM.UnitDetails
{
    public class TblIdmProjImpMcDTO 
    {
        public long PjImcRowId { get; set; }
        public int UtCd { get; set; }
        public int UtSlno { get; set; }
        public int LoanAcc { get; set; }
        public int LoanSub { get; set; }
        public byte PjImcOffc { get; set; }
        public int PjImcUnit { get; set; }
        public int PjImcSno { get; set; }
        public long PjImcItemNo { get; set; }
        public string PjImcEntry { get; set; }
        public string PjImcEntryI { get; set; }
        public string PjImcCrncy { get; set; }
        public decimal PjImcExrd { get; set; }
        public decimal PjImcCif { get; set; }
        public decimal PjImcCduty { get; set; }
        public decimal PjImcTotCost { get; set; }
        public int PjImcStat { get; set; }
        public string PjImcDets { get; set; }
        public string PjImcSup { get; set; }
        public int PjImcQty { get; set; }
        public int PjImcDelry { get; set; }
        public string PjImcSupadr { get; set; }
        public decimal PjImcCpct { get; set; }
        public decimal PjImcCamt { get; set; }
        public bool PjImcNonssi { get; set; }
        public decimal PjImcRcif { get; set; }
        public string PjImcRcrncy { get; set; }
        public decimal PjImcRexch { get; set; }
        public decimal PjImcRcDuty { get; set; }
        public decimal PjImcRtotCost { get; set; }
        public decimal PjImcRcpct { get; set; }
        public decimal PjImcRcamt { get; set; }
        public decimal PjImcBasicCost { get; set; }
        public bool PjImcSupRegd { get; set; }
        public string PjImcBoeno { get; set; }
        public bool PjImcExisting { get; set; }
        public bool PjImcSubvention { get; set; }
        public decimal PjImcSubvCost { get; set; }
        public int PjImcDeprMethod { get; set; }
        public decimal PjImcDeprValue { get; set; }
        public bool PjImcDirectProd { get; set; }
        public decimal PjImcContingency { get; set; }
        public string PjImcSubvNotes { get; set; }
        public string PjImcNotes { get; set; }
        public string PjImcUpload { get; set; }
        public decimal PjImcGst { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string UniqueId { get; set; }
        public decimal PjImcTotalBasicCost { get; set; }
        public decimal PjImcOtherExp { get; set; }

    }
}
