using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DomainEntities
{
    public class GeoTaggingReport : OfficeHierarchy
    {
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DivisionName { get; set; }        
        public string BranchName { get; set; }
        public string ZoneName { get; set; }
        public string CustomerName { get; set; }
        public bool GeoTagStatus { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerType { get; set; }
        public bool EmployeeStatus { get; set; }
        public bool EmployeeStatusInSp { get; set; }
    }

}
