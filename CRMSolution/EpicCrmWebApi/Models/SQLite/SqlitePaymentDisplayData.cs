using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqlitePaymentDisplayData
    {
        public long Id { get; set; }
        public long BatchId { get; set; }
        public long EmployeeId { get; set; }

        [Display(Name = "Date")]
        public System.DateTime PaymentDate { get; set; }
        //public string PaymentDateAsString => this.PaymentDate.ToString("yyyy-MM-dd");

        [Display(Name = "Customer")]
        public string CustomerCode { get; set; }

        [Display(Name = "Type")]
        public string PaymentType { get; set; }

        [Display(Name = "Amount")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "Images?")]
        public int ImageCount { get; set; }

        public string Comment { get; set; }

        [Display(Name = "Processed?")]
        public bool IsProcessed { get; set; }

        [Display(Name = "Payment Id")]
        public long PaymentId { get; set; }

        [Display(Name = "Date Created (UTC)")]
        public System.DateTime DateCreated { get; set; }

        public System.DateTime DateUpdated { get; set; }

        [Display(Name = "Phone Db Id")]
        public string PhoneDbId { get; set; }
        [Display(Name = "Phone Activity Id")]
        public string PhoneActivityId { get; set; }
    }
}