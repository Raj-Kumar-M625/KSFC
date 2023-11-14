using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SqliteDomainPayment
    {
        public string PhoneDbId { get; set; }
        public string CustomerCode { get; set; }  // Customer to whom this payment belongs
        public DateTime TimeStamp { get; set; }  // only date part is relevant - as payment date
        public string PaymentMode { get; set; }  // Cash / Cheque etc.
        public decimal TotalAmount { get; set; } // total payment amount 
        public string Comment { get; set; }
        //public IEnumerable<byte[]> Images { get; set; }  // instrument images
        public IEnumerable<string> Images { get; set; }  // instrument images
        public string ActivityId { get; set; }
    }
}
