using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteDeviceLogDisplayData
    {
        public long Id { get; set; }
        public long BatchId { get; set; }
        public long EmployeeId { get; set; }

        [Display(Name = "At")]
        public System.DateTime TimeStamp { get; set; }

        [Display(Name = "Log Text")]
        public string LogText { get; set; }
    }
}