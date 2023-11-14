using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class ConfigCodeTableModel
    {
        [Display(Name = "Code Id")]
        public long Id { get; set; }

        [Required]
        [Display(Name = "Code Type")]
        public string CodeType { get; set; }

        [Required]
        [Display(Name = "Code Value")]
        public string CodeValue { get; set; }

        [Required]
        [Range(0,100000,ErrorMessage ="Only positive numbers allowed")]
        [Display(Name = "Display Sequence")]
        public int DisplaySequence { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Code Name")]
        public string CodeName { get; set; }

        public int CodeStatus { get; set; }

        [Display(Name = "Tenant Id")]
        public long TenantId { get; set; }

        [Display(Name = "Customer Type")]
        public string ddCodeName { get; set; }

        public List<string> UniqueCodeTypes { get; set; }
    }
}