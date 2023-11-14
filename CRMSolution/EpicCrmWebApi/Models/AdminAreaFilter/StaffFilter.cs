using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class StaffFilter
    {
        public string EmployeeCode { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Grade { get; set; }
        public int Status { get; set; }
        public int Association { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string Zone { get; set; }
        public string Area { get; set; }
        public string Territory { get; set; }
        public String HQ { get; set; }
    }
}