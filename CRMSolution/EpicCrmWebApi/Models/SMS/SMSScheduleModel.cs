using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SMSScheduleModel
    {
        public ICollection<TenantHoliday> Holidays { get; set; }
        public ICollection<TenantWorkDay> WorkDays { get; set; }
        public ICollection<TenantSmsSchedule> SmsSchedule { get; set; }
        public ICollection<TenantSmsType> SmsTypes { get; set; }
    }
}