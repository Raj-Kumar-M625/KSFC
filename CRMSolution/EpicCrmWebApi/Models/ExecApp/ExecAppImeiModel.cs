using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class ExecAppImeiModel
    {
        public long Id { get; set; }

        [Display(Name = "IMEI")]
        [Required]
        [MaxLength(50, ErrorMessage = "IMEI can be upto 50 char")]
        public string IMEINumber { get; set; }

        [MaxLength(100, ErrorMessage = "Comment can be upto 100 char")]
        public string Comment { get; set; }

        [Display(Name ="Start (yyyy-mm-dd)")]
        [Required]
        public System.DateTime EffectiveDate { get; set; }

        [Display(Name ="End (yyyy-mm-dd)")]
        [Required]
        public System.DateTime ExpiryDate { get; set; }

        [Display(Name = "Support Person")]
        public bool IsSupportPerson { get; set; }

        [Display(Name = "Enable Log")]
        public bool EnableLog { get; set; }

        [Display(Name = "Detail Level (1=Zone/2/3/4=HQ)")]
        public int ExecAppDetailLevel { get; set; }
    }
}