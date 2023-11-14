using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SmsSummaryDataModel
    {
        // could be Zone/Area/Territory/HQ Name/Code
        public string Name { get; set; }
        public string Code { get; set; }

        // only for backward compatibility
        public string AreaName => Name;
        public string AreaCode => Code; 

        public int CurrentlyRoamingCount { get; set; }  // people who are in field and have not ended their day
        public int InFieldCount { get; set; }  // people may have ended their day.
        public int RegisteredCount { get; set; }
        public int HeadCount { get; set; }

        public decimal Orders { get; set; }
        public decimal Payments { get; set; }
        public decimal Returns { get; set; }
        public decimal Expenses { get; set; }
        public long Activities { get; set; }

        public long CustomerCount { get; set; }
        public decimal Outstanding { get; set; }
        public decimal LongOutstanding { get; set; }
        public decimal Target { get; set; }

        public IEnumerable<SmsDetailDataModel> Details { get; set; }

        // only for backward compatibility
        public IEnumerable<SalesPersonEx> AreaManagers => Managers; // this is obsolete
        public IEnumerable<SalesPersonEx> Managers { get; set; }
    }

    public class SmsDetailDataModel
    {
        public string StaffCode { get; set; }
        public string Name { get; set; }
        public bool IsInFieldToday { get; set; }
        public bool IsRegisteredOnPhone { get; set; }
        public string Phone { get; set; }

        public decimal Orders { get; set; }
        public decimal Payments { get; set; }
        public decimal Returns { get; set; }
        public decimal Expenses { get; set; }
        public long Activities { get; set; }

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime CurrentLocTime { get; set; }

        public string PhoneModel { get; set; }
        public string PhoneOS { get; set; }
        public string AppVersion { get; set; }
    }
}