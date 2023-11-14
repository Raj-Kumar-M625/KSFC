using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class EditModelOrderItem
    {
        public long Id { get; set; }
        public long OrderId { get; set; }

        [Display(Name = "S.No.")]
        public int SequenceNumber { get; set; }

        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Display(Name = "Product Code")]
        public string ProductCode { get; set; }

        [Display(Name = "Revised Quantity")]
        public int RevisedQuantity { get; set; }
    }
}