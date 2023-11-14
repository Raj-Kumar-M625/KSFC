using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class ldHypoDet
    {
        public int RowID { get; set; }
        public string TypeOfAsset { get; set; }
        public string NameOfAsset { get; set; }
        public string AssetDescription { get; set; }
        public string HypothicationDescription { get; set; }
        public string SecurityDeedNo { get; set; }
        public DateTime DateOfExecution { get; set; }
        public int ValueInLakhs { get; set; }
        public string AccountNumber { get; set; }
        public bool IsActive { get; set; }
        public string Action { get; set; }
    }
}
