using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteReturnOrderDisplayData
    {
        public long Id { get; set; }
        public long BatchId { get; set; }
        public long EmployeeId { get; set; }

        [Display(Name = "Return Amount")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "Customer")]
        public string CustomerCode { get; set; }
        
        [Display(Name ="Return Date")]
        public System.DateTime ReturnOrderDate { get; set; }
        //public string ReturnOrderDateAsString => String.Format("{0:yyyy-MM-dd}", ReturnOrderDate);

        [Display(Name = "Item Count")]
        public long ItemCount { get; set; }

        [Display(Name = "Date Created (UTC)")]
        public System.DateTime DateCreated { get; set; }
        //public string DateCreatedAsString => String.Format("{0:yyyy-MM-dd hh:ss:ss tt}", DateCreated);

        public System.DateTime DateUpdated { get; set; }

        [Display(Name = "Processed?")]
        public bool IsProcessed { get; set; }
        [Display(Name = "ReturnOrder Id")]
        public long ReturnOrderId { get; set; }

        public string Comment { get; set; }

        [Display(Name = "Reference")]
        public string ReferenceNum { get; set; }

        [Display(Name = "Phone Db Id")]
        public string PhoneDbId { get; set; }
        [Display(Name = "Phone Activity Id")]
        public string PhoneActivityId { get; set; }
    }
}