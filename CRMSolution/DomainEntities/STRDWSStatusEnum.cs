using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public enum STRDWSStatus
    {
        Pending = 1,
        SiloChecked = 2,
        Approved = 3,
        WeightApproved = 4,
        AmountApproved = 5,
        Paid = 6,
        Cancelled = 7
    }
}

