using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteOrderDisplayData
    {
        public long Id { get; set; }
        public long BatchId { get; set; }
        public long EmployeeId { get; set; }

        [Display(Name = "Type")]
        public string OrderType { get; set; }

        [Display(Name = "Order Amount")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "GST Amount")]
        public decimal TotalGST { get; set; }

        [Display(Name = "Net Amount")]
        public decimal NetAmount { get; set; }

        [Display(Name = "Customer")]
        public string CustomerCode { get; set; }
        
        [Display(Name ="Order Date")]
        public System.DateTime OrderDate { get; set; }
        //public string OrderDateAsString => String.Format("{0:yyyy-MM-dd}", OrderDate);

        [Display(Name = "Item Count")]
        public long ItemCount { get; set; }

        [Display(Name = "Date Created (UTC)")]
        public System.DateTime DateCreated { get; set; }
        //public string DateCreatedAsString => String.Format("{0:yyyy-MM-dd hh:ss:ss tt}", DateCreated);

        public System.DateTime DateUpdated { get; set; }

        [Display(Name = "Processed?")]
        public bool IsProcessed { get; set; }
        [Display(Name = "Order Id")]
        public long OrderId { get; set; }

        [Display(Name = "Phone Db Id")]
        public string PhoneDbId { get; set; }
        [Display(Name = "Phone Activity Id")]
        public string PhoneActivityId { get; set; }


        [Display(Name = "Max Discount %")]
        public decimal MaxDiscountPercentage { get; set; }

        public string DiscountType { get; set; }

        [Display(Name = "Applied Discount %")]
        public decimal AppliedDiscountPercentage { get; set; }

        public int ImageCount { get; set; }
    }
}