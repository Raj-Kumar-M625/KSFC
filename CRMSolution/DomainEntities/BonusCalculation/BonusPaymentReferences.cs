using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class BonusPaymentReferences
    {
        public long Id { get; set; }
        public string PaymentReference { get; set; }
        public string Comments { get; set; }
        public decimal TotalBonusPaid { get; set; }
        public string AgreementNumber { get; set; }
        public long AgreementCount { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string AccountAddress { get; set; }
        public string AccountEmail { get; set; }
        public string PaymentType { get; set; }
        public string SenderInfo { get; set; }


        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public string CurrentUser { get; set; }
    }
}
