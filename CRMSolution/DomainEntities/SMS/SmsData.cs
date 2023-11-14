using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class Geography
    {
        public string ZoneCode { get; set; }
        public string AreaCode { get; set; }
        public string TerritoryCode { get; set; }     
        public string HQCode { get; set; }
    }

    public class SmsData : Geography
    {
        public string StaffCode { get; set; }
        public bool IsInFieldToday { get; set; }
        public bool IsRegisteredOnPhone { get; set; }
    }

    public class SmsDataEx : SmsData
    {
        public string Name { get; set; }
        public string Phone { get; set; }

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal TotalOrderAmount { get; set; }
        public decimal TotalPaymentAmount { get; set; }
        public decimal TotalReturnAmount { get; set; }
        public decimal TotalExpenseAmount { get; set; }
        public int TotalActivityCount { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime CurrentLocTime { get; set; }

        public string PhoneModel { get; set; }
        public string PhoneOS { get; set; }
        public string AppVersion { get; set; }
    }
}
