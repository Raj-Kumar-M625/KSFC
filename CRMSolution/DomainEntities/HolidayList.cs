using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
   public  class HolidayList
    {
        public long Id { get; set; }
        public string AreaCode { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

       // public System.DateTime StartDate { get;}
       // public System.DateTime EndDate { get; set; }
    }
    public class DownloadHolidayList
    {
        public long Id { get; set; }
        public string AreaCode { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }

    }
}
