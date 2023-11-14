using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class VendorSTR
    {
        public long Id { get; set; }
        public long STRId { get; set; }

        public long STRTagId { get; set; }
        public long TransporterId { get; set; }

       
        public string STRNumber { get; set; }

    
        public DateTime STRDate { get; set; }

  
        public string VehicleNumber { get; set; }


        public string VendorName { get; set; }

        public long NumberOfBags { get; set; }

  
        public Nullable<long> ShedOdo { get; set; }


        public Nullable<int> TotalRunningKms { get; set; }

 
        public Nullable<decimal> TransportationCharges { get; set; }


        public int VehicleCapacity { get; set; }


        public Nullable<decimal> ExtraTonnage { get; set; }

        public Nullable<decimal> ExtraTonCharges { get; set; }

        public Nullable<decimal> TollCharges { get; set; }

        public Nullable<decimal> WeighmentCharges { get; set; }


        public Nullable<decimal> HamaliCharges { get; set; }

     
        public Nullable<decimal> Others { get; set; }

        public Nullable<decimal> NetPayableAmount { get; set; }

        public string Comments { get; set; }

        public string PaymentStatus { get; set; }

        public string STRStatus { get; set; }

        public bool IsEditAllowed => String.IsNullOrEmpty(PaymentStatus) || PaymentStatus.Equals("Pending") || PaymentStatus.Equals("AwaitingApproval");
        public bool IsReadyToPay => PaymentStatus.Equals("Approved");

        public long StartOdo { get; set; }

        public long EndOdo { get; set; }

        public int SiloToShedKms { get; set; }

        public decimal CostPerKm { get; set; }
        public decimal CostPerExtraTon { get; set; }
        public decimal HamaliRatePerBag { get; set; }
        //public decimal GrossWeight { get; set; }
        public decimal NetWeight { get; set; }
        public decimal EntryWeight { get; set; }
        public decimal ExitWeight { get; set; }
        public decimal LoadedWeight { get; set; }

        public string SeasonName { get; set; }

        //Bank Details
        public string AccountName { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string IFSC { get; set; }
        public string BankBranch { get; set; }
        public string PaymentReference { get; set; }
        public bool IsActive { get; set; }

        public string CurrentUser { get; set; }
    }
}
