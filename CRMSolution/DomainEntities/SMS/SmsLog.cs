using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SmsLog
    {
        public int Id { get; set; }
        public long TenantId { get; set; }
        public string TenantName { get; set; }
        public DateTime SMSDateTime { get; set; }
        public string SMSText { get; set; }
        public string SmsApiResponse { get; set; }

        public string SmsType { get; set; }
        public string SenderName { get; set; }
    }
}
