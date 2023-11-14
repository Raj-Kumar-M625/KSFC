using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class CRMUsersFilter
    {
        public string Name { get; set; }
        public string EmployeeCode { get; set; }
        public string IMEI { get; set; }
        public bool IsActiveInSap { get; set; }
        public bool OnPhone { get; set; }
        public bool ExecAppAccess { get; set; }
        public bool OnWebPortal { get; set; }
        public bool IsEmployeeActive { get; set; }
    }
}