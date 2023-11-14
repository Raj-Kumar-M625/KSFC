using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Bill
{
    public class BillListDto
    {        

        public int BillNumber { get; set; }
        public string ReferenceNumber { get; set; }
        public string VendorName { get; set; }
        public string Category { get; set; }
        public string BillDate { get; set; }
        public string DueDate { get; set; }
        public decimal? BillTotal { get; set; }
        public decimal TotalGST { get; set; }
        public decimal? TDS { get; set; }
        public decimal? GSTTDS { get; set; }
        public decimal TotalPayable { get; set; }
        public string CreatedBy { get; set; }
        public string Status { get; set; }




    }
}
