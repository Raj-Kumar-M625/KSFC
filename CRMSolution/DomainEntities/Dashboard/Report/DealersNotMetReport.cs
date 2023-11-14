using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;

namespace DomainEntities
{
    //Author:Venkatesh, Purpose: Get Dealers Not Met Report on: 2022/11/09
    public class DealersNotMetReport : OfficeHierarchy
    {
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string LastActivity { get; set; }
        public new string HQCode { get; set; }
        public new string HQName { get; set; }
        public string ContactNumber { get; set; }
        public bool IsActive { get; set; }



    }
}