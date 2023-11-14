using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Presentation.Models.Bank
{
    public class UploadBankStatementsModel
    {
        public IFormFile File { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Remarks { get; set; }
        public decimal TotalCreditAmount { get; set; }
        public decimal TotalDebitAmount { get; set; }
    }
}
