using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Bank
{
    public class BankStatementInputDto
    {
            public int Id { get; set; }
            public string FileName { get; set; }
            public int? NoOfTransaction { get; set; }
            public decimal? TotalCreditAmount { get; set; }
            public decimal? TotalDebitAmount { get; set; }
            public int TotalCreditTransaction { get; set; }
            public int TotalDebitTransaction { get; set; }
            public int? NoOfProcessedTransaction { get; set; }
            public int? NoOfDuplicateTransaction { get; set; }
            public int? NoOfJunkTransaction { get; set; }
            public int? NoOfSuccessTransaction { get; set; }
            public string UniqueFileName { get; set; }
            public DateTime CreatedDate { get; set; }
            public string CreatedBy { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Remarks { get; set; }
    }
}
