using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.Enums
{
    public static class RolesEnum
    {
        public const string Customer = "Customer";
        public const string Employee = "Employee";
        public const string CE = "Employee,Customer";
        public const string AccountingOfficer = "Accounting Officer";

    }
    public static class SwitchedModuleEnum
    {
        public const string Admin = "Admin";
        public const string EG = "EG";
        public const string LegalOfficer = "Legal Officer";
    }
}
