using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public enum WorkFlowStatus
    {
        All = 0,
        Waiting = 1,
        Due = 2,
        Overdue = 3,
        //CompletedAhead = 4,
        OverdueCompleted  = 5,
        Completed = 6
    }
}
