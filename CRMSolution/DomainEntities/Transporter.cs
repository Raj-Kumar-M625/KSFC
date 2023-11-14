using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class Transporter
    {
        public long Id { get; set; }
        public string CompanyName { get; set; }
        public string VehicleType { get; set; }
        public string VehicleNo { get; set; }
        public string TransportationType { get; set; }
        public int SiloToReturnKM { get; set; }
        public int VehicleCapacityKgs { get; set; }
        public decimal HamaliRatePerBag { get; set; }
        public decimal CostPerKm { get; set; }
        public decimal ExtraCostPerTon { get; set; }
    }
}
