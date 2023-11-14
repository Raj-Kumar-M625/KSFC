using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public enum DBSaveStatus
    {
        InvalidRequest = -1,
        Success = 0,
        CyclicCheckFail = 1,
        RecordNotFound = 2,
    }
}
