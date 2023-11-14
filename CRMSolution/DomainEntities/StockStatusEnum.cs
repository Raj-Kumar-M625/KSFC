using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public enum StockStatus
    {
        Received = 1,
        ReceiveApproved = 2,

        Requested = 3,
        RequestApproved = 4,

        Pending = 5,
        Complete = 6,
        Denied = 7,

        ClearRequest = 8,
        ClearApproved = 9,

        AddRequest = 10,
        AddApproved = 11,
    }

    public enum StockRequestType
    {
        StockIssueRequest = 1,
        StockClearRequest = 2,
        StockAddRequest = 3,
    }
}

