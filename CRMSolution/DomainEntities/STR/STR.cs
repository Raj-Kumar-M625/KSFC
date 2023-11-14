using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class STR
    {
        public long Id { get; set; } = 0;
        public long STRTagId { get; set; }
        public long EmployeeId { get; set; }
        public string VehicleNumber { get; set; }
        public string DriverName { get; set; }
        public string DriverPhone { get; set; }
        public long DWSCount { get; set; } = 0;
        public long BagCount { get; set; } = 0;
        public decimal GrossWeight { get; set; } = 0;
        public decimal NetWeight { get; set; } = 0;
        public long StartOdometer { get; set; } = 0;
        public long EndOdometer { get; set; } = 0;
        public bool IsNew { get; set; } = false;
        public bool IsTransferred { get; set; } = false;
        public string TransfereeName { get; set; } = "";
        public string TransfereePhone { get; set; } = "";
        public int ImageCount { get; set; } = 0;
        public long ActivityId { get; set; } = 0;
        public long ActivityId2 { get; set; } = 0;

        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeePhone { get; set; }

        public string CurrentUser { get; set; }
        public string STRNumber { get; set; }
        public long MoveToStrTagId { get; set; }

        public bool ChangeSTRNumber { get; set; }
        public bool ChangeSTRData { get; set; }

        public long StrTagCyclicCount { get; set; }

        public string Status { get; set; }
    }
}
