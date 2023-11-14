using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class VendorSTRPayment
    {
        public long STRTagId { get; set; }
        public string STRNumber { get; set; }
        public Nullable<long> ShedOdo { get; set; }
        public Nullable<int> TotalRunningKms { get; set; }
        public long StartOdo { get; set; }
        public long EndOdo { get; set; }
        public int SiloToShedKms { get; set; }
        public Nullable<decimal> TransportationCharges { get; set; }

        //public decimal GrossWeight { get; set; }
        public decimal NetWeight { get; set; }
        public decimal LoadedWeight { get; set; }
        public Nullable<decimal> ExtraTonnage { get; set; }
        public Nullable<decimal> ExtraTonCharges { get; set; }
        public Nullable<decimal> TollCharges { get; set; }
        public Nullable<decimal> WeighmentCharges { get; set; }
        public long NumberOfBags { get; set; }
        public decimal HamaliRatePerBag { get; set; }
        public Nullable<decimal> HamaliCharges { get; set; }
        public Nullable<decimal> Others { get; set; }
        public Nullable<decimal> NetPayableAmount { get; set; }
        public string Comments { get; set; }
        public string PaymentStatus { get; set; }
        public int VehicleCapacity { get; set; }
        public decimal CostPerKM { get; set; }
        public decimal CostPerExtraTon { get; set; }

        public string AccountHolderName { get; set; }
        public string BankName { get; set; }
        public string BankAccount { get; set; }
        public string BankIFSC { get; set; }
        public string BankBranch { get; set; }

        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

    }

}
