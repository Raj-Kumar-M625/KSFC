using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqlitePayment : SqliteBase
    {
        public string Id { get; set; }  // ID of the record in Sqlite table
        public string CustomerCode { get; set; }  // Customer to whom this payment belongs
        public DateTime TimeStamp { get; set; }  // only date part is relevant - as payment date
        public string PaymentMode { get; set; }  // Cash / Cheque etc.
        public decimal TotalAmount { get; set; } // total payment amount 
        public string Comment { get; set; }
        public string ActivityId { get; set; }
    }
}