using BusinessLayer;
using CRMUtilities;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class StockLedgerModel : StockTagModel
    {
        public long Id { get; set; }
        public long ItemMasterId { get; set; }

        [Display(Name = "Date")]
        public System.DateTime TransactionDate { get; set; }

        [Display(Name = "Ref. #")]
        public string ReferenceNo { get; set; }

        [Display(Name = "Line #")]
        public int LineNumber { get; set; }

        [Display(Name = "Issue Qty.")]
        public int IssueQuantity { get; set; }

        [Display(Name = "Receive Qty.")]
        public int ReceiveQuantity { get; set; }

        [Display(Name = "Item")]
        public string ItemCode { get; set; }

        [Display(Name = "Item")]
        public string ItemDesc { get; set; }

        [Display(Name = "Item Type")]
        public string Category { get; set; }

        [Display(Name = "UOM")]
        public string Unit { get; set; }
    }
}