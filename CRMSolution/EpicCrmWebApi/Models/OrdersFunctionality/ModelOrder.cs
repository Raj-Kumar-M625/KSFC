using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class ModelOrder
    {
        [Display(Name = "Order Number")]
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
        public DateTime OrderDate { get; set; }
        //public string OrderDateAsString => String.Format("{0:ddd yyyy-MM-dd}", OrderDate);

        [Display(Name = "Orig. Amt (Rs.)")]
        public decimal TotalAmount { get; set; }
        public string TotalAmountAsString => String.Format(new CultureInfo("hi-IN"), "{0:#,#.00}", TotalAmount);

        [Display(Name = "Orig. Gst (Rs.)")]
        public decimal TotalGST { get; set; }

        [Display(Name = "Orig. Net (Rs.)")]
        public decimal NetAmount { get; set; }

        [Display(Name = "Revised Amt (Rs.)")]
        public decimal RevisedTotalAmount { get; set; }

        public decimal RevisedTotalGST { get; set; }

        [Display(Name = "Revised Net (Rs.)")]
        public decimal RevisedNetAmount { get; set; }

        public long ItemsCount { get; set; }

        [Display(Name = "Approved")]
        public bool IsApproved { get; set; }

        [Display(Name = "Approved By")]
        public string ApprovedBy { get; set; }


        public string ApproveRef { get; set; }

        [Display(Name = "Approved Amt (Rs.)")]
        public decimal ApprovedAmt { get; set; }
        public string ApproveComments { get; set; }
        public DateTime ApprovedDate { get; set; }

        [Display(Name = "Employee Phone")]
        public string Phone { get; set; }

        [Display(Name = "Discount Type")]
        public string DiscountType { get; set; }

        [Display(Name = "Image Count")]
        public int ImageCount { get; set; }
    }
}