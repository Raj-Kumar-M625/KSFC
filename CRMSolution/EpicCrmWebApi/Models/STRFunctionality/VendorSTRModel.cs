using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EpicCrmWebApi
{
    public class VendorSTRModel
    {
        public long Id { get; set; }
        public long STRId { get; set; }

        public long STRTagId { get; set; }

        [Display(Name ="STR #")]
        public string STRNumber { get; set; }

        [Display(Name = "STR Date")]
        public DateTime STRDate { get; set; }

        [Display(Name = "Vehicle Number")]
        public string VehicleNumber { get; set; }

        [Display(Name = "Transporter Name")]
        public string VendorName { get; set; }

        [Display(Name = "Number of Bags")]
        public long NumberOfBags { get; set; }

        [Display(Name = "Shed Odo")]
        public Nullable<long> ShedOdo { get; set; }

        [Display(Name = "Total Running kms")]
        public Nullable<int> TotalRunningKms { get; set; }

        [Display(Name = "Transportation Charges (Rs)")]
        public Nullable<decimal> TransportationCharges { get; set; }

        [Display(Name = "Vehicle Capacity (kgs)")]
        public int VehicleCapacity { get; set; }

        [Display(Name = "Extra Tonnage (kgs)")]
        public Nullable<decimal> ExtraTonnage { get; set; }

        [Display(Name = "Extra Ton Charges (Rs)")]
        public Nullable<decimal> ExtraTonCharges { get; set; }

        [Display(Name = "Cost Per Extra Ton (Rs)")]
        public decimal CostPerExtraTon { get; set; }

        [Display(Name = "Toll Charges (Rs)")]
        public Nullable<decimal> TollCharges { get; set; }

        [Display(Name = "Weighment Charges (Rs)")]
        public Nullable<decimal> WeighmentCharges { get; set; }

        [Display(Name = "Hamali Charges (Rs)")]
        public Nullable<decimal> HamaliCharges { get; set; }

        [Display(Name = "Others (Rs)")]
        public Nullable<decimal> Others { get; set; }

        [Display(Name = "Net Payable Amount (Rs)")]
        public Nullable<decimal> NetPayableAmount { get; set; }

        [Display(Name = "Comments")]
        public string Comments { get; set; }

        [Display(Name = "Payment Status")]
        public string PaymentStatus { get; set; }

        [Display(Name = "STR Status")]
        public string STRStatus { get; set; }

        public bool IsEditAllowed { get; set; }

        [Display(Name = "Start Odo")]
        public long StartOdo { get; set; }

        [Display(Name = "End Odo")]
        public long EndOdo { get; set; }

        [Display(Name = "Silo To Shed kms")]
        public int SiloToShedKms { get; set; }

        [Display(Name = "Cost/km")]
        public decimal CostPerKm { get; set; }

        [Display(Name = "Gross Weight (Kgs)")]
        public decimal GrossWeight { get; set; }
        
        [Display(Name = "Net Weight (Kgs)")]
        public decimal NetWeight { get; set; }

        [Display(Name = "Entry Weight")]
        public decimal EntryWeight { get; set; }

        [Display(Name = "Exit Weight")]
        public decimal ExitWeight { get; set; }

        [Display(Name = "Silo Weight (Actual Wt.)")]
        public decimal LoadedWeight { get; set; }

        [Display(Name = "Hamali Rate Per Bag")]
        public decimal HamaliRatePerBag { get; set; }

        //Bank Details

        [Display(Name = "Bank A/c Name")]
        public string AccountName { get; set; }

        [Display(Name = "Bank Name")]
        public string BankName { get; set; }

        [Display(Name = "Bank Account No")]
        public string AccountNumber { get; set; }

        [Display(Name = "Bank IFSC")]
        public string IFSC { get; set; }

        [Display(Name = "Bank Branch")]
        public string BankBranch { get; set; }

        [Display(Name = "Payment Reference")]
        public string PaymentReference { get; set; }

    }
}