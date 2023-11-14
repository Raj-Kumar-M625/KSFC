using System;

namespace Application.DTOs.Bank
{
    public class BankStatementsListDto
    {
        public long Id { get; set; }
        public int BankStatementInputId { get; set; }
        public DateTime Transaction_Date { get; set; }
        public DateTime Value_Date { get; set; }
        public string RefNo_ChequeNo { get; set; }
        public string Description { get; set; }
        public long Branch_Code { get; set; }
        public string Transaction_Mnemonic { get; set; }
        public string Transaction_Literal { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
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
