using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class DownloadCustomer
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string HQCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Type { get; set; } // Dealer/P.Distributor/Distributor 
        public decimal CreditLimit { get; set; }
        public decimal Outstanding { get; set; }
        public decimal LongOutstanding { get; set; }
        public decimal Target { get; set; }
        public decimal Sales { get; set; }
        public decimal Payment { get; set; }
    }

    public class DownloadCustomerExtend : DownloadCustomer
    {
        public int Id { get; set; }

        //Added new items for Add or Edit function
        //public string District { get; set; }
        //public string State { get; set; }
        //public string Branch { get; set; }
        //public string Pincode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Email { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public bool Active { get; set; }
    }
}
