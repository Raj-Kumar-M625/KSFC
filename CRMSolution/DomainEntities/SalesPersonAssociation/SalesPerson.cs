using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SalesPerson
    {
        public string SalesPersonName { get; set; }
        public string StaffCode { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }

        public bool OverridePrivateVehicleRatePerKM { get; set; }
        public decimal TwoWheelerRatePerKM { get; set; }
        public decimal FourWheelerRatePerKM { get; set; }
    }

    public class SalesPersonEx : SalesPerson
    {
        public string AssociationType { get; set; }  // e.g. AreaOffice
        public string AssociationCode { get; set; }  // e.g. B31
    }
}
