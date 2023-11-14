using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class ExpenseApproval
    {
        public long Id { get; set; }
        public long ExpenseId { get; set; }
        public string ApproveLevel { get; set; }
        public System.DateTime ApproveDate { get; set; }
        public string ApproveRef { get; set; }
        public string ApproveNotes { get; set; }
        public decimal ApproveAmount { get; set; }
        public string ApprovedBy { get; set; }
    }
}
