using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.Common
{
    public class DropDownDTO
    {
        [DisplayName("Value")]
        [Required(ErrorMessage = "The Value is required")]
        public int Value { get; set; }

        [DisplayName("Text")]
        [Required(ErrorMessage = "The Text is required")]
        public string Text { get; set; }
    }
}
