using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class STRWeight
    {
        public long Id { get; set; }
        public string STRNumber { get; set; }
        public DateTime STRDate { get; set; }

        public decimal EntryWeight { get; set; }
        public decimal ExitWeight { get; set; }
        public string SiloNumber { get; set; }
        public string SiloIncharge { get; set; }
        public string UnloadingIncharge { get; set; }
        public long ExitOdometer { get; set; }
        public long BagCount { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }
        public decimal DeductionPercent { get; set; }
        public long CyclicCount { get; set; }

        public long DWSCount { get; set; }
        public string VehicleNumber { get; set; }

        // We want to give and edit option silo weight even if it is SiloChecked
        //public bool IsEditAllowed => Status.Equals(STRDWSStatus.Pending.ToString(), StringComparison.OrdinalIgnoreCase);    
        public bool IsEditAllowed => true;

        public string CurrentUser { get; set; }
        public bool IsFinal { get; set; }
    }
}
