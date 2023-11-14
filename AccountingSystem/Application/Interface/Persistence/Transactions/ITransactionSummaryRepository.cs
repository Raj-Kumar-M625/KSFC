using Application.DTOs.Filters;
using Application.Interface.Persistence.Generic;
using Domain.Bill;
using Domain.Transactions;
using Domain.Vendor;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Interface.Persistence.Transactions
{
    public interface ITransactionSummaryRepository : IGenericRepository<TransactionsSummary>
    {
        Task<TransactionsSummary> AddTransactionSummary(TransactionsSummary entity);
        Task<TransactionsSummary> AddTransactionSummaryDetails(List<TransactionsSummary> entity);
        Task<IQueryable<TransactionsSummary>> GetAllTransactions(ReconcileDto reconcileDto);
        Task<List<TransactionsSummary>> UpdateListOfTransactions(List<TransactionsSummary> transactionsSummary);
    }
}
