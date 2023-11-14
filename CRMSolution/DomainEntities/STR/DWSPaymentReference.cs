using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class DWSPaymentReference
    {
        public long Id { get; set; }
        public string PaymentReference { get; set; }
        public string Comments { get; set; }
        public decimal TotalNetPayable { get; set; }
        public long DWSCount { get; set; }
        public string DWSNumbers { get; set; }

        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string AccountAddress { get; set; }
        public string AccountEmail { get; set; }
        public string PaymentType { get; set; }
        public string SenderInfo { get; set; }

        public DateTime LocalTimeStamp { get; set; }

        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateUpdated { get; set; }

        public string CurrentUser { get; set; }
    }
}
