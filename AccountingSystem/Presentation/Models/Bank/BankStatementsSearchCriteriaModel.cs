using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Models.Bank
{
    public class BankStatementsSearchCriteriaModel
    {
        [BindProperty, DataType(DataType.Date)]
        public DateTime DateFrom { get; set; }

        [BindProperty, DataType(DataType.Date)]
        public DateTime DateTo { get; set; }
        public string TransactionType { get; set; }
        public decimal MinAmount { get; set; }
        public decimal MaxAmount { get; set; }
        public long AccountNo { get; set; }
        public string BankName { get; set; }
        public string FileName { get; set; }
        public int Count { get; set; } = 500;
    }
}
