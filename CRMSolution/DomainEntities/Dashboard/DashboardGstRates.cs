using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
   public class DashboardGstRate
    {
        public long Id { get; set; }
        public string GstCode { get; set; }
        public decimal GstRate { get; set; }
        public System.DateTime EffectiveStartDate { get; set; }
        public System.DateTime EffectiveEndDate { get; set; }
    }

    public class GstSaveRate : DashboardGstRate
    {
        public string CurrentStaffCode { get; set; }
    }
}
