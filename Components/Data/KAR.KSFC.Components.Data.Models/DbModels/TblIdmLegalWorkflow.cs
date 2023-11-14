using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblIdmLegalWorkflow
    {
        public int IdmLegalWorkflowId { get; set; }
        public long? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public string EmpIdFrom { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public DateTime? SendDate { get; set; }
        public string EmpIdTo { get; set; }
        public int? WorkFlowStatusID { get; set; }
        public string IdmWfNoting { get; set; }
        public string IdmWfRemarks { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public virtual TblAppLoanMast TblAppLoanMast { get; set; } //by gowtham s on 2/8/22

        public virtual OffcCdtab OffcCdtab { get; set; }  //by gowtham s on 2/8/22
        //  public virtual TblEmpchairDet TblEmpchairDets { get; set; } //by gowtham s on 3/8/22
    }
}
