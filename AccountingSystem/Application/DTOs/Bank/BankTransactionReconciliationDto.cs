using Domain.Bank;
using Domain.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Bank
{
    public class BankTransactionReconciliationDto
    {
        public int Id { get; set; }
        public virtual BankTransactions BankTransaction { get; set; }
        public virtual Reconciliation Reconciliation { get; set; }
        public virtual CommonMaster StatusMaster { get; set; }
    }
}
