using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class DownloadProductPrice
    {
        public string CustomerType { get; set; } //Dealer or Distributor or P.Distributor
        public decimal BillingPrice { get; set; }
    }
}
