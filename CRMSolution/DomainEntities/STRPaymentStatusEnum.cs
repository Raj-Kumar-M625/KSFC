using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public enum STRPaymentStatus
    {
        Pending = 1,
        AwaitingApproval = 2,
        Approved = 3,
        Paid = 4
    }
}
