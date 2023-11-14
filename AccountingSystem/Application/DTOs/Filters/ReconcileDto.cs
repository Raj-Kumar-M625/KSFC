using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Filters
{
    public class ReconcileDto
    {
        public string Date { get; set; }
        public string Status { get; set; }
        public string Bankname { get; set; }
        public decimal PayableAmount { get; set; }
        public decimal PayableMaxAmount { get; set; }
        public string TransactionType { get; set; }
        public string AccountNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }   
        public string Reconcile { get; set; }
    }
}
