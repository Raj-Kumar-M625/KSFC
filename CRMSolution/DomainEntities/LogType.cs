using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public enum LogType
    {
        ErrorLog = 1,
        PhoneBatchProcessLog,
        DataFeedProcessLog,
        ExcelUploadHistory,
        DistanceCalcErrorLog,
        SMSLog,
        SMSJobLog,
        EntityAgreement,
        PurgeDataLog,
    }
}

