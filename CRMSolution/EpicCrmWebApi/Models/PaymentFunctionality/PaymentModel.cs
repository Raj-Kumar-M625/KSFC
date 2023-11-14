using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class PaymentModel
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }

        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        public long DayId { get; set; }
        [Display(Name = "Customer Code")]
        public string CustomerCode { get; set; }

        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        [Display(Name = "Type")]
        public string PaymentType { get; set; }

        [Display(Name ="Date")]
        public System.DateTime PaymentDate { get; set; }

        [Display(Name = "Amount (Rs.)")]
        public decimal TotalAmount { get; set; }

        public string Comment { get; set; }

        [Display(Name = "")]
        public int ImageCount { get; set; }

        [Display(Name = "Created on (UTC)")]
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateUpdated { get; set; }
        public long SqlitePaymentId { get; set; }

        [Display(Name = "Approved")]
        public bool IsApproved { get; set; }

        [Display(Name = "Employee Phone")]
        public string Phone { get; set; }
    }
}