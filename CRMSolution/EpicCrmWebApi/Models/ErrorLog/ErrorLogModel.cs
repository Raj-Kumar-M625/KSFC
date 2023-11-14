using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class ErrorLogModel
    {
        public int Id { get; set; }
        public string Process { get; set; }
        public string LogText { get; set; }
        public string LogSnip { get; set; }
        [Display(Name = "IST Time")]
        public DateTime At { get; set; }
    }
}