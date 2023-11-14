using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class CustomerData : Geography
    {
        public string Code { get; set; }  // could be Zone/Area/Territory/HQ after final rollup
        public int CustomerCount { get; set; }
        public decimal TotalOutstanding { get; set; }
        public decimal TotalLongOutstanding { get; set; }
        public decimal TotalTarget { get; set; }
        //public decimal Sales { get; set; }
        //public decimal Payment { get; set; }
    }
}
