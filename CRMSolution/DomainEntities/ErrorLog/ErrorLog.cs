using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class ErrorLog
    {
        public int Id { get; set; }
        public string Process { get; set; }
        public string LogText { get; set; }
        public string LogSnip { get; set; }
        public Nullable<System.DateTime> At { get; set; }
    }
}
