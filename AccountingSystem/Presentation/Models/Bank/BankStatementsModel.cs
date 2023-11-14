using System;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Models.Bank
{
    public class BankStatementsModel
    {
        public long Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Transaction_Date { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Value_Date { get; set; }
        public string RefNo_ChequeNo { get; set; }
        public string Description { get; set; }
        public long Branch_Code { get; set; }
        public string Transaction_Mnemonic { get; set; }
        public string Transaction_Literal { get; set; }

        [Display(Name = "Debit Amount")]
        public decimal Debit { get; set; }

        [Display(Name = "Credit Amount")]
        public decimal Credit { get; set; }

        [Display(Name = "Balance Amount")]
        public decimal Balance { get; set; }
        public long AccountNo { get; set; }
        public string BankName { get; set; }
        public string FileName { get; set; }
        public bool IsProcessed { get; set; }
        public bool IsDuplicate { get; set; }
        public bool IsJunk { get; set; }
        public bool IsSuccess { get; set; }
    }
}
