using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SalesPersonMiniModel
    {
        public string StaffCode { get; set; }
        public string Name { get; set; }
    }

    public class SalesPersonModel : SalesPersonMiniModel
    {
        public int Id { get; set; }
        public string HQCode { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }  // this is from Sales Person table;
        public long EmployeeId { get; set; }
        public bool OnWeb { get; set; }
        public string HQName { get; set; }
        public string Grade { get; set; }

        public bool TenantEmployeeExist { get; set; }
        public bool TenantEmployeeIsActive { get; set; }

        public string Department { get; set; }
        public string Designation { get; set; }
        public string Ownership { get; set; }
        public string VehicleType { get; set; }
        public string FuelType { get; set; }
        public string VehicleNumber { get; set; }

        public string BusinessRole { get; set; }

        public bool OverridePrivateVehicleRatePerKM { get; set; }
        public decimal TwoWheelerRatePerKM { get; set; }
        public decimal FourWheelerRatePerKM { get; set; }
    }
}
