using Application.Interface.Persistence.Generic;
using Domain.Transactions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Interface.Persistence.Transactions
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        Task<Transaction> AddTransaction(Transaction entity);
        IQueryable<Transaction> GetListOfTransaction();
        IQueryable<Transaction> GetListOfCessTransaction();
        Task<List<Transaction>> GetTransactionsBasedRefId(List<int> billIds);
        Task<List<Transaction>> GetTransactionsBasedBillId(List<int> billIds);
        Task<List<Transaction>> GetTransactionsBasedRefIdForTDS(int tdsPaymentChallanId);
        Task<List<Transaction>> GetTransactionsBasedRefIdForGSTTDS(int gstTdsPaymentChallanId);
        Task<List<Transaction>> UpdateTransactionReconcileStatus(List<Transaction> transactionList);
    }
}
