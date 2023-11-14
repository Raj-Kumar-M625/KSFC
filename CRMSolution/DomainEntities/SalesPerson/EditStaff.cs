using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class EditStaff
    {
        public string StaffCode { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string HQCode { get; set; }
        public string Grade { get; set; }
        public bool IsActive { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string Ownership { get; set; }
        public string VehicleType { get; set; }
        public string FuelType { get; set; }
        public string VehicleNumber { get; set; }
    }
}
