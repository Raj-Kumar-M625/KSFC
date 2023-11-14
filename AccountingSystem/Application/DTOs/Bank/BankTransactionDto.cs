using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Bank
{
    public class BankTransactionDto
    {
        public int Id { get; set; }
        public DateTime? Transaction_Date { get; set; }
        public DateTime? Value_Date { get; set; }
        public string RefNo_ChequeNo { get; set; }
        public string Description { get; set; }
        public long? Branch_Code { get; set; }
        public string Transaction_Mnemonic { get; set; }
        public string Transaction_Literal { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public decimal? Balance { get; set; }
        public long? AccountNo { get; set; }
        public string BankName { get; set; }
        public string FileName { get; set; }
        public string Status { get; set; }
        public bool IsPicked { get; set; }
        public bool IsMatched { get; set; }

        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
