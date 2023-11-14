using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.BankFile
{
    public class BankFileUtrDetailsDto
    {
        public int Id { get; set; }
        public int NoOfTransactions { get; set; }
        public string DifferentBankUTRNumber { get; set; }
        public bool IsBulkPayment { get; set; }
        public string SameBankUTRNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifedBy { get; set; }
        public List<int> GenerateBankFileID { get; set; }
    }
}
