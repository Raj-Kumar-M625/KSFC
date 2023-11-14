using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public class TblIdmFirstInvestmentClause
    {
        public long DCFICId { get; set; }
        public byte? DCFICOffc { get; set; }
        public long? DCFICLoanACC { get; set; }
        public int? DCFICSno { get; set; }
        public DateTime? DCFICRequestDate { get; set; }
        public DateTime? DCFICApproveDate { get; set; }
        public DateTime? DCFICApproveAUDate { get; set; }
        public decimal? DCFICAmount { get; set; }
        public DateTime? DCFICCommunicationDate { get; set; }
        public decimal? DCFICAmountOriginal { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

    }
}
