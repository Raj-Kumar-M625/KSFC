using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class AppVersionModel
    {
        public long Id { get; set; }

        [Display(Name = "App Version")]
        [Required]
        [MaxLength(10, ErrorMessage = "Version Number can be upto 10 char")]
        public string Version { get; set; }

        [MaxLength(100, ErrorMessage = "Comment can be upto 100 char")]
        public string Comment { get; set; }

        [Display(Name = "Start (yyyy-mm-dd)")]
        [Required]
        public System.DateTime EffectiveDate { get; set; }

        [Display(Name = "End (yyyy-mm-dd)")]
        [Required]
        public System.DateTime ExpiryDate { get; set; }
    }
}
