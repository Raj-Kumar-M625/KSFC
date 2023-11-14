using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.AdminModule
{

    public class AssignOfficeMasterHistoryDto
    {
        public string EmployeeId { get; set; }
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        public string ChairCode { get; set; }
        public string ChairName { get; set; }
        public string OpDesigCode { get; set; }
        public string OpDesigName { get; set; }
        public string FromDate { get; set; }
                  
    }
}
