using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class ModelReturns
    {
        [Display(Name = "Id")]
        public long Id { get; set; }

        public long EmployeeId { get; set; }

        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }
        public long DayId { get; set; }

        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        [Display(Name = "Customer Code")]
        public string CustomerCode { get; set; }

        [Display(Name = "Date")]
        public DateTime ReturnsDate { get; set; }
        //public string ReturnsDateAsString => String.Format("{0:ddd yyyy-MM-dd}", ReturnsDate);

        [Display(Name = "Amount (Rs.)")]
        public decimal TotalAmount { get; set; }

        public long ItemsCount { get; set; }

        [Display(Name = "Reference No.")]
        public string ReferenceNumber { get; set; }
        public string Comments { get; set; }

        [Display(Name = "Approved")]
        public bool IsApproved { get; set; }

        [Display(Name = "Employee Phone")]
        public string Phone { get; set; }
    }
}