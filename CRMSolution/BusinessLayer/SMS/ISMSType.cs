using DomainEntities;
using System;

namespace BusinessLayer
{
    public interface ISMSType
    {
        void Process(long tenantId, TenantSmsType record, DateTime currentIstDateTime, 
            string smsRequestLogFile, string smsResponseLogFile);
    }
}
