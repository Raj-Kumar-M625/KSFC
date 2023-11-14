using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class ModelExpenseApproval
    {
        public long Id { get; set; }
        public long ExpenseId { get; set; }

        [Display(Name = "Level")]
        public string ApproveLevel { get; set; }

        [Display(Name = "Approve Date")]
        public System.DateTime ApproveDate { get; set; }

        [Display(Name = "Approve Ref")]
        public string ApproveRef { get; set; }

        [Display(Name = "Approve Notes")]
        public string ApproveNotes { get; set; }

        [Display(Name = "Approve Amount")]
        public decimal ApproveAmount { get; set; }

        [Display(Name = "Approved by")]
        public string ApprovedBy { get; set; }
    }
}