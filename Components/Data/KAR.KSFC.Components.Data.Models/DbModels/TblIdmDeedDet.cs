using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblIdmDeedDet
    {
        public string ApprovedEmpId { get; set; }
        public string DeedDesc { get; set; }
        public string DeedNo { get; set; }
        public string DeedUpload {get; set; }
        public DateTime? ExecutionDate { get; set; }
        public int IdmDeedDetId { get; set; }
        public long? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }

        
        public int? SecurityCd { get; set; }
        public decimal? SecurityValue { get; set; }
        public int? SubregistrarCd { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        //public int UtCd { get; set; }   
        public string PjSecNam { get; set; }
        public string PjSecDets { get; set; }
        //public int PjsecDetsCd { get; set; }    
        //public int PjSecRel { get; set; }
        //public int UtSlno { get; set; }

       

        public virtual TblSecurityRefnoMast TblSecurityRefnoMast { get; set; }
    }
}
