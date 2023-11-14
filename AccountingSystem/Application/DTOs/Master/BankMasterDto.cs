using Application.DTOs.Vendor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Master
{
    public class BankMasterDto
    {
        [Key]
        public int Id { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string IfscCode { get; set; }
        public string AccountNumber { get; set; }
        public bool IsBulkTDS { get; set; }
        public bool IsBulkGSTTDS { get; set; }
        public bool IsBulkPayment { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
    }
}
