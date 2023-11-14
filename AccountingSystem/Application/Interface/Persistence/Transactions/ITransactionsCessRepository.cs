using Application.Interface.Persistence.Generic;
using Domain.CessTransactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Persistence.Transactions
{
    public interface ITransactionsCessRepository : IGenericRepository<TransactionsCess>
    {
        Task<List<TransactionsCess>> UpdateListCessTransaction(List<TransactionsCess> transactionList);
    }
}
