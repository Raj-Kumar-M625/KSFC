using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM.UnitDetails
{
    public class TblIdmFurnDTO
    {
        public long PjfRowId { get; set; }
        public int UtCd { get; set; }
        public int UtSlno { get; set; }
        public int LoanAcc { get; set; }
        public byte PjfOffc { get; set; }
        public int PjfUnit { get; set; }
        public int PjfSno { get; set; }
        public long PjfItemNo { get; set; }
        public string PjfDets { get; set; }
        public decimal PjfCst { get; set; }
        public int PjfQty { get; set; }
        public decimal PjfTax { get; set; }
        public decimal PjfTotcst { get; set; }
        public decimal PjfCpct { get; set; }
        public decimal PjfRcpct { get; set; }
        public decimal PjfContingency { get; set; }
        public decimal PjfCamt { get; set; }
        public decimal PjfRcamt { get; set; }
        public string PjfSup { get; set; }
        public string PjfSupadr { get; set; }
        public bool PjfReg { get; set; }
        public string PjfInvNo { get; set; }
        public DateTime PjfInvDt { get; set; }
        public int PjfCletStat { get; set; }
        public int PjfStat { get; set; }
        public int PjfDelry { get; set; }
        public int PjfTotCstr { get; set; }
        public bool PjfNonSsi { get; set; }
        public int PjfAqrdStat { get; set; }
        public bool PjfExisting { get; set; }
        public string PjfNotes { get; set; }
        public string PjfUpload { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string UniqueId { get; set; }


    }
}
