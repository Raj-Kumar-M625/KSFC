using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class ApprovalData
    {
        public long Id { get; set; }
        public string ApproveRef { get; set; }
        public decimal ApprovedAmt { get; set; }
        public string ApproveComments { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string ApprovedBy { get; set; }
        public bool IsApproved { get; set; }
        public string ReportType { get; set; }
    }
}
