using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM.InspectionOfUnit
{
    public class IdmDchgWorkingCapitalDTO
    {
        public long DcwcRowId { get; set; }
        public long LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public int? Action { get; set; }    
        public byte? OffcCd { get; set; }
        public string DcwcObank { get; set; }
        public string DcwcNbank { get; set; }
        public string DcwcOnoc { get; set; }
        public DateTime? DcwcOnocdt { get; set; }
        public DateTime? DcwcSandt { get; set; }
        public decimal? DcwcAmount { get; set; }
        public string DcwcRem { get; set; }
        public DateTime? DcwcMemdt { get; set; }
        public int? DcwcIno { get; set; }
        public string DcwcNbnkAdr1 { get; set; }
        public string DcwcNbnkAdr2 { get; set; }
        public string DcwcNbnkAdr3 { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
